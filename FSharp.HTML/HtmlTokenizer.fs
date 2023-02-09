module FSharp.HTML.HtmlTokenizer

open System
open System.Text.RegularExpressions
open FSharp.Idioms
open FslexFsyacc.Runtime
open TryTokenizer
open Consumption

let firstEntry i inp =
    match inp with
    | "" -> { index= i;length= 0;value= EOF }
    | On tryDOCTYPE (x,rest) ->
        let a = "<!DOCTYPE".Length
        let b = ">".Length
        let c = x.Length-1
        let xx = x.[a..c-b].Trim()
        { index= i;length= x.Length;value= DOCTYPE xx }
    | On tryComment (x,rest) ->
        let a = "<!--".Length
        let b = "-->".Length
        let c = x.Length-1
        let xx = x.[a..c-b]
        { index= i;length= x.Length;value= COMMENT xx }
    | On tryCDATA (x,rest) ->
        let a = "<![CDATA[".Length
        let b = "]]>".Length
        let c = x.Length-1
        let xx = x.[a..c-b]
        { index= i;length= x.Length;value= CDATA xx }
    | On tryEndTag (x,rest) ->
        let tagName = x.[2..x.Length-2].TrimEnd().ToLower()
        { index= i;length= x.Length;value= TAGEND tagName }
    | On tryStartTagOpen (x,rest) ->
        let tagName = x.[1..].ToLower()
        let markup,attrs,len = consumeAttributes rest
        let len = x.Length + len
        match markup with
        | "/>" ->
            { index= i;length= len;value= TAGSELFCLOSING(tagName,attrs) }
        | ">" ->
            { index= i;length= len;value= TAGSTART(tagName,attrs) }
        | _ -> failwith markup
    | On tryText (x,rest) ->
        { index= i;length= x.Length;value= TEXT x }

    | _ -> failwith inp

let afterStartTag restloop tagName i (rest:string) =
    seq {
        match tagName with
        | "textarea" | "title" ->
            let rgx = Regex($@"^([\s\S]*?)(</{tagName}\s*>)",RegexOptions.IgnoreCase)
            let mm = rgx.Match(rest)

            let g1 = mm.Groups.[1].Value
            let pt1 = { index= i;length= g1.Length;value= TEXT g1 }
            yield pt1

            let g2 = mm.Groups.[2].Value
            let pt2 = { index= pt1.nextIndex;length= g2.Length;value= TAGEND tagName }

            yield pt2

            let n = pt2.nextIndex
            yield! restloop n (rest.Substring(n-i))
        | "script" ->
            let js,entTag,rest = consumeNestedJavaScript rest
            let pt1 = { index= i;length= js.Length;value= TEXT js }
            yield pt1
            let pt2 = { index= pt1.nextIndex;length= entTag.Length;value= TAGEND "script" }
            yield pt2

            yield! restloop pt2.nextIndex rest
        | "style" ->
            let css,endTag,rest = consumeNestedCss rest

            let pt1 = { index= i;length= css.Length;value= TEXT css }
            yield pt1

            let pt2 = { index= pt1.nextIndex;length= endTag.Length;value= TAGEND "style" }
            yield pt2

            yield! restloop pt2.nextIndex rest
        | _ -> 
            yield! restloop i rest
    }

let tokenize (idx:int) (txt:string) =
    let rec loop i (inp:string) =
        seq {
            let postok = firstEntry i inp
            yield postok

            match postok.value with
            | EOF -> ()
            | TAGSTART(tagname,_) ->
                let pos = postok.nextIndex
                let inp = inp.Substring(postok.length)
                yield! afterStartTag loop tagname pos inp
            | _ ->
                let pos = postok.nextIndex
                let inp = inp.Substring(postok.length)
                yield! loop pos inp
        }

    loop idx txt


