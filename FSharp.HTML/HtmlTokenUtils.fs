module FSharp.HTML.HtmlTokenUtils
open FSharp.Literals
open FSharp.Idioms
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime

open FSharp.Idioms.RegularExpressions
open FSharp.Idioms.ActivePatterns

open System.Text.RegularExpressions
open TryTokenizer

let unifyVoidElement(token:Position<HtmlToken>) =    
    match token.value with
    | TAGSTART (name,attrs) when 
        TagNames.voidElements.Contains name ->
        TAGSELFCLOSING(name,attrs)
        |> fun value -> Some {
            token with 
                value = value
        }
    | TAGEND name when 
        TagNames.voidElements.Contains name ->
        None
    | _ -> 
        Some token

let getTag (token:Position<HtmlToken>) =
    match token.value with
    | EOF -> "EOF"
    | DOCTYPE        _ -> "DOCTYPE"
    | COMMENT        _ -> "COMMENT"
    | TEXT           _ -> "TEXT"
    | CDATA          _ -> "CDATA"
    | TAGSELFCLOSING _ -> "TAGSELFCLOSING"
    | TAGSTART       _ -> "TAGSTART"
    | TAGEND         _ -> "TAGEND"

let getLexeme (token:Position<HtmlToken>) = 
    match token with
    | {value=EOF       } -> null
    | {value=DOCTYPE s } -> box s
    | {value=TEXT    s } -> box s
    | {value=COMMENT s } -> box s
    | {value=CDATA   s } -> box s
    | {value=TAGEND  s } -> box s
    | {value=TAGSELFCLOSING (nm,attrs)} -> box (nm,attrs)
    | {value=TAGSTART       (nm,attrs)} -> box (nm,attrs)

let tokenizeEntry getTagLeft (offset:int) (inp:string) =
    match inp with
    | "" -> { index= offset;length= 0;value= EOF }
    | Search(Regex(@"^<!DOCTYPE\s+([^>]*)>",RegexOptions.IgnoreCase)) x ->
        let y = x.Groups.[1].Value.TrimEnd()
        { index= offset;length= x.Length;value= DOCTYPE y }

    | Rgx @"^<!--([\s\S]*?)-->" x ->
        let y = x.Groups.[1].Value
        { index= offset;length= x.Length;value= COMMENT y }

    | Search(Regex(@"^<!\[CDATA\[([\s\S]*?)\]\]>",RegexOptions.IgnoreCase)) x ->
        let y = x.Groups.[1].Value
        { index= offset;length= x.Length;value= CDATA y }

    | Rgx @"^</([-:\.\w]+)\s*>" x ->
        let tagName = x.Groups.[1].Value.ToLower()
        { index= offset;length= x.Length;value= TAGEND tagName }

    | Rgx @"^<[-:\.\w]+" _ ->
        getTagLeft offset inp
    | Rgx @"^[^<]+" x -> //| On tryText x ->
        { index= offset;length= x.Length;value= TEXT x.Value }

    | _ -> failwith inp

let tokenizeRaw (restloop) (offset:int) (inp:string) =
    let rgx = Regex($@"^([\s\S]*?)(</(\w+)\s*>)",RegexOptions.IgnoreCase)
    let m = rgx.Match(inp)
    let gps = m.Groups
    seq {
        match gps.[1].Value with
        | "" -> ()
        | s -> yield { index=offset;length= s.Length;value= TEXT s }

        let tagName= gps.[3].Value.ToLower()
        let pt2 = { index=offset+gps.[2].Index;length= gps.[2].Length;value= TAGEND tagName }
        yield pt2

        yield! restloop pt2.nextIndex (m.Result("$'"))
    }

///
let tokenizeCss (restloop) (offset:int) (inp:string) =
    let rec loop i rest =
        seq {
            match rest with
            | "" -> failwith "eof"
            | Search(Regex(@"^</style\s*>",RegexOptions.IgnoreCase)) m ->
                let postok = { index= i;length= m.Length;value= TAGEND "style" }
                yield postok
                yield! restloop postok.nextIndex rest.[postok.length..]

            | On tryMultiLineComment m
            | On tryDoubleStringLiteral m
            | Rgx "^[^/\"<]+" m -> 
                let postok = { index= i;length= m.Length;value= TEXT m.Value }
                yield postok
                yield! loop postok.nextIndex rest.[postok.length..]

            | x -> 
                let postok = { index= i;length= 1;value= TEXT x.[0..0] }
                yield postok
                yield! loop postok.nextIndex rest.[postok.length..]

        }
    loop offset inp

//已知问题：当正则表达式常量其中包括结束标记，如/</script>/时，会错误提前退出。
let tokenizeJavaScript (restloop) (offset:int) (inp:string) =
    let rec loop i rest =
        seq {
            match rest with
            | "" -> failwith "eof"
            | Search(Regex(@"^</script\s*>",RegexOptions.IgnoreCase)) m ->
                let postok = { index= i;length= m.Length;value= TAGEND "script" }
                yield postok
                yield! restloop postok.nextIndex rest.[postok.length..]

            | On tryDoubleStringLiteral m
            | On trySingleStringLiteral m
            | On tryGraveAccent m
            | On trySingleLineComment m
            | On tryMultiLineComment m
            | Rgx "^[^\"'`/<]+" m -> 
                let postok = { index= i;length= m.Length;value= TEXT m.Value }
                yield postok
                yield! loop postok.nextIndex rest.[postok.length..]

            | x -> 
                let postok = { index= i;length= 1;value= TEXT x.[0..0] }
                yield postok
                yield! loop postok.nextIndex rest.[postok.length..]
        }
    loop offset inp

let tokenize getTagLeft (offset:int) (input:string) =
    let rec loop i (rest:string) =
        seq {
            let postok = tokenizeEntry getTagLeft i rest
            yield postok

            match postok.value with
            | EOF -> ()
            | TAGSTART(("textarea" | "title"),_) ->
                yield! tokenizeRaw loop postok.nextIndex rest.[postok.length..]
            | TAGSTART("script",_) -> 
                yield! tokenizeJavaScript loop postok.nextIndex rest.[postok.length..]
            | TAGSTART("style",_)  -> 
                yield! tokenizeCss loop postok.nextIndex rest.[postok.length..]
            | _ ->
                yield! loop postok.nextIndex rest.[postok.length..]
        }

    loop offset input
