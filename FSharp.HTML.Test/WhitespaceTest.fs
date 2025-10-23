namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal

type WhitespaceTest(output:ITestOutputHelper) =
          
    [<Fact>]
    member _.``trim whitespace``() =
        let x = "<p>  <b> xyz</b>Geckos are a group<i>. </i>  </p>"
        let y = 
             HtmlCompiler.compileText x

        let yy = [HtmlElement("p",[],[HtmlWS "  ";HtmlElement("b",[],[HtmlText " xyz"]);HtmlText "Geckos are a group";HtmlElement("i",[],[HtmlText ". "]);HtmlWS "  "])]

        Should.equal y yy

        let z =
            y
            |> Whitespace.trimWhitespace

        let zz = [HtmlElement("p",[],[
            HtmlElement("b",[],[HtmlText "xyz"]);
            HtmlText "Geckos are a group";
            HtmlElement("i",[],[HtmlText "."])
            ])]

        Should.equal z zz
