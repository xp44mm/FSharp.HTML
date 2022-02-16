namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type ParserTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
          

    [<Fact>]
    member _.``well formed``() =
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
        let y = Parser.parseDoc x
        show y

        let e = HtmlDocument("html",[|
            HtmlElement("html",Map.empty,[|
                HtmlText "\r\n          ";
                HtmlElement("head",Map.empty,[|
                    HtmlText "\r\n            ";
                    HtmlElement("meta",Map["charset","utf-8"],[||]);
                    HtmlText "\r\n            ";
                    HtmlElement("title",Map.empty,[|HtmlText "My test page"|]);
                        HtmlText "\r\n          "|]);
                        HtmlText "\r\n          ";
                        HtmlElement("body",Map.empty,[|
                            HtmlText "\r\n            ";
                            HtmlElement("img",Map["alt","My test image";"src","images/firefox-icon.png"],[||]);
                            HtmlText "\r\n          "|]);
                            HtmlText "\r\n        "|]);
                            HtmlText "\r\n        "|])
        Should.equal e y