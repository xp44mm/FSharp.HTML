﻿namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type OptgroupAnalyzerTest(output:ITestOutputHelper) =
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

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> HtmlParseTable.parse


    [<Fact>]
    member _.``well-formed``() =
        let x = """
<select id="dino-select">
    <optgroup label="a">
        <option>1</option>
        <option>2</option>
        <option>3</option>
    </optgroup>
    <optgroup label="b">
        <option>11</option>
        <option>22</option>
        <option>33</option>
        </optgroup>

    <optgroup label="c">
        <option>111</option>
        <option>222</option>
        <option>333</option>
        </optgroup>

</select>
            """
        let y = parse x
        show y
    [<Fact>]
    member _.``basis``() =
        let x = """
<select id="dino-select">
    <optgroup label="a">
        <option>1</option>
        <option>2</option>
        <option>3</option>
    </optgroup>
    <optgroup label="b">
        <option>11</option>
        <option>22</option>
        <option>33</option>
    <optgroup label="c">
        <option>111</option>
        <option>222</option>
        <option>333</option>
</select>
            """
        let y = parse x
        show y
