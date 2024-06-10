namespace FSharp.HTML

type TagLeftToken =
    | LANGLE of tag:string
    | RANGLE // >
    | DIV_RANGLE // />
    | ATTR_NAME of string
    | ATTR_VALUE of string

open FslexFsyacc

open FSharp.Idioms.Literal

open FSharp.Idioms
open FSharp.Idioms.ActivePatterns
open TryTokenizer

module TagLeftToken =
    let getTag (token:Position<TagLeftToken>) =
        match token.value with
        | LANGLE _ -> "LANGLE"
        | RANGLE -> "RANGLE"
        | DIV_RANGLE -> "DIV_RANGLE"
        | ATTR_NAME _ -> "ATTR_NAME"
        | ATTR_VALUE _ -> "ATTR_VALUE"

    let getLexeme (token:Position<TagLeftToken>) = 
        match token with
        | {index=i;value=LANGLE tagname }-> box(i,tagname)
        | {value=ATTR_NAME  s }-> box s
        | {value=ATTR_VALUE s }-> box s
        | {value=RANGLE      } as postok -> box postok.nextIndex
        | {value=DIV_RANGLE  } as postok -> box postok.nextIndex

    let tokenize (offset:int) (inp:string) = // (restloop) 
        let rec loop (i:int) (rest:string) = seq {
            match rest with
            | Rgx @"^<([-:\.\w]+)" x ->
                let tagName = x.Groups.[1].Value.ToLower()
                yield { index= offset;length= x.Length;value= LANGLE tagName }
                yield! loop (i+x.Length) (x.Result("$'"))

            | On tryWS x ->
                yield! loop (i+x.Length) (x.Result("$'"))

            | Rgx @"^[\S-[""'=>/]]+" x ->
                yield { index=i;length=x.Length;value=ATTR_NAME x.Value }
                yield! loop (i+x.Length) (x.Result("$'"))

            | Rgx @"^=\s*([\S-[""'=><`]]+)" x -> // Unquoted Attribute Value
                yield { index=i;length=x.Length;value=ATTR_VALUE x.Groups.[1].Value }
                yield! loop (i+x.Length) (x.Result("$'"))

            | Rgx @"^=\s*([""'])([^\1]*?)\1" x -> // Quoted Attribute Value
                yield { index=i;length=x.Length;value=ATTR_VALUE x.Groups.[2].Value }
                yield! loop (i+x.Length) (x.Result("$'"))

            | StartsWith "/>" x ->
                let postok = { index=i;length=x.Length;value=DIV_RANGLE }
                //pickRestData postok.nextIndex rest.[postok.length..]
                yield postok

            | StartsWith ">" x ->
                let postok = { index=i;length=x.Length;value=RANGLE }
                //pickRestData postok.nextIndex rest.[postok.length..]
                yield postok

            | rest -> failwith $"{rest}"
        }
        loop offset inp
