module FSharp.HTML.thead_tbody_tfoot_Driver


//let getOmittedTagends (startTags:seq<string>) =
//    let getTag (token:string) =
//        match token with
//        |"td"|"th"
//        |"tr"
//        |"thead"|"tbody"|"tfoot"
//        |"caption"|"colgroup"
//            -> token
//        | _ -> "*"

//    let analyze (tokens:seq<string>) = 
//        thead_tbody_tfoot_DFA.analyzer.analyze(tokens,getTag)




//    if Seq.isEmpty startTags then
//        []
//    else
//        startTags
//        |> analyze
//        |> Seq.head
