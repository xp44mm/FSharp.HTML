namespace FSharp.HTML

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.LexYacc
open FSharp.LexYacc.Bootstrap

open System.IO
open System.Text

type AttributeYaccTest(output: ITestOutputHelper) as this =
    let name = 
        let x = this.GetType().Name
        x.Substring(0, x.Length - 4)

    let srcPath =
        Path.Combine(__SOURCE_DIRECTORY__, "attribute.fsyacc")

    let text = File.ReadAllText(srcPath, Encoding.UTF8)

    [<Fact>] // (Skip = "生成")
    member _.``生成Yacc文件``() =
        let rawFsyacc = FsyaccCompiler.compile text
        let flatFsyacc = FlatFsyaccFile.from rawFsyacc

        let printer = FsyaccFilePrinter.from flatFsyacc
        let src = printer.printYaccModule(flatFsyacc.header)
        //output.WriteLine(src)

        let path =
            Path.Combine(Dir.attributes, name + ".fs")
        File.WriteAllText(path, src, Encoding.UTF8)
        output.WriteLine($"output:\r\n{path}")

        ()
