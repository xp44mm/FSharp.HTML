namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System
open System.IO
open System.Text.RegularExpressions

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FSharp.Idioms

type HtmlParseTableTest(output:ITestOutputHelper) =
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

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let projPath = Path.Combine(solutionPath,@"FSharp.HTML")
    let filePath = Path.Combine(projPath, "html.fsyacc")
    let text = File.ReadAllText(filePath)

    [<Fact>]
    member _.``0-产生式冲突``() =
        let fsyacc = FsyaccFile.parse text
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let pconflicts = ConflictFactory.productionConflict tbl.ambiguousTable
        show pconflicts
        Assert.True(pconflicts.IsEmpty)

    [<Fact>]
    member _.``1-符号多用警告``() =
        let fsyacc = FsyaccFile.parse text
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let warning = ConflictFactory.overloadsWarning tbl
        //show warning
        Assert.True(warning.IsEmpty)

    [<Fact>]
    member _.``2-优先级冲突``() =
        let fsyacc = FsyaccFile.parse text
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        //show tbl.kernelProductions
        let srconflicts = ConflictFactory.shiftReduceConflict tbl
        //show srconflicts
        Assert.True(srconflicts.IsEmpty)

    [<Fact>]
    member _.``3 - print the template of type annotaitions``() =
        let fsyacc = FsyaccFile.parse text
        let grammar = Grammar.from fsyacc.mainProductions

        let symbols = 
            grammar.symbols
            |> Set.filter(fun x -> Regex.IsMatch(x,@"^([-\w]+|<[-\w]+>)$"))
            |> Set.map(symbolRender)

        let sourceCode = 
            [
                for i in symbols do
                    $"{i}: \"HtmlAttribute list\""
            ] |> String.concat "\r\n"
        output.WriteLine(sourceCode)

    [<Fact(Skip="once and for all!")>] // 
    member _.``5-generate parsing table``() =
        let name = "HtmlParseTable"
        let moduleName = $"FSharp.HTML.{name}"

        let fsyacc = FsyaccFile.parse text
        let parseTbl = fsyacc.toFsyaccParseTable()
        //解析表数据
        let fsharpCode = parseTbl.generate(moduleName)
        let outputDir = Path.Combine(projPath, $"{name}.fs")

        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output path:"+outputDir)

    [<Fact>]
    member _.``6 - valid ParseTable``() =
        let fsyacc = FsyaccFile.parse text
        let t = fsyacc.toFsyaccParseTable()

        Should.equal t.header        HtmlParseTable.header
        Should.equal t.productions   HtmlParseTable.productions
        Should.equal t.actions       HtmlParseTable.actions
        Should.equal t.kernelSymbols HtmlParseTable.kernelSymbols
        Should.equal t.semantics     HtmlParseTable.semantics
        Should.equal t.declarations  HtmlParseTable.declarations


    [<Fact>]
    member _.``7 - list all tokens``() =
        let fsyacc = FsyaccFile.parse text
        let grammar = Grammar.from fsyacc.mainProductions        
        let tokens = 
            grammar.symbols - grammar.nonterminals
        output.WriteLine($"any={clazz tokens}")

    [<Fact>]
    member _.``8 - first or last token of node``() =
        let fsyacc = FsyaccFile.parse text

        let grammar = Grammar.from fsyacc.mainProductions
        let last = grammar.lasts.["node"] |> Set.add "DOCTYPE"
        let first = grammar.firsts.["node"]

        output.WriteLine($"last={clazz last}")
        output.WriteLine($"first={clazz first}")