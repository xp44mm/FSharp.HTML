namespace FSharp.HTML

open System
open System.IO
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals

type DlAnalyzerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let parse txt =
        txt
        |> Tokenizer.tokenize
        |> HtmlTokenUtils.preamble
        |> snd
    
        |> Seq.choose HtmlTokenUtils.unifyVoidElement
    
        |> ListDFA.analyze
        |> Seq.concat
    
        |> RubyDFA.analyze
        |> Seq.concat
    
        |> OptgroupDFA.analyze
        |> Seq.concat
    
        |> OptionDFA.analyze
        |> Seq.concat
    
        |> ColgroupDFA.analyze
        |> Seq.concat
    
        |> CaptionDFA.analyze
        |> Seq.concat
    
        |> TrDFA.analyze
        |> Seq.concat
    
        |> TdDFA.analyze
        |> Seq.concat
    
        |> ParagraphDFA.analyze
        |> Seq.concat
    
        |> DlDFA.analyze
        |> Seq.concat
        
        |> PrecNodesParseTable.parse
        |> Whitespace.removeWsChildren
        |> Whitespace.trimWhitespace

    [<Fact>]
    member _.``well formed``() =
        let x = """
<dl>
<div>
 <dt> Last modified time </dt>
 <dd> 2004-12-23T23:33Z </dd>
</div>
<div>
 <dt> Recommended update interval </dt>
 <dd> 60s </dd>
</div>
<div>
 <dt> Authors </dt>
 <dt> Editors </dt>
 <dd> Robert Rothman </dd>
 <dd> Daniel Jackson </dd>
</div>
</dl>
"""

        let y = parse x
        show y

    [<Fact>]
    member _.``basis in div``() =
        let x = """
<dl>
<div itemscope itemtype="http://schema.org/Product">
 <dt itemprop="name">Café ou Chocolat Liégeois
 <dd itemprop="offers" itemscope itemtype="http://schema.org/Offer">
  <span itemprop="price">3.50</span>
  <data itemprop="priceCurrency" value="EUR">€</data>
 <dd itemprop="description">
  2 boules Café ou Chocolat, 1 boule Vanille, sauce café ou chocolat, chantilly
</div>

<div itemscope itemtype="http://schema.org/Product">
 <dt itemprop="name">Américaine
 <dd itemprop="offers" itemscope itemtype="http://schema.org/Offer">
  <span itemprop="price">3.50</span>
  <data itemprop="priceCurrency" value="EUR">€</data>
 <dd itemprop="description">
  1 boule Crème brûlée, 1 boule Vanille, 1 boule Caramel, chantilly
</div>
</dl>
"""
        let y = parse x
        show y


    [<Fact>]
    member _.``basis in dl``() =
        let x = """
<dl>
<dt itemscope itemtype="http://schema.org/Product" itemref="1-offer 1-description">
 <span itemprop="name">Café ou Chocolat Liégeois</span>
<dd id="1-offer" itemprop="offers" itemscope itemtype="http://schema.org/Offer">
 <span itemprop="price">3.50</span>
 <data itemprop="priceCurrency" value="EUR">€</data>
<dd id="1-description" itemprop="description">
 2 boules Café ou Chocolat, 1 boule Vanille, sauce café ou chocolat, chantilly

<dt itemscope itemtype="http://schema.org/Product" itemref="2-offer 2-description">
 <span itemprop="name">Américaine</span>
<dd id="2-offer" itemprop="offers" itemscope itemtype="http://schema.org/Offer">
 <span itemprop="price">3.50</span>
 <data itemprop="priceCurrency" value="EUR">€</data>
<dd id="2-description" itemprop="description">
 1 boule Crème brûlée, 1 boule Vanille, 1 boule Caramel, chantilly
</dl>
"""

        let y = parse x
        show y


    [<Fact>]
    member _.``ListElementsWithoutListContainer``() =
        let x = """<!DOCTYPE html><body><dl>
            <dt>hello<dd>world</dd><dt>how<dd>do</dl>you</body><!--do-->
            """
        let y = parse x
        show y
