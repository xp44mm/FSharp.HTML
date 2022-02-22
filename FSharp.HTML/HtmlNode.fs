namespace FSharp.HTML

/// Represents an HTML node. The names of elements are always normalized to lowercase
type HtmlNode =
    | HtmlElement of name:string * attributes:list<string*string> * elements:HtmlNode list
    | HtmlText of content:string
    | HtmlComment of content:string
    | HtmlCData of content:string

