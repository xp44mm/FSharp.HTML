module FSharp.HTML.HtmlParseTable
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils"
let productions = [|0,[|"";"document"|];-1,[|"document";"DOCTYPE";";";"nodes"|];-2,[|"document";"nodes"|];-3,[|"element";"TAGSELFCLOSING"|];-4,[|"element";"TAGSTART";"nodes";"TAGEND"|];-5,[|"element";"voidElement"|];-6,[|"node";"CDATA"|];-7,[|"node";"COMMENT"|];-8,[|"node";"TEXT"|];-9,[|"node";"element"|];-10,[|"nodes"|];-11,[|"nodes";"node"|];-12,[|"nodes";"nodes";";";"node"|];-13,[|"voidElement";"<area>"|];-14,[|"voidElement";"<area>";"</area>"|];-15,[|"voidElement";"<base>"|];-16,[|"voidElement";"<base>";"</base>"|];-17,[|"voidElement";"<br>"|];-18,[|"voidElement";"<br>";"</br>"|];-19,[|"voidElement";"<col>"|];-20,[|"voidElement";"<col>";"</col>"|];-21,[|"voidElement";"<embed>"|];-22,[|"voidElement";"<embed>";"</embed>"|];-23,[|"voidElement";"<hr>"|];-24,[|"voidElement";"<hr>";"</hr>"|];-25,[|"voidElement";"<img>"|];-26,[|"voidElement";"<img>";"</img>"|];-27,[|"voidElement";"<input>"|];-28,[|"voidElement";"<input>";"</input>"|];-29,[|"voidElement";"<link>"|];-30,[|"voidElement";"<link>";"</link>"|];-31,[|"voidElement";"<meta>"|];-32,[|"voidElement";"<meta>";"</meta>"|];-33,[|"voidElement";"<param>"|];-34,[|"voidElement";"<param>";"</param>"|];-35,[|"voidElement";"<source>"|];-36,[|"voidElement";"<source>";"</source>"|];-37,[|"voidElement";"<track>"|];-38,[|"voidElement";"<track>";"</track>"|];-39,[|"voidElement";"<wbr>"|];-40,[|"voidElement";"<wbr>";"</wbr>"|]|]
let actions = [|0,[|"",-10;";",-10;"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"DOCTYPE",2;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"document",1;"element",14;"node",15;"nodes",5;"voidElement",10|];1,[|"",0|];2,[|";",3|];3,[|"",-10;";",-10;"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"element",14;"node",15;"nodes",4;"voidElement",10|];4,[|"",-1;";",16|];5,[|"",-2;";",16|];6,[|"",-3;";",-3;"TAGEND",-3|];7,[|";",-10;"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"TAGEND",-10;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"element",14;"node",15;"nodes",8;"voidElement",10|];8,[|";",16;"TAGEND",9|];9,[|"",-4;";",-4;"TAGEND",-4|];10,[|"",-5;";",-5;"TAGEND",-5|];11,[|"",-6;";",-6;"TAGEND",-6|];12,[|"",-7;";",-7;"TAGEND",-7|];13,[|"",-8;";",-8;"TAGEND",-8|];14,[|"",-9;";",-9;"TAGEND",-9|];15,[|"",-11;";",-11;"TAGEND",-11|];16,[|"<area>",18;"<base>",20;"<br>",22;"<col>",24;"<embed>",26;"<hr>",28;"<img>",30;"<input>",32;"<link>",34;"<meta>",36;"<param>",38;"<source>",40;"<track>",42;"<wbr>",44;"CDATA",11;"COMMENT",12;"TAGSELFCLOSING",6;"TAGSTART",7;"TEXT",13;"element",14;"node",17;"voidElement",10|];17,[|"",-12;";",-12;"TAGEND",-12|];18,[|"",-13;";",-13;"</area>",19;"TAGEND",-13|];19,[|"",-14;";",-14;"TAGEND",-14|];20,[|"",-15;";",-15;"</base>",21;"TAGEND",-15|];21,[|"",-16;";",-16;"TAGEND",-16|];22,[|"",-17;";",-17;"</br>",23;"TAGEND",-17|];23,[|"",-18;";",-18;"TAGEND",-18|];24,[|"",-19;";",-19;"</col>",25;"TAGEND",-19|];25,[|"",-20;";",-20;"TAGEND",-20|];26,[|"",-21;";",-21;"</embed>",27;"TAGEND",-21|];27,[|"",-22;";",-22;"TAGEND",-22|];28,[|"",-23;";",-23;"</hr>",29;"TAGEND",-23|];29,[|"",-24;";",-24;"TAGEND",-24|];30,[|"",-25;";",-25;"</img>",31;"TAGEND",-25|];31,[|"",-26;";",-26;"TAGEND",-26|];32,[|"",-27;";",-27;"</input>",33;"TAGEND",-27|];33,[|"",-28;";",-28;"TAGEND",-28|];34,[|"",-29;";",-29;"</link>",35;"TAGEND",-29|];35,[|"",-30;";",-30;"TAGEND",-30|];36,[|"",-31;";",-31;"</meta>",37;"TAGEND",-31|];37,[|"",-32;";",-32;"TAGEND",-32|];38,[|"",-33;";",-33;"</param>",39;"TAGEND",-33|];39,[|"",-34;";",-34;"TAGEND",-34|];40,[|"",-35;";",-35;"</source>",41;"TAGEND",-35|];41,[|"",-36;";",-36;"TAGEND",-36|];42,[|"",-37;";",-37;"</track>",43;"TAGEND",-37|];43,[|"",-38;";",-38;"TAGEND",-38|];44,[|"",-39;";",-39;"</wbr>",45;"TAGEND",-39|];45,[|"",-40;";",-40;"TAGEND",-40|]|]
let kernelSymbols = [|1,"document";2,"DOCTYPE";3,";";4,"nodes";5,"nodes";6,"TAGSELFCLOSING";7,"TAGSTART";8,"nodes";9,"TAGEND";10,"voidElement";11,"CDATA";12,"COMMENT";13,"TEXT";14,"element";15,"node";16,";";17,"node";18,"<area>";19,"</area>";20,"<base>";21,"</base>";22,"<br>";23,"</br>";24,"<col>";25,"</col>";26,"<embed>";27,"</embed>";28,"<hr>";29,"</hr>";30,"<img>";31,"</img>";32,"<input>";33,"</input>";34,"<link>";35,"</link>";36,"<meta>";37,"</meta>";38,"<param>";39,"</param>";40,"<source>";41,"</source>";42,"<track>";43,"</track>";44,"<wbr>";45,"</wbr>"|]
let semantics = [|-1,"s0,List.rev s2";-2,"\"\",List.rev s0";-3,"let name,attrs = s0\r\nHtmlElement(name, attrs,[])";-4,"let name,attrs = s0\r\nif s2 = name then\r\n    HtmlElement(name, attrs,s1)\r\nelse failwithf \"%A ... %s\" s0 s2";-5,"s0";-6,"HtmlCData s0";-7,"HtmlComment s0";-8,"HtmlText s0";-9,"s0";-10,"[]";-11,"[s0]";-12,"s2::s0";-13,"HtmlElement(fst s0,snd s0, [])";-14,"HtmlElement(fst s0,snd s0, [])";-15,"HtmlElement(fst s0,snd s0, [])";-16,"HtmlElement(fst s0,snd s0, [])";-17,"HtmlElement(fst s0,snd s0, [])";-18,"HtmlElement(fst s0,snd s0, [])";-19,"HtmlElement(fst s0,snd s0, [])";-20,"HtmlElement(fst s0,snd s0, [])";-21,"HtmlElement(fst s0,snd s0, [])";-22,"HtmlElement(fst s0,snd s0, [])";-23,"HtmlElement(fst s0,snd s0, [])";-24,"HtmlElement(fst s0,snd s0, [])";-25,"HtmlElement(fst s0,snd s0, [])";-26,"HtmlElement(fst s0,snd s0, [])";-27,"HtmlElement(fst s0,snd s0, [])";-28,"HtmlElement(fst s0,snd s0, [])";-29,"HtmlElement(fst s0,snd s0, [])";-30,"HtmlElement(fst s0,snd s0, [])";-31,"HtmlElement(fst s0,snd s0, [])";-32,"HtmlElement(fst s0,snd s0, [])";-33,"HtmlElement(fst s0,snd s0, [])";-34,"HtmlElement(fst s0,snd s0, [])";-35,"HtmlElement(fst s0,snd s0, [])";-36,"HtmlElement(fst s0,snd s0, [])";-37,"HtmlElement(fst s0,snd s0, [])";-38,"HtmlElement(fst s0,snd s0, [])";-39,"HtmlElement(fst s0,snd s0, [])";-40,"HtmlElement(fst s0,snd s0, [])"|]
let declarations = [|"<area>","string*HtmlAttribute list";"<base>","string*HtmlAttribute list";"<br>","string*HtmlAttribute list";"<col>","string*HtmlAttribute list";"<embed>","string*HtmlAttribute list";"<hr>","string*HtmlAttribute list";"<img>","string*HtmlAttribute list";"<input>","string*HtmlAttribute list";"<link>","string*HtmlAttribute list";"<meta>","string*HtmlAttribute list";"<param>","string*HtmlAttribute list";"<source>","string*HtmlAttribute list";"<track>","string*HtmlAttribute list";"<wbr>","string*HtmlAttribute list";"TAGEND","string";"TAGSTART","string*HtmlAttribute list";"CDATA","string";"COMMENT","string";"DOCTYPE","string";"TEXT","string";"TAGSELFCLOSING","string*HtmlAttribute list";"document","string*HtmlNode list";"element","HtmlNode";"node","HtmlNode";"nodes","HtmlNode list";"voidElement","HtmlNode"|]
open System
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
let mappers:(int*(obj[]->obj))[] = [|
    -1,fun (ss:obj[]) ->
        // document -> DOCTYPE ";" nodes
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<HtmlNode list> ss.[2]
        let result:string*HtmlNode list =
            s0,List.rev s2
        box result
    -2,fun (ss:obj[]) ->
        // document -> nodes
        let s0 = unbox<HtmlNode list> ss.[0]
        let result:string*HtmlNode list =
            "",List.rev s0
        box result
    -3,fun (ss:obj[]) ->
        // element -> TAGSELFCLOSING
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            let name,attrs = s0
            HtmlElement(name, attrs,[])
        box result
    -4,fun (ss:obj[]) ->
        // element -> TAGSTART nodes TAGEND
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let s1 = unbox<HtmlNode list> ss.[1]
        let s2 = unbox<string> ss.[2]
        let result:HtmlNode =
            let name,attrs = s0
            if s2 = name then
                HtmlElement(name, attrs,s1)
            else failwithf "%A ... %s" s0 s2
        box result
    -5,fun (ss:obj[]) ->
        // element -> voidElement
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode =
            s0
        box result
    -6,fun (ss:obj[]) ->
        // node -> CDATA
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlCData s0
        box result
    -7,fun (ss:obj[]) ->
        // node -> COMMENT
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlComment s0
        box result
    -8,fun (ss:obj[]) ->
        // node -> TEXT
        let s0 = unbox<string> ss.[0]
        let result:HtmlNode =
            HtmlText s0
        box result
    -9,fun (ss:obj[]) ->
        // node -> element
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode =
            s0
        box result
    -10,fun (ss:obj[]) ->
        // nodes -> 
        let result:HtmlNode list =
            []
        box result
    -11,fun (ss:obj[]) ->
        // nodes -> node
        let s0 = unbox<HtmlNode> ss.[0]
        let result:HtmlNode list =
            [s0]
        box result
    -12,fun (ss:obj[]) ->
        // nodes -> nodes ";" node
        let s0 = unbox<HtmlNode list> ss.[0]
        let s2 = unbox<HtmlNode> ss.[2]
        let result:HtmlNode list =
            s2::s0
        box result
    -13,fun (ss:obj[]) ->
        // voidElement -> "<area>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -14,fun (ss:obj[]) ->
        // voidElement -> "<area>" "</area>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -15,fun (ss:obj[]) ->
        // voidElement -> "<base>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -16,fun (ss:obj[]) ->
        // voidElement -> "<base>" "</base>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -17,fun (ss:obj[]) ->
        // voidElement -> "<br>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -18,fun (ss:obj[]) ->
        // voidElement -> "<br>" "</br>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -19,fun (ss:obj[]) ->
        // voidElement -> "<col>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -20,fun (ss:obj[]) ->
        // voidElement -> "<col>" "</col>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -21,fun (ss:obj[]) ->
        // voidElement -> "<embed>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -22,fun (ss:obj[]) ->
        // voidElement -> "<embed>" "</embed>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -23,fun (ss:obj[]) ->
        // voidElement -> "<hr>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -24,fun (ss:obj[]) ->
        // voidElement -> "<hr>" "</hr>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -25,fun (ss:obj[]) ->
        // voidElement -> "<img>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -26,fun (ss:obj[]) ->
        // voidElement -> "<img>" "</img>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -27,fun (ss:obj[]) ->
        // voidElement -> "<input>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -28,fun (ss:obj[]) ->
        // voidElement -> "<input>" "</input>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -29,fun (ss:obj[]) ->
        // voidElement -> "<link>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -30,fun (ss:obj[]) ->
        // voidElement -> "<link>" "</link>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -31,fun (ss:obj[]) ->
        // voidElement -> "<meta>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -32,fun (ss:obj[]) ->
        // voidElement -> "<meta>" "</meta>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -33,fun (ss:obj[]) ->
        // voidElement -> "<param>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -34,fun (ss:obj[]) ->
        // voidElement -> "<param>" "</param>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -35,fun (ss:obj[]) ->
        // voidElement -> "<source>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -36,fun (ss:obj[]) ->
        // voidElement -> "<source>" "</source>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -37,fun (ss:obj[]) ->
        // voidElement -> "<track>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -38,fun (ss:obj[]) ->
        // voidElement -> "<track>" "</track>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -39,fun (ss:obj[]) ->
        // voidElement -> "<wbr>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result
    -40,fun (ss:obj[]) ->
        // voidElement -> "<wbr>" "</wbr>"
        let s0 = unbox<string*HtmlAttribute list> ss.[0]
        let result:HtmlNode =
            HtmlElement(fst s0,snd s0, [])
        box result|]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string*HtmlNode list>