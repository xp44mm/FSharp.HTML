module FSharp.HTML.ColgroupTokenUtils

let endTag = set["colgroup"]
let selfClosingTag = set["col"]
let startTag = set["colgroup"]
let id = set["COMMENT";"WS"]

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text t -> 
        if t.Trim() = "" then
            "WS"
        else "TEXT"
    | CData _ -> "CDATA"
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
    | SEMICOLON -> ";"

