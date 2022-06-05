module FSharp.HTML.HtmlNodeCreator
open FslexFsyacc.Runtime
open FSharp.Literals.Literal

let htmlText (token:Position<HtmlToken>) =
    match token.value with
    | Text x -> HtmlText x
    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

let htmlComment (token:Position<HtmlToken>) =
    match token.value with
    | Comment x -> HtmlComment x
    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

let htmlCData (token:Position<HtmlToken>) =
    match token.value with
    | CData x -> HtmlCData x
    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

let getNameAttributes (token:Position<HtmlToken>) =
    match token.value with
    | TagStart (name,attrs) 
    | TagSelfClosing (name,attrs)
        -> name,attrs
    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

let getName (token:Position<HtmlToken>) =
    match token.value with
    | TagStart (name,_) 
    | TagSelfClosing (name,_)
    | TagEnd name
        -> name
    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

