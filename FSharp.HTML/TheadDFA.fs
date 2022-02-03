module FSharp.HTML.TheadDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.TheadTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</caption>",1u;"</colgroup>",1u;"</table>",7u;"</tbody>",1u;"</tfoot>",1u;"</thead>",1u;"<caption/>",1u;"<colgroup/>",1u;"<table>",1u;"<tbody/>",1u;"<tbody>",7u;"<tfoot/>",1u;"<tfoot>",7u;"<thead/>",1u;"<thead>",7u;"CDATA",7u;"COMMENT",7u;"DOCTYPE",7u;"EOF",7u;"TAGEND",7u;"TAGSELFCLOSING",7u;"TAGSTART",7u;"TEXT",7u|];1u,[|"</table>",6u;"<tbody/>",4u;"<tbody>",4u;"<tfoot/>",5u;"<tfoot>",5u;"<thead/>",3u;"<thead>",3u;"COMMENT",2u|];2u,[|"</table>",6u;"<tbody/>",4u;"<tbody>",4u;"<tfoot/>",5u;"<tfoot>",5u;"<thead/>",3u;"<thead>",3u;"COMMENT",2u|];7u,[|"</table>",11u;"<tbody/>",9u;"<tbody>",9u;"<tfoot/>",10u;"<tfoot>",10u;"<thead/>",8u;"<thead>",8u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|4u|],[|1u;2u|],"lexbuf";[|5u|],[|1u;2u|],"lexbuf";[|6u|],[|1u;2u|],"lexbuf";[|8u|],[|1u;7u|],"lexbuf @ [TagEnd \"thead|tbody|tfoot\"]";[|9u|],[|1u;7u|],"lexbuf @ [TagEnd \"thead|tbody|tfoot\"]";[|10u|],[|1u;7u|],"lexbuf @ [TagEnd \"thead|tbody|tfoot\"]";[|11u|],[|1u;7u|],"lexbuf @ [TagEnd \"thead|tbody|tfoot\"]";[|1u;7u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.TheadTokenUtils
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
        lexbuf @ [TagEnd "thead|tbody|tfoot"]
    [|9u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "thead|tbody|tfoot"]
    [|10u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "thead|tbody|tfoot"]
    [|11u|],[|1u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "thead|tbody|tfoot"]
    [|1u;7u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)