module FSharp.HTML.BrRemover

let splitByFirstBr (nodes:HtmlNode list) =
    let rec loop acc (nodes:HtmlNode list) =
        match nodes with
        | [] -> acc |> List.rev,[]
        | HtmlElement("br",_,_) :: t -> acc |> List.rev,t
        | h :: t -> loop (h::acc) t
    loop [] nodes

let splitByBr (nodes:HtmlNode list) =
    let rec loop acc (nodes:HtmlNode list) =
        let group, rest = splitByFirstBr nodes
        let acc = 
            if group.IsEmpty then acc else group :: acc
        match rest with
        | [] -> acc |> List.rev
        | _ -> loop acc rest

    loop [] nodes

