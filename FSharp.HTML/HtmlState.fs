namespace FSharp.HTML
open FSharp.HTML.TextParser

open System
open System.IO

type CharList =
    { mutable Contents : char list }
    static member Empty = { Contents = [] }
    override x.ToString() = String(x.Contents |> List.rev |> List.toArray)

    member x.Length = x.Contents.Length
    member x.Cons(c) = x.Contents <- c :: x.Contents
    member x.Clear() = x.Contents <- []

type HtmlState =
    { 
        Attributes : (CharList * CharList) list ref
        CurrentTag : CharList ref
        Content : CharList ref
        InsertionMode : InsertionMode ref

        Tokens : HtmlToken list ref
        Reader : TextReader 
        
        }

    member x.Pop() = x.Reader.Read() |> ignore
    member x.Peek() = x.Reader.Peek() |> char
    member x.Pop(count) =
        [|0..(count-1)|] |> Array.map (fun _ -> x.Reader.Read()|>char)

    member x.ContentLength = (!x.Content).Length

    member x.NewAttribute() = 
        x.Attributes := (CharList.Empty, CharList.Empty) :: (!x.Attributes)

    member x.ConsAttrName() =
        match !x.Attributes with
        | [] -> 
            x.NewAttribute(); 
            x.ConsAttrName()
        | (h,_) :: _ -> 
            let c = x.Reader.Read()|>char
            h.Cons(Char.ToLowerInvariant c)

    member x.CurrentTagName() =
        (!x.CurrentTag).ToString().Trim()


    member x.ConsAttrValue(c) =
        match !x.Attributes with
        | [] -> 
            x.NewAttribute(); 
            x.ConsAttrValue(c)
        | (_,h) :: _ -> 
            h.Cons(c)

    member x.ConsAttrValue() =
        let c = x.Reader.Read()|>char
        x.ConsAttrValue(c)

    member private x.GetAttributes() =
        !x.Attributes
        |> List.choose (fun (key, value) ->
            if key.Length > 0 then 
                let attr = HtmlAttribute(key.ToString(), value.ToString())
                Some attr
            else None)
        |> List.rev

    member x.IsScriptTag
        with get() =
            match x.CurrentTagName().ToLower() with
            | "script" | "style" -> true
            | _ -> false


    member x.EmitSelfClosingTag() =
        let name = (!x.CurrentTag).ToString().Trim()
        let result = Tag(true, name, x.GetAttributes())
        x.CurrentTag := CharList.Empty
        x.InsertionMode := DefaultMode
        x.Attributes := []
        x.Tokens := result :: !x.Tokens

    member x.EmitTag(isEnd) =
        let name = (!x.CurrentTag).ToString().Trim()
        let result =
            if isEnd then
                if x.ContentLength > 0 then x.EmitFromContent() 
                TagEnd(name)
            else 
                Tag(false, name, x.GetAttributes())

        x.InsertionMode :=
            if x.IsScriptTag && (not isEnd) then 
                ScriptMode
            else 
                DefaultMode

        x.CurrentTag := CharList.Empty
        x.Attributes := []
        x.Tokens := result :: !x.Tokens

    member x.ConsAttributeValueFromContent() =
        assert (!x.InsertionMode = InsertionMode.CharRefMode)
        let content = (!x.Content).ToString() // |> HtmlCharRefs.substitute
        for c in content.ToCharArray() do
            x.ConsAttrValue c
        x.Content := CharList.Empty
        x.InsertionMode := DefaultMode

    // emit a token
    member x.EmitFromContent() =
        let result =
            let content = (!x.Content).ToString()
            match !x.InsertionMode with
            | DefaultMode ->
                Text content
            | ScriptMode -> content |> Text
            | CharRefMode -> 
                content.Trim() 
                //|> HtmlCharRefs.substitute 
                |> Text
            | CommentMode -> Comment content
            | DocTypeMode -> DocType content
            | CDATAMode -> CData (content.Replace("<![CDATA[", "").Replace("]]>", ""))
        x.Content := CharList.Empty
        x.InsertionMode := DefaultMode

        match result with
        | Text t when String.IsNullOrEmpty(t) -> 
            ()
        | _ -> 
            x.Tokens := result :: !x.Tokens

    member x.Cons() = 
        let c = x.Reader.Read()|>char
        (!x.Content).Cons(c)

    member x.Cons(char) = 
        (!x.Content).Cons(char)

    member x.Cons(char) = 
        char
        |> Array.iter ((!x.Content).Cons)

    member x.Cons(char : string) = 
        x.Cons(char.ToCharArray())

    member private x.ConsTag() =
        match x.Reader.Read()|>char with
        | TextParser.Whitespace _ -> ()
        | a -> (!x.CurrentTag).Cons(Char.ToLowerInvariant a)

    static member Create (reader:TextReader) =
        { 
            Attributes = ref []
            CurrentTag = ref CharList.Empty
            Content = ref CharList.Empty
            InsertionMode = ref DefaultMode
            Tokens = ref []
            Reader = reader 
            }
