namespace FSharp.HTML

open System
open System.ComponentModel
open FSharp.HTML
open System.Runtime.CompilerServices

/// Represents an HTML document
//[<StructuredFormatDisplay("{_Print}")>]
type HtmlDocument =
    | HtmlDocument of docType:string * elements:HtmlNode list

    /// <summary>
    /// Creates an html document
    /// </summary>
    /// <param name="docType">The document type specifier string</param>
    /// <param name="children">The child elements of this document</param>
    static member New(docType, children:seq<_>) =
        HtmlDocument(docType, List.ofSeq children)

    /// <summary>
    /// Creates an html document
    /// </summary>
    /// <param name="children">The child elements of this document</param>
    static member New(children:seq<_>) =
        HtmlDocument("", List.ofSeq children)

    //override x.ToString() =
    //    match x with
    //    | HtmlDocument(docType, elements) ->
    //        (if String.IsNullOrEmpty docType then "" else "<!DOCTYPE " + docType + ">" + Environment.NewLine)
    //        +
    //        (elements |> List.map (fun x -> x.ToString()) |> String.Concat)

    ///// <exclude />
    //[<EditorBrowsable(EditorBrowsableState.Never)>]
    //[<CompilerMessage("This method is intended for use in generated code only.", 10001, IsHidden=true, IsError=false)>]
    //member x._Print =
    //    let str = x.ToString()
    //    if str.Length > 512 then str.Substring(0, 509) + "..."
    //    else str

// --------------------------------------------------------------------------------------

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
/// Module with operations on HTML documents
module HtmlDocument = 
    
    /// Returns the doctype of the document
    let docType doc =
        match doc with
        | HtmlDocument(docType = docType) -> docType 
    
    //// Gets all of the root elements of the document
    let elements doc =
        match doc with
        | HtmlDocument(elements = elements) -> elements
                
[<Extension>]
/// Extension methods with operations on HTML documents
type HtmlDocumentExtensions =

    /// <summary>
    /// Returns all of the root elements of the current document
    /// </summary>
    /// <param name="doc">The given document</param>
    [<Extension>]
    static member Elements(doc:HtmlDocument) = 
        HtmlDocument.elements doc

