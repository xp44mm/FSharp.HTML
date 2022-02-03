module FSharp.HTML.OptgroupTokenUtils

open FSharp.Literals

let relatedTagNames = set [
    "optgroup";"select"
]

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing (name,_) -> 
        if name = "optgroup" then
            $"<{name}/>"
        else "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if name = "optgroup" then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->  
        if relatedTagNames.Contains name then
            $"</{name}>"
        else "TAGEND"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

