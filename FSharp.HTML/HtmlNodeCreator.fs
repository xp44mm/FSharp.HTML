module FSharp.HTML.HtmlNodeCreator
open FslexFsyacc.Runtime
open FSharp.Literals.Literal

//let htmlText (token:Position<HtmlToken>) =
//    match token.value with
//    | TEXT x -> HtmlText x
//    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

//let htmlComment (token:Position<HtmlToken>) =
//    match token.value with
//    | COMMENT x -> HtmlComment x
//    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

//let htmlCData (token:Position<HtmlToken>) =
//    match token.value with
//    | CDATA x -> HtmlCData x
//    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

//let getNameAttributes (token:Position<HtmlToken>) =
//    match token.value with
//    | TAGSTART (name,attrs) 
//    | TAGSELFCLOSING (name,attrs)
//        -> name,attrs
//    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

//let getName (token:Position<HtmlToken>) =
//    match token.value with
//    | TAGSTART (name,_) 
//    | TAGSELFCLOSING (name,_)
//    | TAGEND name
//        -> name
//    | _ -> failwith $"HtmlNodeCreator:{stringify token}"

