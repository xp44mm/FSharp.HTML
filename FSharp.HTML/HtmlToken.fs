namespace FSharp.HTML

type HtmlToken =
    | EOF
    | DOCTYPE        of string
    | TEXT           of string
    | COMMENT        of string
    | CDATA          of string
    | TAGSELFCLOSING of tag:string * attrs:list<string*string>
    | TAGSTART       of tag:string * attrs:list<string*string>
    | TAGEND         of tag:string
