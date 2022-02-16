module FSharp.HTML.TdDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.TdTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</script>",1u;"</td>",1u;"</template>",1u;"</th>",1u;"</tr>",4u;"<script/>",1u;"<td/>",1u;"<td>",4u;"<template/>",1u;"<th/>",1u;"<th>",4u;"<tr>",1u;"CDATA",4u;"COMMENT",4u;"EOF",4u;"TAGEND",4u;"TAGSELFCLOSING",4u;"TAGSTART",4u;"TEXT",4u;"WS",4u|];1u,[|"</tr>",3u;"<td/>",3u;"<td>",3u;"<th/>",3u;"<th>",3u;"COMMENT",2u;"WS",2u|];2u,[|"</tr>",3u;"<td/>",3u;"<td>",3u;"<th/>",3u;"<th>",3u;"COMMENT",2u;"WS",2u|];4u,[|"</tr>",5u;"<td/>",5u;"<td>",5u;"<th/>",5u;"<th>",5u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|5u|],[|1u;4u|],"lexbuf @ [TagEnd \"th|td\"]";[|1u;4u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.TdTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|5u|],[|1u;4u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "th|td"]
    [|1u;4u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)