namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type OptionAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let parse txt =
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.map HtmlTokenUtils.adapt

        |> OptgroupDFA.analyze
        |> Seq.concat

        |> OptionDFA.analyze
        |> Seq.concat


        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse

    [<Fact>]
    member _.``basis``() =
        let simpleHtml = """
<label for="dino-select">Choose a dinosaur:</label>
<select id="dino-select">
    <optgroup label="a">
        <option>1</option>
        <option>2
        <option>3
    </optgroup>
    <optgroup label="b">
        <option>11
        <option>22
        <option>33
    <optgroup label="c">
        <option>111
        <option>222
        <option>333
</select>
            """
        let y = parse simpleHtml |> snd
        show y
        //问题：会有多余的尾空格
        let e = [HtmlElement("label",[HtmlAttribute("for","dino-select")],[HtmlText "Choose a dinosaur:"]);HtmlElement("select",[HtmlAttribute("id","dino-select")],[HtmlElement("optgroup",[HtmlAttribute("label","a")],[HtmlElement("option",[],[HtmlText "1"]);HtmlElement("option",[],[HtmlText "2 "]);HtmlElement("option",[],[HtmlText "3 "])]);HtmlElement("optgroup",[HtmlAttribute("label","b")],[HtmlElement("option",[],[HtmlText "11 "]);HtmlElement("option",[],[HtmlText "22 "]);HtmlElement("option",[],[HtmlText "33 "])]);HtmlElement("optgroup",[HtmlAttribute("label","c")],[HtmlElement("option",[],[HtmlText "111 "]);HtmlElement("option",[],[HtmlText "222 "]);HtmlElement("option",[],[HtmlText "333 "])])])]
        Should.equal e y