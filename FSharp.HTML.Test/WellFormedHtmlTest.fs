namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type WellFormedHtmlTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let readAllText path =
        let path = Path.Combine(__SOURCE_DIRECTORY__, path)
        let text = File.ReadAllText(path)
        text

    let evade txt =
        txt
        |> Tokenizer.tokenize
        |> Seq.choose (HtmlTokenUtils.unifyVoidElement)
        // 临时措施
        |> Seq.filter(function Text x when String.IsNullOrWhiteSpace x -> false | _ -> true)

        |> ListDFA.analyze
        |> Seq.concat

        |> ParagraphDFA.analyze
        |> Seq.concat


    let parse txt =
        txt
        |> evade

        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse


    let attrubutes node =
        match node with
        | HtmlElement(_,ls,_) -> ls
        | x -> failwith(Literal.stringify x)

    [<Theory>]
    [<InlineData(@"<a href=http://test.com/index>Test</a>")>]
    [<InlineData(@"<a href = http://test.com/index>Test</a>")>]
    [<InlineData(@"<a href =http://test.com/index>Test</a>")>]
    [<InlineData(@"<a href= http://test.com/index>Test</a>")>]
    member _.``Can handle slashes in unquoted attributes`` content =
        let result = parse content
        let expected =
            //HtmlDocument.New
                [ HtmlNode.NewElement("a",
                    [ "href", @"http://test.com/index" ],
                    [ HtmlNode.NewText "Test" ]) ]
        snd result |> Should.equal expected

    [<Fact>]
    member _.``Can handle char refs in unquoted attributes``() =

        let result = parse "<a alt=&lt;>Test</a>"
        show result

    [<Fact>]
    member _.``Can handle multiple unquoted attributes``() =
        let result = parse "<a src = target alt = logo>Test</a>"
        show result
    [<Fact>]
    member _.``Can handle multiple char refs in a text run``() =
        let html =  "<div>&quot;Foo&quot;</div>"
        let result = parse html
        show result
    [<Fact>]
    member _.``Can handle attributes with no value``() =
        let html = """<li itemscope itemtype="http://schema.org/Place"></li>"""
        let node = parse html |> snd |> List.head
        let expected =
            [
                HtmlAttribute("itemscope", "")
                HtmlAttribute("itemtype", "http://schema.org/Place")
            ]
        let result =
            match node with
            | HtmlElement(_,ls,_) -> ls
            | x -> failwith(Literal.stringify x)
        result |> Should.equal expected

    [<Fact>]
    member _.``Can handle attributes next to each other``() =
        let html = """<h1 class="foo"style="font-size: 0.7em">Test</h1>"""
        let node = parse html |> snd |> List.head
        let expected =
            [
                HtmlAttribute("class", "foo")
                HtmlAttribute("style", "font-size: 0.7em")
            ]
        node.Attributes() |> Should.equal expected

    [<Fact>]
    member _.``Can handle long html encoded attributes without StackOverflow``() =
        let html =
            parse (
            [
                "<html><body><p attrib=\""
                for i in 0 .. 50000 do "&lt;br/&gt;"
                "\">Test</p></body></html>"
            ] |> String.concat ""
            ) |> snd
        let result = html.Head
        show result
        //result |> Should.equal "Test"

    [<Theory>]
    [<InlineData("var r = \"</script>\"")>]
    [<InlineData("var r = '</script>'")>]
    [<InlineData("var r = /</g")>]
    [<InlineData("""var r = /\/</g""")>]
    [<InlineData("""var r = /a\/</g""")>]
    [<InlineData("""var r = /\\/g""")>]
    [<InlineData("//</script>\n")>]
    [<InlineData("/*</script>*/")>]
    [<InlineData("/*</script>**/")>]
    [<InlineData("""/*
    </script>
    Test comment
    */""")>]
    [<InlineData("function(sel) {return sel.replace(/([/.])/g, '\\$1');};")>]
    member _.``Can handle special characters in scripts`` content =
        let html = sprintf "<script>%s</script>" content
        let node = parse html |> snd |> List.head
        let expected = HtmlNode.NewElement("script", [ HtmlNode.NewText content ])
        node |> Should.equal expected


    [<Fact>]
    member _.``Can handle html with doctype and xml namespaces``() =
        let html =
            """<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"><html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml"><body>content</body></html>"""
        let htmlDoc = parse html
        show htmlDoc

    [<Fact>]
    member _.``Can handle html without html tag``() =
       let html = """<body>
           <div>no html-tag</div>
           </body>"""

       let htmlDoc = parse html

       show htmlDoc

    [<Fact>]
    member _.``Can parse non-self-closing tags of elements that can't have children when followed by comments``() =
        let html = """<hr class="hr4"><!--comment1--><!--comment2--><hr class="hr5" />"""
        let result = parse html
        show result

    [<Fact>]
    member _.``Ignores spurious closing tags``() =
        let html =
            """<li class="even">
            <a class="clr" href="/pj/ldbdetails/kEW6eAApVxWdogIXhoHAew%3D%3D/?board=dep">
            <span class="time em">21:36<br /><small>On time</small></span>
            </span><span class="station">Brighton (East Sussex)</span>
            <span class="platform"><small>Platform</small><br />17</span></a>
            </li>"""
        let e = Assert.Throws(fun() -> 
            let result = parse html
            show result
        )
        
        show e
    [<Fact>]
    member _.``Renders textarea closing tag``() =
        let html = """<textarea cols="40" rows="2"></textarea>"""
        let result = parse html
        show result



    [<Fact>]
    member _.``Can handle CDATA blocks``() =
        let cData = """
          Trying to provoke the CDATA parser with almost complete CDATA end tags
          ]
          >
          ]]
          ]>
          All done!
    """

        let html = """
        <!DOCTYPE html>
        <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en" xmlns:fb="http://www.facebook.com/2008/fbml" xmlns:og="http://opengraphprotocol.org/schema/">
         <head>
            <script type="text/javascript">
                var google_tag_params = { PROP_intent: "RENT", PROP_use: "RES", PROP_loc: "London", PROP_minprice: "1500", PROP_maxprice: "1750", PROP_beds: "1" };
            </script>

            <p>
             <![CDATA[""" + cData + """]]>
            </p>
         </head>
         <body>
             <ul>
                 <li>1</li>
                 <li>2</li>
             </ul>
         </body>
        </html>
        """

        let doc = parse html
        show doc

    [<Fact>]
    member _.``Can handle large CDATA blocks``() =
        let bigString : string = new System.String ('a', 100000)
        let html = """
        <!DOCTYPE html>
        <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en" xmlns:fb="http://www.facebook.com/2008/fbml" xmlns:og="http://opengraphprotocol.org/schema/">
         <head>
            <p>
             <![CDATA[""" + bigString + """]]>
            </p>
         </head>
        </html>
        """

        let doc = parse html
        show doc
    [<Fact>]
    member _.``Can parse nested lists correctly when stops on recurse``() =
        let html = """
            <ul>
                <li>
                    <ul>
                        <li>1</li>
                        <li>2</li>
                    </ul>
                </li>
                <li>3</li>
                <li>4</li>
           </ul>
        """

        let y = parse html
        show y
    [<Fact>]
    member _.``Can parse nested lists correctly when continues on recurse``() =
        let html = """
            <ul>
                <li>
                    <ul>
                        <li>1</li>
                        <li>2</li>
                    </ul>
                </li>
                <li>3</li>
                <li>4</li>
           </ul>
        """
        let y = parse html
        show y


    [<Fact>]
    member _.``Can parse nested lists correctly when continues closing tags are missing``() =
        let html = """
            <ul>
                <li>
                    <ul><li>1<li>2</ul>
                <li>3
                <li>4
           </ul>
        """
        let y = parse html
        show y



    [<Fact>]
    member _.``Can parse pre blocks``() =
        let html = "<pre>\r\n        This code should be indented and\r\n        have line feeds in it</pre>"
        let y = parse html
        show y


    [<Fact>]
    member _.``Can parse pre containing code blocks``() =
        let html = "<pre><code>\r\n        let f a b = a * b\r\n        f 5 6 |> Should.equal 30</code></pre>"
        let y = parse html
        show y


    [<Fact>]
    member _.``Can parse pre blocks with char refs``() =
        let html = "<pre>let hello =\r\n    fun who -&gt;\r\n        &quot;hello&quot; + who</pre>"
        let y = parse html
        show y


    [<Fact>]
    member _.``Drops whitespace outside pre``() =
        let html =
            "<div>foo    <pre>    bar    </pre>    baz</div>"

        let y = parse html
        show y


    [<Fact>]
    member _.``Doesn't insert whitespace on attribute name when there are two whitespace characters before an attribute``() =
        let x =
            "<a data-lecture-id=\"27\"\r\ndata-modal-iframe=\"https://class.coursera.org/mathematicalmethods-001/lecture/view?lecture_id=27\"></a>"
        let y = parse x
        show y

    [<Fact>]
    member _.``Includes DOCTYPE when transforming HtmlDocument to string``() =
        let html = """<!DOCTYPE html><html lang="en"><head><title>Test</title></head><body>I Just Love F#</body></html>"""
        let doc = parse html
        show doc

    [<Theory>]
    [<InlineData("""var ns="xmlns=\"http:\/\/test.com\"";""")>]
    [<InlineData("""var ns='xmlns=\'http:\/\/test.com\'';""")>]
    member _.``Can handle escaped characters in a string inside script tag`` content =
        let result = parse (sprintf "<script>%s</script>" content)
        show result
    [<Fact>]
    member _.``Parsing non-html content doesn't cause an infinite loop - Github-1264``() =
        let content =
          """Steve Jobs steve@apple.com Education: - Master of Mathematics Honours Computer Science and Combinatorics &
              Optimization. I
              specialized in systems and real-time programming, programming language
              implementation, and mathematical optimization.
          Skills:
            - Proficient in Rust, C++, Scheme, x86(_64) LaTeX,
              (Postgre)SQL, Gurobi, AWS, Google Cloud Platform, .NET (Core), C#,
              Python, low-level profiling and optimization on Linux and Windows.
            - Can do things with Java, Haskell, Clojure,
              Scala, AMPS, redis, OpenGL.
            Instructional support assistant at the School,
            September to January 2010.
              - Started the Java project[3], a custom IDE for students in an
                introductory computer science course."""

        let result = parse content
        show result
    [<Fact>] 
    member _.``Can handle incomplete tags at end of file without creating an infinite loop``() =
        let e = Assert.Throws(fun()->
            let result = parse """<html><head></head></html"""
            show result        
        ) 
        show e