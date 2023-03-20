namespace FSharp.HTML

open Xunit
open Xunit.Abstractions
open FSharp.Literals.Literal
open FSharp.xUnit

type thead_tbody_tfoot_DriverTest(output:ITestOutputHelper) =
    static let ds = ArrayDataSource [
        ["td"],["td"]
        ["tr"],["tr"]
        ["thead"],["thead"]
        ["caption"],["caption"]
    ]

    static member keys = ds.keys


    //[<Fact>]
    //member _.``empty test`` () = 
    //    let xs = seq []
    //    let y = thead_tbody_tfoot_Driver.getOmittedTagends xs
    //    Should.equal y []

    //[<Theory>]
    //[<MemberData(nameof thead_tbody_tfoot_DriverTest.keys)>]
    //member _.``getOmittedTagends`` ([<System.ParamArray>]xs:string[]) =
    //    let y = thead_tbody_tfoot_Driver.getOmittedTagends xs
    //    output.WriteLine(stringify y)
