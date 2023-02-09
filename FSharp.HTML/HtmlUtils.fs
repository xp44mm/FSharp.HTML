module FSharp.HTML.HtmlUtils

/// Parses input text as a HtmlDocument tree
let parseDoc (txt:string) = 
    let doctype,nodes =
        txt
        |> HtmldocCompiler.compile
    let nodes =
        nodes
        |> List.map Whitespace.removeWS
        |> Whitespace.trimWhitespace
        |> List.map HtmlCharRefs.unescapseNode
    doctype,nodes
    
let stringifyNode (node:HtmlNode) = 
    Render.stringifyNode node

let stringifyDoc (docType:string, elements:HtmlNode list) =
    Render.stringifyDoc(docType,elements)
