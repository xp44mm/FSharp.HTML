namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type TheadAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let parse txt =
        txt
        |> Tokenizer.tokenize
        |> HtmlTokenUtils.preamble
        |> snd
        |> Seq.choose HtmlTokenUtils.unifyVoidElement

        |> ListDFA.analyze
        |> Seq.concat

        |> RubyDFA.analyze
        |> Seq.concat

        |> OptgroupDFA.analyze
        |> Seq.concat

        |> OptionDFA.analyze
        |> Seq.concat

        |> ColgroupDFA.analyze
        |> Seq.concat

        |> CaptionDFA.analyze
        |> Seq.concat

        |> TheadDFA.analyze
        |> Seq.concat

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> NodesParseTable.parse
        |> Whitespace.removeWsChildren

    [<Fact>]
    member _.``well formed``() =
        let x = """
<table>
<caption>Council budget (in £) 2018</caption>
<thead>
    <tr>
        <th scope="col">Items</th>
        <th scope="col">Expenditure</th>
    </tr>
</thead>
<tbody>
    <tr>
        <th scope="row">Donuts</th>
        <td>3,000</td>
    </tr>
    <tr>
        <th scope="row">Stationery</th>
        <td>18,000</td>
    </tr>
</tbody>
</table>
"""
        //let mutable ls = ResizeArray()
        //try
        //    let y = 
        //        evade x
        //        |> Seq.map(fun x -> 
        //            ls.Add(x)
        //            x
        //        )
        //        |> Seq.toList
        //    show y
        //with _ ->
        //    let ls = List.ofSeq ls
        //    show ls

        let y = parse x
        show y


    [<Fact>]
    member _.``basis``() =
        let x = """
<table>
<caption>Council budget (in £) 2018
<thead>
    <tr>
        <th scope="col">Items</th>
        <th scope="col">Expenditure</th>
    </tr>
<tbody>
    <tr>
        <th scope="row">Donuts</th>
        <td>3,000</td>
    </tr>
    <tr>
        <th scope="row">Stationery</th>
        <td>18,000</td>
    </tr>
</table>
"""
        //let ls = ResizeArray()
        //try
        //    let y = 
        //        evade x
        //        |> Seq.map(fun x -> 
        //            ls.Add(x)
        //            x
        //        )
        //        |> Seq.toList
        //    show y
        //with _ ->
        //    let ls = List.ofSeq ls
        //    show ls

        let y = parse x
        show y

