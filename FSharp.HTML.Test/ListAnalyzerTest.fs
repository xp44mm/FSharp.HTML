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

    let parse txt =
        txt
        |> Tokenizer.tokenize
        |> HtmlTokenUtils.preamble
        |> snd
        |> Seq.choose HtmlTokenUtils.unifyVoidElement

        |> PrecNodesParseTable.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace

    [<Fact>]
    member _.``ListElementsWithoutListContainer``() =
        let x = @"<!DOCTYPE html><body><ul><li>hello<li>world</li><li>how<li>do</ul>you</body><!--do-->"
        let y = parse x
        show y
    [<Fact>]
    member _.``MDN Demo``() =
        let x = """
        <ul>
            <li>Neil Armstrong</li>
            <li>Alan Bean</li>
            <li>Peter Conrad
            <li>Edgar Mitchell</li>
            <li>Alan Shepard
        </ul>
        """
        //let z = Tokenizer.tokenize x |> Seq.toList
        //show z

        let y = parse x
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
        let y = parse simpleHtml
        show y

    [<Fact>]
    member _.``MDN menu``() =
        let simpleHtml = """
        <menu>
        <li><button onclick="copy()">Copy</button>
        <li><button onclick="cut()">Cut</button></li>
        <li><button onclick="paste()">Paste</button>
        </menu>        
        """
        let y = parse simpleHtml
        show y

