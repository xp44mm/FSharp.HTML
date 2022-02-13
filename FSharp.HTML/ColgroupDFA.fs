module FSharp.HTML.ColgroupDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.ColgroupTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</colgroup>",6u;"<col/>",2u;"<colgroup>",1u;"CDATA",6u;"COMMENT",6u;"EOF",6u;"TAGEND",6u;"TAGSELFCLOSING",6u;"TAGSTART",6u;"TEXT",6u;"WS",6u|];1u,[|"</colgroup>",5u;"<col/>",4u;"COMMENT",3u;"WS",3u|];2u,[|"</colgroup>",5u;"<col/>",4u;"COMMENT",3u;"WS",3u|];3u,[|"</colgroup>",5u;"<col/>",4u;"COMMENT",3u;"WS",3u|];6u,[|"<col/>",7u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|4u|],[|1u;2u;3u|],"lexbuf";[|5u|],[||],"lexbuf";[|7u|],[|1u;2u;6u|],"lexbuf @ [ TagStart(\"colgroup\",[]) ]";[|2u|],[||],"lexbuf @ [ TagEnd \"colgroup\" ]";[|1u;6u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.ColgroupTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|4u|],[|1u;2u;3u|],fun (lexbuf:token list) ->
        lexbuf
    [|5u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|7u|],[|1u;2u;6u|],fun (lexbuf:token list) ->
        lexbuf @ [ TagStart("colgroup",[]) ]
    [|2u|],[||],fun (lexbuf:token list) ->
        lexbuf @ [ TagEnd "colgroup" ]
    [|1u;6u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)