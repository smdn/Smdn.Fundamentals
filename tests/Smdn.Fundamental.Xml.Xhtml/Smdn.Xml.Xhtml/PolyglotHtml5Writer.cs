// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

using Smdn.Xml.Linq;

namespace Smdn.Xml.Xhtml {
  [TestFixture]
  public class PolyglotHtml5WriterTests {
    private static readonly string[] VoldElementNames = new[] { "area", "base", "br", "col", "embed", "hr", "img", "input", "keygen", "link", "meta", "param", "source", "track", "wbr" };
    private static readonly string[] SelfClosingElementNames = new[] { "div", "span", "script", "html", "body", "head" };

    [Test]
    public void TestConstructor_SettingsNull()
    {
      var doc = new XDocument(new XElement("html"));

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(sb)) {
          Assert.That(writer.Settings, Is.Not.Null);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(sb, null)) {
          Assert.That(writer.Settings, Is.Not.Null);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(Stream.Null)) {
          Assert.That(writer.Settings, Is.Not.Null);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(Stream.Null, null)) {
          Assert.That(writer.Settings, Is.Not.Null);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(TextWriter.Null)) {
          Assert.That(writer.Settings, Is.Not.Null);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(TextWriter.Null, null)) {
          Assert.That(writer.Settings, Is.Not.Null);

          doc.Save(writer);
        }
      });
    }

    [Test]
    public void TestFlush()
    {
      using var output = new MemoryStream();
      using var writer = new PolyglotHtml5Writer(
        output,
        new XmlWriterSettings() {
          CloseOutput = false,
        }
      );

      Assert.That(output.Length, Is.Zero);

      writer.WriteDocType("html", null, null, null);
      writer.Flush();

      Assert.That(output.Length, Is.Not.Zero);
    }

    [Test]
    public async Task TestFlushAsync()
    {
#if SYSTEM_IO_STREAM_DISPOSEASYNC
      await
#endif
      using var output = new MemoryStream();

#if SYSTEM_XML_XMLWRITER_DISPOSEASYNC
      await
#endif
      using var writer = new PolyglotHtml5Writer(
        output,
        new XmlWriterSettings() {
          Async = true,
          CloseOutput = false,
        }
      );

      Assert.That(output.Length, Is.Zero);

      await writer.WriteDocTypeAsync("html", null, null, null).ConfigureAwait(false);
      await writer.FlushAsync().ConfigureAwait(false);

      Assert.That(output.Length, Is.Not.Zero);
    }

    [Test]
    public void TestDispose_CloseOutput()
    {
      var doc = new XDocument(new XElement("html"));

      using (var output = new MemoryStream()) {
        var settings = new XmlWriterSettings();

        settings.CloseOutput = true;

        using (var writer = new PolyglotHtml5Writer(output, settings)) {
          doc.Save(writer);
        }

        Assert.Throws<ObjectDisposedException>(() => output.ReadByte());
      }
    }

#if SYSTEM_XML_XMLWRITER_DISPOSEASYNC
    [Test]
    public async Task TestDisposeAsync_CloseOutput()
    {
      var doc = new XDocument(new XElement("html"));

      using (var output = new MemoryStream()) {
        var settings = new XmlWriterSettings();

        settings.CloseOutput = true;

        await using (var writer = new PolyglotHtml5Writer(output, settings)) {
          doc.Save(writer);
        }

        Assert.Throws<ObjectDisposedException>(() => output.ReadByte());
      }
    }
#endif

    [Test]
    public void TestDispose_DoNotCloseOutput()
    {
      var doc = new XDocument(new XElement("html"));

      using (var output = new MemoryStream()) {
        var settings = new XmlWriterSettings();

        settings.CloseOutput = false;

        using (var writer = new PolyglotHtml5Writer(output, settings)) {
          doc.Save(writer);
        }

        Assert.DoesNotThrow(() => output.ReadByte());
      }
    }

#if SYSTEM_XML_XMLWRITER_DISPOSEASYNC
    [Test]
    public async Task TestDisposeAsync_DoNotCloseOutput()
    {
      var doc = new XDocument(new XElement("html"));

      using (var output = new MemoryStream()) {
        var settings = new XmlWriterSettings();

        settings.CloseOutput = false;

        await using (var writer = new PolyglotHtml5Writer(output, settings)) {
          doc.Save(writer);
        }

        Assert.DoesNotThrow(() => output.ReadByte());
      }
    }
#endif

    private static string ToString(XDocument doc, NewLineHandling newLineHandling, string? newLineChars = null)
    {
      var settings = new XmlWriterSettings();

      settings.Async = false;
      settings.NewLineHandling = newLineHandling;
      settings.NewLineChars = newLineChars ?? "\n";

      return ToString(doc, settings);
    }

    private static Task<string> ToStringAsync(XDocument doc, NewLineHandling newLineHandling, string? newLineChars = null)
    {
      var settings = new XmlWriterSettings();

      settings.Async = true;
      settings.NewLineHandling = newLineHandling;
      settings.NewLineChars = newLineChars ?? "\n";

      return ToStringAsync(doc, settings);
    }

    private static string ToString(XDocument doc, XmlWriterSettings? settings = null)
    {
      if (settings == null) {
        settings = new XmlWriterSettings();

        settings.Async = false;
        settings.NewLineChars = "\n";
        settings.ConformanceLevel = ConformanceLevel.Document;
        settings.Indent = true;
        settings.IndentChars = " ";
        settings.NewLineOnAttributes = false;
      }

      var output = new StringBuilder();

      using (var writer = new PolyglotHtml5Writer(output, settings)) {
        doc.Save(writer);
      }

      return output.ToString();
    }

    private static async Task<string> ToStringAsync(XDocument doc, XmlWriterSettings? settings = null)
    {
      if (settings == null) {
        settings = new XmlWriterSettings();

        settings.Async = true;
        settings.NewLineChars = "\n";
        settings.ConformanceLevel = ConformanceLevel.Document;
        settings.Indent = true;
        settings.IndentChars = " ";
        settings.NewLineOnAttributes = false;
      }

      var output = new StringBuilder();

#if SYSTEM_XML_XMLWRITER_DISPOSEASYNC
      await
#endif
      using (var writer = new PolyglotHtml5Writer(output, settings)) {
#if SYSTEM_XML_LINQ_XNODE_WRITETOASYNC
        await doc.SaveAsync(writer, default).ConfigureAwait(false);
#else
        doc.Save(writer);

        Assert.Ignore("XNode.WriteToAsync is not supported on this framework");

        await Task.FromResult(0);
#endif
      }

      return output.ToString();
    }

    private static string ToString(XmlDocument doc)
    {
      var settings = new XmlWriterSettings();

      settings.ConformanceLevel = ConformanceLevel.Document;
      settings.Indent = true;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = false;

      var output = new StringBuilder();

      using (var writer = new PolyglotHtml5Writer(output, settings)) {
        doc.Save(writer);
      }

      return output.ToString();
    }

    [Test]
    public async Task TestWrite_Html5Document([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XDocumentType("html", null!, null!, null!),
        new XElement(
          "html",
          new XElement(
            "head",
            new XElement(
              "meta",
              new XAttribute("charset", "utf-8")
            ),
            new XElement(
              "title",
              "title"
            ),
            new XElement(
              "link",
              new XAttribute("rel", "canonical"),
              new XAttribute("href", "http://example.com/")
            )
          ),
          new XElement(
            "body",
            new XElement(
              "p",
              new XElement(
                "span",
                new XAttribute("style", "display:none;"),
                new XText("p1")
              )
            ),
            new XElement(
              "p",
              new XText("p2-1"),
              new XElement(
                "span",
                new XText("p2-2")
              )
            )
          )
        )
      );

      const string expected = @"<!DOCTYPE html>
<html>
 <head>
  <meta charset=""utf-8"" />
  <title>title</title>
  <link rel=""canonical"" href=""http://example.com/"" />
 </head>
 <body>
  <p>
   <span style=""display:none;"">p1</span>
  </p>
  <p>p2-1<span>p2-2</span></p>
 </body>
</html>";

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo(expected.Replace("\r\n", "\n"))
      );
    }

    [Test]
    public async Task TestWriteProlog_AlwaysOmitXmlDeclaration([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XDeclaration("1.0", "utf-8", "yes"),
        new XElement("html")
      );

      var settings = new XmlWriterSettings();

      settings.Async = asAsync;
      settings.Indent = false;
      settings.OmitXmlDeclaration = false;

      Assert.That(
        asAsync ? await ToStringAsync(doc, settings).ConfigureAwait(false) : ToString(doc, settings),
        Is.EqualTo("<html></html>")
      );

      settings.OmitXmlDeclaration = true;

      Assert.That(
        asAsync ? await ToStringAsync(doc, settings).ConfigureAwait(false) : ToString(doc, settings),
        Is.EqualTo("<html></html>")
      );
    }

    [Test]
    public async Task TestWriteDocType_XDocument_NullId([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XDocumentType("html", null!, null!, null!),
        new XElement("html")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<!DOCTYPE html>\n<html></html>")
      );
    }

    [Test]
    public async Task TestWriteDocType_XDocument_EmptyId([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XDocumentType("html", string.Empty, string.Empty, string.Empty),
        new XElement("html")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<!DOCTYPE html>\n<html></html>")
      );
    }

    [Test]
    public void TestWriteDocType_XmlDocument()
    {
      var doc = new XmlDocument() {
#if SYSTEM_XML_XMLDOCUMENT_XMLRESOLVER
        XmlResolver = null!
#endif
      };

      doc.AppendChild(doc.CreateDocumentType("html", null, null, null));
      doc.AppendChild(doc.CreateElement("html"));

      Assert.That(ToString(doc), Is.EqualTo("<!DOCTYPE html>\n<html></html>"));
    }

    [Test]
    public async Task TestWriteAttribute_ValueEscaped(
      [Values(true, false)] bool asAsync,
      [Values(NewLineHandling.None, NewLineHandling.Entitize, NewLineHandling.Replace)] NewLineHandling newLineHandling
    )
    {
      var doc = new XDocument(
        new XElement(
          "meta",
          new XAttribute("name", "description"),
          new XAttribute("content", "<>&\"\'")
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, newLineHandling).ConfigureAwait(false) : ToString(doc, newLineHandling),
        Is.EqualTo("<meta name=\"description\" content=\"&lt;&gt;&amp;&quot;\'\" />")
      );
    }

    [Test]
    public async Task TestWriteAttribute_NewLineHandling_None([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "meta",
          new XAttribute("name", "description"),
          new XAttribute("content", "\r\n\t")
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, NewLineHandling.None).ConfigureAwait(false) : ToString(doc, NewLineHandling.None),
        Is.EqualTo("<meta name=\"description\" content=\"\r\n\t\" />")
      );
    }

    [Test]
    public async Task TestWriteAttribute_NewLineHandling_EntitizeAndReplace(
      [Values(true, false)] bool asAsync,
      [Values(NewLineHandling.Entitize, NewLineHandling.Replace)] NewLineHandling newLineHandling
    )
    {
      var doc = new XDocument(
        new XElement(
          "meta",
          new XAttribute("name", "description"),
          new XAttribute("content", "\r\n\t")
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, newLineHandling).ConfigureAwait(false) : ToString(doc, newLineHandling),
        Is.EqualTo("<meta name=\"description\" content=\"&#xD;&#xA;&#x9;\" />")
      );
    }

    [Test]
    public async Task TestWriteTextNode_TextEscaped(
      [Values(true, false)] bool asAsync,
      [Values(NewLineHandling.None, NewLineHandling.Entitize, NewLineHandling.Replace)] NewLineHandling newLineHandling
    )
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("<>&\"\'")
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, newLineHandling).ConfigureAwait(false) : ToString(doc, newLineHandling),
        Is.EqualTo("<div>&lt;&gt;&amp;\"\'</div>")
      );
    }

    [Test]
    public async Task TestWriteTextNode_NewLineHandling_None([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("\r\n\t")
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, NewLineHandling.None).ConfigureAwait(false) : ToString(doc, NewLineHandling.None),
        Is.EqualTo("<div>\r\n\t</div>")
      );
    }

    [Test]
    public async Task TestWriteTextNode_NewLineHandling_Entitize([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("\r\n")
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, NewLineHandling.Entitize).ConfigureAwait(false) : ToString(doc, NewLineHandling.Entitize),
        Is.EqualTo("<div>&#xD;\n</div>")
      );
    }

    [Test]
    public async Task TestWriteTextNode_NewLineHandling_Replace([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("\r\n-\r-\n")
        )
      );

      Assert.That(
        asAsync
          ? await ToStringAsync(doc, NewLineHandling.Replace, "\n").ConfigureAwait(false)
          : ToString(doc, NewLineHandling.Replace, "\n"),
        Is.EqualTo("<div>\n-\n-\n</div>")
      );
      Assert.That(
        asAsync
          ? await ToStringAsync(doc, NewLineHandling.Replace, "\r\n").ConfigureAwait(false)
          : ToString(doc, NewLineHandling.Replace, "\r\n"),
        Is.EqualTo("<div>\r\n-\r\n-\r\n</div>")
      );
    }

    [Test]
    public async Task TestWriteIndent([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XElement(
            "p",
            "text"
          ),
          new XElement(
            "p",
            "text"
          ),
          new XElement(
            "p",
            "text"
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<div>\n <p>text</p>\n <p>text</p>\n <p>text</p>\n</div>")
      );
    }

    [Test]
    public async Task TestIndent_NoIndent([Values(true, false)] bool asAsync)
    {
      var settings = new XmlWriterSettings();

      settings.Async = asAsync;
      settings.Indent = false;
      settings.IndentChars = "\t";
      settings.NewLineChars = "\r\n";
      settings.NewLineOnAttributes = true;
      settings.NewLineHandling = NewLineHandling.Replace;
      settings.OmitXmlDeclaration = true;

      var doc = new XDocument(
        new XElement(
          "div",
          new XElement(
            "p",
            "text"
          ),
          new XElement(
            "p",
            "text"
          ),
          new XElement(
            "p",
            new XAttribute("translate", "no"),
            "text"
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc, settings).ConfigureAwait(false) : ToString(doc, settings),
        Is.EqualTo("<div><p>text</p><p>text</p><p translate=\"no\">text</p></div>")
      );
    }

    [Test]
    public async Task TestWriteIndent_XmlSpace_Default([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XElement(
            "p",
            new XAttribute(XNamespace.Xml + "space", "default"),
            new XElement(
              "span",
              "text"
            )
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<div>\n <p xml:space=\"default\">\n  <span>text</span>\n </p>\n</div>")
      );
    }

    [Test]
    public async Task TestWriteIndent_XmlSpace_Preserve([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XElement(
            "p",
            new XAttribute(XNamespace.Xml + "space", "preserve"),
            new XElement(
              "span",
              "text"
            )
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<div>\n <p xml:space=\"preserve\"><span>text</span></p>\n</div>")
      );
    }

    [Test]
    public async Task TestWriteIndent_AvoidIndentingEmptyElement_XDocument([Values(true, false)] bool asAsync)
    {
      var content = "<body><p></p></body>";
      var doc = XDocument.Load(new StringReader(content));

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<body>\n <p></p>\n</body>")
      );
    }

    [Test]
    public void TestWriteIndent_AvoidIndentingEmptyElement_XmlDocument()
    {
      var content = "<body><p></p></body>";
      var doc = new XmlDocument() {
        XmlResolver = null!
      };
      var settings = new XmlReaderSettings() {
        DtdProcessing = DtdProcessing.Ignore,
        XmlResolver = null!,
      };

      doc.Load(XmlReader.Create(new StringReader(content), settings));

      Assert.That(ToString(doc), Is.EqualTo("<body>\n <p></p>\n</body>"));
    }

    [Test]
    public void TestWriteIndent_AttributeNode_NewLineOnAttributes([Values(true, false)] bool asAsync)
    {
      var nsXhtml = (XNamespace)"http://www.w3.org/1999/xhtml";

      var doc = new XDocument(
        new XElement(
          nsXhtml + "html",
          new XAttribute((XName)"xmlns", nsXhtml.NamespaceName),
          new XAttribute(XNamespace.Xml + "lang", "ja"),
          new XAttribute("lang", "ja"),
          new XElement(nsXhtml + "head")
        )
      );

      var settings = new XmlWriterSettings();

      settings.Async = asAsync;
      settings.Indent = true;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = true;

      Assert.ThrowsAsync<NotSupportedException>(async () => {
        Assert.That(
          asAsync ? await ToStringAsync(doc, settings).ConfigureAwait(false) : ToString(doc, settings),
          Is.EqualTo("<html\n xmlns=\"http://www.w3.org/1999/xhtml\"\n xml:lang=\"ja\"\n lang=\"ja\">\n <head></head></html>")
        );
      });
    }

    [Test]
    public async Task TestWriteIndent_AttributeNode_NoIndent([Values(true, false)] bool asAsync)
    {
      var nsXhtml = (XNamespace)"http://www.w3.org/1999/xhtml";

      var doc = new XDocument(
        new XElement(
          nsXhtml + "html",
          new XAttribute((XName)"xmlns", nsXhtml.NamespaceName),
          new XAttribute(XNamespace.Xml + "lang", "ja"),
          new XAttribute("lang", "ja"),
          new XElement(nsXhtml + "head")
        )
      );

      var settings = new XmlWriterSettings();

      settings.Async = asAsync;
      settings.Indent = false;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = true;

      Assert.That(
        asAsync ? await ToStringAsync(doc, settings).ConfigureAwait(false) : ToString(doc, settings),
        Is.EqualTo("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ja\" lang=\"ja\"><head></head></html>")
      );
    }

    [Test]
    public async Task TestWriteIndent_AttributeNode_NoNewLineOnAttributes([Values(true, false)] bool asAsync)
    {
      var nsXhtml = (XNamespace)"http://www.w3.org/1999/xhtml";

      var doc = new XDocument(
        new XElement(
          nsXhtml + "html",
          new XAttribute((XName)"xmlns", nsXhtml.NamespaceName),
          new XAttribute(XNamespace.Xml + "lang", "ja"),
          new XAttribute("lang", "ja"),
          new XElement(nsXhtml + "head")
        )
      );

      var settings = new XmlWriterSettings();

      settings.Async = asAsync;
      settings.Indent = true;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = false;

      Assert.That(
        asAsync ? await ToStringAsync(doc, settings).ConfigureAwait(false) : ToString(doc, settings),
        Is.EqualTo("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ja\" lang=\"ja\">\n <head></head>\n</html>")
      );
    }

    [Test]
    public async Task TestWriteIndent_CommentNode([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XComment("line1"),
        new XElement(
          "div",
          new XComment("line2"),
          new XComment("line3"),
          new XElement(
            "div",
            new XComment("line4")
          ),
          new XComment("line5")
        ),
        new XComment("line6")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<!--line1-->\n<div>\n <!--line2-->\n <!--line3-->\n <div>\n  <!--line4-->\n </div>\n <!--line5-->\n</div>\n<!--line6-->")
      );
    }

    [Test]
    public async Task TestWriteIndent_CommentNode_AfterElementWithAttribute([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XComment("line1"),
        new XElement(
          "div",
          new XAttribute("id", "body"),
          new XComment("line2")
        ),
        new XComment("line3")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<!--line1-->\n<div id=\"body\">\n <!--line2-->\n</div>\n<!--line3-->")
      );
    }

    [Test]
    public async Task TestWriteIndent_CommentNode_MixedContent([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XComment("line1"),
        new XElement(
          "div",
          new XText("text"),
          new XComment("line2")
        ),
        new XComment("line3")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<!--line1-->\n<div>text<!--line2--></div>\n<!--line3-->")
      );
    }

    [Test]
    public async Task TestWriteIndent_CommentNode_XmlSpace_Preserve([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XComment("line1"),
        new XElement(
          "div",
          new XAttribute(XNamespace.Xml + "space", "preserve"),
          new XComment("line2")
        ),
        new XComment("line3")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<!--line1-->\n<div xml:space=\"preserve\"><!--line2--></div>\n<!--line3-->")
      );
    }

    [Test]
    public async Task TestWriteNonIndentingElements_MixedContent_TextOnly([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "pre",
            " text "
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <pre> text </pre>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteNonIndentingElements_MixedContent1([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "pre",
            "text ",
            new XElement("span", "span"),
            " text"
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <pre>text <span>span</span> text</pre>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteNonIndentingElements_MixedContent2([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "pre",
            "text",
            new XElement("span", "span")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <pre>text<span>span</span></pre>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteNonIndentingElements_NonMixedContent([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "pre",
            new XElement("span", "span")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <pre><span>span</span></pre>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteNonIndentingElements_NonMixedContent_WithXhtmlNamespace([Values(true, false)] bool asAsync)
    {
      var ns = (XNamespace)W3CNamespaces.Xhtml;

      var doc = new XDocument(
        new XElement(
          ns + "p",
          new XElement(
            ns + "pre",
            new XElement(ns + "span", "span")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <pre><span>span</span></pre>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_NonMixedContent([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "p",
            new XElement("span", "span")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <p>\n  <span>span</span>\n </p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_NonMixedContent_WithXhtmlNamespace([Values(true, false)] bool asAsync)
    {
      var ns = (XNamespace)W3CNamespaces.Xhtml;

      var doc = new XDocument(
        new XElement(
          ns + "p",
          new XElement(
            ns + "p",
            new XElement(ns + "span", "span")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <p>\n  <span>span</span>\n </p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_EmptyElement_WithNoAttribute([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "head",
          new XElement(
            "script"
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<head>\n <script></script>\n</head>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_EmptyElement_WithAttribute([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "head",
          new XElement(
            "script",
            new XAttribute("async", "async")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<head>\n <script async=\"async\"></script>\n</head>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_MixedContent_TextOnly([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "p",
            "text"
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <p>text</p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_MixedContent1([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "p",
            new XText("text "),
            new XElement("span", "span"),
            new XText(" text")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <p>text <span>span</span> text</p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_MixedContent2([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "p",
            new XText("text"),
            new XElement("span", "span")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <p>text<span>span</span></p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_MixedContent3([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "p",
            new XElement("span", "span"),
            new XText("text")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <p>\n  <span>span</span>text</p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndentingElements_MixedContent4([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "ul",
          new XElement(
            "li",
            new XText("1"),
            new XElement(
              "ul",
              new XElement(
                "li",
                new XText("2")
              )
            )
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<ul>\n <li>1<ul><li>2</li></ul></li>\n</ul>")
      );
    }

    private static System.Collections.IEnumerable YieldTestCases_TestWriteVoidElements()
    {
      foreach (var asAsync in new[] { false, true }) {
        foreach (var voidElement in VoldElementNames) {
          yield return new object?[] { asAsync, voidElement };
        }
      }
    }

    [TestCaseSource(nameof(YieldTestCases_TestWriteVoidElements))]
    public async Task TestWriteVoidElements(bool asAsync, string voidElement)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            voidElement
          ),
          new XElement(
            voidElement
          ),
          new XElement(
            voidElement
          )
        )
      );

      var e = voidElement;

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo($"<p>\n <{e} />\n <{e} />\n <{e} />\n</p>")
      );
    }

    private static System.Collections.IEnumerable YieldTestCases_TestWriteVoidElements_WithXhtmlNamespace()
    {
      foreach (var asAsync in new[] { false, true }) {
        foreach (var voidElement in VoldElementNames) {
          yield return new object?[] { asAsync, voidElement };
        }
      }
    }

    [TestCaseSource(nameof(YieldTestCases_TestWriteVoidElements_WithXhtmlNamespace))]
    public async Task TestWriteVoidElements_WithXhtmlNamespace(bool asAsync, string voidElement)
    {
      var ns = (XNamespace)W3CNamespaces.Xhtml;

      var doc = new XDocument(
        new XElement(
          ns + "p",
          new XElement(
            ns + voidElement
          ),
          new XElement(
            ns + voidElement
          ),
          new XElement(
            ns + voidElement
          )
        )
      );

      var e = voidElement;

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo($"<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <{e} />\n <{e} />\n <{e} />\n</p>")
      );
    }

    private static System.Collections.IEnumerable YieldTestCases_TestWriteVoidElements_WithAttribute()
    {
      foreach (var asAsync in new[] { false, true }) {
        foreach (var voidElement in VoldElementNames) {
          yield return new object?[] { asAsync, voidElement };
        }
      }
    }

    [TestCaseSource(nameof(YieldTestCases_TestWriteVoidElements_WithAttribute))]
    public async Task TestWriteVoidElements_WithAttribute(bool asAsync, string voidElement)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            voidElement,
            new XAttribute("id", "v1")
          ),
          new XElement(
            voidElement
          ),
          new XElement(
            voidElement,
            new XAttribute("id", "v3")
          ),
          new XElement(
            voidElement
          )
        )
      );

      var e = voidElement;

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo($"<p>\n <{e} id=\"v1\" />\n <{e} />\n <{e} id=\"v3\" />\n <{e} />\n</p>")
      );
    }

    private static System.Collections.IEnumerable YieldTestCases_TestWriteSelfClosingElements()
    {
      foreach (var asAsync in new[] { false, true }) {
        foreach (var selfClosingElement in SelfClosingElementNames) {
          yield return new object?[] { asAsync, selfClosingElement };
        }
      }
    }

    [TestCaseSource(nameof(YieldTestCases_TestWriteSelfClosingElements))]
    public async Task TestWriteSelfClosingElements(bool asAsync, string selfClosingElement)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            selfClosingElement
          ),
          new XElement(
            selfClosingElement
          ),
          new XElement(
            selfClosingElement
          )
        )
      );

      var e = selfClosingElement;

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo($"<p>\n <{e}></{e}>\n <{e}></{e}>\n <{e}></{e}>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndent_CDataNode([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XElement(
          "p",
          new XElement(
            "p",
            new XElement("span", "span"),
            new XCData("cdata")
          )
        )
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<p>\n <p>\n  <span>span</span><![CDATA[cdata]]></p>\n</p>")
      );
    }

    [Test]
    public async Task TestWriteIndent_EntityReferenceNode([Values(true, false)] bool asAsync)
    {
      var fragment = new XElement(
        "p",
        new XElement(
          "p",
          new XElement("span", "span"),
          new XEntityReference("copy")
        )
      );

      var settings = new XmlWriterSettings() {
        Async = asAsync,
        ConformanceLevel = ConformanceLevel.Fragment,
        NewLineChars = "\n",
        Indent = true,
        IndentChars = " ",
      };
      var sb = new StringBuilder();

      if (asAsync) {
#if SYSTEM_XML_LINQ_XNODE_WRITETOASYNC
        await using var writer = new PolyglotHtml5Writer(new StringWriter(sb), settings);

        await fragment.WriteToAsync(writer, default).ConfigureAwait(false);
#else
        Assert.Ignore("WriteToAsync is not supported on this framework");

        await Task.FromResult(0);
#endif
      }
      else {
        using var writer = new PolyglotHtml5Writer(new StringWriter(sb), settings);

        fragment.WriteTo(writer);
      }

      Assert.That(sb.ToString(), Is.EqualTo("<p>\n <p>\n  <span>span</span>&copy;</p>\n</p>"));
    }

    [Test]
    public async Task TestWrite_ProcessingInstruction([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"style.xsl\""),
        new XDocumentType("html", string.Empty, string.Empty, string.Empty),
        new XElement("html")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("<?xml-stylesheet type=\"text/xsl\" href=\"style.xsl\"?><!DOCTYPE html>\n<html></html>")
      );
    }

    [Test]
    public async Task TestWrite_Whitespace([Values(true, false)] bool asAsync)
    {
      var doc = new XDocument(
        new XText("\n"),
        new XDocumentType("html", null!, null!, null!),
        new XText(" "),
        new XElement("html")
      );

      Assert.That(
        asAsync ? await ToStringAsync(doc).ConfigureAwait(false) : ToString(doc),
        Is.EqualTo("\n<!DOCTYPE html> \n<html></html>")
      );
    }

    [Test]
    public void TestWriteCharEntity([Values(true, false)] bool asAsync)
    {
      var writer = new PolyglotHtml5Writer(
        new StringWriter(new StringBuilder()),
        new XmlWriterSettings() {
          Async = asAsync,
          ConformanceLevel = ConformanceLevel.Fragment,
        }
      );

      if (asAsync)
        Assert.DoesNotThrowAsync(async () => await writer.WriteCharEntityAsync('\uD23E').ConfigureAwait(false));
      else
        Assert.DoesNotThrow(() => writer.WriteCharEntity('\uD23E'));
    }

    [Test]
    public void TestWriteSurrogateCharEntity([Values(true, false)] bool asAsync)
    {
      var writer = new PolyglotHtml5Writer(
        new StringWriter(new StringBuilder()),
        new XmlWriterSettings() {
          Async = asAsync,
          ConformanceLevel = ConformanceLevel.Fragment,
        }
      );

      if (asAsync)
        Assert.DoesNotThrowAsync(async () => await writer.WriteSurrogateCharEntityAsync('\uDF41', '\uD920').ConfigureAwait(false));
      else
        Assert.DoesNotThrow(() => writer.WriteSurrogateCharEntity('\uDF41', '\uD920'));
    }

    [Test]
    public void TestWriteBase64([Values(true, false)] bool asAsync)
    {
      var writer = new PolyglotHtml5Writer(
        new StringWriter(new StringBuilder()),
        new XmlWriterSettings() {
          Async = asAsync,
          ConformanceLevel = ConformanceLevel.Fragment,
        }
      );

      var buffer = new byte[1];

      if (asAsync)
        Assert.DoesNotThrowAsync(async () => await writer.WriteBase64Async(buffer, 0, buffer.Length).ConfigureAwait(false));
      else
        Assert.DoesNotThrow(() => writer.WriteBase64(buffer, 0, buffer.Length));
    }

    [Test]
    public void TestWriteChars([Values(true, false)] bool asAsync)
    {
      var writer = new PolyglotHtml5Writer(
        new StringWriter(new StringBuilder()),
        new XmlWriterSettings() {
          Async = asAsync,
          ConformanceLevel = ConformanceLevel.Fragment,
        }
      );

      var buffer = "chars".ToCharArray();

      if (asAsync)
        Assert.DoesNotThrowAsync(async () => await writer.WriteCharsAsync(buffer, 0, buffer.Length).ConfigureAwait(false));
      else
        Assert.DoesNotThrow(() => writer.WriteChars(buffer, 0, buffer.Length));
    }
  }
}
