module FSharp.HTML.TagLeftCompiler

open FSharp.Literals.Literal
open FSharp.Idioms
open FSharp.Idioms.ActivePatterns

open FslexFsyacc.Runtime

open TryTokenizer

open System
open System.Text.RegularExpressions

let parser =
    Parser<Position<TagLeftToken>>(
        TagLeftParseTable.rules,
        TagLeftParseTable.actions,
        TagLeftParseTable.closures,

        TagLeftToken.getTag,
        TagLeftToken.getLexeme)

let stateSymbolList = TagLeftParseTable.theoryParser.getStateSymbolPairs()

/// 解析文本为结构化数据
let compile (offset:int) (inp:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    TagLeftToken.tokenize offset inp
    |> Seq.map(fun tok ->
        tokens <- tok :: tokens
        tok
    )
    |> Seq.map(fun lookahead ->
        states <- parser.shift(states,lookahead)
    )
    |> Seq.iter(fun _ -> ())

    match parser.tryReduce(states) with
    | Some reducedstates -> states <- reducedstates
    | None -> ()

    match states with
    |[1,lxm; 0,null] ->
        TagLeftParseTable.unboxRoot lxm
    | _ ->
        failwith $"syntax error:{stringify tokens}\r\n{stringify states}"

