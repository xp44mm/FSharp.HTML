module FSharp.HTML.CharRefLexerTailer


open System
open FSharp.LexYacc
open FSharp.Idioms
open FSharp.Idioms.Literal

/// 
let decode (buff: char list) =
    buff
    |> List.toArray
    |> Array.map(fun ch -> ch, int ch)
    |> ArrayStack
    |> LexicalIterator.ofArrayStack
    |> CharRefLexer.anal.split
    |> Seq.map(fun (rule_id, buff) ->
        let action = CharRefLexer.actions.[rule_id]
        action buff
    )
    |> String.concat ""
