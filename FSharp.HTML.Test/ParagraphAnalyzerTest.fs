namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type ParagraphAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let evade txt =
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.choose (HtmlTokenUtils.adapt>>HtmlTokenUtils.unifyVoidElement)

        |> ParagraphDFA.analyze
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
<p>Geckos are a group.</p>

<p>Some species live in houses.</p>
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

        //let e = [HtmlElement("table",[],[HtmlElement("caption",[],[HtmlText "Council budget (in £) 2018"]);HtmlElement("thead",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","col")],[HtmlText "Items"]);HtmlElement("th",[HtmlAttribute("scope","col")],[HtmlText "Expenditure"])])]);HtmlElement("tbody",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Donuts"]);HtmlElement("td",[],[HtmlText "3,000"])]);HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Stationery"]);HtmlElement("td",[],[HtmlText "18,000"])])])])]
        //Should.equal e y

    [<Fact>]
    member _.``basis``() =
        let x = """
<p>Geckos are a group.

<p>Some species live in houses.
"""
        //let mutable ls = ResizeArray()
        //try
        //    let y = 
        //        evade x // ($"<div>{x}</div>")
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

        //Should.equal e y

