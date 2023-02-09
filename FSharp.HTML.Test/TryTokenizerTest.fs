namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open System.IO

open FSharp.Literals
open FSharp.xUnit

type TryTokenizerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 = DOCTYPE``() =
        let x = "<!DOCTYPE html>"
        let y = TryTokenizer.tryDOCTYPE x |> fun y -> fst y.Value
        show y
        Should.equal y x
        
    [<Fact>]
    member _.``02 = Comment``() =
        let x = "<!-- where is this comment in the DOM? -->"
        let y = TryTokenizer.tryComment x |> fun y -> fst y.Value
        show y
        Should.equal y x

    [<Fact>]
    member _.``03 = CDATA``() =
        let x = "<![CDATA[x<y]]>"
        let y = TryTokenizer.tryCDATA x |> fun y -> fst y.Value
        show y
        Should.equal y x

    [<Fact>]
    member _.``04 = tryAttributeName``() =
        let xs = "read-only"
        let ys = TryTokenizer.tryAttributeName xs |> fun y -> fst y.Value
        show ys
        Should.equal ys xs

    [<Fact>]
    member _.``05 = tryUnquotedAttributeValue``() =
        let xs = "=*/"
        let ys = TryTokenizer.tryUnquotedAttributeValue xs |> fun y -> fst y.Value
        show ys
        Should.equal ys xs

    [<Fact>]
    member _.``06 = tryQuotedAttributeValue``() =
        let xs = """='"'"""
        let ys = TryTokenizer.tryQuotedAttributeValue xs |> fun y -> fst y.Value
        show ys
        Should.equal ys xs

        let xd = """="'" """.Trim()
        let yd = TryTokenizer.tryQuotedAttributeValue xd |> fun y -> fst y.Value
        show yd
        Should.equal yd xd
