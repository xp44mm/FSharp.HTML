module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

let x = [
    TagStart("p",[]);Text "Geckos are a group. ";TagEnd "p";TagStart("p",[]);TagEnd "p";EOF]



let [<EntryPoint>] main _ = 0
