namespace FSharp.HTML

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.LexYacc.Bootstrap
open FSharp.LexYacc

open System.IO
open System.Text

type CommentLexerTest(output: ITestOutputHelper) as this =
    let name =
        let x = this.GetType().Name
        x.Substring(0, x.Length - 4)

    let srcPath = Path.Combine(__SOURCE_DIRECTORY__, "comment.fslex")

    let text = File.ReadAllText(srcPath, Encoding.UTF8)

    [<Fact>] // (Skip="掠过生成")
    member _.``生成``() =
        let printer = FslexFilePrinter.forChar text
        let src = printer.print()
        output.WriteLine(src)

        let path = Path.Combine(Dir.html, name + ".fs")
        File.WriteAllText(path, src, Encoding.UTF8)
        output.WriteLine($"output:\r\n{path}")

        ()
