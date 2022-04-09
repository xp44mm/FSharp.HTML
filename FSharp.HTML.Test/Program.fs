module FSharp.HTML.Program
open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

let endTag = set ["caption";"colgroup";"table";"tbody";"tfoot";"thead";"tr"]
let selfClosingTag = set ["caption";"colgroup";"tbody";"tfoot";"thead";"tr"]
let startTag = set ["table";"tbody";"tfoot";"thead";"tr"]
let id = set ["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT";"WS"]

let [<EntryPoint>] main _ = 
    0
