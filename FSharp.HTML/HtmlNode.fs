namespace FSharp.HTML

/// Represents an HTML node. The names of elements are always normalized to lowercase
type HtmlNode =
    | HtmlElement of
        name: string *
        attributes: list<string * string> *
        elements: HtmlNode list
    | HtmlComment of string
    | HtmlCData of string
    | HtmlText of string
    | HtmlDoctype of string
