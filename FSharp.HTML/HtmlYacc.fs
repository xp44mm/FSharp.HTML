module FSharp.HTML.HtmlYacc
open FSharp.LexYacc
let stateSymbols: string[] = [|"";"html";"nodes";"CDATA";"COMMENT";"DOCTYPE";"TAGSELFCLOSING";"TAGSTART";"nodes";"TAGEND";"TEXT";"WS";"require_nodes";"node";"node"|]
let actions: (string * int) list list = [["",-9;"CDATA",3;"COMMENT",4;"DOCTYPE",5;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",10;"WS",11;"html",1;"node",13;"nodes",2;"require_nodes",12];["",0];["",-1];["",-2;"CDATA",-2;"COMMENT",-2;"DOCTYPE",-2;"TAGEND",-2;"TAGSELFCLOSING",-2;"TAGSTART",-2;"TEXT",-2;"WS",-2];["",-3;"CDATA",-3;"COMMENT",-3;"DOCTYPE",-3;"TAGEND",-3;"TAGSELFCLOSING",-3;"TAGSTART",-3;"TEXT",-3;"WS",-3];["",-4;"CDATA",-4;"COMMENT",-4;"DOCTYPE",-4;"TAGEND",-4;"TAGSELFCLOSING",-4;"TAGSTART",-4;"TEXT",-4;"WS",-4];["",-5;"CDATA",-5;"COMMENT",-5;"DOCTYPE",-5;"TAGEND",-5;"TAGSELFCLOSING",-5;"TAGSTART",-5;"TEXT",-5;"WS",-5];["CDATA",3;"COMMENT",4;"DOCTYPE",5;"TAGEND",-9;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",10;"WS",11;"node",13;"nodes",8;"require_nodes",12];["TAGEND",9];["",-6;"CDATA",-6;"COMMENT",-6;"DOCTYPE",-6;"TAGEND",-6;"TAGSELFCLOSING",-6;"TAGSTART",-6;"TEXT",-6;"WS",-6];["",-7;"CDATA",-7;"COMMENT",-7;"DOCTYPE",-7;"TAGEND",-7;"TAGSELFCLOSING",-7;"TAGSTART",-7;"TEXT",-7;"WS",-7];["",-8;"CDATA",-8;"COMMENT",-8;"DOCTYPE",-8;"TAGEND",-8;"TAGSELFCLOSING",-8;"TAGSTART",-8;"TEXT",-8;"WS",-8];["",-10;"CDATA",3;"COMMENT",4;"DOCTYPE",5;"TAGEND",-10;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",10;"WS",11;"node",14];["",-11;"CDATA",-11;"COMMENT",-11;"DOCTYPE",-11;"TAGEND",-11;"TAGSELFCLOSING",-11;"TAGSTART",-11;"TEXT",-11;"WS",-11];["",-12;"CDATA",-12;"COMMENT",-12;"DOCTYPE",-12;"TAGEND",-12;"TAGSELFCLOSING",-12;"TAGSTART",-12;"TEXT",-12;"WS",-12]]
let rules: list<string list*(obj list->obj)> = [
    ["";"html"], fun (ss:obj list) -> ss.[0]
    ["html";"nodes"], fun(ss:obj list)->
        let s0 = unbox<HtmlNode list> ss.[0]
        let result:HtmlNode list =
            List.rev s0
        box result
    ["node";"CDATA"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlCData s0
        box result
    ["node";"COMMENT"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlComment s0
        box result
    ["node";"DOCTYPE"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlDoctype s0
        box result
    ["node";"TAGSELFCLOSING"], fun(ss:obj list)->
        let s0 = unbox<string * list<string*string>> ss.[0]
        let result:HtmlNode =
            let name, attrs = s0
            HtmlElement(name, attrs, [])
        box result
    ["node";"TAGSTART";"nodes";"TAGEND"], fun(ss:obj list)->
        let s0 = unbox<string * list<string*string>> ss.[0]
        let s1 = unbox<HtmlNode list> ss.[1]
        let s2 = unbox<string> ss.[2]
        let result:HtmlNode =
            let name, attrs = s0
            if name = s2 then
                HtmlElement(name, attrs, List.rev s1)
            else
                failwith $"unmatched tag:<{name}></{s2}>"
        box result
    ["node";"TEXT"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlText s0
        box result
    ["node";"WS"], fun(ss:obj list)->
        let result:HtmlNode =
            HtmlText ""
        box result
    ["nodes"], fun(ss:obj list)->
        let result:HtmlNode list =
            []
        box result
    ["nodes";"require_nodes"], fun(ss:obj list)->
        let s0 = unbox<HtmlNode list> ss.[0]
        let result:HtmlNode list =
            s0
        box result
    ["require_nodes";"node"], fun(ss:obj list)->
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode list =
            [s0]
        box result
    ["require_nodes";"require_nodes";"node"], fun(ss:obj list)->
        let s0 = unbox<HtmlNode list> ss.[0]
        let s1 = unbox<HtmlNode> ss.[1]
        let result:HtmlNode list =
            s1::s0
        box result
]
let unboxRoot =
    unbox<HtmlNode list>
let parser = ParseTable.from(rules, actions)