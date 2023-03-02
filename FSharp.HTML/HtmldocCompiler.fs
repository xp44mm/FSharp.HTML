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

///从状态中获取开始标签，其未封闭。
let getTagStarts (states:list<int*obj>) =
    let iterator =
        states
        |> Seq.map(fun(i,o)->stateSymbolList.[i],o)
        |> Seq.filter(fun(sym,l)->
            match sym with
            | "TAGSTART" | "TAGEND" -> true
            | _ -> false
        )
        |> Iterator

    seq {
        match iterator.tryNext() with
        | Some (si,li) when si = "TAGEND" ->
            let enm = unbox<string> li
            match iterator.tryNext() with
            | Some (sj,lj) when sj = "TAGSTART" ->
                let snm,_ = unbox<string*(string*string)list> lj
                if enm = snm then
                    ()
                else
                    failwith $"unmatched tags:<{snm}></{enm}>"
            | _ -> failwith $"orphan end tag:</{enm}>"
        | Some x -> yield x
        | None -> ()
        yield! Seq.make iterator.tryNext
    }
    |> Seq.map(
            snd
            >> unbox<string*(string*string)list>
            >> fst
        )

let newOmittedTagend tok nm = {
    tok with
        length = 0
        value = TAGEND nm
}

//todo: match states pattern using fslex
let insertOmittedTagend (states:list<int*obj>) (tok:Position<HtmlToken>) =
    seq {
        match tok.value with
        | EOF ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGEND enm ->
            let omittedTagends,found =
                let tagstarts =
                    states
                    |> getTagStarts
                    |> Iterator
                let rec loop acc =
                    match tagstarts.tryNext() with
                    | Some snm when snm = enm ->
                        List.rev acc,true
                    | Some snm ->
                        let acc = newOmittedTagend tok snm::acc
                        loop acc
                    | None ->
                        List.rev acc,false
                loop []

            if found then
                yield! omittedTagends
                yield tok
            else
                failwith $"orphan end tag:</{enm}>"

        | TAGSTART ("body",_) | TAGSELFCLOSING ("body",_) ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter((=)"head")
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok

        | TAGSTART ("li",_) | TAGSELFCLOSING ("li",_) ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter((=)"li")
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok

        | TAGSTART (("dt"|"dd"),_) | TAGSELFCLOSING (("dt"|"dd"),_) ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter(function "dt"|"dd" -> true | _ -> false)
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART       (("address"|"article"|"aside"|"blockquote"|"details"|"div"|"dl"|"fieldset"|"figcaption"|"figure"|"footer"|"form"|"h1"|"h2"|"h3"|"h4"|"h5"|"h6"|"header"|"hgroup"|"hr"|"main"|"menu"|"nav"|"ol"|"p"|"pre"|"section"|"table"| "ul"),_)
        | TAGSELFCLOSING (("address"|"article"|"aside"|"blockquote"|"details"|"div"|"dl"|"fieldset"|"figcaption"|"figure"|"footer"|"form"|"h1"|"h2"|"h3"|"h4"|"h5"|"h6"|"header"|"hgroup"|"hr"|"main"|"menu"|"nav"|"ol"|"p"|"pre"|"section"|"table"| "ul"),_)
            ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter((=)"p")
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART (("rt"|"rp"),_) | TAGSELFCLOSING (("rt"|"rp"),_) ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter(function "rt"|"rp" -> true | _ -> false)
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART ("optgroup",_) | TAGSELFCLOSING ("optgroup",_) ->
            let omittedTagends =
                let tagstarts =
                    states
                    |> getTagStarts
                    |> Iterator
                seq {
                    match tagstarts.tryNext() with
                    | Some ("option" as nm1) ->
                        yield nm1
                        match tagstarts.tryNext() with
                        | Some ("optgroup" as nm2) -> yield nm2
                        | _ -> ()
                    | Some ("optgroup" as nm) -> yield nm
                    | _ -> ()
                }
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok
        | TAGSTART ("option",_) | TAGSELFCLOSING ("option",_) ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter((=)"option")
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART ("col",_) | TAGSELFCLOSING ("col",_) ->
            let omittedTagends =
                let tagstarts =
                    states
                    |> getTagStarts
                    |> Iterator
                [
                    match tagstarts.tryNext() with
                    | Some ("caption" as nm) -> yield nm
                    | _ -> ()
                ]
                |> List.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok
        | TAGSTART ("colgroup",_) | TAGSELFCLOSING ("colgroup",_) ->
            let omittedTagends =
                let tagstarts =
                    states
                    |> getTagStarts
                    |> Iterator
                [
                    // <col>是void元素，已经转化为selfclosing不用补充结束标签
                    match tagstarts.tryNext() with
                    | Some (("colgroup"|"caption") as nm) -> yield nm
                    | _ -> ()
                ]
                |> List.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | TAGSTART (("td"|"th"),_) | TAGSELFCLOSING (("td"|"th"),_) ->
            let omittedTagends =
                states
                |> getTagStarts
                |> Seq.truncate 1
                |> Seq.filter(function "td"|"th"|"colgroup"|"caption" -> true | _ -> false)
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok

        | TAGSTART ("tr",_) | TAGSELFCLOSING ("tr",_) ->
            let omittedTagends =
                let tagstarts =
                    states
                    |> getTagStarts
                    |> Iterator
                seq {
                    match tagstarts.tryNext() with
                    | Some (("td"|"th") as nm1) ->
                        yield nm1
                        match tagstarts.tryNext() with
                        | Some ("tr" as nm2) -> yield nm2
                        | _ -> ()
                    | Some (("tr"|"caption"|"colgroup") as nm) -> yield nm
                    | _ -> ()
                }
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | TAGSTART       (("thead"|"tbody"|"tfoot"),_)
        | TAGSELFCLOSING (("thead"|"tbody"|"tfoot"),_) ->
            let omittedTagends =
                let tagstarts =
                    states
                    |> getTagStarts
                    |> Iterator
                seq {
                    //("td"|"th")?"tr"?("thead"|"tbody"|"tfoot"|"caption"|"colgroup")?
                    match tagstarts.tryNext() with
                    | Some (("td"|"th") as nm1) ->
                        yield nm1
                        match tagstarts.tryNext() with
                        | Some ("tr" as nm2) ->
                            yield nm2
                            match tagstarts.tryNext() with
                            | Some (("thead"|"tbody"|"tfoot") as nm3) ->
                                yield nm3
                            | _ -> ()
                        | Some (("thead"|"tbody"|"tfoot") as nm3) ->
                            yield nm3
                        | _ -> ()
                    | Some ("tr" as nm2) ->
                        yield nm2
                        match tagstarts.tryNext() with
                        | Some (("thead"|"tbody"|"tfoot") as nm3) ->
                            yield nm3
                        | _ -> ()
                    | Some (("thead"|"tbody"|"tfoot"|"caption"|"colgroup") as nm3) ->
                        yield nm3
                    | _ -> ()
                }
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | _ -> yield tok
    }

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
            |> getTagStarts
            |> Seq.isEmpty
            |> not
        | _ -> true
    )
    |> Seq.collect(fun tok ->
        //不可以柯里化，否则会错误地缓存states快照
        insertOmittedTagend states tok
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

    match parser.tryReduce(states) with
    | Some reducedstates -> states <- reducedstates
    | None -> ()

    match states with
    |[1,lxm; 0,null] ->
        HtmldocParseTable.unboxRoot lxm
    | _ ->
        failwith $"a:{stringify tokens}\r\n{stringify states}"


