module FSharp.HTML.Render

open System
open System.Text.RegularExpressions
open System.Text
open FSharp.Idioms.Literal

let quoteAttrValue (valuestring: string) =
    let tokens = Regex.Split(valuestring, "([&\"])")
    tokens
    |> Array.map(
        function
        | "&" -> "&amp;"
        | "\"" -> "&quot;"
        | x -> x
    )
    |> String.concat ""
    |> sprintf "\"%s\""

let encodeNormalText (text: string) =
    let tokens = Regex.Split(text, "([&<])")
    let sb = StringBuilder()

    tokens
    |> Array.map(
        function
        | "&" -> "&amp;"
        | "<" -> "&lt;"
        | x -> x
    )
    |> Array.iter(fun x -> sb.Append(x) |> ignore)
    sb.ToString()

let stringifyAttributes attributes =
    attributes
    |> List.map(fun (name, value) -> $" {name}={quoteAttrValue value}")
    |> String.concat ""

let openingTag name attributes = $"<{name}{stringifyAttributes attributes}>"

let selfClosingTag name attributes =
    $"<{name}{stringifyAttributes attributes}/>"

let closingTag name = $"</{name}>"

let comment str = $"<!--{str}-->"

let cdata str = $"<![CDATA[{str}]]>"

let rec stringifyNode (node: HtmlNode) =
    match node with
    | HtmlDoctype docType ->
        if String.IsNullOrEmpty docType then
            "<!DOCTYPE html>"
        else
            $"<!DOCTYPE {docType}>"
    | HtmlElement(name, attributes, []) ->
        if HtmlSchema.voidElements.Contains name then
            openingTag name attributes
        else
            selfClosingTag name attributes
    | HtmlElement(("script" | "style" | "title" | "textarea") as name,
                  attributes,
                  elements) ->
        let content =
            let sb = StringBuilder()
            for elem in elements do
                match elem with
                |  HtmlText x |  HtmlWS x -> sb.Append(x) |> ignore
                | _ -> failwith(stringify elem)
            sb.ToString()
        $"{openingTag name attributes}{content}{closingTag name}"

    | HtmlElement(name, attributes, elements) ->
        let content =
            elements
            |> List.map stringifyNode
            |> String.concat ""
        $"{openingTag name attributes}{content}{closingTag name}"

    | HtmlText str -> encodeNormalText str
    | HtmlWS str -> str
    | HtmlComment str -> comment str
    | HtmlCData str -> cdata str

let stringifyDocument (nodes: HtmlNode list) =
    let sb = StringBuilder()
    for node in nodes do
        sb.Append(stringifyNode node) |> ignore
    sb.ToString()
