module FSharp.HTML.HtmlParseTable
let rules = [|["document";"DOCTYPE"],"s0,[]";["document";"nodes"],"\"\",List.rev s0";["document";"DOCTYPE";";";"nodes"],"s0,List.rev s2";["nodes";"nodes";";";"node"],"s2::s0";["nodes";"node"],"[s0]";["nodes"],"[]";["node";"TEXT"],"HtmlText s0";["node";"COMMENT"],"HtmlComment s0";["node";"CDATA"],"HtmlCData s0";["node";"element"],"s0";["element";"TAGSELFCLOSING"],"let name,attrs = s0\r\nHtmlElement(name, attrs,[])";["element";"TAGSTART";"nodes";"TAGEND"],"let name,attrs = s0\r\nif s2 = name then\r\n    HtmlElement(name, attrs,s1)\r\nelse failwithf \"%A ... %s\" s0 s2";["element";"voidElement"],"s0";["voidElement";"<area>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<area>";"</area>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<base>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<base>";"</base>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<br>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<br>";"</br>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<col>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<col>";"</col>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<embed>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<embed>";"</embed>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<hr>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<hr>";"</hr>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<img>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<img>";"</img>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<input>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<input>";"</input>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<link>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<link>";"</link>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<meta>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<meta>";"</meta>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<param>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<param>";"</param>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<source>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<source>";"</source>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<track>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<track>";"</track>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<wbr>"],"HtmlElement(fst s0,snd s0, [])";["voidElement";"<wbr>";"</wbr>"],"HtmlElement(fst s0,snd s0, [])"|]
let actions = [|[|"",-11;";",-11;"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"DOCTYPE",2;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"document",1;"element",14;"node",15;"nodes",5;"voidElement",10|];[|"",0|];[|"",-1;";",3|];[|"",-11;";",-11;"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"element",14;"node",15;"nodes",4;"voidElement",10|];[|"",-2;";",16|];[|"",-3;";",16|];[|"",-4;";",-4;"TAGEND",-4|];[|";",-11;"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"TAGEND",-11;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"element",14;"node",15;"nodes",8;"voidElement",10|];[|";",16;"TAGEND",9|];[|"",-5;";",-5;"TAGEND",-5|];[|"",-6;";",-6;"TAGEND",-6|];[|"",-7;";",-7;"TAGEND",-7|];[|"",-8;";",-8;"TAGEND",-8|];[|"",-9;";",-9;"TAGEND",-9|];[|"",-10;";",-10;"TAGEND",-10|];[|"",-12;";",-12;"TAGEND",-12|];[|"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"element",14;"node",17;"voidElement",10|];[|"",-13;";",-13;"TAGEND",-13|];[|"",-14;";",-14;"</area>",19;"TAGEND",-14|];[|"",-15;";",-15;"TAGEND",-15|];[|"",-16;";",-16;"</base>",21;"TAGEND",-16|];[|"",-17;";",-17;"TAGEND",-17|];[|"",-18;";",-18;"</br>",23;"TAGEND",-18|];[|"",-19;";",-19;"TAGEND",-19|];[|"",-20;";",-20;"</col>",25;"TAGEND",-20|];[|"",-21;";",-21;"TAGEND",-21|];[|"",-22;";",-22;"</embed>",27;"TAGEND",-22|];[|"",-23;";",-23;"TAGEND",-23|];[|"",-24;";",-24;"</hr>",29;"TAGEND",-24|];[|"",-25;";",-25;"TAGEND",-25|];[|"",-26;";",-26;"</img>",31;"TAGEND",-26|];[|"",-27;";",-27;"TAGEND",-27|];[|"",-28;";",-28;"</input>",33;"TAGEND",-28|];[|"",-29;";",-29;"TAGEND",-29|];[|"",-30;";",-30;"</link>",35;"TAGEND",-30|];[|"",-31;";",-31;"TAGEND",-31|];[|"",-32;";",-32;"</meta>",37;"TAGEND",-32|];[|"",-33;";",-33;"TAGEND",-33|];[|"",-34;";",-34;"</param>",39;"TAGEND",-34|];[|"",-35;";",-35;"TAGEND",-35|];[|"",-36;";",-36;"</source>",41;"TAGEND",-36|];[|"",-37;";",-37;"TAGEND",-37|];[|"",-38;";",-38;"</track>",43;"TAGEND",-38|];[|"",-39;";",-39;"TAGEND",-39|];[|"",-40;";",-40;"</wbr>",45;"TAGEND",-40|];[|"",-41;";",-41;"TAGEND",-41|]|]
let closures = [|[|0,0,[||];-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[|"";";"|];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,0,[||];-18,0,[||];-19,0,[||];-20,0,[||];-21,0,[||];-22,0,[||];-23,0,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||];-29,0,[||];-30,0,[||];-31,0,[||];-32,0,[||];-33,0,[||];-34,0,[||];-35,0,[||];-36,0,[||];-37,0,[||];-38,0,[||];-39,0,[||];-40,0,[||];-41,0,[||]|];[|0,1,[|""|]|];[|-1,1,[|""|];-2,1,[||]|];[|-2,2,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[|"";";"|];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,0,[||];-18,0,[||];-19,0,[||];-20,0,[||];-21,0,[||];-22,0,[||];-23,0,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||];-29,0,[||];-30,0,[||];-31,0,[||];-32,0,[||];-33,0,[||];-34,0,[||];-35,0,[||];-36,0,[||];-37,0,[||];-38,0,[||];-39,0,[||];-40,0,[||];-41,0,[||]|];[|-2,3,[|""|];-13,1,[||]|];[|-3,1,[|""|];-13,1,[||]|];[|-4,1,[|"";";";"TAGEND"|]|];[|-4,0,[||];-5,0,[||];-5,1,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[|";";"TAGEND"|];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,0,[||];-18,0,[||];-19,0,[||];-20,0,[||];-21,0,[||];-22,0,[||];-23,0,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||];-29,0,[||];-30,0,[||];-31,0,[||];-32,0,[||];-33,0,[||];-34,0,[||];-35,0,[||];-36,0,[||];-37,0,[||];-38,0,[||];-39,0,[||];-40,0,[||];-41,0,[||]|];[|-5,2,[||];-13,1,[||]|];[|-5,3,[|"";";";"TAGEND"|]|];[|-6,1,[|"";";";"TAGEND"|]|];[|-7,1,[|"";";";"TAGEND"|]|];[|-8,1,[|"";";";"TAGEND"|]|];[|-9,1,[|"";";";"TAGEND"|]|];[|-10,1,[|"";";";"TAGEND"|]|];[|-12,1,[|"";";";"TAGEND"|]|];[|-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-13,2,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,0,[||];-18,0,[||];-19,0,[||];-20,0,[||];-21,0,[||];-22,0,[||];-23,0,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||];-29,0,[||];-30,0,[||];-31,0,[||];-32,0,[||];-33,0,[||];-34,0,[||];-35,0,[||];-36,0,[||];-37,0,[||];-38,0,[||];-39,0,[||];-40,0,[||];-41,0,[||]|];[|-13,3,[|"";";";"TAGEND"|]|];[|-14,1,[|"";";";"TAGEND"|];-15,1,[||]|];[|-15,2,[|"";";";"TAGEND"|]|];[|-16,1,[|"";";";"TAGEND"|];-17,1,[||]|];[|-17,2,[|"";";";"TAGEND"|]|];[|-18,1,[|"";";";"TAGEND"|];-19,1,[||]|];[|-19,2,[|"";";";"TAGEND"|]|];[|-20,1,[|"";";";"TAGEND"|];-21,1,[||]|];[|-21,2,[|"";";";"TAGEND"|]|];[|-22,1,[|"";";";"TAGEND"|];-23,1,[||]|];[|-23,2,[|"";";";"TAGEND"|]|];[|-24,1,[|"";";";"TAGEND"|];-25,1,[||]|];[|-25,2,[|"";";";"TAGEND"|]|];[|-26,1,[|"";";";"TAGEND"|];-27,1,[||]|];[|-27,2,[|"";";";"TAGEND"|]|];[|-28,1,[|"";";";"TAGEND"|];-29,1,[||]|];[|-29,2,[|"";";";"TAGEND"|]|];[|-30,1,[|"";";";"TAGEND"|];-31,1,[||]|];[|-31,2,[|"";";";"TAGEND"|]|];[|-32,1,[|"";";";"TAGEND"|];-33,1,[||]|];[|-33,2,[|"";";";"TAGEND"|]|];[|-34,1,[|"";";";"TAGEND"|];-35,1,[||]|];[|-35,2,[|"";";";"TAGEND"|]|];[|-36,1,[|"";";";"TAGEND"|];-37,1,[||]|];[|-37,2,[|"";";";"TAGEND"|]|];[|-38,1,[|"";";";"TAGEND"|];-39,1,[||]|];[|-39,2,[|"";";";"TAGEND"|]|];[|-40,1,[|"";";";"TAGEND"|];-41,1,[||]|];[|-41,2,[|"";";";"TAGEND"|]|]|]
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils"
let declarations = [|"<area>","string*HtmlAttribute list";"<base>","string*HtmlAttribute list";"<br>","string*HtmlAttribute list";"<col>","string*HtmlAttribute list";"<embed>","string*HtmlAttribute list";"<hr>","string*HtmlAttribute list";"<img>","string*HtmlAttribute list";"<input>","string*HtmlAttribute list";"<link>","string*HtmlAttribute list";"<meta>","string*HtmlAttribute list";"<param>","string*HtmlAttribute list";"<source>","string*HtmlAttribute list";"<track>","string*HtmlAttribute list";"<wbr>","string*HtmlAttribute list";"TAGEND","string";"TAGSTART","string*HtmlAttribute list";"CDATA","string";"COMMENT","string";"DOCTYPE","string";"TEXT","string";"TAGSELFCLOSING","string*HtmlAttribute list";"document","string*HtmlNode list";"element","HtmlNode";"node","HtmlNode";"nodes","HtmlNode list";"voidElement","HtmlNode"|]
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
                if s2 = name then
                    HtmlElement(name, attrs,s1)
                else failwithf "%A ... %s" s0 s2
            box result
    ["element";"voidElement"],fun (ss:obj[]) ->
            let s0 = unbox<HtmlNode> ss.[0]
            let result:HtmlNode =
                s0
            box result
    ["voidElement";"<area>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<area>";"</area>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<base>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<base>";"</base>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<br>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<br>";"</br>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<col>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<col>";"</col>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<embed>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<embed>";"</embed>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<hr>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<hr>";"</hr>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<img>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<img>";"</img>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<input>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<input>";"</input>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<link>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<link>";"</link>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<meta>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<meta>";"</meta>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<param>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<param>";"</param>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<source>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<source>";"</source>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<track>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<track>";"</track>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<wbr>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
    ["voidElement";"<wbr>";"</wbr>"],fun (ss:obj[]) ->
            let s0 = unbox<string*HtmlAttribute list> ss.[0]
            let result:HtmlNode =
                HtmlElement(fst s0,snd s0, [])
            box result
|]
open FslexFsyacc.Runtime
let parser = Parser(fxRules, actions, closures)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string*HtmlNode list>