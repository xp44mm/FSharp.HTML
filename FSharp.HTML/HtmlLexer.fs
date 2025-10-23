module FSharp.HTML.HtmlLexer
open System
open FSharp.Idioms
open FSharp.LexYacc
let anal:Analyzer = {nextStates=Map [0u,set [[-7,59;61,1048575],25u;[60,60],1u];1u,set [[-7,32;34,46;48,61;63,1048575],23u;[33,33],2u;[47,47],21u];2u,set [[45,45],3u;[68,68;100,100],5u;[91,91],14u];3u,set [[45,45],4u];5u,set [[79,79;111,111],6u];6u,set [[67,67;99,99],7u];7u,set [[84,84;116,116],8u];8u,set [[89,89;121,121],9u];9u,set [[80,80;112,112],10u];10u,set [[69,69;101,101],11u];11u,set [[32,32],12u];12u,set [[-7,61;63,1048575],12u;[62,62],13u];14u,set [[67,67;99,99],15u];15u,set [[68,68;100,100],16u];16u,set [[65,65;97,97],17u];17u,set [[84,84;116,116],18u];18u,set [[65,65;97,97],19u];19u,set [[91,91],20u];21u,set [[-7,61;63,1048575],21u;[62,62],22u];23u,set [[-7,61;63,1048575],23u;[62,62],24u];25u,set [[-7,59;61,1048575],25u]];flags=Map [1u,set [6uy,Accept];4u,set [0uy,Accept];13u,set [1uy,Accept];20u,set [2uy,Accept];22u,set [3uy,Accept];24u,set [4uy,Accept];25u,set [5uy,Accept]];lookbehinds=Map []}
let actions comment cdata attribute: Map<byte,  char list -> HtmlToken list > = Map [
    0uy, fun buff -> 
        let c = comment()
        [COMMENT c]
    1uy, fun buff -> 
        let str = String(buff |> List.skip 10 |> Array.ofList)
        [DOCTYPE(str.Substring(0, str.Length - 1).Trim())]
    2uy, fun buff -> 
        let c = cdata()
        [CDATA c]
    3uy, fun buff -> 
        let str = String(buff |> List.skip 2 |> Array.ofList)
        [TAGEND (str.Substring(0, str.Length - 1).Trim())]
    4uy, fun buff -> [attribute buff]
    5uy, fun buff -> 
        let s = String(Array.ofList buff)
        [
            if String.IsNullOrWhiteSpace s then
                WS s
            else
                TEXT s
        ]
    6uy, fun buff -> failwithf "html lexer: %c"  buff.[0]
    ]