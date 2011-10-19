// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2011 smdn
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
using System.Xml;

#if NET_3_5
using System.Linq;
#else
using Smdn.Collections;
#endif

namespace Smdn.Xml.Xhtml {
  public static class XmlElementExtensions {
    public static void SetStyleAttribute(this XmlElement element, IDictionary<string, string> styles)
    {
      if (element == null)
        throw new ArgumentNullException("element");
      if (styles == null)
        throw new ArgumentNullException("styles");

      if (0 < styles.Count)
#if NET_4_0
        element.SetAttribute("style", string.Join(" ", styles.Select(pair => string.Concat(pair.Key, ": ", pair.Value, ";"))));
#else
        element.SetAttribute("style", string.Join(" ", styles.Select(pair => string.Concat(pair.Key, ": ", pair.Value, ";")).ToArray()));
#endif
    }
  }
}

