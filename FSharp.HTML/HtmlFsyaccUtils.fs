module FSharp.HTML.HtmlFsyaccUtils

let tbodies = set ["thead";"tbody";"tfoot"]
let tcelles = set ["th";"td";]
let dcelles = set ["dt";"dd";]

let htmlNode sn en =
    let el attrs children = HtmlElement(sn, attrs, children)
    if sn = en then        
        el
    elif en = "thead|tbody|tfoot" && tbodies.Contains sn then
        el
    elif en = "th|td" && tcelles.Contains sn then
        el
    elif en = "dt|dd" && dcelles.Contains sn then
        el
    else failwithf "%A</%s>" sn en
