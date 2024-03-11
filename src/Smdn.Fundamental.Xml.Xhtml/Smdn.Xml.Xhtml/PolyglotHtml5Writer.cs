// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Smdn.Xml.Xhtml;

#pragma warning disable SA1001
public class PolyglotHtml5Writer :
  XmlWriter
#if SYSTEM_IASYNCDISPOSABLE
  , IAsyncDisposable
#endif
#pragma warning restore SA1001
{
  public override XmlWriterSettings? Settings => settings;
  public override WriteState WriteState => baseWriter is null ? WriteState.Closed : baseWriter.WriteState;
  public override string? XmlLang => BaseWriter.XmlLang;
  public override XmlSpace XmlSpace => BaseWriter.XmlSpace;
  protected virtual XmlWriter BaseWriter => baseWriter ?? throw new ObjectDisposedException(GetType().FullName);

  private XmlWriter baseWriter;
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

  private ElementContext? currentElementContext = null;
  private readonly Stack<ElementContext> elementContextStack = new(4 /*nest level*/);

  private sealed class ElementContext {
    private static readonly HashSet<string> VoidElements = new(StringComparer.OrdinalIgnoreCase) {
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
    public string? Namespace { get; }
    public bool IsMixedContent { get; private set; }
    public bool IsEmpty { get; private set; } = true;
    public bool IsClosed { get; private set; } = false;

    public bool IsNonVoidElement => !(VoidElements.Contains(LocalName) && IsNamespaceXhtml);
    private bool IsNamespaceXhtml => string.IsNullOrEmpty(Namespace) || string.Equals(Namespace, W3CNamespaces.Xhtml, StringComparison.Ordinal);

    public ElementContext(string localName, string? ns, ElementContext? parentElementContext)
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

  private static readonly XmlWriterSettings DefaultWriterSettings = new();

  private static XmlWriterSettings ToNonIndentingSettings(XmlWriterSettings? settings)
  {
    settings ??= DefaultWriterSettings;

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

  public PolyglotHtml5Writer(string outputFileName, XmlWriterSettings? settings = null)
    : this(Create(outputFileName, ToNonIndentingSettings(settings)), settings)
  {
  }

  public PolyglotHtml5Writer(StringBuilder output, XmlWriterSettings? settings = null)
    : this(Create(output, ToNonIndentingSettings(settings)), settings)
  {
  }

  public PolyglotHtml5Writer(Stream output, XmlWriterSettings? settings = null)
    : this(Create(output, ToNonIndentingSettings(settings)), settings)
  {
  }

  public PolyglotHtml5Writer(TextWriter output, XmlWriterSettings? settings = null)
    : this(Create(output, ToNonIndentingSettings(settings)), settings)
  {
  }

  private PolyglotHtml5Writer(XmlWriter baseWriter, XmlWriterSettings? settings)
  {
    this.baseWriter = baseWriter ?? throw new ArgumentNullException(nameof(baseWriter));
    this.settings = settings ?? DefaultWriterSettings.Clone();
  }

  protected override void Dispose(bool disposing)
  {
    try {
      if (disposing)
#if SYSTEM_IO_STREAM_CLOSE
        baseWriter?.Close();
#else
        baseWriter?.Dispose();
#endif

      baseWriter = null!;
    }
    finally {
      base.Dispose(disposing);

      ExtendedState = ExtendedWriteState.Closed;
    }
  }

#if SYSTEM_IASYNCDISPOSABLE && !SYSTEM_XML_XMLWRITER_DISPOSEASYNC
  public virtual async ValueTask DisposeAsync()
  {
    await DisposeAsyncCore().ConfigureAwait(false);

    Dispose(false);

    GC.SuppressFinalize(this);
  }

  protected virtual
#if SYSTEM_XML_XMLWRITER_DISPOSEASYNC
  async
#endif
  ValueTask DisposeAsyncCore()
  {
    if (baseWriter is not null) {
#if SYSTEM_XML_XMLWRITER_DISPOSEASYNC
      await baseWriter.DisposeAsync().ConfigureAwait(false);
#else
      baseWriter.Dispose();
#endif
    }

    baseWriter = null!;

#if !SYSTEM_XML_XMLWRITER_DISPOSEASYNC
    return default;
#endif
  }
#endif

  public override void WriteDocType(string name, string? pubid, string? sysid, string? subset)
  {
    if (string.IsNullOrEmpty(pubid) && string.IsNullOrEmpty(sysid) && string.IsNullOrEmpty(subset))
      baseWriter.WriteRaw("<!DOCTYPE " + name + ">");
    else
      baseWriter.WriteDocType(name, pubid, sysid, subset);

    ExtendedState = ExtendedWriteState.Prolog;
  }

  public override async Task WriteDocTypeAsync(string name, string? pubid, string? sysid, string? subset)
  {
    if (string.IsNullOrEmpty(pubid) && string.IsNullOrEmpty(sysid) && string.IsNullOrEmpty(subset))
      await baseWriter.WriteRawAsync("<!DOCTYPE " + name + ">").ConfigureAwait(false);
    else
      await baseWriter.WriteDocTypeAsync(name, pubid, sysid, subset).ConfigureAwait(false);

    ExtendedState = ExtendedWriteState.Prolog;
  }

  private void PreWriteStartElement()
  {
    switch (ExtendedState) {
      case ExtendedWriteState.ElementOpening:
      case ExtendedWriteState.AttributeEnd:
        ExtendedState = ExtendedWriteState.ElementContent;
        break;
    }
  }

  private void PostWriteStartElement(string localName, string? ns)
  {
    // is nested element start?
    if (currentElementContext is not null && !currentElementContext.IsClosed) {
      currentElementContext.MarkAsNonEmpty(); // has child elements

      elementContextStack.Push(currentElementContext);
    }

    currentElementContext = new ElementContext(localName, ns, currentElementContext);

    ExtendedState = ExtendedWriteState.ElementOpening;
  }

  public override void WriteStartElement(string? prefix, string localName, string? ns)
  {
    PreWriteStartElement();

    WriteIndent();

    baseWriter.WriteStartElement(prefix, localName, ns);

    PostWriteStartElement(localName, ns);
  }

  public override async Task WriteStartElementAsync(string? prefix, string localName, string? ns)
  {
    PreWriteStartElement();

    await WriteIndentAsync().ConfigureAwait(false);

    await baseWriter.WriteStartElementAsync(prefix, localName, ns).ConfigureAwait(false);

    PostWriteStartElement(localName, ns);
  }

  public override void WriteEndElement()
  {
    if (currentElementContext is not null && currentElementContext.IsNonVoidElement)
      // prepend empty text node to avoid emitting self closing tags <.../>
      baseWriter.WriteString(string.Empty);

    baseWriter.WriteEndElement();

    ExtendedState = ExtendedWriteState.ElementOpened;

    if (currentElementContext is not null && currentElementContext.IsEmpty)
      CloseCurrentElement();
  }

  public override async Task WriteEndElementAsync()
  {
    if (currentElementContext is not null && currentElementContext.IsNonVoidElement)
      // prepend empty text node to avoid emitting self closing tags <.../>
      await baseWriter.WriteStringAsync(string.Empty).ConfigureAwait(false);

    await baseWriter.WriteEndElementAsync().ConfigureAwait(false);

    ExtendedState = ExtendedWriteState.ElementOpened;

    if (currentElementContext is not null && currentElementContext.IsEmpty)
      CloseCurrentElement();
  }

  public override void WriteFullEndElement()
  {
    ExtendedState = ExtendedWriteState.ElementClosing;

    WriteIndent();

    baseWriter.WriteFullEndElement();

    CloseCurrentElement();
  }

  public override async Task WriteFullEndElementAsync()
  {
    ExtendedState = ExtendedWriteState.ElementClosing;

    await WriteIndentAsync().ConfigureAwait(false);

    baseWriter.WriteFullEndElement();

    CloseCurrentElement();
  }

  private void CloseCurrentElement()
  {
    if (currentElementContext is not null)
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

  private bool PreWriteIndent(
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out string? indentString
  )
  {
    indentString = default;

    if (!settings.Indent)
      return false;
    if (ExtendedState is ExtendedWriteState.Start or ExtendedWriteState.DocumentStart)
      return false;
    if (XmlSpace == XmlSpace.Preserve)
      return false;
    if (currentElementContext is not null) {
      if (currentElementContext.IsMixedContent)
        return false;
      if (
        ExtendedState == ExtendedWriteState.ElementClosing &&
        currentElementContext.IsEmpty &&
        currentElementContext.IsNonVoidElement
      ) {
        return false;
      }
    }

    var indentLevel = elementContextStack.Count;

    if (ExtendedState == ExtendedWriteState.ElementContent)
      indentLevel += 1;

    // create indent string (NewLineChars + IndentChars * IndentLevel)
    if (indentStrings.Count <= indentLevel) {
      for (var level = indentStrings.Count; level <= indentLevel; level++) {
        var nextIndentString = settings.NewLineChars;

        for (var l = 0; l < level; l++)
          nextIndentString += settings.IndentChars;

        indentStrings.Add(nextIndentString);
      }
    }

    indentString = indentStrings[indentLevel];

    return true;
  }

  protected virtual void WriteIndent()
  {
    if (PreWriteIndent(out var indentString))
      baseWriter.WriteRaw(indentString);
  }

  protected virtual Task WriteIndentAsync()
  {
    if (PreWriteIndent(out var indentString))
      return baseWriter.WriteRawAsync(indentString);
    else
#if SYSTEM_THREADING_TASKS_TASK_COMPLETEDTASK
      return Task.CompletedTask;
#else
      return Task.FromResult(0);
#endif
  }

  /*
   * call through to the base writer
   */
  public override void Flush() => baseWriter.Flush();
  public override Task FlushAsync() => baseWriter.FlushAsync();

  public override string? LookupPrefix(string ns) => baseWriter.LookupPrefix(ns);

  public override void WriteStartDocument()
  {
    baseWriter.WriteStartDocument();

    ExtendedState = ExtendedWriteState.DocumentStart;
  }

  public override async Task WriteStartDocumentAsync()
  {
    await baseWriter.WriteStartDocumentAsync().ConfigureAwait(false);

    ExtendedState = ExtendedWriteState.DocumentStart;
  }

  public override void WriteStartDocument(bool standalone)
  {
    baseWriter.WriteStartDocument(standalone);

    ExtendedState = ExtendedWriteState.DocumentStart;
  }

  public override async Task WriteStartDocumentAsync(bool standalone)
  {
    await baseWriter.WriteStartDocumentAsync(standalone).ConfigureAwait(false);

    ExtendedState = ExtendedWriteState.DocumentStart;
  }

  public override void WriteEndDocument()
  {
    baseWriter.WriteEndDocument();

    ExtendedState = ExtendedWriteState.DocumentEnd;
  }

  public override async Task WriteEndDocumentAsync()
  {
    await baseWriter.WriteEndDocumentAsync().ConfigureAwait(false);

    ExtendedState = ExtendedWriteState.DocumentEnd;
  }

  public override void WriteStartAttribute(string? prefix, string localName, string? ns)
  {
    if (settings.Indent && settings.NewLineOnAttributes)
      throw new NotSupportedException("NewLineOnAttributes is not supported");
    // WriteIndent(); // causes InvalidOperationException

    baseWriter.WriteStartAttribute(prefix, localName, ns);

    ExtendedState = ExtendedWriteState.AttributeStart;
  }

  protected override Task WriteStartAttributeAsync(string? prefix, string localName, string? ns)
  {
    WriteStartAttribute(prefix, localName, ns); // cannot call baseWriter.WriteStartAttributeAsync

#if SYSTEM_THREADING_TASKS_TASK_COMPLETEDTASK
    return Task.CompletedTask;
#else
    return Task.FromResult(0);
#endif
  }

  public override void WriteEndAttribute()
  {
    baseWriter.WriteEndAttribute();

    ExtendedState = ExtendedWriteState.AttributeEnd;
  }

  protected override Task WriteEndAttributeAsync()
  {
    WriteEndAttribute(); // cannot call baseWriter.WriteEndAttributeAsync

#if SYSTEM_THREADING_TASKS_TASK_COMPLETEDTASK
    return Task.CompletedTask;
#else
    return Task.FromResult(0);
#endif
  }

  public override void WriteProcessingInstruction(string name, string? text)
  {
    baseWriter.WriteProcessingInstruction(name, text);

    SetWrittenContentState(markAsMixedContent: false);
  }

  public override async Task WriteProcessingInstructionAsync(string name, string? text)
  {
    await baseWriter.WriteProcessingInstructionAsync(name, text).ConfigureAwait(false);

    SetWrittenContentState(markAsMixedContent: false);
  }

  private void PreWriteComment()
  {
    switch (ExtendedState) {
      case ExtendedWriteState.ElementOpening:
      case ExtendedWriteState.ElementOpened:
      case ExtendedWriteState.AttributeEnd:
        ExtendedState = ExtendedWriteState.ElementContent;
        break;
    }
  }

  private void PostWriteComment()
  {
    if (ExtendedState == ExtendedWriteState.DocumentStart)
      ExtendedState = ExtendedWriteState.Prolog;
    else
      SetWrittenContentState(markAsMixedContent: false);
  }

  public override void WriteComment(string? text)
  {
    PreWriteComment();

    WriteIndent();

    baseWriter.WriteComment(text);

    PostWriteComment();
  }

  public override async Task WriteCommentAsync(string? text)
  {
    PreWriteComment();

    await WriteIndentAsync().ConfigureAwait(false);

    await baseWriter.WriteCommentAsync(text).ConfigureAwait(false);

    PostWriteComment();
  }

  public override void WriteBase64(byte[] buffer, int index, int count)
  {
    SetWritingContentState();

    baseWriter.WriteBase64(buffer, index, count);

    SetWrittenContentState();
  }

  public override async Task WriteBase64Async(byte[] buffer, int index, int count)
  {
    SetWritingContentState();

    await baseWriter.WriteBase64Async(buffer, index, count).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteCData(string? text)
  {
    SetWritingContentState();

    baseWriter.WriteCData(text);

    SetWrittenContentState();
  }

  public override async Task WriteCDataAsync(string? text)
  {
    SetWritingContentState();

    await baseWriter.WriteCDataAsync(text).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteCharEntity(char ch)
  {
    SetWritingContentState();

    baseWriter.WriteCharEntity(ch);

    SetWrittenContentState();
  }

  public override async Task WriteCharEntityAsync(char ch)
  {
    SetWritingContentState();

    await baseWriter.WriteCharEntityAsync(ch).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteChars(char[] buffer, int index, int count)
  {
    SetWritingContentState();

    baseWriter.WriteChars(buffer, index, count);

    SetWrittenContentState();
  }

  public override async Task WriteCharsAsync(char[] buffer, int index, int count)
  {
    SetWritingContentState();

    await baseWriter.WriteCharsAsync(buffer, index, count).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteEntityRef(string name)
  {
    SetWritingContentState();

    baseWriter.WriteEntityRef(name);

    SetWrittenContentState();
  }

  public override async Task WriteEntityRefAsync(string name)
  {
    SetWritingContentState();

    await baseWriter.WriteEntityRefAsync(name).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteRaw(char[] buffer, int index, int count)
  {
    SetWritingContentState();

    baseWriter.WriteRaw(buffer, index, count);

    SetWrittenContentState();
  }

  public override async Task WriteRawAsync(char[] buffer, int index, int count)
  {
    SetWritingContentState();

    await baseWriter.WriteRawAsync(buffer, index, count).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteRaw(string data)
  {
    SetWritingContentState();

    baseWriter.WriteRaw(data);

    SetWrittenContentState();
  }

  public override async Task WriteRawAsync(string data)
  {
    SetWritingContentState();

    await baseWriter.WriteRawAsync(data).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteString(string? text)
  {
    SetWritingContentState();

    baseWriter.WriteString(text);

    SetWrittenContentState();
  }

  public override async Task WriteStringAsync(string? text)
  {
    SetWritingContentState();

    await baseWriter.WriteStringAsync(text).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteSurrogateCharEntity(char lowChar, char highChar)
  {
    SetWritingContentState();

    baseWriter.WriteSurrogateCharEntity(lowChar, highChar);

    SetWrittenContentState();
  }

  public override async Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
  {
    SetWritingContentState();

    await baseWriter.WriteSurrogateCharEntityAsync(lowChar, highChar).ConfigureAwait(false);

    SetWrittenContentState();
  }

  public override void WriteWhitespace(string? ws)
  {
    SetWritingContentState();

    baseWriter.WriteWhitespace(ws);

    SetWrittenContentState(markAsNonEmpty: false);
  }

  public override async Task WriteWhitespaceAsync(string? ws)
  {
    SetWritingContentState();

    await baseWriter.WriteWhitespaceAsync(ws).ConfigureAwait(false);

    SetWrittenContentState(markAsNonEmpty: false);
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

  private void SetWrittenContentState(bool markAsMixedContent = true, bool markAsNonEmpty = true)
  {
    if (ExtendedState != ExtendedWriteState.ElementContent)
      return;

#if DEBUG
    if (currentElementContext is null)
      throw new InvalidOperationException("invalid state");
#endif

    if (markAsMixedContent)
      currentElementContext!.MarkAsMixedContent();

    if (markAsNonEmpty)
      currentElementContext!.MarkAsNonEmpty();
  }
}
