// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace Smdn.Xml.Xhtml {
  [TestFixture]
  public class PolyglotHtml5WriterTests {
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

    private static string ToString(XDocument doc, NewLineHandling newLineHandling, string newLineChars = null)
    {
      var settings = new XmlWriterSettings();

      settings.NewLineHandling = newLineHandling;
      settings.NewLineChars = newLineChars ?? "\n";

      return ToString(doc, settings);
    }

    private static string ToString(XDocument doc, XmlWriterSettings settings = null)
    {
      if (settings == null) {
        settings = new XmlWriterSettings();
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
    public void TestWrite_Html5Document()
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

      Assert.That(ToString(doc), Is.EqualTo(expected.Replace("\r\n", "\n")));
    }

    [Test]
    public void TestWriteProlog_AlwaysOmitXmlDeclaration()
    {
      var doc = new XDocument(new XElement("html"));

      var settings = new XmlWriterSettings();

      settings.Indent = false;
      settings.OmitXmlDeclaration = false;

      Assert.That(ToString(doc, settings), Is.EqualTo("<html></html>"));

      settings.OmitXmlDeclaration = true;

      Assert.That(ToString(doc, settings), Is.EqualTo("<html></html>"));
    }

    [Test]
    public void TestWriteDocType_XDocument_NullId()
    {
      var doc = new XDocument(
        new XDocumentType("html", null!, null!, null!),
        new XElement("html")
      );

      Assert.That(ToString(doc), Is.EqualTo("<!DOCTYPE html>\n<html></html>"));
    }

    [Test]
    public void TestWriteDocType_XDocument_EmptyId()
    {
      var doc = new XDocument(
        new XDocumentType("html", string.Empty, string.Empty, string.Empty),
        new XElement("html")
      );

      Assert.That(ToString(doc), Is.EqualTo("<!DOCTYPE html>\n<html></html>"));
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

    [TestCase(NewLineHandling.None)]
    [TestCase(NewLineHandling.Entitize)]
    [TestCase(NewLineHandling.Replace)]
    public void TestWriteAttribute_ValueEscaped(NewLineHandling newLineHandling)
    {
      var doc = new XDocument(
        new XElement(
          "meta",
          new XAttribute("name", "description"),
          new XAttribute("content", "<>&\"\'")
        )
      );

      Assert.That(ToString(doc, newLineHandling), Is.EqualTo("<meta name=\"description\" content=\"&lt;&gt;&amp;&quot;\'\" />"));
    }

    [Test]
    public void TestWriteAttribute_NewLineHandling_None()
    {
      var doc = new XDocument(
        new XElement(
          "meta",
          new XAttribute("name", "description"),
          new XAttribute("content", "\r\n\t")
        )
      );

      Assert.That(ToString(doc, NewLineHandling.None), Is.EqualTo("<meta name=\"description\" content=\"\r\n\t\" />"));
    }

    [TestCase(NewLineHandling.Entitize)]
    [TestCase(NewLineHandling.Replace)]
    public void TestWriteAttribute_NewLineHandling_EntitizeAndReplace(NewLineHandling newLineHandling)
    {
      var doc = new XDocument(
        new XElement(
          "meta",
          new XAttribute("name", "description"),
          new XAttribute("content", "\r\n\t")
        )
      );

      Assert.That(ToString(doc, newLineHandling), Is.EqualTo("<meta name=\"description\" content=\"&#xD;&#xA;&#x9;\" />"));
    }

    [TestCase(NewLineHandling.None)]
    [TestCase(NewLineHandling.Entitize)]
    [TestCase(NewLineHandling.Replace)]
    public void TestWriteTextNode_TextEscaped(NewLineHandling newLineHandling)
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("<>&\"\'")
        )
      );

      Assert.That(ToString(doc, newLineHandling), Is.EqualTo("<div>&lt;&gt;&amp;\"\'</div>"));
    }

    [Test]
    public void TestWriteTextNode_NewLineHandling_None()
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("\r\n\t")
        )
      );

      Assert.That(ToString(doc, NewLineHandling.None), Is.EqualTo("<div>\r\n\t</div>"));
    }

    [Test]
    public void TestWriteTextNode_NewLineHandling_Entitize()
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("\r\n")
        )
      );

      Assert.That(ToString(doc, NewLineHandling.Entitize), Is.EqualTo("<div>&#xD;\n</div>"));
    }

    [Test]
    public void TestWriteTextNode_NewLineHandling_Replace()
    {
      var doc = new XDocument(
        new XElement(
          "div",
          new XText("\r\n-\r-\n")
        )
      );

      Assert.That(ToString(doc, NewLineHandling.Replace, "\n"), Is.EqualTo("<div>\n-\n-\n</div>"));
      Assert.That(ToString(doc, NewLineHandling.Replace, "\r\n"), Is.EqualTo("<div>\r\n-\r\n-\r\n</div>"));
    }

    [Test]
    public void TestWriteIndent()
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

      Assert.That(ToString(doc), Is.EqualTo("<div>\n <p>text</p>\n <p>text</p>\n <p>text</p>\n</div>"));
    }

    [Test]
    public void TestIndent_NoIndent()
    {
      var settings = new XmlWriterSettings();

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

      Assert.That(ToString(doc, settings), Is.EqualTo("<div><p>text</p><p>text</p><p translate=\"no\">text</p></div>"));
    }

    [Test]
    public void TestWriteIndent_XmlSpace_Default()
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

      Assert.That(ToString(doc), Is.EqualTo("<div>\n <p xml:space=\"default\">\n  <span>text</span>\n </p>\n</div>"));
    }

    [Test]
    public void TestWriteIndent_XmlSpace_Preserve()
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

      Assert.That(ToString(doc), Is.EqualTo("<div>\n <p xml:space=\"preserve\"><span>text</span></p>\n</div>"));
    }

    [Test]
    public void TestWriteIndent_AvoidIndentingEmptyElement_XDocument()
    {
      var content = "<body><p></p></body>";
      var doc = XDocument.Load(new StringReader(content));

      Assert.That(ToString(doc), Is.EqualTo("<body>\n <p></p>\n</body>"));
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
    public void TestWriteIndent_AttributeNode_NewLineOnAttributes()
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

      settings.Indent = true;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = true;

      Assert.Throws<NotSupportedException>(() => {
        Assert.That(ToString(doc, settings), Is.EqualTo("<html\n xmlns=\"http://www.w3.org/1999/xhtml\"\n xml:lang=\"ja\"\n lang=\"ja\">\n <head></head></html>"));

      });
    }

    [Test]
    public void TestWriteIndent_AttributeNode_NoIndent()
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

      settings.Indent = false;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = true;

      Assert.That(ToString(doc, settings), Is.EqualTo("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ja\" lang=\"ja\"><head></head></html>"));
    }

    [Test]
    public void TestWriteIndent_AttributeNode_NoNewLineOnAttributes()
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

      settings.Indent = true;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = false;

      Assert.That(ToString(doc, settings), Is.EqualTo("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ja\" lang=\"ja\">\n <head></head>\n</html>"));
    }

    [Test]
    public void TestWriteIndent_CommentNode()
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

      Assert.That(ToString(doc), Is.EqualTo("<!--line1-->\n<div>\n <!--line2-->\n <!--line3-->\n <div>\n  <!--line4-->\n </div>\n <!--line5-->\n</div>\n<!--line6-->"));
    }

    [Test]
    public void TestWriteIndent_CommentNode_AfterElementWithAttribute()
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

      Assert.That(ToString(doc), Is.EqualTo("<!--line1-->\n<div id=\"body\">\n <!--line2-->\n</div>\n<!--line3-->"));
    }

    [Test]
    public void TestWriteIndent_CommentNode_MixedContent()
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

      Assert.That(ToString(doc), Is.EqualTo("<!--line1-->\n<div>text<!--line2--></div>\n<!--line3-->"));
    }

    [Test]
    public void TestWriteIndent_CommentNode_XmlSpace_Preserve()
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

      Assert.That(ToString(doc), Is.EqualTo("<!--line1-->\n<div xml:space=\"preserve\"><!--line2--></div>\n<!--line3-->"));
    }

    [Test]
    public void TestWriteNonIndentingElements_MixedContent_TextOnly()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <pre> text </pre>\n</p>"));
    }

    [Test]
    public void TestWriteNonIndentingElements_MixedContent1()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <pre>text <span>span</span> text</pre>\n</p>"));
    }

    [Test]
    public void TestWriteNonIndentingElements_MixedContent2()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <pre>text<span>span</span></pre>\n</p>"));
    }

    [Test]
    public void TestWriteNonIndentingElements_NonMixedContent()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <pre><span>span</span></pre>\n</p>"));
    }

    [Test]
    public void TestWriteNonIndentingElements_NonMixedContent_WithXhtmlNamespace()
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

      Assert.That(ToString(doc), Is.EqualTo("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <pre><span>span</span></pre>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_NonMixedContent()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <p>\n  <span>span</span>\n </p>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_NonMixedContent_WithXhtmlNamespace()
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

      Assert.That(ToString(doc), Is.EqualTo("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <p>\n  <span>span</span>\n </p>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_EmptyElement_WithNoAttribute()
    {
      var doc = new XDocument(
        new XElement(
          "head",
          new XElement(
            "script"
          )
        )
      );

      Assert.That(ToString(doc), Is.EqualTo("<head>\n <script></script>\n</head>"));
    }

    [Test]
    public void TestWriteIndentingElements_EmptyElement_WithAttribute()
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

      Assert.That(ToString(doc), Is.EqualTo("<head>\n <script async=\"async\"></script>\n</head>"));
    }

    [Test]
    public void TestWriteIndentingElements_MixedContent_TextOnly()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <p>text</p>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_MixedContent1()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <p>text <span>span</span> text</p>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_MixedContent2()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <p>text<span>span</span></p>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_MixedContent3()
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

      Assert.That(ToString(doc), Is.EqualTo("<p>\n <p>\n  <span>span</span>text</p>\n</p>"));
    }

    [Test]
    public void TestWriteIndentingElements_MixedContent4()
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

      Assert.That(ToString(doc), Is.EqualTo("<ul>\n <li>1<ul><li>2</li></ul></li>\n</ul>"));
    }

    [TestCase("area")]
    [TestCase("base")]
    [TestCase("br")]
    [TestCase("col")]
    [TestCase("embed")]
    [TestCase("hr")]
    [TestCase("img")]
    [TestCase("input")]
    [TestCase("keygen")]
    [TestCase("link")]
    [TestCase("meta")]
    [TestCase("param")]
    [TestCase("source")]
    [TestCase("track")]
    [TestCase("wbr")]
    public void TestWriteVoidElements(string voidElement)
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
        ToString(doc),
        Is.EqualTo($"<p>\n <{e} />\n <{e} />\n <{e} />\n</p>")
      );
    }

    [TestCase("area")]
    [TestCase("base")]
    [TestCase("br")]
    [TestCase("col")]
    [TestCase("embed")]
    [TestCase("hr")]
    [TestCase("img")]
    [TestCase("input")]
    [TestCase("keygen")]
    [TestCase("link")]
    [TestCase("meta")]
    [TestCase("param")]
    [TestCase("source")]
    [TestCase("track")]
    [TestCase("wbr")]
    public void TestWriteVoidElements_WithXhtmlNamespace(string voidElement)
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
        ToString(doc),
        Is.EqualTo($"<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <{e} />\n <{e} />\n <{e} />\n</p>")
      );
    }

    [TestCase("area")]
    [TestCase("base")]
    [TestCase("br")]
    [TestCase("col")]
    [TestCase("embed")]
    [TestCase("hr")]
    [TestCase("img")]
    [TestCase("input")]
    [TestCase("keygen")]
    [TestCase("link")]
    [TestCase("meta")]
    [TestCase("param")]
    [TestCase("source")]
    [TestCase("track")]
    [TestCase("wbr")]
    public void TestWriteVoidElements_WithAttribute(string voidElement)
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
        ToString(doc),
        Is.EqualTo($"<p>\n <{e} id=\"v1\" />\n <{e} />\n <{e} id=\"v3\" />\n <{e} />\n</p>")
      );
    }

    [TestCase("div")]
    [TestCase("span")]
    [TestCase("script")]
    [TestCase("html")]
    [TestCase("body")]
    [TestCase("head")]
    public void TestWriteSelfClosingElements(string selfClosingElement)
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
        ToString(doc),
        Is.EqualTo($"<p>\n <{e}></{e}>\n <{e}></{e}>\n <{e}></{e}>\n</p>")
      );
    }
  }
}
