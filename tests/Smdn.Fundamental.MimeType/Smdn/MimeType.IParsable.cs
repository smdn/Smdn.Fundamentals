// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
  private static System.Collections.IEnumerable YieldParseValidTestCases()
  {
    yield return new object[] { "text/plain", MimeType.TextPlain };
    yield return new object[] { "message/rfc822", MimeType.MessageRfc822 };
    yield return new object[] { "application/rdf+xml", MimeType.CreateApplicationType("rdf+xml") };
    yield return new object[] { new string('x', 63) + "/" + new string('x', 63), new MimeType(new string('x', 63), new string('x', 63)) };
  }

  private static System.Collections.IEnumerable YieldParseInvalidFormatTestCases()
  {
    yield return new object[] { null, typeof(ArgumentNullException) };
    yield return new object[] { string.Empty, typeof(ArgumentException) };
    yield return new object[] { "text", typeof(FormatException) };
    yield return new object[] { "text/", typeof(FormatException) };
    yield return new object[] { "/", typeof(FormatException) };
    yield return new object[] { "/plain", typeof(FormatException) };
    yield return new object[] { "text/plain/", typeof(FormatException) };
    yield return new object[] { "text/plain/foo", typeof(FormatException) };
    yield return new object[] { new string('x', 63) + "/" + new string('x', 64), typeof(FormatException) };
    yield return new object[] { new string('x', 64) + "/" + new string('x', 63), typeof(FormatException) };
    yield return new object[] { new string('x', 64) + "/" + new string('x', 64), typeof(FormatException) };
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void Parse(string s, MimeType expected)
    => Assert.That(MimeType.Parse(s, provider: null), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void Parse_ReadOnlySpanOfChar(string s, MimeType expected)
    => Assert.That(MimeType.Parse(s.AsSpan(), provider: null), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void TryParse(string s, MimeType expected)
  {
    Assert.That(MimeType.TryParse(s, provider: null, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void TryParse_ReadOnlySpanOfChar(string s, MimeType expected)
  {
    Assert.That(MimeType.TryParse(s.AsSpan(), provider: null, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }

#if FEATURE_GENERIC_MATH
  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void IParsable_Parse(string s, MimeType expected)
  {
    Assert.That(Parse<MimeType>(s), Is.EqualTo(expected));

    static TSelf Parse<TSelf>(string s) where TSelf : IParsable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void ISpanParsable_Parse(string s, MimeType expected)
  {
    Assert.That(Parse<MimeType>(s.AsSpan()), Is.EqualTo(expected));

    static TSelf Parse<TSelf>(ReadOnlySpan<char> s) where TSelf : ISpanParsable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void IParsable_TryParse(string s, MimeType expected)
  {
    Assert.That(TryParse<MimeType>(s, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));

    static bool TryParse<TSelf>(string s, out TSelf result) where TSelf : IParsable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void ISpanParsable_TryParse(string s, MimeType expected)
  {
    Assert.That(TryParse<MimeType>(s.AsSpan(), out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));

    static bool TryParse<TSelf>(ReadOnlySpan<char> s, out TSelf result) where TSelf : ISpanParsable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }
#endif

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void Parse_InvalidFormat(string s, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => MimeType.Parse(s, provider: null));

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void Parse_ReadOnlySpanOfChar_InvalidFormat(string s, Type expectedExceptionType)
  {
    if (s is null)
      Assert.Pass();

    Assert.Throws(expectedExceptionType, () => MimeType.Parse(s.AsSpan(), provider: null));
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void TryParse_InvalidFormat(string s, Type discard)
    => Assert.That(MimeType.TryParse(s, provider: null, out _), Is.False);

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void TryParse_ReadOnlySpanOfChar_InvalidFormat(string s, Type discard)
  {
    if (s is null)
      Assert.Pass();

    Assert.That(MimeType.TryParse(s.AsSpan(), provider: null, out _), Is.False);
  }

#if FEATURE_GENERIC_MATH
  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void IParsable_Parse_InvalidFormat(string s, Type expectedExceptionType)
  {
    Assert.Throws(expectedExceptionType, () => Parse<MimeType>(s));

    static TSelf Parse<TSelf>(string s) where TSelf : IParsable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void ISpanParsable_Parse_InvalidFormat(string s, Type expectedExceptionType)
  {
    if (s is null)
      Assert.Pass();

    Assert.Throws(expectedExceptionType, () => Parse<MimeType>(s.AsSpan()));

    static TSelf Parse<TSelf>(ReadOnlySpan<char> s) where TSelf : ISpanParsable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void IParsable_TryParse_InvalidFormat(string s, Type discard)
  {
    if (s is null)
      Assert.Pass();

    Assert.That(TryParse<MimeType>(s, out _), Is.False);

    static bool TryParse<TSelf>(string s, out TSelf result) where TSelf : IParsable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void ISpanParsable_TryParse_InvalidFormat(string s, Type discard)
  {
    if (s is null)
      Assert.Pass();

    Assert.That(TryParse<MimeType>(s.AsSpan(), out _), Is.False);

    static bool TryParse<TSelf>(ReadOnlySpan<char> s, out TSelf result) where TSelf : ISpanParsable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }
#endif
}
