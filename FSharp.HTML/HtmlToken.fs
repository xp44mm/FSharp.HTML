namespace FSharp.HTML

type HtmlToken =
    | CDATA of string
    | COMMENT of string
    | DOCTYPE of string
    | TAGEND of tag: string
    | TAGSELFCLOSING of tag: string * attrs: list<string * string>
    | TAGSTART of tag: string * attrs: list<string * string>
    | TEXT of string
    | WS of string
    | EOF

module HtmlToken =
    let index (tok: HtmlToken) =
        match tok with
        | HtmlToken.CDATA _ -> 0
        | HtmlToken.COMMENT _ -> 1
        | HtmlToken.DOCTYPE _ -> 2
        | HtmlToken.TAGEND _ -> 3
        | HtmlToken.TAGSELFCLOSING _ -> 4
        | HtmlToken.TAGSTART _ -> 5
        | HtmlToken.TEXT _ -> 6
        | HtmlToken.WS _ -> 7
        | HtmlToken.EOF -> 8
    let tag (tok: HtmlToken) =
        match tok with
        | HtmlToken.CDATA _ -> "CDATA"
        | HtmlToken.COMMENT _ -> "COMMENT"
        | HtmlToken.DOCTYPE _ -> "DOCTYPE"
        | HtmlToken.TAGEND _ -> "TAGEND"
        | HtmlToken.TAGSELFCLOSING _ -> "TAGSELFCLOSING"
        | HtmlToken.TAGSTART _ -> "TAGSTART"
        | HtmlToken.TEXT _ -> "TEXT"
        | HtmlToken.WS _ -> "WS"
        | HtmlToken.EOF -> "EOF"
    let tagToValue (tag: string) =
        match tag with
        | "CDATA" -> 0
        | "COMMENT" -> 1
        | "DOCTYPE" -> 2
        | "TAGEND" -> 3
        | "TAGSELFCLOSING" -> 4
        | "TAGSTART" -> 5
        | "TEXT" -> 6
        | "WS" -> 7
        | "EOF" -> 8
        | _ -> failwith tag
    let lexeme (tok: HtmlToken) =
        match tok with
        | HtmlToken.CDATA x -> box x
        | HtmlToken.COMMENT x -> box x
        | HtmlToken.DOCTYPE x -> box x
        | HtmlToken.TAGEND x -> box x
        | HtmlToken.TAGSELFCLOSING(a, b) -> box(a, b)
        | HtmlToken.TAGSTART(a, b) -> box(a, b)
        | HtmlToken.TEXT x -> box x
        | HtmlToken.WS x -> box x
        | HtmlToken.EOF -> null
