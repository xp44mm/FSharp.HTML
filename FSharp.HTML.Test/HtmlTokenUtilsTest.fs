namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit

type HtmlTokenUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 = preamble``() =
        let x = """
        <!DOCTYPE HTML>
        <html>
          <head>
            <title>Hello</title>
          </head>
          <body>
            <p>Welcome to this example.</p>
          </body>
        </html>
        """

        let a,b = 
            x
            |> Tokenizer.tokenize
            |> HtmlTokenUtils.preamble

        show a
        show (b |> Seq.toList)

    [<Fact>]
    member _.``02 = preamble without doctype``() =
        let x = """
        <html>
          <head>
            <title>Hello</title>
          </head>
          <body>
            <p>Welcome to this example.</p>
          </body>
        </html>
        """

        let a,b = 
            x
            |> Tokenizer.tokenize
            |> HtmlTokenUtils.preamble

        show a
        show (b |> Seq.toList)


