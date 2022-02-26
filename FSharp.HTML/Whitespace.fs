module FSharp.HTML.Whitespace

//its Permitted content contains no textNode.
let notextElement = set [
    "acronym";
    "applet";
    "basefont";
    "bgsound";
    //"big";
    "blink";
    //"body";
    //"caption";
    //"center";
    "colgroup";
    "content";
    //"dd";
    "dialog";
    "dir";
    //"dt";
    "element";
    //"figcaption";
    //"font";
    "frame";
    "frameset";
    "head";
    "html";
    "image";
    "isindex";
    "legend";
    //"li";
    "listing";
    "marquee";
    "menu";
    //"menuitem";
    "multicol";
    "nextid";
    "nobr";
    "noembed";
    "noframes";
    "ol";
    "optgroup";
    //"option";
    "plaintext";
    "rb";
    "rbc";
    //"rp";
    //"rt";
    "rtc";
    "select";
    "shadow";
    "slot";
    "spacer";
    //"strike";
    //"summary";
    "table";
    "tbody";
    //"td";
    "tfoot";
    //"th";
    "thead";
    "tr";
    //"tt";
    "ul";
    "xmp";
    ]

let rec removeWsChildren (nodes:HtmlNode list) =
    nodes
    |> List.filter(function
        | HtmlText s when s.Trim() = "" -> false
        | _ -> true
    )
    |> List.map removeWS

and removeWS (node:HtmlNode) =
    match node with
    | HtmlElement("pre", _, _) -> node
    | HtmlElement(tag, attrs, children) when notextElement.Contains tag ->
        let children =
            children
            |> removeWsChildren
            |> List.map removeWS
        HtmlElement(tag, attrs, children)
    | HtmlElement(tag, attrs, children) ->
        let children =
            children
            |> List.map removeWS
        HtmlElement(tag, attrs, children)
    | HtmlCData _ -> node
    | HtmlComment _ -> node
    | HtmlText _ -> node

