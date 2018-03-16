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

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml {
  public class XHtmlClassAttribute : XAttribute {
    private static readonly char[] classListSeparator = { ' ' };

    internal static string[] SplitClassList(string classList)
    {
      return classList?.Split(classListSeparator, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(),
    }

    public static string JoinClassList(IEnumerable<string> classList)
    {
      if (classList == null)
        throw new ArgumentNullException(nameof(classList));

      return string.Join(" ", classList);
    }

    public XHtmlClassAttribute(string @class)
      : base(XHtmlAttributeNames.Class, @class)
    {
    }

    public XHtmlClassAttribute(params string[] classList)
      : base(XHtmlAttributeNames.Class, JoinClassList(classList))
    {
    }

    public XHtmlClassAttribute(IEnumerable<string> classList)
      : base(XHtmlAttributeNames.Class, JoinClassList(classList))
    {
    }
  }
}