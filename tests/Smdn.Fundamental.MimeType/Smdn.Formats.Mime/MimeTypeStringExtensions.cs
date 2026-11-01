// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Formats.Mime;

[TestFixture()]
public class MimeTypeStringExtensionsTests {
  [TestCase("text/plain", "text", "plain")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  public void Split(string s, string expectedType, string expectedSubType)
  {
    string? type = default, subType = default;

    Assert.That(() => (type, subType) = MimeTypeStringExtensions.Split(s), Throws.Nothing);
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
  public void Split_Invalid(string? s, Type expectedExceptionType)
    => Assert.That(
      () => MimeTypeStringExtensions.Split(s!),
      Throws.TypeOf(expectedExceptionType)
    );

  [TestCase(null)]
  [TestCase("")]
  [TestCase("text")]
  [TestCase("text/")]
  [TestCase("/")]
  [TestCase("/plain")]
  [TestCase("text/plain/")]
  [TestCase("text/plain/foo")]
  public void TrySplit_Invalid(string? s)
    => Assert.That(MimeTypeStringExtensions.TrySplit(s, out _), Is.False);
}
