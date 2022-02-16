namespace FSharp.HTML

type HtmlToken =
    | DocType of string
    | Text of string
    | Comment of string
    | CData of string

    | TagSelfClosing of name:string * attrs:(string*string) list
    | TagStart of name:string * attrs:(string*string) list
    | TagEnd of name:string

    | EOF // virtual

    | SEMICOLON // virtual
