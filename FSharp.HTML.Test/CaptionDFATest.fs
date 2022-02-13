namespace FSharp.HTML

open FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open System.Text.RegularExpressions

type CaptionDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FSharp.HTML")
    let filePath = Path.Combine(sourcePath, @"caption.fslex") // **input**
    let text = File.ReadAllText(filePath)
    let fslex = FslexFile.parse text

    [<Fact>]
    member _.``0 = compiler test``() =
        let hdr,dfs,rls = FslexCompiler.parseToStructuralData text
        show hdr
        show dfs
        show rls
        
    [<Fact>]
    member _.``1 = verify``() =
        let y = fslex.verify()

        Assert.True(y.undeclared.IsEmpty)
        Assert.True(y.unused.IsEmpty)

    [<Fact>]
    member _.``2 = universal characters``() =
        let res = fslex.getRegularExpressions()

        let pairs = 
            res
            |> Array.collect(fun re -> re.getCharacters())
            |> Set.ofArray
            |> Seq.groupBy(fun x ->
                if Regex.IsMatch(x,@"^\w+$") then
                    "id"
                elif Regex.IsMatch(x,@"^<\w+>$") then
                    "startTag"
                elif Regex.IsMatch(x,@"^</\w+>$") then
                    "endTag"
                elif Regex.IsMatch(x,@"^<\w+/>$") then
                    "selfClosingTag"
                else
                    ""
            )

        let mappers =
            [
                "id" , fun (x:string) -> x
                "startTag", fun x -> x.[1..x.Length-2]
                "endTag", fun x -> x.[2..x.Length-2]
                "selfClosingTag", fun x -> x.[1..x.Length-3]
            ] |> Map.ofList

        for (tag,sq) in pairs do
            let mapper =
                if mappers.ContainsKey tag then
                    mappers.[tag]
                else fun x -> x
            let st = sq |> Seq.map mapper |> Set.ofSeq
            let outp = $"let {tag} = {Literal.stringify st}"
            output.WriteLine(outp)

    [<Fact(Skip="once and for all!")>] //
    member _.``3 = generate DFA``() =
        let name = "CaptionDFA" // **input**
        let moduleName = $"FSharp.HTML.{name}"

        let fslex = FslexFile.parse text
        let y = fslex.toFslexDFAFile()
        let result = y.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir, result)
        output.WriteLine("dfa output path:" + outputDir)

    [<Fact>]
    member _.``4 = valid DFA``() =
        let fslex = FslexFile.parse text
        let y = fslex.toFslexDFAFile()

        Should.equal y.nextStates CaptionDFA.nextStates
        Should.equal y.header     CaptionDFA.header
        Should.equal y.rules      CaptionDFA.rules

