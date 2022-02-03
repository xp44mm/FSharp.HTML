module FSharp.HTML.RubyDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.RubyTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</rp>",9u;"</rt>",9u;"</ruby>",9u;"<rp/>",9u;"<rp>",5u;"<rt/>",9u;"<rt>",1u;"<ruby/>",9u;"<ruby>",9u;"CDATA",9u;"COMMENT",9u;"DOCTYPE",9u;"EOF",9u;"TAGEND",9u;"TAGSELFCLOSING",9u;"TAGSTART",9u;"TEXT",9u|];1u,[|"</rt>",3u;"</ruby>",4u;"<rp/>",4u;"<rp>",4u;"<rt/>",4u;"<rt>",4u;"<ruby/>",4u;"<ruby>",4u;"COMMENT",2u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|];2u,[|"</rt>",3u;"</ruby>",4u;"<rp/>",4u;"<rp>",4u;"<rt/>",4u;"<rt>",4u;"<ruby/>",4u;"<ruby>",4u;"COMMENT",2u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|];5u,[|"</rp>",7u;"</ruby>",8u;"<rp/>",8u;"<rp>",8u;"<rt/>",8u;"<rt>",8u;"<ruby/>",8u;"<ruby>",8u;"COMMENT",6u;"TAGEND",6u;"TAGSELFCLOSING",6u;"TAGSTART",6u;"TEXT",6u|];6u,[|"</rp>",7u;"</ruby>",8u;"<rp/>",8u;"<rp>",8u;"<rt/>",8u;"<rt>",8u;"<ruby/>",8u;"<ruby>",8u;"COMMENT",6u;"TAGEND",6u;"TAGSELFCLOSING",6u;"TAGSTART",6u;"TEXT",6u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[||],"lexbuf";[|4u|],[|1u;2u|],"lexbuf @ [TagEnd \"rt\"]";[|7u|],[||],"lexbuf";[|8u|],[|5u;6u|],"lexbuf @ [TagEnd \"rp\"]";[|1u;5u;9u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.RubyTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|4u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "rt"]
    [|7u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|8u|],[|5u;6u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "rp"]
    [|1u;5u;9u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)