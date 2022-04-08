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

type TableParseTableTest(output:ITestOutputHelper) =
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
        |> String.concat "\r\n"
        |> sprintf "[\r\n%s\r\n]"
    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let projPath = Path.Combine(solutionPath,@"FSharp.HTML")
    let filePath = Path.Combine(projPath, "table.fsyacc") // **input**
    let text = File.ReadAllText(filePath)
    let rawFsyacc = FsyaccFile.parse text
    let fsyacc = NormFsyaccFile.fromRaw rawFsyacc

    [<Fact>]
    member _.``01 - 显示冲突状态的冲突项目``() =
        let collection =
            AmbiguousCollection.create <| fsyacc.getMainProductions()
        let conflicts =
            collection.filterConflictedClosures()

        let y = Map [
            7,Map [
            "<tbody/>",set [
                {production= ["tbodies"];dot= 0};
                {production= ["tbody";"<tbody/>"];dot= 0}];
            "<tbody>",set [
            {production= ["tbodies"];dot= 0};
            {production= ["tbody";"<tbody>";"</tbody>"];dot= 0};
            {production= ["tbody";"<tbody>";"tr";"</tbody>"];dot= 0}];
            "<tr/>",set [
            {production= ["tbodies"];dot= 0};
            {production= ["tr";"<tr/>"];dot= 0}];
            "<tr>",set [
            {production= ["tbodies"];dot= 0};
            {production= ["tr";"<tr>";"</tr>"];dot= 0}]]]


        show conflicts

        //Should.equal y conflicts

    [<Fact>]
    member _.``02 - 汇总冲突的产生式``() =
        let collection =
            AmbiguousCollection.create <| fsyacc.getMainProductions()
        let conflicts =
            collection.filterConflictedClosures()

        let productions =
            AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
            |> List.ofArray

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = [
            ]

        Should.equal y pprods

    [<Fact>]
    member _.``03 - print the template of type annotaitions``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()

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

    [<Fact>]
    member _.``07 - list all tokens``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()     
        let tokens = 
            grammar.symbols - grammar.nonterminals
        output.WriteLine($"any={clazz tokens}")

    [<Fact>]
    member _.``08 - tr precede & follow``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        //let x = Map.keys grammar.precedes 
        //output.WriteLine(Literal.stringify x)
        let p = grammar.precedes.["<tr>"]
        let f = grammar.follows.["</tr>"]

        output.WriteLine($"ptr={clazz p}")
        output.WriteLine($"ftr={clazz f}")


    [<Fact>]
    member _.``09 - thead precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = grammar.precedes.["</thead>"]

        output.WriteLine($"thead={clazz p}")

    [<Fact>]
    member _.``10 - tbody precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = grammar.precedes.["</tbody>"]

        output.WriteLine($"tbody={clazz p}")
    [<Fact>]
    member _.``11 - tfoot precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = grammar.precedes.["</tfoot>"]

        output.WriteLine($"tfoot={clazz p}")

    [<Fact>]
    member _.``12 - table precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = grammar.precedes.["</table>"]

        output.WriteLine($"table={clazz p}")

