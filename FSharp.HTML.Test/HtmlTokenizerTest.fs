namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit

type HtmlTokenizerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
    let tokenlist x =
        x |> Tokenizer.tokenize |> Seq.toList

    [<Fact>]
    member _.``The DOCTYPE``() =
        let x = "<!DOCTYPE html>"
        let y = tokenlist x
        show y
        Should.equal y [DocType "html"]
        
    [<Fact>]
    member _.``Void elements``() =
        let x = """<link rel="author license" href="/about">"""
        let y = tokenlist x
        show y
        Should.equal y [TagStart("link",[HtmlAttribute("rel","author license");HtmlAttribute("href","/about")])]
    
    [<Fact>]
    member _.``The template element``() =
        let x = """<template id="template"><p>Smile!</p></template>"""
        let y = tokenlist x
        show y
        Should.equal y [
            TagStart("template",[HtmlAttribute("id","template")]);
            TagStart("p",[]);
            Text "Smile!";
            TagEnd "p";
            TagEnd "template"]

    [<Fact>]
    member _.``Raw text elements script``() =
        let x = """<script referrerpolicy="origin">
        fetch('/api/data');    // not fetched with <script>'s referrer policy
        import('./utils.mjs'); // is fetched with <script>'s referrer policy ("origin" in this case)
        </script>"""
        let y = tokenlist x
        show y
        Should.equal y [TagStart("script",[HtmlAttribute("referrerpolicy","origin")]);HtmlToken.Text "\r\n        fetch('/api/data');    // not fetched with <script>'s referrer policy\r\n        import('./utils.mjs'); // is fetched with <script>'s referrer policy (\"origin\" in this case)\r\n        ";TagEnd "script"]

    [<Fact>]
    member _.``Raw text elements style``() =
        let x = """<style>
         body { color: black; background: white; }
         em { font-style: normal; color: red; }
        </style>"""
        let y = tokenlist x
        show y
        Should.equal y [TagStart("style",[]);HtmlToken.Text "\r\n         body { color: black; background: white; }\r\n         em { font-style: normal; color: red; }\r\n        ";TagEnd "style"]

    [<Fact>]
    member _.``Escapable raw text elements``() =
        let x = """<textarea cols=80 name=comments>You rock!</textarea>"""
        let y = tokenlist x
        show y
        Should.equal y [TagStart("textarea",[HtmlAttribute("cols","80");HtmlAttribute("name","comments")]);HtmlToken.Text "You rock!";TagEnd "textarea"]

    [<Fact>]
    member _.``self closing tag``() =
        let x = """<cdr:license xmlns:cdr="https://www.example.com/cdr/metadata" name="MIT"/>"""
        let y = tokenlist x
        show y
        Should.equal y [TagSelfClosing("cdr:license",[HtmlAttribute("xmlns:cdr","https://www.example.com/cdr/metadata");HtmlAttribute("name","MIT")])]

    [<Fact>]
    member _.``CDATA sections``() =
        let x = """<![CDATA[x<y]]>"""
        let y = tokenlist x
        show y
        Should.equal y [CData "x<y"]

    [<Fact>]
    member _.``Comments``() =
        let x = """<!-- where is this comment in the DOM? -->"""
        let y = tokenlist x
        show y
        Should.equal y [Comment " where is this comment in the DOM? "]

