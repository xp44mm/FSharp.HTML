namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open FSharp.Idioms
open FSharp.Literals.Literal
open FSharp.xUnit

open FslexFsyacc.Yacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Runtime

type TagLeftParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    ///符号在输入文件中的表示
    let symbolRender sym =
        if Regex.IsMatch(sym,@"^\w+$") then
            sym
        else JsonString.quote sym

    ///符号类
    let clazz symbols =
        symbols
        |> Seq.map symbolRender
        |> String.concat "|"

    let filePath = Path.Combine(Dir.projPath, "TagLeft.fsyacc") // **input**
    let text = File.ReadAllText(filePath)

    //let rawFsyacc = RawFsyaccFile.parse text
    //let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    // **input**
    let parseTblName = "TagLeftParseTable"
    let moduleName = $"FSharp.HTML.{parseTblName}"
    let parseTblPath = Path.Combine(Dir.projPath, $"{parseTblName}.fs")

    let grammar text =
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toGrammar

    let ambiguousCollection text =
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toAmbiguousCollection

    //解析表数据
    let parseTbl text = 
        text
        |> FlatFsyaccFileUtils.parse
        |> FlatFsyaccFileUtils.toFsyaccParseTableFile

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse

        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar = grammar text

        let tokens = grammar.terminals
        let res = set ["ATTR_NAME";"ATTR_VALUE";"DIV_RANGLE";"LANGLE";"RANGLE"]

        show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let collection = ambiguousCollection text

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods

    [<Fact>]
    member _.``04 - list all states``() =
        let collection = ambiguousCollection text
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar = grammar text
        let terminals =
            grammar.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            grammar.nonterminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let sourceCode =
            [
                "// Do not list symbols whose return value is always `null`"
                ""
                "// terminals: ref to the returned type of `getLexeme`"
                "%type<> " + terminals
                ""
                "// nonterminals"
                "%type<> " + nonterminals
            ] 
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)

    [<Fact()>] // Skip="once and for all!"
    member _.``04 - generate parsing table``() =
        let parseTbl = parseTbl text
        let fsharpCode = parseTbl.generateModule(moduleName)

        File.WriteAllText(parseTblPath,fsharpCode)
        output.WriteLine("output path:"+parseTblPath)

    [<Fact>]
    member _.``08 - valid ParseTable``() =
        let src = parseTbl text

        Should.equal src.actions TagLeftParseTable.actions
        Should.equal src.closures TagLeftParseTable.closures

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

