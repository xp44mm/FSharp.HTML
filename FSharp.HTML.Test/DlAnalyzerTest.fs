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

    let evade txt =
        txt
        |> fun txt -> new StringReader(txt)
        |> HtmlTokenizer.tokenise
        |> List.choose (HtmlTokenUtils.adapt>>HtmlTokenUtils.unifyVoidElement)

        |> DlDFA.analyze
        |> Seq.concat


    let parse txt =
        txt
        |> evade

        |> SemiNodeDFA.analyze
        |> Seq.concat
        |> HtmlParseTable.parse

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
        //let mutable ls = ResizeArray()
        //try
        //    let y = 
        //        evade x
        //        |> Seq.map(fun x -> 
        //            ls.Add(x)
        //            x
        //        )
        //        |> Seq.toList
        //    show y
        //with _ ->
        //    let ls = List.ofSeq ls
        //    show ls

        let y = parse x |> snd
        show y

        //let e = [HtmlElement("table",[],[HtmlElement("caption",[],[HtmlText "Council budget (in £) 2018"]);HtmlElement("thead",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","col")],[HtmlText "Items"]);HtmlElement("th",[HtmlAttribute("scope","col")],[HtmlText "Expenditure"])])]);HtmlElement("tbody",[],[HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Donuts"]);HtmlElement("td",[],[HtmlText "3,000"])]);HtmlElement("tr",[],[HtmlElement("th",[HtmlAttribute("scope","row")],[HtmlText "Stationery"]);HtmlElement("td",[],[HtmlText "18,000"])])])])]
        //Should.equal e y




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
        //let mutable ls = ResizeArray()
        //try
        //    let y = 
        //        evade x // ($"<div>{x}</div>")
        //        |> Seq.map(fun x -> 
        //            ls.Add(x)
        //            x
        //        )
        //        |> Seq.toList
        //    show y
        //with _ ->
        //    let ls = List.ofSeq ls
        //    show ls

        let y = parse x |> snd
        show y

        //Should.equal e y

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
        //let mutable ls = ResizeArray()
        //try
        //    let y = 
        //        evade x // ($"<div>{x}</div>")
        //        |> Seq.map(fun x -> 
        //            ls.Add(x)
        //            x
        //        )
        //        |> Seq.toList
        //    show y
        //with _ ->
        //    let ls = List.ofSeq ls
        //    show ls

        let y = parse x |> snd
        show y

        //Should.equal e y

