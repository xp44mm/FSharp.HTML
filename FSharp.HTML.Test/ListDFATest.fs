﻿namespace FSharp.HTML

open FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

type ListDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FSharp.HTML")
    let filePath = Path.Combine(sourcePath, @"list.fslex")
    let text = File.ReadAllText(filePath)

    [<Fact>]
    member _.``0 - compiler test``() =
        let hdr,dfs,rls = FslexCompiler.parseToStructuralData text
        show hdr
        show dfs
        show rls
        
    [<Fact(Skip="once and for all!")>] // 
    member _.``1 - generate DFA``() =

        let name = "ListDFA" // **input**
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

        Should.equal y.nextStates ListDFA.nextStates
        Should.equal y.header     ListDFA.header
        Should.equal y.rules      ListDFA.rules

