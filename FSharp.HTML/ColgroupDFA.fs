module FSharp.HTML.ColgroupDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.ColgroupTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</colgroup>",7u;"<col/>",2u;"<colgroup>",1u;"CDATA",7u;"COMMENT",7u;"DOCTYPE",7u;"EOF",7u;"TAGEND",7u;"TAGSELFCLOSING",7u;"TAGSTART",7u;"TEXT",7u|];1u,[|"<col/>",5u;"COMMENT",3u|];2u,[|"</colgroup>",6u;"<col/>",5u;"COMMENT",4u|];3u,[|"<col/>",5u;"COMMENT",3u|];4u,[|"</colgroup>",6u;"<col/>",5u;"COMMENT",4u|];7u,[|"<col/>",8u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|5u|],[|1u;2u;3u;4u|],"lexbuf";[|6u|],[||],"lexbuf";[|8u|],[|1u;2u;7u|],"lexbuf @ [ TagStart(\"colgroup\",[]) ]";[|2u|],[||],"lexbuf @ [ TagEnd \"colgroup\" ]";[|1u;7u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.ColgroupTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|5u|],[|1u;2u;3u;4u|],fun (lexbuf:token list) ->
        lexbuf
    [|6u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|8u|],[|1u;2u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [ TagStart("colgroup",[]) ]
    [|2u|],[||],fun (lexbuf:token list) ->
        lexbuf @ [ TagEnd "colgroup" ]
    [|1u;7u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)