module FSharp.HTML.RubyTokenUtils

open FSharp.Literals

let rubyTagNames = set [
    "rt";"rp";"ruby"
]

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing _ -> "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if name = "rt" || name = "rp" then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->  
        if rubyTagNames.Contains name then
            $"</{name}>"
        else "TAGEND"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

