module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal
open FslexFsyacc.Runtime
open FSharp.Idioms
open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc.Runtime


[<EntryPoint>]
let main _ = 
    let x = "<style>"
    let postok = TagLeftCompiler.compile 0 x
    Console.WriteLine(stringify postok)


    0
