// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  [TestCase("text/plain", "text", "plain")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars '/' 63chars
  public void Constructor_String(string mimeType, string expectedType, string expectedSubType)
  {
    var mime = new MimeType(mimeType);

    Assert.AreEqual(expectedType, mime.Type);
    Assert.AreEqual(expectedSubType, mime.SubType);
    Assert.AreEqual(mimeType, mime.ToString());
  }

  [TestCase("text", "plain")]
  [TestCase("message", "rfc822")]
  [TestCase("application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 64chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/64chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 64chars/64chars
  public void Constructor_String_String(string type, string subtype)
  {
    var mime = new MimeType(type, subtype);

    Assert.AreEqual(type, mime.Type);
    Assert.AreEqual(subtype, mime.SubType);
    Assert.AreEqual($"{type}/{subtype}", mime.ToString());
  }

  [TestCase("text", "plain")]
  [TestCase("message", "rfc822")]
  [TestCase("application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 64chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/64chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 64chars/64chars
  public void Constructor_ValueTuple2(string type, string subtype)
  {
    var m = (type, subtype);
    var mime = new MimeType(m);

    Assert.AreEqual(type, mime.Type);
    Assert.AreEqual(subtype, mime.SubType);
    Assert.AreEqual($"{type}/{subtype}", mime.ToString());
  }

  [TestCase(null, typeof(ArgumentNullException))]
  [TestCase("", typeof(ArgumentException))]
  [TestCase("text", typeof(ArgumentException))]
  [TestCase("text/", typeof(ArgumentException))]
  [TestCase("/", typeof(ArgumentException))]
  [TestCase("/plain", typeof(ArgumentException))]
  [TestCase("text/plain/", typeof(ArgumentException))]
  [TestCase("text/plain/foo", typeof(ArgumentException))]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", typeof(ArgumentException))] // 128chars (64chars '/' 63chars)
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", typeof(ArgumentException))] // 128chars (63chars '/' 64chars)
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", typeof(ArgumentException))] // 129chars (64chars '/' 64chars)
  public void Constructor_String_ArgumentException(string mimeType, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => new MimeType(mimeType));

  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("text", null, typeof(ArgumentNullException))]
  [TestCase(null, "plain", typeof(ArgumentNullException))]
  [TestCase("", "", typeof(ArgumentException))]
  [TestCase("text", "", typeof(ArgumentException))]
  [TestCase("", "plain", typeof(ArgumentException))]
  public void Constructor_String_String_ArgumentException(string type, string subtype, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => new MimeType(type, subtype));

  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("text", null, typeof(ArgumentNullException))]
  [TestCase(null, "plain", typeof(ArgumentNullException))]
  [TestCase("", "", typeof(ArgumentException))]
  [TestCase("text", "", typeof(ArgumentException))]
  [TestCase("", "plain", typeof(ArgumentException))]
  public void Constructor_ValueTuple2_ArgumentException(string type, string subtype, Type expectedExceptionType)
  {
    var mimeType = (type, subtype);

    Assert.Throws(expectedExceptionType, () => new MimeType(mimeType));
  }

  [Test]
  public void Deconstruct()
  {
    var (type, subType) = MimeType.TextPlain;

    Assert.AreEqual("text", type);
    Assert.AreEqual("plain", subType);
  }

  [Test]
  public void Test_ToString()
  {
    Assert.AreEqual("text/plain", MimeType.TextPlain.ToString());
    Assert.AreEqual("application/octet-stream", MimeType.ApplicationOctetStream.ToString());
    Assert.AreEqual("text/html", MimeType.CreateTextType("html").ToString());
  }

  [Test]
  public void ExplicitToStringCoversion()
  {
    Assert.AreEqual("text/plain", (string)MimeType.TextPlain);
    Assert.AreEqual("application/octet-stream", (string)MimeType.ApplicationOctetStream);
    Assert.AreEqual("text/html", (string)MimeType.CreateTextType("html"));

    Assert.IsNull((string)((MimeType)null));
  }
}
