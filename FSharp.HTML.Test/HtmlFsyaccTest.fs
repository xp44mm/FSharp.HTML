namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit

type HtmlFsyaccTest(output:ITestOutputHelper) =
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

        |> SemiNodeDFA.analyze
        |> Seq.concat

        |> HtmlParseTable.parse

    [<Fact>]
    member _.``Void elements``() =
        let x = """<br rel="author license" href="/about">"""
        let y = parse(x)
        show y
        //Should.equal y [Tag(false,"link",[HtmlAttribute("rel","author license");HtmlAttribute("href","/about")])]
    
    [<Fact>]
    member _.``The template element``() =
        let x = """<template id="template"><p>Smile!</p></template>"""
        let y = parse(x)
        show y
        //Should.equal y [
        //    Tag(false,"template",[HtmlAttribute("id","template")]);
        //    Tag(false,"p",[]);
        //    HtmlToken.Text "Smile!";
        //    TagEnd "p";
        //    TagEnd "template"]

    [<Fact>]
    member _.``Raw text elements script``() =
        let x = """<script referrerpolicy="origin">
        fetch('/api/data');    // not fetched with <script>'s referrer policy
        import('./utils.mjs'); // is fetched with <script>'s referrer policy ("origin" in this case)
        </script>"""
        let y = parse(x)
        show y
        //Should.equal y [Tag(false,"script",[HtmlAttribute("referrerpolicy","origin")]);HtmlToken.Text "\r\n        fetch('/api/data');    // not fetched with <script>'s referrer policy\r\n        import('./utils.mjs'); // is fetched with <script>'s referrer policy (\"origin\" in this case)\r\n        ";TagEnd "script"]

    [<Fact>]
    member _.``Raw text elements style``() =
        let x = """<style>
         body { color: black; background: white; }
         em { font-style: normal; color: red; }
        </style>"""
        let y = parse(x)
        show y
        //Should.equal y [Tag(false,"style",[]);HtmlToken.Text "\r\n         body { color: black; background: white; }\r\n         em { font-style: normal; color: red; }\r\n        ";TagEnd "style"]

    [<Fact>]
    member _.``Escapable raw text elements``() =
        let x = """<textarea cols=80 name=comments>You rock!</textarea>"""
        let y = parse(x)
        show y
        //Should.equal y [Tag(false,"textarea",[HtmlAttribute("cols","80");HtmlAttribute("name","comments")]);HtmlToken.Text "You rock!";TagEnd "textarea"]

    [<Fact>]
    member _.``self closing tag``() =
        let x = """<cdr:license xmlns:cdr="https://www.example.com/cdr/metadata" name="MIT"/>"""
        let y = parse(x)
        show y
        //Should.equal y [Tag(true,"cdr:license",[HtmlAttribute("xmlns:cdr","https://www.example.com/cdr/metadata");HtmlAttribute("name","MIT")])]

    [<Fact>]
    member _.``CDATA sections``() =
        let x = """<![CDATA[x<y]]>"""
        let y = parse(x)
        show y
        //Should.equal y [CData "x<y"]

    [<Fact>]
    member _.``Comments``() =
        let x = """<!-- where is this comment in the DOM? -->"""
        let y = parse(x)
        show y
        //Should.equal y [Comment " where is this comment in the DOM? "]

