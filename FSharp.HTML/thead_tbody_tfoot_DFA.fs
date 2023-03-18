module FSharp.HTML.thead_tbody_tfoot_DFA
let nextStates = [0u,["*",8u;"caption",7u;"colgroup",7u;"tbody",6u;"td",1u;"tfoot",6u;"th",1u;"thead",6u;"tr",4u];1u,["tbody",3u;"tfoot",3u;"thead",3u;"tr",2u];2u,["tbody",3u;"tfoot",3u;"thead",3u];4u,["tbody",5u;"tfoot",5u;"thead",5u]]
open FslexFsyacc.Runtime
type token = string
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [1u;2u;3u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [4u;5u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [6u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [7u],[],fun (lexbuf:list<_>) ->
        lexbuf
    [8u],[],fun (lexbuf:list<_>) ->
        []
]
let analyzer = Analyzer<_,_>(nextStates, rules)