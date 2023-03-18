namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals.Literal
open FSharp.xUnit
open FslexFsyacc.Runtime

type TagLeftCompilerData() =
    static let dataSource = SingleDataSource([
        "<html>",{index= 0;length= 6;value= TAGSTART("html",[])}
        "<meta charset=\"utf-8\">",{index= 0;length= 22;value= TAGSTART("meta",["charset","utf-8"])}
        "<img src='images/firefox-icon.png' alt='My test image'>",{index= 0;length= 55;value= TAGSTART("img",["src","images/firefox-icon.png";"alt","My test image"])}
        "<html></html>",{index= 0;length= 6;value= TAGSTART("html",[])}
    ])
    static member keys = dataSource.keys
    static member get key = dataSource.[key]


type TagLeftCompilerTest(output:ITestOutputHelper) =

    [<Theory>]
    [<MemberData(nameof TagLeftCompilerData.keys, MemberType=typeof<TagLeftCompilerData>)>]
    member _.``01 = tokenize test``(x:string) =
        let postok = 
            TagLeftToken.tokenize 0 x
            |> Seq.toList
        output.WriteLine(stringify postok)

    [<Theory>]
    [<MemberData(nameof TagLeftCompilerData.keys, MemberType=typeof<TagLeftCompilerData>)>]
    member _.``01 = compile test``(x:string) =
        let postok = 
            TagLeftCompiler.compile 0 x
        output.WriteLine(stringify postok)

        let e = TagLeftCompilerData.get x
        Should.equal e postok


          
