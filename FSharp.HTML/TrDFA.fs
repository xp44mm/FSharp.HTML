module FSharp.HTML.TrDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.TrTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</caption>",1u;"</colgroup>",1u;"</table>",16u;"</tbody>",1u;"</tfoot>",1u;"</thead>",1u;"</tr>",5u;"<caption/>",1u;"<colgroup/>",1u;"<table>",1u;"<tbody/>",1u;"<tbody>",3u;"<tfoot/>",1u;"<tfoot>",4u;"<thead/>",1u;"<thead>",2u;"<tr/>",5u;"<tr>",16u;"CDATA",16u;"COMMENT",16u;"EOF",16u;"TAGEND",16u;"TAGSELFCLOSING",16u;"TAGSTART",16u;"TEXT",16u;"WS",16u|];1u,[|"</table>",15u;"</tbody>",17u;"</tfoot>",17u;"</thead>",17u;"<tr/>",11u;"<tr>",11u;"COMMENT",10u;"WS",10u|];2u,[|"</table>",17u;"</tbody>",17u;"</tfoot>",17u;"</thead>",12u;"<tr/>",11u;"<tr>",11u;"COMMENT",6u;"WS",6u|];3u,[|"</table>",17u;"</tbody>",13u;"</tfoot>",17u;"</thead>",17u;"<tr/>",11u;"<tr>",11u;"COMMENT",8u;"WS",8u|];4u,[|"</table>",17u;"</tbody>",17u;"</tfoot>",14u;"</thead>",17u;"<tr/>",11u;"<tr>",11u;"COMMENT",9u;"WS",9u|];5u,[|"</table>",15u;"</tbody>",13u;"</tfoot>",14u;"</thead>",12u;"<tr/>",11u;"<tr>",11u;"COMMENT",7u;"WS",7u|];6u,[|"</thead>",12u;"<tr/>",11u;"<tr>",11u;"COMMENT",6u;"WS",6u|];7u,[|"</table>",15u;"</tbody>",13u;"</tfoot>",14u;"</thead>",12u;"<tr/>",11u;"<tr>",11u;"COMMENT",7u;"WS",7u|];8u,[|"</tbody>",13u;"<tr/>",11u;"<tr>",11u;"COMMENT",8u;"WS",8u|];9u,[|"</tfoot>",14u;"<tr/>",11u;"<tr>",11u;"COMMENT",9u;"WS",9u|];10u,[|"</table>",15u;"<tr/>",11u;"<tr>",11u;"COMMENT",10u;"WS",10u|];16u,[|"</table>",17u;"</tbody>",17u;"</tfoot>",17u;"</thead>",17u;"<tr/>",17u;"<tr>",17u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|11u|],[|1u;2u;3u;4u;5u;6u;7u;8u;9u;10u|],"lexbuf";[|12u|],[|2u;5u;6u;7u|],"lexbuf";[|13u|],[|3u;5u;7u;8u|],"lexbuf";[|14u|],[|4u;5u;7u;9u|],"lexbuf";[|15u|],[|1u;5u;7u;10u|],"lexbuf";[|17u|],[|1u;2u;3u;4u;5u;16u|],"lexbuf @ [TagEnd \"tr\"]";[|1u;2u;3u;4u;5u;16u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.TrTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|11u|],[|1u;2u;3u;4u;5u;6u;7u;8u;9u;10u|],fun (lexbuf:token list) ->
        lexbuf
    [|12u|],[|2u;5u;6u;7u|],fun (lexbuf:token list) ->
        lexbuf
    [|13u|],[|3u;5u;7u;8u|],fun (lexbuf:token list) ->
        lexbuf
    [|14u|],[|4u;5u;7u;9u|],fun (lexbuf:token list) ->
        lexbuf
    [|15u|],[|1u;5u;7u;10u|],fun (lexbuf:token list) ->
        lexbuf
    [|17u|],[|1u;2u;3u;4u;5u;16u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "tr"]
    [|1u;2u;3u;4u;5u;16u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)