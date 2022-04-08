module FSharp.HTML.TheadDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.TheadTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</caption>",1u;"</colgroup>",1u;"</script>",1u;"</table>",4u;"</tbody>",1u;"</template>",1u;"</tfoot>",1u;"</thead>",1u;"</tr>",1u;"<caption/>",1u;"<colgroup/>",1u;"<script/>",1u;"<table>",1u;"<tbody/>",1u;"<tbody>",4u;"<template/>",1u;"<tfoot/>",1u;"<tfoot>",4u;"<thead/>",1u;"<thead>",4u;"<tr/>",1u;"CDATA",4u;"COMMENT",4u;"EOF",4u;"TAGEND",4u;"TAGSELFCLOSING",4u;"TAGSTART",4u;"TEXT",4u;"WS",4u|];1u,[|"</table>",3u;"<tbody/>",3u;"<tbody>",3u;"<tfoot/>",3u;"<tfoot>",3u;"<thead/>",3u;"<thead>",3u;"COMMENT",2u;"WS",2u|];2u,[|"</table>",3u;"<tbody/>",3u;"<tbody>",3u;"<tfoot/>",3u;"<tfoot>",3u;"<thead/>",3u;"<thead>",3u;"COMMENT",2u;"WS",2u|];4u,[|"</table>",5u;"<tbody/>",5u;"<tbody>",5u;"<tfoot/>",5u;"<tfoot>",5u;"<thead/>",5u;"<thead>",5u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|5u|],[|1u;4u|],"lexbuf @ [TagEnd \"thead|tbody|tfoot|tr\"]";[|1u;4u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.TheadTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|5u|],[|1u;4u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "thead|tbody|tfoot|tr"]
    [|1u;4u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)