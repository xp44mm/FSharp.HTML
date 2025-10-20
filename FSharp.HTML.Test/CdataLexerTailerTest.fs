namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.LexYacc

type CdataLexerTailerTest(output: ITestOutputHelper) =

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
    member this.``parse cdata section``(i: int) =
        let cases = [
            "x<y]]>", "x<y"  // 基础用例，包含特殊字符
            "hello world]]>", "hello world"  // 普通文本
            "]]>", ""  // 空CDATA内容
            "a]]b>c]]>", "a]]b>c"  // 包含部分结束标记字符
            "<div>content</div>]]>", "<div>content</div>"  // 包含HTML标签
            "text with ] characters]]>", "text with ] characters"  // 包含单个]字符
            "line1\nline2\tline3]]>", "line1\nline2\tline3"  // 包含特殊空白字符
            "special chars: &<>\"']]>", "special chars: &<>\"'"  // 各种特殊字符
            "content]]>more content]]>", "content"  // 遇到第一个]]>就结束，后面的内容不读取
        ]
        let x,e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = CdataLexerTailer.section iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse cdata edge cases``(i: int) =
        let cases = [
            "]]]]>]]>", "]]"  // 多个]字符，第一个]]>结束
            "a]b]c]d]]>", "a]b]c]d"  // 多个分散的]字符
            " Unicode: 中文 🚀 ]] >]]>", " Unicode: 中文 🚀 ]] >"  // Unicode字符
            "]] >]]>", "]] >"  // ]]>前面有空格的情况
        ]
        let x,e = cases.[i] // 调整索引
        let iter = LexicalIterator.forChar x
        let y = CdataLexerTailer.section iter
        output.WriteLine(stringify y)
        Should.equal e y
