module FSharp.HTML.HtmlTokenUtils
open FSharp.Literals
open FSharp.Idioms
open FslexFsyacc.Runtime

let unifyVoidElement(token:Position<HtmlToken>) =    
    match token.value with
    | TAGSTART (name,attrs) when 
        TagNames.voidElements.Contains name ->
        TAGSELFCLOSING(name,attrs)
        |> fun value -> Some {
            token with 
                //length = 0
                value = value
        }
    | TAGEND name when 
        TagNames.voidElements.Contains name ->
        None
    | _ -> 
        Some token

let getTag (token:Position<HtmlToken>) =
    match token.value with
    | EOF -> "EOF"
    | DOCTYPE        _ -> "DOCTYPE"
    | COMMENT        _ -> "COMMENT"
    | TEXT           _ -> "TEXT"
    | CDATA          _ -> "CDATA"
    | TAGSELFCLOSING _ -> "TAGSELFCLOSING"
    | TAGSTART       _ -> "TAGSTART"
    | TAGEND         _ -> "TAGEND"

let getLexeme (token:Position<HtmlToken>) = 
    match token.value with
    | EOF -> null
    | DOCTYPE s -> box s
    | TEXT    s -> box s
    | COMMENT s -> box s
    | CDATA   s -> box s
    | TAGEND  s -> box s
    | TAGSELFCLOSING (nm,attrs) -> box (nm,attrs)
    | TAGSTART       (nm,attrs) -> box (nm,attrs)

