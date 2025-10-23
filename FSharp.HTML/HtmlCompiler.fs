module FSharp.HTML.HtmlCompiler

open System

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.LexYacc

let tokenRow tok =
    { token = tok; tag = HtmlToken.tag tok; lexeme = HtmlToken.lexeme tok }

let getSymbols states =
    states
    |> List.map(fun s -> HtmlYacc.stateSymbols.[s.state])

/// 不检查错误的reduce, todo add to ParseTable
let reduce states token =
    let tok = Some token
    let rec loop states =
        match HtmlYacc.parser.nextAction states tok with
        | NextParseAction.Reduced nextStates -> loop nextStates
        | NextParseAction.Shifted _ -> states
        | _ -> states
    loop states

/// 强制reduce 用node 的一个合法follow Token，其实也是node的first token。
let forceReduce (states: StateInParseStack list) =
    let ws = tokenRow(HtmlToken.TEXT "")
    reduce states ws

let getOpeningsAll (states: StateInParseStack list) =
    let states = forceReduce states

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
                let tagname, _ = unbox<string * list<string * string>> s.lexeme
                let openings = tagname :: openings
                if tagname = name then
                    openings
                else
                    loop openings tail
            else
                loop openings tail
        | [] -> openings

    let states = forceReduce states
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

let compileText (input: string) =
    let tokens =
        input.ToCharArray()
        |> Array.map(fun ch -> ch, int ch)
        |> ArrayStack
        |> LexicalIterator.ofArrayStack
        |> HtmlLexerTailer.tokenize

    let mutable states = [ { state = 0; lexeme = null } ]

    seq {
        yield! tokens
        yield EOF
    }
    |> Seq.collect(fun tok ->
        match tok with
        | EOF ->
            let closingTags =
                states
                |> getOpeningsAll
                |> List.map(fun tag -> TAGEND tag)
            closingTags @ [ EOF ]

        | TAGEND tag ->
            states
            |> getOpenings tag
            |> List.map(fun tag -> TAGEND tag)

        | _ -> [ tok ]
    )
    |> Seq.map(fun tok ->
        //Console.WriteLine($"token:{stringify tok}")
        { token = tok; tag = HtmlToken.tag tok; lexeme = HtmlToken.lexeme tok }
    )
    |> Seq.iter(fun tok -> states <- HtmlYacc.parser.shift states tok)

    //Console.WriteLine($"states:{stringify symbols}")

    match HtmlYacc.parser.tryReduceBeforeAccept(states) with
    | Some reducedstates -> states <- reducedstates
    | None -> ()

    match states with
    | [ { state = 1; lexeme = lxm }; { state = 0; lexeme = null } ] ->
        HtmlYacc.unboxRoot lxm
    | _ -> 
        let symbols = getSymbols states
        failwith $"{stringify symbols}"
