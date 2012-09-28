using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class MimeTypeTests {
    [Test]
    public void TestConstructor()
    {
      var mime = new MimeType("text", "plain");

      Assert.AreEqual("text", mime.Type);
      Assert.AreEqual("plain", mime.SubType);
    }

    [Test]
    public void TestConstructorInvalidArgument()
    {
      Assert.Throws<ArgumentNullException>(() => new MimeType((string)null), "#1");
      Assert.Throws<ArgumentException>(() => new MimeType(string.Empty), "#2");
      Assert.Throws<ArgumentException>(() => new MimeType("text"), "#3");
      Assert.Throws<ArgumentException>(() => new MimeType("text/"), "#4");
      Assert.Throws<ArgumentException>(() => new MimeType("/plain"), "#5");
      Assert.Throws<ArgumentException>(() => new MimeType("text/plain/hoge"), "#6");
    }

    [Test]
    public void TestEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("text", "plain")).Equals(new MimeType("text", "PLAIN")));
      Assert.IsFalse((new MimeType("text", "plain")).Equals(new MimeType("TEXT", "plain")));
      Assert.IsFalse((new MimeType("text", "plain")).Equals(new MimeType("TEXT", "PLAIN")));

      Assert.IsFalse((new MimeType("text", "html")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("image", "plain")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("application", "octet-stream")).Equals(new MimeType("text", "plain")));
    }

    [Test]
    public void TestTypeEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain").TypeEquals(new MimeType("text", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").TypeEquals(new MimeType("TEXT", "plain"))));

      Assert.IsTrue((new MimeType("text", "plain").TypeEquals(new MimeType("text", "html"))));
      Assert.IsFalse((new MimeType("text", "plain").TypeEquals(new MimeType("image", "x-icon"))));
    }

    [Test]
    public void TestSubTypeEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "PLAIN"))));

      Assert.IsTrue((new MimeType("text", "plain").SubTypeEquals(new MimeType("image", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "html"))));
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("text", "plain")));
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("text", "PLAIN")));
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("TEXT", "plain")));
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("TEXT", "PLAIN")));
    }

    [Test]
    public void TestTypeEqualsIgnoreCase()
    {
      Assert.IsTrue((new MimeType("text", "plain").TypeEqualsIgnoreCase(new MimeType("text", "plain"))));
      Assert.IsTrue((new MimeType("text", "plain").TypeEqualsIgnoreCase(new MimeType("TEXT", "plain"))));
    }

    [Test]
    public void TestSubTypeEqualsIgnoreCase()
    {
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEqualsIgnoreCase(new MimeType("text", "plain"))));
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEqualsIgnoreCase(new MimeType("text", "PLAIN"))));
    }

    [Test]
    public void TestGetMimeTypeByExtension()
    {
      Assert.AreEqual(MimeType.TextPlain, MimeType.GetMimeTypeByExtension("hoge.txt"));
      Assert.AreEqual(MimeType.CreateTextType("html"), MimeType.GetMimeTypeByExtension("index.html"));
      Assert.AreEqual(MimeType.CreateImageType("png"), MimeType.GetMimeTypeByExtension("image.png"));
      Assert.AreEqual(null, MimeType.GetMimeTypeByExtension(".hoge"));
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("text/plain", MimeType.TextPlain.ToString());
      Assert.AreEqual("application/octet-stream", MimeType.ApplicationOctetStream.ToString());
      Assert.AreEqual("text/html", MimeType.CreateTextType("html").ToString());
    }

    [Test]
    public void TestExplicitToStringCoversion()
    {
      Assert.AreEqual("text/plain", (string)MimeType.TextPlain);
      Assert.AreEqual("application/octet-stream", (string)MimeType.ApplicationOctetStream);
      Assert.AreEqual("text/html", (string)MimeType.CreateTextType("html"));

      Assert.IsNull((string)((MimeType)null));
    }

    [Test]
    public void TestTryParse()
    {
      MimeType result;

      Assert.IsFalse(MimeType.TryParse(null, out result), "#1");
      Assert.IsNull(result, "#1");

      Assert.IsFalse(MimeType.TryParse(string.Empty, out result), "#2");
      Assert.IsNull(result, "#2");

      Assert.IsFalse(MimeType.TryParse("text", out result), "#3");
      Assert.IsNull(result, "#3");

      Assert.IsFalse(MimeType.TryParse("text/", out result), "#4");
      Assert.IsNull(result, "#4");

      Assert.IsFalse(MimeType.TryParse("/plain", out result), "#5");
      Assert.IsNull(result, "#5");

      Assert.IsFalse(MimeType.TryParse("text/plain/hoge", out result), "#6");
      Assert.IsNull(result, "#6");

      Assert.IsTrue(MimeType.TryParse("text/plain", out result), "#7");
      Assert.AreEqual(MimeType.TextPlain, result, "#7");
    }
  }
}