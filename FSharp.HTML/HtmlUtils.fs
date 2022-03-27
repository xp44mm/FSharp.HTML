module FSharp.HTML.HtmlUtils

/// Parses input text as a HtmlDocument tree
let parseDoc txt = 
    Parser.parseDoc txt

/// Parses input text as a HtmlNode sequence, and ignore doctype.
let parseNodes txt =
    Parser.parseNodes txt

let stringifyNode (node:HtmlNode) = 
    Render.stringifyNode node

let stringifyDoc (docType, elements) =
    Render.stringifyDoc(docType,elements)