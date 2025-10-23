module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Literal

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Idioms.Literal
open FSharp.xUnit
open FSharp.LexYacc


[<EntryPoint>]
let main _ = 
    let x = "\r\n        "
    let z = String.IsNullOrWhiteSpace x
    Console.WriteLine(stringify z)            

    x.ToCharArray()
    |> Array.iter(fun c ->
        Console.WriteLine(stringify c)            
        Console.WriteLine(stringify (Char.IsWhiteSpace c))
    )

    0
