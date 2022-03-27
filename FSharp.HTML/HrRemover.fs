module FSharp.HTML.HrRemover

let splitByFirstHr (nodes:HtmlNode list) =
    let rec loop acc (nodes:HtmlNode list) =
        match nodes with
        | [] -> acc |> List.rev,[]
        | HtmlElement("hr",_,_) :: t -> acc |> List.rev,t
        | h :: t -> loop (h::acc) t
    loop [] nodes

let splitByHr (nodes:HtmlNode list) =
    let rec loop acc (nodes:HtmlNode list) =
        let group, rest = splitByFirstHr nodes
        let acc = 
            if group.IsEmpty then acc else group :: acc

        match rest with
        | [] -> acc |> List.rev
        | _ -> loop acc rest

    loop [] nodes

