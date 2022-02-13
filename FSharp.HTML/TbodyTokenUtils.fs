module FSharp.HTML.TbodyTokenUtils

open FSharp.Literals

// start
let sTagNames = set [
    "table"
    "tr"
    ]
// end
let eTagNames = set [
    "caption"
    "colgroup"
    ]

// close
let cTagNames = set [
    "caption"
    "colgroup"
    "tr"
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

    //| _ -> failwith (Literal.stringify token)

