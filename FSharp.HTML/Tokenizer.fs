﻿module FSharp.HTML.Tokenizer

open FSharp.Idioms
open System.Text.RegularExpressions
open TryTokenizer
open Consumption

let tokenize(inp:string) =
    let rec loop (inp:string) =
        seq {
            match inp with
            | "" -> ()
    
            | On tryText (x,rest) -> 
                yield Text x
                yield! loop rest

            | On tryDOCTYPE (x,rest) ->
                let a = "<!DOCTYPE".Length
                let b = ">".Length
                let c = x.Length-1
                let xx = x.[a..c-b].Trim()
                yield DocType xx
                yield! loop rest

            | On tryComment (x,rest) -> 
                let a = "<!--".Length
                let b = "-->".Length
                let c = x.Length-1
                let xx = x.[a..c-b]
                yield Comment xx
                yield! loop rest

            | On tryCDATA (x,rest) -> 
                let a = "<![CDATA[".Length
                let b = "]]>".Length
                let c = x.Length-1
                let xx = x.[a..c-b]
                yield CData xx
                yield! loop rest

            | On tryEndTag (x,rest) ->
                let tagName = x.[2..x.Length-2].TrimEnd().ToLower()
                yield TagEnd tagName
                yield! loop rest

            | On tryStartTagOpen (x,rest) ->
                let tagName = x.[1..].ToLower()
                let markup,attrs,rest = consumeAttributeNames rest
                match markup with
                | "/>" -> 
                    yield TagSelfClosing(tagName,attrs)
                    yield! loop rest
                | ">" -> 
                    yield TagStart(tagName,attrs)
                    match tagName with
                    | "textarea" | "title" ->
                        let rgx = Regex($@"^([\s\S]*?)</{tagName}\s*>",RegexOptions.IgnoreCase)
                        let mm = rgx.Match(rest)
                        yield Text mm.Groups.[1].Value
                        yield TagEnd tagName
                        let rerest = mm.Result("$'")
                        yield! loop rerest
                    | "script" -> 
                        let txt,_,rest = consumeNestedJavaScript rest
                        yield Text txt
                        yield TagEnd tagName
                        yield! loop rest
                    | "style" -> 
                        let css,_,rest = consumeNestedCss rest
                        yield Text css
                        yield TagEnd tagName
                        yield! loop rest
                    | _ -> 
                        yield! loop rest
                | _ -> failwith markup
            | _ -> failwith inp
        }
    
    loop inp
