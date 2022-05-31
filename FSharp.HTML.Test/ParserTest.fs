namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals

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
        let y = Parser.parseDoc x
        show y

    [<Fact>]
    member _.``parseNodes``() =
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
        let y = HtmlUtils.parseDoc x
        show y

