module FSharp.HTML.HtmldocCompiler

open FSharp.Literals.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime

let parser =
    Parser<Position<HtmlToken>>(
        HtmldocParseTable.rules,
        HtmldocParseTable.actions,
        HtmldocParseTable.closures,

        HtmlTokenUtils.getTag,
        HtmlTokenUtils.getLexeme)

let stateSymbolList = HtmldocParseTable.theoryParser.getStateSymbolPairs()

///从状态中获取未封闭开始标签。
let unclosedTagStarts (states:list<int*obj>) =
    let states =
        parser.tryReduce(states,{index=0;length=0;value=COMMENT ""}) //use lookahead eof to reduce tagstart tagend.
        |> Option.defaultValue states

    states
    |> Seq.map(fun(i,o)->stateSymbolList.[i],o)
    |> Seq.filter(fun(sym,l)-> sym = "TAGSTART")
    |> Seq.map(
        snd
        >> unbox<string*(string*string)list>
        >> fst
        )

/// 解析文本为结构化数据
let compile (input:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    HtmlTokenUtils.tokenize TagLeftCompiler.compile 0 input
    |> Seq.choose HtmlTokenUtils.unifyVoidElement
    |> Seq.filter(fun tok ->
        // 删除文件根部的自由空白
        match tok.value with
        | TEXT x when x.Trim() = "" ->
            states
            |> unclosedTagStarts
            |> Seq.isEmpty
            |> not
        | _ -> true
    )
    |> Seq.collect(fun tok ->
        //不可以柯里化，否则会错误地缓存states快照
        states
        |> unclosedTagStarts
        |> OmittedTagend.insertOmittedTagend <| tok
    )
    //|> Seq.map(fun lookahead ->
    //    match parser.tryReduce(states,lookahead) with
    //    | Some reducedstates -> states <- reducedstates
    //    | None -> ()
    //    lookahead
    //)
    |> Seq.iter(fun tok ->
        tokens <- tok :: tokens
        states <- parser.shift(states,tok)
    )

    //match parser.tryReduce(states) with
    //| Some reducedstates -> states <- reducedstates
    //| None -> ()

    match parser.accept states with
    |[1,lxm; 0,null] ->
        HtmldocParseTable.unboxRoot lxm
    | _ ->
        failwith $"a:{stringify tokens}\r\n{stringify states}"


