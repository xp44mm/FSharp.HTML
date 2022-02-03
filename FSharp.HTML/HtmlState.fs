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
        
        //HasFormattedParent: bool ref
        }

    member x.Pop() = x.Reader.Read() |> ignore
    member x.Peek() = x.Reader.PeekChar()
    member x.Pop(count) =
        [|0..(count-1)|] |> Array.map (fun _ -> x.Reader.ReadChar())

    member private x.Contents = (!x.Content).ToString()
    member x.ContentLength = (!x.Content).Length

    member x.NewAttribute() = 
        x.Attributes := (CharList.Empty, CharList.Empty) :: (!x.Attributes)

    member x.ConsAttrName() =
        match !x.Attributes with
        | [] -> x.NewAttribute(); x.ConsAttrName()
        | (h,_) :: _ -> 
            let c = Char.ToLowerInvariant(x.Reader.ReadChar())
            h.Cons(c)

    member x.CurrentTagName() =
        (!x.CurrentTag).ToString().Trim()

    member private x.CurrentAttrName() =
        match !x.Attributes with
        | [] -> String.Empty
        | (h,_) :: _ -> h.ToString()

    member x.ConsAttrValue(c) =
        match !x.Attributes with
        | [] -> 
            x.NewAttribute(); 
            x.ConsAttrValue(c)
        | (_,h) :: _ -> 
            h.Cons(c)

    member x.ConsAttrValue() =
        let c = x.Reader.ReadChar()
        x.ConsAttrValue(c)

    member private x.GetAttributes() =
        !x.Attributes
        |> List.choose (fun (key, value) ->
            if key.Length > 0 then 
                let attr = HtmlAttribute(key.ToString(), value.ToString())
                Some attr
            else None)
        |> List.rev

    member x.EmitSelfClosingTag() =
        let name = (!x.CurrentTag).ToString().Trim()
        let result = Tag(true, name, x.GetAttributes())
        x.CurrentTag := CharList.Empty
        x.InsertionMode := DefaultMode
        x.Attributes := []
        x.Tokens := result :: !x.Tokens

    member private x.IsFormattedTag
        with get() =
            match x.CurrentTagName().ToLower() with
            | "pre" -> true
            | _ -> false

    member x.IsScriptTag
        with get() =
            match x.CurrentTagName().ToLower() with
            | "script" | "style" -> true
            | _ -> false

    member x.EmitTag(isEnd) =
        let name = (!x.CurrentTag).ToString().Trim()
        let result =
            if isEnd then
                if x.ContentLength > 0 then x.Emit() 
                TagEnd(name)
            else 
                Tag(false, name, x.GetAttributes())

        //// pre is the only default formatted tag, nested pres are not
        //// allowed in the spec.
        //if x.IsFormattedTag then
        //    x.HasFormattedParent := not isEnd
        //else
        //    x.HasFormattedParent := !x.HasFormattedParent || x.IsFormattedTag

        x.InsertionMode :=
            if x.IsScriptTag && (not isEnd) then ScriptMode
            else DefaultMode

        x.CurrentTag := CharList.Empty
        x.Attributes := []
        x.Tokens := result :: !x.Tokens

    member x.EmitToAttributeValue() =
        assert (!x.InsertionMode = InsertionMode.CharRefMode)
        let content = (!x.Content).ToString() |> HtmlCharRefs.substitute
        for c in content.ToCharArray() do
            x.ConsAttrValue c
        x.Content := CharList.Empty
        x.InsertionMode := DefaultMode

    // emit a token
    member x.Emit() : unit =
        let result =
            let content = (!x.Content).ToString()
            match !x.InsertionMode with
            | DefaultMode ->
                //if !x.HasFormattedParent then
                    Text content
                //else
                //    let normalizedContent = wsRegex.Value.Replace(content, " ")
                //    if normalizedContent = " " then Text "" else Text normalizedContent
            | ScriptMode -> content |> Text
            | CharRefMode -> content.Trim() |> HtmlCharRefs.substitute |> Text
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

    // for content cons a char from stream
    member x.Cons() = 
        let c = x.Reader.ReadChar()
        (!x.Content).Cons(c)

    member x.Cons(char) = 
        (!x.Content).Cons(char)

    member x.Cons(char) = 
        char
        |> Array.iter ((!x.Content).Cons)

    member x.Cons(char : string) = 
        x.Cons(char.ToCharArray())

    member x.ConsTag() =
        match x.Reader.ReadChar() with
        | TextParser.Whitespace _ -> ()
        | a -> (!x.CurrentTag).Cons(Char.ToLowerInvariant a)

    member private x.ClearContent() =
        (!x.Content).Clear()

    static member Create (reader:TextReader) =
        { 
            Attributes = ref []
            CurrentTag = ref CharList.Empty
            Content = ref CharList.Empty
            //HasFormattedParent = ref false
            InsertionMode = ref DefaultMode
            Tokens = ref []
            Reader = reader 
            }
