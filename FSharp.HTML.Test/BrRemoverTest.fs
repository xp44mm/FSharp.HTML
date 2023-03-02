namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type BrRemoverTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
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

        let _,y = HtmlUtils.parseDoc x
        show y

        let z = 
            match y.[0] with
            | HtmlElement("p",_,children) ->
                BrRemover.splitByFirstBr children
            | m -> failwith (Literal.stringify m)
        
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

        let _,y = HtmlUtils.parseDoc x
        show y
        let z = 
            match y.[0] with
            | HtmlElement("p",_,children) ->
                BrRemover.splitByBr children
            | m -> failwith (Literal.stringify m)
        
        show z
        let e = [[HtmlText "xyz"];[HtmlText "uvw"];[HtmlText "abc"]]

        Should.equal z e

