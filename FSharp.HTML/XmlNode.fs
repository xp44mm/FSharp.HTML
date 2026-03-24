namespace FSharp.XML

/////
//type XmlNode =
//    | XmlProlog of version: string * encoding: string
//    | XmlElement of
//        name: string *
//        attributes: list<string * string> *
//        elements: XmlNode list
//    | XmlComment of string
//    | XmlCData of string
//    | XmlText of string
//    | XmlWS of string

type XmlNode =
    | XmlProcessingInstruction of target: string * data: (string * string) list
    | XmlElement of name: string * attributes: (string * string) list * children: XmlNode list
    | XmlDocumentType of string list * string list list
    | XmlComment of string
    | XmlText of content: string
