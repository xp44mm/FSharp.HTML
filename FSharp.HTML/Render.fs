module FSharp.HTML.Render

open System

let stringifyAttributes attributes =
    attributes
    |> List.map(fun(name, value) ->
        $" {name}={HtmlCharRefs.quoteAttrValue value}"
    )
    |> String.concat ""

let startTag name attributes =
    $"<{name}{stringifyAttributes attributes}>"

let endTag name = $"</{name}>"

let stringifyComment str = $"<!--{str}-->"

let stringifyCData str = $"<![CDATA[{str}]]>"

let rec stringifyNode (node:HtmlNode) =
    match node with
    | HtmlElement(name, attributes, []) ->
        $"<{name}{stringifyAttributes attributes}/>"

    | HtmlElement(("script"| "style"|"title"|"textarea") as name, attributes, elements) ->
        let content =
            match elements with
            | [] -> ""
            | [HtmlText x] -> x
            | _ -> failwithf "rawtext:%A" node
        $"{startTag name attributes}{content}{endTag name}"

    | HtmlElement(name, attributes, elements) ->
        let content =
            elements 
            |> List.map stringifyNode
            |> String.concat ""
        $"{startTag name attributes}{content}{endTag name}"
                    
    | HtmlText str -> 
        HtmlCharRefs.escapeNormalText str
    | HtmlComment str ->
        stringifyComment str
    | HtmlCData str ->
        stringifyCData str

let stringifyDoc (docType, elements) =
    let dec =
        if String.IsNullOrEmpty docType then 
            "<!DOCTYPE html>"
        else
            $"<!DOCTYPE {docType}>"
    let nodes =
        elements 
        |> List.map(stringifyNode)

    dec::nodes
    |> String.concat Environment.NewLine

