// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats.Mime;

[TestFixture()]
public class MimeTypeStringExtensionsTests {
#pragma warning disable 0618
  [Test]
  [Category("obsolete")]
  public void MimeType_TryParse_Tuple()
  {
    Assert.That(MimeType.TryParse("text/plain", out (string type, string subType) result), Is.True);
    Assert.That(result.type, Is.EqualTo("text"));
    Assert.That(result.subType, Is.EqualTo("plain"));
  }
#pragma warning restore 0618

#pragma warning disable 0618
  [Test]
  [Category("obsolete")]
  public void MimeType_Parse_Tuple()
  {
#pragma warning disable CA1305
    (var type, var subType) = MimeType.Parse("text/plain");

    Assert.That(type, Is.EqualTo("text"));
    Assert.That(subType, Is.EqualTo("plain"));

    Assert.Throws<ArgumentNullException>(() => MimeType.Parse(null!));
    Assert.Throws<ArgumentException>(() => MimeType.Parse(string.Empty));
    Assert.Throws<ArgumentException>(() => MimeType.Parse("text/"));
    Assert.Throws<ArgumentException>(() => MimeType.Parse("/plain"));
#pragma warning restore CA1305
  }
#pragma warning restore 0618

  [TestCase("text/plain", "text", "plain")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  public void Split(string s, string expectedType, string expectedSubType)
  {
    string type = default, subType = default;

    Assert.DoesNotThrow(() => (type, subType) = MimeTypeStringExtensions.Split(s));
    Assert.That(type, Is.EqualTo(expectedType), nameof(expectedType));
    Assert.That(subType, Is.EqualTo(expectedSubType), nameof(expectedSubType));
  }

  [TestCase("text/plain", "text", "plain")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  public void TrySplit(string s, string expectedType, string expectedSubType)
  {
    Assert.That(MimeTypeStringExtensions.TrySplit(s, out var result), Is.True);
    Assert.That(result.Type, Is.EqualTo(expectedType), nameof(expectedType));
    Assert.That(result.SubType, Is.EqualTo(expectedSubType), nameof(expectedSubType));
  }

  [TestCase(null, typeof(ArgumentNullException))]
  [TestCase("", typeof(ArgumentException))]
  [TestCase("text", typeof(ArgumentException))]
  [TestCase("text/", typeof(ArgumentException))]
  [TestCase("/", typeof(ArgumentException))]
  [TestCase("/plain", typeof(ArgumentException))]
  [TestCase("text/plain/", typeof(ArgumentException))]
  [TestCase("text/plain/foo", typeof(ArgumentException))]
  public void Split_Invalid(string s, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => MimeTypeStringExtensions.Split(s));

  [TestCase(null)]
  [TestCase("")]
  [TestCase("text")]
  [TestCase("text/")]
  [TestCase("/")]
  [TestCase("/plain")]
  [TestCase("text/plain/")]
  [TestCase("text/plain/foo")]
  public void TrySplit_Invalid(string s)
    => Assert.That(MimeTypeStringExtensions.TrySplit(s, out _), Is.False);
}
