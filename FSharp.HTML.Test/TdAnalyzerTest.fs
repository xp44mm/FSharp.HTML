namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open System.IO
open FSharp.HTML.Parser

type TdAnalyzerTest(output:ITestOutputHelper) =
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

        |> TrDFA.analyze
        |> Seq.concat

        |> TdDFA.analyze
        |> Seq.concat

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> NodesParseTable.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace

    [<Fact>]
    member _.``well formed``() =
        let x = """
<table>
<caption>Alien football stars</caption>
<tr>
    <th scope="col">Player</th>
    <th scope="col">Gloobles</th>
    <th scope="col">Za'taak</th>
</tr>
<tr>
    <th scope="row">TR-7</th>
    <td>7</td>
    <td>4,569</td>
</tr>
<tr>
    <th scope="row">Khiresh Odo</th>
    <td>7</td>
    <td>7,223</td>
</tr>
<tr>
    <th scope="row">Mia Oolong</th>
    <td>9</td>
    <td>6,219</td>
</tr>
</table>
"""

        let y = parse x
        show y


    [<Fact>]
    member _.``basis``() =
        let x = """
<table>
<caption>Alien football stars
<tr>
    <th scope="col">Player
    <th scope="col">Gloobles
    <th scope="col">Za'taak

<tr>
    <th scope="row">TR-7
    <td>7
    <td>4,569</td>

<tr>
    <th scope="row">Khiresh Odo
    <td>7</td>
    <td>7,223</td>
</tr>
<tr>
    <th scope="row">Mia Oolong</th>
    <td>9
    <td>6,219</td>
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


    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    [<InlineData(7)>]
    [<InlineData(8)>]
    [<InlineData(9)>]
    [<InlineData(10)>]
    [<InlineData(11)>]
    [<InlineData(12)>]
    [<InlineData(13)>]
    [<InlineData(14)>]
    [<InlineData(15)>]
    [<InlineData(16)>]
    [<InlineData(17)>]
    [<InlineData(18)>]
    [<InlineData(19)>]
    [<InlineData(20)>]
    [<InlineData(21)>]
    [<InlineData(22)>]
    [<InlineData(23)>]
    [<InlineData(24)>]
    [<InlineData(25)>]
    [<InlineData(26)>]
    [<InlineData(27)>]
    [<InlineData(28)>]
    [<InlineData(29)>]
    [<InlineData(30)>]
    [<InlineData(31)>]
    [<InlineData(32)>]
    [<InlineData(33)>]
    [<InlineData(34)>]
    [<InlineData(35)>]
    [<InlineData(36)>]
    [<InlineData(37)>]
    [<InlineData(38)>]
    [<InlineData(39)>]
    [<InlineData(40)>]
    [<InlineData(41)>]
    [<InlineData(42)>]
    [<InlineData(43)>]
    [<InlineData(44)>]
    [<InlineData(45)>]
    [<InlineData(46)>]
    [<InlineData(47)>]
    [<InlineData(48)>]
    [<InlineData(49)>]
    [<InlineData(50)>]
    [<InlineData(51)>]
    [<InlineData(52)>]
    [<InlineData(53)>]
    [<InlineData(54)>]
    [<InlineData(55)>]
    [<InlineData(56)>]
    [<InlineData(57)>]
    [<InlineData(58)>]
    [<InlineData(59)>]
    [<InlineData(60)>]
    [<InlineData(61)>]
    [<InlineData(62)>]
    [<InlineData(63)>]
    [<InlineData(64)>]
    [<InlineData(65)>]
    [<InlineData(66)>]
    [<InlineData(67)>]
    [<InlineData(68)>]
    [<InlineData(69)>]
    [<InlineData(70)>]
    [<InlineData(71)>]
    [<InlineData(72)>]
    [<InlineData(73)>]
    [<InlineData(74)>]
    [<InlineData(75)>]
    [<InlineData(76)>]
    [<InlineData(77)>]
    [<InlineData(78)>]
    [<InlineData(79)>]
    [<InlineData(80)>]
    [<InlineData(81)>]
    [<InlineData(82)>]
    [<InlineData(83)>]
    [<InlineData(84)>]
    [<InlineData(85)>]
    [<InlineData(86)>]
    [<InlineData(87)>]
    [<InlineData(88)>]
    [<InlineData(89)>]
    [<InlineData(90)>]
    [<InlineData(91)>]
    [<InlineData(92)>]
    member _.``tables``(i) =
        let file = Path.Combine(__SOURCE_DIRECTORY__,$"table{i}.html")
        let txt = File.ReadAllText(file)
        let y = 
            txt
            |> Tokenizer.tokenize
            |> unifyVoidElement
            |> complementLi
            |> complementRuby
            |> complementOption
            |> complementTable
            |> Seq.toList
        show y