module FSharp.HTML.CdataLexerTailer

open System
open FSharp.LexYacc
open FSharp.Idioms
open System.Text

let section (iter: LexicalIterator<char * int>) =
    let next = CdataLexer.anal.getIterator iter
    let result = StringBuilder(512)
    let rec loop () =
        match next() with
        | None -> ()
        | Some(rule_id, buff) ->
            if buff.Length = 1 then
                result.Append(buff[0]) |> ignore
                loop ()
            else
                // "]]>"
                ()
    loop ()
    result.ToString()
