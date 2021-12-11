// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml;

// HTML 5.2
// W3C Recommendation, 14 December 2017
// https://www.w3.org/TR/html52/
public static class XHtmlAttributeNames {
  // 3.2.5 Global attributes
  public static readonly XName AccessKey = "accesskey";
  public static readonly XName Class = "class";
  public static readonly XName ContentEditable = "contenteditable";
  public static readonly XName Dir = "dir";
  public static readonly XName Draggable = "draggable";
  public static readonly XName Hidden = "hidden";
  public static readonly XName Id = "id";
  public static readonly XName Lang = "lang";
  public static readonly XName SpellCheck = "spellcheck";
  public static readonly XName Style = "style";
  public static readonly XName TabIndex = "tabindex";
  public static readonly XName Title = "title";
  public static readonly XName Translate = "translate";

  public static readonly XName Href = "href";
  public static readonly XName HrefLang = "hreflang";
  public static readonly XName Alt = "alt";
  public static readonly XName Src = "src";
  public static readonly XName Rel = "rel";
  public static readonly XName Cite = "cite";
  public static readonly XName Type = "type";
  public static readonly XName Target = "target";
  public static readonly XName Media = "media";
  public static readonly XName Download = "download";
}
