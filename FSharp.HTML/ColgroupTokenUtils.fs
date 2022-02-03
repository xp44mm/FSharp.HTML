module FSharp.HTML.ColgroupTokenUtils

open FSharp.Literals

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing (name,_) -> 
        if name = "col" then
            $"<{name}/>"
        else "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if name = "colgroup" then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->  
        if name = "colgroup" then
            $"</{name}>"
        else "TAGEND"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

