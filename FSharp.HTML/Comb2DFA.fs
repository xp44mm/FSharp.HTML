module FSharp.HTML.Comb2DFA
let nextStates = [0u,["x",1u;"y",3u;"z",4u];1u,["y",2u]]
open FslexFsyacc.Runtime
type token = string
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [1u;2u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [3u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [4u],[],fun (lexbuf:list<_>) ->
        []
]
let analyzer = Analyzer<_,_>(nextStates, rules)