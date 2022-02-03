namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit

type UnifyVoidElementTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let evade txt =
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.choose (HtmlTokenUtils.adapt>>HtmlTokenUtils.unifyVoidElement)

    [<Fact>]
    member _.``self closing``() =
        let x = "<br/>"
        let y = evade x
        Should.equal y [TagSelfClosing("br",[])]

    [<Fact>]
    member _.``start end``() =
        let x = "<br></br>"
        let y = evade x
        Should.equal y [TagSelfClosing("br",[])]
        