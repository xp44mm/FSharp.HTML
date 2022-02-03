module FSharp.HTML.ListTokenUtils

open FSharp.Literals

let listTagNames = set [
    "li";"script";"template"
    ]

let parentTagNames = set [
    "ol";"ul";"menu";
    ]

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing (name,_) -> 
        if listTagNames.Contains name then
            $"<{name}/>"
        else "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if name = "li" || parentTagNames.Contains name then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->
        if listTagNames.Contains name || parentTagNames.Contains name then
            $"</{name}>"
        else "TAGEND"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

