module FSharp.HTML.TryTokenizer

open System
open System.Text.RegularExpressions

open FSharp.Idioms

let tryWS =
    Regex @"^\s+"
    |> tryRegexMatch

let tryLineTerminator =
    Regex @"^(\r?\n|\r)"
    |> tryRegexMatch

// mixture of text and character references
let tryText =
   Regex(@"^[^<]+")
   |> tryRegexMatch

let tryDOCTYPE =
    Regex(@"^<!DOCTYPE\s+[^>]*>",RegexOptions.IgnoreCase)
    |> tryRegexMatch

let tryComment =
    Regex(@"^<!--[\s\S]*?-->")
    |> tryRegexMatch

let tryCDATA =
    Regex(@"^<!\[CDATA\[[\s\S]*?\]\]>",RegexOptions.IgnoreCase)
    |> tryRegexMatch

let TagNameChar = "[-:\.\w]"

let tryEndTag =
   Regex($"^</{TagNameChar}+\\s*>")
   |> tryRegexMatch

let tryStartTagOpen =
    Regex($"^<{TagNameChar}+")
    |> tryRegexMatch

// """, "'", "=", ">", "/"
let tryAttributeName =
    Regex(@"^[\S-[""'=>/]]+")
    |> tryRegexMatch

// """, "'", "=", ">", "<", "`"
let tryUnquotedAttributeValue =
    Regex(@"^=\s*[\S-[""'=><`]]+")
    |> tryRegexMatch

let tryQuotedAttributeValue =
    Regex(@"^=\s*([""'])[^\1]*?\1")
    |> tryRegexMatch

///所有解析失败的标签归类为bogus
let tryBogusComment =
    Regex(@"^<[?/![^>]*>")
    |> tryRegexMatch

let tryNamedCharacterReference =
   Regex(@"^&\w+;")
   |> tryRegexMatch

let tryDecimalNumericCharacterReference =
   Regex(@"^&#\d+;")
   |> tryRegexMatch

let tryHexadecimalNumericCharacterReference =
   Regex(@"^&#[xX][0-9a-fA-F]+;")
   |> tryRegexMatch

// --- javascript ====

let trySingleLineComment =
    Regex @"^//.*"
    |> tryRegexMatch

let tryMultiLineComment =
    Regex @"^/\*[\s\S]*?\*/"
    |> tryRegexMatch

let trySingleStringLiteral =
    Regex @"^'(\\\\|\\'|[^'])*'"
    |> tryRegexMatch

let tryDoubleStringLiteral =
    Regex """^"(\\\\|\\"|[^"])*(")"""
    |> tryRegexMatch

let tryGraveAccent =
    Regex @"^`(\\\\|\\`|[^`])*`"
    |> tryRegexMatch

let tryRegularExpressionLiteral =
    Regex @"^/(\\\\|\\/|[^/])+/[gimsuy]*"
    |> tryRegexMatch


