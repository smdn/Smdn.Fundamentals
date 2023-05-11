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
    Assert.IsTrue(MimeType.TryParse("text/plain", out (string type, string subType) result));
    Assert.AreEqual("text", result.type);
    Assert.AreEqual("plain", result.subType);
  }
#pragma warning restore 0618

#pragma warning disable 0618
  [Test]
  [Category("obsolete")]
  public void MimeType_Parse_Tuple()
  {
#pragma warning disable CA1305
    (var type, var subType) = MimeType.Parse("text/plain");

    Assert.AreEqual("text", type);
    Assert.AreEqual("plain", subType);

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
    Assert.AreEqual(expectedType, type, nameof(expectedType));
    Assert.AreEqual(expectedSubType, subType, nameof(expectedSubType));
  }

  [TestCase("text/plain", "text", "plain")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  public void TrySplit(string s, string expectedType, string expectedSubType)
  {
    Assert.IsTrue(MimeTypeStringExtensions.TrySplit(s, out var result));
    Assert.AreEqual(expectedType, result.Type, nameof(expectedType));
    Assert.AreEqual(expectedSubType, result.SubType, nameof(expectedSubType));
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
    => Assert.IsFalse(MimeTypeStringExtensions.TrySplit(s, out _));
}
