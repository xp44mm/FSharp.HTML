namespace FSharp.HTML

open Xunit

open FSharp.Idioms.Literal
open FSharp.xUnit

type BrRemoverTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member _.``splitByFirstBr``() =
        let x = """
        <p>
        <span>123 </span>
        <span>xyz </span>
        <br>
        uvw
        <br>
        abc
        </p>
"""

        let y = HtmlCompiler.compileText x |> Whitespace.trimWhitespace
        show y

        let z = 
            match y.[0] with
            | HtmlElement("p",_,children) ->
                BrRemover.splitByFirstBr children
            | m -> failwith (stringify m)
        
        show z
        let e = [HtmlElement("span",[],[HtmlText "123"]);HtmlElement("span",[],[HtmlText "xyz"])],[HtmlText "uvw";HtmlElement("br",[],[]);HtmlText "abc"]
        Should.equal z e

    [<Fact>]
    member _.``splitByBr``() =
        let x = """
        <p>xyz
        <br>
        uvw
        <br>
        abc
        </p>
        """

        let y = 
            HtmlCompiler.compileText x
            |> Whitespace.trimWhitespace

        show y
        let z = 
            match y.[0] with
            | HtmlElement("p",_,children) ->
                BrRemover.splitByBr children
            | m -> failwith (stringify m)
        
        show z
        let e = [[HtmlText "xyz"];[HtmlText "uvw"];[HtmlText "abc"]]

        Should.equal z e

