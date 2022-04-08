module FSharp.HTML.Dir

open Xunit
open Xunit.Abstractions
open System
open System.IO
open System.Text.RegularExpressions

open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FSharp.Idioms

let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
let projPath = Path.Combine(solutionPath,@"FSharp.HTML")
