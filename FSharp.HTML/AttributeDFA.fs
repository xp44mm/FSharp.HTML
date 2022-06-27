module FSharp.HTML.AttributeDFA
let nextStates = [|0u,[|"N",3u;"V",1u|];1u,[|"N",2u|]|]
open FslexFsyacc.Runtime
open System
open FSharp.HTML
type token = string
let getTag (tok:token) = if tok.[0] = '=' then "V" else "N"
let rules:(uint32[]*uint32[]*_)[] = [|
    [|2u|],[||],fun (lexbuf:token list) ->
        lexbuf.[1], lexbuf.[0].Substring(1)
    [|3u|],[||],fun (lexbuf:token list) ->
        lexbuf.[0], ""
|]
let analyzer = Analyzer(nextStates, rules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)