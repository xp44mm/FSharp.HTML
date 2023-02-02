namespace FSharp.HTML

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Runtime

type UnifyVoidElementTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    static let source = [
            "<br/>",[{index= 0;length= 5;value= TAGSELFCLOSING("br",[])};{index= 5;length= 0;value= EOF}]
            "<p><br></br></p>",[{index= 0;length= 3;value= TAGSTART("p",[])};{index= 3;length= 4;value= TAGSELFCLOSING("br",[])};{index= 12;length= 4;value= TAGEND "p"};{index= 16;length= 0;value= EOF}]
        ]

    static let mp = Map.ofList source

    static member keys = 
        source
        |> Seq.map (fst>>Array.singleton)
        

    [<Theory;MemberData(nameof UnifyVoidElementTest.keys)>]
    member _.``self closing``(x:string) =
        let y = 
            x
            |> Tokenizer.tokenize
            |> Seq.choose (HtmlTokenUtils.unifyVoidElement)
            |> Seq.toList

        let a = mp.[x]
        show y
        Should.equal y a

