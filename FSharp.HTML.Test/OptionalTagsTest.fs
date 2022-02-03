namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type OptionalTagsTest(output:ITestOutputHelper) =
    let readAllText path =
        let path = Path.Combine(__SOURCE_DIRECTORY__, path)
        let text = File.ReadAllText(path)
        text

    let parse txt =
        //path
        //|> readAllText
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.map HtmlTokenUtils.adapt
        |> ListDFA.analyze
        |> Seq.concat
        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse
        //|> fun (tp,ls) -> HtmlDocument(tp,ls)

    [<Fact>]
    member _.``ListElementsWithoutListContainer``() =
        let simpleHtml = @"<!DOCTYPE html><li>hello<li>world<ul>how<li>do</ul>you</body><!--do-->"
        let result = parse simpleHtml
        let expected =
            //HtmlDocument.New
                [ HtmlNode.NewElement
                      ("ul",
                       [ HtmlNode.NewElement("li", [ HtmlNode.NewElement("div")])
                         HtmlNode.NewElement("li")]) ]
        snd result |> Should.equal expected


    [<Fact>]
    member _.``Can handle unclosed divs inside lis correctly``() =
        let simpleHtml = "<ul><li><div></li><li></li></ul>"
        let result = parse simpleHtml
        let expected =
            //HtmlDocument.New
                [ HtmlNode.NewElement
                      ("ul",
                       [ HtmlNode.NewElement("li", [ HtmlNode.NewElement("div")])
                         HtmlNode.NewElement("li")]) ]
        snd result |> Should.equal expected

    [<Fact>]
    member _.``Can handle unclosed tags correctly``() =
        let simpleHtml = """<html>
                             <head>
                                <script language="JavaScript" src="/bwx_generic.js"></script>
                                <link rel="stylesheet" type="text/css" href="/bwx_style.css">
                                </head>
                            <body>
                                <img src="myimg.jpg">
                                <table title="table">
                                    <tr><th>Column 1</th><th>Column 2</th></tr>
                                    <tr><td>1</td><td>yes</td></tr>
                                </table>
                            </body>
                        </html>"""
        let result = parse simpleHtml

        let expected =
            //HtmlDocument.New
                [ HtmlNode.NewElement
                      ("html",
                       [ HtmlNode.NewElement("head",
                                             [ HtmlNode.NewElement("script",
                                                                   [ "language", "JavaScript"
                                                                     "src", "/bwx_generic.js" ])
                                               HtmlNode.NewElement("link",
                                                                   [ "rel", "stylesheet"
                                                                     "type", "text/css"
                                                                     "href", "/bwx_style.css" ]) ])

                         HtmlNode.NewElement
                             ("body",
                              [ HtmlNode.NewElement("img", [ "src", "myimg.jpg" ])

                                HtmlNode.NewElement
                                    ("table", [ "title", "table" ],
                                     [ HtmlNode.NewElement("tr",
                                                           [ HtmlNode.NewElement("th", [ HtmlNode.NewText "Column 1" ])
                                                             HtmlNode.NewElement("th", [ HtmlNode.NewText "Column 2" ]) ])
                                       HtmlNode.NewElement("tr",
                                                           [ HtmlNode.NewElement("td", [ HtmlNode.NewText "1" ])
                                                             HtmlNode.NewElement("td", [ HtmlNode.NewText "yes" ]) ]) ]) ]) ]) ]
        snd result |> Should.equal expected

