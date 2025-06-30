// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeTypeTests {
#pragma warning restore IDE0040
  [Test]
  public void IFormattable_ToString(
    [Values(
      "text/plain",
      "message/rfc822",
      "application/rdf+xml"
    )] string mimeType,
    [Values(
      null,
      ""
    )] string? format
  )
    => Assert.That(new MimeType(mimeType).ToString(format: format, formatProvider: null), Is.EqualTo(mimeType));

  [Test]
  public void IFormattable_ToString_InvalidFormat(
    [Values(
      "text/plain",
      "message/rfc822",
      "application/rdf+xml"
    )] string mimeType,
    [Values(
      "D",
      "X",
      "unsupported format"
    )] string format
  )
    => Assert.Throws<FormatException>(() => new MimeType(mimeType).ToString(format: format, formatProvider: null));
}
