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

        |> PrecNodesParseTable.parse
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


