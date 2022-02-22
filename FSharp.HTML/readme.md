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
let doc = Parser.parseDoc sourceText
```

you can get a `HtmlDocument` instance. see to `ParserTest.fs`.

## API

The user can parse the string through the functions in the `Parser` module.

```F#
module FSharp.HTML.Parser

/// Parses input text as a HtmlDocument tree
let parseDoc txt = ...


/// Parses input text as a HtmlNode sequence, and ignore doctype.
let parseNodes txt = ...

```

You can also use a tokenizer to get a token sequence.

```F#
let tokens = Tokenizer.tokenize txt 
```

The main structure types are defined as follows:

```F#
/// Represents an HTML node. The names of elements are always normalized to lowercase
type HtmlNode =
    | HtmlElement of name:string * attributes:list<string*string> * elements:HtmlNode list
    | HtmlText of content:string
    | HtmlComment of content:string
    | HtmlCData of content:string

/// Represents an HTML document
type HtmlDocument =
    | HtmlDocument of docType:string * elements:HtmlNode list

type HtmlToken =
    | DocType of string
    | Text of string
    | Comment of string
    | CData of string

    | TagSelfClosing of name:string * attrs:list<string*string>
    | TagStart of name:string * attrs:list<string*string>
    | TagEnd of name:string

    | EOF

    | SEMICOLON

```

All parsing processes in a package are public, and you are free to compose them to implement your functional requirements.