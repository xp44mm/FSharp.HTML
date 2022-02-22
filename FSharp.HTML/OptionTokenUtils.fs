module FSharp.HTML.OptionTokenUtils

let endTag = set ["datalist";"optgroup";"option";"select"]
let selfClosingTag = set ["optgroup";"option"]
let startTag = set ["optgroup";"option"]

let getTag (token:HtmlToken) =
    match token with
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
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
    | DocType _ -> "DOCTYPE"

