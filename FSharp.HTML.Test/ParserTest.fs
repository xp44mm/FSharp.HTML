namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type ParserTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
          
    [<Fact>]
    member _.``parseDoc``() =
        let x = """
        <!DOCTYPE html>
        <html>
          <head>
            <meta charset="utf-8">
            <title>My test page</title>
          </head>
          <body>
            <img src="images/firefox-icon.png" alt="My test image">
          </body>
        </html>
        """
        let dtp, nodes = HtmldocCompiler.compile x
        Should.equal dtp "html"
        
        show nodes
        let e =     [
            HtmlText "\r\n        ";
            HtmlElement("html",[],[HtmlText "\r\n          ";
            HtmlElement("head",[],[HtmlText "\r\n            ";
            HtmlElement("meta",["charset","utf-8"],[]);
            HtmlText "\r\n            ";HtmlElement("title",[],[
            HtmlText "My test page"]);HtmlText "\r\n          "]);
            HtmlText "\r\n          ";HtmlElement("body",[],[
            HtmlText "\r\n            ";
            HtmlElement("img",["src","images/firefox-icon.png";"alt","My test image"],[]);
            HtmlText "\r\n          "]);HtmlText "\r\n        "]);
            HtmlText "\r\n        "]

        Should.equal nodes e
