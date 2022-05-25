module FSharp.HTML.Compiler

open System
open System.Text.RegularExpressions

open FSharp.Literals.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime

let parser = PrecNodesParseTable.parser
let getTag = HtmlTokenUtils.getTag
let getLexeme = HtmlTokenUtils.getLexeme

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
                        |> unbox<string*list<string*string>>
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

let parse txt =
    let mutable states = [0,null]

    //omitted TagEnd complement omitted
    let complementOmmittedTagEnd (tokens:seq<_>) =
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
                        yield TagEnd bnm
                        yield! loop ()
                | Some (TagEnd enm) ->
                    match tryFindTagStart states with
                    | None ->
                        let _ = iterator.consume()
                        yield Text $"</{enm}>"
                        yield! loop ()
                    | Some(bnm,attrs) ->
                        if enm = bnm then
                            yield iterator.consume() // tok
                            yield! loop ()
                        else
                            yield TagEnd bnm
                            iterator.restart()
                            yield! loop ()
                | Some (TagStart (name,_)|TagSelfClosing (name,_)) ->
                    match tryFindTagStart states with
                    // 有未闭合的，
                    | Some(bnm,attrs) when 
                        TagNames.follows.ContainsKey bnm && 
                        TagNames.follows.[bnm].Contains name ->
                            //自动闭合
                            yield TagEnd bnm
                            let _ = iterator.restart()
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

    txt
    |> Tokenizer.tokenize
    |> Seq.filter(HtmlTokenUtils.isVoidTagEnd>>not)
    |> Seq.map HtmlTokenUtils.voidTagStartToSelfClosing
    |> complementOmmittedTagEnd
    |> Seq.iteri(fun i tok ->
        //Console.WriteLine($"tok:{stringify tok}")
        let nextStates = parser.shift(states,tok)
        states <- nextStates
        )

    match parser.tryReduce(states) with
    | Some nextStates ->
        states <- nextStates
        //Console.WriteLine($"Accept{stringifyStates states}")
    | None ->
        //Console.WriteLine(stringify x)
        ()

    if parser.isAccept states then
        match states with
        |[(1,lxm);0,null] -> lxm |> unbox<HtmlNode list>
        | _ -> failwith $"states:{stringifyStates states}\r\ntok:EOF"
    else failwith $"states:{stringifyStates states}"
