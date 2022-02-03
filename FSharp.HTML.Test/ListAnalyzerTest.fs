namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type ListAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let readAllText path =
        let path = Path.Combine(__SOURCE_DIRECTORY__, path)
        let text = File.ReadAllText(path)
        text

    let parse txt =
        //path
        //|> readAllText
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.map HtmlTokenUtils.adapt
        |> ListDFA.analyze
        |> Seq.concat
        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse
        //|> fun (tp,ls) -> HtmlDocument(tp,ls)

    [<Fact>]
    member _.``ListElementsWithoutListContainer``() =
        let simpleHtml = @"<!DOCTYPE html><body><ul><li>hello<li>world</li><li>how<li>do</ul>you</body><!--do-->"
        let y = parse simpleHtml |> snd
        let e = [HtmlElement("body",[],[HtmlElement("ul",[],[HtmlElement("li",[],[HtmlText "hello"]);HtmlElement("li",[],[HtmlText "world"]);HtmlElement("li",[],[HtmlText "how"]);HtmlElement("li",[],[HtmlText "do"])]);HtmlText "you"]);HtmlComment "do"]
        Should.equal e y
    [<Fact>]
    member _.``MDN Demo``() =
        let simpleHtml = """
        <p>Apollo astronauts:</p>
        
        <ul>
            <li>Neil Armstrong</li>
            <li>Alan Bean</li>
            <li>Peter Conrad
            <li>Edgar Mitchell</li>
            <li>Alan Shepard
        </ul>
        
        """
        let y = parse simpleHtml |> snd
        show y
        //let e = [HtmlElement("body",[],[HtmlText "you";HtmlElement("ul",[],[HtmlElement("li",[],[HtmlText "do"]);HtmlElement("li",[],[HtmlText "how"]);HtmlElement("li",[],[HtmlText "world"]);HtmlElement("li",[],[HtmlText "hello"])])]);HtmlComment "do"]
       
        //Should.equal e y
    [<Fact>]
    member _.``MDN ol``() =
        let simpleHtml = """
        <ol>
        <li>first item
        <li>second item</li>
        <li>third item
</ol>
        """
        let y = parse simpleHtml |> snd
        show y

    [<Fact>]
    member _.``MDN menu``() =
        let simpleHtml = """
<menu>
<li><button onclick="copy()">Copy</button>
<li><button onclick="cut()">Cut</button></li>
<li><button onclick="paste()">Paste</button>
</menu>        """
        let y = parse simpleHtml |> snd
        show y

