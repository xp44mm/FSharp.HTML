namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.LexYacc

type CommentLexerTailerTest(output: ITestOutputHelper) =

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse basic tags``(i: int) =
        let cases = [
            "-->", ""
            "--->", "-"
            "--->other", "-"
            "t-->other", "t"
        ]
        let x,e = cases.[i]

        let iter = LexicalIterator.forChar x

        let y = CommentLexerTailer.restComment iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    member this.``parse comments with content``(i: int) =
        let cases = [
            "comment content-->", "comment content"
            " multiple words -->", " multiple words "
            "line1\nline2\nline3-->", "line1\nline2\nline3"
            "special chars !@#$%^&*()-->", "special chars !@#$%^&*()"
            "with <tags> inside-->", "with <tags> inside"
        ]
        let x,e = cases.[i]

        let iter = LexicalIterator.forChar x

        let y = CommentLexerTailer.restComment iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse nested dashes and arrows``(i: int) =
        let cases = [
            "->-->", "->"
            "--->-->", "-"
            "---->", "--"
            "-->-->", ""
        ]
        let x,e = cases.[i]

        let iter = LexicalIterator.forChar x

        let y = CommentLexerTailer.restComment iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse edge cases``(i: int) =
        let cases = [
            " -->", " "  // 前面有空格
            "\t-->", "\t"  // 前面有制表符
            "\n-->", "\n"  // 前面有换行
            "  -->end", "  "  // 后面有其他内容
        ]
        let x,e = cases.[i]

        let iter = LexicalIterator.forChar x

        let y = CommentLexerTailer.restComment iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse empty and minimal comments``(i: int) =
        let cases = [
            "-->", ""  // 空注释
            " -->", " "  // 只有一个空格
            "\u0000-->", "\u0000"  // 包含空字符
            "\u00A0-->", "\u00A0"  // 包含非断空格
        ]
        let x,e = cases.[i]

        let iter = LexicalIterator.forChar x

        let y = CommentLexerTailer.restComment iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse comments with unicode and special characters``(i: int) =
        let cases = [
            "中文注释-->", "中文注释"
            "🎉emoji-->", "🎉emoji"
            "café-->", "café"
            "π≈3.14-->", "π≈3.14"
        ]
        let x,e = cases.[i]

        let iter = LexicalIterator.forChar x

        let y = CommentLexerTailer.restComment iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Fact>]
    member this.``parse multiple test cases together``() =
        let testCases = [
            "simple-->", "simple"
            "with spaces -->", "with spaces "
            "with-dashes--->", "with-dashes-"
            "multiple\nlines-->", "multiple\nlines"
            "special!@#$%-->", "special!@#$%"
            "-->", ""  // 最短可能
        ]

        for input, expected in testCases do
            let iter = LexicalIterator.forChar input
            let result = CommentLexerTailer.restComment iter
            output.WriteLine($"Input: '{input}' -> Result: '{result}'")
            Should.equal expected result
