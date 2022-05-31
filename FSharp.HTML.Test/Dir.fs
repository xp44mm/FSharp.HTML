module FSharp.HTML.Dir

open System.IO


let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
let projPath = Path.Combine(solutionPath,@"FSharp.HTML")

let TestData = Path.Combine(__SOURCE_DIRECTORY__,"TestData")
let omitted = Path.Combine(TestData,"omitted")
let wellformed = Path.Combine(TestData,"wellformed")

