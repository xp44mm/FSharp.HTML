module FSharp.HTML.Parser
open FSharp.Literals.Literal

/// Locate doctype at the beginning of tokens
let consumeDoctype tokens =
    let maybeDoctype, tokens = 
        tokens
        |> HtmlTokenUtils.preamble
    let docType = 
        maybeDoctype
        |> Option.map(function
            | {value=DocType dt} -> dt
            | _ -> "html"
        )
        |> Option.defaultValue "html"
    docType,tokens

let basicTokenize txt =
    let docType,tokens =
        txt
        |> Tokenizer.tokenize
        |> consumeDoctype

    let tokens =
        tokens
        |> Seq.choose HtmlTokenUtils.unifyVoidElement
    docType,tokens

/// Parses input text as a HtmlDocument tree
let parseDoc txt =
    let docType,tokens =
        txt
        |> basicTokenize

    let nodes =
        tokens
        |> Compiler.parse
        
    docType,nodes
