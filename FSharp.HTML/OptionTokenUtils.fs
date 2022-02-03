module FSharp.HTML.OptionTokenUtils

open FSharp.Literals

let sTagNames = set [
   "option";"optgroup"
]

let endTagNames = 
    set [
        "select";"datalist"
    ] + sTagNames

// 注意是否会有自封闭元素
let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing (name,_) -> 
        if sTagNames.Contains name then
            $"<{name}/>"
        else "TAGSELFCLOSING"
    | TagStart (name,_) -> 
        if sTagNames.Contains name then
            $"<{name}>"
        else "TAGSTART"
    | TagEnd name ->  
        if endTagNames.Contains name then
            $"</{name}>"
        else "TAGEND"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

