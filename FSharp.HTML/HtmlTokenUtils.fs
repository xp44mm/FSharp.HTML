module FSharp.HTML.HtmlTokenUtils
open FSharp.Literals
open FSharp.Idioms

/// consume doctype from tokens
let preamble (tokens:HtmlToken seq) =
    let iterator = Iterator(tokens.GetEnumerator())
    let rec loop () =
        match iterator.tryNext() with
        | Some(Text t) when t.Trim() = "" -> loop ()
        | Some(DocType _) as sm ->
            let rest =
                iterator.tryNext()
                |> Seq.unfold(
                    Option.map(fun v -> v,iterator.tryNext())
                )
                |> Seq.skipWhile(function
                    | Text t when t.Trim() = "" -> true
                    | _ -> false
                )
            sm,rest
        | maybe -> 
            let rest =
                maybe
                |> Seq.unfold(
                    Option.map(fun v -> v,iterator.tryNext())
                )
            None,rest
    loop()

let unifyVoidElement (token:HtmlToken) =    
    match token with
    | TagStart (name,attrs) when 
        TagNames.voidElements.Contains name ->
        TagSelfClosing(name,attrs)
        |> Some
    | TagEnd name when 
        TagNames.voidElements.Contains name ->
        None
    | _ -> 
        Some token

let voidTagStartToSelfClosing(token:HtmlToken) =    
    match token with
    | TagStart (name,attrs) when 
        TagNames.voidElements.Contains name ->
        TagSelfClosing(name,attrs)
    | _ -> token

let isVoidTagEnd(token:HtmlToken) =    
    match token with
    | TagEnd name when 
        TagNames.voidElements.Contains name ->
        true
    | _ -> false

let getTag (token:HtmlToken) =
    match token with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing _ -> "TAGSELFCLOSING"
    | TagStart _ -> "TAGSTART"
    | TagEnd _ -> "TAGEND"
    | EOF -> "EOF"

let getLexeme (token:HtmlToken) =
    match token with
    | DocType s -> box s
    | Comment s -> box s
    | Text    s -> box s
    | CData   s -> box s
    | TagSelfClosing (name,attrs) -> box (name,attrs)
    | TagStart (name,attrs) -> box (name,attrs)
    | TagEnd name ->  box name
    | _ -> null

