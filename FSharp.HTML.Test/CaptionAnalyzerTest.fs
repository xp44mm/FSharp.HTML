namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type CaptionAnalyzerTest(output:ITestOutputHelper) =
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

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> NodesParseTable.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace
    [<Fact>]
    member _.``basis``() =
        let x = """
<table>
<caption>He-Man and Skeletor facts
<tr>
    <td> </td>
    <th scope="col" class="heman">He-Man</th>
    <th scope="col" class="skeletor">Skeletor</th>
</tr>
</table>
"""
        //let mutable ls = ResizeArray()
        //try
        //    let y = 
        //        evade x
        //        |> Seq.map(fun x -> 
        //            ls.Add(x)
        //            x
        //        )
        //        |> Seq.toList
        //    show y
        //with _ ->
        //    let ls = List.ofSeq ls
        //    show ls

        let y = parse x
        show y

    [<Fact>]
    member _.``well formed html``() =
        let x = """
<table>
<caption> School auction sign-up sheet </caption>
</table>
"""
        let y = parse x
        show y

