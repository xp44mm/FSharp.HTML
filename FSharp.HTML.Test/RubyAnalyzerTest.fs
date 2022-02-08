namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type RubyAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let parse txt =
        txt
        |> Tokenizer.tokenize
        |> Seq.choose (HtmlTokenUtils.unifyVoidElement)
        // 临时措施
        |> Seq.filter(function Text x when String.IsNullOrWhiteSpace x -> false | _ -> true)


        |> RubyDFA.analyze
        |> Seq.concat

        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse

    [<Fact>]
    member _.``ListElementsWithoutListContainer``() =
        let simpleHtml = """
        <ruby>
        明日 <rp>(<rt>Ashita<rp>)</rp>
        </ruby>
            """
        let y = parse simpleHtml |> snd
        show y
        //let e = [HtmlElement("ruby",[],[HtmlText " 明日 ";HtmlElement("rp",[],[HtmlText "("]);HtmlElement("rt",[],[HtmlText "Ashita"]);HtmlElement("rp",[],[HtmlText ")"])])]
        //Should.equal e y