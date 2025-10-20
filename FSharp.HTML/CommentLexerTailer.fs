module FSharp.HTML.CommentLexerTailer

open System
open FSharp.LexYacc
open FSharp.Idioms
open System.Text

let restComment (iter: LexicalIterator<char * int>) =
    let next = CommentLexer.anal.getIterator iter
    let result = StringBuilder(1024)
    let rec loop () =
        match next() with
        | None -> ()
        | Some(rule_id, buff) ->
            //let act = CommentLexer.actions.[rule_id]
            if buff.Length = 1 then
                result.Append(buff[0]) |> ignore
                loop ()
            else
                //result.Append(buff) |> ignore
                ()
    loop ()
    result.ToString()
