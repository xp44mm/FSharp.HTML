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