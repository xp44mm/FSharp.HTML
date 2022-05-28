module FSharp.HTML.PrecNodesParseTable
let rules = [|["children";"nodes"],"List.rev s0";["children"],"[]";["nodes";"nodes";"node"],"s1::s0";["nodes";"node"],"[s0]";["node";"TEXT"],"HtmlText s0";["node";"COMMENT"],"HtmlComment s0";["node";"CDATA"],"HtmlCData s0";["node";"element"],"s0";["element";"TAGSELFCLOSING"],"let name,attrs = s0\r\nHtmlElement(name, attrs,[])";["element";"TAGSTART";"children";"TAGEND"],"let name,attrs = s0\r\nif name = s2 then\r\n    HtmlElement(name, attrs, s1)\r\nelse\r\n    failwith $\"unmatched:<{name}></{s2}>\""|]
let actions = [|[|"",-1;"CDATA",7;"COMMENT",8;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"children",1;"element",10;"node",11;"nodes",2|];[|"",0|];[|"",-2;"CDATA",7;"COMMENT",8;"TAGEND",-2;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"element",10;"node",12|];[|"",-3;"CDATA",-3;"COMMENT",-3;"TAGEND",-3;"TAGSELFCLOSING",-3;"TAGSTART",-3;"TEXT",-3|];[|"CDATA",7;"COMMENT",8;"TAGEND",-1;"TAGSELFCLOSING",3;"TAGSTART",4;"TEXT",9;"children",5;"element",10;"node",11;"nodes",2|];[|"TAGEND",6|];[|"",-4;"CDATA",-4;"COMMENT",-4;"TAGEND",-4;"TAGSELFCLOSING",-4;"TAGSTART",-4;"TEXT",-4|];[|"",-5;"CDATA",-5;"COMMENT",-5;"TAGEND",-5;"TAGSELFCLOSING",-5;"TAGSTART",-5;"TEXT",-5|];[|"",-6;"CDATA",-6;"COMMENT",-6;"TAGEND",-6;"TAGSELFCLOSING",-6;"TAGSTART",-6;"TEXT",-6|];[|"",-7;"CDATA",-7;"COMMENT",-7;"TAGEND",-7;"TAGSELFCLOSING",-7;"TAGSTART",-7;"TEXT",-7|];[|"",-8;"CDATA",-8;"COMMENT",-8;"TAGEND",-8;"TAGSELFCLOSING",-8;"TAGSTART",-8;"TEXT",-8|];[|"",-9;"CDATA",-9;"COMMENT",-9;"TAGEND",-9;"TAGSELFCLOSING",-9;"TAGSTART",-9;"TEXT",-9|];[|"",-10;"CDATA",-10;"COMMENT",-10;"TAGEND",-10;"TAGSELFCLOSING",-10;"TAGSTART",-10;"TEXT",-10|]|]
let closures = [|[|0,0,[||];-1,0,[|""|];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||]|];[|0,1,[|""|]|];[|-2,1,[|"";"TAGEND"|];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-10,1,[||]|];[|-3,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-1,0,[|"TAGEND"|];-2,0,[||];-3,0,[||];-4,0,[||];-4,1,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||]|];[|-4,2,[||]|];[|-4,3,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-5,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-6,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-7,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-8,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-9,1,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|];[|-10,2,[|"";"CDATA";"COMMENT";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"|]|]|]
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils\r\ntype token = HtmlToken"
let declarations = [|"TAGSELFCLOSING","string*list<string*string>";"TAGSTART","string*list<string*string>";"TAGEND","string";"CDATA","string";"COMMENT","string";"TEXT","string";"node","HtmlNode";"element","HtmlNode";"children","HtmlNode list";"nodes","HtmlNode list"|]
open System
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
type token = HtmlToken
let fxRules:(string list*(obj[]->obj))[] = [|
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
            let s0 = unbox<string*list<string*string>> ss.[0]
            let result:HtmlNode =
                let name,attrs = s0
                HtmlElement(name, attrs,[])
            box result
    ["element";"TAGSTART";"children";"TAGEND"],fun (ss:obj[]) ->
            let s0 = unbox<string*list<string*string>> ss.[0]
            let s1 = unbox<HtmlNode list> ss.[1]
            let s2 = unbox<string> ss.[2]
            let result:HtmlNode =
                let name,attrs = s0
                if name = s2 then
                    HtmlElement(name, attrs, s1)
                else
                    failwith $"unmatched:<{name}></{s2}>"
            box result
|]
open FslexFsyacc.Runtime
let parser = Parser<token>(fxRules,actions,closures,getTag,getLexeme)
let parse(tokens:seq<token>) =
    tokens
    |> parser.parse
    |> unbox<HtmlNode list>