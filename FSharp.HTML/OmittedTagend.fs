module FSharp.HTML.OmittedTagend

open FSharp.Idioms.Literal
open FSharp.Idioms

open FslexFsyacc.Runtime

let getOmittedTagends anal (startTags:seq<string>) =
    if Seq.isEmpty startTags then
        []
    else
        startTags
        |> anal
        |> Seq.head

let (|Belong|_|) sq elem =
    sq
    |> Seq.tryFind(fun e -> e = elem)

let comb1 (ts:seq<string>) =
    let ts = ts |> Set.ofSeq
    let getTag tok = if ts.Contains tok then "t" else "f"
    let anal (tokens:seq<string>) = Comb1DFA.analyzer.analyze(tokens,getTag)
    getOmittedTagends anal

let comb2 (xs:seq<string>) (ys:seq<string>) =
    let getTag tok =
        match tok with
        | Belong xs _ -> "x"
        | Belong ys _ -> "y"
        | _ -> "z"
    let anal (tokens:seq<string>) = Comb2DFA.analyzer.analyze(tokens,getTag)
    getOmittedTagends anal

let comb2plus1 (ks:seq<string>) (ls:seq<string>) (ms:seq<string>) =
    let getTag tok =
        match tok with
        | Belong ks _ -> "k"
        | Belong ls _ -> "l"
        | Belong ms _ -> "m"
        | _ -> "n"
    let anal (tokens:seq<string>) = Comb2plus1DFA.analyzer.analyze(tokens,getTag)
    getOmittedTagends anal

let comb3plus1 (js:seq<string>) (ks:seq<string>) (ls:seq<string>) (ms:seq<string>) =
    let getTag tok =
        match tok with
        | Belong js _ -> "j"
        | Belong ks _ -> "k"
        | Belong ls _ -> "l"
        | Belong ms _ -> "m"
        | _ -> "n"
    let anal (tokens:seq<string>) = Comb3plus1DFA.analyzer.analyze(tokens,getTag)
    getOmittedTagends anal

let newOmittedTagend tok nm = {
    tok with
        length = 0
        value = TAGEND nm
}

//todo: match states pattern using fslex
let insertOmittedTagend (unclosedTagStarts:seq<string>) (tok:Position<HtmlToken>) =
    seq {
        match tok.value with
        | EOF ->
            let omittedTagends =
                unclosedTagStarts
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGEND enm ->
            let omittedTagends,found =
                let tagstarts =
                    unclosedTagStarts
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
                unclosedTagStarts
                |> comb1 ["head"]
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok

        | TAGSTART ("li",_) | TAGSELFCLOSING ("li",_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["li"]
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok

        | TAGSTART (("dt"|"dd"),_) | TAGSELFCLOSING (("dt"|"dd"),_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["dt";"dd"]
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART       (("address"|"article"|"aside"|"blockquote"|"details"|"div"|"dl"|"fieldset"|"figcaption"|"figure"|"footer"|"form"|"h1"|"h2"|"h3"|"h4"|"h5"|"h6"|"header"|"hgroup"|"hr"|"main"|"menu"|"nav"|"ol"|"p"|"pre"|"section"|"table"| "ul"),_)
        | TAGSELFCLOSING (("address"|"article"|"aside"|"blockquote"|"details"|"div"|"dl"|"fieldset"|"figcaption"|"figure"|"footer"|"form"|"h1"|"h2"|"h3"|"h4"|"h5"|"h6"|"header"|"hgroup"|"hr"|"main"|"menu"|"nav"|"ol"|"p"|"pre"|"section"|"table"| "ul"),_)
            ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["p"]
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART (("rt"|"rp"),_) | TAGSELFCLOSING (("rt"|"rp"),_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["rt";"rp"]
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART ("optgroup",_) | TAGSELFCLOSING ("optgroup",_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb2 ["option"] ["optgroup"]
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok
        | TAGSTART ("option",_) | TAGSELFCLOSING ("option",_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["option"]
                |> Seq.map(newOmittedTagend tok)

            yield! omittedTagends
            yield tok
        | TAGSTART ("col",_) | TAGSELFCLOSING ("col",_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["caption"] // <col>是void元素，已经转化为selfclosing不用补充结束标签
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok
        | TAGSTART ("colgroup",_) | TAGSELFCLOSING ("colgroup",_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["colgroup";"caption"] // <col>是void元素，已经转化为selfclosing不用补充结束标签
                |> List.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | TAGSTART (("td"|"th"),_) | TAGSELFCLOSING (("td"|"th"),_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb1 ["td";"th";"colgroup";"caption"]
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | TAGSTART ("tr",_) | TAGSELFCLOSING ("tr",_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb2plus1 ["td";"th"] ["tr"] ["colgroup";"caption"]
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | TAGSTART       (("thead"|"tbody"|"tfoot"),_)
        | TAGSELFCLOSING (("thead"|"tbody"|"tfoot"),_) ->
            let omittedTagends =
                unclosedTagStarts
                |> comb3plus1 ["td";"th"] ["tr"] ["thead";"tbody";"tfoot"] ["colgroup";"caption"]
                |> Seq.map(newOmittedTagend tok)
            yield! omittedTagends
            yield tok

        | _ -> yield tok
    }

