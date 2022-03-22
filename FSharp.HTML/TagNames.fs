﻿module FSharp.HTML.TagNames

let meta = []

let htmlTags = set [
  "a"
  "abbr"
  "acronym"
  "address"
  "applet"
  "area"
  "article"
  "aside"
  "audio"
  "b"
  "base"
  "basefont"
  "bdi"
  "bdo"
  "bgsound"
  "big"
  "blink"
  "blockquote"
  "body"
  "br"
  "button"
  "canvas"
  "caption"
  "center"
  "cite"
  "code"
  "col"
  "colgroup"
  "command"
  "content"
  "data"
  "datalist"
  "dd"
  "del"
  "details"
  "dfn"
  "dialog"
  "dir"
  "div"
  "dl"
  "dt"
  "element"
  "em"
  "embed"
  "fieldset"
  "figcaption"
  "figure"
  "font"
  "footer"
  "form"
  "frame"
  "frameset"
  "h1"
  "h2"
  "h3"
  "h4"
  "h5"
  "h6"
  "head"
  "header"
  "hgroup"
  "hr"
  "html"
  "i"
  "iframe"
  "image"
  "img"
  "input"
  "ins"
  "isindex"
  "kbd"
  "keygen"
  "label"
  "legend"
  "li"
  "link"
  "listing"
  "main"
  "map"
  "mark"
  "marquee"
  "math"
  "menu"
  "menuitem"
  "meta"
  "meter"
  "multicol"
  "nav"
  "nextid"
  "nobr"
  "noembed"
  "noframes"
  "noscript"
  "object"
  "ol"
  "optgroup"
  "option"
  "output"
  "p"
  "param"
  "picture"
  "plaintext"
  "pre"
  "progress"
  "q"
  "rb"
  "rbc"
  "rp"
  "rt"
  "rtc"
  "ruby"
  "s"
  "samp"
  "script"
  "section"
  "select"
  "shadow"
  "slot"
  "small"
  "source"
  "spacer"
  "span"
  "strike"
  "strong"
  "style"
  "sub"
  "summary"
  "sup"
  "svg"
  "table"
  "tbody"
  "td"
  "template"
  "textarea"
  "tfoot"
  "th"
  "thead"
  "time"
  "title"
  "tr"
  "track"
  "tt"
  "u"
  "ul"
  "var"
  "video"
  "wbr"
  "xmp"
]

let svgTags = set [
  "a"
  "altGlyph"
  "altGlyphDef"
  "altGlyphItem"
  "animate"
  "animateColor"
  "animateMotion"
  "animateTransform"
  "animation"
  "audio"
  "canvas"
  "circle"
  "clipPath"
  "color-profile"
  "cursor"
  "defs"
  "desc"
  "discard"
  "ellipse"
  "feBlend"
  "feColorMatrix"
  "feComponentTransfer"
  "feComposite"
  "feConvolveMatrix"
  "feDiffuseLighting"
  "feDisplacementMap"
  "feDistantLight"
  "feDropShadow"
  "feFlood"
  "feFuncA"
  "feFuncB"
  "feFuncG"
  "feFuncR"
  "feGaussianBlur"
  "feImage"
  "feMerge"
  "feMergeNode"
  "feMorphology"
  "feOffset"
  "fePointLight"
  "feSpecularLighting"
  "feSpotLight"
  "feTile"
  "feTurbulence"
  "filter"
  "font"
  "font-face"
  "font-face-format"
  "font-face-name"
  "font-face-src"
  "font-face-uri"
  "foreignObject"
  "g"
  "glyph"
  "glyphRef"
  "handler"
  "hkern"
  "iframe"
  "image"
  "line"
  "linearGradient"
  "listener"
  "marker"
  "mask"
  "metadata"
  "missing-glyph"
  "mpath"
  "path"
  "pattern"
  "polygon"
  "polyline"
  "prefetch"
  "radialGradient"
  "rect"
  "script"
  "set"
  "solidColor"
  "stop"
  "style"
  "svg"
  "switch"
  "symbol"
  "tbreak"
  "text"
  "textArea"
  "textPath"
  "title"
  "tref"
  "tspan"
  "unknown"
  "use"
  "video"
  "view"
  "vkern"
]

let mathmlTags = set [
  "abs"
  "and"
  "annotation"
  "annotation-xml"
  "apply"
  "approx"
  "arccos"
  "arccosh"
  "arccot"
  "arccoth"
  "arccsc"
  "arccsch"
  "arcsec"
  "arcsech"
  "arcsin"
  "arcsinh"
  "arctan"
  "arctanh"
  "arg"
  "bvar"
  "card"
  "cartesianproduct"
  "ceiling"
  "ci"
  "cn"
  "codomain"
  "complexes"
  "compose"
  "condition"
  "conjugate"
  "cos"
  "cosh"
  "cot"
  "coth"
  "csc"
  "csch"
  "csymbol"
  "curl"
  "declare"
  "degree"
  "determinant"
  "diff"
  "divergence"
  "divide"
  "domain"
  "domainofapplication"
  "emptyset"
  "encoding"
  "eq"
  "equivalent"
  "eulergamma"
  "exists"
  "exp"
  "exponentiale"
  "factorial"
  "factorof"
  "false"
  "floor"
  "fn"
  "forall"
  "function"
  "gcd"
  "geq"
  "grad"
  "gt"
  "ident"
  "image"
  "imaginary"
  "imaginaryi"
  "implies"
  "in"
  "infinity"
  "int"
  "integers"
  "intersect"
  "interval"
  "inverse"
  "lambda"
  "laplacian"
  "lcm"
  "leq"
  "limit"
  "list"
  "ln"
  "log"
  "logbase"
  "lowlimit"
  "lt"
  "maction"
  "malign"
  "maligngroup"
  "malignmark"
  "malignscope"
  "math"
  "matrix"
  "matrixrow"
  "max"
  "mean"
  "median"
  "menclose"
  "merror"
  "mfenced"
  "mfrac"
  "mfraction"
  "mglyph"
  "mi"
  "min"
  "minus"
  "mlabeledtr"
  "mmultiscripts"
  "mn"
  "mo"
  "mode"
  "moment"
  "momentabout"
  "mover"
  "mpadded"
  "mphantom"
  "mprescripts"
  "mroot"
  "mrow"
  "ms"
  "mspace"
  "msqrt"
  "mstyle"
  "msub"
  "msubsup"
  "msup"
  "mtable"
  "mtd"
  "mtext"
  "mtr"
  "munder"
  "munderover"
  "naturalnumbers"
  "neq"
  "none"
  "not"
  "notanumber"
  "notin"
  "notprsubset"
  "notsubset"
  "or"
  "otherwise"
  "outerproduct"
  "partialdiff"
  "pi"
  "piece"
  "piecewice"
  "piecewise"
  "plus"
  "power"
  "primes"
  "product"
  "prsubset"
  "quotient"
  "rationals"
  "real"
  "reals"
  "reln"
  "rem"
  "root"
  "scalarproduct"
  "sdev"
  "sec"
  "sech"
  "select"
  "selector"
  "semantics"
  "sep"
  "set"
  "setdiff"
  "sin"
  "sinh"
  "subset"
  "sum"
  "tan"
  "tanh"
  "tendsto"
  "times"
  "transpose"
  "true"
  "union"
  "uplimit"
  "var"
  "variance"
  "vector"
  "vectorproduct"
  "xor"
]

let allTagNames = 
    htmlTags + mathmlTags + svgTags

let voidElements = set [
  "area"
  "base"
  "br"
  "col"
  "embed"
  "hr"
  "img"
  "input"
  "link"
  "meta"
  "param"
  "source"
  "track"
  "wbr"
]

let rawTextElements = set [
    "script"
    "style"
    ]

let escapableRawTextElements = set [
    "textarea"
    "title"
    ]

let phrasing = set [
    "a"; "em"; "strong"; "small"; "mark"; "abbr"; "dfn"; "i"; "b"; "s"; "u"; "code"; "var"; "samp"; "kbd"; "sup"; "sub"; "q"; "cite"; "span"; "bdo"; "bdi"; "br"; "wbr"; "ins"; "del"; "img"; "embed"; "object"; "iframe"; "map"; "area"; "script"; "noscript"; "ruby"; "video"; "audio"; "input"; "textarea"; "select"; "button"; "label"; "output"; "datalist"; "keygen"; "progress"; "command"; "canvas"; "time"; "meter";
]

let flowContent = set [
    "a";
    "abbr";
    "address";
    "article";
    "aside";
    "audio";
    "b";
    "bdo";
    "bdi";
    "blockquote";
    "br";
    "button";
    "canvas";
    "cite";
    "code";
    "command";
    "data";
    "datalist";
    "del";
    "details";
    "dfn";
    "div";
    "dl";
    "em";
    "embed";
    "fieldset";
    "figure";
    "footer";
    "form";
    "h1";
    "h2";
    "h3";
    "h4";
    "h5";
    "h6";
    "header";
    "hgroup";
    "hr";
    "i";
    "iframe";
    "img";
    "input";
    "ins";
    "kbd";
    "keygen";
    "label";
    "main";
    "map";
    "mark";
    "math";
    "menu";
    "meter";
    "nav";
    "noscript";
    "object";
    "ol";
    "output";
    "p";
    "picture";
    "pre";
    "progress";
    "q";
    "ruby";
    "s";
    "samp";
    "script";
    "section";
    "select";
    "small";
    "span";
    "strong";
    "sub";
    "sup";
    "svg";
    "table";
    "template";
    "textarea";
    "time";
    "u";
    "ul";
    "var";
    "video";
    "and";
    "wbr";
]


