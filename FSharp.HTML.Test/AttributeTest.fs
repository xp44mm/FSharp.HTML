namespace FSharp.HTML

open Xunit

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.LexYacc

type AttributeTest(output: ITestOutputHelper) =

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    member this.``parse basic tags``(i: int) =
        let cases = [
            "<div>", TAGSTART("div", [])
            "<br/>", TAGSELFCLOSING("br", [])
            "<span id=\"\">", TAGSTART("span", [ "id", "" ])
        ]
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = AttributeCompiler.compile iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``parse tags with attributes``(i: int) =
        let cases = [
            "<div id=\"test\">", TAGSTART("div", [ "id", "test" ])
            "<span class=\"container\" data-value=\"123\">", TAGSTART("span", [ "class", "container"; "data-value", "123" ])
            "<input type=\"text\" disabled>", TAGSELFCLOSING("input", [ "type", "text"; "disabled", "" ])
            "<img src=\"image.jpg\"/>", TAGSELFCLOSING("img", [ "src", "image.jpg" ])
        ]
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = AttributeCompiler.compile iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    member this.``parse self closing tags``(i: int) =
        let cases = [
            "<br/>", TAGSELFCLOSING("br", [])
            "<input type=\"submit\" value=\"OK\"/>", TAGSELFCLOSING("input", [ "type", "submit"; "value", "OK" ])
            "<meta name=\"viewport\" content=\"width=device-width\"/>", TAGSELFCLOSING("meta", [ "name", "viewport"; "content", "width=device-width" ])
        ]
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = AttributeCompiler.compile iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    member this.``parse tags with empty and single quoted values``(i: int) =
        let cases = [
            "<div id=\"\">", TAGSTART("div", [ "id", "" ])
            "<span data-empty=\"\">", TAGSTART("span", [ "data-empty", "" ])
            "<div id='test'>", TAGSTART("div", [ "id", "test" ])
        ]
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = AttributeCompiler.compile iter
        output.WriteLine(stringify y)
        Should.equal e y

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    member this.``parse various html tags``(i: int) =
        let cases = [
            "<meta charset=\"utf-8\">", TAGSELFCLOSING("meta", [ "charset", "utf-8" ])
            "<link rel=\"stylesheet\" href=\"style.css\"/>", TAGSELFCLOSING("link", [ "rel", "stylesheet"; "href", "style.css" ])
            "<input type=\"checkbox\" checked>", TAGSELFCLOSING("input", [ "type", "checkbox"; "checked", "" ])
            "<img src=\"test.jpg\" alt=\"test image\"/>", TAGSELFCLOSING("img", [ "src", "test.jpg"; "alt", "test image" ])
            "<span>", TAGSTART("span", [])
        ]
        let x, e = cases.[i]
        let iter = LexicalIterator.forChar x
        let y = AttributeCompiler.compile iter
        output.WriteLine(stringify y)
        Should.equal e y
