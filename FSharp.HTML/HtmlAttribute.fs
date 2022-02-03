namespace FSharp.HTML

//open System.Runtime.CompilerServices


/// <summary>Represents an HTML attribute. The name is always normalized to lowercase</summary>
/// <namespacedoc>
///   <summary>Contains the primary types for the FSharp.HTML package.</summary>
/// </namespacedoc>
///
type HtmlAttribute =
    | HtmlAttribute of name:string * value:string

    ///// <summary>
    ///// Creates an html attribute
    ///// </summary>
    ///// <param name="name">The name of the attribute</param>
    ///// <param name="value">The value of the attribute</param>
    //static member New(name:string, value:string) =
    //    HtmlAttribute(name.ToLowerInvariant(), value)


// --------------------------------------------------------------------------------------

//[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
///// Module with operations on HTML attributes
//module HtmlAttribute = 

//    /// Gets the name of the given attribute
//    let name attr = 
//        match attr with
//        | HtmlAttribute(name = name) -> name

//    /// Gets the value of the given attribute
//    let value attr = 
//        match attr with
//        | HtmlAttribute(value = value) -> value   

// --------------------------------------------------------------------------------------

//[<Extension>]
///// Extension methods with operations on HTML attributes
//type HtmlAttributeExtensions =

//    /// Gets the name of the current attribute
//    [<Extension>]
//    static member Name(attr:HtmlAttribute) = 
//        HtmlAttribute.name attr

//    /// Gets the value of the current attribute
//    [<Extension>]
//    static member Value(attr:HtmlAttribute) = 
//        HtmlAttribute.value attr
