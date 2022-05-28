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
    member _.``well-formed``() =
        let x = """
<table>
<caption>Superheros and sidekicks</caption>
<colgroup>
    <col>
    <col span="2" class="batman">
    <col span="2" class="flash">
    </colgroup>
<tr>
</tr>
</table>
"""
        let y = parse x
        show y

    [<Fact>]
    member _.``basis``() =
        let x = """
<table>
<caption>Superheros and sidekicks</caption>
    <col>
    <col span="2" class="batman">
    <col span="2" class="flash">
<tr>
</tr>
</table>
"""
        let y = parse x
        show y
