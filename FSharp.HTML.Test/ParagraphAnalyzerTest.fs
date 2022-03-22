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

        |> OptgroupDFA.analyze
        |> Seq.concat

        |> OptionDFA.analyze
        |> Seq.concat

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

        |> TdDFA.analyze
        |> Seq.concat

        |> ParagraphDFA.analyze
        |> Seq.concat

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> NodesParseTable.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace


    [<Fact>]
    member _.``well formed``() =
        let x = """
<p>Geckos are a group.</p>

<p>Some species live in houses.</p>
"""
        let y = parse x
        show y

    [<Fact>]
    member _.``basis``() =
        let x = """
<div>
<p>Geckos are a group.
<p>Some species live in houses.
</div>
"""

        let y = parse x
        show y


