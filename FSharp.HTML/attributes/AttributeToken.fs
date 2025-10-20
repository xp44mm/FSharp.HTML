namespace FSharp.HTML

type AttributeToken =
    | EQUALS
    | GT
    | ID of string
    | LT
    | QUOTED of string
    | SOL_GT

module AttributeToken =
    let index (tok:AttributeToken) =
        match tok with
        | AttributeToken.EQUALS -> 0
        | AttributeToken.GT -> 1
        | AttributeToken.ID _ -> 2
        | AttributeToken.LT -> 3
        | AttributeToken.QUOTED _ -> 4
        | AttributeToken.SOL_GT -> 5
    let tag (tok:AttributeToken) =
        match tok with
        | AttributeToken.EQUALS -> "="
        | AttributeToken.GT -> ">"
        | AttributeToken.ID _ -> "ID"
        | AttributeToken.LT -> "<"
        | AttributeToken.QUOTED _ -> "QUOTED"
        | AttributeToken.SOL_GT -> "/>"
    let tagToValue (tag:string) =
        match tag with
        | "=" -> 0
        | ">" -> 1
        | "ID" -> 2
        | "<" -> 3
        | "QUOTED" -> 4
        | "/>" -> 5
        | _ -> failwith tag
    let lexeme (tok:AttributeToken) =
        match tok with
        | AttributeToken.EQUALS -> null
        | AttributeToken.GT -> null
        | AttributeToken.ID x -> box x
        | AttributeToken.LT -> null
        | AttributeToken.QUOTED x -> box x
        | AttributeToken.SOL_GT -> null

