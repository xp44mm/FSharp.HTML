namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit
open System
open System.IO

type FileTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    static member files =
        Directory.GetFiles(Dir.omitted)
        |> Seq.map(fun f -> Path.GetFileName f)
        |> Seq.map Array.singleton

    [<Theory(Skip="generated");MemberData(nameof FileTest.files)>]
    member this.``generate wellformed file``(file) =
        let txt = File.ReadAllText(Path.Combine(Dir.omitted, file))
        let _,nodes = Parser.parseDoc txt
        let content =
            nodes
            |> Seq.map HtmlUtils.stringifyNode
            |> String.concat "\r\n"
        File.WriteAllText(
            Path.Combine(Dir.wellformed, file),
            content,System.Text.Encoding.UTF8)
        
    [<Theory;MemberData(nameof FileTest.files)>]
    member this.``omitted vs wellformed``(file) =
        let _,nodes1 = 
            let txt = File.ReadAllText(Path.Combine(Dir.omitted, file))            
            Parser.parseDoc txt
        let _,nodes2 = 
            let txt = File.ReadAllText(Path.Combine(Dir.wellformed, file))            
            Parser.parseDoc txt
        Should.equal nodes1 nodes2



