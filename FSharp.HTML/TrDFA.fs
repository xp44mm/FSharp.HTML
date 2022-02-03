module FSharp.HTML.TrDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.TrTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</tbody>",8u;"</tfoot>",8u;"</thead>",8u;"</thead|tbody|tfoot>",8u;"</tr>",1u;"<tbody>",1u;"<tfoot>",1u;"<thead>",1u;"<tr/>",1u;"<tr>",8u;"CDATA",8u;"COMMENT",8u;"DOCTYPE",8u;"EOF",8u;"TAGEND",8u;"TAGSELFCLOSING",8u;"TAGSTART",8u;"TEXT",8u|];1u,[|"</tbody>",5u;"</tfoot>",6u;"</thead>",4u;"</thead|tbody|tfoot>",7u;"<tr/>",3u;"<tr>",3u;"COMMENT",2u|];2u,[|"</tbody>",5u;"</tfoot>",6u;"</thead>",4u;"</thead|tbody|tfoot>",7u;"<tr/>",3u;"<tr>",3u;"COMMENT",2u|];8u,[|"</tbody>",9u;"</tfoot>",9u;"</thead>",9u;"</thead|tbody|tfoot>",9u;"<tr/>",9u;"<tr>",9u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|4u|],[|1u;2u|],"lexbuf";[|5u|],[|1u;2u|],"lexbuf";[|6u|],[|1u;2u|],"lexbuf";[|7u|],[|1u;2u|],"lexbuf";[|9u|],[|1u;8u|],"lexbuf @ [TagEnd \"tr\"]";[|1u;8u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.TrTokenUtils
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
    [|7u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|9u|],[|1u;8u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "tr"]
    [|1u;8u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)