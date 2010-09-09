// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
using System.Text;
using System.Xml;

namespace Smdn.Xml.Xhtml {
  public static class XhtmlUtils {
    public static string ToString(XmlNode node, XmlWriterSettings settings)
    {
      if (node == null)
        throw new ArgumentNullException();

      var sb = new StringBuilder();
      var writer = XmlWriter.Create(sb, settings);

      if (node is XhtmlDocument) {
        ProcessDocument(node as XhtmlDocument, delegate(XmlDocument doc) {
          doc.WriteContentTo(writer);
        });
      }
      else {
        node.WriteContentTo(writer);
      }

      writer.Flush();

      if (node is XhtmlDocument)
        sb.Replace("\"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"[]>", "\"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");

      sb.Replace("<pre>\u200b", "<pre>");

      return sb.ToString();
    }

    private static void ProcessDocument(XhtmlDocument document, Action<XmlDocument> action)
    {
      var nsmgr = new XmlNamespaceManager(document.NameTable);

      try {
        nsmgr.PushScope();
        nsmgr.AddNamespace("x", W3CNamespaces.Xhtml);

        // TODO: clone
#if false
        // System.Xml.XmlException: DocumentType cannot be imported.
        var doc = (XmlDocument)document.Clone();
#else
        var doc = document;
#endif

        foreach (XmlNode pre in document.SelectNodes("//x:pre", nsmgr)) {
          // HACK: insert 'ZERO WIDTH SPACE' (U+200B) to avoid indenting
          pre.PrependChild(document.CreateTextNode("\u200b"));
        }

        action(doc);
      }
      finally {
        nsmgr.PopScope();
      }
    }
  }
}
