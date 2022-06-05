module FSharp.HTML.HtmlTokenUtils
open FSharp.Literals
open FSharp.Idioms
open FslexFsyacc.Runtime

/// consume doctype from tokens
let preamble (tokens:seq<Position<HtmlToken>>) =
    let iterator = 
        tokens.GetEnumerator()
        |> Iterator

    let rec loop () =
        match iterator.tryNext() with
        | Some {value = Text t } when t.Trim() = "" -> loop ()
        | Some {value = DocType _ } as sm ->
            let rest =
                iterator.tryNext()
                |> Seq.unfold(
                    Option.map(fun v -> v,iterator.tryNext())
                )
                |> Seq.skipWhile(function
                    | {value = Text t } when t.Trim() = "" -> true
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
    loop ()

let unifyVoidElement(token:Position<HtmlToken>) =    
    match token.value with
    | TagStart (name,attrs) when 
        TagNames.voidElements.Contains name ->
        TagSelfClosing(name,attrs)
        |> fun value -> Some {
            token with value = value
        }
    | TagEnd name when 
        TagNames.voidElements.Contains name ->
        None
    | _ -> 
        Some token

let getTag (token:Position<HtmlToken>) =
    match token.value with
    | DocType _ -> "DOCTYPE"
    | Comment _ -> "COMMENT"
    | Text _ -> "TEXT"
    | CData _ -> "CDATA"
    | TagSelfClosing _ -> "TAGSELFCLOSING"
    | TagStart _ -> "TAGSTART"
    | TagEnd _ -> "TAGEND"

let getLexeme (token:Position<HtmlToken>) = box token


