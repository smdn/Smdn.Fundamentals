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
    => Assert.AreEqual(expected, MimeType.Parse(s, provider: null));

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void Parse_ReadOnlySpanOfChar(string s, MimeType expected)
    => Assert.AreEqual(expected, MimeType.Parse(s.AsSpan(), provider: null));

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void TryParse(string s, MimeType expected)
  {
    Assert.IsTrue(MimeType.TryParse(s, provider: null, out var result));
    Assert.AreEqual(expected, result);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void TryParse_ReadOnlySpanOfChar(string s, MimeType expected)
  {
    Assert.IsTrue(MimeType.TryParse(s.AsSpan(), provider: null, out var result));
    Assert.AreEqual(expected, result);
  }

#if FEATURE_GENERIC_MATH
  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void IParseable_Parse(string s, MimeType expected)
  {
    Assert.AreEqual(expected, Parse<MimeType>(s));

    static TSelf Parse<TSelf>(string s) where TSelf : IParseable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void ISpanParseable_Parse(string s, MimeType expected)
  {
    Assert.AreEqual(expected, Parse<MimeType>(s.AsSpan()));

    static TSelf Parse<TSelf>(ReadOnlySpan<char> s) where TSelf : ISpanParseable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void IParseable_TryParse(string s, MimeType expected)
  {
    Assert.IsTrue(TryParse<MimeType>(s, out var result));
    Assert.AreEqual(expected, result);

    static bool TryParse<TSelf>(string s, out TSelf result) where TSelf : IParseable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }

  [TestCaseSource(nameof(YieldParseValidTestCases))]
  public void ISpanParseable_TryParse(string s, MimeType expected)
  {
    Assert.IsTrue(TryParse<MimeType>(s.AsSpan(), out var result));
    Assert.AreEqual(expected, result);

    static bool TryParse<TSelf>(ReadOnlySpan<char> s, out TSelf result) where TSelf : ISpanParseable<TSelf>
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
    => Assert.IsFalse(MimeType.TryParse(s, provider: null, out _));

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void TryParse_ReadOnlySpanOfChar_InvalidFormat(string s, Type discard)
  {
    if (s is null)
      Assert.Pass();

    Assert.IsFalse(MimeType.TryParse(s.AsSpan(), provider: null, out _));
  }

#if FEATURE_GENERIC_MATH
  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void IParseable_Parse_InvalidFormat(string s, Type expectedExceptionType)
  {
    Assert.Throws(expectedExceptionType, () => Parse<MimeType>(s));

    static TSelf Parse<TSelf>(string s) where TSelf : IParseable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void ISpanParseable_Parse_InvalidFormat(string s, Type expectedExceptionType)
  {
    if (s is null)
      Assert.Pass();

    Assert.Throws(expectedExceptionType, () => Parse<MimeType>(s.AsSpan()));

    static TSelf Parse<TSelf>(ReadOnlySpan<char> s) where TSelf : ISpanParseable<TSelf>
      => TSelf.Parse(s, provider: null);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void IParseable_TryParse_InvalidFormat(string s, Type discard)
  {
    if (s is null)
      Assert.Pass();

    Assert.IsFalse(TryParse<MimeType>(s, out _));

    static bool TryParse<TSelf>(string s, out TSelf result) where TSelf : IParseable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }

  [TestCaseSource(nameof(YieldParseInvalidFormatTestCases))]
  public void ISpanParseable_TryParse_InvalidFormat(string s, Type discard)
  {
    if (s is null)
      Assert.Pass();

    Assert.IsFalse(TryParse<MimeType>(s.AsSpan(), out _));

    static bool TryParse<TSelf>(ReadOnlySpan<char> s, out TSelf result) where TSelf : ISpanParseable<TSelf>
      => TSelf.TryParse(s, provider: null, out result);
  }
#endif
}
