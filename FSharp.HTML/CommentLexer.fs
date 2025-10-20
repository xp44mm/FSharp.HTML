module FSharp.HTML.CommentLexer
open System
open FSharp.LexYacc
let anal:Analyzer = {nextStates=Map [0u,set [[-7,44;46,1048575],4u;[45,45],1u];1u,set [[45,45],2u];2u,set [[62,62],3u]];flags=Map [1u,set [1uy,Accept];3u,set [0uy,Accept];4u,set [1uy,Accept]];lookbehinds=Map []}
let actions : Map<byte,  char list -> char list > = Map [
    0uy, fun buff -> buff
    1uy, fun buff -> buff
    ]