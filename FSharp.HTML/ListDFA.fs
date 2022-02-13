module FSharp.HTML.ListDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.ListTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</li>",1u;"</menu>",4u;"</ol>",4u;"</script>",1u;"</template>",1u;"</ul>",4u;"<li/>",1u;"<li>",4u;"<menu>",1u;"<ol>",1u;"<script/>",1u;"<template/>",1u;"<ul>",1u;"CDATA",4u;"COMMENT",4u;"EOF",4u;"TAGEND",4u;"TAGSELFCLOSING",4u;"TAGSTART",4u;"TEXT",4u;"WS",4u|];1u,[|"</menu>",3u;"</ol>",3u;"</ul>",3u;"<li/>",3u;"<li>",3u;"COMMENT",2u;"WS",2u|];2u,[|"</menu>",3u;"</ol>",3u;"</ul>",3u;"<li/>",3u;"<li>",3u;"COMMENT",2u;"WS",2u|];4u,[|"</menu>",5u;"</ol>",5u;"</ul>",5u;"<li/>",5u;"<li>",5u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|5u|],[|1u;4u|],"lexbuf @ [ TagEnd \"li\" ]";[|1u;4u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.ListTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|5u|],[|1u;4u|],fun (lexbuf:token list) ->
        lexbuf @ [ TagEnd "li" ]
    [|1u;4u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)