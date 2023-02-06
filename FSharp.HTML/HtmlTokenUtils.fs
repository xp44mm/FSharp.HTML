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

//let getLexeme (token:Position<HtmlToken>) = box token


