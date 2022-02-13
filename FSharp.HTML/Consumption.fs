module FSharp.HTML.Consumption

open FSharp.Idioms
open System.Text.RegularExpressions
open TryTokenizer

let consumeAttributeNames (inp:string) =
    let rec loop (revTokens:list<string>) (inp:string) =
        match inp with
        | On tryWS (_,rest) -> 
            loop revTokens rest

        | On tryAttributeName (x,rest) ->             
            let tokens = x::revTokens
            loop tokens rest

        | On tryUnquotedAttributeValue (x,rest) -> 
            let y = x.Substring(1).TrimStart() // remove ^=\s*
            let v = sprintf "=%s" y
            let tokens = v::revTokens
            loop tokens rest

        | On tryQuotedAttributeValue (x,rest) -> 
            let y = x.Substring(1).TrimStart() // remove ^=\s*
            let v = sprintf "=%s" y.[1..y.Length-2] // unquote
            let tokens = v::revTokens
            loop tokens rest

        | On (tryPrefix @"/?>") (x,rest) -> 
            let attributes = 
                if revTokens.IsEmpty then 
                    [] 
                else
                    AttributeDFA.analyze revTokens
                    |> Seq.map HtmlAttribute
                    |> Seq.toList
                    |> List.rev
            x, attributes, rest
        | _ -> failwithf "%A" (List.rev revTokens,inp)

    loop [] inp
    

let consumeNestedCss (inp:string) =
    let continueTries = 
        [|
            Regex("^[^/\"<]+") |> tryRegexMatch
            tryMultiLineComment
            tryDoubleStringLiteral
            fun inp -> Some(inp.[0..0],inp.[1..])
        |]
        |> Array.map(fun f -> 
            f 
            >> Option.map(fun(x,rest) -> 
                // 0 = continue
                0,x,rest))
    
    let breakTry =
        Regex("^</style\\s*>",RegexOptions.IgnoreCase) 
        |> tryRegexMatch
        >> Option.map(fun(x,rest)-> 
            // 1 = break
            1,x,rest)
    
    let tries = [| breakTry; yield! continueTries; |]

    let rec loop text seed =
        if seed = "" then failwithf "%A" (inp,text,seed)
        let exit,nextText,rest =
            tries
            |> Array.pick(fun f -> f seed)

        if exit = 0 then
            let text = text+nextText
            loop text rest
        else
            text,nextText,rest
    loop "" inp

let consumeNestedJavaScript (inp:string) =
    let continueTries = 
        [|
            Regex("^[^\"'`/<]+") |> tryRegexMatch
            tryDoubleStringLiteral
            trySingleStringLiteral
            tryGraveAccent
            trySingleLineComment
            tryMultiLineComment
            // todo: regular expression literal
            fun inp -> Some(inp.[0..0],inp.[1..])
        |]
        |> Array.map(fun f -> 
            f 
            >> Option.map(fun(x,rest) -> 
                // 0 = continue
                0,x,rest))
   
    let breakTry =
        Regex("^</script\\s*>",RegexOptions.IgnoreCase) 
        |> tryRegexMatch
        >> Option.map(fun(x,rest)-> 
            // 1 = break
            1,x,rest)
   
    let tries = [| breakTry; yield! continueTries; |]

    let rec loop (texts:string list) seed =
        if seed = "" then failwithf "%A" (inp,texts,seed)
        let exit,text,rest =
            try
                tries
                |> Array.pick(fun f -> f seed)
            with _ -> failwithf "%A" (inp,texts,seed)
        if exit = 0 then
            let texts = text::texts
            loop texts rest
        else
            let content = texts |> List.rev |> String.concat ""
            content,text,rest
    loop [] inp