module FSharp.HTML.HtmlUtils

///// Parses input text as a HtmlDocument tree
//let parseDoc txt = 
//    let doctype,nodes =
//        txt
//        |> Parser.parseDoc
//    let nodes =
//        nodes
//        |> Whitespace.removeWsChildren
//        |> Whitespace.trimWhitespace
//        |> List.map HtmlCharRefs.unescapseNode
//    doctype,nodes

/// Parses input text as a HtmlDocument tree
let parseDoc (txt:string) = 
    let doctype,nodes =
        txt
        |> HtmldocCompiler.compile
    let nodes =
        nodes
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace
        |> List.map HtmlCharRefs.unescapseNode
    doctype,nodes
    
let stringifyNode (node:HtmlNode) = 
    Render.stringifyNode node

let stringifyDoc (docType, elements) =
    Render.stringifyDoc(docType,elements)
