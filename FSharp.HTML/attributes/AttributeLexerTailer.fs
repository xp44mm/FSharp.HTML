module FSharp.HTML.AttributeLexerTailer

open System
open FSharp.LexYacc
open FSharp.Idioms

let tokenize (iter: LexicalIterator<char * int>) =
    iter
    |> AttributeLexer.anal.split
    |> Seq.collect(fun (rule_id, buff) -> 
        AttributeLexer.actions.[rule_id] buff
        )
