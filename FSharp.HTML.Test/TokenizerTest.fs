
namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit

type TokenizerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 = Text``() =
        let x = "hello world!"
        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = Text x
        Should.equal y e

    [<Fact>]
    member _.``02 = DOCTYPE``() =
        let x = "<!DOCTYPE html>"
        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = DocType "html"
        Should.equal y e

    [<Fact>]
    member _.``03 = Comment``() =
        let x = "<!-- where is this comment in the DOM? -->"

        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = Comment " where is this comment in the DOM? "
        Should.equal y e

    [<Fact>]
    member _.``04 = CDATA``() =
        let x = "<![CDATA[x<y]]>"

        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = CData "x<y"
        Should.equal y e

    [<Fact>]
    member _.``05 = TagEnd``() =
        let x = "</h3 >"

        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = TagEnd "h3"
        Should.equal y e

    [<Fact>]
    member _.``06 = StartTagOpen``() =
        let x = """<div id="xy" class='"' sep=/ visible>"""

        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = TagStart("div",[HtmlAttribute("id","xy");HtmlAttribute("class","\"");HtmlAttribute("sep","/");HtmlAttribute("visible","")])
        Should.equal y e

    [<Fact>]
    member _.``07 = self closing``() =
        let x = """<div id="xy" class='"' sep=/ visible/>"""

        let y = Tokenizer.tokenize x |> Seq.exactlyOne
        show y
        let e = TagSelfClosing("div",[HtmlAttribute("id","xy");HtmlAttribute("class","\"");HtmlAttribute("sep","/");HtmlAttribute("visible","")])
        Should.equal y e

    [<Fact>]
    member _.``08 = title``() =
        let x = """<title>x>y</title>"""
        let y = Tokenizer.tokenize x |> Seq.toList
        show y
        let e = [TagStart("title",[]);Text "x>y";TagEnd "title"]
        Should.equal y e

    [<Fact>]
    member _.``09 = style``() =
        let x = """
        <style>
        p {
          color: #26b72b;
        }
        </style>
        """
        let y = Tokenizer.tokenize (x.Trim()) |> Seq.toList
        show y
        let e = [TagStart("style",[]);Text "\r\n        p {\r\n          color: #26b72b;\r\n        }\r\n        ";TagEnd "style"]
        Should.equal y e
    [<Fact>]
    member _.``10 = script``() =
        let x = """
        <script>
        const userInfo = JSON.parse(document.getElementById("data").text);
        console.log("User information: %o", userInfo);
        </script>
        """
        let y = Tokenizer.tokenize (x.Trim()) |> Seq.toList
        show y
        let e = [TagStart("script",[]);Text "\r\n        const userInfo = JSON.parse(document.getElementById(\"data\").text);\r\n        console.log(\"User information: %o\", userInfo);\r\n        ";TagEnd "script"]
        Should.equal y e
