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
    public void TestEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("text", "html")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("image", "plain")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("application", "octet-stream")).Equals(new MimeType("text", "plain")));
    }

    [Test]
    public void TestTypeEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain").TypeEquals(new MimeType("text", "plain"))));
      Assert.IsTrue((new MimeType("text", "plain").TypeEquals(new MimeType("text", "html"))));
      Assert.IsFalse((new MimeType("text", "plain").TypeEquals(new MimeType("image", "x-icon"))));
    }

    [Test]
    public void TestSubTypeEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "plain"))));
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEquals(new MimeType("image", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "html"))));
    }

    [Test]
    public void TestGetMimeTypeByExtension()
    {
      Assert.AreEqual(MimeType.TextPlain, MimeType.GetMimeTypeByExtension("hoge.txt"));
      Assert.AreEqual(MimeType.CreateTextType("html"), MimeType.GetMimeTypeByExtension("index.html"));
      Assert.AreEqual(MimeType.CreateImageType("png"), MimeType.GetMimeTypeByExtension("image.png"));
      Assert.AreEqual(null, MimeType.GetMimeTypeByExtension(".hoge"));
    }
  }
}