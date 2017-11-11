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

    [Test]
    public void TestWriteDocType()
    {
      var doc = new XDocument(
        new XDocumentType("html", null, null, null),
        new XElement("html")
      );

      Assert.AreEqual("<!DOCTYPE html>\n<html />",
                      ToString(doc));
    }
  }
}