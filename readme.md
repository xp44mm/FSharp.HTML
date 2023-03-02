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
let sourceText = ...
let doctype,nodes = HtmlUtils.parseDoc sourceText
```

doctype is a string that is extracted from doctype tag. and nodes is a `HtmlNode list`.

All parsing processes in a package are public, and you are free to compose them to implement your functional requirements. Parser is highly configurable, see source code [HtmlUtils](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlUtils.fs)

Parse only html structures without changing the content. Please use `HtmldocCompiler.compile`. In fact, the `HtmlUtils.parseDoc` is defined as follows:

```fsharp
let parseDoc (txt:string) = 
    let doctype,nodes =
        txt
        |> HtmldocCompiler.compile
    let nodes =
        nodes
        |> List.map Whitespace.removeWS
        |> Whitespace.trimWhitespace
        |> List.map HtmlCharRefs.unescapseNode
    doctype,nodes
```

Knowing the above code, you can determine the parsing result as your needs.

generate html source text:

```Fsharp
Render.stringifyNode
Render.stringifyDoc

HtmlUtils.stringifyNode
HtmlUtils.stringifyDoc
```

some transform:

```fsharp
BrRemover.splitByBr
HrRemover.splitByHr
```

## API

The user can parse the string through the functions in the `HtmlUtils` module.

[HtmlUtils](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlUtils.fs)

You can also use a tokenizer to get a token sequence.

```fsharp
let tokens = HtmlTokenizer.tokenize txt 
```

The main structure types are defined as follows:

- The type `HtmlNode` see to [HtmlNode](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlNode.fs).

- The type `HtmlToken` see to [HtmlToken](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlToken.fs).
