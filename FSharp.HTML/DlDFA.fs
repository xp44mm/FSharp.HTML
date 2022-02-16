module FSharp.HTML.DlDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.DlTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</dd>",1u;"</div>",4u;"</dl>",4u;"</dt>",1u;"</script>",1u;"</template>",1u;"<dd/>",1u;"<dd>",5u;"<div>",1u;"<dl>",1u;"<dt/>",1u;"<dt>",5u;"<script/>",1u;"<template/>",1u;"CDATA",4u;"COMMENT",4u;"EOF",4u;"TAGEND",4u;"TAGSELFCLOSING",4u;"TAGSTART",4u;"TEXT",4u;"WS",4u|];1u,[|"<dd/>",3u;"<dd>",3u;"<dt/>",3u;"<dt>",3u;"COMMENT",2u;"WS",2u|];2u,[|"<dd/>",3u;"<dd>",3u;"<dt/>",3u;"<dt>",3u;"COMMENT",2u;"WS",2u|];4u,[|"<dd/>",6u;"<dd>",6u;"<dt/>",6u;"<dt>",6u|];5u,[|"</div>",8u;"</dl>",8u;"<dd/>",6u;"<dd>",6u;"<dt/>",6u;"<dt>",6u;"CDATA",7u;"COMMENT",7u;"TAGEND",7u;"TAGSELFCLOSING",7u;"TAGSTART",7u;"TEXT",7u|];7u,[|"</div>",8u;"</dl>",8u;"CDATA",7u;"COMMENT",7u;"TAGEND",7u;"TAGSELFCLOSING",7u;"TAGSTART",7u;"TEXT",7u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[|1u;2u|],"lexbuf";[|6u|],[|1u;4u;5u|],"lexbuf @ [TagEnd \"dt|dd\"]";[|8u|],[|5u;7u|],"lexbuf @ [TagEnd \"dt|dd\"]";[|1u;4u;5u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.DlTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf
    [|6u|],[|1u;4u;5u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "dt|dd"]
    [|8u|],[|5u;7u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "dt|dd"]
    [|1u;4u;5u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)