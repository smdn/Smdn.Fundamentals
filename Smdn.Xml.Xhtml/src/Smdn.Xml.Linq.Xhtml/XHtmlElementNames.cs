// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2018 smdn
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml {
  // HTML 5.2
  // W3C Recommendation, 14 December 2017
  // https://www.w3.org/TR/html52/
  public static class XHtmlElementNames {
    // 4.1 The root element
    public static readonly XName Html = XHtmlNamespaces.Html.GetName("html");

    // 4.2 Document metadata
    public static readonly XName Head = XHtmlNamespaces.Html.GetName("head");
    public static readonly XName Title = XHtmlNamespaces.Html.GetName("title");
    public static readonly XName Base = XHtmlNamespaces.Html.GetName("base");
    public static readonly XName Link = XHtmlNamespaces.Html.GetName("link");
    public static readonly XName Meta = XHtmlNamespaces.Html.GetName("meta");
    public static readonly XName Style = XHtmlNamespaces.Html.GetName("style");

    // 4.3 Sections
    public static readonly XName Body = XHtmlNamespaces.Html.GetName("body");
    public static readonly XName Article = XHtmlNamespaces.Html.GetName("article");
    public static readonly XName Section = XHtmlNamespaces.Html.GetName("section");
    public static readonly XName Nav = XHtmlNamespaces.Html.GetName("nav");
    public static readonly XName Aside = XHtmlNamespaces.Html.GetName("aside");
    public static readonly XName H1 = XHtmlNamespaces.Html.GetName("h1");
    public static readonly XName H2 = XHtmlNamespaces.Html.GetName("h2");
    public static readonly XName H3 = XHtmlNamespaces.Html.GetName("h3");
    public static readonly XName H4 = XHtmlNamespaces.Html.GetName("h4");
    public static readonly XName H5 = XHtmlNamespaces.Html.GetName("h5");
    public static readonly XName H6 = XHtmlNamespaces.Html.GetName("h6");
    public static readonly XName Header = XHtmlNamespaces.Html.GetName("header");
    public static readonly XName Footer = XHtmlNamespaces.Html.GetName("footer");

    // 4.4 Grouping content
    public static readonly XName P = XHtmlNamespaces.Html.GetName("p");
    public static readonly XName Address = XHtmlNamespaces.Html.GetName("address");
    public static readonly XName HR = XHtmlNamespaces.Html.GetName("hr");
    public static readonly XName Pre = XHtmlNamespaces.Html.GetName("pre");
    public static readonly XName Blockquote = XHtmlNamespaces.Html.GetName("blockquote");
    public static readonly XName OL = XHtmlNamespaces.Html.GetName("ol");
    public static readonly XName UL = XHtmlNamespaces.Html.GetName("ul");
    public static readonly XName LI = XHtmlNamespaces.Html.GetName("li");
    public static readonly XName DL = XHtmlNamespaces.Html.GetName("dl");
    public static readonly XName DT = XHtmlNamespaces.Html.GetName("dt");
    public static readonly XName DD = XHtmlNamespaces.Html.GetName("dd");
    public static readonly XName Figure = XHtmlNamespaces.Html.GetName("figure");
    public static readonly XName FigCaption = XHtmlNamespaces.Html.GetName("figcaption");
    public static readonly XName Main = XHtmlNamespaces.Html.GetName("main");
    public static readonly XName Div = XHtmlNamespaces.Html.GetName("div");

    // 4.5 Text-level semantics
    public static readonly XName A = XHtmlNamespaces.Html.GetName("a");
    public static readonly XName EM = XHtmlNamespaces.Html.GetName("em");
    public static readonly XName Strong = XHtmlNamespaces.Html.GetName("strong");
    public static readonly XName Small = XHtmlNamespaces.Html.GetName("small");
    public static readonly XName S = XHtmlNamespaces.Html.GetName("s");
    public static readonly XName Cite = XHtmlNamespaces.Html.GetName("cite");
    public static readonly XName Q = XHtmlNamespaces.Html.GetName("q");
    public static readonly XName Dfn = XHtmlNamespaces.Html.GetName("dfn");
    public static readonly XName Abbr = XHtmlNamespaces.Html.GetName("abbr");
    public static readonly XName Ruby = XHtmlNamespaces.Html.GetName("ruby");
    public static readonly XName RB = XHtmlNamespaces.Html.GetName("rb");
    public static readonly XName RT = XHtmlNamespaces.Html.GetName("rt");
    public static readonly XName RTC = XHtmlNamespaces.Html.GetName("rtc");
    public static readonly XName RP = XHtmlNamespaces.Html.GetName("rp");
    public static readonly XName Data = XHtmlNamespaces.Html.GetName("data");
    public static readonly XName Time = XHtmlNamespaces.Html.GetName("time");
    public static readonly XName Code = XHtmlNamespaces.Html.GetName("code");
    public static readonly XName Var = XHtmlNamespaces.Html.GetName("var");
    public static readonly XName Samp = XHtmlNamespaces.Html.GetName("samp");
    public static readonly XName Kbd = XHtmlNamespaces.Html.GetName("kbd");
    public static readonly XName Sub = XHtmlNamespaces.Html.GetName("sub");
    public static readonly XName Sup = XHtmlNamespaces.Html.GetName("sup");
    public static readonly XName I = XHtmlNamespaces.Html.GetName("i");
    public static readonly XName B = XHtmlNamespaces.Html.GetName("b");
    public static readonly XName U = XHtmlNamespaces.Html.GetName("u");
    public static readonly XName Mark = XHtmlNamespaces.Html.GetName("mark");
    public static readonly XName DBI = XHtmlNamespaces.Html.GetName("dbi");
    public static readonly XName DBO = XHtmlNamespaces.Html.GetName("dbo");
    public static readonly XName Span = XHtmlNamespaces.Html.GetName("span");
    public static readonly XName BR = XHtmlNamespaces.Html.GetName("br");
    public static readonly XName WBR = XHtmlNamespaces.Html.GetName("wbr");

    // 4.6 Edits
    public static readonly XName Ins = XHtmlNamespaces.Html.GetName("ins");
    public static readonly XName Del = XHtmlNamespaces.Html.GetName("del");

    // 4.7 Embedded content
    public static readonly XName Picture = XHtmlNamespaces.Html.GetName("picture");
    public static readonly XName Source = XHtmlNamespaces.Html.GetName("source");
    public static readonly XName Img = XHtmlNamespaces.Html.GetName("img");
    public static readonly XName IFrame = XHtmlNamespaces.Html.GetName("iframe");
    public static readonly XName Embed = XHtmlNamespaces.Html.GetName("embed");
    public static readonly XName Object = XHtmlNamespaces.Html.GetName("object");
    public static readonly XName Param = XHtmlNamespaces.Html.GetName("param");
    public static readonly XName Video = XHtmlNamespaces.Html.GetName("video");
    public static readonly XName Audio = XHtmlNamespaces.Html.GetName("audio");
    public static readonly XName Track = XHtmlNamespaces.Html.GetName("track");
    public static readonly XName Map = XHtmlNamespaces.Html.GetName("map");
    public static readonly XName Area = XHtmlNamespaces.Html.GetName("area");

    // 4.7.17 MathML
    public static readonly XName Math = XHtmlNamespaces.MathML.GetName("math");

    // 4.7.18 SVG
    public static readonly XName SVG = XHtmlNamespaces.Svg.GetName("svg");

    // 4.9 Tabular data
    public static readonly XName Table = XHtmlNamespaces.Html.GetName("table");
    public static readonly XName Caption = XHtmlNamespaces.Html.GetName("caption");
    public static readonly XName ColGroup = XHtmlNamespaces.Html.GetName("colgroup");
    public static readonly XName Col = XHtmlNamespaces.Html.GetName("col");
    public static readonly XName TBody = XHtmlNamespaces.Html.GetName("tbody");
    public static readonly XName THead = XHtmlNamespaces.Html.GetName("thead");
    public static readonly XName TFoot = XHtmlNamespaces.Html.GetName("tfoot");
    public static readonly XName TR = XHtmlNamespaces.Html.GetName("tr");
    public static readonly XName TD = XHtmlNamespaces.Html.GetName("td");
    public static readonly XName TH = XHtmlNamespaces.Html.GetName("th");

    // 4.10 Forms
    public static readonly XName Form = XHtmlNamespaces.Html.GetName("form");
    public static readonly XName Label = XHtmlNamespaces.Html.GetName("label");
    public static readonly XName Input = XHtmlNamespaces.Html.GetName("input");
    public static readonly XName Button = XHtmlNamespaces.Html.GetName("button");
    public static readonly XName Select = XHtmlNamespaces.Html.GetName("select");
    public static readonly XName DataList = XHtmlNamespaces.Html.GetName("datalist");
    public static readonly XName OptGroup = XHtmlNamespaces.Html.GetName("optgroup");
    public static readonly XName Option = XHtmlNamespaces.Html.GetName("option");
    public static readonly XName TextArea = XHtmlNamespaces.Html.GetName("textarea");
    public static readonly XName Output = XHtmlNamespaces.Html.GetName("output");
    public static readonly XName Progress = XHtmlNamespaces.Html.GetName("progress");
    public static readonly XName Meter = XHtmlNamespaces.Html.GetName("meter");
    public static readonly XName FieldSet = XHtmlNamespaces.Html.GetName("fieldset");
    public static readonly XName Legend = XHtmlNamespaces.Html.GetName("legend");

    // 4.11 Interactive elements
    public static readonly XName Details = XHtmlNamespaces.Html.GetName("details");
    public static readonly XName Summary = XHtmlNamespaces.Html.GetName("summary");
    public static readonly XName Dialog = XHtmlNamespaces.Html.GetName("dialog");

    // 4.12 Scripting
    public static readonly XName Script = XHtmlNamespaces.Html.GetName("script");
    public static readonly XName NoScript = XHtmlNamespaces.Html.GetName("noscript");
    public static readonly XName Template = XHtmlNamespaces.Html.GetName("template");
    public static readonly XName Canvas = XHtmlNamespaces.Html.GetName("canvas");
  }
}