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

let blockLevelFamily = set [
    "address"   
    "article"   
    "aside"     
    "blockquote"
    "dd"        
    "details"   
    "dialog"    
    "div"       
    "dl"        
    "dt"        
    "fieldset"  
    "figcaption"
    "figure"    
    "footer"    
    "form"      
    "h1"        
    "h2"        
    "h3"        
    "h4"        
    "h5"        
    "h6"        
    "header"    
    "hgroup"    
    "hr"        
    "li"        
    "main"      
    //"nav"       
    //"ol"        
    "p"         
    //"pre"       
    "section"   
    //"table"     
    //"ul" 
    
    //补充的
    "option"
    "th"
    "td"
    "caption"
]

open System.Text.RegularExpressions
// todo:根据所在元素，来确定空白的处理方式

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

let trimWhitespace elements =
    let rec forward trimStart acc children =
        match children with
        | h::t -> 
            match h with
            | HtmlText x -> 
                match x.TrimStart() with
                | "" -> forward trimStart acc t
                | y -> 
                    let trimStart = Regex.IsMatch(y,@"\s+$")
                    forward trimStart (HtmlText y::acc) t

            | HtmlElement("pre",_,_) -> forward true (h::acc) t

            | HtmlElement(tag,attrs,subchildren) ->
                let trimStart = trimStart || blockLevelFamily.Contains tag
                let trimStart,subchildren = forward trimStart [] subchildren
                let hh = HtmlElement(tag,attrs,subchildren)
                let trimStart = trimStart || blockLevelFamily.Contains tag
                forward trimStart (hh::acc) t

            | HtmlComment _ -> forward trimStart (h::acc) t

            | HtmlCData _ -> forward false (h::acc) t
        | [] ->
            trimStart, acc

    let rec backward trimEnd acc revchildren =
        match revchildren with
        | h::t -> 
            match h with
            | HtmlText x -> 
                match x.TrimEnd() with
                | "" -> backward trimEnd acc t
                | y -> 
                    let y = Regex.Replace(y,"\s+", " ")
                    backward false (HtmlText y::acc) t

            | HtmlElement("pre",_,_) -> backward true (h::acc) t

            | HtmlElement(nm,attrs,subchildren) -> // pre
                let trimEnd = trimEnd || blockLevelFamily.Contains nm
                let trimEnd,subchildren = backward trimEnd [] subchildren
                let hh = HtmlElement(nm,attrs,subchildren)
                let trimEnd = trimEnd || blockLevelFamily.Contains nm
                backward trimEnd (hh::acc) t
            | HtmlComment _ -> backward trimEnd (h::acc) t
            | HtmlCData _ -> backward false (h::acc) t
        | [] -> trimEnd, acc

    elements
    |> forward  true [] |> snd
    |> backward true [] |> snd

