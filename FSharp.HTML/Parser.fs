module FSharp.HTML.Parser

/// Locate doctype at the beginning of tokens
let consumeDoctype tokens =
    let maybeDoctype, tokens = 
        tokens
        |> HtmlTokenUtils.preamble
    let docType = 
        maybeDoctype
        |> Option.map(function
            | DocType dt -> dt
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
        |> Seq.filter(HtmlTokenUtils.isVoidTagEnd>>not)
        |> Seq.map HtmlTokenUtils.voidTagStartToSelfClosing
    docType,tokens

/// Parses input text as a HtmlDocument tree
let parseDoc txt =
    let docType,tokens =
        txt
        |> basicTokenize
    let nodes =
        tokens
        |> Compiler.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace
        |> List.map HtmlCharRefs.unescapseNode
        
    docType,nodes


///// no doctype, the tags are all in pairs in tokens
//let getWellFormedNodes tokens =
//    tokens
//    |> PrecNodesParseTable.parse
//    |> Whitespace.removeWsChildren
//    |> Whitespace.trimWhitespace
//    |> List.map HtmlCharRefs.unescapseNode

//let getNodes tokens =
//    tokens
//    |> getWellFormedNodes

//let getWellFormedDoc tokens =
//    let docType, tokens =
//        consumeDoctype tokens
//    let nodes = 
//        tokens
//        |> Seq.filter(HtmlTokenUtils.isVoidTagEnd>>not)
//        |> Seq.map HtmlTokenUtils.voidTagStartToSelfClosing

//        |> getWellFormedNodes 
//    HtmlDocument(docType,nodes)


///// Parses input text as a HtmlNode sequence, and ignore doctype.
//let parseNodes txt =
//    txt
//    |> Tokenizer.tokenize
//    |> consumeDoctype
//    |> snd
//    |> getNodes

///// Parses input text as a HtmlDocument tree
//let parseWellFormedDoc txt =
//    txt
//    |> Tokenizer.tokenize
//    |> getWellFormedDoc

///// Parses input text as a HtmlNode sequence, and ignore doctype.
//let parseWellFormedNodes txt =
//    txt
//    |> Tokenizer.tokenize
//    |> consumeDoctype
//    |> snd
//    |> Seq.filter(HtmlTokenUtils.isVoidTagEnd>>not)
//    |> Seq.map HtmlTokenUtils.voidTagStartToSelfClosing

//    |> getWellFormedNodes


