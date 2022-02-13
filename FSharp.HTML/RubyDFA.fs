module FSharp.HTML.RubyDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.RubyTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</ruby>",9u;"<rp/>",9u;"<rp>",4u;"<rt/>",9u;"<rt>",1u;"<ruby/>",9u;"<ruby>",9u;"CDATA",9u;"COMMENT",9u;"EOF",9u;"TAGEND",9u;"TAGSELFCLOSING",9u;"TAGSTART",9u;"TEXT",9u|];1u,[|"</rt>",3u;"</ruby>",7u;"<rp/>",7u;"<rp>",7u;"<rt/>",7u;"<rt>",7u;"<ruby/>",7u;"<ruby>",7u;"CDATA",2u;"COMMENT",2u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|];2u,[|"</rt>",3u;"</ruby>",7u;"<rp/>",7u;"<rp>",7u;"<rt/>",7u;"<rt>",7u;"<ruby/>",7u;"<ruby>",7u;"CDATA",2u;"COMMENT",2u;"TAGEND",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|];4u,[|"</rp>",6u;"</ruby>",8u;"<rp/>",8u;"<rp>",8u;"<rt/>",8u;"<rt>",8u;"<ruby/>",8u;"<ruby>",8u;"CDATA",5u;"COMMENT",5u;"TAGEND",5u;"TAGSELFCLOSING",5u;"TAGSTART",5u;"TEXT",5u|];5u,[|"</rp>",6u;"</ruby>",8u;"<rp/>",8u;"<rp>",8u;"<rt/>",8u;"<rt>",8u;"<ruby/>",8u;"<ruby>",8u;"CDATA",5u;"COMMENT",5u;"TAGEND",5u;"TAGSELFCLOSING",5u;"TAGSTART",5u;"TEXT",5u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[||],"lexbuf";[|6u|],[||],"lexbuf";[|7u|],[|1u;2u|],"lexbuf @ [TagEnd \"rt\"]";[|8u|],[|4u;5u|],"lexbuf @ [TagEnd \"rp\"]";[|1u;4u;9u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.RubyTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|6u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|7u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "rt"]
    [|8u|],[|4u;5u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "rp"]
    [|1u;4u;9u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)