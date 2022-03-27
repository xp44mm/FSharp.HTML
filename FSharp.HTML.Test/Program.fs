module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

let x = [
    "<!-- HTM_Numeric_Character_References_Example.html\r\n    - Copyright (c) HerongYang.com. All Rights Reserved.\r\n    -->";
    "<html>
    <head>
    <!-- Numeric Character References in \"title\" element --><title>\"Character Entity References\" Example</title></head><body>
    <!-- Numeric Character References in element content --><pre>\r\n    U+0003C \" \" \" \"\r\n    U+000A9 © ©\r\n    
    </pre><!-- Numeric Character References in attribute value --><img src=\"image.gif\" alt=\"In attribute value: ©\"/>
    <!-- Bad examples --><script>U+000A9 &#169; &#xA9;</script></body></html>"]




let [<EntryPoint>] main _ = 0
