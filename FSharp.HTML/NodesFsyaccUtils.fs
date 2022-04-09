module FSharp.HTML.NodesFsyaccUtils

let tcelles = set ["th";"td";]
let dcelles = set ["dt";"dd";]

let htmlNode sn en attrs children =
    if sn = en then
        HtmlElement(sn, attrs, children)
    elif en = "th|td" && tcelles.Contains sn then
        HtmlElement(sn, attrs, children)
    elif en = "dt|dd" && dcelles.Contains sn then
        HtmlElement(sn, attrs, children)
    else failwithf "Misnested tags:<%s></%s>" sn en
