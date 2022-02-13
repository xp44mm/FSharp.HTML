module FSharp.HTML.CaptionDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.CaptionTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</table>",5u;"<caption/>",5u;"<caption>",1u;"<colgroup/>",5u;"<colgroup>",5u;"<table/>",5u;"<table>",5u;"<tbody/>",5u;"<tbody>",5u;"<tfoot/>",5u;"<tfoot>",5u;"<thead/>",5u;"<thead>",5u;"<tr/>",5u;"<tr>",5u;"CDATA",5u;"COMMENT",5u;"EOF",5u;"TAGEND",5u;"TAGSELFCLOSING",5u;"TAGSTART",5u;"TEXT",5u|];1u,[|"</caption>",3u;"</table>",4u;"<caption/>",4u;"<caption>",4u;"<colgroup/>",4u;"<colgroup>",4u;"<table/>",4u;"<table>",4u;"<tbody/>",4u;"<tbody>",4u;"<tfoot/>",4u;"<tfoot>",4u;"<thead/>",4u;"<thead>",4u;"<tr/>",4u;"<tr>",4u;"CDATA",2u;"COMMENT",2u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|];2u,[|"</caption>",3u;"</table>",4u;"<caption/>",4u;"<caption>",4u;"<colgroup/>",4u;"<colgroup>",4u;"<table/>",4u;"<table>",4u;"<tbody/>",4u;"<tbody>",4u;"<tfoot/>",4u;"<tfoot>",4u;"<thead/>",4u;"<thead>",4u;"<tr/>",4u;"<tr>",4u;"CDATA",2u;"COMMENT",2u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[||],"lexbuf";[|4u|],[|1u;2u|],"lexbuf @ [TagEnd \"caption\"]";[|1u;5u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.CaptionTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|4u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "caption"]
    [|1u;5u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)