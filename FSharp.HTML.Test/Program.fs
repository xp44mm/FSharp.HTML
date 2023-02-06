module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals.Literal
open FslexFsyacc.Runtime
open FSharp.Idioms

let iterateTagStarts (states:list<string>) =
    states
    |> Seq.filter(fun s ->
        //Console.WriteLine($"filter:{s}")
        match s with
        | "TAGSTART" | "TAGEND" -> true
        | _ -> false
    )
    |> (fun sq ->
        let iterator =
            RetractableIterator(sq.GetEnumerator())
        seq {
            match iterator.tryNext() with
            | Some s0 when s0 = "TAGEND" ->
                Console.WriteLine($"s0:{s0}")
                match iterator.tryNext() with
                | Some s1 when s1 = "TAGSTART" ->
                    Console.WriteLine($"s1:{s1}")
                    iterator.dequeue(2) |> ignore
                | _ -> failwith $"orphan end tag."
            | _ -> iterator.dequequeNothing()

            while iterator.ongoing() do
                match iterator.tryNext() with
                | Some ss ->
                    Console.WriteLine($"rest:{ss}")
                    yield iterator.dequeueHead()
                | None -> ()
        }
    )
    //|> Seq.map(
    //        snd
    //        >> unbox<string*(string*string)list>
    //        >> fst
    //    )
let states = ["TAGEND";"{node*}";"TAGSTART";"{node+}";"TAGSTART";""]
let [<EntryPoint>] main _ = 
    states
    |> iterateTagStarts
    |> Seq.iter(fun s -> Console.WriteLine($"iter:{s}"))
    0
