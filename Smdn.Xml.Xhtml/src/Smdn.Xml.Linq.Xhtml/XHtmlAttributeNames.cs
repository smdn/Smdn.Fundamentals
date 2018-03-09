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
    public static readonly XName Alt = "alt";
    public static readonly XName Src = "src";
    public static readonly XName Rel = "rel";
    public static readonly XName Cite = "cite";
    public static readonly XName Type = "type";
    public static readonly XName Download = "download";
  }
}