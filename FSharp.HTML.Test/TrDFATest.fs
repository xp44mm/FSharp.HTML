namespace FSharp.HTML

open FslexFsyacc.Fslex

open System.IO

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open System.Text.RegularExpressions

type TrDFATest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let sourcePath = Path.Combine(solutionPath, @"FSharp.HTML")
    let filePath = Path.Combine(sourcePath, @"tr.fslex") // **input**
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
                elif Regex.IsMatch(x,@"^<[^<>/]+>$") then
                    "startTag"
                elif Regex.IsMatch(x,@"^</[^<>/]+>$") then
                    "endTag"
                elif Regex.IsMatch(x,@"^<[^<>/]+/>$") then
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
     
    [<Fact>]
    member _.``3 = print all tokens``() =
        let res = fslex.getRegularExpressions()

        let tokens = 
            res
            |> Array.collect(fun re -> re.getCharacters())
            |> Array.toList
            |> List.filter(fun x -> Regex.IsMatch(x,@"\W"))
            |> List.distinct
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)
            
        let outp =
            tokens
            |> List.map(FSharp.Idioms.Quotation.quote)
            |> String.concat "\r\n"
            |> sprintf "[%s]"

        output.WriteLine(outp)


    [<Fact(Skip="once and for all!")>] //
    member _.``4 = generate DFA``() =
        let name = "TrDFA" // **input**
        let moduleName = $"FSharp.HTML.{name}"

        let y = fslex.toFslexDFAFile()
        let result = y.generate(moduleName)

        let outputDir = Path.Combine(sourcePath, $"{name}.fs")
        File.WriteAllText(outputDir, result)
        output.WriteLine("dfa output path:" + outputDir)

    [<Fact>]
    member _.``5 = valid DFA``() =
        let y = fslex.toFslexDFAFile()

        Should.equal y.nextStates TrDFA.nextStates
        Should.equal y.header     TrDFA.header
        Should.equal y.rules      TrDFA.rules

