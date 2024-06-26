﻿namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal
open FSharp.xUnit

type RenderTest(output:ITestOutputHelper) =
    let show res =
        res
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member _.``parseDoc``() =
        let x = """
    <!DOCTYPE html>
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

        let y = 
            x
            |> HtmlUtils.parseDoc
            |> Render.stringifyDoc

        output.WriteLine(y)





