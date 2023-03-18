namespace FSharp.HTML

open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Literals.Literal
open FSharp.xUnit
open FslexFsyacc.Fslex

type thead_tbody_tfoot_DFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    let filePath = Path.Combine(Dir.projPath, "thead_tbody_tfoot.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    let name = "thead_tbody_tfoot_DFA"
    let moduleName = $"FSharp.HTML.{name}"
    let modulePath = Path.Combine(Dir.projPath, $"{name}.fs")
            
    [<Fact>]
    member _.``02 - verify``() =
        let y = fslex.verify()
        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact()>] // Skip="once and for all!"
    member _.``04 - generate DFA``() =
        let y = fslex.toFslexDFAFile()
        let result = y.generate(moduleName)

        File.WriteAllText(modulePath, result, Encoding.UTF8)
        output.WriteLine("output lex:" + modulePath)

    [<Fact>]
    member _.``10 - valid DFA``() =
        let src = fslex.toFslexDFAFile()
        Should.equal src.nextStates thead_tbody_tfoot_DFA.nextStates

        let headerFslex =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFslex =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let src = File.ReadAllText(modulePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 src

        Should.equal headerFslex header
        Should.equal semansFslex semans

    //[<Fact>]
    //member _.``01 - compiler test``() =
    //    let hdr,dfs,rls = FslexCompiler.compile text
    //    show hdr
    //    show dfs
    //    show rls

    //[<Fact>]
    //member _.``03 - universal characters``() =
    //    let res = fslex.getRegularExpressions()

    //    let y = 
    //        res
    //        |> List.collect(fun re -> re.getCharacters())
    //        |> Set.ofList
    //    show y


