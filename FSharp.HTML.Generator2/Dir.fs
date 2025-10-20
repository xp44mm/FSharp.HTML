module FSharp.HTML.Dir

open System.IO

let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName

let html = Path.Combine(solutionPath, "FSharp.HTML")

let attributes = Path.Combine(html, "attributes")
