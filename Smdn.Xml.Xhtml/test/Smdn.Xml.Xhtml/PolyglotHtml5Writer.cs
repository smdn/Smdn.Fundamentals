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

    private static string ToString(XDocument doc)
    {
      var settings = new XmlWriterSettings();

      settings.ConformanceLevel = ConformanceLevel.Document;
      settings.Indent = true;
      settings.IndentChars = " ";
      settings.NewLineChars = "\n";
      settings.NewLineOnAttributes = false;
      settings.OmitXmlDeclaration = true;

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
      settings.OmitXmlDeclaration = true;

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

      Assert.AreEqual(expected, ToString(doc));
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

    [Test]
    public void TestWriteDocType_XmlDocument()
    {
      var doc = new XmlDocument();

      doc.AppendChild(doc.CreateDocumentType("html", null, null, null));
      doc.AppendChild(doc.CreateElement("html"));

      Assert.AreEqual("<!DOCTYPE html>\n<html></html>",
                      ToString(doc));
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