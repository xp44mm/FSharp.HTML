namespace FSharp.HTML

open Xunit
open FSharp.LexYacc

type TokenGenerationTest(output: ITestOutputHelper) =

    [<Fact>]
    member _.``生成-AttributeToken``() =
        let symbols =
            [
                AttributeToken.EQUALS, "="
                AttributeToken.SOL_GT, "/>"
                AttributeToken.GT, ">"
                AttributeToken.LT, "<"
            ]
        let src = DiscriminatedUnions.generate symbols
        output.WriteLine(src)

    [<Fact>]
    member _.``生成-HtmlToken``() =
        let symbols: (HtmlToken * string) list = []
        let src = DiscriminatedUnions.generate symbols
        output.WriteLine(src)

