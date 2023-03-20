namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals.Literal
open FSharp.xUnit

type thead_tbody_tfoot_DS() =
    static let ds = ArrayDataSource [
        ["td"],["td"]
        ["tr"],["tr"]
        ["thead"],["thead"]
        ["caption"],["caption"]
    ]

    static member keys = ds.keys

type tr_DS() =
    static let ds = ArrayDataSource [
        ["th"],["th"]
        ["tr"],["tr"]
        ["td";"tr"],["td";"tr"]
        ["caption"],["caption"]
        ["td";"p"],["td"]
    ]

    static member keys = ds.keys

type OmittedTagendTest(output:ITestOutputHelper) =
    let tagends = OmittedTagend.comb2plus1 ["td";"th"] ["tr"] ["colgroup";"caption"]

    [<Fact>]
    member _.``empty test`` () = 
        let xs = seq []
        let y = tagends xs
        Should.equal y []

    [<Theory>]
    [<MemberData(nameof tr_DS.keys,MemberType=typeof<tr_DS>)>]
    member _.``getOmittedTagends`` ([<System.ParamArray>]xs:string[]) =
        let y = tagends xs
        output.WriteLine(stringify y)
