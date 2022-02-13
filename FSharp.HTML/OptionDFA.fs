module FSharp.HTML.OptionDFA
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.OptionTokenUtils\r\ntype token = HtmlToken"
let nextStates = [|0u,[|"</datalist>",5u;"</optgroup>",5u;"</option>",5u;"</select>",5u;"<optgroup/>",5u;"<optgroup>",5u;"<option/>",5u;"<option>",1u;"CDATA",5u;"COMMENT",5u;"EOF",5u;"TAGEND",5u;"TAGSELFCLOSING",5u;"TAGSTART",5u;"TEXT",5u|];1u,[|"</datalist>",4u;"</optgroup>",4u;"</option>",3u;"</select>",4u;"<optgroup/>",4u;"<optgroup>",4u;"<option/>",4u;"<option>",4u;"TEXT",2u|];2u,[|"</datalist>",4u;"</optgroup>",4u;"</option>",3u;"</select>",4u;"<optgroup/>",4u;"<optgroup>",4u;"<option/>",4u;"<option>",4u;"TEXT",2u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|3u|],[||],"lexbuf";[|4u|],[|1u;2u|],"lexbuf @ [TagEnd \"option\"]";[|1u;5u|],[||],"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.OptionTokenUtils
type token = HtmlToken
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf
    [|4u|],[|1u;2u|],fun (lexbuf:token list) ->
        lexbuf @ [TagEnd "option"]
    [|1u;5u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)