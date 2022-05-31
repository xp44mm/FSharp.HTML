module FSharp.HTML.TryTokenizer

open System
open System.Text.RegularExpressions

open FSharp.Idioms

let tryWS =
    Regex @"^\s+"
    |> tryMatch

let tryLineTerminator =
    Regex @"^(\r?\n|\r)"
    |> tryMatch

// mixture of text and character references
let tryText =
   Regex(@"^[^<]+")
   |> tryMatch

let tryDOCTYPE =
    Regex(@"^<!DOCTYPE\s+[^>]*>",RegexOptions.IgnoreCase)
    |> tryMatch

let tryComment =
    Regex(@"^<!--[\s\S]*?-->")
    |> tryMatch

let tryCDATA =
    Regex(@"^<!\[CDATA\[[\s\S]*?\]\]>",RegexOptions.IgnoreCase)
    |> tryMatch

let TagNameChar = "[-:\.\w]"

let tryEndTag =
   Regex($"^</{TagNameChar}+\\s*>")
   |> tryMatch

let tryStartTagOpen =
    Regex($"^<{TagNameChar}+")
    |> tryMatch

// """, "'", "=", ">", "/"
let tryAttributeName =
    Regex(@"^[\S-[""'=>/]]+")
    |> tryMatch

// """, "'", "=", ">", "<", "`"
let tryUnquotedAttributeValue =
    Regex(@"^=\s*[\S-[""'=><`]]+")
    |> tryMatch

let tryQuotedAttributeValue =
    Regex(@"^=\s*([""'])[^\1]*?\1")
    |> tryMatch

///所有解析失败的标签归类为bogus
let tryBogusComment =
    Regex(@"^<[?/![^>]*>")
    |> tryMatch

let tryNamedCharacterReference =
   Regex(@"^&\w+;")
   |> tryMatch

let tryDecimalNumericCharacterReference =
   Regex(@"^&#\d+;")
   |> tryMatch

let tryHexadecimalNumericCharacterReference =
   Regex(@"^&#[xX][0-9a-fA-F]+;")
   |> tryMatch

let trySingleLineComment =
    Regex @"^//.*"
    |> tryMatch

let tryMultiLineComment =
    Regex @"^/\*[\s\S]*?\*/"
    |> tryMatch

let trySingleStringLiteral =
    Regex @"^'(\\\\|\\'|[^'])*'"
    |> tryMatch

let tryDoubleStringLiteral =
    Regex """^"(\\\\|\\"|[^"])*(")"""
    |> tryMatch

let tryGraveAccent =
    Regex @"^`(\\\\|\\`|[^`])*`"
    |> tryMatch

let tryRegularExpressionLiteral =
    Regex @"^/(\\\\|\\/|[^/])+/[gimsuy]*"
    |> tryMatch


