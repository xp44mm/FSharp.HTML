module FSharp.HTML.HtmldocParseTable
let actions = [["CDATA",7;"COMMENT",8;"DOCTYPE",2;"EOF",-8;"TAGSELFCLOSING",9;"TAGSTART",10;"TEXT",13;"htmldoc",1;"node",15;"{node*}",5;"{node+}",14];["",0];["CDATA",7;"COMMENT",8;"EOF",-8;"TAGSELFCLOSING",9;"TAGSTART",10;"TEXT",13;"node",15;"{node*}",3;"{node+}",14];["EOF",4];["",-1];["EOF",6];["",-2];["CDATA",-3;"COMMENT",-3;"EOF",-3;"TAGEND",-3;"TAGSELFCLOSING",-3;"TAGSTART",-3;"TEXT",-3];["CDATA",-4;"COMMENT",-4;"EOF",-4;"TAGEND",-4;"TAGSELFCLOSING",-4;"TAGSTART",-4;"TEXT",-4];["CDATA",-5;"COMMENT",-5;"EOF",-5;"TAGEND",-5;"TAGSELFCLOSING",-5;"TAGSTART",-5;"TEXT",-5];["CDATA",7;"COMMENT",8;"TAGEND",-8;"TAGSELFCLOSING",9;"TAGSTART",10;"TEXT",13;"node",15;"{node*}",11;"{node+}",14];["TAGEND",12];["CDATA",-6;"COMMENT",-6;"EOF",-6;"TAGEND",-6;"TAGSELFCLOSING",-6;"TAGSTART",-6;"TEXT",-6];["CDATA",-7;"COMMENT",-7;"EOF",-7;"TAGEND",-7;"TAGSELFCLOSING",-7;"TAGSTART",-7;"TEXT",-7];["CDATA",7;"COMMENT",8;"EOF",-9;"TAGEND",-9;"TAGSELFCLOSING",9;"TAGSTART",10;"TEXT",13;"node",16];["CDATA",-10;"COMMENT",-10;"EOF",-10;"TAGEND",-10;"TAGSELFCLOSING",-10;"TAGSTART",-10;"TEXT",-10];["CDATA",-11;"COMMENT",-11;"EOF",-11;"TAGEND",-11;"TAGSELFCLOSING",-11;"TAGSTART",-11;"TEXT",-11]]
let closures = [[0,0,[];-1,0,[];-2,0,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,["EOF"];-9,0,[];-10,0,[];-11,0,[]];[0,1,[""]];[-1,1,[];-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,["EOF"];-9,0,[];-10,0,[];-11,0,[]];[-1,2,[]];[-1,3,[""]];[-2,1,[]];[-2,2,[""]];[-3,1,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]];[-4,1,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]];[-5,1,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]];[-3,0,[];-4,0,[];-5,0,[];-6,0,[];-6,1,[];-7,0,[];-8,0,["TAGEND"];-9,0,[];-10,0,[];-11,0,[]];[-6,2,[]];[-6,3,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]];[-7,1,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]];[-3,0,[];-4,0,[];-5,0,[];-6,0,[];-7,0,[];-9,1,["EOF";"TAGEND"];-11,1,[]];[-10,1,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]];[-11,2,["CDATA";"COMMENT";"EOF";"TAGEND";"TAGSELFCLOSING";"TAGSTART";"TEXT"]]]
open FSharp.HTML
let rules:(string list*(obj list->obj))list = [
    ["htmldoc";"DOCTYPE";"{node*}";"EOF"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<HtmlNode list> ss.[1]
        let result:string*HtmlNode list =
            s0,List.rev s1
        box result
    ["htmldoc";"{node*}";"EOF"],fun(ss:obj list)->
        let s0 = unbox<HtmlNode list> ss.[0]
        let result:string*HtmlNode list =
            "html",List.rev s0
        box result
    ["{node*}";"{node+}"],fun(ss:obj list)->
        let s0 = unbox<HtmlNode list> ss.[0]
        let result:HtmlNode list =
            s0
        box result
    ["{node*}"],fun(ss:obj list)->
        let result:HtmlNode list =
            []
        box result
    ["{node+}";"{node+}";"node"],fun(ss:obj list)->
        let s0 = unbox<HtmlNode list> ss.[0]
        let s1 = unbox<HtmlNode> ss.[1]
        let result:HtmlNode list =
            s1::s0
        box result
    ["{node+}";"node"],fun(ss:obj list)->
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode list =
            [s0]
        box result
    ["node";"TEXT"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlText s0
        box result
    ["node";"COMMENT"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlComment s0
        box result
    ["node";"CDATA"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlCData s0
        box result
    ["node";"TAGSELFCLOSING"],fun(ss:obj list)->
        let s0 = unbox<string * list<string*string>> ss.[0]
        let result:HtmlNode =
            let name,attrs = s0
            HtmlElement(name, attrs,[])
        box result
    ["node";"TAGSTART";"{node*}";"TAGEND"],fun(ss:obj list)->
        let s0 = unbox<string * list<string*string>> ss.[0]
        let s1 = unbox<HtmlNode list> ss.[1]
        let s2 = unbox<string> ss.[2]
        let result:HtmlNode =
            let name,attrs = s0
            if name = s2 then
                HtmlElement(name, attrs, List.rev s1)
            else
                failwith $"unmatched tag:<{name}></{s2}>"
        box result
]
let unboxRoot =
    unbox<string*HtmlNode list>
let theoryParser = FslexFsyacc.Runtime.TheoryParser.create(rules, actions, closures)
let stateSymbolPairs = theoryParser.getStateSymbolPairs()