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
    => Assert.That(RFC822DateTimeFormats.ParseDateTime(s), Is.EqualTo(expected));

  private static IEnumerable YieldFullTestCases_Comprehensive_ParseDateTimeOffset()
    => YieldTestCases_ParseDateTimeOffset_All(comprehensive: true);

  [TestCaseSource(nameof(YieldFullTestCases_Comprehensive_ParseDateTimeOffset))]
  public void Comprehensive_ParseDateTimeOffset(string s, DateTimeOffset expected)
    => Assert.That(RFC822DateTimeFormats.ParseDateTimeOffset(s), Is.EqualTo(expected));

  /*
   * ParseDateTime/TryParseDateTime
   */
  internal static IEnumerable YieldTestCases_ParseDateTime()
    => YieldTestCases_ParseDateTime_All(comprehensive: false);

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime(string s, DateTime expected)
    => Assert.That(RFC822DateTimeFormats.ParseDateTime(s), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime(string s, DateTime expected)
  {
    Assert.That(RFC822DateTimeFormats.TryParseDateTime(s, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
    => Assert.That(RFC822DateTimeFormats.ParseDateTime(s.AsSpan()), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
  {
    Assert.That(RFC822DateTimeFormats.TryParseDateTime(s.AsSpan(), out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
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
    => Assert.That(RFC822DateTimeFormats.ParseDateTimeOffset(s), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset(string s, DateTimeOffset expected)
  {
    Assert.That(RFC822DateTimeFormats.TryParseDateTimeOffset(s, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
    => Assert.That(RFC822DateTimeFormats.ParseDateTimeOffset(s.AsSpan()), Is.EqualTo(expected));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
  {
    Assert.That(RFC822DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out var result), Is.True);
    Assert.That(result, Is.EqualTo(expected));
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
    => Assert.That(RFC822DateTimeFormats.TryParseDateTime(s, out _), Is.False);

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.That(RFC822DateTimeFormats.TryParseDateTime(s.AsSpan(), out _), Is.False);
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
    => Assert.That(RFC822DateTimeFormats.TryParseDateTimeOffset(s, out _), Is.False);

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.That(RFC822DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out _), Is.False);
#endif

  [Test]
  public void ParseDateTime_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => RFC822DateTimeFormats.ParseDateTime((string)null!));

  [Test]
  public void TryParseDateTime_ArgumentNull()
    => Assert.That(RFC822DateTimeFormats.TryParseDateTime((string)null, out _), Is.False);

  [Test]
  public void ParseDateTimeOffset_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => RFC822DateTimeFormats.ParseDateTimeOffset((string)null!));

  [Test]
  public void TryParseDateTimeOffset_ArgumentNull()
    => Assert.That(RFC822DateTimeFormats.TryParseDateTimeOffset((string)null, out _), Is.False);
}
