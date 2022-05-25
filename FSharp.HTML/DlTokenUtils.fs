module FSharp.HTML.DlTokenUtils

let endTag = set ["dd";"div";"dl";"dt";"script";"template"]
let selfClosingTag = set ["dd";"dt";"script";"template"]
let startTag = set ["dd";"div";"dl";"dt"]

let getTag (token:HtmlToken) =
    match token with
    | Comment _ -> "COMMENT"
    | CData _ -> "CDATA"
    | Text t -> 
        if t.Trim() = "" then
            "WS"
        else "TEXT"
    | TagSelfClosing (name,_) -> 
        if selfClosingTag.Contains name then
            $"<{name}/>"
        else "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if startTag.Contains name then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->
        if endTag.Contains name then
            $"</{name}>"
        else "TAGEND"

    | EOF -> "EOF"

    //| SEMICOLON -> ";"
    | DocType _ -> "DOCTYPE"


