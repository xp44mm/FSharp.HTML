module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals.Literal

let x = """<p>abc<p>eof test<i>itali"""

let [<EntryPoint>] main _ = 
    let res = Compiler.parse x
    let str = res|> List.map HtmlUtils.stringifyNode |> stringify
    Console.WriteLine($"{str}")
    0
