namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal
open FSharp.xUnit
open System

type comb1_DS() =
    static let ds = ArrayDataSource [
        ["f";"f"]  ,[]
        ["f";"t"]  ,[]
        ["f"]      ,[]
        ["t";"f"]  ,["t"]
        ["t";"t"]  ,["t"]
        ["t"]      ,["t"]
    ]
    static member keys = ds.keys
    static member get key = ds.[key]

type comb2_DS() =
    static let ds = ArrayDataSource [
        ["x";"y";"z"]  ,["x";"y"]
        ["x";"y"]  ,["x";"y"]

        ["x"]  ,["x"]
        ["x";"z"]  ,["x"]

        ["y"]  ,["y"]
        ["y";"z"]  ,["y"]

        ["z"],[]
    ]
    static member keys = ds.keys
    static member get key = ds.[key]


type comb2plus1_DS() =
    static let ds = ArrayDataSource [
        ["k";"l";"m";"n"],["k";"l"]
        ["k";"l";"m"]    ,["k";"l"]
        ["k";"l";"n"]    ,["k";"l"]
        ["k";"l"]        ,["k";"l"]

        ["l";"m";"n"],["l"]
        ["l";"m"]    ,["l"]
        ["l";"n"]    ,["l"]
        ["l"]        ,["l"]

        ["k";"m";"n"],["k"]
        ["k";"m"]    ,["k"]
        ["k";"n"]    ,["k"]
        ["k"]        ,["k"]

        ["m";"n"]    ,["m"]
        ["m"]        ,["m"]

        ["n"]        ,[]
    ]
    static member keys = ds.keys
    static member get key = ds.[key]

type comb3plus1_DS() =
    static let ds = ArrayDataSource [
        ["j";"k";"l";"m";"n"],["j";"k";"l"]
        ["j";"k";"l";"m"]    ,["j";"k";"l"]
        ["j";"k";"l";"n"]    ,["j";"k";"l"]
        ["j";"k";"l"]        ,["j";"k";"l"]

        ["j";"l";"m";"n"],["j";"l"]
        ["j";"l";"m"]    ,["j";"l"]
        ["j";"l";"n"]    ,["j";"l"]
        ["j";"l"]        ,["j";"l"]

        ["j";"k";"m";"n"],["j";"k"]
        ["j";"k";"m"]    ,["j";"k"]
        ["j";"k";"n"]    ,["j";"k"]
        ["j";"k"]        ,["j";"k"]

        ["k";"l";"m";"n"],["k";"l"]
        ["k";"l";"m"]    ,["k";"l"]
        ["k";"l";"n"]    ,["k";"l"]
        ["k";"l"]        ,["k";"l"]

        ["l";"m";"n"],["l"]
        ["l";"m"]    ,["l"]
        ["l";"n"]    ,["l"]
        ["l"]        ,["l"]

        ["k";"m";"n"],["k"]
        ["k";"m"]    ,["k"]
        ["k";"n"]    ,["k"]
        ["k"]        ,["k"]

        ["j";"m";"n"],["j"]
        ["j";"m"]    ,["j"]
        ["j";"n"]    ,["j"]
        ["j"]        ,["j"]

        ["m";"n"]    ,["m"]
        ["m"]        ,["m"]

        ["n"]        ,[]
    ]
    static member keys = ds.keys
    static member get key = ds.[key]

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

    [<Fact>]
    member _.``comb1 empty test`` () = 
        let xs = []
        let y = OmittedTagend.comb1 [] xs
        Should.equal y []

    [<Fact>]
    member _.``comb2 empty test`` () = 
        let xs = []
        let y = OmittedTagend.comb2 [] [] xs
        Should.equal y []

    [<Fact>]
    member _.``comb2plus1 empty test`` () = 
        let xs = []
        let y = OmittedTagend.comb2plus1 [] [] [] xs
        Should.equal y []

    [<Fact>]
    member _.``comb3Plus1 empty test`` () = 
        let xs = seq []
        let y = OmittedTagend.comb3plus1 [] [] [] [] xs
        Should.equal y []

    [<Theory>]
    [<MemberData(nameof comb1_DS.keys,MemberType=typeof<comb1_DS>)>]
    member _.``comb1 some data test`` ([<ParamArray>]xs:string[]) = 
        let ys = OmittedTagend.comb1 ["t"] xs
        let es = comb1_DS.get xs
        output.WriteLine(stringify ys)
        Should.equal es ys

    [<Theory>]
    [<MemberData(nameof comb2_DS.keys,MemberType=typeof<comb2_DS>)>]
    member _.``comb2 some data test`` ([<ParamArray>]xs:string[]) = 
        let ys = OmittedTagend.comb2 ["x"] ["y"] xs
        let es = comb2_DS.get xs
        output.WriteLine(stringify ys)
        Should.equal es ys

    [<Theory>]
    [<MemberData(nameof comb2plus1_DS.keys,MemberType=typeof<comb2plus1_DS>)>]
    member _.``comb2plus1 some data test`` ([<ParamArray>]xs:string[]) = 
        let ys = OmittedTagend.comb2plus1 ["k"] ["l"] ["m"] xs
        let es = comb2plus1_DS.get xs
        output.WriteLine(stringify ys)
        Should.equal es ys

    [<Theory>]
    [<MemberData(nameof comb3plus1_DS.keys,MemberType=typeof<comb3plus1_DS>)>]
    member _.``comb3plus1 some data test`` ([<ParamArray>]xs:string[]) = 
        let ys =
            xs
            |> OmittedTagend.comb3plus1 ["j"] ["k"] ["l"] ["m"]
        let es = comb3plus1_DS.get xs
        output.WriteLine(stringify ys)
        Should.equal es ys


    [<Theory>]
    [<MemberData(nameof thead_tbody_tfoot_DS.keys,MemberType=typeof<thead_tbody_tfoot_DS>)>]
    member _.``thead_tbody_tfoot getOmittedTagends`` ([<System.ParamArray>]xs:string[]) =
        let y = 
            xs
            |> OmittedTagend.comb3plus1 ["td";"th"] ["tr"] ["thead";"tbody";"tfoot"] ["colgroup";"caption"] 
        output.WriteLine(stringify y)

    [<Theory>]
    [<MemberData(nameof tr_DS.keys,MemberType=typeof<tr_DS>)>]
    member _.``tr getOmittedTagends`` ([<System.ParamArray>]xs:string[]) =
        let tagends = OmittedTagend.comb2plus1 ["td";"th"] ["tr"] ["colgroup";"caption"]
        let y = tagends xs
        output.WriteLine(stringify y)



