// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public partial class RFC822DateTimeFormatsTests {
  /*
   * ParseDateTime/ParseDateTimeOffset comprehensive tests
   */
  private static IEnumerable YieldTestCases_Comprehensive_ParseDateTime()
    => YieldTestCases_ParseDateTime_All(comprehensive: true);

  [TestCaseSource(nameof(YieldTestCases_Comprehensive_ParseDateTime))]
  public void Comprehensive_ParseDateTime(string s, DateTime expected)
    => Assert.AreEqual(expected, RFC822DateTimeFormats.ParseDateTime(s));

  private static IEnumerable YieldFullTestCases_Comprehensive_ParseDateTimeOffset()
    => YieldTestCases_ParseDateTimeOffset_All(comprehensive: true);

  [TestCaseSource(nameof(YieldFullTestCases_Comprehensive_ParseDateTimeOffset))]
  public void Comprehensive_ParseDateTimeOffset(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, RFC822DateTimeFormats.ParseDateTimeOffset(s));

  /*
   * ParseDateTime/TryParseDateTime
   */
  internal static IEnumerable YieldTestCases_ParseDateTime()
    => YieldTestCases_ParseDateTime_All(comprehensive: false);

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime(string s, DateTime expected)
    => Assert.AreEqual(expected, RFC822DateTimeFormats.ParseDateTime(s));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime(string s, DateTime expected)
  {
    Assert.IsTrue(RFC822DateTimeFormats.TryParseDateTime(s, out var result));
    Assert.AreEqual(expected, result);
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
    => Assert.AreEqual(expected, RFC822DateTimeFormats.ParseDateTime(s.AsSpan()));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
  {
    Assert.IsTrue(RFC822DateTimeFormats.TryParseDateTime(s.AsSpan(), out var result));
    Assert.AreEqual(expected, result);
  }
#endif

  /*
   * ParseDateTimeOffset/TryParseDateTimeOffset
   */
  internal static IEnumerable YieldTestCases_ParseDateTimeOffset()
    => YieldTestCases_ParseDateTimeOffset_All(comprehensive: false);

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, RFC822DateTimeFormats.ParseDateTimeOffset(s));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset(string s, DateTimeOffset expected)
  {
    Assert.IsTrue(RFC822DateTimeFormats.TryParseDateTimeOffset(s, out var result));
    Assert.AreEqual(expected, result);
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, RFC822DateTimeFormats.ParseDateTimeOffset(s.AsSpan()));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
  {
    Assert.IsTrue(RFC822DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out var result));
    Assert.AreEqual(expected, result);
  }
#endif

  /*
   * tests for invalid format
   */
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat_Comprehensive))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTime(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void ParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTime(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTime(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTime(s.AsSpan(), out _));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat_Comprehensive))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTimeOffset(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTimeOffset(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTimeOffset(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out _));
#endif

  [Test]
  public void ParseDateTime_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => RFC822DateTimeFormats.ParseDateTime((string)null!));

  [Test]
  public void TryParseDateTime_ArgumentNull()
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTime((string)null, out _));

  [Test]
  public void ParseDateTimeOffset_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => RFC822DateTimeFormats.ParseDateTimeOffset((string)null!));

  [Test]
  public void TryParseDateTimeOffset_ArgumentNull()
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTimeOffset((string)null, out _));
}
