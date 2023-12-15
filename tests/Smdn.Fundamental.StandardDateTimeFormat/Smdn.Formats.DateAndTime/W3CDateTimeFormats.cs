// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
    => Assert.That(W3CDateTimeFormats.ParseDateTime(s), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime(string s, DateTime expected)
  {
    Assert.That(W3CDateTimeFormats.TryParseDateTime(s, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
    => Assert.That(W3CDateTimeFormats.ParseDateTime(s.AsSpan()), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
  {
    Assert.That(W3CDateTimeFormats.TryParseDateTime(s.AsSpan(), out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }
#endif

  /*
   * ParseDateTimeOffset/TryParseDateTimeOffset
   */
  internal static IEnumerable YieldTestCases_ParseDateTimeOffset()
    => ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset_All(comprehensive: false);

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset(string s, DateTimeOffset expected)
    => Assert.That(W3CDateTimeFormats.ParseDateTimeOffset(s), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset(string s, DateTimeOffset expected)
  {
    Assert.That(W3CDateTimeFormats.TryParseDateTimeOffset(s, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
    => Assert.That(W3CDateTimeFormats.ParseDateTimeOffset(s.AsSpan()), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
  {
    Assert.That(W3CDateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
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
    => Assert.That(W3CDateTimeFormats.TryParseDateTime(s, out _), Is.False);

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.That(W3CDateTimeFormats.TryParseDateTime(s.AsSpan(), out _), Is.False);
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
    => Assert.That(W3CDateTimeFormats.TryParseDateTimeOffset(s, out _), Is.False);

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.That(W3CDateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out _), Is.False);
#endif

  [Test]
  public void ParseDateTime_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => W3CDateTimeFormats.ParseDateTime((string)null!));

  [Test]
  public void TryParseDateTime_ArgumentNull()
    => Assert.That(W3CDateTimeFormats.TryParseDateTime((string)null, out _), Is.False);

  [Test]
  public void ParseDateTimeOffset_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => W3CDateTimeFormats.ParseDateTimeOffset((string)null!));

  [Test]
  public void TryParseDateTimeOffset_ArgumentNull()
    => Assert.That(W3CDateTimeFormats.TryParseDateTimeOffset((string)null, out _), Is.False);
}
