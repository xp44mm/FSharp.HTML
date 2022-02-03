namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type DlParserTest(output:ITestOutputHelper) =
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

        |> DlDFA.analyze
        |> Seq.concat

        |> ListDFA.analyze
        |> Seq.concat

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> HtmlParseTable.parse
        //|> fun (tp,ls) -> HtmlDocument(tp,ls)

    [<Fact>]
    member _.``ListElementsWithoutListContainer``() =
        let simpleHtml = """<!DOCTYPE html><body><dl>
            <dt>hello<dd>world</dd><dt>how<dd>do</dl>you</body><!--do-->
            """
        let y = parse simpleHtml |> snd
        show y
        let e = [HtmlElement("body",[],[HtmlElement("dl",[],[HtmlElement("dt",[],[HtmlText "hello"]);HtmlElement("dd",[],[HtmlText "world"]);HtmlElement("dt",[],[HtmlText "how"]);HtmlElement("dd",[],[HtmlText "do"])]);HtmlText "you"]);HtmlComment "do"]
        Should.equal e y