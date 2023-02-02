module FSharp.HTML.HtmldocCompiler

open FSharp.Literals.Literal
open FSharp.Idioms
open FslexFsyacc.Runtime
open System
open System.Reactive
open System.Reactive.Linq

let getTag = HtmlTokenUtils.getTag

let getLexeme (token:Position<HtmlToken>) = 
    match token.value with
    | EOF -> null
    | DOCTYPE s -> box s
    | TEXT    s -> box s
    | COMMENT s -> box s
    | CDATA   s -> box s
    | TAGEND  s -> box s
    | TAGSELFCLOSING (nm,attrs) -> box (nm,attrs)
    | TAGSTART       (nm,attrs) -> box (nm,attrs)

let parser = 
    Parser<Position<HtmlToken>>(
        HtmldocParseTable.rules,
        HtmldocParseTable.actions,
        HtmldocParseTable.closures,getTag,getLexeme)
        
let stateSymbolList = HtmldocParseTable.theoryParser.getStateSymbolPairs()

///尝试在状态栈中寻找第一个开始标签（不平衡的），获取这个标签的名字
let tryFindTagStart (states:list<int*obj>) =
    let rec loop (balance:int)(rest:list<int*obj>) =
        match rest with
        | [] -> None
        | (i,lexeme) :: tail ->
            match stateSymbolList.[i] with
            | "TAGSTART" ->
                let balance = balance + 1
                if balance > 0 then
                    let nm =
                        lexeme
                        |> unbox<string*(string*string)list>
                        |> fst
                    Some(nm)
                else
                    loop balance tail
            | "TAGEND" ->
                loop (balance-1) tail
            | _ ->
                loop balance tail
    loop 0 states


/// 解析文本为结构化数据
let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    let mutable result = defaultValue<_>

    //检测标签是否封闭
    let insertOmittedTagEnd(tokens:seq<Position<HtmlToken>>) =
        let iterator =
            RetractableIterator(tokens.GetEnumerator())

        seq {
            while iterator.ongoing() do
                match iterator.tryNext() with
                | None -> ()
                | Some tok ->
                match tok.value with
                | EOF ->
                    match tryFindTagStart states with
                    | None -> yield iterator.dequeueHead()
                    | Some (sname) ->
                        let omittedTagend = {
                            tok with
                                length = 0
                                value = TAGEND sname
                        }
                        iterator.dequequeNothing()
                        yield omittedTagend
                | TAGEND nm ->
                    match tryFindTagStart states with
                    | Some (sname) ->
                        if nm = sname then
                            yield iterator.dequeueHead()
                        else
                            let omittedTagend = {
                                tok with
                                    length = 0
                                    value = TAGEND sname
                            }
                            iterator.dequequeNothing()
                            yield omittedTagend
                    | None -> failwith $"多余的结束标签</{nm}>"
                | _ -> yield iterator.dequeueHead()
        }
    //try

    txt
    |> Tokenizer.tokenize
    |> Seq.skipWhile(fun tok ->
        match tok.value with
        | TEXT x when x.Trim() = "" -> true
        | _ -> false
    )
    |> Seq.choose HtmlTokenUtils.unifyVoidElement
    |> insertOmittedTagEnd
    |> Seq.map(fun tok ->
        tokens <- tok :: tokens
        tok
    )
    |> Seq.map(fun lookahead ->
        match parser.tryReduce(states,lookahead) with
        | Some reducedstates -> states <- reducedstates
        | None -> ()
        lookahead
    )
    |> Seq.iter(fun lookahead ->
        states <- parser.shift(states,lookahead)
    )

    //with _ -> failwith $"b:{stringify tokens}\r\n{stringify states}"

    match parser.tryReduce(states) with
    | Some reducedstates -> states <- reducedstates
    | None -> ()

    match states with
    |[1,lxm; 0,null] ->
        try
        result <- HtmldocParseTable.unboxRoot lxm
        with _ -> failwith $"{stringify lxm}"
    | _ ->
        failwith $"a:{stringify tokens}\r\n{stringify states}"

    result

