namespace FSharp.HTML

type HtmlToken =
    | DocType of string
    | Text of string
    | Comment of string
    | CData of string
    | TagSelfClosing of name:string * attrs:list<string*string>
    | TagStart of name:string * attrs:list<string*string>
    | TagEnd of name:string
    //| EOF
