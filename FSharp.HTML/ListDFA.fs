module FSharp.HTML.ListDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.ListTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</li>",1u;"</menu>",7u;"</ol>",7u;"</script>",1u;"</template>",1u;"</ul>",7u;"<li/>",1u;"<li>",7u;"<menu>",1u;"<ol>",1u;"<script/>",1u;"<template/>",1u;"<ul>",1u;"CDATA",7u;"COMMENT",7u;"DOCTYPE",7u;"EOF",7u;"TAGEND",7u;"TAGSELFCLOSING",7u;"TAGSTART",7u;"TEXT",7u|];1u,[|"</menu>",6u;"</ol>",4u;"</ul>",5u;"<li/>",3u;"<li>",3u;"COMMENT",2u|];2u,[|"</menu>",6u;"</ol>",4u;"</ul>",5u;"<li/>",3u;"<li>",3u;"COMMENT",2u|];7u,[|"</menu>",11u;"</ol>",9u;"</ul>",10u;"<li/>",8u;"<li>",8u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|4u|],[|1u;2u|],"lexbuf";[|5u|],[|1u;2u|],"lexbuf";[|6u|],[|1u;2u|],"lexbuf";[|8u|],[|1u;7u|],"lexbuf @ [TagEnd \"li\"]";[|9u|],[|1u;7u|],"lexbuf @ [TagEnd \"li\"]";[|10u|],[|1u;7u|],"lexbuf @ [TagEnd \"li\"]";[|11u|],[|1u;7u|],"lexbuf @ [TagEnd \"li\"]";[|1u;7u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.ListTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|4u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|5u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|6u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|8u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "li"]
    [|9u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "li"]
    [|10u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "li"]
    [|11u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "li"]
    [|1u;7u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)