namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type CaptionAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let evade txt =
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.choose (HtmlTokenUtils.adapt>>HtmlTokenUtils.unifyVoidElement)

        |> ColgroupDFA.analyze
        |> Seq.concat

        |> CaptionDFA.analyze
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
<caption>He-Man and Skeletor facts
<tr>
    <td> </td>
    <th scope="col" class="heman">He-Man</th>
    <th scope="col" class="skeletor">Skeletor</th>
</tr>
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

        let e = [HtmlElement("table",[],[HtmlElement("caption",[],[HtmlText "He-Man and Skeletor facts "]);HtmlElement("tr",[],[HtmlElement("td",[],[]);HtmlElement("th",[HtmlAttribute("scope","col");HtmlAttribute("class","heman")],[HtmlText "He-Man"]);HtmlElement("th",[HtmlAttribute("scope","col");HtmlAttribute("class","skeletor")],[HtmlText "Skeletor"])])])]
        Should.equal e y
    [<Fact>]
    member _.``well formed html``() =
        let x = """
<table>
<caption> School auction sign-up sheet </caption>
</table>
"""
        let y = parse x |> snd
        show y

        let e = [HtmlElement("table",[],[HtmlElement("caption",[],[HtmlText " School auction sign-up sheet "])])]
        Should.equal e y
