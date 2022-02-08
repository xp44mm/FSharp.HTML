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
                yield DocType x
                yield! loop rest

            | On tryComment (x,rest) -> 
                yield Comment x
                yield! loop rest

            | On tryCDATA (x,rest) -> 
                yield CData x
                yield! loop rest

            | On tryEndTag (x,rest) ->
                let tagName = x.[2..x.Length-2].TrimEnd().ToLower()
                yield TagEnd tagName
                yield! loop rest

            | On tryStartTagOpen (x,rest) ->
                let tagName = x.[1..].ToLower()
                let lt,attrs,rest = consumeAttributeNames rest
                match lt with
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
                | _ -> failwith lt
            | _ -> failwith inp
        }
    
    loop inp
