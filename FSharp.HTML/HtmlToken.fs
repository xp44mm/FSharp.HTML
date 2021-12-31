namespace FSharp.HTML

type HtmlToken =
    | DocType of string
    | Tag of isSelfClosing:bool * name:string * attrs:HtmlAttribute list
    | TagEnd of name:string
    | Text of string
    | Comment of string
    | CData of string
    | EOF

    | TagSelfClosing of name:string * attrs:HtmlAttribute list
    | TagStart of name:string * attrs:HtmlAttribute list
    | SEMICOLON
    override x.ToString() =
        match x with
        | DocType dt -> sprintf "doctype %s" dt
        | Tag(selfClose,name,_) -> sprintf "tag %b %s" selfClose name
        | TagEnd name -> sprintf "tagEnd %s" name
        | Text _ -> "text"
        | Comment _ -> "comment"
        | EOF -> "eof"
        | CData _ -> "cdata"

    member x.IsEndTag name =
        match x with
        | TagEnd(endName) when name = endName -> true
        | _ -> false

