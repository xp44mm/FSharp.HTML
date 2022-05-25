module FSharp.HTML.ParagraphTokenUtils

let blockTag = set [
    "address"; "article"; "aside"; "blockquote"; "details"; "div"; "dl"; 
    "fieldset"; "figcaption"; "figure"; "footer"; "form"; 
    "h1"; "h2"; "h3"; "h4"; "h5"; "h6"; "header"; "hgroup"; "hr"; 
    "main"; "menu"; "nav"; "ol"; "p"; "pre"; "section"; "table"; "ul"
]

let getTag (token:HtmlToken) =
    match token with
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"

    | TagStart("p",_) -> "<p>"
    | TagEnd "p" ->  "</p>"
    | TagSelfClosing ("p",_) -> "<p/>"

    | TagStart(name,_)
    | TagEnd name
    | TagSelfClosing (name,_) when blockTag.Contains name -> "BLOCKTAG"

    | TagStart _ -> "TAGSTART"
    | TagEnd _ -> "TAGEND"
    | TagSelfClosing _ -> "TAGSELFCLOSING"

    | EOF -> "EOF"
    | DocType _ -> "DOCTYPE"

