module FSharp.HTML.Compiler

open FSharp.Literals.Literal
open FSharp.Idioms
open FslexFsyacc.Runtime
open System
open System.Reactive
open System.Reactive.Linq

open FSharp.Literals.Literal

//let getTag = HtmlTokenUtils.getTag
//let getLexeme = HtmlTokenUtils.getLexeme

//let parser = 
//    Parser<Position<HtmlToken>>(
//        NodesParseTable.rules,
//        NodesParseTable.actions,
//        NodesParseTable.closures,getTag,getLexeme)
        
//let stateSymbolList = NodesParseTable.theoryParser.getStateSymbolPairs()

//let parseTokens(tokens:seq<Position<HtmlToken>>) =
//    tokens
//    |> parser.parse
//    |> NodesParseTable.unboxRoot

///// consume doctype from tokens
//let preamble (tokens:seq<Position<HtmlToken>>) =
//    let iterator = 
//        tokens.GetEnumerator()
//        |> Iterator

//    let rec loop () =
//        match iterator.tryNext() with
//        | Some {value = TEXT t } when t.Trim() = "" -> loop ()
//        | Some {value = DOCTYPE _ } as sm ->
//            let rest =
//                iterator.tryNext()
//                |> Seq.unfold(
//                    Option.map(fun v -> v,iterator.tryNext())
//                )
//                |> Seq.skipWhile(function
//                    | {value = TEXT t } when t.Trim() = "" -> true
//                    | _ -> false
//                )
//            sm,rest
//        | maybe -> 
//            let rest =
//                maybe
//                |> Seq.unfold(
//                    Option.map(fun v -> v,iterator.tryNext())
//                )
//            None,rest
//    loop ()
///// Locate doctype at the beginning of tokens
//let consumeDoctype tokens =
//    let maybeDoctype, tokens = 
//        tokens
//        |> preamble
//    let docType = 
//        maybeDoctype
//        |> Option.map(function
//            | {value=DOCTYPE dt} -> dt
//            | _ -> "html"
//        )
//        |> Option.defaultValue "html"
//    docType,tokens


/////尝试在状态栈中寻找第一个开始标签（不平衡的）
//let tryFindTagStart (states:list<int*obj>) =
//    let rec loop (balance:int)(rest:list<int*obj>) =
//        match rest with
//        | [] -> None
//        | (i,lexeme)::tail ->
//            match stateSymbolList.[i] with
//            | "TAGSTART" ->
//                let balance = balance + 1
//                if balance > 0 then
//                    let name,attrs =
//                        lexeme
//                        |> unbox<_>
//                        |> HtmlNodeCreator.getNameAttributes
//                    Some(name,attrs)
//                else
//                    loop balance tail
//            | "TAGEND" ->
//                loop (balance-1) tail
//            | _ ->
//                loop balance tail
//    loop 0 states

//let stringifyStates states =
//    let symbols =
//        states
//        |> List.map(fun(i,_)-> $"{stateSymbolList.[i]}")
//    stringify symbols

//let parse (tokens:seq<Position<HtmlToken>>) =
//    let mutable states = [0,null]

//    //omitted TagEnd complement omitted
//    let complementOmmittedTagEnd (tokens:seq<Position<HtmlToken>>) =
//        let iterator =
//            tokens.GetEnumerator()
//            |> RetractableIterator

//        let rec loop () =
//            seq {
//                match iterator.tryNext() with
//                | None -> 
//                    match tryFindTagStart states with
//                    | None -> ()
//                    | Some(bnm,attrs) ->
//                        yield { 
//                            index = -1
//                            length = 0
//                            value = TAGEND bnm }
//                        yield! loop ()
//                | Some ({value=TAGEND enm}as baseline) ->
//                    match tryFindTagStart states with
//                    | None ->
//                        iterator.dequeue(1) |> ignore
//                        yield {
//                            baseline with value = TEXT $"</{enm}>"} // tok.getRaw(inp)
//                        yield! loop ()
//                    | Some(bnm,attrs) ->
//                        if enm = bnm then
//                            yield iterator.dequeueHead()
//                            yield! loop ()
//                        else
//                            yield { baseline with 
//                                        length = 0
//                                        value = TAGEND bnm}
//                            iterator.dequequeNothing()
//                            yield! loop ()
//                | Some ({value=TAGSTART (name,_)|TAGSELFCLOSING (name,_)}as baseline) ->
//                    match tryFindTagStart states with
//                    // 有未闭合的，
//                    | Some(bnm,attrs) when 
//                        TagNames.follows.ContainsKey bnm && 
//                        TagNames.follows.[bnm].Contains name ->
//                            //insert 虚拟tok
//                            yield { baseline with
//                                        length = 0
//                                        value = TAGEND bnm}
//                            iterator.dequequeNothing()
//                            yield! loop ()
//                    | _ ->
//                        // 没有未闭合的，继续正常流程
//                        yield iterator.dequeueHead() // tok
//                        yield! loop ()
//                | _ ->
//                    yield iterator.dequeueHead() // tok
//                    yield! loop ()
//            }

//        loop ()

//    tokens
//    |> complementOmmittedTagEnd
//    |> Seq.iter(fun tok ->
//        Console.WriteLine($"tok:{stringify tok}")
//        let nextStates = parser.shift(states,tok)
//        states <- nextStates
//        )

//    match parser.tryReduce(states) with
//    | Some nextStates ->
//        states <- nextStates
//        Console.WriteLine($"Accept{stringifyStates states}")
//    | None ->
//        Console.WriteLine(stringify states)
//        ()

//    if parser.isAccept states then
//        match states with
//        |[(1,lxm);0,null] -> lxm |> unbox<HtmlNode list>
//        | _ -> failwith $"states:{stringifyStates states}\r\ntok:EOF"
//    else failwith $"states:{stringifyStates states}"

