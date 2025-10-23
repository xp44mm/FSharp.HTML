module FSharp.HTML.HtmlUtils

/// Parses input text as a HtmlDocument tree
let parseDoc (txt: string) =
    txt
    |> HtmlCompiler.compileText
    |> Whitespace.trimWhitespace
    |> List.map CharacterReference.processCharRefs

let stringifyDoc (elements: HtmlNode list) = Render.stringifyDocument(elements)
