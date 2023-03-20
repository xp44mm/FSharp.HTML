module FSharp.HTML.Comb2plus1DFA
let nextStates = [0u,["k",1u;"l",3u;"m",4u;"n",5u];1u,["l",2u]]
open FslexFsyacc.Runtime
type token = string
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [1u;2u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [3u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [4u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [5u],[],fun (lexbuf:list<_>) ->
        []
]
let analyzer = Analyzer<_,_>(nextStates, rules)