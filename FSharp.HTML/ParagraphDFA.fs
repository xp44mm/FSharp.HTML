module FSharp.HTML.ParagraphDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.ParagraphTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</p>",5u;"<p/>",5u;"<p>",1u;"BLOCKTAG",5u;"CDATA",5u;"COMMENT",5u;"DOCTYPE",5u;"EOF",5u;"TAGEND",5u;"TAGSELFCLOSING",5u;"TAGSTART",5u;"TEXT",5u|];1u,[|"</p>",3u;"<p/>",4u;"<p>",4u;"BLOCKTAG",4u;"COMMENT",2u;"EOF",4u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|];2u,[|"</p>",3u;"<p/>",4u;"<p>",4u;"BLOCKTAG",4u;"COMMENT",2u;"EOF",4u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[||],"lexbuf";[|4u|],[|1u;2u|],"lexbuf @ [TagEnd \"p\"]";[|1u;5u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.ParagraphTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|4u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "p"]
    [|1u;5u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)