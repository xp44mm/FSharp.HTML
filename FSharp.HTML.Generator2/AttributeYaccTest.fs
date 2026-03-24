namespace FSharp.HTML

open Xunit
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

    [<Fact(Skip = "没有改变，不必重复生成")>] //
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
        
    [<Fact>]
    member _.``验证``() =
        // actual
        let stateSymbols = AttributeYacc.stateSymbols
        let actions = AttributeYacc.actions
        let rules = AttributeYacc.rules

        // expect
        let rawFsyacc = FsyaccCompiler.compile text
        let flatFsyacc = FlatFsyaccFile.from rawFsyacc
        let yaccFile = FsyaccFilePrinter.from flatFsyacc

        // compare
        Should.equal yaccFile.stateSymbols stateSymbols
        Should.equal yaccFile.actions actions

        let productions1 = yaccFile.rules |> List.map fst
        let productions2 = rules |> List.map fst
        Should.equal productions1 productions2

        // todo: header 比较
        // todo: actions 比较

        ()
