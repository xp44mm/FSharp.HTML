module FSharp.HTML.ParagraphTokenUtils

let blockTag = set [
    "address"; "article"; "aside"; "blockquote"; "details"; "div"; "dl"; "fieldset"; "figcaption"; "figure"; "footer"; "form"; "h1"; "h2"; "h3"; "h4"; "h5"; "h6"; "header"; "hgroup"; "hr"; "main"; "menu"; "nav"; "ol"; "p"; "pre"; "section"; "table"; "ul"
]

open FSharp.Literals

// start
let sTagNames = set [
    "p"
    ]
// end
let eTagNames = set [
    "p"
    ]

// close
let cTagNames = set [
    "p"
    ]

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagStart (name,_) -> 
        if sTagNames.Contains name then
            $"<{name}>"
        elif blockTag.Contains name then
            "BLOCKTAG"
        else "TAGSTART"
    | TagEnd name ->  
        if eTagNames.Contains name then
            $"</{name}>"
        elif blockTag.Contains name then
            "BLOCKTAG"
        else "TAGEND"
    | TagSelfClosing (name,_) -> 
        if cTagNames.Contains name then
            $"<{name}/>"
        elif blockTag.Contains name then
            "BLOCKTAG"
        else "TAGSELFCLOSING"
    | EOF -> "EOF"
    | SEMICOLON -> ";"

    | _ -> failwith (Literal.stringify token)

