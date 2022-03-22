module FSharp.HTML.Parser

/// convert void element from <br> or <br></br> to <br/>
let unifyVoidElement tokens = 
    tokens 
    |> Seq.choose HtmlTokenUtils.unifyVoidElement

/// complement omitted </option> </optgroup>
let complementOption tokens =
    tokens
    |> OptgroupDFA.analyze
    |> Seq.concat

    |> OptionDFA.analyze
    |> Seq.concat

/// complement omitted </caption> </colgroup> </thead> <tbody> </tbody> </tfoot> </tr> </th> </td>
let complementTable tokens =
    tokens
    |> ColgroupDFA.analyze
    |> Seq.concat

    |> CaptionDFA.analyze
    |> Seq.concat

    |> TheadDFA.analyze
    |> Seq.concat

    |> TbodyDFA.analyze
    |> Seq.concat

    |> TrDFA.analyze
    |> Seq.concat

    |> TdDFA.analyze
    |> Seq.concat

/// complement omitted </li>
let complementLi tokens =
    tokens
    |> ListDFA.analyze
    |> Seq.concat

/// complement omitted </rt> </rp>
let complementRuby tokens =
    tokens
    |> RubyDFA.analyze
    |> Seq.concat

/// complement omitted </p>
let complementParagraph tokens =
    tokens
    |> ParagraphDFA.analyze
    |> Seq.concat

/// complement omitted </dt> </dd>
let complementDl tokens =
    tokens
    |> DlDFA.analyze
    |> Seq.concat

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

/// no doctype, the tags are all in pairs in tokens
let getWellFormedNodes tokens =
    tokens
    |> SemiNodeDFA.analyze
    |> Seq.concat
    |> NodesParseTable.parse

let getNodes tokens =
    tokens
    |> unifyVoidElement
    |> complementLi
    |> complementRuby
    |> complementOption
    |> complementTable
    |> complementParagraph
    |> complementDl
    |> getWellFormedNodes

let getDoc tokens =
    let docType, tokens =
        consumeDoctype tokens
    let nodes = 
        tokens
        |> getNodes 
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace
    HtmlDocument(docType,nodes)

/// Parses input text as a HtmlDocument tree
let parseDoc txt =
    txt
    |> Tokenizer.tokenize
    |> getDoc

/// Parses input text as a HtmlNode sequence, and ignore doctype.
let parseNodes txt =
    txt
    |> Tokenizer.tokenize
    |> consumeDoctype
    |> snd
    |> getNodes
    |> Whitespace.removeWsChildren
    |> Whitespace.trimWhitespace