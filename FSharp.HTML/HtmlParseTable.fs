module FSharp.HTML.HtmlParseTable
let rules = [|["document";"DOCTYPE"],"s0,[]";["document";"nodes"],"\"\",List.rev s0";["document";"DOCTYPE";";";"nodes"],"s0,List.rev s2";["nodes";"nodes";";";"node"],"s2::s0";["nodes";"node"],"[s0]";["nodes"],"[]";["node";"TEXT"],"HtmlText s0";["node";"COMMENT"],"HtmlComment s0";["node";"CDATA"],"HtmlCData s0";["node";"element"],"s0";["element";"TAGSELFCLOSING"],"let name,attrs = s0\r\nHtmlElement(name, attrs,[])";["element";"TAGSTART";"nodes";"TAGEND"],"let name,attrs = s0\r\nlet el = HtmlFsyaccUtils.htmlNode name s2\r\nel attrs (List.rev s1)"|]
let actions = [|[|"",-10;";",-10;"CDATA",10;"COMMENT",11;"DOCTYPE",2;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",12;"document",1;"element",13;"node",14;"nodes",5|];[|"",0|];[|"",-1;";",3|];[|"",-10;";",-10;"CDATA",10;"COMMENT",11;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",12;"element",13;"node",14;"nodes",4|];[|"",-2;";",15|];[|"",-3;";",15|];[|"",-4;";",-4;"TAGEND",-4|];[|";",-10;"CDATA",10;"COMMENT",11;"TAGEND",-10;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",12;"element",13;"node",14;"nodes",8|];[|";",15;"TAGEND",9|];[|"",-5;";",-5;"TAGEND",-5|];[|"",-6;";",-6;"TAGEND",-6|];[|"",-7;";",-7;"TAGEND",-7|];[|"",-8;";",-8;"TAGEND",-8|];[|"",-9;";",-9;"TAGEND",-9|];[|"",-11;";",-11;"TAGEND",-11|];[|"CDATA",10;"COMMENT",11;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",12;"element",13;"node",16|];[|"",-12;";",-12;"TAGEND",-12|]|]
let closures = [|[|0,0,[||];-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[|"";";"|];-11,0,[||];-12,0,[||]|];[|0,1,[|""|]|];[|-1,1,[|""|];-2,1,[||]|];[|-2,2,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[|"";";"|];-11,0,[||];-12,0,[||]|];[|-2,3,[|""|];-12,1,[||]|];[|-3,1,[|""|];-12,1,[||]|];[|-4,1,[|"";";";"TAGEND"|]|];[|-4,0,[||];-5,0,[||];-5,1,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[|";";"TAGEND"|];-11,0,[||];-12,0,[||]|];[|-5,2,[||];-12,1,[||]|];[|-5,3,[|"";";";"TAGEND"|]|];[|-6,1,[|"";";";"TAGEND"|]|];[|-7,1,[|"";";";"TAGEND"|]|];[|-8,1,[|"";";";"TAGEND"|]|];[|-9,1,[|"";";";"TAGEND"|]|];[|-11,1,[|"";";";"TAGEND"|]|];[|-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-12,2,[||]|];[|-12,3,[|"";";";"TAGEND"|]|]|]
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils"
let declarations = [|"TAGSTART","string*HtmlAttribute list";"TAGEND","string";"TAGSELFCLOSING","string*HtmlAttribute list";"CDATA","string";"COMMENT","string";"DOCTYPE","string";"TEXT","string";"document","string*HtmlNode list";"element","HtmlNode";"node","HtmlNode";"nodes","HtmlNode list";"voidElement","HtmlNode"|]
open System
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
let fxRules:(string list*(obj[]->obj))[] = [|
    ["document";"DOCTYPE"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:string*HtmlNode list =
                s0,[]
            box result
    ["document";"nodes"],fun (ss:obj[]) ->
            let s0 = unbox<HtmlNode list> ss.[0]
            let result:string*HtmlNode list =
                "",List.rev s0
            box result
    ["document";"DOCTYPE";";";"nodes"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let s2 = unbox<HtmlNode list> ss.[2]
            let result:string*HtmlNode list =
                s0,List.rev s2
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
    ["nodes"],fun (ss:obj[]) ->
            let result:HtmlNode list =
                []
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
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                let name,attrs = s0
                HtmlElement(name, attrs,[])
            box result
    ["element";"TAGSTART";"nodes";"TAGEND"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let s1 = unbox<HtmlNode list> ss.[1]
            let s2 = unbox<string> ss.[2]
            let result:HtmlNode =
                let name,attrs = s0
                let el = HtmlFsyaccUtils.htmlNode name s2
                el attrs (List.rev s1)
            box result
|]
open FslexFsyacc.Runtime
let parser = Parser(fxRules, actions, closures)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string*HtmlNode list>