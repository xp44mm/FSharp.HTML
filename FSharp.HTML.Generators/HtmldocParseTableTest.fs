namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open FSharp.xUnit

open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Precedences
open FslexFsyacc.Runtime.YACCs

type HtmldocParseTableTest(output:ITestOutputHelper) =

    ///符号在输入文件中的表示
    let symbolRender (sym:string) =
        if Regex.IsMatch(sym,@"^\w+$") then
            sym
        else JsonString.quote sym

    ///符号类
    let clazz symbols =
        symbols
        |> Seq.map symbolRender
        |> String.concat "|"

    // **input**
    let parseTblName = "HtmldocParseTable" 
    let parseTblModule = $"FSharp.HTML.{parseTblName}"

    let parseTblPath = Path.Combine(Dir.projPath, $"{parseTblName}.fs")

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "htmldoc.fsyacc")
    let text = File.ReadAllText(filePath)

    let rawFsyacc =
        text
        |> FsyaccCompiler.compile

    let fsyacc =
        rawFsyacc
        |> FslexFsyacc.Runtime.YACCs.FlatFsyaccFile.from

    let coder = FsyaccParseTableCoder.from fsyacc

    let tbl =
        fsyacc.getYacc()

    let bnf = tbl.bnf

    //let grammar text =
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toGrammar

    //let ambiguousCollection text =
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toAmbiguousCollection

    ////解析表数据
    //let parseTbl text = 
    //    text
    //    |> FlatFsyaccFileUtils.parse
    //    |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    //[<Fact>]
    //member _.``01 - norm fsyacc file``() =
    //    let fsyacc = 
    //        text
    //        |> FlatFsyaccFileUtils.parse

    //    let s0 = 
    //        fsyacc.rules
    //        |> FlatFsyaccFileRule.getStartSymbol

    //    let src = 
    //        fsyacc.start(s0, Set.empty)
    //        |> RawFsyaccFileUtils.fromFlat
    //        |> RawFsyaccFileUtils.render

    //    output.WriteLine(src)

    //[<Fact>]
    //member _.``02 - list all tokens``() =
    //    let grammar = grammar text

    //    let tokens = grammar.terminals
    //    let res = set ["CDATA";"COMMENT";"DOCTYPE";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]

    //    //show tokens
    //    Should.equal tokens res


    //[<Fact>]
    //member _.``04 - list all states``() =
    //    let collection = ambiguousCollection text
        
    //    let text = collection.render()
    //    output.WriteLine(text)

    //[<Fact>]
    //member _.``05 - list the type annotaitions``() =
    //    let grammar = grammar text
    //    let terminals =
    //        grammar.terminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let nonterminals =
    //        grammar.nonterminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let sourceCode =
    //        [
    //            "// Do not list symbols whose return value is always `null`"
    //            ""
    //            "// terminals: ref to the returned type of `getLexeme`"
    //            "%type<> " + terminals
    //            ""
    //            "// nonterminals"
    //            "%type<> " + nonterminals
    //        ] 
    //        |> String.concat "\r\n"

    //    output.WriteLine(sourceCode)

    [<Fact>]
    member _.``02 - print conflict productions``() =
        let st = ConflictedProduction.from fsyacc.rules
        if st.IsEmpty then
            output.WriteLine("no conflict")

        for cp in st do
        output.WriteLine($"{stringify cp}")

    [<Fact(
    Skip="once and for all!"
    )>]
    member _.``04 - generate parsing table``() =
        let outp = coder.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine($"{parseTblPath}")

    [<Fact>]
    member _.``10 - valid ParseTable``() =
        Should.equal coder.tokens HtmldocParseTable.tokens
        Should.equal coder.kernels HtmldocParseTable.kernels
        Should.equal coder.actions HtmldocParseTable.actions

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            HtmldocParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = coder.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact>]
    member _.``101 - first or last token of node``() =
        let grammar = tbl.bnf.grammar

        let last = grammar.lasts.["node"]
        let first = grammar.firsts.["node"]

        output.WriteLine($"last={clazz last}")
        output.WriteLine($"first={clazz first}")

