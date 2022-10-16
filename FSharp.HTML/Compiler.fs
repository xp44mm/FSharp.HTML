module FSharp.HTML.Compiler

open FSharp.Literals.Literal
open FSharp.Idioms
open FslexFsyacc.Runtime
open System

let getTag = HtmlTokenUtils.getTag
let getLexeme = HtmlTokenUtils.getLexeme

let parser = 
    Parser<Position<HtmlToken>>(
        NodesParseTable.rules,
        NodesParseTable.actions,
        NodesParseTable.closures,getTag,getLexeme)

let parseTokens(tokens:seq<Position<HtmlToken>>) =
    tokens
    |> parser.parse
    |> NodesParseTable.unboxRoot


///尝试在状态栈中寻找第一个开始标签
let tryFindTagStart (states:list<int*obj>) =
    let rec loop (balance:int)(rest:list<int*obj>) =
        match rest with
        | [] -> None
        | (i,lexeme)::tail ->
            match parser.getSymbol i with
            | "TAGSTART" ->
                let balance = balance + 1
                if balance > 0 then
                    let name,attrs =
                        lexeme
                        |> unbox<_>
                        |> HtmlNodeCreator.getNameAttributes
                    Some(name,attrs)
                else
                    loop balance tail
            | "TAGEND" ->
                loop (balance-1) tail
            | _ ->
                loop balance tail
    loop 0 states

let stringifyStates states =
    let symbols =
        states
        |> List.map(fun(i,_)-> $"{parser.getSymbol i}")
    stringify symbols

let parse (tokens:seq<Position<HtmlToken>>) =
    let mutable states = [0,null]

    //omitted TagEnd complement omitted
    let complementOmmittedTagEnd (tokens:seq<Position<HtmlToken>>) =
        let iterator =
            tokens.GetEnumerator()
            |> RetractableIterator

        let rec loop () =
            seq {
                match iterator.tryNext() with
                | None -> 
                    match tryFindTagStart states with
                    | None -> ()
                    | Some(bnm,attrs) ->
                        yield { 
                            index = -1
                            length = 0
                            value = TagEnd bnm }
                        yield! loop ()
                | Some ({value=TagEnd enm}as baseline) ->
                    match tryFindTagStart states with
                    | None ->
                        iterator.consume() |> ignore
                        yield {
                            baseline with value = Text $"</{enm}>"} // tok.getRaw(inp)
                        yield! loop ()
                    | Some(bnm,attrs) ->
                        if enm = bnm then
                            yield iterator.consume()
                            yield! loop ()
                        else
                            yield { baseline with 
                                        length = 0
                                        value = TagEnd bnm}
                            iterator.restart()
                            yield! loop ()
                | Some ({value=TagStart (name,_)|TagSelfClosing (name,_)}as baseline) ->
                    match tryFindTagStart states with
                    // 有未闭合的，
                    | Some(bnm,attrs) when 
                        TagNames.follows.ContainsKey bnm && 
                        TagNames.follows.[bnm].Contains name ->
                            //insert 虚拟tok
                            yield { baseline with
                                        length = 0
                                        value = TagEnd bnm}
                            iterator.restart()
                            yield! loop ()
                    | _ ->
                        // 没有未闭合的，继续正常流程
                        yield iterator.consume() // tok
                        yield! loop ()
                | _ ->
                    yield iterator.consume() // tok
                    yield! loop ()
            }

        loop ()

    tokens
    |> complementOmmittedTagEnd
    |> Seq.iter(fun tok ->
        Console.WriteLine($"tok:{stringify tok}")
        let nextStates = parser.shift(states,tok)
        states <- nextStates
        )

    match parser.tryReduce(states) with
    | Some nextStates ->
        states <- nextStates
        Console.WriteLine($"Accept{stringifyStates states}")
    | None ->
        Console.WriteLine(stringify states)
        ()

    if parser.isAccept states then
        match states with
        |[(1,lxm);0,null] -> lxm |> unbox<HtmlNode list>
        | _ -> failwith $"states:{stringifyStates states}\r\ntok:EOF"
    else failwith $"states:{stringifyStates states}"
