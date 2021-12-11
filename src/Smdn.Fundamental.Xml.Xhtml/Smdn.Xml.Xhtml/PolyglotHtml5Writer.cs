// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Smdn.Xml.Xhtml {
  public class PolyglotHtml5Writer : XmlWriter {
    public override XmlWriterSettings Settings => settings;
    public override WriteState WriteState => baseWriter.WriteState;
    public override string XmlLang => baseWriter.XmlLang;
    public override XmlSpace XmlSpace => baseWriter.XmlSpace;
    protected virtual XmlWriter BaseWriter => baseWriter;

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

    protected ExtendedWriteState ExtendedState { get; private set; } = ExtendedWriteState.Start;

    private ElementContext currentElementContext = null;
    private readonly Stack<ElementContext> elementContextStack = new(4 /*nest level*/);

    private class ElementContext {
      private static readonly HashSet<string> voidElements = new(StringComparer.OrdinalIgnoreCase) {
        "area",
        "base",
        "br",
        "col",
        "embed",
        "hr",
        "img",
        "input",
        "keygen",
        "link",
        "meta",
        "param",
        "source",
        "track",
        "wbr",
      };

      public string LocalName { get; }
      public string Namespace { get; }
      public bool IsMixedContent { get; private set; }
      public bool IsEmpty { get; private set; } = true;
      public bool IsClosed { get; private set; } = false;

      public bool IsNonVoidElement => !(voidElements.Contains(LocalName) && IsNamespaceXhtml);
      private bool IsNamespaceXhtml => string.IsNullOrEmpty(Namespace) || string.Equals(Namespace, W3CNamespaces.Xhtml, StringComparison.Ordinal);

      public ElementContext(string localName, string ns, ElementContext parentElementContext)
      {
        LocalName = localName;
        Namespace = ns;

        IsMixedContent = parentElementContext?.IsMixedContent ?? false;

        if (IsNamespaceXhtml && string.Equals(localName, "pre", StringComparison.OrdinalIgnoreCase))
          // <pre> is treated as mixed content always
          IsMixedContent = true;
      }

      public void MarkAsMixedContent() => IsMixedContent = true;
      public void MarkAsNonEmpty() => IsEmpty = false;
      public void MarkAsClosed() => IsClosed = true;
    }

    private static readonly XmlWriterSettings defaultSettings = new();

    private static XmlWriterSettings ToNonIndentingSettings(XmlWriterSettings settings)
    {
      if (settings == null)
        settings = defaultSettings;

      var s = settings.Clone();

      s.Indent = false;
      s.OmitXmlDeclaration = true;

      return s;
    }

#if !(NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER)
    private static XmlWriter Create(string outputFileName, XmlWriterSettings settings)
    {
      settings.CloseOutput = true;

      return Create(File.Create(outputFileName), settings);
    }
#endif

    public PolyglotHtml5Writer(string outputFileName, XmlWriterSettings settings = null)
      : this(Create(outputFileName, ToNonIndentingSettings(settings)), settings)
    {
    }

    public PolyglotHtml5Writer(StringBuilder output, XmlWriterSettings settings = null)
      : this(Create(output, ToNonIndentingSettings(settings)), settings)
    {
    }

    public PolyglotHtml5Writer(Stream output, XmlWriterSettings settings = null)
      : this(Create(output, ToNonIndentingSettings(settings)), settings)
    {
    }

    public PolyglotHtml5Writer(TextWriter output, XmlWriterSettings settings = null)
      : this(Create(output, ToNonIndentingSettings(settings)), settings)
    {
    }

    private PolyglotHtml5Writer(XmlWriter baseWriter, XmlWriterSettings settings)
    {
      this.baseWriter = baseWriter ?? throw new ArgumentNullException(nameof(baseWriter));
      this.settings = settings ?? defaultSettings.Clone();
    }

    protected override void Dispose(bool disposing)
    {
      try {
        if (disposing)
#if SYSTEM_IO_STREAM_CLOSE
          baseWriter.Close();
#else
          baseWriter.Dispose();
#endif
      }
      finally {
        base.Dispose(disposing);

        ExtendedState = ExtendedWriteState.Closed;
      }
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      if (string.IsNullOrEmpty(pubid) && string.IsNullOrEmpty(sysid) && string.IsNullOrEmpty(subset))
        baseWriter.WriteRaw("<!DOCTYPE " + name + ">");
      else
        baseWriter.WriteDocType(name, pubid, sysid, subset);

      ExtendedState = ExtendedWriteState.Prolog;
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      switch (ExtendedState) {
        case ExtendedWriteState.ElementOpening:
        case ExtendedWriteState.AttributeEnd:
          ExtendedState = ExtendedWriteState.ElementContent;
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

      ExtendedState = ExtendedWriteState.ElementOpening;
    }

    public override void WriteEndElement()
    {
      if (currentElementContext.IsNonVoidElement)
        // prepend empty text node to avoid emitting self closing tags <.../>
        baseWriter.WriteString(string.Empty);

      baseWriter.WriteEndElement();

      ExtendedState = ExtendedWriteState.ElementOpened;

      if (currentElementContext.IsEmpty)
        CloseCurrentElement();
    }

    public override void WriteFullEndElement()
    {
      ExtendedState = ExtendedWriteState.ElementClosing;

      WriteIndent();

      baseWriter.WriteFullEndElement();

      CloseCurrentElement();
    }

    private void CloseCurrentElement()
    {
      currentElementContext.MarkAsClosed();

      ExtendedState = ExtendedWriteState.ElementClosed;

      if (0 < elementContextStack.Count) {
        currentElementContext = elementContextStack.Pop();

        ExtendedState = ExtendedWriteState.ElementContent;
      }
      else {
        currentElementContext = null;
      }
    }

    private readonly List<string> indentStrings = new(4);

    protected virtual void WriteIndent()
    {
      if (!settings.Indent)
        return;
      if (ExtendedState is ExtendedWriteState.Start or ExtendedWriteState.DocumentStart)
        return;
      if (XmlSpace == XmlSpace.Preserve)
        return;
      if (currentElementContext != null) {
        if (currentElementContext.IsMixedContent)
          return;
        if (ExtendedState == ExtendedWriteState.ElementClosing &&
            currentElementContext.IsEmpty &&
            currentElementContext.IsNonVoidElement) {
          return;
        }
      }

      var indentLevel = elementContextStack.Count;

      if (ExtendedState == ExtendedWriteState.ElementContent)
        indentLevel += 1;

      // create indent string (NewLineChars + IndentChars * IndentLevel)
      if (indentStrings.Count <= indentLevel) {
        for (var level = indentStrings.Count; level <= indentLevel; level++) {
          var indentString = settings.NewLineChars;

          for (var l = 0; l < level; l++)
            indentString += settings.IndentChars;

          indentStrings.Add(indentString);
        }
      }

      baseWriter.WriteRaw(indentStrings[indentLevel]);
    }

    /*
     * call through to the base writer
     */
    public override void Flush() => baseWriter.Flush();

    public override string LookupPrefix(string ns) => baseWriter.LookupPrefix(ns);

    public override void WriteStartDocument()
    {
      baseWriter.WriteStartDocument();

      ExtendedState = ExtendedWriteState.DocumentStart;
    }

    public override void WriteStartDocument(bool standalone)
    {
      baseWriter.WriteStartDocument(standalone);

      ExtendedState = ExtendedWriteState.DocumentStart;
    }

    public override void WriteEndDocument()
    {
      baseWriter.WriteEndDocument();

      ExtendedState = ExtendedWriteState.DocumentEnd;
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      if (settings.Indent && settings.NewLineOnAttributes)
        throw new NotSupportedException("NewLineOnAttributes is not supported");
      // WriteIndent(); // causes InvalidOperationException

      baseWriter.WriteStartAttribute(prefix, localName, ns);

      ExtendedState = ExtendedWriteState.AttributeStart;
    }

    public override void WriteEndAttribute()
    {
      baseWriter.WriteEndAttribute();

      ExtendedState = ExtendedWriteState.AttributeEnd;
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      baseWriter.WriteProcessingInstruction(name, text);

      if (ExtendedState == ExtendedWriteState.ElementContent)
        currentElementContext.MarkAsNonEmpty();
    }

    public override void WriteComment(string text)
    {
      switch (ExtendedState) {
        case ExtendedWriteState.ElementOpening:
        case ExtendedWriteState.ElementOpened:
        case ExtendedWriteState.AttributeEnd:
          ExtendedState = ExtendedWriteState.ElementContent;
          break;
      }

      WriteIndent();

      baseWriter.WriteComment(text);

      if (ExtendedState == ExtendedWriteState.DocumentStart)
        ExtendedState = ExtendedWriteState.Prolog;
      else if (ExtendedState == ExtendedWriteState.ElementContent)
        currentElementContext.MarkAsNonEmpty();
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      SetWritingContentState();

      baseWriter.WriteBase64(buffer, index, count);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteCData(string text)
    {
      SetWritingContentState();

      baseWriter.WriteCData(text);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteCharEntity(char ch)
    {
      SetWritingContentState();

      baseWriter.WriteCharEntity(ch);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      SetWritingContentState();

      baseWriter.WriteChars(buffer, index, count);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteEntityRef(string name)
    {
      SetWritingContentState();

      baseWriter.WriteEntityRef(name);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      SetWritingContentState();

      baseWriter.WriteRaw(buffer, index, count);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteRaw(string data)
    {
      SetWritingContentState();

      baseWriter.WriteRaw(data);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteString(string text)
    {
      SetWritingContentState();

      baseWriter.WriteString(text);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      SetWritingContentState();

      baseWriter.WriteSurrogateCharEntity(lowChar, highChar);

      if (ExtendedState == ExtendedWriteState.ElementContent) {
        currentElementContext.MarkAsMixedContent();
        currentElementContext.MarkAsNonEmpty();
      }
    }

    public override void WriteWhitespace(string ws)
    {
      SetWritingContentState();

      baseWriter.WriteWhitespace(ws);

      if (ExtendedState == ExtendedWriteState.ElementContent)
        currentElementContext.MarkAsMixedContent();
    }

    private void SetWritingContentState()
    {
      switch (ExtendedState) {
        case ExtendedWriteState.ElementOpening:
        case ExtendedWriteState.ElementOpened:
        case ExtendedWriteState.AttributeEnd:
          ExtendedState = ExtendedWriteState.ElementContent;
          break;

        case ExtendedWriteState.AttributeStart:
          ExtendedState = ExtendedWriteState.AttributeValue;
          break;
      }
    }
  }
}
