module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal
open FslexFsyacc
open FSharp.Idioms
open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Idioms.Literal
open FSharp.xUnit
open FSharp.LexYacc


[<EntryPoint>]
let main _ = 
    let buff = [ '<'; '/'; 'd'; 'i'; 'v'; '>' ]
    let str = String(buff |> List.skip 2 |> Array.ofList)
    let name = str.Substring(0, str.Length - 1)
    Console.WriteLine($"{name}")

    0
