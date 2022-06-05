namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Runtime

type TokenizerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    static let source = [
        "hello world!",[{index= 0;length= 12;value= Text "hello world!"}]
        "<!DOCTYPE html>", [{index= 0;length= 15;value= DocType "html"}]
        "<!-- where is this comment in the DOM? -->",[{index= 0;length= 42;value= Comment " where is this comment in the DOM? "}]
        "<![CDATA[x<y]]>",[{index= 0;length= 15;value= CData "x<y"}]
        "</h3 >",[{index= 0;length= 6;value= TagEnd "h3"}]
        """<div id="xy" class='"' sep=/ visible>""",[{index= 0;length= 37;value= TagStart("div",["id","xy";"class","\"";"sep","/";"visible",""])}]
        """<div id="xy" class='"' sep=/ visible/>""",[{index= 0;length= 38;value= TagSelfClosing("div",["id","xy";"class","\"";"sep","/";"visible",""])}]
        """<title>x>y</title>""", [{index= 0;length= 7;value= TagStart("title",[])};{index= 7;length= 3;value= Text "x>y"};{index= 10;length= 8;value= TagEnd "title"}]
        """
        <style>
        p {
          color: #26b72b;
        }
        </style>
        """,[{index= 0;length= 10;value= Text "\r\n        "};
        {index= 10;length= 7;value= TagStart("style",[])};
        {index= 17;length= 61;value= Text "\r\n        p {\r\n          color: #26b72b;\r\n        }\r\n        "};
        {index= 78;length= 8;value= TagEnd "style"};{index= 86;length= 10;value= Text "\r\n        "}]
        """
        <script>
        const userInfo = JSON.parse(document.getElementById("data").text);
        console.log("User information: %o", userInfo);
        </script>
        """,[{index= 0;length= 10;value= Text "\r\n        "};{index= 10;length= 8;value= TagStart("script",[])};{index= 18;length= 142;value= Text "\r\n        const userInfo = JSON.parse(document.getElementById(\"data\").text);\r\n        console.log(\"User information: %o\", userInfo);\r\n        "};{index= 160;length= 9;value= TagEnd "script"};{index= 169;length= 10;value= Text "\r\n        "}]
        ]

    static let mp = Map.ofList source

    static member inputs = 
        source
        |> Seq.map (fst>>Array.singleton)

    [<Theory; MemberData(nameof TokenizerTest.inputs)>]
    member _.``SeniorTokenizer test``(x) =
        let y = Tokenizer.tokenize x |> Seq.toList
        show y
        Should.equal y mp.[x]