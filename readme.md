# FSharp.HTML

A parse for HTML5 based on the official W3C specification.

## Usage

the html source text is:

```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>My test page</title>
  </head>
  <body>
    <img src="images/firefox-icon.png" alt="My test image">
  </body>
</html>
```

we can use this code to parse html source to `HtmlNode list`:

```fsharp
let sourceText = <html>...</html>
let nodes: HtmlNode list = HtmlUtils.parseDoc sourceText
```

doctype is a string that is extracted from doctype tag. and nodes is a `HtmlNode list`.

```fsharp
type HtmlNode =
    | HtmlElement of
        name: string *
        attributes: list<string * string> *
        elements: HtmlNode list
    | HtmlComment of string
    | HtmlCData of string
    | HtmlText of string
    | HtmlDoctype of string
    | HtmlWS of string
```

All parsing processes in a package are public, and you are free to compose them to implement your functional requirements. Parser is highly configurable, see source code [HtmlUtils](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlUtils.fs)

```Fsharp
module FSharp.HTML.HtmlUtils

let parseDoc (txt: string) =
    txt
    |> HtmlCompiler.compileText
    |> Whitespace.trimWhitespace
    |> List.map CharacterReference.processCharRefs
```


## API

The user can parse the string through the functions in the `HtmlUtils` module.

[HtmlUtils](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlUtils.fs)

- The type `HtmlNode` see to [HtmlNode](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlNode.fs).

- The type `HtmlToken` see to [HtmlToken](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlToken.fs).
