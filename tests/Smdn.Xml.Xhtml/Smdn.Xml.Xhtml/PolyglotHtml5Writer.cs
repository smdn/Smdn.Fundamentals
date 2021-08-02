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
          Assert.IsNotNull(writer.Settings);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(sb, null)) {
          Assert.IsNotNull(writer.Settings);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(Stream.Null)) {
          Assert.IsNotNull(writer.Settings);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(Stream.Null, null)) {
          Assert.IsNotNull(writer.Settings);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(TextWriter.Null)) {
          Assert.IsNotNull(writer.Settings);

          doc.Save(writer);
        }
      });

      Assert.DoesNotThrow(() => {
        var sb = new StringBuilder();

        using (var writer = new PolyglotHtml5Writer(TextWriter.Null, null)) {
          Assert.IsNotNull(writer.Settings);

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
        new XDocumentType("html", null, null, null),
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

      Assert.AreEqual(expected.Replace("\r\n", "\n"), ToString(doc));
    }

    [Test]
    public void TestWriteProlog_AlwaysOmitXmlDeclaration()
    {
      var doc = new XDocument(new XElement("html"));

      var settings = new XmlWriterSettings();

      settings.Indent = false;
      settings.OmitXmlDeclaration = false;

      Assert.AreEqual("<html></html>",
                      ToString(doc, settings));

      settings.OmitXmlDeclaration = true;

      Assert.AreEqual("<html></html>",
                      ToString(doc, settings));
    }

    [Test]
    public void TestWriteDocType_XDocument_NullId()
    {
      var doc = new XDocument(
        new XDocumentType("html", null, null, null),
        new XElement("html")
      );

      Assert.AreEqual("<!DOCTYPE html>\n<html></html>",
                      ToString(doc));
    }

    [Test]
    public void TestWriteDocType_XDocument_EmptyId()
    {
      var doc = new XDocument(
        new XDocumentType("html", string.Empty, string.Empty, string.Empty),
        new XElement("html")
      );

      Assert.AreEqual("<!DOCTYPE html>\n<html></html>",
                      ToString(doc));
    }

#if NETFRAMEWORK || NETSTANDARD2_0
    [Test]
    public void TestWriteDocType_XmlDocument()
    {
      var doc = new XmlDocument();

      doc.AppendChild(doc.CreateDocumentType("html", null, null, null));
      doc.AppendChild(doc.CreateElement("html"));

      Assert.AreEqual("<!DOCTYPE html>\n<html></html>",
                      ToString(doc));
    }
#endif

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

      Assert.AreEqual("<meta name=\"description\" content=\"&lt;&gt;&amp;&quot;\'\" />",
                      ToString(doc, newLineHandling));
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

      Assert.AreEqual("<meta name=\"description\" content=\"\r\n\t\" />",
                      ToString(doc, NewLineHandling.None));
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

      Assert.AreEqual("<meta name=\"description\" content=\"&#xD;&#xA;&#x9;\" />",
                      ToString(doc, newLineHandling));
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

      Assert.AreEqual("<div>&lt;&gt;&amp;\"\'</div>",
                      ToString(doc, newLineHandling));
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

      Assert.AreEqual("<div>\r\n\t</div>",
                      ToString(doc, NewLineHandling.None));
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

      Assert.AreEqual("<div>&#xD;\n</div>",
                      ToString(doc, NewLineHandling.Entitize));
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

      Assert.AreEqual("<div>\n-\n-\n</div>",
                      ToString(doc, NewLineHandling.Replace, "\n"));
      Assert.AreEqual("<div>\r\n-\r\n-\r\n</div>",
                      ToString(doc, NewLineHandling.Replace, "\r\n"));
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

      Assert.AreEqual("<div>\n <p>text</p>\n <p>text</p>\n <p>text</p>\n</div>",
                      ToString(doc));
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

      Assert.AreEqual("<div><p>text</p><p>text</p><p translate=\"no\">text</p></div>",
                      ToString(doc, settings));
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

      Assert.AreEqual("<div>\n <p xml:space=\"default\">\n  <span>text</span>\n </p>\n</div>",
                      ToString(doc));
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

      Assert.AreEqual("<div>\n <p xml:space=\"preserve\"><span>text</span></p>\n</div>",
                      ToString(doc));
    }

    [Test]
    public void TestWriteIndent_AvoidIndentingEmptyElement_XDocument()
    {
      var content = "<body><p></p></body>";
      var doc = XDocument.Load(new StringReader(content));

      Assert.AreEqual("<body>\n <p></p>\n</body>",
                      ToString(doc));
    }

    [Test]
    public void TestWriteIndent_AvoidIndentingEmptyElement_XmlDocument()
    {
      var content = "<body><p></p></body>";
      var doc = new XmlDocument();

      doc.Load(new StringReader(content));

      Assert.AreEqual("<body>\n <p></p>\n</body>",
                      ToString(doc));
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
        Assert.AreEqual("<html\n xmlns=\"http://www.w3.org/1999/xhtml\"\n xml:lang=\"ja\"\n lang=\"ja\">\n <head></head></html>",
                        ToString(doc, settings));

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

      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ja\" lang=\"ja\"><head></head></html>",
                      ToString(doc, settings));
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

      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ja\" lang=\"ja\">\n <head></head>\n</html>",
                      ToString(doc, settings));
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

      Assert.AreEqual("<!--line1-->\n<div>\n <!--line2-->\n <!--line3-->\n <div>\n  <!--line4-->\n </div>\n <!--line5-->\n</div>\n<!--line6-->",
                      ToString(doc));
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

      Assert.AreEqual("<!--line1-->\n<div id=\"body\">\n <!--line2-->\n</div>\n<!--line3-->",
                      ToString(doc));
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

      Assert.AreEqual("<!--line1-->\n<div>text<!--line2--></div>\n<!--line3-->",
                      ToString(doc));
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

      Assert.AreEqual("<!--line1-->\n<div xml:space=\"preserve\"><!--line2--></div>\n<!--line3-->",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <pre> text </pre>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <pre>text <span>span</span> text</pre>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <pre>text<span>span</span></pre>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <pre><span>span</span></pre>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <pre><span>span</span></pre>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <p>\n  <span>span</span>\n </p>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <p>\n  <span>span</span>\n </p>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<head>\n <script></script>\n</head>",
                      ToString(doc));
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

      Assert.AreEqual("<head>\n <script async=\"async\"></script>\n</head>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <p>text</p>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <p>text <span>span</span> text</p>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <p>text<span>span</span></p>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<p>\n <p>\n  <span>span</span>text</p>\n</p>",
                      ToString(doc));
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

      Assert.AreEqual("<ul>\n <li>1<ul><li>2</li></ul></li>\n</ul>",
                      ToString(doc));
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

      Assert.AreEqual(string.Format("<p>\n <{0} />\n <{0} />\n <{0} />\n</p>", voidElement),
                      ToString(doc));
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

      Assert.AreEqual(string.Format("<p xmlns=\"http://www.w3.org/1999/xhtml\">\n <{0} />\n <{0} />\n <{0} />\n</p>", voidElement),
                      ToString(doc));
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

      Assert.AreEqual(string.Format("<p>\n <{0} id=\"v1\" />\n <{0} />\n <{0} id=\"v3\" />\n <{0} />\n</p>", voidElement),
                      ToString(doc));
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

      Assert.AreEqual(string.Format("<p>\n <{0}></{0}>\n <{0}></{0}>\n <{0}></{0}>\n</p>", selfClosingElement),
                      ToString(doc));
    }
  }
}