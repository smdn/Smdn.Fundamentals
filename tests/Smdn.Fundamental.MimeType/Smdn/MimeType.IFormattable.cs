// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
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
    )] string format
  )
    => Assert.AreEqual(mimeType, new MimeType(mimeType).ToString(format: format, formatProvider: null));

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
