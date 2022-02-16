module FSharp.HTML.TbodyDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.TbodyTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</caption>",1u;"</colgroup>",1u;"<caption/>",1u;"<colgroup/>",1u;"<table>",1u;"<tr/>",4u;"<tr>",4u;"CDATA",4u;"COMMENT",4u;"EOF",4u;"TAGEND",4u;"TAGSELFCLOSING",4u;"TAGSTART",4u;"TEXT",4u;"WS",4u|];1u,[|"<tr/>",3u;"<tr>",3u;"COMMENT",2u;"WS",2u|];2u,[|"<tr/>",3u;"<tr>",3u;"COMMENT",2u;"WS",2u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf @ [TagStart(\"tbody\",[])]";[|1u;4u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.TbodyTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf @ [TagStart("tbody",[])]
    [|1u;4u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)