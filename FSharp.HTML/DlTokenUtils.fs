module FSharp.HTML.DlTokenUtils

open FSharp.Literals

// start
let sTagNames = set [
    "dt";"dd";"dl";"div"
    ]
// end
let eTagNames = set [
    "dt";"dd";"dl";"div"
    ]

// close
let cTagNames = set [
    "dt";"dd";
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


