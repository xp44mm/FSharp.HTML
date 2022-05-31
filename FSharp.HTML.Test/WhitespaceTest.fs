namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type WhitespaceTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
          

    [<Fact>]
    member _.``Permitted content no text``() =
        let y = 
            TagNames.htmlTags - TagNames.voidElements - TagNames.escapableRawTextElements
                - TagNames.rawTextElements - TagNames.flowContent
        show y

    [<Fact>]
    member _.``trim whitespace``() =
        let x = "<p>  <b> xyz</b>Geckos are a group<i>. </i>  </p>"
        let y = 
            HtmlUtils.parseDoc x
            |> snd
            |> Whitespace.trimWhitespace

        show y

