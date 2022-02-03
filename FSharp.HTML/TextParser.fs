namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions


module TextParser =

    let toPattern f c = if f c then Some c else None

    let (|EndOfFile|_|) (c : char) =
        let value = c |> int
        if (value = -1 || value = 65535) then Some c else None

    let (|Whitespace|_|) = toPattern Char.IsWhiteSpace
    let (|LetterDigit|_|) = toPattern Char.IsLetterOrDigit
    let (|Letter|_|) = toPattern Char.IsLetter

    let wsRegex = lazy Regex("\\s+", RegexOptions.Compiled)
    let invalidTypeNameRegex = lazy Regex("[^0-9a-zA-Z_]+", RegexOptions.Compiled)
    let headingRegex = lazy Regex("""h\d""", RegexOptions.Compiled)

    type TextReader with
    
        member x.PeekChar() = x.Peek() |> char

        member x.ReadChar() = x.Read() |> char

        member x.ReadNChar(n) =
            let buffer = Array.zeroCreate n
            x.ReadBlock(buffer, 0, n) |> ignore
            String(buffer)
    
    

//[<AutoOpen>]
//module Utils =

//    let inline toLower (s:string) = s.ToLowerInvariant()
//    let inline getNameSet names = names |> Seq.map toLower |> Set.ofSeq

