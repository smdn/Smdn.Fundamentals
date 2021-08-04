// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Xml.Linq;

using Smdn.Xml.Xhtml;

namespace Smdn.Xml.Linq.Xhtml {
  public static class XHtmlNamespaces {
    public static readonly XNamespace Html = (XNamespace)W3CNamespaces.Html;
    public static readonly XNamespace MathML = (XNamespace)W3CNamespaces.MathML;
    public static readonly XNamespace Svg = (XNamespace)W3CNamespaces.Svg;
    public static readonly XNamespace XLink = (XNamespace)W3CNamespaces.XLink;
  }
}
