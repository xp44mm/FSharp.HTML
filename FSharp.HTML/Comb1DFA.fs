module FSharp.HTML.Comb1DFA
let nextStates = [0u,["f",2u;"t",1u]]
open FslexFsyacc.Runtime
type token = string
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [1u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [2u],[],fun (lexbuf:list<_>) ->
        []
]
let analyzer = Analyzer<_,_>(nextStates, rules)