module FSharp.HTML.ListTokenUtils

let id = set ["COMMENT";"WS"];
let selfClosingTag = set ["li";"script";"template"];
let startTag = set ["li";"menu";"ol";"ul"]
let endTag = set ["li";"menu";"ol";"script";"template";"ul"];

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


