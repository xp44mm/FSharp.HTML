module FSharp.HTML.HtmlLexerTailer

open System

open FSharp.LexYacc
open FSharp.LexYacc.Bootstrap

/// 分析*.fslex文件
let tokenize (iter: LexicalIterator<char * int>) =
    let comment () = CommentLexerTailer.restComment iter

    let cdata () = CdataLexerTailer.section iter

    let attr buff = AttributeCompiler.compile2 buff

    let actions = HtmlLexer.actions comment cdata attr

    let tryNext = HtmlLexer.anal.getIterator iter

    let rec loop () =
        seq {
            match tryNext() with
            | None -> ()
            | Some(rule_id, buff) ->
                let action = actions.[rule_id]
                let tok = action buff
                yield! tok
                yield! loop()
        }
    loop()
