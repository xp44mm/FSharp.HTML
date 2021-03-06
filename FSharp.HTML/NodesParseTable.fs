module FSharp.HTML.NodesParseTable
let actions = [|[|"",-1;"CDATA",7;"COMMENT",8;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"children",1;"element",10;"node",11;"nodes",2|];[|"",0|];[|"",-2;"CDATA",7;"COMMENT",8;"TAGEND",-2;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"element",10;"node",12|];[|"",-3;"CDATA",-3;"COMMENT",-3;"TAGEND",-3;"TAGSELFCLOSING",-3;"TAGSTART",-3;"TEXT",-3|];[|"CDATA",7;"COMMENT",8;"TAGEND",-1;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"children",5;"element",10;"node",11;"nodes",2|];[|"TAGEND",6|];[|"",-4;"CDATA",-4;"COMMENT",-4;"TAGEND",-4;"TAGSELFCLOSING",-4;"TAGSTART",-4;"TEXT",-4|];[|"",-5;"CDATA",-5;"COMMENT",-5;"TAGEND",-5;"TAGSELFCLOSING",-5;"TAGSTART",-5;"TEXT",-5|];[|"",-6;"CDATA",-6;"COMMENT",-6;"TAGEND",-6;"TAGSELFCLOSING",-6;"TAGSTART",-6;"TEXT",-6|];[|"",-7;"CDATA",-7;"COMMENT",-7;"TAGEND",-7;"TAGSELFCLOSING",-7;"TAGSTART",-7;"TEXT",-7|];[|"",-8;"CDATA",-8;"COMMENT",-8;"TAGEND",-8;"TAGSELFCLOSING",-8;"TAGSTART",-8;"TEXT",-8|];[|"",-9;"CDATA",-9;"COMMENT",-9;"TAGEND",-9;"TAGSELFCLOSING",-9;"TAGSTART",-9;"TEXT",-9|];[|"",-10;"CDATA",-10;"COMMENT",-10;"TAGEND",-10;"TAGSELFCLOSING",-10;"TAGSTART",-10;"TEXT",-10|]|]
let closures = [|[|0,0,[||];-1,0,[|""|];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||]|];[|0,1,[|""|]|];[|-2,1,[|"";"TAGEND"|];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-10,1,[||]|];[|-3,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-1,0,[|"TAGEND"|];-2,0,[||];-3,0,[||];-4,0,[||];-4,1,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||]|];[|-4,2,[||]|];[|-4,3,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-5,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-6,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-7,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-8,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-9,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-10,2,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|]|]
open System
open FslexFsyacc.Runtime
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
open FSharp.HTML.HtmlNodeCreator
type token = Position<HtmlToken>
let rules:(string list*(obj[]->obj))[] = [|
    ["children";"nodes"],fun (ss:obj[]) ->
        let s0 = unbox<HtmlNode list> ss.[0]
        let result:HtmlNode list =
            List.rev s0
        box result
    ["children"],fun (ss:obj[]) ->
        let result:HtmlNode list =
            []
        box result
    ["nodes";"nodes";"node"],fun (ss:obj[]) ->
        let s0 = unbox<HtmlNode list> ss.[0]
        let s1 = unbox<HtmlNode> ss.[1]
        let result:HtmlNode list =
            s1::s0
        box result
    ["nodes";"node"],fun (ss:obj[]) ->
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode list =
            [s0]
        box result
    ["node";"TEXT"],fun (ss:obj[]) ->
        let s0 = unbox<Position<HtmlToken>> ss.[0]
        let result:HtmlNode =
            htmlText s0
        box result
    ["node";"COMMENT"],fun (ss:obj[]) ->
        let s0 = unbox<Position<HtmlToken>> ss.[0]
        let result:HtmlNode =
            htmlComment s0
        box result
    ["node";"CDATA"],fun (ss:obj[]) ->
        let s0 = unbox<Position<HtmlToken>> ss.[0]
        let result:HtmlNode =
            htmlCData s0
        box result
    ["node";"element"],fun (ss:obj[]) ->
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode =
            s0
        box result
    ["element";"TAGSELFCLOSING"],fun (ss:obj[]) ->
        let s0 = unbox<Position<HtmlToken>> ss.[0]
        let result:HtmlNode =
            let name,attrs = getNameAttributes s0
            HtmlElement(name, attrs,[])
        box result
    ["element";"TAGSTART";"children";"TAGEND"],fun (ss:obj[]) ->
        let s0 = unbox<Position<HtmlToken>> ss.[0]
        let s1 = unbox<HtmlNode list> ss.[1]
        let s2 = unbox<Position<HtmlToken>> ss.[2]
        let result:HtmlNode =
            let name,attrs = getNameAttributes s0
            if name = getName s2 then
                HtmlElement(name, attrs, s1)
            else
                failwith $"unmatched tag:<{name}></{s2}>"
        box result
|]
let parser = Parser<token>(rules,actions,closures,getTag,getLexeme)
let parse(tokens:seq<token>) =
    tokens
    |> parser.parse
    |> unbox<HtmlNode list>