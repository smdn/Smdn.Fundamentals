// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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
using System.IO;
using System.Text;
using System.Xml;

namespace Smdn.Xml.Xhtml {
  public class PolyglotHtml5Writer : XmlWriter {
    private static readonly string[] voidElements = {
      "area", "base", "br", "col", "embed",
      "hr", "img", "input", "keygen", "link",
      "meta", "param", "source", "track", "wbr"
    };

    static PolyglotHtml5Writer()
    {
      Array.Sort(voidElements, StringComparer.Ordinal);
    }

    private readonly XmlWriter baseWriter;
    private readonly XmlWriterSettings settings;
    private bool shouldEmitIndent = false;

    public override XmlWriterSettings Settings => settings;

    private class ElementContext {
      public readonly string LocalName;
      public readonly bool IsInNonIndenting;
      public bool IsMixedContent = false;
      public bool IsEmpty = true;

      public ElementContext(string localName, bool isInNonIndenting)
      {
        this.LocalName = localName;
        this.IsInNonIndenting = isInNonIndenting;
      }
    }

    private ElementContext currentElementContext = null;
    private readonly Stack<ElementContext> elementContextStack = new Stack<ElementContext>(4 /*nest level*/);

    private static XmlWriterSettings ToNonIndentingSettings(XmlWriterSettings settings)
    {
      var s = settings.Clone();

      s.Indent = false;
      s.IndentChars = string.Empty;
      s.NewLineChars = string.Empty;

      s.NewLineHandling = NewLineHandling.None;

      return s;
    }

    public PolyglotHtml5Writer(string outputFileName, XmlWriterSettings settings)
      : this(Create(outputFileName, ToNonIndentingSettings(settings)), settings)
    {
    }

    public PolyglotHtml5Writer(StringBuilder output, XmlWriterSettings settings)
      : this(Create(output, ToNonIndentingSettings(settings)), settings)
    {
    }

    public PolyglotHtml5Writer(Stream output, XmlWriterSettings settings)
      : this(Create(output, ToNonIndentingSettings(settings)), settings)
    {
    }

    public PolyglotHtml5Writer(TextWriter output, XmlWriterSettings settings)
      : this(Create(output, ToNonIndentingSettings(settings)), settings)
    {
    }

    private PolyglotHtml5Writer(XmlWriter baseWriter, XmlWriterSettings settings)
    {
      if (baseWriter == null)
        throw new ArgumentNullException(nameof(baseWriter));

      this.baseWriter = baseWriter;
      this.settings = settings;
    }

    protected override void Dispose(bool disposing)
    {
      try {
        if (disposing)
          baseWriter.Close();
      }
      finally {
        base.Dispose(disposing);
      }
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      if (pubid == null && sysid == null && subset == null)
        baseWriter.WriteRaw("<!DOCTYPE " + name + ">");
      else
        baseWriter.WriteDocType(name, pubid, sysid, subset);

      shouldEmitIndent = true;
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      var isInNonIndenting = false;

      if (currentElementContext != null) {
        // TODO: compare namespace
        if (Array.BinarySearch(voidElements, currentElementContext.LocalName) < 0)
          currentElementContext.IsEmpty = false;

        isInNonIndenting = currentElementContext.IsInNonIndenting;
      }
 
      if (!isInNonIndenting)
        WriteIndent();

      baseWriter.WriteStartElement(prefix, localName, ns);

      // TODO: compare namespace
      isInNonIndenting |= string.Equals(localName, "pre", StringComparison.OrdinalIgnoreCase);

      currentElementContext = new ElementContext(localName, isInNonIndenting);

      elementContextStack.Push(currentElementContext);

      shouldEmitIndent = true;
    }

    public override void WriteEndElement()
    {
      // TODO: compare namespace
      if (Array.BinarySearch(voidElements, currentElementContext.LocalName) < 0)
        // prepend empty text node if element is self-closing
        baseWriter.WriteString(string.Empty);

      baseWriter.WriteEndElement();

      if (currentElementContext.IsEmpty)
        currentElementContext = elementContextStack.Pop();
    }

    public override void WriteFullEndElement()
    {
      currentElementContext = elementContextStack.Pop();

      if (!(currentElementContext.IsMixedContent || currentElementContext.IsInNonIndenting)) {
        if (currentElementContext.IsEmpty)
          WriteIndent(elementContextStack.Count - 1);
        else
          WriteIndent();
      }

      baseWriter.WriteFullEndElement();
    }

    private void WriteIndent()
    {
      WriteIndent(elementContextStack.Count);
    }

    protected virtual void WriteIndent(int indentLevel)
    {
      if (!shouldEmitIndent)
        return;

      baseWriter.WriteRaw(settings.NewLineChars);

      if (settings.Indent) {
        for (var i = 0; i < indentLevel; i++) {
          baseWriter.WriteRaw(settings.IndentChars);
        }
      }
    }

    /*
     * call through to the base writer
     */
    public override void Flush()
    {
      baseWriter.Flush();
    }

    public override string LookupPrefix(string ns)
    {
      return baseWriter.LookupPrefix(ns);
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteBase64(buffer, index, count);
    }

    public override void WriteCData(string text)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteCData(text);
    }

    public override void WriteCharEntity(char ch)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteCharEntity(ch);
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteChars(buffer, index, count);
    }

    public override void WriteComment(string text)
    {
      currentElementContext.IsEmpty = false;

      baseWriter.WriteComment(text);
    }

    public override void WriteEndAttribute()
    {
      baseWriter.WriteEndAttribute();
    }

    public override void WriteEndDocument()
    {
      baseWriter.WriteEndDocument();
    }

    public override void WriteEntityRef(string name)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteEntityRef(name);
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      currentElementContext.IsEmpty = false;

      baseWriter.WriteProcessingInstruction(name, text);

      shouldEmitIndent = true;
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteRaw(buffer, index, count);
    }

    public override void WriteRaw(string data)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteRaw(data);
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      baseWriter.WriteStartAttribute(prefix, localName, ns);
    }

    public override void WriteStartDocument()
    {
      baseWriter.WriteStartDocument();
    }

    public override void WriteStartDocument(bool standalone)
    {
      baseWriter.WriteStartDocument(standalone);
    }

    public override void WriteString(string text)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteString(text);
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      currentElementContext.IsMixedContent = true;
      currentElementContext.IsEmpty = false;

      baseWriter.WriteSurrogateCharEntity(lowChar, highChar);
    }

    public override void WriteWhitespace(string ws)
    {
      currentElementContext.IsMixedContent = true;

      baseWriter.WriteWhitespace(ws);
    }

    public override WriteState          WriteState  => baseWriter.WriteState;
    public override string              XmlLang     => baseWriter.XmlLang;
    public override XmlSpace            XmlSpace    => baseWriter.XmlSpace;
  }
}