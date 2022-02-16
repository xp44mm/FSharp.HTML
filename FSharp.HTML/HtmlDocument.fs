namespace FSharp.HTML

/// Represents an HTML document
type HtmlDocument =
    | HtmlDocument of docType:string * elements:HtmlNode[]

