﻿module FSharp.HTML.Tokenizer

open FSharp.Idioms
open System.Text.RegularExpressions
open TryTokenizer
open Consumption

let afterStartTag tagName (rest:string) =
    seq {
        match tagName with
        | "textarea" | "title" ->
            let rgx = Regex($@"^([\s\S]*?)(</{tagName}\s*>)",RegexOptions.IgnoreCase)
            let mm = rgx.Match(rest)

            let g1 = mm.Groups.[1].Value
            yield g1.Length, TEXT g1

            let g2 = mm.Groups.[2].Value
            yield g2.Length, TAGEND tagName
        | "script" -> 
            let js,entTag,rest = consumeNestedJavaScript rest
            yield js.Length,TEXT js
            yield entTag.Length, TAGEND "script"
        | "style" -> 
            let css,endTag,rest = consumeNestedCss rest
            yield css.Length, TEXT css
            yield endTag.Length, TAGEND "style"
        | _ -> ()
    }

//其他普通的token
let entry (inp:string) =
    seq {
        match inp with    
        | On tryDOCTYPE (x,rest) ->
            let a = "<!DOCTYPE".Length
            let b = ">".Length
            let c = x.Length-1
            let xx = x.[a..c-b].Trim()
            yield x.Length, DOCTYPE xx
        | On tryComment (x,rest) -> 
            let a = "<!--".Length
            let b = "-->".Length
            let c = x.Length-1
            let xx = x.[a..c-b]
            yield x.Length, COMMENT xx
        | On tryCDATA (x,rest) -> 
            let a = "<![CDATA[".Length
            let b = "]]>".Length
            let c = x.Length-1
            let xx = x.[a..c-b]
            yield x.Length, CDATA xx
        | On tryEndTag (x,rest) ->
            let tagName = x.[2..x.Length-2].TrimEnd().ToLower()
            yield x.Length, TAGEND tagName
        | On tryStartTagOpen (x,rest) ->
            let tagName = x.[1..].ToLower()
            
            let markup,attrs,len = consumeAttributes rest
            let len = x.Length + len
            match markup with
            | "/>" -> 
                yield len,TAGSELFCLOSING(tagName,attrs)
            | ">" -> 
                yield len,TAGSTART(tagName,attrs)
            | _ -> failwith markup
        | On tryText (x,rest) -> 
            yield x.Length, TEXT x

        | _ -> failwith inp
    }

open FslexFsyacc.Runtime

let tokenize(inp:string) =
    let mutable i = 0
    let mutable tagStarted = None
    seq {
        while i < inp.Length do
            match tagStarted with
            | Some tagName -> 
                tagStarted <- None
                yield! afterStartTag tagName inp.[i..]
            | None -> 
                yield! entry inp.[i..]
        yield 0, EOF
    }
    |> Seq.map(fun (len,tok) ->
        let pos = {
            index = i
            length = len
            value = tok
        }
        i <- i + len
        match tok with
        | TAGSTART(nm,_) -> 
            tagStarted <- Some nm
        | _ -> ()
        pos
    )

