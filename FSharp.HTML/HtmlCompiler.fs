module FSharp.HTML.HtmlCompiler

open System

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.LexYacc

let tokenRow tok = 
    { token = tok; tag = HtmlToken.tag tok; lexeme = HtmlToken.lexeme tok }

let getOpeningsAll (states: StateInParseStack list) =
    let tok = tokenRow HtmlToken.EOF
    let states = 
        HtmlYacc.parser.tryReduceBeforeShift states tok
        |> Option.defaultValue states

    states
    |> List.choose(fun s ->
        if HtmlYacc.stateSymbols.[s.state] = "TAGSTART" then
            let tagname, _ = unbox<string * list<string * string>> s.lexeme
            Some tagname
        else
            None
    )

let getOpenings (name: string) (states: StateInParseStack list) =
    let rec loop openings (states: StateInParseStack list) =
        match states with
        | s :: tail ->
            if HtmlYacc.stateSymbols.[s.state] = "TAGSTART" then
                let tagname, _ =
                    unbox<string * list<string * string>> s.lexeme
                let openings = tagname :: openings
                if tagname = name then
                    openings
                else
                    loop openings tail
            else
                loop openings tail
        | [] -> openings

    let tok = tokenRow (HtmlToken.TAGEND "")
    let states = 
        HtmlYacc.parser.tryReduceBeforeShift states tok
        |> Option.defaultValue states

    loop [] states |> List.rev

let parseTokens (tokens: seq<HtmlToken>) =
    tokens
    |> Seq.map(fun tok ->
        { token = tok; tag = HtmlToken.tag tok; lexeme = HtmlToken.lexeme tok }
    )
    |> Iterator
    |> HtmlYacc.parser.parse

/// 解析文本为结构化数据
let compile (iter: LexicalIterator<char * int>) =
    iter
    |> HtmlLexerTailer.tokenize
    |> parseTokens
    |> HtmlYacc.unboxRoot

let compile2 (input: string) =
    let tokens =
        input.ToCharArray()
        |> Array.map(fun ch -> ch, int ch)
        |> ArrayStack
        |> LexicalIterator.ofArrayStack
        |> HtmlLexerTailer.tokenize

    let mutable states = [ { state = 0; lexeme = null } ]

    seq {
        yield! tokens
        EOF
    }
    |> Seq.collect(fun tok ->
        match tok with
        | EOF ->
            states
            |> getOpeningsAll
            |> List.map(fun tag -> TAGEND tag)
        | TAGEND tag ->
            states
            |> getOpenings tag
            |> List.map(fun tag -> TAGEND tag)
        | _ -> [ tok ]
    )
    |> Seq.map(fun tok ->
        { token = tok; tag = HtmlToken.tag tok; lexeme = HtmlToken.lexeme tok }
    )
    |> Seq.iter(fun tok -> states <- HtmlYacc.parser.shift states tok)

    match HtmlYacc.parser.tryReduceBeforeAccept(states) with
    | Some reducedstates -> states <- reducedstates
    | None -> ()

    match states with
    | [ { state = 1; lexeme = lxm }; { state = 0; lexeme = null } ] ->
        HtmlYacc.unboxRoot lxm
    | _ -> failwith $"{stringify states}"
