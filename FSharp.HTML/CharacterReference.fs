module FSharp.HTML.CharacterReference

let decodeCharRefs (s: string) =
    s.ToCharArray()
    |> Array.toList
    |> CharRefLexerTailer.decode

let rec processCharRefs (node: HtmlNode) : HtmlNode =
    match node with
    | HtmlElement(name, attributes, children) ->
        let processedAttrs =
            attributes
            |> List.map(fun (key, value) -> (key, decodeCharRefs value))
        let processedChildren = children |> List.map processCharRefs
        HtmlElement(name, processedAttrs, processedChildren)
    | HtmlText text -> HtmlText(decodeCharRefs text)
    | HtmlComment text -> HtmlComment text
    | HtmlCData text -> HtmlCData text 
    | HtmlDoctype doctype -> HtmlDoctype doctype
    | HtmlWS whitespace -> HtmlWS whitespace
