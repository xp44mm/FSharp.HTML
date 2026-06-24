namespace FSharp.XML

type XmlNode =
    | XmlProcessingInstruction of target: string * data: (string * string) list
    | XmlElement of name: string * attributes: (string * string) list * children: XmlNode list
    | XmlDocumentType of string list * string list list
    | XmlComment of string
    | XmlText of content: string
