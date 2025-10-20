namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal
open FSharp.xUnit

type HtmlCompilerTest(output: ITestOutputHelper) =

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
        let nodes = HtmlCompiler.compile2 x
        output.WriteLine(stringify nodes)
        let e =
            [
                HtmlText ""
                HtmlDoctype "html"
                HtmlText ""
                HtmlElement(
                    "html",
                    [],
                    [
                        HtmlText ""
                        HtmlElement(
                            "head",
                            [],
                            [
                                HtmlText ""
                                HtmlElement("meta", [ "charset", "utf-8" ], [])
                                HtmlText ""
                                HtmlElement(
                                    "title",
                                    [],
                                    [ HtmlText "My test page" ]
                                )
                                HtmlText ""
                            ]
                        )
                        HtmlText ""
                        HtmlElement(
                            "body",
                            [],
                            [
                                HtmlText ""
                                HtmlElement(
                                    "img",
                                    [
                                        "src", "images/firefox-icon.png"
                                        "alt", "My test image"
                                    ],
                                    []
                                )
                                HtmlText ""
                            ]
                        )
                        HtmlText ""
                    ]
                )
                HtmlText ""
            ]

        Should.equal e nodes
