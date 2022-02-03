module FSharp.HTML.HtmlTokenUtils
open FSharp.Literals

let adapt (token:HtmlToken) =
    match token with
    | Tag (true ,name,attrs) -> TagSelfClosing(name,attrs)
    | Tag (false,name,attrs) -> TagStart(name,attrs)
    | _ -> token

let unifyVoidElement (token:HtmlToken) =
    match token with
    | TagStart (name,attrs) ->
        if TagNames.voidElements.Contains name then
            TagSelfClosing(name,attrs)
        else token
        |> Some
    | TagEnd name ->
        if TagNames.voidElements.Contains name then
            None
        else Some token
    | _ -> Some token

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing _ -> "TAGSELFCLOSING"
    | TagStart _ -> "TAGSTART"
    | TagEnd _ -> "TAGEND"
    | SEMICOLON -> ";"
    | EOF -> "EOF"
    | _ -> failwith (Literal.stringify token)

let getLexeme (token:HtmlToken) =
    match token with
    | DocType s -> box s
    | Comment s -> box s
    | Text    s -> box s
    | CData   s -> box s
    | TagSelfClosing (name,attrs) -> box (name,attrs)
    | TagStart (name,attrs) -> box (name,attrs)
    | TagEnd name ->  box name
    | _ -> null

