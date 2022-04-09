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

    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "table.fsyacc") // **input**
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
            ["tbody"]," No exist terminal!";
            ["tfoot"]," No exist terminal!";
            ["thead"]," No exist terminal!";
            ["tr"]," No exist terminal!";
            ["tbody";"<tbody>";"tr";"</tbody>"],"</tbody>";
            ["tfoot";"<tfoot>";"tr";"</tfoot>"],"</tfoot>";
            ["thead";"<thead>";"tr";"</thead>"],"</thead>";
            ["tr";"<tr>";"</tr>"],"</tr>";
            ["tbody";"<tbody/>"],"<tbody/>";
            ["tfoot";"<tfoot/>"],"<tfoot/>";
            ["thead";"<thead/>"],"<thead/>";
            ["tr";"<tr/>"],"<tr/>"]

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
        let p = 
            grammar.precedes.["<tr>"]
            |> Set.toList
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)

        let f = 
            grammar.follows.["</tr>"]
            |> Set.toList
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)

        output.WriteLine($"ptr={clazz p}")
        output.WriteLine($"ftr={clazz f}")

    [<Fact>]
    member _.``09 - thead precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = 
            grammar.precedes.["</thead>"]
            |> Set.toList
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)

        output.WriteLine($"thead={clazz p}")

    [<Fact>]
    member _.``10 - tbody precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = 
            grammar.precedes.["</tbody>"]
            |> Set.toList
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)

        output.WriteLine($"tbody={clazz p}")
    [<Fact>]
    member _.``11 - tfoot precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = 
            grammar.precedes.["</tfoot>"]
            |> Set.toList
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)

        output.WriteLine($"tfoot={clazz p}")

    [<Fact>]
    member _.``12 - table precede``() =
        let grammar = Grammar.from <| fsyacc.getMainProductions()
        let p = 
            grammar.precedes.["</table>"]
            |> Set.toList
            |> List.sortBy(fun x -> Regex.Match(x,@"\w+").Value)

        output.WriteLine($"table={clazz p}")

