namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal
open FSharp.xUnit
open System.IO
open System.Text
open System.Reflection
type HtmlCompilerTest(output: ITestOutputHelper) =

    static member files =
        Directory.GetFiles(Dir.omitted)
        |> Seq.map(fun f -> Path.GetFileName f)
        |> Seq.map Array.singleton

    [<Fact>]
    member _.``file name test``() =
        let path = Path.Combine(Dir.TestData, "omitted")
        Directory.GetFiles(path)
        |> Seq.map(fun f -> Path.GetFileName f)
        |> Seq.iter(fun f ->
            output.WriteLine(sprintf "[<InlineData(%s)>]" (stringify f))
        )

    [<Theory>]
    [<InlineData("with.html")>]
    [<InlineData("without.html")>]
    member _.``TestData test``(path: string) =
        let htmlText =
            File.ReadAllText(Path.Combine(Dir.TestData, path), Encoding.UTF8)

        let nodes = HtmlCompiler.compileText htmlText
        output.WriteLine(stringify nodes)

        //Should.equal e nodes
        ()

    [<Theory>]
    [<InlineData("cdata.html")>]
    [<InlineData("dtdd.html")>]
    [<InlineData("eof.html")>]
    [<InlineData("html.html")>]
    [<InlineData("li.html")>]
    [<InlineData("optgroup.html")>]
    [<InlineData("p.html")>]
    [<InlineData("ruby.html")>]
    [<InlineData("table1.html")>]
    [<InlineData("table2.html")>]
    [<InlineData("table3.html")>]
    member _.``omitted``(filename: string) =
        let folder = MethodBase.GetCurrentMethod().Name
        let path = Path.Combine(Dir.TestData, folder, filename)
        let htmlText = File.ReadAllText(path, Encoding.UTF8)

        let nodes = HtmlCompiler.compileText htmlText
        output.WriteLine(stringify nodes)

        //Should.equal e nodes
        ()

    [<Theory>]
    [<InlineData("cdata.html")>]
    [<InlineData("cdr.html")>]
    [<InlineData("comment.html")>]
    [<InlineData("DOCTYPE.html")>]
    [<InlineData("h3.html")>]
    [<InlineData("helloworld.html")>]
    [<InlineData("link.html")>]
    [<InlineData("script.html")>]
    [<InlineData("scriptjs.html")>]
    [<InlineData("style.html")>]
    [<InlineData("stylecss.html")>]
    [<InlineData("tagself.html")>]
    [<InlineData("tagstart.html")>]
    [<InlineData("template.html")>]
    [<InlineData("textarea.html")>]
    [<InlineData("title.html")>]
    member _.``HtmlTokenizer``(filename: string) =
        let folder = MethodBase.GetCurrentMethod().Name
        let path = Path.Combine(Dir.TestData, folder, filename)
        let htmlText = File.ReadAllText(path, Encoding.UTF8)

        let nodes = HtmlCompiler.compileText htmlText
        output.WriteLine(stringify nodes)

        //Should.equal e nodes
        ()

    [<Theory>]
    [<InlineData("cdata.html")>]
    [<InlineData("dtdd.html")>]
    [<InlineData("eof.html")>]
    [<InlineData("html.html")>]
    [<InlineData("li.html")>]
    [<InlineData("optgroup.html")>]
    [<InlineData("p.html")>]
    [<InlineData("ruby.html")>]
    [<InlineData("table1.html")>]
    [<InlineData("table2.html")>]
    [<InlineData("table3.html")>]
    member _.``wellformed``(filename: string) =
        let folder = MethodBase.GetCurrentMethod().Name
        let path = Path.Combine(Dir.TestData, folder, filename)
        let htmlText = File.ReadAllText(path, Encoding.UTF8)

        let nodes = HtmlCompiler.compileText htmlText
        output.WriteLine(stringify nodes)

        //Should.equal e nodes
        ()

    [<Fact>]
    member _.``compile test``() =
        let x =
            """
        <!DOCTYPE html>
        <html>
          <head>
            <meta charset="utf-8">
            <title>My test page</title>
          </head>
          <body>
            <img src="images/firefox-icon.png" alt="My test image">
          </body>
        </html>
        """
        let nodes = HtmlCompiler.compileText x
        output.WriteLine(stringify nodes)
        let e =[HtmlWS "\r\n        ";HtmlDoctype "html";HtmlWS "\r\n        ";HtmlElement("html",[],[HtmlWS "\r\n          ";HtmlElement("head",[],[HtmlWS "\r\n            ";HtmlElement("meta",["charset","utf-8"],[]);HtmlWS "\r\n            ";HtmlElement("title",[],[HtmlText "My test page"]);HtmlWS "\r\n          "]);HtmlWS "\r\n          ";HtmlElement("body",[],[HtmlWS "\r\n            ";HtmlElement("img",["src","images/firefox-icon.png";"alt","My test image"],[]);HtmlWS "\r\n          "]);HtmlWS "\r\n        "]);HtmlWS "\r\n        "]

        Should.equal e nodes
