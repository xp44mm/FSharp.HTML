module FSharp.HTML.AttributeCompiler

open System

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.LexYacc

let parseTokens (tokens: seq<AttributeToken>) =
    tokens
    |> Seq.map(fun tok ->
        {
            token = tok
            tag = AttributeToken.tag tok
            lexeme = AttributeToken.lexeme tok
        }
    )
    |> Iterator
    |> AttributeYacc.parser.parse

/// 解析文本为结构化数据
let compile (iter: LexicalIterator<char * int>) =
    iter
    |> AttributeLexerTailer.tokenize
    |> parseTokens
    |> AttributeYacc.unboxRoot

let compile2 (buff: list<char>) =
    buff
    |> List.toArray
    |> Array.map(fun ch -> ch, int ch)
    |> ArrayStack
    |> LexicalIterator.ofArrayStack
    |> compile

