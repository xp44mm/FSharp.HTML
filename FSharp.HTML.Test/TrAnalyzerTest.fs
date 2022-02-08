namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type TrAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let evade txt =
        txt
        |> Tokenizer.tokenize
        |> Seq.choose (HtmlTokenUtils.unifyVoidElement)
        // 临时措施
        |> Seq.filter(function Text x when String.IsNullOrWhiteSpace x -> false | _ -> true)

        |> ColgroupDFA.analyze
        |> Seq.concat

        |> CaptionDFA.analyze
        |> Seq.concat

        |> TheadDFA.analyze
        |> Seq.concat

        |> TbodyDFA.analyze
        |> Seq.concat

        |> TrDFA.analyze
        |> Seq.concat


    let parse txt =
        txt
        |> evade

        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse

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

        let y = parse x |> snd
        show y

        let e = [HtmlElement("table",[],[HtmlElement("caption",[],[HtmlText "Council budget (in £) 2018"]);HtmlElement("thead",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","col")],[HtmlText "Items"]);HtmlElement("th",[HtmlAttribute("scope","col")],[HtmlText "Expenditure"])])]);HtmlElement("tbody",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Donuts"]);HtmlElement("td",[],[HtmlText "3,000"])]);HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Stationery"]);HtmlElement("td",[],[HtmlText "18,000"])])])])]
        Should.equal e y


    [<Fact>]
    member _.``basis``() =
        let x = """
<table>
<caption>Council budget (in £) 2018
    <tr>
        <th scope="row">Donuts</th>
        <td>3,000</td>
    <tr>
        <th scope="row">Stationery</th>
        <td>18,000</td>
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

        let y = parse x |> snd
        show y

        //let e = [HtmlElement("table",[],[
        //    HtmlElement("caption",[],[HtmlText "Council budget (in £) 2018 "]);
        //    HtmlElement("tbody",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Donuts"]);HtmlElement("td",[],[HtmlText "3,000"])]);HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Stationery"]);HtmlElement("td",[],[HtmlText "18,000"])])])])]
        //Should.equal e y

