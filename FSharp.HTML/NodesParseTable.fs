module FSharp.HTML.NodesParseTable
let rules = [|["children";"nodes"],"Array.ofList(List.rev s0)";["children"],"[||]";["nodes";"nodes";";";"node"],"s2::s0";["nodes";"node"],"[s0]";["node";"TEXT"],"HtmlText s0";["node";"COMMENT"],"HtmlComment s0";["node";"CDATA"],"HtmlCData s0";["node";"element"],"s0";["element";"TAGSELFCLOSING"],"let name,attrs = s0\r\nlet attrs = Map.ofList attrs\r\nHtmlElement(name, attrs,[||])";["element";"TAGSTART";"children";"TAGEND"],"let name,attrs = s0\r\nlet attrs = Map.ofList attrs\r\nNodesFsyaccUtils.htmlNode name s2 attrs s1"|]
let actions = [|[|"",-1;"CDATA",7;"COMMENT",8;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"children",1;"element",10;"node",11;"nodes",2|];[|"",0|];[|"",-2;";",12;"TAGEND",-2|];[|"",-3;";",-3;"TAGEND",-3|];[|"CDATA",7;"COMMENT",8;"TAGEND",-1;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"children",5;"element",10;"node",11;"nodes",2|];[|"TAGEND",6|];[|"",-4;";",-4;"TAGEND",-4|];[|"",-5;";",-5;"TAGEND",-5|];[|"",-6;";",-6;"TAGEND",-6|];[|"",-7;";",-7;"TAGEND",-7|];[|"",-8;";",-8;"TAGEND",-8|];[|"",-9;";",-9;"TAGEND",-9|];[|"CDATA",7;"COMMENT",8;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"element",10;"node",13|];[|"",-10;";",-10;"TAGEND",-10|]|]
let closures = [|[|0,0,[||];-1,0,[|""|];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||]|];[|0,1,[|""|]|];[|-2,1,[|"";"TAGEND"|];-10,1,[||]|];[|-3,1,[|"";";";"TAGEND"|]|];[|-1,0,[|"TAGEND"|];-2,0,[||];-3,0,[||];-4,0,[||];-4,1,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||]|];[|-4,2,[||]|];[|-4,3,[|"";";";"TAGEND"|]|];[|-5,1,[|"";";";"TAGEND"|]|];[|-6,1,[|"";";";"TAGEND"|]|];[|-7,1,[|"";";";"TAGEND"|]|];[|-8,1,[|"";";";"TAGEND"|]|];[|-9,1,[|"";";";"TAGEND"|]|];[|-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-10,2,[||]|];[|-10,3,[|"";";";"TAGEND"|]|]|]
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils"
let declarations = [|"TAGSELFCLOSING","string*(string*string) list";"TAGSTART","string*(string*string) list";"TAGEND","string";"CDATA","string";"COMMENT","string";"TEXT","string";"node","HtmlNode";"element","HtmlNode";"children","HtmlNode[]";"nodes","HtmlNode list"|]
open System
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
let fxRules:(string list*(obj[]->obj))[] = [|
    ["children";"nodes"],fun (ss:obj[]) ->
            let s0 = unbox<HtmlNode list> ss.[0]
            let result:HtmlNode[] =
                Array.ofList(List.rev s0)
            box result
    ["children"],fun (ss:obj[]) ->
            let result:HtmlNode[] =
                [||]
            box result
    ["nodes";"nodes";";";"node"],fun (ss:obj[]) ->
            let s0 = unbox<HtmlNode list> ss.[0]
            let s2 = unbox<HtmlNode> ss.[2]
            let result:HtmlNode list =
                s2::s0
            box result
    ["nodes";"node"],fun (ss:obj[]) ->
            let s0 = unbox<HtmlNode> ss.[0]
            let result:HtmlNode list =
                [s0]
            box result
    ["node";"TEXT"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:HtmlNode =
                HtmlText s0
            box result
    ["node";"COMMENT"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:HtmlNode =
                HtmlComment s0
            box result
    ["node";"CDATA"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:HtmlNode =
                HtmlCData s0
            box result
    ["node";"element"],fun (ss:obj[]) ->
            let s0 = unbox<HtmlNode> ss.[0]
            let result:HtmlNode =
                s0
            box result
    ["element";"TAGSELFCLOSING"],fun (ss:obj[]) ->
            let s0 = unbox<string*(string*string) list> ss.[0]
            let result:HtmlNode =
                let name,attrs = s0
                let attrs = Map.ofList attrs
                HtmlElement(name, attrs,[||])
            box result
    ["element";"TAGSTART";"children";"TAGEND"],fun (ss:obj[]) ->
            let s0 = unbox<string*(string*string) list> ss.[0]
            let s1 = unbox<HtmlNode[]> ss.[1]
            let s2 = unbox<string> ss.[2]
            let result:HtmlNode =
                let name,attrs = s0
                let attrs = Map.ofList attrs
                NodesFsyaccUtils.htmlNode name s2 attrs s1
            box result
|]
open FslexFsyacc.Runtime
let parser = Parser(fxRules, actions, closures)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<HtmlNode[]>