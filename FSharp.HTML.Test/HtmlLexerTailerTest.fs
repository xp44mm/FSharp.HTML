namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.LexYacc

type HtmlLexerTailerTest(output: ITestOutputHelper) =

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
    member this.``parse html tokens``(i: int) =
        let cases = [
            // 0. 原始测试用例
            "   <!DOCTYPE html>", [WS"   "; DOCTYPE "html"]
            
            // 1. 注释测试
            "<!-- This is a comment -->", [COMMENT " This is a comment "]
            
            // 2. CDATA 测试
            "<![CDATA[<some><xml></data>]]>", [CDATA "<some><xml></data>"]
            
            // 3. 结束标签测试
            "</div>", [TAGEND "div"]
            
            // 4. 自闭合标签测试
            "<br/>", [TAGSELFCLOSING ("br", [])]
            
            // 5. 开始标签测试（带属性）
            "<div class=\"container\">", [TAGSTART ("div", [("class", "container")])]
            
            // 6. 纯文本测试
            "Hello World", [TEXT "Hello World"]
            
            // 7. 空白文本测试
            "   \t\n\r", [WS"   \t\n\r"]
            
            // 8. 混合内容和标签
            "Text<div>More text</div>", [TEXT "Text"; TAGSTART ("div", []); TEXT "More text"; TAGEND "div"]
            
            // 9. 复杂属性
            "<input type=\"text\" value=\"test\" disabled>", [TAGSELFCLOSING ("input", [("type", "text"); ("value", "test"); ("disabled", "")])]
            
            // 10. 大写标签名
            "<DIV CLASS=\"test\">", [TAGSTART ("DIV", [("CLASS", "test")])]
            
            // 11. 混合大小写
            "<Div Class=\"mixed\">", [TAGSTART ("Div", [("Class", "mixed")])]
            
            // 12. 带命名空间的标签
            "<svg:path d=\"M0 0\"/>", [TAGSELFCLOSING ("svg:path", [("d", "M0 0")])]
            
            // 13. 布尔属性
            "<input checked>", [TAGSELFCLOSING("input",["checked",""])]
                        
            // 14. 复杂的混合场景
            "  <div class=\"main\">Hello<!--comment-->World<br/></div>  ", 
            [WS"  "; TAGSTART ("div", [("class", "main")]); TEXT "Hello"; COMMENT "comment"; TEXT "World"; TAGSELFCLOSING ("br", []); TAGEND "div"; WS "  "]
        ]
        
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = HtmlLexerTailer.tokenize iter |> List.ofSeq
        output.WriteLine($"Input: {x}")
        output.WriteLine($"Expected: {stringify e}")
        output.WriteLine($"Actual: {stringify y}")
        Should.equal e y

    [<Fact>]
    member this.``test all token types``() =
        let html = """
<!DOCTYPE html>
<!-- Comment -->
<![CDATA[cdata content]]>
<html lang="en">
  <head>
    <title>Test</title>
    <meta charset="UTF-8"/>
  </head>
  <body>
    Hello World
    <div class="container">
      Text content
    </div>
  </body>
</html>
"""
        let expected = [WS "\r\n";DOCTYPE "html";WS "\r\n";COMMENT " Comment ";WS "\r\n";CDATA "cdata content";WS "\r\n";TAGSTART("html",["lang","en"]);WS "\r\n  ";TAGSTART("head",[]);WS "\r\n    ";TAGSTART("title",[]);TEXT "Test";TAGEND "title";WS "\r\n    ";TAGSELFCLOSING("meta",["charset","UTF-8"]);WS "\r\n  ";TAGEND "head";WS "\r\n  ";TAGSTART("body",[]);TEXT "\r\n    Hello World\r\n    ";TAGSTART("div",["class","container"]);TEXT "\r\n      Text content\r\n    ";TAGEND "div";WS "\r\n  ";TAGEND "body";WS "\r\n";TAGEND "html";WS "\r\n"]
        let iter = LexicalIterator.forChar html
        let actual = HtmlLexerTailer.tokenize iter |> List.ofSeq
        output.WriteLine($"Input: {html}")
        output.WriteLine($"Expected: {stringify expected}")
        output.WriteLine($"Actual: {stringify actual}")
        Should.equal expected actual

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    member this.``edge cases``(i: int) =
        let cases = [
            // 0. 只有空白
            "   \t\n  \r   ", [WS "   \t\n  \r   "]
            
            // 1. 只有文本
            "Just some text without tags", [TEXT "Just some text without tags"]
            
            // 2. 多个连续空白
            "   \t\n\r   <div>  \t  </div>  ", [WS "   \t\n\r   "; TAGSTART ("div", []); WS"  \t  "; TAGEND "div"; WS"  "]
        ]
        
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = HtmlLexerTailer.tokenize iter |> List.ofSeq
        output.WriteLine($"Input: {x}")
        output.WriteLine($"Expected: {stringify e}")
        output.WriteLine($"Actual: {stringify y}")
        Should.equal e y
