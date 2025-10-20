module FSharp.HTML.AttributeYacc
open FSharp.LexYacc
let stateSymbols: string[] = [|"";"tag";"ID";"=";"attributeValue";"ID";"QUOTED";"attributes";"attribute";"/>";">";"<";"ID";"close"|]
let actions: (string * int) list list = [["<",11;"tag",1];["",0];["/>",-1;"=",3;">",-1;"ID",-1];["ID",5;"QUOTED",6;"attributeValue",4];["/>",-2;">",-2;"ID",-2];["/>",-3;">",-3;"ID",-3];["/>",-4;">",-4;"ID",-4];["/>",9;">",10;"ID",2;"attribute",8;"close",13];["/>",-6;">",-6;"ID",-6];["",-7];["",-8];["ID",12];["/>",-5;">",-5;"ID",-5;"attributes",7];["",-9]]
let rules: list<string list*(obj list->obj)> = [
    ["";"tag"], fun (ss:obj list) -> ss.[0]
    ["attribute";"ID"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string*string =
            s0,""
        box result
    ["attribute";"ID";"=";"attributeValue"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<string> ss.[2]
        let result:string*string =
            s0,s2
        box result
    ["attributeValue";"ID"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["attributeValue";"QUOTED"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
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
    ["close";"/>"], fun(ss:obj list)->
        let result:bool =
            false
        box result
    ["close";">"], fun(ss:obj list)->
        let result:bool =
            true
        box result
    ["tag";"<";"ID";"attributes";"close"], fun(ss:obj list)->
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<list<string*string>> ss.[2]
        let s3 = unbox<bool> ss.[3]
        let result:HtmlToken =
            let name = s1
            let attributes = List.rev s2
            if s3 && not (TagNames.voidElements.Contains name) then
                TAGSTART(name,attributes)
            else
                TAGSELFCLOSING(name,attributes)
        box result
]
let unboxRoot =
    unbox<HtmlToken>
let parser = ParseTable.from(rules, actions)