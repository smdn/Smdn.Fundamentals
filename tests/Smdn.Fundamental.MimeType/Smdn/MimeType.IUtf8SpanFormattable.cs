// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_IUTF8SPANFORMATTABLE
using System;
using System.Text;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
  [Test]
  public void IUtf8SpanFormattable_TryFormat(
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
  {
    Span<byte> utf8Destination = stackalloc byte[mimeType.Length];

    Assert.That(
      new MimeType(mimeType).TryFormat(utf8Destination, out var bytesWritten, format.AsSpan(), provider: null),
      Is.True
    );
    Assert.That(bytesWritten, Is.EqualTo(mimeType.Length), nameof(bytesWritten));
    Assert.That(Encoding.ASCII.GetString(utf8Destination.Slice(0, bytesWritten)), Is.EqualTo(mimeType));
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(9)] // "text/plain".Length - 1
  public void IUtf8SpanFormattable_TryFormat_DestinationTooShort(int length)
    => Assert.That(
      new MimeType("text/plain").TryFormat(stackalloc byte[length], out _, format: default, provider: null),
      Is.False
    );

  [TestCase("D")]
  [TestCase("X")]
  [TestCase("unsupported fromat")]
  public void IUtf8SpanFormattable_TryFormat_UnsupportedFormatString(string format)
    => Assert.Throws<FormatException>(
      () => new MimeType("text/plain").TryFormat(stackalloc byte[10], out _, format: format.AsSpan(), provider: null)
    );
}
#endif
