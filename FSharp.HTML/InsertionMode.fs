namespace FSharp.HTML

type InsertionMode =
    | DefaultMode
    | ScriptMode
    | CharRefMode
    | CommentMode
    | DocTypeMode
    | CDATAMode

    //override x.ToString() =
    //    match x with
    //    | DefaultMode -> "default"
    //    | ScriptMode -> "script"
    //    | CharRefMode -> "charref"
    //    | CommentMode -> "comment"
    //    | DocTypeMode -> "doctype"
    //    | CDATAMode -> "cdata"

