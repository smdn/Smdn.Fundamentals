// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
  [Test]
  public void ISpanFormattable_TryFormat(
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
    Span<char> destination = stackalloc char[mimeType.Length];

    Assert.That(
      new MimeType(mimeType).TryFormat(destination, out var charsWritten, format.AsSpan(), provider: null),
      Is.True
    );
    Assert.That(charsWritten, Is.EqualTo(mimeType.Length), nameof(charsWritten));
    Assert.That(destination.Slice(0, charsWritten).ToString(), Is.EqualTo(mimeType));
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(9)] // "text/plain".Length - 1
  public void ISpanFormattable_TryFormat_DestinationTooShort(int length)
    => Assert.That(
      new MimeType("text/plain").TryFormat(stackalloc char[length], out _, format: default, provider: null),
      Is.False
    );

  [TestCase("D")]
  [TestCase("X")]
  [TestCase("unsupported fromat")]
  public void ISpanFormattable_TryFormat_UnsupportedFormatString(string format)
    => Assert.Throws<FormatException>(
      () => new MimeType("text/plain").TryFormat(stackalloc char[10], out _, format: format.AsSpan(), provider: null)
    );
}
