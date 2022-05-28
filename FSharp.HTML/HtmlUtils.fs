module FSharp.HTML.HtmlUtils

/// Parses input text as a HtmlDocument tree
let parseDoc txt = Parser.parseDoc txt
//let parseWellFormedDoc txt = Parser.parseWellFormedDoc txt

///// Parses input text as a HtmlNode sequence, and ignore doctype.
//let parseNodes txt = Parser.parseNodes txt
//let parseWellFormedNodes txt = Parser.parseWellFormedNodes txt


let stringifyNode (node:HtmlNode) = 
    Render.stringifyNode node

let stringifyDoc (docType, elements) =
    Render.stringifyDoc(docType,elements)