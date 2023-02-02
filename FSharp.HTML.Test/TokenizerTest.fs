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
        "hello world!",[{index= 0;length= 12;value= TEXT "hello world!"};{index= 12;length= 0;value= EOF}]
        "<!DOCTYPE html>",[{index= 0;length= 15;value= DOCTYPE "html"};{index= 15;length= 0;value= EOF}]
        "<!-- where is this comment in the DOM? -->",[{index= 0;length= 42;value= COMMENT " where is this comment in the DOM? "};{index= 42;length= 0;value= EOF}]
        "<![CDATA[x<y]]>",[{index= 0;length= 15;value= CDATA "x<y"};{index= 15;length= 0;value= EOF}]
        "</h3 >",[{index= 0;length= 6;value= TAGEND "h3"};{index= 6;length= 0;value= EOF}]
        """<div id="xy" class='"' sep=/ visible>""",[{index= 0;length= 37;value= TAGSTART("div",["id","xy";"class","\"";"sep","/";"visible",""])};{index= 37;length= 0;value= EOF}]
        """<div id="xy" class='"' sep=/ visible/>""",[{index= 0;length= 38;value= TAGSELFCLOSING("div",["id","xy";"class","\"";"sep","/";"visible",""])};{index= 38;length= 0;value= EOF}]
        """<title>x>y</title>""",[{index= 0;length= 7;value= TAGSTART("title",[])};{index= 7;length= 3;value= TEXT "x>y"};{index= 10;length= 8;value= TAGEND "title"};{index= 18;length= 0;value= EOF}]
        """
        <style>
        p {
          color: #26b72b;
        }
        </style>
        """,[{index= 0;length= 10;value= TEXT "\r\n        "};{index= 10;length= 7;value= TAGSTART("style",[])};{index= 17;length= 61;value= TEXT "\r\n        p {\r\n          color: #26b72b;\r\n        }\r\n        "};{index= 78;length= 8;value= TAGEND "style"};{index= 86;length= 10;value= TEXT "\r\n        "};{index= 96;length= 0;value= EOF}]
        """
        <script>
        const userInfo = JSON.parse(document.getElementById("data").text);
        console.log("User information: %o", userInfo);
        </script>
        """,[{index= 0;length= 10;value= TEXT "\r\n        "};{index= 10;length= 8;value= TAGSTART("script",[])};{index= 18;length= 142;value= TEXT "\r\n        const userInfo = JSON.parse(document.getElementById(\"data\").text);\r\n        console.log(\"User information: %o\", userInfo);\r\n        "};{index= 160;length= 9;value= TAGEND "script"};{index= 169;length= 10;value= TEXT "\r\n        "};{index= 179;length= 0;value= EOF}]
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
