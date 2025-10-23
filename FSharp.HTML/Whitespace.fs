module FSharp.HTML.Whitespace

open System.Text.RegularExpressions

let trimWhitespace elements =
    let rec forward trimStart acc children =
        match children with
        | [] -> trimStart, acc
        | h :: t ->
            match h with
            | HtmlDoctype _ -> forward trimStart (h :: acc) t
            | HtmlCData _ -> forward false (h :: acc) t
            | HtmlComment _ -> forward trimStart (h :: acc) t
            | HtmlWS _ -> forward trimStart acc t
            | HtmlText x ->
                match x.TrimStart() with
                | "" -> forward trimStart acc t
                | y ->
                    let trimStart = Regex.IsMatch(y, @"\s+$")
                    forward trimStart (HtmlText y :: acc) t

            | HtmlElement(("pre"|"textarea"|"script"|"style"|"code"), _, _) -> forward true (h :: acc) t

            | HtmlElement(tag, attrs, subchildren) ->
                let trimStart =
                    trimStart
                    || HtmlSchema.blockLevelFamily.Contains tag
                let trimStart, subchildren = forward trimStart [] subchildren
                let hh = HtmlElement(tag, attrs, subchildren)
                let trimStart =
                    trimStart
                    || HtmlSchema.blockLevelFamily.Contains tag
                forward trimStart (hh :: acc) t

    let rec backward trimEnd acc revchildren =
        match revchildren with
        | [] -> trimEnd, acc
        | h :: t ->
            match h with
            | HtmlDoctype _ -> backward trimEnd (h :: acc) t
            | HtmlComment _ -> backward trimEnd (h :: acc) t
            | HtmlCData _ -> backward false (h :: acc) t
            | HtmlWS _ -> backward trimEnd acc t
            | HtmlText x ->
                match x.TrimEnd() with
                | "" -> backward trimEnd acc t
                | y ->
                    let y = Regex.Replace(y, "\s+", " ")
                    backward false (HtmlText y :: acc) t

            | HtmlElement(("pre"|"textarea"|"script"|"style"|"code"), _, _) -> backward true (h :: acc) t

            | HtmlElement(nm, attrs, subchildren) ->
                let trimEnd = trimEnd || HtmlSchema.blockLevelFamily.Contains nm
                let trimEnd, subchildren = backward trimEnd [] subchildren
                let hh = HtmlElement(nm, attrs, subchildren)
                let trimEnd = trimEnd || HtmlSchema.blockLevelFamily.Contains nm
                backward trimEnd (hh :: acc) t

    elements
    |> forward true []
    |> snd
    |> backward true []
    |> snd


