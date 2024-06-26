﻿namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc

type HtmlTokenizerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    static let folder = Path.Combine(Dir.TestData,"HtmlTokenizer")

    // generated by `print files`
    static let dataSource = SingleDataSource [
        "cdata.html",[{index= 0;length= 15;value= CDATA "x<y"};{index= 15;length= 2;value= TEXT "\r\n"};{index= 17;length= 0;value= EOF}]
        "cdr.html",[{index= 0;length= 74;value= TAGSELFCLOSING("cdr:license",["xmlns:cdr","https://www.example.com/cdr/metadata";"name","MIT"])};{index= 74;length= 2;value= TEXT "\r\n"};{index= 76;length= 0;value= EOF}]
        "comment.html",[{index= 0;length= 42;value= COMMENT " where is this comment in the DOM? "};{index= 42;length= 2;value= TEXT "\r\n"};{index= 44;length= 0;value= EOF}]
        "DOCTYPE.html",[{index= 0;length= 15;value= DOCTYPE "html"};{index= 15;length= 2;value= TEXT "\r\n"};{index= 17;length= 0;value= EOF}]
        "link.html",[{index= 0;length= 41;value= TAGSTART("link",["rel","author license";"href","/about"])};{index= 41;length= 2;value= TEXT "\r\n"};{index= 43;length= 0;value= EOF}]
        "h3.html",[{index= 0;length= 6;value= TAGEND "h3"};{index= 6;length= 2;value= TEXT "\r\n"};{index= 8;length= 0;value= EOF}];
        "helloworld.html",[{index= 0;length= 14;value= TEXT "hello world!\r\n"};{index= 14;length= 0;value= EOF}];
        "script.html",[{index= 0;length= 32;value= TAGSTART("script",["referrerpolicy","origin"])};{index= 32;length= 8;value= TEXT "\r\nfetch("};{index= 40;length= 11;value= TEXT "'/api/data'"};{index= 51;length= 6;value= TEXT ");    "};{index= 57;length= 46;value= TEXT "// not fetched with <script>'s referrer policy"};{index= 103;length= 9;value= TEXT "\r\nimport("};{index= 112;length= 13;value= TEXT "'./utils.mjs'"};{index= 125;length= 3;value= TEXT "); "};{index= 128;length= 69;value= TEXT "// is fetched with <script>'s referrer policy (\"origin\" in this case)"};{index= 197;length= 2;value= TEXT "\r\n"};{index= 199;length= 9;value= TAGEND "script"};{index= 208;length= 2;value= TEXT "\r\n"};{index= 210;length= 0;value= EOF}]
        "scriptjs.html",[{index= 0;length= 8;value= TAGSTART("script",[])};{index= 8;length= 58;value= TEXT "\r\n    const userInfo = JSON.parse(document.getElementById("};{index= 66;length= 6;value= TEXT "\"data\""};{index= 72;length= 26;value= TEXT ").text);\r\n    console.log("};{index= 98;length= 22;value= TEXT "\"User information: %o\""};{index= 120;length= 14;value= TEXT ", userInfo);\r\n"};{index= 134;length= 9;value= TAGEND "script"};{index= 143;length= 2;value= TEXT "\r\n"};{index= 145;length= 0;value= EOF}]
        "style.html",[{index= 0;length= 7;value= TAGSTART("style",[])};{index= 7;length= 87;value= TEXT "\r\n body { color: black; background: white; }\r\n em { font-style: normal; color: red; }\r\n"};{index= 94;length= 8;value= TAGEND "style"};{index= 102;length= 2;value= TEXT "\r\n"};{index= 104;length= 0;value= EOF}]
        "stylecss.html",[{index= 0;length= 7;value= TAGSTART("style",[])};{index= 7;length= 29;value= TEXT "\r\np {\r\n  color: #26b72b;\r\n}\r\n"};{index= 36;length= 8;value= TAGEND "style"};{index= 44;length= 2;value= TEXT "\r\n"};{index= 46;length= 0;value= EOF}];
        "tagself.html",[{index= 0;length= 38;value= TAGSELFCLOSING("div",["id","xy";"class","\"";"dir","/";"visible",""])};{index= 38;length= 2;value= TEXT "\r\n"};{index= 40;length= 0;value= EOF}];
        "tagstart.html",[{index= 0;length= 37;value= TAGSTART("div",["id","xy";"class","\"";"dir","/";"visible",""])};{index= 37;length= 2;value= TEXT "\r\n"};{index= 39;length= 0;value= EOF}];
        "template.html",[{index= 0;length= 24;value= TAGSTART("template",["id","template"])};{index= 24;length= 3;value= TAGSTART("p",[])};{index= 27;length= 6;value= TEXT "Smile!"};{index= 33;length= 4;value= TAGEND "p"};{index= 37;length= 11;value= TAGEND "template"};{index= 48;length= 2;value= TEXT "\r\n"};{index= 50;length= 0;value= EOF}]
        "textarea.html",[{index= 0;length= 32;value= TAGSTART("textarea",["cols","80";"name","comments"])};{index= 32;length= 9;value= TEXT "You rock!"};{index= 41;length= 11;value= TAGEND "textarea"};{index= 52;length= 2;value= TEXT "\r\n"};{index= 54;length= 0;value= EOF}]
        "title.html",[{index= 0;length= 7;value= TAGSTART("title",[])};{index= 7;length= 3;value= TEXT "x>y"};{index= 10;length= 8;value= TAGEND "title"};{index= 18;length= 2;value= TEXT "\r\n"};{index= 20;length= 0;value= EOF}];
        ]
    static member keys = dataSource.keys
    static member get key = dataSource.[key]

    [<Fact(Skip="get filenames")>] // 
    member _.``print files``() =
        let files =
            Directory.GetFiles(folder)
            |> Seq.map(fun f -> 
                let flnm = Path.GetFileName f
                flnm,[])
            |> Seq.toList
        show files

    [<Theory>]
    [<MemberData(nameof HtmlTokenizerTest.keys)>]
    member _.``tokenlist test``(f) =
        let path = Path.Combine(folder,f)
        let text = File.ReadAllText(path)
        let y =
            text
            |> HtmlTokenUtils.tokenize TagLeftCompiler.compile 0
            |> Seq.toList
        show y
        let e = HtmlTokenizerTest.get f
        Should.equal y e
        
