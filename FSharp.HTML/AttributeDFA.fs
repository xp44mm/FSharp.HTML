module FSharp.HTML.AttributeDFA
let header = "open System\r\nopen FSharp.HTML\r\ntype token = string\r\nlet getTag (tok:token) = if tok.[0] = '=' then \"V\" else \"N\""
let nextStates = [|0u,[|"N",3u;"V",1u|];1u,[|"N",2u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|2u|],[||],"lexbuf.[1], lexbuf.[0].Substring(1)";[|3u|],[||],"lexbuf.[0], \"\""|]
open System
open FSharp.HTML
type token = string
let getTag (tok:token) = if tok.[0] = '=' then "V" else "N"
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|2u|],[||],fun (lexbuf:token list) ->
        lexbuf.[1], lexbuf.[0].Substring(1)
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf.[0], ""
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)