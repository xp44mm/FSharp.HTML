module FSharp.HTML.HtmlTokenizer

open System

/// Tokenises a stream into a sequence of HTML tokens.
let tokenise reader =
    let state = HtmlState.Create reader

    let rec data (state:HtmlState) =
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            state.Tokens := EOF :: !state.Tokens
        | '&' ->
            if state.ContentLength > 0 then
                state.EmitFromContent();
            else
                state.InsertionMode := CharRefMode
                charRef state
        | '<' ->
            if state.ContentLength > 0 then
                state.EmitFromContent();
            else 
                state.Pop(); 
                tagOpen state
        | _ ->
            match !state.InsertionMode with
            | DefaultMode -> 
                state.Cons(); 
                data state
            | ScriptMode -> 
                script state;
            | CharRefMode -> 
                charRef state
            | DocTypeMode -> 
                docType state
            | CommentMode -> 
                comment state
            | CDATAMode -> 
                data state
    // 
    and charRef state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            state.EmitFromContent()
        | ';' -> 
            state.Cons(); 
            state.EmitFromContent()
        | '<' -> 
            state.EmitFromContent()
        | _ -> 
            state.Cons(); 
            charRef state
    // prefix: '<'
    and tagOpen state =
        match state.Peek() with
        | '!' -> 
            state.Pop(); 
            markupDeclaration state
        | '?' -> 
            state.Pop(); 
            bogusComment state
        | '/' -> 
            state.Pop(); 
            endTagOpen state
        | TextParser.Letter _ -> 
            let a = state.Reader.Read() |> char
            (!state.CurrentTag).Cons(Char.ToLowerInvariant a)
            tagName false state
        | _ -> 
            state.Cons('<'); 
            data state
    // prefix: "<!"
    and markupDeclaration state =
        match state.Pop(2) with
        | [|'-';'-'|] -> 
            comment state
        | current ->
            match new String(Array.append current (state.Pop(5))) with
            | "DOCTYPE" -> 
                docType state
            | "[CDATA[" -> 
                state.Cons("<![CDATA[".ToCharArray()); 
                cData 0 state
            | _ -> 
                bogusComment state

    // prefix: "<!DOCTYPE"
    and docType state =
        match state.Peek() with
        | '>' ->
            state.Pop();
            state.InsertionMode := DocTypeMode
            state.EmitFromContent()
        | _ -> 
            state.Cons();
            docType state
    // prefix: "<![CDATA["
    and cData i (state:HtmlState) =
        match state.Peek() with
        | ']' when i = 0 || i = 1 ->
            state.Cons()
            cData (i + 1) state
        | '>' when i = 2 ->
            state.Cons()
            state.InsertionMode := CDATAMode
            state.EmitFromContent()
        | TextParser.EndOfFile _ ->
            state.InsertionMode := CDATAMode
            state.EmitFromContent()
        | _ ->
            state.Cons()
            cData 0 state

    and bogusComment state =
        let exitBogusComment state =
            state.InsertionMode := CommentMode
            state.EmitFromContent()
        //let rec bogusComment' (state:HtmlState) =
        //bogusComment' state
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            exitBogusComment state
        | '>' -> 
            state.Cons(); 
            exitBogusComment state
        | _ -> 
            state.Cons(); 
            bogusComment state
    // prefix: "<!--"
    and comment state =
        match state.Peek() with
        | TextParser.EndOfFile _ ->
            state.InsertionMode := CommentMode
            state.EmitFromContent();
        | '-' -> 
            state.Pop(); 
            commentEndDash state;
        | _ -> 
            state.Cons(); 
            comment state
    // prefix: "<!-- xxx -"
    and commentEndDash state =
        match state.Peek() with
        | TextParser.EndOfFile _ ->
            state.InsertionMode := CommentMode
            state.EmitFromContent();
        | '-' -> 
            state.Pop(); 
            commentEndState state
        | _ ->
            state.Cons(); 
            comment state;

    // prefix: "<!-- xxx --"
    and commentEndState state =
        match state.Peek() with
        | TextParser.EndOfFile _ ->
            state.InsertionMode := CommentMode
            state.EmitFromContent();
        | '>' ->
            state.Pop();
            state.InsertionMode := CommentMode
            state.EmitFromContent();
        | _ -> 
            state.Cons(); 
            comment state
    
    // prefix: '</'
    and endTagOpen state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            data state
        | TextParser.Letter _ ->
            let a = state.Reader.Read() |> char
            (!state.CurrentTag).Cons(Char.ToLowerInvariant a)
            tagName true state
        | '>' -> 
            state.Pop(); 
            data state
        | _ -> 
            comment state
    // // prefix: '<' | '</'
    and tagName isEndTag state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            state.EmitTag(isEndTag)
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            beforeAttributeName state
        | '/' -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(isEndTag)
        | _ -> 
            let a = state.Reader.Read() |> char
            (!state.CurrentTag).Cons(Char.ToLowerInvariant a)
            tagName isEndTag state
    // prefix: '< xxxx /'
    and selfClosingStartTag state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            data state
        | '>' -> 
            state.Pop(); 
            state.EmitSelfClosingTag()
        | _ -> 
            beforeAttributeName state

    // prefix: '< xxxx '
    and beforeAttributeName state =
        match state.Peek() with
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            beforeAttributeName state
        | '/' -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(false)
        | _ -> 
            attributeName state
    // prefix: 'tagName whitespace'
    and attributeName state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> 
            state.EmitTag(false)
        | TextParser.Whitespace _ -> 
            afterAttributeName state
        | '=' -> 
            state.Pop(); 
            beforeAttributeValue state
        | '/' -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(false)
        | TextParser.LetterDigit _ 
        | _ -> 
            state.ConsAttrName(); 
            attributeName state

    and afterAttributeName state =
        match state.Peek() with
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            afterAttributeName state
        | '/' -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(false)
        | '=' -> 
            state.Pop(); 
            beforeAttributeValue state
        | _ -> 
            state.NewAttribute(); 
            attributeName state



    and script state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | ''' -> 
            state.Cons(); scriptSingleQuoteString state
        | '"' -> 
            state.Cons(); scriptDoubleQuoteString state
        | '/' -> 
            state.Cons(); scriptSlash state
        | '<' -> 
            state.Pop(); scriptLessThanSign state
        | _ -> 
            state.Cons(); script state

    and scriptSingleQuoteString state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | ''' -> 
            state.Cons(); script state
        | '\\' -> 
            state.Cons(); scriptSingleQuoteStringBackslash state
        | _ -> 
            state.Cons(); scriptSingleQuoteString state
    and scriptDoubleQuoteString state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '"' -> 
            state.Cons(); script state
        | '\\' -> 
            state.Cons(); scriptDoubleQuoteStringBackslash state
        | _ -> 
            state.Cons(); scriptDoubleQuoteString state
    and scriptSingleQuoteStringBackslash state =
        match state.Peek() with
        | _ -> 
            state.Cons(); scriptSingleQuoteString state
    and scriptDoubleQuoteStringBackslash state =
        match state.Peek() with
        | _ -> 
            state.Cons(); scriptDoubleQuoteString state
    and scriptSlash state =
        match state.Peek() with
        | '/' -> 
            state.Cons(); 
            scriptSingleLineComment state
        | '*' -> 
            state.Cons(); 
            scriptMultiLineComment state
        | _ -> script state
    
    and scriptMultiLineComment state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '*' -> 
            state.Cons(); 
            scriptMultiLineCommentStar state
        | _ -> 
            state.Cons(); 
            scriptMultiLineComment state
    and scriptMultiLineCommentStar state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '/' -> 
            state.Cons(); 
            script state
        | _ -> 
            scriptMultiLineComment state
    and scriptSingleLineComment state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '\n' -> 
            state.Cons(); 
            script state
        | _ -> 
            state.Cons(); 
            scriptSingleLineComment state
    
    and scriptLessThanSign state =
        match state.Peek() with
        | '/' -> 
            state.Pop(); 
            scriptEndTagOpen state
        | '!' -> 
            state.Cons('<'); 
            state.Cons(); 
            scriptDataEscapeStart state
        | _ -> 
            state.Cons('<'); 
            state.Cons(); 
            script state
    // prefix: "<!"
    and scriptDataEscapeStart state =
        match state.Peek() with
        | '-' -> 
            state.Cons(); 
            scriptDataEscapeStartDash state
        | _ -> 
            script state
    // prefix: "<!-"
    and scriptDataEscapeStartDash state =
        match state.Peek() with
        | '-' -> 
            state.Cons(); 
            scriptDataEscapedDashDash state
        | _ -> 
            script state
    // prefix: "<!--"
    and scriptDataEscapedDashDash state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '-' -> 
            state.Cons(); 
            scriptDataEscapedDashDash state
        | '<' -> 
            state.Pop(); 
            scriptDataEscapedLessThanSign state
        | '>' -> 
            state.Cons(); 
            script state
        | _ -> 
            state.Cons(); 
            scriptDataEscaped state
    and scriptDataEscapedLessThanSign state =
        match state.Peek() with
        | '/' -> 
            state.Pop(); 
            scriptDataEscapedEndTagOpen state
        | TextParser.Letter _ -> 
            state.Cons('<'); 
            state.Cons(); 
            scriptDataDoubleEscapeStart state
        | _ -> 
            state.Cons('<'); 
            state.Cons(); 
            scriptDataEscaped state
    
    and scriptDataDoubleEscapeStart state =
        match state.Peek() with
        | TextParser.Whitespace _ | '/' | '>' when state.IsScriptTag -> 
            state.Cons(); 
            scriptDataDoubleEscaped state
        | TextParser.Letter _ -> 
            state.Cons(); 
            scriptDataDoubleEscapeStart state
        | _ -> 
            state.Cons(); 
            scriptDataEscaped state
    and scriptDataDoubleEscaped state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '-' -> 
            state.Cons(); scriptDataDoubleEscapedDash state
        | '<' -> 
            state.Cons(); scriptDataDoubleEscapedLessThanSign state
        | _ -> 
            state.Cons(); scriptDataDoubleEscaped state
    and scriptDataDoubleEscapedDash state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '-' -> 
            state.Cons(); scriptDataDoubleEscapedDashDash state
        | '<' -> 
            state.Cons(); scriptDataDoubleEscapedLessThanSign state
        | _ -> 
            state.Cons(); scriptDataDoubleEscaped state
    and scriptDataDoubleEscapedLessThanSign state =
        match state.Peek() with
        | '/' -> 
            state.Cons(); scriptDataDoubleEscapeEnd state
        | _ -> 
            state.Cons(); scriptDataDoubleEscaped state
    and scriptDataDoubleEscapeEnd state =
        match state.Peek() with
        | TextParser.Whitespace _ | '/' | '>' when state.IsScriptTag -> 
            state.Cons(); scriptDataDoubleEscaped state
        | TextParser.Letter _ -> 
            state.Cons(); scriptDataDoubleEscapeEnd state
        | _ -> 
            state.Cons(); scriptDataDoubleEscaped state
    and scriptDataDoubleEscapedDashDash state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '-' -> 
            state.Cons(); scriptDataDoubleEscapedDashDash state
        | '<' -> 
            state.Cons(); scriptDataDoubleEscapedLessThanSign state
        | '>' -> 
            state.Cons(); script state
        | _ -> 
            state.Cons(); scriptDataDoubleEscaped state
    and scriptDataEscapedEndTagOpen state =
        match state.Peek() with
        | TextParser.Letter _ -> 
            scriptDataEscapedEndTagName state
        | _ -> 
            state.Cons([|'<';'/'|]); 
            state.Cons(); scriptDataEscaped state
    and scriptDataEscapedEndTagName state =
        match state.Peek() with
        | TextParser.Whitespace _ when state.IsScriptTag -> 
            state.Pop(); beforeAttributeName state
        | '/' when state.IsScriptTag -> 
            state.Pop(); selfClosingStartTag state
        | '>' when state.IsScriptTag -> 
            state.Pop(); state.EmitTag(true);
        | '>' ->
            state.Cons([|'<'; '/'|]);
            state.Cons(state.CurrentTagName());
            (!state.CurrentTag).Clear()
            script state
        | TextParser.Letter _ -> 
            let a = state.Reader.Read() |> char
            (!state.CurrentTag).Cons(Char.ToLowerInvariant a)
            scriptDataEscapedEndTagName state
        | _ -> 
            state.Cons([|'<';'/'|]); state.Cons(); scriptDataEscaped state
    and scriptDataEscaped state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '-' -> 
            state.Cons(); scriptDataEscapedDash state
        | '<' -> 
            scriptDataEscapedLessThanSign state
        | _ -> 
            state.Cons(); scriptDataEscaped state
    and scriptDataEscapedDash state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | '-' -> 
            state.Cons(); scriptDataEscapedDashDash state
        | '<' -> 
            scriptDataEscapedLessThanSign state
        | _ -> 
            state.Cons(); scriptDataEscaped state
    
    and scriptEndTagOpen state =
        match state.Peek() with
        | TextParser.Letter _ -> 
            scriptEndTagName state
        | _ -> 
            state.Cons('<'); 
            state.Cons('/'); 
            script state

    and scriptEndTagName state =
        match state.Peek() with
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            beforeAttributeName state
        | '/' when state.IsScriptTag -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' when state.IsScriptTag -> 
            state.Pop(); 
            state.EmitTag(true);
        | TextParser.Letter _ -> 
            let a = state.Reader.Read() |> char
            (!state.CurrentTag).Cons(Char.ToLowerInvariant a)
            scriptEndTagName state
        | _ ->
            state.Cons([|'<'; '/'|]);
            state.Cons(state.CurrentTagName());
            (!state.CurrentTag).Clear()
            script state

    and beforeAttributeValue state =
        match state.Peek() with
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            beforeAttributeValue state
        | TextParser.EndOfFile _ -> 
            state.EmitTag(false)
        | '/' -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(false)
        | '"' -> 
            state.Pop(); 
            attributeValueQuoted '"' state
        | '\'' -> 
            state.Pop(); 
            attributeValueQuoted '\'' state
        | _ -> 
            attributeValueUnquoted state
    and attributeValueUnquoted state =
        match state.Peek() with
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            state.NewAttribute(); 
            beforeAttributeName state
        | '/' -> 
            state.Pop(); 
            attributeValueUnquotedSlash state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(false)
        | '&' ->
            assert (state.ContentLength = 0)
            state.InsertionMode := InsertionMode.CharRefMode
            attributeValueUnquotedCharRef  ['/'; '>'] state
        | _ -> 
            state.ConsAttrValue(); 
            attributeValueUnquoted state

    and attributeValueUnquotedSlash state =
        match state.Peek() with
        | '>' -> 
            selfClosingStartTag state
        | _ -> 
            state.ConsAttrValue('/'); 
            state.ConsAttrValue(); 
            attributeValueUnquoted state
    and attributeValueQuoted quote state =
        match state.Peek() with
        | TextParser.EndOfFile _ -> data state
        | c when c = quote -> 
            state.Pop(); 
            afterAttributeValueQuoted state
        | '&' ->
            assert (state.ContentLength = 0)
            state.InsertionMode := InsertionMode.CharRefMode
            attributeValueQuotedCharRef quote state
        | _ -> 
            state.ConsAttrValue(); 
            attributeValueQuoted quote state
    and attributeValueQuotedCharRef quote state =
        match state.Peek() with
        | ';' ->
            state.Cons()
            state.ConsAttributeValueFromContent()
            attributeValueQuoted quote state
        | TextParser.EndOfFile _ ->
            state.ConsAttributeValueFromContent()
            attributeValueQuoted quote state
        | c when c = quote ->
            state.ConsAttributeValueFromContent()
            attributeValueQuoted quote state
        | _ ->
            state.Cons()
            attributeValueQuotedCharRef quote state
    and attributeValueUnquotedCharRef stop state =
        match state.Peek() with
        | ';' ->
            state.Cons()
            state.ConsAttributeValueFromContent()
            attributeValueUnquoted state
        | TextParser.EndOfFile _ ->
            state.ConsAttributeValueFromContent()
            attributeValueUnquoted state
        | c when List.exists ((=) c) stop ->
            state.ConsAttributeValueFromContent()
            attributeValueUnquoted state
        | _ ->
            state.Cons()
            attributeValueUnquotedCharRef stop state
    and afterAttributeValueQuoted state =
        match state.Peek() with
        | TextParser.Whitespace _ -> 
            state.Pop(); 
            state.NewAttribute(); 
            afterAttributeValueQuoted state
        | '/' -> 
            state.Pop(); 
            selfClosingStartTag state
        | '>' -> 
            state.Pop(); 
            state.EmitTag(false)
        | _ -> 
            state.NewAttribute(); 
            attributeName state

    let next = ref (state.Reader.Peek())

    while !next <> -1 do
        data state
        next := state.Reader.Peek()

    !state.Tokens |> List.rev

