namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open FSharp.Idioms
open FSharp.Literals
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc

type NodesParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    ///符号在输入文件中的表示
    let symbolRender sym =
        if Regex.IsMatch(sym,@"^\w+$") then
            sym
        else Quotation.quote sym

    ///符号类
    let clazz symbols =
        symbols
        |> Seq.map symbolRender
        |> String.concat "|"

    let filePath = Path.Combine(Dir.projPath, "nodes.fsyacc") // **input**
    let text = File.ReadAllText(filePath)
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``1 - 显示冲突状态的冲突项目``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        let conflicts =
            collection.filterConflictedClosures()
        show conflicts

    [<Fact>]
    member _.``2 - 汇总冲突的产生式``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        let conflicts =
            collection.filterConflictedClosures()

        let productions =
            AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions 
                collection.grammar.terminals 
                productions
            |> List.ofArray

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = [
            ]

        Should.equal y pprods

    [<Fact>]
    member _.``3 - print the template of type annotaitions``() =
        let grammar = 
            fsyacc.getMainProductions()
            |> Grammar.from

        let symbols = 
            grammar.symbols
            |> Set.filter(fun x -> Regex.IsMatch(x,@"^([-\w]+|<[-\w]+>)$"))
            |> Set.map(symbolRender)

        let sourceCode = 
            [
                for i in symbols do
                    $"{i}: \"list<string*string>\""
            ] |> String.concat "\r\n"
        output.WriteLine(sourceCode)

    [<Fact(Skip="once and for all!")>] //
    member _.``5 - generate parsing table``() =
        let name = "NodesParseTable" // **input**
        let moduleName = $"FSharp.HTML.{name}"

        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generate(moduleName)
        let outputDir = Path.Combine(Dir.projPath, $"{name}.fs")

        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output path:"+outputDir)

    [<Fact>]
    member _.``6 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTableFile()
        Should.equal t.header       NodesParseTable.header
        Should.equal t.actions      NodesParseTable.actions
        Should.equal t.rules        NodesParseTable.rules
        Should.equal t.declarations NodesParseTable.declarations

    [<Fact>]
    member _.``7 - list all tokens``() =
        let grammar = 
            fsyacc.getMainProductions()
            |> Grammar.from

        let tokens = 
            grammar.symbols - grammar.nonterminals
        output.WriteLine($"any={clazz tokens}")

    [<Fact>]
    member _.``8 - first or last token of node``() =
        let grammar = 
            fsyacc.getMainProductions()
            |> Grammar.from

        let last = grammar.lasts.["node"]
        let first = grammar.firsts.["node"]

        output.WriteLine($"last={clazz last}")
        output.WriteLine($"first={clazz first}")

    [<Fact>]
    member _.``9 - closures``() =
        let tbl = NodesParseTable.parser.getParserTable()
        let str = tbl.collection()

        let name = "nodes"
        let outputDir = Path.Combine(Dir.projPath, $"{name}.txt")
        File.WriteAllText(outputDir,str,Encoding.UTF8)
        output.WriteLine($"output:\r\n{outputDir}")
