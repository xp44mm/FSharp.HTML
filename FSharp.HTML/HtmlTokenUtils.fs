module FSharp.HTML.HtmlTokenUtils
open FSharp.Literals

let adapt (token:HtmlToken) =
    match token with
    | Tag (true ,name,attrs) -> TagSelfClosing(name,attrs)
    | Tag (false,name,attrs) -> TagStart(name,attrs)
    | _ -> token

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing (name,_) -> $"TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if TagNames.specialTagNames.Contains name then
            $"<{name}>"
        else "TAGSTART"

    | TagEnd name ->  
        if TagNames.specialTagNames.Contains name then
            $"</{name}>"
        else "TAGEND"
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

