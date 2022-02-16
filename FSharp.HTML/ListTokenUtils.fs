module FSharp.HTML.ListTokenUtils

let selfClosingTag = set ["li";"script";"template"];
let startTag = set ["li";"menu";"ol";"ul"]
let endTag = set ["li";"menu";"ol";"script";"template";"ul"];

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

    | SEMICOLON -> ";"
    | DocType _ -> "DOCTYPE"


