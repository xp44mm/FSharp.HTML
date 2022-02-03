module FSharp.HTML.CaptionTokenUtils

open FSharp.Literals

let cTagNames = set [
    "table"
    "caption"
    "colgroup"
    "thead"
    "tbody"
    "tfoot"
    "tr"
    ]

let sTagNames = set [
    "table"
    "caption"
    "colgroup"
    "thead"
    "tbody"
    "tfoot"
    "tr"
    ]

let eTagNames = set [
    "table"
    "caption"
    ]

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing (name,_) -> 
        if cTagNames.Contains name then
            $"<{name}/>"
        else "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if sTagNames.Contains name then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->  
        if eTagNames.Contains name then
            $"</{name}>"
        else "TAGEND"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

