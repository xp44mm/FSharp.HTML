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
        |> HtmlTokenUtils.preamble
        |> snd
        |> Seq.choose HtmlTokenUtils.unifyVoidElement

        |> ListDFA.analyze
        |> Seq.concat

        |> RubyDFA.analyze
        |> Seq.concat

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> NodesParseTable.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace
    [<Fact>]
    member _.``well formed``() =
        let x = """
        <ruby>
        明日 <rp>(</rp><rt>Ashita</rt><rp>)</rp>
        </ruby>
            """
        let y = parse x
        show y

    [<Fact>]
    member _.``basis``() =
        let x = """
        <ruby>
        明日 <rp>(<rt>Ashita<rp>)
        </ruby>
            """
        let y = parse x
        show y
