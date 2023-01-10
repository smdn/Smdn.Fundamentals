// Smdn.Fundamental.Xml.Xhtml.dll (Smdn.Fundamental.Xml.Xhtml-3.0.1)
//   Name: Smdn.Fundamental.Xml.Xhtml
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+c848761b03aeddaf02bfeb277f3f5672e904cf60
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Shim, Version=3.1.3.0, Culture=neutral
//     Smdn.Fundamental.Xml.Linq, Version=3.0.1.0, Culture=neutral
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Smdn.Xml.Xhtml;

namespace Smdn.Xml.Linq.Xhtml {
  public static class Extensions {
    public static XElement GetElementById(this XContainer container, string id) {}
    public static bool HasHtmlClass(this XElement element, IEnumerable<string> classList) {}
    public static bool HasHtmlClass(this XElement element, string @class) {}
  }

  public static class XHtmlAttributeNames {
    public static readonly XName AccessKey; // = "accesskey"
    public static readonly XName Alt; // = "alt"
    public static readonly XName Cite; // = "cite"
    public static readonly XName Class; // = "class"
    public static readonly XName ContentEditable; // = "contenteditable"
    public static readonly XName Dir; // = "dir"
    public static readonly XName Download; // = "download"
    public static readonly XName Draggable; // = "draggable"
    public static readonly XName Hidden; // = "hidden"
    public static readonly XName Href; // = "href"
    public static readonly XName HrefLang; // = "hreflang"
    public static readonly XName Id; // = "id"
    public static readonly XName Lang; // = "lang"
    public static readonly XName Media; // = "media"
    public static readonly XName Rel; // = "rel"
    public static readonly XName SpellCheck; // = "spellcheck"
    public static readonly XName Src; // = "src"
    public static readonly XName Style; // = "style"
    public static readonly XName TabIndex; // = "tabindex"
    public static readonly XName Target; // = "target"
    public static readonly XName Title; // = "title"
    public static readonly XName Translate; // = "translate"
    public static readonly XName Type; // = "type"
  }

  public class XHtmlClassAttribute : XAttribute {
    public static string JoinClassList(IEnumerable<string> classList) {}

    public XHtmlClassAttribute(IEnumerable<string> classList) {}
    public XHtmlClassAttribute(params string[] classList) {}
    public XHtmlClassAttribute(string @class) {}
  }

  public static class XHtmlElementNames {
    public static readonly XName A; // = "{http://www.w3.org/1999/xhtml}a"
    public static readonly XName Abbr; // = "{http://www.w3.org/1999/xhtml}abbr"
    public static readonly XName Address; // = "{http://www.w3.org/1999/xhtml}address"
    public static readonly XName Area; // = "{http://www.w3.org/1999/xhtml}area"
    public static readonly XName Article; // = "{http://www.w3.org/1999/xhtml}article"
    public static readonly XName Aside; // = "{http://www.w3.org/1999/xhtml}aside"
    public static readonly XName Audio; // = "{http://www.w3.org/1999/xhtml}audio"
    public static readonly XName B; // = "{http://www.w3.org/1999/xhtml}b"
    public static readonly XName BR; // = "{http://www.w3.org/1999/xhtml}br"
    public static readonly XName Base; // = "{http://www.w3.org/1999/xhtml}base"
    public static readonly XName Blockquote; // = "{http://www.w3.org/1999/xhtml}blockquote"
    public static readonly XName Body; // = "{http://www.w3.org/1999/xhtml}body"
    public static readonly XName Button; // = "{http://www.w3.org/1999/xhtml}button"
    public static readonly XName Canvas; // = "{http://www.w3.org/1999/xhtml}canvas"
    public static readonly XName Caption; // = "{http://www.w3.org/1999/xhtml}caption"
    public static readonly XName Cite; // = "{http://www.w3.org/1999/xhtml}cite"
    public static readonly XName Code; // = "{http://www.w3.org/1999/xhtml}code"
    public static readonly XName Col; // = "{http://www.w3.org/1999/xhtml}col"
    public static readonly XName ColGroup; // = "{http://www.w3.org/1999/xhtml}colgroup"
    public static readonly XName DBI; // = "{http://www.w3.org/1999/xhtml}dbi"
    public static readonly XName DBO; // = "{http://www.w3.org/1999/xhtml}dbo"
    public static readonly XName DD; // = "{http://www.w3.org/1999/xhtml}dd"
    public static readonly XName DL; // = "{http://www.w3.org/1999/xhtml}dl"
    public static readonly XName DT; // = "{http://www.w3.org/1999/xhtml}dt"
    public static readonly XName Data; // = "{http://www.w3.org/1999/xhtml}data"
    public static readonly XName DataList; // = "{http://www.w3.org/1999/xhtml}datalist"
    public static readonly XName Del; // = "{http://www.w3.org/1999/xhtml}del"
    public static readonly XName Details; // = "{http://www.w3.org/1999/xhtml}details"
    public static readonly XName Dfn; // = "{http://www.w3.org/1999/xhtml}dfn"
    public static readonly XName Dialog; // = "{http://www.w3.org/1999/xhtml}dialog"
    public static readonly XName Div; // = "{http://www.w3.org/1999/xhtml}div"
    public static readonly XName EM; // = "{http://www.w3.org/1999/xhtml}em"
    public static readonly XName Embed; // = "{http://www.w3.org/1999/xhtml}embed"
    public static readonly XName FieldSet; // = "{http://www.w3.org/1999/xhtml}fieldset"
    public static readonly XName FigCaption; // = "{http://www.w3.org/1999/xhtml}figcaption"
    public static readonly XName Figure; // = "{http://www.w3.org/1999/xhtml}figure"
    public static readonly XName Footer; // = "{http://www.w3.org/1999/xhtml}footer"
    public static readonly XName Form; // = "{http://www.w3.org/1999/xhtml}form"
    public static readonly XName H1; // = "{http://www.w3.org/1999/xhtml}h1"
    public static readonly XName H2; // = "{http://www.w3.org/1999/xhtml}h2"
    public static readonly XName H3; // = "{http://www.w3.org/1999/xhtml}h3"
    public static readonly XName H4; // = "{http://www.w3.org/1999/xhtml}h4"
    public static readonly XName H5; // = "{http://www.w3.org/1999/xhtml}h5"
    public static readonly XName H6; // = "{http://www.w3.org/1999/xhtml}h6"
    public static readonly XName HR; // = "{http://www.w3.org/1999/xhtml}hr"
    public static readonly XName Head; // = "{http://www.w3.org/1999/xhtml}head"
    public static readonly XName Header; // = "{http://www.w3.org/1999/xhtml}header"
    public static readonly XName Html; // = "{http://www.w3.org/1999/xhtml}html"
    public static readonly XName I; // = "{http://www.w3.org/1999/xhtml}i"
    public static readonly XName IFrame; // = "{http://www.w3.org/1999/xhtml}iframe"
    public static readonly XName Img; // = "{http://www.w3.org/1999/xhtml}img"
    public static readonly XName Input; // = "{http://www.w3.org/1999/xhtml}input"
    public static readonly XName Ins; // = "{http://www.w3.org/1999/xhtml}ins"
    public static readonly XName Kbd; // = "{http://www.w3.org/1999/xhtml}kbd"
    public static readonly XName LI; // = "{http://www.w3.org/1999/xhtml}li"
    public static readonly XName Label; // = "{http://www.w3.org/1999/xhtml}label"
    public static readonly XName Legend; // = "{http://www.w3.org/1999/xhtml}legend"
    public static readonly XName Link; // = "{http://www.w3.org/1999/xhtml}link"
    public static readonly XName Main; // = "{http://www.w3.org/1999/xhtml}main"
    public static readonly XName Map; // = "{http://www.w3.org/1999/xhtml}map"
    public static readonly XName Mark; // = "{http://www.w3.org/1999/xhtml}mark"
    public static readonly XName Math; // = "{http://www.w3.org/1998/Math/MathML}math"
    public static readonly XName Meta; // = "{http://www.w3.org/1999/xhtml}meta"
    public static readonly XName Meter; // = "{http://www.w3.org/1999/xhtml}meter"
    public static readonly XName Nav; // = "{http://www.w3.org/1999/xhtml}nav"
    public static readonly XName NoScript; // = "{http://www.w3.org/1999/xhtml}noscript"
    public static readonly XName OL; // = "{http://www.w3.org/1999/xhtml}ol"
    public static readonly XName Object; // = "{http://www.w3.org/1999/xhtml}object"
    public static readonly XName OptGroup; // = "{http://www.w3.org/1999/xhtml}optgroup"
    public static readonly XName Option; // = "{http://www.w3.org/1999/xhtml}option"
    public static readonly XName Output; // = "{http://www.w3.org/1999/xhtml}output"
    public static readonly XName P; // = "{http://www.w3.org/1999/xhtml}p"
    public static readonly XName Param; // = "{http://www.w3.org/1999/xhtml}param"
    public static readonly XName Picture; // = "{http://www.w3.org/1999/xhtml}picture"
    public static readonly XName Pre; // = "{http://www.w3.org/1999/xhtml}pre"
    public static readonly XName Progress; // = "{http://www.w3.org/1999/xhtml}progress"
    public static readonly XName Q; // = "{http://www.w3.org/1999/xhtml}q"
    public static readonly XName RB; // = "{http://www.w3.org/1999/xhtml}rb"
    public static readonly XName RP; // = "{http://www.w3.org/1999/xhtml}rp"
    public static readonly XName RT; // = "{http://www.w3.org/1999/xhtml}rt"
    public static readonly XName RTC; // = "{http://www.w3.org/1999/xhtml}rtc"
    public static readonly XName Ruby; // = "{http://www.w3.org/1999/xhtml}ruby"
    public static readonly XName S; // = "{http://www.w3.org/1999/xhtml}s"
    public static readonly XName SVG; // = "{http://www.w3.org/2000/svg}svg"
    public static readonly XName Samp; // = "{http://www.w3.org/1999/xhtml}samp"
    public static readonly XName Script; // = "{http://www.w3.org/1999/xhtml}script"
    public static readonly XName Section; // = "{http://www.w3.org/1999/xhtml}section"
    public static readonly XName Select; // = "{http://www.w3.org/1999/xhtml}select"
    public static readonly XName Small; // = "{http://www.w3.org/1999/xhtml}small"
    public static readonly XName Source; // = "{http://www.w3.org/1999/xhtml}source"
    public static readonly XName Span; // = "{http://www.w3.org/1999/xhtml}span"
    public static readonly XName Strong; // = "{http://www.w3.org/1999/xhtml}strong"
    public static readonly XName Style; // = "{http://www.w3.org/1999/xhtml}style"
    public static readonly XName Sub; // = "{http://www.w3.org/1999/xhtml}sub"
    public static readonly XName Summary; // = "{http://www.w3.org/1999/xhtml}summary"
    public static readonly XName Sup; // = "{http://www.w3.org/1999/xhtml}sup"
    public static readonly XName TBody; // = "{http://www.w3.org/1999/xhtml}tbody"
    public static readonly XName TD; // = "{http://www.w3.org/1999/xhtml}td"
    public static readonly XName TFoot; // = "{http://www.w3.org/1999/xhtml}tfoot"
    public static readonly XName TH; // = "{http://www.w3.org/1999/xhtml}th"
    public static readonly XName THead; // = "{http://www.w3.org/1999/xhtml}thead"
    public static readonly XName TR; // = "{http://www.w3.org/1999/xhtml}tr"
    public static readonly XName Table; // = "{http://www.w3.org/1999/xhtml}table"
    public static readonly XName Template; // = "{http://www.w3.org/1999/xhtml}template"
    public static readonly XName TextArea; // = "{http://www.w3.org/1999/xhtml}textarea"
    public static readonly XName Time; // = "{http://www.w3.org/1999/xhtml}time"
    public static readonly XName Title; // = "{http://www.w3.org/1999/xhtml}title"
    public static readonly XName Track; // = "{http://www.w3.org/1999/xhtml}track"
    public static readonly XName U; // = "{http://www.w3.org/1999/xhtml}u"
    public static readonly XName UL; // = "{http://www.w3.org/1999/xhtml}ul"
    public static readonly XName Var; // = "{http://www.w3.org/1999/xhtml}var"
    public static readonly XName Video; // = "{http://www.w3.org/1999/xhtml}video"
    public static readonly XName WBR; // = "{http://www.w3.org/1999/xhtml}wbr"
  }

  public static class XHtmlNamespaces {
    public static readonly XNamespace Html; // = "http://www.w3.org/1999/xhtml"
    public static readonly XNamespace MathML; // = "http://www.w3.org/1998/Math/MathML"
    public static readonly XNamespace Svg; // = "http://www.w3.org/2000/svg"
    public static readonly XNamespace XLink; // = "http://www.w3.org/1999/xlink"
  }

  public class XHtmlStyleAttribute : XAttribute {
    protected static string ToJoined(IEnumerable<KeyValuePair<string, string>> styles) {}
    protected static string ToJoined(KeyValuePair<string, string> style) {}

    public XHtmlStyleAttribute(IEnumerable<KeyValuePair<string, string>> styles) {}
    public XHtmlStyleAttribute(IReadOnlyDictionary<string, string> styles) {}
    public XHtmlStyleAttribute(KeyValuePair<string, string> style) {}
    public XHtmlStyleAttribute(params KeyValuePair<string, string>[] styles) {}
  }
}

namespace Smdn.Xml.Xhtml {
  public static class HtmlConvert {
    public static string DecodeNumericCharacterReference(string s) {}
    public static string EscapeHtml(ReadOnlySpan<char> s) {}
    public static string EscapeXhtml(ReadOnlySpan<char> s) {}
    public static string UnescapeHtml(ReadOnlySpan<char> s) {}
    public static string UnescapeXhtml(ReadOnlySpan<char> s) {}
  }

  public class PolyglotHtml5Writer : XmlWriter {
    protected enum ExtendedWriteState : int {
      AttributeEnd = 6,
      AttributeStart = 4,
      AttributeValue = 5,
      Closed = 12,
      DocumentEnd = 11,
      DocumentStart = 1,
      ElementClosed = 10,
      ElementClosing = 9,
      ElementContent = 8,
      ElementOpened = 7,
      ElementOpening = 3,
      Prolog = 2,
      Start = 0,
    }

    public PolyglotHtml5Writer(Stream output, XmlWriterSettings settings = null) {}
    public PolyglotHtml5Writer(StringBuilder output, XmlWriterSettings settings = null) {}
    public PolyglotHtml5Writer(TextWriter output, XmlWriterSettings settings = null) {}
    public PolyglotHtml5Writer(string outputFileName, XmlWriterSettings settings = null) {}

    protected virtual XmlWriter BaseWriter { get; }
    protected PolyglotHtml5Writer.ExtendedWriteState ExtendedState { get; }
    public override XmlWriterSettings Settings { get; }
    public override WriteState WriteState { get; }
    public override string XmlLang { get; }
    public override XmlSpace XmlSpace { get; }

    protected override void Dispose(bool disposing) {}
    public override void Flush() {}
    public override string LookupPrefix(string ns) {}
    public override void WriteBase64(byte[] buffer, int index, int count) {}
    public override void WriteCData(string text) {}
    public override void WriteCharEntity(char ch) {}
    public override void WriteChars(char[] buffer, int index, int count) {}
    public override void WriteComment(string text) {}
    public override void WriteDocType(string name, string pubid, string sysid, string subset) {}
    public override void WriteEndAttribute() {}
    public override void WriteEndDocument() {}
    public override void WriteEndElement() {}
    public override void WriteEntityRef(string name) {}
    public override void WriteFullEndElement() {}
    protected virtual void WriteIndent() {}
    public override void WriteProcessingInstruction(string name, string text) {}
    public override void WriteRaw(char[] buffer, int index, int count) {}
    public override void WriteRaw(string data) {}
    public override void WriteStartAttribute(string prefix, string localName, string ns) {}
    public override void WriteStartDocument() {}
    public override void WriteStartDocument(bool standalone) {}
    public override void WriteStartElement(string prefix, string localName, string ns) {}
    public override void WriteString(string text) {}
    public override void WriteSurrogateCharEntity(char lowChar, char highChar) {}
    public override void WriteWhitespace(string ws) {}
  }

  public static class W3CNamespaces {
    public const string Html = "http://www.w3.org/1999/xhtml";
    public const string MathML = "http://www.w3.org/1998/Math/MathML";
    public const string Svg = "http://www.w3.org/2000/svg";
    public const string XLink = "http://www.w3.org/1999/xlink";
    public const string Xhtml = "http://www.w3.org/1999/xhtml";
  }
}
