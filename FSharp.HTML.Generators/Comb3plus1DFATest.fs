namespace FSharp.HTML

open System.IO
open System.Text

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc.Fslex

type Comb3plus1DFATest(output:ITestOutputHelper) =

    let name = "Comb3plus1DFA"
    let moduleName = $"FSharp.HTML.{name}"
    let modulePath = Path.Combine(Dir.projPath, $"{name}.fs")
            
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "comb3plus1.fslex")
    let text = File.ReadAllText(filePath)
    let fslex = FslexFileUtils.parse text

    [<Fact>]
    member _.``02 - verify``() =
        let y = 
            fslex 
            |> FslexFileUtils.verify

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)


    [<Fact(
    Skip="once and for all!"
    )>]
    member _.``04 - generate DFA``() =
        let y = 
            fslex 
            |> FslexFileUtils.toFslexDFAFile
        let result = 
            y 
            |> FslexDFAFileUtils.generate(moduleName)

        File.WriteAllText(modulePath, result, Encoding.UTF8)
        output.WriteLine("output lex:")
        output.WriteLine(modulePath)

    [<Fact>]
    member _.``10 - valid DFA``() =
        let src = fslex |> FslexFileUtils.toFslexDFAFile
        Should.equal src.nextStates Comb3plus1DFA.nextStates

        let headerFslex =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFslex =
            let mappers = src |> FslexDFAFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let src = File.ReadAllText(modulePath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 src

        Should.equal headerFslex header
        Should.equal semansFslex semans

    //[<Fact>]
    //member _.``10 - valid DFA``() =
    //    let src = fslex.toFslexDFAFile()
    //    Should.equal src.nextStates Comb3plus1DFA.nextStates

    //    let headerFslex =
    //        FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

    //    let semansFslex =
    //        let mappers = src.generateMappers()
    //        FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

    //    let header,semans =
    //        let src = File.ReadAllText(modulePath, Encoding.UTF8)
    //        FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 1 src

    //    Should.equal headerFslex header
    //    Should.equal semansFslex semans

