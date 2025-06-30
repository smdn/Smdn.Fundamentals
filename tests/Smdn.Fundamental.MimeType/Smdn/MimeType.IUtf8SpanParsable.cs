// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_IUTF8SPANPARSABLE
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeTypeTests {
#pragma warning restore IDE0040
  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void IUtf8SpanParsable_Parse(string s, MimeType expected)
  {
    var utf8Text = Encoding.UTF8.GetBytes(s).AsSpan();

    Assert.That(MimeType.Parse(utf8Text, provider: null), Is.EqualTo(expected));
    Assert.That(Parse<MimeType>(utf8Text), Is.EqualTo(expected));

    static TSelf Parse<TSelf>(ReadOnlySpan<byte> utf8Text) where TSelf : IUtf8SpanParsable<TSelf>
      => TSelf.Parse(utf8Text, provider: null);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void IUtf8SpanParsable_TryParse(string s, MimeType expected)
  {
    var utf8Text = Encoding.UTF8.GetBytes(s).AsSpan();

    Assert.That(MimeType.TryParse(utf8Text, provider: null, out var result1), Is.True);
    Assert.That(result1, Is.EqualTo(expected));

    Assert.That(TryParse<MimeType>(utf8Text, out var result2), Is.True);
    Assert.That(result2, Is.Not.Null);
    Assert.That(result2, Is.EqualTo(expected));

    static bool TryParse<TSelf>(ReadOnlySpan<byte> utf8Text, [NotNullWhen(true)] out TSelf? result) where TSelf : class, IUtf8SpanParsable<TSelf>
      => TSelf.TryParse(utf8Text, provider: null, out result);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void IUtf8SpanParsable_Parse_InvalidFormat(string? s, Type expectedExceptionType)
  {
    if (s is null) {
      Assert.Pass();
      return;
    }

    Assert.Throws(expectedExceptionType, () => MimeType.Parse(Encoding.UTF8.GetBytes(s).AsSpan(), provider: null));
    Assert.Throws(expectedExceptionType, () => Parse<MimeType>(Encoding.UTF8.GetBytes(s).AsSpan()));

    static TSelf Parse<TSelf>(ReadOnlySpan<byte> utf8Text) where TSelf : IUtf8SpanParsable<TSelf>
      => TSelf.Parse(utf8Text, provider: null);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void IUtf8SpanParsable_TryParse_InvalidFormat(string s, Type discard)
  {
    if (s is null) {
      Assert.Pass();
      return;
    }

    Assert.That(MimeType.TryParse(Encoding.UTF8.GetBytes(s).AsSpan(), provider: null, out _), Is.False);
    Assert.That(TryParse<MimeType>(Encoding.UTF8.GetBytes(s).AsSpan()), Is.False);

    static bool TryParse<TSelf>(ReadOnlySpan<byte> utf8Text) where TSelf : IUtf8SpanParsable<TSelf>
      => TSelf.TryParse(utf8Text, provider: null, out _);
  }
}
#endif
