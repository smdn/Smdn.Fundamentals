// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

using System;
using System.Xml;

namespace Smdn.Xml.Xhtml {
  public class XhtmlDocument : XmlDocument {
    public XhtmlDocument()
      : base()
    {
    }

    public XhtmlDocument(XmlNameTable nt)
      : base(nt)
    {
    }

    public XmlElement CreateXhtmlElement(string name)
    {
      return CreateXhtmlElement(name, null, null);
    }

    public XmlElement CreateXhtmlElement(string name, string id, string @class)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      var element = CreateElement(name, W3CNamespaces.Xhtml);

      if (id     != null) element.SetAttribute("id", id);
      if (@class != null) element.SetAttribute("class", @class);

      return element;
    }

    public XmlElement CreateXhtmlAnchor(string href)
    {
      return CreateXhtmlAnchor(href, null, null, null);
    }

    public XmlElement CreateXhtmlAnchor(string href, string title)
    {
      return CreateXhtmlAnchor(href, null, title, null);
    }

    public XmlElement CreateXhtmlAnchor(string href, string id, string title, string @class)
    {
      var anchor = CreateXhtmlElement("a");

      if (id        != null) anchor.SetAttribute("id", id);
      if (href      != null) anchor.SetAttribute("href", href);
      if (@class    != null) anchor.SetAttribute("class", @class);
      if (title     != null) anchor.SetAttribute("title", title);

      return anchor;
    }

    public XmlElement CreateXhtmlImage(string src)
    {
      return CreateXhtmlImage(src, null, null, null, null);
    }

    public XmlElement CreateXhtmlImage(string src, string alt, string title)
    {
      return CreateXhtmlImage(src, alt, title, null, null);
    }

    public XmlElement CreateXhtmlImage(string src, string alt, string title, string width, string height)
    {
      var img = CreateXhtmlElement("img");

      if (src    != null) img.SetAttribute("src", src);
      if (alt    != null) img.SetAttribute("alt", alt);
      if (title  != null) img.SetAttribute("title", title);
      if (width  != null) img.SetAttribute("width", width);
      if (height != null) img.SetAttribute("height", height);

      return img;
    }

    public XmlElement CreateXhtmlPre()
    {
      return CreateXhtmlPre(null);
    }

    public XmlElement CreateXhtmlPre(bool preserveWhitespaces)
    {
      if (preserveWhitespaces)
        return CreateXhtmlPre("preserve");
      else
        return CreateXhtmlPre(null /* "default" */);
    }

    public XmlElement CreateXhtmlPre(string xmlSpaceAttribute)
    {
      var pre = CreateXhtmlElement("pre");

      if (xmlSpaceAttribute != null)
        pre.SetAttribute("xml:space", xmlSpaceAttribute);

      return pre;
    }
  }
}
