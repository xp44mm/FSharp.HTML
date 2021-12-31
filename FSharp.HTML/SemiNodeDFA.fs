module FSharp.HTML.SemiNodeDFA
let nextStates = [|0u,[|"</area>",1u;"</base>",1u;"</br>",1u;"</col>",1u;"</embed>",1u;"</hr>",1u;"</img>",1u;"</input>",1u;"</link>",1u;"</meta>",1u;"</param>",1u;"</source>",1u;"</track>",1u;"</wbr>",1u;"<area>",1u;"<base>",1u;"<br>",1u;"<col>",1u;"<embed>",1u;"<hr>",1u;"<img>",1u;"<input>",1u;"<link>",1u;"<meta>",1u;"<param>",1u;"<source>",1u;"<track>",1u;"<wbr>",1u;"CDATA",1u;"COMMENT",1u;"DOCTYPE",1u;"EOF",3u;"TAGEND",1u;"TAGSELFCLOSING",1u;"TAGSTART",4u;"TEXT",1u|];1u,[|"<area>",2u;"<base>",2u;"<br>",2u;"<col>",2u;"<embed>",2u;"<hr>",2u;"<img>",2u;"<input>",2u;"<link>",2u;"<meta>",2u;"<param>",2u;"<source>",2u;"<track>",2u;"<wbr>",2u;"CDATA",2u;"COMMENT",2u;"TAGSELFCLOSING",2u;"TAGSTART",2u;"TEXT",2u|]|]
let lexemesFromFinal = [|2u,[|1u|]|]
let universalFinals = [|1u;2u;3u;4u|]
let indicesFromFinal = [|1u,2;2u,0;3u,1;4u,2|]
let header = "open System\r\nopen FSharp.HTML\r\nopen FSharp.HTML.HtmlTokenUtils\r\ntype token = HtmlToken"
let semantics = [|"[lexbuf.Head;SEMICOLON]";"[]";"lexbuf"|]
open System
open FSharp.HTML
open FSharp.HTML.HtmlTokenUtils
type token = HtmlToken
let mappers = [|
    fun (lexbuf:token list) ->
        [lexbuf.Head;SEMICOLON]
    fun (lexbuf:token list) ->
        []
    fun (lexbuf:token list) ->
        lexbuf
|]
let finalMappers =
    indicesFromFinal
    |> Array.map(fun(fnl, i) -> fnl,mappers.[i])
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)