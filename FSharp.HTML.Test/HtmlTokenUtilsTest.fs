namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals.Literal
open FSharp.xUnit
open FslexFsyacc.Runtime

type tokenizeEntry() =
    static let dataSource = TheoryDataSource([
        "<!DOCTYPE HTML>",{index= 0;length= 15;value= DOCTYPE "HTML"}
        "<!-- Bad -->",{index= 0;length= 12;value= COMMENT " Bad "}
        "<![CDATA[x<y]]>",{index= 0;length= 15;value= CDATA "x<y"}
        "</html>",{index= 0;length= 7;value= TAGEND "html"}
        //"<a",{index= 0;length= 2;value= LANGLE "a"}
        "hello!",{index= 0;length= 6;value= TEXT "hello!"}
    ])
    static member keys = dataSource.keys
    static member get key = dataSource.[key]

type tokenizeRaw() =
    static let dataSource = TheoryDataSource([
        "</title>", [{index= 0;length= 8;value= TAGEND "title"}]
        ">></title>", [{index= 0;length= 2;value= TEXT ">>"};{index= 2;length= 8;value= TAGEND "title"}]

        ])
    static member keys = dataSource.keys
    static member get key = dataSource.[key]

type tokenizeCss() =
    static let dataSource = TheoryDataSource([
        "</style>", [{index= 0;length= 8;value= TAGEND "style"}]
        "/*abc*/</style>", [{index= 0;length= 7;value= TEXT "/*abc*/"};{index= 7;length= 8;value= TAGEND "style"}]
        ".some{}</style>", [{index= 0;length= 7;value= TEXT ".some{}"};{index= 7;length= 8;value= TAGEND "style"}]
        "\"<>\"</style>", [{index= 0;length= 4;value= TEXT "\"<>\""};{index= 4;length= 8;value= TAGEND "style"}]
        "<<</style>", [{index= 0;length= 1;value= TEXT "<"};{index= 1;length= 1;value= TEXT "<"};{index= 2;length= 8;value= TAGEND "style"}]

        ])
    static member keys = dataSource.keys
    static member get key = dataSource.[key]

type tokenizeJavaScript() =
    static let dataSource = TheoryDataSource([
        "</script>", [{index= 0;length= 9;value= TAGEND "script"}]
        "''</script>", [{index= 0;length= 2;value= TEXT "''"};{index= 2;length= 9;value= TAGEND "script"}]
        "\"\"</script>", [{index= 0;length= 2;value= TEXT "\"\""};{index= 2;length= 9;value= TAGEND "script"}]
        "void</script>", [{index= 0;length= 4;value= TEXT "void"};{index= 4;length= 9;value= TAGEND "script"}]
        "<<</script>", [{index= 0;length= 1;value= TEXT "<"};{index= 1;length= 1;value= TEXT "<"};{index= 2;length= 9;value= TAGEND "script"}]
        @"/\\/</script>", [{index= 0;length= 1;value= TEXT "/"};{index= 1;length= 2;value= TEXT "\\\\"};{index= 3;length= 1;value= TEXT "/"};{index= 4;length= 9;value= TAGEND "script"}]
        "</</script>", [{index= 0;length= 1;value= TEXT "<"};{index= 1;length= 1;value= TEXT "/"};{index= 2;length= 9;value= TAGEND "script"}]


        ])
    static member keys = dataSource.keys
    static member get key = dataSource.[key]

type SelfClosing() =
    static let dataSource = TheoryDataSource([
            "<br/>",[{index= 0;length= 5;value= TAGSELFCLOSING("br",[])};{index= 5;length= 0;value= EOF}]
            "<p><br></br></p>",[{index= 0;length= 3;value= TAGSTART("p",[])};{index= 3;length= 4;value= TAGSELFCLOSING("br",[])};{index= 12;length= 4;value= TAGEND "p"};{index= 16;length= 0;value= EOF}]
        ])

    static member keys = dataSource.keys
    static member get key = dataSource.[key]


type HtmlTokenUtilsTest(output:ITestOutputHelper) =

    [<Theory>]
    [<MemberData(nameof tokenizeEntry.keys, MemberType=typeof<tokenizeEntry>)>]
    member _.``01 = tokenizeEntry``(x:string) =
        let getTagRight i (s:string) = { index= i;length= s.Length;value= TAGSTART("",[]) }
        let postok = HtmlTokenUtils.tokenizeEntry getTagRight 0 x
        output.WriteLine(stringify postok)
        let e = tokenizeEntry.get x
        Should.equal e postok
                
    [<Theory>]
    [<MemberData(nameof tokenizeRaw.keys, MemberType=typeof<tokenizeRaw>)>]
    member _.``03 = tokenizeRaw``(x:string) =
        let restloop offset rest = []
        let postok =
            x
            |> HtmlTokenUtils.tokenizeRaw restloop 0
            |> Seq.toList
        output.WriteLine(stringify postok)
        let e = tokenizeRaw.get x
        Should.equal e postok

    [<Theory>]
    [<MemberData(nameof tokenizeCss.keys, MemberType=typeof<tokenizeCss>)>]
    member _.``04 = tokenizeCss``(x:string) =
        let restloop offset rest = []
        let postok =
            x
            |> HtmlTokenUtils.tokenizeCss restloop 0
            |> Seq.toList
        output.WriteLine(stringify postok)
        let e = tokenizeCss.get x
        Should.equal e postok

    [<Theory>]
    [<MemberData(nameof tokenizeJavaScript.keys, MemberType=typeof<tokenizeJavaScript>)>]
    member _.``04 = tokenizeJavaScript``(x:string) =
        let restloop offset rest = []
        let postoks =
            x
            |> HtmlTokenUtils.tokenizeJavaScript restloop 0
            |> Seq.map(fun postok ->
                output.WriteLine(stringify postok)
                postok
            )
            |> Seq.toList

        let e = tokenizeJavaScript.get x
        Should.equal e postoks
                
    [<Theory>]
    [<MemberData(nameof SelfClosing.keys, MemberType=typeof<SelfClosing>)>]
    member _.``self closing``(x:string) =
        let y = 
            x
            |> HtmlTokenUtils.tokenize TagLeftCompiler.compile 0
            |> Seq.choose HtmlTokenUtils.unifyVoidElement
            |> Seq.toList

        let e = SelfClosing.get x
        Should.equal y e



