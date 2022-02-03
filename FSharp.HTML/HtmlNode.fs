namespace FSharp.HTML

open System
open System.ComponentModel
open System.Text
open System.Runtime.CompilerServices

/// Represents an HTML node. The names of elements are always normalized to lowercase
//[<StructuredFormatDisplay("{_Print}")>]
type HtmlNode =
    | HtmlElement of name:string * attributes:HtmlAttribute list * elements:HtmlNode list
    | HtmlText of content:string
    | HtmlComment of content:string
    | HtmlCData of content:string

    /// <summary>
    /// Creates an html element
    /// </summary>
    /// <param name="name">The name of the element</param>
    static member NewElement(name:string) =
        HtmlElement(name.ToLowerInvariant(), [], [])

    /// <summary>
    /// Creates an html element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="attrs">The HtmlAttribute(s) of the element</param>
    static member NewElement(name:string, attrs:seq<_>) =
        let attrs = 
            attrs 
            |> Seq.map(fun (n,v) -> HtmlAttribute(n,v) )
            |> Seq.toList
        HtmlElement(name.ToLowerInvariant(), attrs, [])

    /// <summary>
    /// Creates an html element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="children">The children elements of this element</param>
    static member NewElement(name:string, children:seq<_>) =
        HtmlElement(name.ToLowerInvariant(), [], List.ofSeq children)


    /// <summary>
    /// Creates an html element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="attrs">The HtmlAttribute(s) of the element</param>
    /// <param name="children">The children elements of this element</param>
    static member NewElement(name:string, attrs:seq<_>, children:seq<_>) =
        let attrs = 
            attrs 
            |> Seq.map (fun (n,v) -> HtmlAttribute(n,v) )
            |> Seq.toList
        HtmlElement(name.ToLowerInvariant(), attrs, List.ofSeq children)

    /// <summary>
    /// Creates a text content element
    /// </summary>
    /// <param name="content">The actual content</param>
    static member NewText content = HtmlText(content)

    /// <summary>
    /// Creates a comment element
    /// </summary>
    /// <param name="content">The actual content</param>
    static member NewComment content = HtmlComment(content)

    /// <summary>
    /// Creates a CData element
    /// </summary>
    /// <param name="content">The actual content</param>
    static member NewCData content = HtmlCData(content)

    //override x.ToString() =
    //    let isVoidElement =
    //        let set =
    //            [| "area"; "base"; "br"; "col"; "command"; "embed"; "hr"; "img"; "input"
    //               "keygen"; "link"; "meta"; "param"; "source"; "track"; "wbr" |]
    //            |> Set.ofArray
    //        fun name -> Set.contains name set
    //    let rec serialize (sb:StringBuilder) indentation canAddNewLine html =
    //        let append (str:string) = sb.Append str |> ignore
    //        let appendEndTag name =
    //            append "</"
    //            append name
    //            append ">"
    //        let newLine plus =
    //            sb.AppendLine() |> ignore
    //            String(' ', indentation + plus) |> append
    //        match html with
    //        | HtmlElement(name, attributes, elements) ->
    //            let onlyText = elements |> List.forall (function HtmlText _ -> true | _ -> false)
    //            if canAddNewLine && not onlyText then
    //                newLine 0
    //            append "<"
    //            append name
    //            for HtmlAttribute(name, value) in attributes do
    //                append " "
    //                append name
    //                append "=\""
    //                append value
    //                append "\""
    //            if isVoidElement name then
    //                append " />"
    //            elif elements.IsEmpty then
    //                append ">"
    //                appendEndTag name
    //            else
    //                append ">"
    //                if not onlyText then
    //                    newLine 2
    //                let mutable canAddNewLine = false
    //                for element in elements do
    //                    serialize sb (indentation + 2) canAddNewLine element
    //                    canAddNewLine <- true
    //                if not onlyText then
    //                    newLine 0
    //                appendEndTag name
    //        | HtmlText str -> append str
    //        | HtmlComment str ->
    //                append "<!--"
    //                append str
    //                append "-->"
    //        | HtmlCData str ->
    //                append "<![CDATA["
    //                append str
    //                append "]]>"

    //    let sb = StringBuilder()
    //    serialize sb 0 false x |> ignore
    //    sb.ToString()

    /// <exclude />
    //[<EditorBrowsable(EditorBrowsableState.Never)>]
    //[<CompilerMessage("This method is intended for use in generated code only.", 10001, IsHidden=true, IsError=false)>]
    //member x._Print =
    //    let str = x.ToString()
    //    if str.Length > 512 then str.Substring(0, 509) + "..."
    //    else str
// --------------------------------------------------------------------------------------

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
/// Module with operations on HTML nodes
module HtmlNode =

    /// Gets the given nodes name
    let name n =
        match n with
        | HtmlElement(name = name) -> name
        | _ -> ""
        
    /// Gets all of the nodes immediately under this node
    let elements n =
        match n with
        | HtmlElement(elements = elements) -> elements
        | _ -> []

    /// Gets all of the attributes of this node
    let attributes n =
        match n with
        | HtmlElement(attributes = attributes) -> attributes
        | _ -> []

    /// <summary>
    /// Tries to return an attribute that exists on the current node
    /// </summary>
    /// <param name="name">The name of the attribute to return.</param>
    /// <param name="n">The given node</param>
    let inline tryGetAttribute (name:string) n =
        let name = name.ToLowerInvariant()
        n 
        |> attributes 
        |> List.tryFind (function HtmlAttribute(nm,_) -> nm = name)
    
    /// <summary>
    /// Returns the attribute with the given name. If the
    /// attribute does not exist then this will throw an exception
    /// </summary>
    /// <param name="name">The name of the attribute to select</param>
    /// <param name="n">The given node</param>
    let inline attribute name n = 
        match tryGetAttribute name n with
        | Some v -> v
        | None -> failwithf "Unable to find attribute (%s)" name

    /// <summary>
    /// Return the value of the named attribute, or an empty string if not found.
    /// </summary>
    /// <param name="name">The name of the attribute to get the value from</param>
    /// <param name="n">The given node</param>
    let inline attributeValue name n = 
        let x = 
            n 
            |> tryGetAttribute name 
            |> Option.map (function HtmlAttribute(_,value) -> value)
        defaultArg x ""

    /// <summary>
    /// Returns true if the current node has an attribute that
    /// matches both the name and the value
    /// </summary>
    /// <param name="name">The name of the attribute</param>
    /// <param name="value">The value of the attribute</param>
    /// <param name="n">The given html node</param>
    let inline hasAttribute (name:string) (value:string) n = 
        match tryGetAttribute name n with
        | Some (HtmlAttribute(_,v)) -> 
            let x = v.ToLowerInvariant()
            x = value.ToLowerInvariant()
        | None -> false

// --------------------------------------------------------------------------------------

[<Extension>]
/// Extension methods with operations on HTML nodes
type HtmlNodeExtensions =
               
    /// Gets the given nodes name
    [<Extension>]
    static member Name(n:HtmlNode) = 
        HtmlNode.name n
        
    /// Gets all of the nodes immediately under this node
    [<Extension>]
    static member Elements(n:HtmlNode) = 
        HtmlNode.elements n

    /// Gets all of the attributes of this node
    [<Extension>]
    static member Attributes(n:HtmlNode) = 
        HtmlNode.attributes n

    /// <summary>
    /// Tries to select an attribute with the given name from the current node.
    /// </summary>
    /// <param name="n">The given node</param>
    /// <param name="name">The name of the attribute to select</param>
    [<Extension>]
    static member TryGetAttribute(n:HtmlNode, name:string) = 
        HtmlNode.tryGetAttribute name n

    /// <summary>
    /// Returns the attribute with the given name. If the
    /// attribute does not exist then this will throw an exception
    /// </summary>
    /// <param name="n">The given node</param>
    /// <param name="name">The name of the attribute to select</param>
    [<Extension>]
    static member Attribute(n:HtmlNode, name) = 
        HtmlNode.attribute name n

    /// <summary>
    /// Return the value of the named attribute, or an empty string if not found.
    /// </summary>
    /// <param name="n">The given node</param>
    /// <param name="name">The name of the attribute to get the value from</param>
    [<Extension>]
    static member AttributeValue(n:HtmlNode, name) =
      HtmlNode.attributeValue name n

    /// <summary>
    /// Returns true if the current node has an attribute that
    /// matches both the name and the value
    /// </summary>
    /// <param name="n">The given node</param>
    /// <param name="name">The name of the attribute</param>
    /// <param name="value">The value of the attribute</param>
    [<Extension>]
    static member HasAttribute(n:HtmlNode, name, value) = 
        HtmlNode.hasAttribute name value n






