module FSharp.HTML.TryTokenizer

open System
open System.Text.RegularExpressions

open FSharp.Idioms.RegularExpressions

let tryWS =
    Regex @"^\s+"
    |> trySearch

let tryLineTerminator =
    Regex @"^(\r?\n|\r)"
    |> trySearch

let TagNameChar = @"[-:\.\w]"

let tryEndTag =
   Regex($"^</{TagNameChar}+\\s*>")
   |> trySearch

let tryStartTagOpen =
    Regex($"^<{TagNameChar}+")
    |> trySearch

///所有解析失败的标签归类为bogus
let tryBogusComment =
    Regex(@"^<[?/![^>]*>")
    |> trySearch

let tryNamedCharacterReference =
   Regex(@"^&\w+;")
   |> trySearch

let tryDecimalNumericCharacterReference =
   Regex(@"^&#\d+;")
   |> trySearch

let tryHexadecimalNumericCharacterReference =
   Regex(@"^&#[xX][0-9a-fA-F]+;")
   |> trySearch

let trySingleLineComment =
    Regex @"^//[^\r\n]*"
    |> trySearch

let tryMultiLineComment =
    Regex @"^/\*[\s\S]*?\*/"
    |> trySearch

let trySingleStringLiteral =
    Regex @"^'(\\\\|\\'|[^'])*'"
    |> trySearch

let tryDoubleStringLiteral =
    Regex """^"(\\\\|\\"|[^"])*(")"""
    |> trySearch

let tryGraveAccent =
    Regex @"^`(\\\\|\\`|[^`])*`"
    |> trySearch

let tryRegularExpressionLiteral =
    Regex @"^/(\\\\|\\/|[^/])+/[gimsuy]*"
    |> trySearch


