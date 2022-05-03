// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET45_OR_GREATER || NETSTANDARD1_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_READONLYSPAN
#endif

using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public partial class W3CDateTimeFormatsTests {
  /*
   * ParseDateTime/TryParseDateTime
   */

  internal static IEnumerable YieldTestCases_ParseDateTime()
    => ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTime_All(comprehensive: false);

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime(string s, DateTime expected)
    => Assert.AreEqual(expected, W3CDateTimeFormats.ParseDateTime(s));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime(string s, DateTime expected)
  {
    Assert.IsTrue(W3CDateTimeFormats.TryParseDateTime(s, out var result));
    Assert.AreEqual(expected, result);
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
    => Assert.AreEqual(expected, W3CDateTimeFormats.ParseDateTime(s.AsSpan()));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
  {
    Assert.IsTrue(W3CDateTimeFormats.TryParseDateTime(s.AsSpan(), out var result));
    Assert.AreEqual(expected, result);
  }
#endif

  /*
   * ParseDateTimeOffset/TryParseDateTimeOffset
   */
  internal static IEnumerable YieldTestCases_ParseDateTimeOffset()
    => ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset_All(comprehensive: false);

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, W3CDateTimeFormats.ParseDateTimeOffset(s));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset(string s, DateTimeOffset expected)
  {
    Assert.IsTrue(W3CDateTimeFormats.TryParseDateTimeOffset(s, out var result));
    Assert.AreEqual(expected, result);
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, W3CDateTimeFormats.ParseDateTimeOffset(s.AsSpan()));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
  {
    Assert.IsTrue(W3CDateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out var result));
    Assert.AreEqual(expected, result);
  }
#endif

  /*
   * tests for invalid format
   */
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat_Comprehensive))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTime(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void ParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTime(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTime(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTime(s.AsSpan(), out _));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat_Comprehensive))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTimeOffset(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTimeOffset(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTimeOffset(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out _));
#endif

  [Test]
  public void ParseDateTime_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => W3CDateTimeFormats.ParseDateTime((string)null));

  [Test]
  public void TryParseDateTime_ArgumentNull()
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTime((string)null, out _));

  [Test]
  public void ParseDateTimeOffset_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => W3CDateTimeFormats.ParseDateTimeOffset((string)null));

  [Test]
  public void TryParseDateTimeOffset_ArgumentNull()
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTimeOffset((string)null, out _));
}
