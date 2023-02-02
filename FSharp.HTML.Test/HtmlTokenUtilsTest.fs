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
        """,(Some {index= 10;length= 15;value= DOCTYPE "HTML"},[{index= 35;length= 6;value= TAGSTART("html",[])};{index= 41;length= 12;value= TEXT "\r\n          "};{index= 53;length= 6;value= TAGSTART("head",[])};{index= 59;length= 14;value= TEXT "\r\n            "};{index= 73;length= 7;value= TAGSTART("title",[])};{index= 80;length= 5;value= TEXT "Hello"};{index= 85;length= 8;value= TAGEND "title"};{index= 93;length= 12;value= TEXT "\r\n          "};{index= 105;length= 7;value= TAGEND "head"};{index= 112;length= 12;value= TEXT "\r\n          "};{index= 124;length= 6;value= TAGSTART("body",[])};{index= 130;length= 14;value= TEXT "\r\n            "};{index= 144;length= 3;value= TAGSTART("p",[])};{index= 147;length= 24;value= TEXT "Welcome to this example."};{index= 171;length= 4;value= TAGEND "p"};{index= 175;length= 12;value= TEXT "\r\n          "};{index= 187;length= 7;value= TAGEND "body"};{index= 194;length= 10;value= TEXT "\r\n        "};{index= 204;length= 7;value= TAGEND "html"};{index= 211;length= 10;value= TEXT "\r\n        "};{index= 221;length= 0;value= EOF}])

        """
        <html>
          <head>
            <title>Hello</title>
          </head>
          <body>
            <p>Welcome to this example.</p>
          </body>
        </html>
        """,(None,[{index= 10;length= 6;value= TAGSTART("html",[])};{index= 16;length= 12;value= TEXT "\r\n          "};{index= 28;length= 6;value= TAGSTART("head",[])};{index= 34;length= 14;value= TEXT "\r\n            "};{index= 48;length= 7;value= TAGSTART("title",[])};{index= 55;length= 5;value= TEXT "Hello"};{index= 60;length= 8;value= TAGEND "title"};{index= 68;length= 12;value= TEXT "\r\n          "};{index= 80;length= 7;value= TAGEND "head"};{index= 87;length= 12;value= TEXT "\r\n          "};{index= 99;length= 6;value= TAGSTART("body",[])};{index= 105;length= 14;value= TEXT "\r\n            "};{index= 119;length= 3;value= TAGSTART("p",[])};{index= 122;length= 24;value= TEXT "Welcome to this example."};{index= 146;length= 4;value= TAGEND "p"};{index= 150;length= 12;value= TEXT "\r\n          "};{index= 162;length= 7;value= TAGEND "body"};{index= 169;length= 10;value= TEXT "\r\n        "};{index= 179;length= 7;value= TAGEND "html"};{index= 186;length= 10;value= TEXT "\r\n        "};{index= 196;length= 0;value= EOF}])
        ] 

    static let mp = 
        source
        |> Map.ofSeq

    static member inputs = 
        source
        |> Seq.map(fun(a,_)->Array.singleton a)

    [<Theory;MemberData(nameof HtmlTokenUtilsTest.inputs)>]
    member _.``preamble test``(x) =

        let a,b = 
            x
            |> Tokenizer.tokenize
            |> Compiler.preamble
        let b =  b |> Seq.toList
        show (a,b)
        Should.equal (a,b) mp.[x]


