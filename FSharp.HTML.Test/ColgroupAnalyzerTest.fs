namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type ColgroupAnalyzerTest(output:ITestOutputHelper) =
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

    let parse txt =
        txt
        |> evade

        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse

    [<Fact>]
    member _.``basis``() =
        let x = """
<table>
<caption>Superheros and sidekicks</caption>
    <col>
    <col span="2" class="batman">
    <col span="2" class="flash">
<tr>
    <td> </td>
</tr>
</table>
"""
        let y = parse x |> snd
        show y
        let e = [HtmlElement("table",[],[HtmlElement("caption",[],[HtmlText "Superheros and sidekicks"]);HtmlElement("colgroup",[],[HtmlElement("col",[],[]);HtmlElement("col",[HtmlAttribute("span","2");HtmlAttribute("class","batman")],[]);HtmlElement("col",[HtmlAttribute("span","2");HtmlAttribute("class","flash")],[])]);HtmlElement("tr",[],[HtmlElement("td",[],[])])])]
        Should.equal e y