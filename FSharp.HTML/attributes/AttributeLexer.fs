module FSharp.HTML.AttributeLexer
open System
open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.LexYacc
open FSharp.HTML
let anal:Analyzer = {nextStates=Map [0u,set [[-7,-1;35,38;40,46;48,59;63,91;93,95;97,126;128,1048575],13u;[0,8;11,12;14,31;33,33;92,92;96,96;127,127],14u;[9,10;13,13;32,32],1u;[34,34],7u;[39,39],10u;[47,47],4u;[60,60],2u;[61,61],6u;[62,62],3u];1u,set [[9,10;13,13;32,32],1u];4u,set [[62,62],5u];7u,set [[-7,33;35,1048575],8u;[34,34],9u];8u,set [[-7,33;35,1048575],8u;[34,34],9u];10u,set [[-7,38;40,1048575],11u;[39,39],12u];11u,set [[-7,38;40,1048575],11u;[39,39],12u];13u,set [[-7,-1;35,38;40,46;48,59;63,91;93,95;97,126;128,1048575],13u]];flags=Map [1u,set [0uy,Accept];2u,set [1uy,Accept];3u,set [2uy,Accept];4u,set [8uy,Accept];5u,set [3uy,Accept];6u,set [4uy,Accept];7u,set [8uy,Accept];9u,set [5uy,Accept];10u,set [8uy,Accept];12u,set [6uy,Accept];13u,set [7uy,Accept];14u,set [8uy,Accept]];lookbehinds=Map []}
let actions : Map<byte,  char list -> AttributeToken list > = Map [
    0uy, fun buff -> []
    1uy, fun buff -> [LT]
    2uy, fun buff -> [GT]
    3uy, fun buff -> [SOL_GT]
    4uy, fun buff -> [EQUALS]
    5uy, fun buff -> 
        let str = String(buff |> List.skip 1 |> Array.ofList)
        [QUOTED(str.Substring(0, str.Length - 1))]
    6uy, fun buff -> 
        let str = String(buff |> List.skip 1 |> Array.ofList)
        [QUOTED(str.Substring(0, str.Length - 1))]
    7uy, fun buff -> [ID(String(Array.ofList buff))]
    8uy, fun buff -> failwith(stringify buff.[0])
    ]