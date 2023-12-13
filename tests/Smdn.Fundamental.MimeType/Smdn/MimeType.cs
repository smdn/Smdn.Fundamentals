// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  [TestCase(nameof(MimeType.TextPlain), "text", "plain")]
  [TestCase(nameof(MimeType.TextRtf), "text", "rtf")]
  [TestCase(nameof(MimeType.TextHtml), "text", "html")]
  [TestCase(nameof(MimeType.MultipartAlternative), "multipart", "alternative")]
  [TestCase(nameof(MimeType.MultipartDigest), "multipart", "digest")]
  [TestCase(nameof(MimeType.MultipartMixed), "multipart", "mixed")]
  [TestCase(nameof(MimeType.MultipartParallel), "multipart", "parallel")]
  [TestCase(nameof(MimeType.MultipartFormData), "multipart", "form-data")]
  [TestCase(nameof(MimeType.ApplicationOctetStream), "application", "octet-stream")]
  [TestCase(nameof(MimeType.ApplicationXWwwFormUrlEncoded), "application", "x-www-form-urlencoded")]
  [TestCase(nameof(MimeType.MessagePartial), "message", "partial")]
  [TestCase(nameof(MimeType.MessageExternalBody), "message", "external-body")]
  [TestCase(nameof(MimeType.MessageRfc822), "message", "rfc822")]
  public void MimeTypeFields(string fieldName, string expectedMimeType, string expectedMimeSubType)
    => Assert.That(
      typeof(MimeType).GetField(fieldName)?.GetValue(null) as MimeType,
      Is.EqualTo(new MimeType(expectedMimeType, expectedMimeSubType))
    );

  [TestCase("text/plain", "text", "plain")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars '/' 63chars
  public void Constructor_String(string mimeType, string expectedType, string expectedSubType)
  {
    var mime = new MimeType(mimeType);

    Assert.That(mime.Type, Is.EqualTo(expectedType));
    Assert.That(mime.SubType, Is.EqualTo(expectedSubType));
    Assert.That(mime.ToString(), Is.EqualTo(mimeType));
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

    Assert.That(mime.Type, Is.EqualTo(type));
    Assert.That(mime.SubType, Is.EqualTo(subtype));
    Assert.That(mime.ToString(), Is.EqualTo($"{type}/{subtype}"));
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

    Assert.That(mime.Type, Is.EqualTo(type));
    Assert.That(mime.SubType, Is.EqualTo(subtype));
    Assert.That(mime.ToString(), Is.EqualTo($"{type}/{subtype}"));
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

    Assert.That(type, Is.EqualTo("text"));
    Assert.That(subType, Is.EqualTo("plain"));
  }

  [Test]
  public void Test_ToString()
  {
    Assert.That(MimeType.TextPlain.ToString(), Is.EqualTo("text/plain"));
    Assert.That(MimeType.ApplicationOctetStream.ToString(), Is.EqualTo("application/octet-stream"));
    Assert.That(MimeType.CreateTextType("html").ToString(), Is.EqualTo("text/html"));
  }

  [Test]
  public void ExplicitToStringCoversion()
  {
    Assert.That((string)MimeType.TextPlain, Is.EqualTo("text/plain"));
    Assert.That((string)MimeType.ApplicationOctetStream, Is.EqualTo("application/octet-stream"));
    Assert.That((string)MimeType.CreateTextType("html"), Is.EqualTo("text/html"));

    Assert.That((string)((MimeType)null), Is.Null);
  }
}
