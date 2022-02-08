namespace FSharp.HTML

open FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type AttributeDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FSharp.HTML")
    let filePath = Path.Combine(sourcePath, @"attribute.fslex") // **input**
    let text = File.ReadAllText(filePath)
            
    [<Fact(Skip="once and for all!")>] //
    member _.``1 - generate DFA``() =

        let name = "AttributeDFA" // **input**
        let moduleName = $"FSharp.HTML.{name}"

        let fslex = FslexFile.parse text
        let y = fslex.toFslexDFAFile()
        let result = y.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir, result)
        output.WriteLine("dfa output path:" + outputDir)

    [<Fact>]
    member _.``2 - valid DFA``() =
        let fslex = FslexFile.parse text
        let y = fslex.toFslexDFAFile()

        Should.equal y.nextStates AttributeDFA.nextStates
        Should.equal y.header     AttributeDFA.header
        Should.equal y.rules      AttributeDFA.rules

