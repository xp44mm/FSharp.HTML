module FSharp.HTML.Parser
open FSharp.Literals.Literal

//let basicTokenize txt =
//    let docType,tokens =
//        txt
//        |> Tokenizer.tokenize
//        |> Compiler.consumeDoctype

//    let tokens =
//        tokens
//        |> Seq.choose HtmlTokenUtils.unifyVoidElement
//    docType,tokens

///// Parses input text as a doctype, and nodes.
//let parseDoc txt =
//    let docType,tokens =
//        txt
//        |> basicTokenize

//    let nodes =
//        tokens
//        |> Compiler.parse
        
//    docType,nodes
