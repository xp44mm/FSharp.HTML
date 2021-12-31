namespace FSharp.HTML

open Xunit
open Xunit.Abstractions

open FSharp.HTML.TagNames
open FSharp.Literals


type CodeGeneratorTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let textElements = 
        rawTextElements + escapableRawTextElements

    let normalElements =
        allTagNames - voidElements

    [<Fact>]
    member _.``void element``() =
        let lines = 
            [
                for name in voidElements do
                    $"""    | "<{name}>" {{HtmlElement("{name}", s0, [])}}"""
                    $"""    | "<{name}>" "</{name}>" {{HtmlElement(fst s0,snd s0, [])}}"""
            ] 
            |> String.concat System.Environment.NewLine
        output.WriteLine(lines)

    [<Fact>]
    member _.``normalElement``() =
        let lines = 
            [
                for name in normalElements do
                    $"""    | "<{name}>" nodes "</{name}>" {{HtmlElement("{name}", s0, s1)}}"""
            ] 
            |> String.concat System.Environment.NewLine
        output.WriteLine(lines)

    [<Fact(Skip="standby")>]
    member _.``selfClosingElement``() =
        let lines = 
            [
                for name in allTagNames do
                    $"""    | "<{name}/>" {{HtmlNode}}"""
            ] 
            |> String.concat System.Environment.NewLine
        output.WriteLine(lines)

    [<Fact(Skip="standby")>]
    member _.``text Element``() =
        let lines = 
            [
                for name in textElements do
                    $"""    | "<{name}>" TEXT "</{name}>" {{HtmlNode}}"""
            ] 
            |> String.concat System.Environment.NewLine
        output.WriteLine(lines)


