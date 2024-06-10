module FSharp.HTML.Comb3plus1DFA
let nextStates = [0u,["j",1u;"k",4u;"l",6u;"m",7u;"n",8u];1u,["k",2u;"l",3u];2u,["l",3u];4u,["l",5u]]
open FslexFsyacc
type token = string
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [1u;2u;3u],[],fun (lexbuf: list<_>) ->
        lexbuf
    [4u;5u],[],fun (lexbuf: list<_>) ->
        lexbuf
    [6u],[],fun (lexbuf: list<_>) ->
        lexbuf
    [7u],[],fun (lexbuf: list<_>) ->
        lexbuf
    [8u],[],fun (lexbuf: list<_>) ->
        []
]
let analyzer = Analyzer<_,_>(nextStates, rules)