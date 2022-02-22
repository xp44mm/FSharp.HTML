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

