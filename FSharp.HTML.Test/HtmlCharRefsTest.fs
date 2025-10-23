namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal
open FSharp.xUnit

type HtmlCharRefsTest(output:ITestOutputHelper) =
    let decode (s:string) =
            s.ToCharArray()
            |> List.ofArray
            |> CharRefLexerTailer.decode

    [<Fact>]
    member _.``substituteNamedCharacterReference``() =
        let x = "&lt;"
        let y = 
            x.ToCharArray()
            |> List.ofArray
            |> CharRefLexerTailer.decode

        let e = "<" 
        Should.equal e y

    [<Fact>]
    member _.``Named Character Reference``() =
        let x = "&lt;, &gt; and &amp; Character References"
        let y = decode x
        output.WriteLine(stringify y)
        //Should.equal e y

    [<Fact>]
    member _.``Decimal Numeric Character Reference``() =
        let x = "&#60;"

        let y = decode x
            //HtmlCharRefs.tokenizeRawText x
            //|> Seq.toList
        let e = "<"
        Should.equal e y

    [<Fact>]
    member _.``Hexadecimal Numeric Character Reference``() =
        let x = "&#x22;"

        let y =  decode x
            //HtmlCharRefs.tokenizeRawText x
            //|> Seq.toList

        //show y
        let e = "\""

        Should.equal e y 

    [<Fact>]
    member _.``unescapseNode``() =
        let x = """<!DOCTYPE html>
<!-- HTM_Numeric_Character_References_Example.html
    - Copyright (c) HerongYang.com. All Rights Reserved.
-->
<html>
<head>
<!-- Numeric Character References in "title" element -->
<title>&#34;Character Entity References&#x22; Example</title>
</head>
<body>
        
<!-- Numeric Character References in element content -->
<pre>
U+0003C &#34; &#x22; &#034; &#x0022;
U+000A9 &#169; &#xA9;
</pre>
        
<!-- Numeric Character References in attribute value -->
<img src="image.gif" alt="In attribute value: &#xA9;">
        
<!-- Bad examples -->
<script>U+000A9 &#169; &#xA9;</script>
        
</body>
</html>
"""
        let e = [HtmlDoctype "html";HtmlComment " HTM_Numeric_Character_References_Example.html\r\n    - Copyright (c) HerongYang.com. All Rights Reserved.\r\n";HtmlElement("html",[],[HtmlElement("head",[],[HtmlComment " Numeric Character References in \"title\" element ";HtmlElement("title",[],[HtmlText "\"Character Entity References\" Example"])]);HtmlElement("body",[],[HtmlComment " Numeric Character References in element content ";HtmlElement("pre",[],[HtmlText "\r\nU+0003C \" \" \" \"\r\nU+000A9 © ©\r\n"]);HtmlComment " Numeric Character References in attribute value ";HtmlElement("img",["src","image.gif";"alt","In attribute value: ©"],[]);HtmlComment " Bad examples ";HtmlElement("script",[],[HtmlText "U+000A9 © ©"])])])]

        let nodes = 
            x
            |> HtmlCompiler.compileText
            |> Whitespace.trimWhitespace
            |> List.map CharacterReference.processCharRefs
            
        Should.equal e nodes
