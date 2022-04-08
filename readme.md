# FSharp.HTML

a parse for HTML5 based on the official W3C specification.

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

we can use this code to parse html source to HtmlDocument:

```F#
let sourceText = ...
let doc = HtmlUtils.parseDoc sourceText
```

you can get a `HtmlDocument` instance. see to [ParserTest](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML.Test/ParserTest.fs).

All parsing processes in a package are public, and you are free to compose them to implement your functional requirements. Parser is highly configurable, see source code [Parser](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/Parser.fs)

## API

The user can parse the string through the functions in the `Parser` module.

```F#
module FSharp.HTML.HtmlUtils

/// Parses input text as a HtmlDocument tree
let parseDoc (txt:string) = ...
let parseWellFormedDoc (txt:string) = ...


/// Parses input text as a HtmlNode sequence, and ignore doctype.
let parseNodes (txt:string) = ...
let parseWellFormedNodes (txt:string) = ...

let stringifyNode (node:HtmlNode) = 
    ...
let stringifyDoc (docType, elements) =
    ...
```

You can also use a tokenizer to get a token sequence.

```F#
let tokens = Tokenizer.tokenize txt 
```

The main structure types are defined as follows:

- The type `HtmlNode` see to [HtmlNode](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlNode.fs).

- The type `HtmlDocument` see to [HtmlDocument](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlDocument.fs).

- The type `HtmlToken` see to [HtmlToken](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/HtmlToken.fs).
