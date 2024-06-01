module FSharp.HTML.TagLeftParseTable
let tokens = set ["ATTR_NAME";"ATTR_VALUE";"DIV_RANGLE";"LANGLE";"RANGLE"]
let kernels = [[0,0];[0,1];[-1,1;-2,1];[-2,2];[-4,1;-7,2];[-4,2];[-5,1];[-6,1];[-7,1];[-7,3]]
let kernelSymbols = ["";"tagleft";"ATTR_NAME";"ATTR_VALUE";"attributes";"attribute";"DIV_RANGLE";"RANGLE";"LANGLE";"closeAngle"]
let actions = [["LANGLE",8;"tagleft",1];["",0];["ATTR_NAME",-1;"ATTR_VALUE",3;"DIV_RANGLE",-1;"RANGLE",-1];["ATTR_NAME",-2;"DIV_RANGLE",-2;"RANGLE",-2];["ATTR_NAME",2;"DIV_RANGLE",6;"RANGLE",7;"attribute",5;"closeAngle",9];["ATTR_NAME",-4;"DIV_RANGLE",-4;"RANGLE",-4];["",-5];["",-6];["ATTR_NAME",-3;"DIV_RANGLE",-3;"RANGLE",-3;"attributes",4];["",-7]]
open FSharp.HTML
open FslexFsyacc.Runtime
let rules : list<string list*(obj list->obj)> = [
    ["";"tagleft"], fun(ss:obj list)-> ss.[0]
    ["attribute";"ATTR_NAME"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string*string =
            s0,""
        box result
    ["attribute";"ATTR_NAME";"ATTR_VALUE"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:string*string =
            s0,s1
        box result
    ["attributes"], fun(ss:obj list)->
        let result:list<string*string> =
            []
        box result
    ["attributes";"attributes";"attribute"], fun(ss:obj list)->
        let s0 = unbox<list<string*string>> ss.[0]
        let s1 = unbox<string*string> ss.[1]
        let result:list<string*string> =
            s1::s0
        box result
    ["closeAngle";"DIV_RANGLE"], fun(ss:obj list)->
        let s0 = unbox<int> ss.[0]
        let result:int*string =
            s0,"/>"
        box result
    ["closeAngle";"RANGLE"], fun(ss:obj list)->
        let s0 = unbox<int> ss.[0]
        let result:int*string =
            s0,">"
        box result
    ["tagleft";"LANGLE";"attributes";"closeAngle"], fun(ss:obj list)->
        let s0 = unbox<int*string> ss.[0]
        let s1 = unbox<list<string*string>> ss.[1]
        let s2 = unbox<int*string> ss.[2]
        let result:Position<HtmlToken> =
            let i,tagname = s0
            let j,angle = s2
            let attrs = List.rev s1
            let postok =
                match angle with
                | ">"  -> TAGSTART(tagname,attrs)
                | "/>" -> TAGSELFCLOSING(tagname,attrs)
                | _ -> failwith ""
            {index=i;length=j-i;value=postok}
        box result
]
let unboxRoot =
    unbox<Position<HtmlToken>>
let app: FslexFsyacc.Runtime.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}