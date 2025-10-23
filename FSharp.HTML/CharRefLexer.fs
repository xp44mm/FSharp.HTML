module FSharp.HTML.CharRefLexer
open System
open FSharp.LexYacc
open FSharp.Idioms
let anal:Analyzer = {nextStates=Map [0u,set [[-7,37;39,1048575],10u;[38,38],2u];1u,set [[59,59],3u;[65,90;97,122],1u];2u,set [[35,35],5u;[65,90;97,122],1u];4u,set [[48,57],4u;[59,59],6u];5u,set [[48,57],4u;[88,88;120,120],7u];7u,set [[48,57;65,70;97,102],8u];8u,set [[48,57;65,70;97,102],8u;[59,59],9u]];flags=Map [1u,set [0uy,Accept];2u,set [3uy,Accept];3u,set [0uy,Accept];6u,set [1uy,Accept];9u,set [2uy,Accept];10u,set [3uy,Accept]];lookbehinds=Map []}
let actions : Map<byte,  char list -> string > = Map [
    0uy, fun buff -> 
        let str = String(buff |> List.skip 1 |> Array.ofList)
        let str = str.TrimEnd(';')
        match HtmlSchema.namedCharRefs.TryFind(str) with
        | Some c -> c
        | None -> String(Array.ofList buff)
    1uy, fun buff -> 
        let str = buff |> List.skip 2
        let d,_,_ = Decimal.takeDigitsValueAndCount str
        String([|char d|])
    2uy, fun buff -> 
        let buff1 = buff |> List.skip 3
        let buff2 = buff1 |> List.take (buff1.Length - 1)
        let d = Hexadecimal.getValue buff2
        String([|char d|])
    3uy, fun buff -> String(Array.ofList buff)
    ]