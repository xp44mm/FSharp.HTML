module FSharp.HTML.SemiNodeDFA
let header = "// Add a virtual semicolon to avoid conflicting.\r\nopen System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"CDATA",1u;"COMMENT",1u;"EOF",3u;"TAGEND",1u;"TAGSELFCLOSING",1u;"TAGSTART",4u;"TEXT",1u|];1u,[|"CDATA",2u;"COMMENT",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|2u|],[|1u|],"[lexbuf.Head;SEMICOLON]";[|3u|],[||],"[]";[|1u;4u|],[||],"lexbuf"|]
// Add a virtual semicolon to avoid conflicting.
open System
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|2u|],[|1u|],fun (lexbuf:token list) ->
        [lexbuf.Head;SEMICOLON]
    [|3u|],[||],fun (lexbuf:token list) ->
        []
    [|1u;4u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)