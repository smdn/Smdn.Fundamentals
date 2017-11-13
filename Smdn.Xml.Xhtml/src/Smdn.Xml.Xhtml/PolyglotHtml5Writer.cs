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
    public override XmlWriterSettings Settings  => settings;
    public override WriteState WriteState       => baseWriter.WriteState;
    public override string XmlLang              => baseWriter.XmlLang;
    public override XmlSpace XmlSpace           => baseWriter.XmlSpace;
    protected virtual XmlWriter BaseWriter      => baseWriter;

    private readonly XmlWriter baseWriter;
    private readonly XmlWriterSettings settings;
    private bool shouldEmitIndent = false;

    private ElementContext currentElementContext = null;
    private readonly Stack<ElementContext> elementContextStack = new Stack<ElementContext>(4 /*nest level*/);

    private class ElementContext {
      private static readonly HashSet<string> voidElements = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
        "area", "base", "br", "col", "embed",
        "hr", "img", "input", "keygen", "link",
        "meta", "param", "source", "track", "wbr"
      };

      public readonly string LocalName;
      public readonly string Namespace;
      public bool IsMixedContent { get; private set; }
      public bool IsEmpty = true;
      public bool IsClosed = false;

      public bool IsNonVoidElement => !(voidElements.Contains(LocalName) && IsNamespaceXhtml);
      private bool IsNamespaceXhtml => string.IsNullOrEmpty(Namespace) || string.Equals(Namespace, W3CNamespaces.Xhtml, StringComparison.Ordinal);

      public ElementContext(string localName, string ns, ElementContext parentElementContext)
      {
        this.LocalName = localName;
        this.Namespace = ns;

        if (parentElementContext == null)
          IsMixedContent = false;
        else
          IsMixedContent = parentElementContext.IsMixedContent;

        if (IsNamespaceXhtml && string.Equals(localName, "pre", StringComparison.OrdinalIgnoreCase))
          // <pre> is treated as mixed content always
          IsMixedContent = true;
      }

      public void MarkAsMixedContent()
      {
        IsMixedContent = true;
      }
    }

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
      if (string.IsNullOrEmpty(pubid) && string.IsNullOrEmpty(sysid) && string.IsNullOrEmpty(subset))
        baseWriter.WriteRaw("<!DOCTYPE " + name + ">");
      else
        baseWriter.WriteDocType(name, pubid, sysid, subset);

      shouldEmitIndent = true;
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      var parentElementContext = currentElementContext;

      if (currentElementContext == null) {
        if (shouldEmitIndent) {
          WriteIndent(0);

          shouldEmitIndent = false;
        }
      }
      else {
        if (!currentElementContext.IsClosed) {
          currentElementContext.IsEmpty = false; // has child elements

          elementContextStack.Push(currentElementContext);
        }

        if (!currentElementContext.IsMixedContent)
          WriteIndent(elementContextStack.Count);
      }

      baseWriter.WriteStartElement(prefix, localName, ns);

      currentElementContext = new ElementContext(localName, ns, parentElementContext);

      shouldEmitIndent = true;
    }

    public override void WriteEndElement()
    {
      if (currentElementContext.IsNonVoidElement)
        // prepend empty text node to avoid emitting self closing tags <.../>
        baseWriter.WriteString(string.Empty);

      baseWriter.WriteEndElement();

      if (currentElementContext.IsEmpty) {
        currentElementContext.IsClosed = true;

        if (0 < elementContextStack.Count)
          currentElementContext = elementContextStack.Pop();
        else
          currentElementContext = null;
      }
    }

    public override void WriteFullEndElement()
    {
      currentElementContext.IsClosed = true;

      if (!currentElementContext.IsMixedContent)
        WriteIndent(elementContextStack.Count);

      if (0 < elementContextStack.Count)
        currentElementContext = elementContextStack.Pop();
      else
        currentElementContext = null;

      baseWriter.WriteFullEndElement();
    }

    protected virtual void WriteIndent(int indentLevel)
    {
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

    public override void WriteStartDocument()
    {
      baseWriter.WriteStartDocument();
    }

    public override void WriteStartDocument(bool standalone)
    {
      baseWriter.WriteStartDocument(standalone);
    }

    public override void WriteEndDocument()
    {
      baseWriter.WriteEndDocument();
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      baseWriter.WriteStartAttribute(prefix, localName, ns);
    }

    public override void WriteEndAttribute()
    {
      baseWriter.WriteEndAttribute();
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      baseWriter.WriteBase64(buffer, index, count);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteCData(string text)
    {
      baseWriter.WriteCData(text);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteCharEntity(char ch)
    {
      baseWriter.WriteCharEntity(ch);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      baseWriter.WriteChars(buffer, index, count);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteComment(string text)
    {
      if (currentElementContext == null) {
        if (shouldEmitIndent)
          WriteIndent(0);
      }
      else {
        if (!currentElementContext.IsMixedContent)
          WriteIndent(elementContextStack.Count + 1);
      }

      baseWriter.WriteComment(text);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content)
        currentElementContext.IsEmpty = false;

      shouldEmitIndent = true;
    }

    public override void WriteEntityRef(string name)
    {
      baseWriter.WriteEntityRef(name);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      baseWriter.WriteProcessingInstruction(name, text);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content)
        currentElementContext.IsEmpty = false;

      shouldEmitIndent = true;
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      baseWriter.WriteRaw(buffer, index, count);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteRaw(string data)
    {
      baseWriter.WriteRaw(data);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteString(string text)
    {
      baseWriter.WriteString(text);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      baseWriter.WriteSurrogateCharEntity(lowChar, highChar);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.IsEmpty = false;
      }
    }

    public override void WriteWhitespace(string ws)
    {
      baseWriter.WriteWhitespace(ws);

      if (currentElementContext != null && baseWriter.WriteState == WriteState.Content)
        currentElementContext.MarkAsMixedContent();
    }
  }
}