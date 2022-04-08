module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

let precedes = [
    "</caption>";
    "<caption/>";
    "</colgroup>";
    "<colgroup/>";

    "</thead>";
    "<thead/>";
    "<thead>"
    "</tbody>";
    "<tbody/>";
    "<tbody>";
    "</tfoot>";
    "<tfoot/>";
    "<tfoot>";

    "<table>";
    ]





let [<EntryPoint>] main _ = 0
