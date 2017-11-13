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

    protected enum ExtendedWriteState {
      Start,
      DocumentStart,
      Prolog,
      ElementOpening,
      AttributeStart,
      AttributeValue,
      AttributeEnd,
      ElementOpened,
      ElementContent,
      ElementClosing,
      ElementClosed,
      DocumentEnd,
      Closed,
    }

    protected ExtendedWriteState ExtendedState => state;

    private ExtendedWriteState state = ExtendedWriteState.Start;

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
      public bool IsEmpty { get; private set; } = true;
      public bool IsClosed { get; private set; } = false;

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

      public void MarkAsNonEmpty()
      {
        IsEmpty = false;
      }

      public void MarkAsClosed()
      {
        IsClosed = true;
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

        state = ExtendedWriteState.Closed;
      }
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      if (string.IsNullOrEmpty(pubid) && string.IsNullOrEmpty(sysid) && string.IsNullOrEmpty(subset))
        baseWriter.WriteRaw("<!DOCTYPE " + name + ">");
      else
        baseWriter.WriteDocType(name, pubid, sysid, subset);

      state = ExtendedWriteState.Prolog;
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      switch (state) {
        case ExtendedWriteState.ElementOpening:
        case ExtendedWriteState.AttributeEnd:
        case ExtendedWriteState.ElementClosed:
          state = ExtendedWriteState.ElementContent;
          break;
      }

      WriteIndent();

      baseWriter.WriteStartElement(prefix, localName, ns);

      // is nested element start?
      if (currentElementContext != null && !currentElementContext.IsClosed) {
        currentElementContext.MarkAsNonEmpty(); // has child elements

        elementContextStack.Push(currentElementContext);
      }

      currentElementContext = new ElementContext(localName, ns, currentElementContext);

      state = ExtendedWriteState.ElementOpening;
    }

    public override void WriteEndElement()
    {
      if (currentElementContext.IsNonVoidElement)
        // prepend empty text node to avoid emitting self closing tags <.../>
        baseWriter.WriteString(string.Empty);

      baseWriter.WriteEndElement();

      state = ExtendedWriteState.ElementOpened;

      if (currentElementContext.IsEmpty) {
        currentElementContext.MarkAsClosed();

        state = ExtendedWriteState.ElementClosed;

        if (0 < elementContextStack.Count)
          currentElementContext = elementContextStack.Pop();
        else
          currentElementContext = null;
      }
    }

    public override void WriteFullEndElement()
    {
      var prevElementContext = currentElementContext;

      state = ExtendedWriteState.ElementClosing;

      WriteIndent();

      if (0 < elementContextStack.Count)
        currentElementContext = elementContextStack.Pop();
      else
        currentElementContext = null;

      baseWriter.WriteFullEndElement();

      prevElementContext.MarkAsClosed();

      state = ExtendedWriteState.ElementClosed;
    }

    protected virtual void WriteIndent()
    {
      if (state == ExtendedWriteState.Start || state == ExtendedWriteState.DocumentStart)
        return;

      if (XmlSpace == XmlSpace.Preserve)
        return;

      if (currentElementContext != null && currentElementContext.IsMixedContent)
        return;

      baseWriter.WriteRaw(settings.NewLineChars);

      if (!settings.Indent)
        return;

      var indentLevel = elementContextStack.Count;

      if (state == ExtendedWriteState.ElementContent)
        indentLevel += 1;

      for (var i = 0; i < indentLevel; i++) {
        baseWriter.WriteRaw(settings.IndentChars);
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

      state = ExtendedWriteState.DocumentStart;
    }

    public override void WriteStartDocument(bool standalone)
    {
      baseWriter.WriteStartDocument(standalone);

      state = ExtendedWriteState.DocumentStart;
    }

    public override void WriteEndDocument()
    {
      baseWriter.WriteEndDocument();

      state = ExtendedWriteState.DocumentEnd;
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      baseWriter.WriteStartAttribute(prefix, localName, ns);

      state = ExtendedWriteState.AttributeStart;
    }

    public override void WriteEndAttribute()
    {
      baseWriter.WriteEndAttribute();

      state = ExtendedWriteState.AttributeEnd;
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      baseWriter.WriteProcessingInstruction(name, text);

      if (state == ExtendedWriteState.ElementContent)
        currentElementContext.MarkAsNonEmpty();
    }

    public override void WriteComment(string text)
    {
      switch (state) {
        case ExtendedWriteState.ElementOpening:
        case ExtendedWriteState.ElementOpened:
        case ExtendedWriteState.AttributeEnd:
          state = ExtendedWriteState.ElementContent;
          break;
      }

      WriteIndent();

      baseWriter.WriteComment(text);

      if (state == ExtendedWriteState.DocumentStart)
        state = ExtendedWriteState.Prolog;
      else if (state == ExtendedWriteState.ElementContent)
        currentElementContext.MarkAsNonEmpty();
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      SetWritingContentState();

      baseWriter.WriteBase64(buffer, index, count);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteCData(string text)
    {
      SetWritingContentState();

      baseWriter.WriteCData(text);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteCharEntity(char ch)
    {
      SetWritingContentState();

      baseWriter.WriteCharEntity(ch);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      SetWritingContentState();

      baseWriter.WriteChars(buffer, index, count);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteEntityRef(string name)
    {
      SetWritingContentState();

      baseWriter.WriteEntityRef(name);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      SetWritingContentState();

      baseWriter.WriteRaw(buffer, index, count);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteRaw(string data)
    {
      SetWritingContentState();

      baseWriter.WriteRaw(data);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteString(string text)
    {
      SetWritingContentState();

      baseWriter.WriteString(text);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      SetWritingContentState();

      baseWriter.WriteSurrogateCharEntity(lowChar, highChar);

      if (state == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteWhitespace(string ws)
    {
      SetWritingContentState();

      baseWriter.WriteWhitespace(ws);

      if (state == ExtendedWriteState.ElementContent)
        currentElementContext.MarkAsMixedContent();
    }

    private void SetWritingContentState()
    {
      switch (state) {
        case ExtendedWriteState.ElementOpening:
        case ExtendedWriteState.ElementOpened:
        case ExtendedWriteState.AttributeEnd:
        case ExtendedWriteState.ElementClosed:
          state = ExtendedWriteState.ElementContent;
          break;

        case ExtendedWriteState.AttributeStart:
          state = ExtendedWriteState.AttributeValue;
          break;
      }
    }
  }
}