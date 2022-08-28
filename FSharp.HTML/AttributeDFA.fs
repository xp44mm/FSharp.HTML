module FSharp.HTML.AttributeDFA
let nextStates = [0u,["N",3u;"V",1u];1u,["N",2u]]
open FslexFsyacc.Runtime
open System
open FSharp.HTML
type token = string
let getTag (tok:token) = if tok.[0] = '=' then "V" else "N"
let rules:list<uint32 list*uint32 list*_> = [
    [2u],[],fun(lexbuf:token list)->
        lexbuf.[1], lexbuf.[0].Substring(1)
    [3u],[],fun(lexbuf:token list)->
        lexbuf.[0], ""
]
let analyzer = AnalyzerL(nextStates, rules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)