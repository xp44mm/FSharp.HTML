# FSharp.HTML

a parse for HTML5 based on the official W3C specification.

The usage is:

```F#
open FSharp.HTML
let html = "<p>hello world!</p>"
let y = Parser.parseDoc html
```

see also [ParserTest](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML.Test/ParserTest.fs)

This Parser is highly configurable, see source code [Parser](https://github.com/xp44mm/FSharp.HTML/blob/master/FSharp.HTML/Parser.fs)
