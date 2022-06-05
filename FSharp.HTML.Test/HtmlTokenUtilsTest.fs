namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Runtime

type HtmlTokenUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    static let source = [
        """
        <!DOCTYPE HTML>
        <html>
          <head>
            <title>Hello</title>
          </head>
          <body>
            <p>Welcome to this example.</p>
          </body>
        </html>
        """,Some {index= 10;length= 15;value= DocType "HTML"},[{index= 35;length= 6;value= TagStart("html",[])};{index= 41;length= 12;value= Text "\r\n          "};{index= 53;length= 6;value= TagStart("head",[])};{index= 59;length= 14;value= Text "\r\n            "};{index= 73;length= 7;value= TagStart("title",[])};{index= 80;length= 5;value= Text "Hello"};{index= 85;length= 8;value= TagEnd "title"};{index= 93;length= 12;value= Text "\r\n          "};{index= 105;length= 7;value= TagEnd "head"};{index= 112;length= 12;value= Text "\r\n          "};{index= 124;length= 6;value= TagStart("body",[])};{index= 130;length= 14;value= Text "\r\n            "};{index= 144;length= 3;value= TagStart("p",[])};{index= 147;length= 24;value= Text "Welcome to this example."};{index= 171;length= 4;value= TagEnd "p"};{index= 175;length= 12;value= Text "\r\n          "};{index= 187;length= 7;value= TagEnd "body"};{index= 194;length= 10;value= Text "\r\n        "};{index= 204;length= 7;value= TagEnd "html"};{index= 211;length= 10;value= Text "\r\n        "}]

        """
        <html>
          <head>
            <title>Hello</title>
          </head>
          <body>
            <p>Welcome to this example.</p>
          </body>
        </html>
        """,None,[{index= 10;length= 6;value= TagStart("html",[])};{index= 16;length= 12;value= Text "\r\n          "};{index= 28;length= 6;value= TagStart("head",[])};{index= 34;length= 14;value= Text "\r\n            "};{index= 48;length= 7;value= TagStart("title",[])};{index= 55;length= 5;value= Text "Hello"};{index= 60;length= 8;value= TagEnd "title"};{index= 68;length= 12;value= Text "\r\n          "};{index= 80;length= 7;value= TagEnd "head"};{index= 87;length= 12;value= Text "\r\n          "};{index= 99;length= 6;value= TagStart("body",[])};{index= 105;length= 14;value= Text "\r\n            "};{index= 119;length= 3;value= TagStart("p",[])};{index= 122;length= 24;value= Text "Welcome to this example."};{index= 146;length= 4;value= TagEnd "p"};{index= 150;length= 12;value= Text "\r\n          "};{index= 162;length= 7;value= TagEnd "body"};{index= 169;length= 10;value= Text "\r\n        "};{index= 179;length= 7;value= TagEnd "html"};{index= 186;length= 10;value= Text "\r\n        "}]
        ] 

    static let mp = 
        source
        |> Seq.map(fun(a,b,c)-> a,(b,c))
        |> Map.ofSeq

    static member inputs = 
        source
        |> Seq.map(fun(a,b,c)->Array.singleton a)

    [<Theory;MemberData(nameof HtmlTokenUtilsTest.inputs)>]
    member _.``preamble test``(x) =

        let a,b = 
            x
            |> Tokenizer.tokenize
            |> HtmlTokenUtils.preamble
        let b =  b |> Seq.toList
        show a
        Should.equal (a,b) mp.[x]


