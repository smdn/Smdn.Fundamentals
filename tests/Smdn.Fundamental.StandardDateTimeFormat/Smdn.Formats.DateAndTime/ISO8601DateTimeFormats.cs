// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
#if SYSTEM_GLOBALIZATION_ISOWEEK
using System.Globalization;
#endif

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public partial class ISO8601DateTimeFormatsTests {
  /*
   * ParseDateTime/ParseDateTimeOffset comprehensive tests
   */
  private static IEnumerable YieldTestCases_Comprehensive_ParseDateTime()
    => YieldTestCases_ParseDateTime_All(comprehensive: true);

  [TestCaseSource(nameof(YieldTestCases_Comprehensive_ParseDateTime))]
  public void Comprehensive_ParseDateTime(string s, DateTime expected)
    => Assert.AreEqual(expected, ISO8601DateTimeFormats.ParseDateTime(s));

  private static IEnumerable YieldFullTestCases_Comprehensive_ParseDateTimeOffset()
    => YieldTestCases_ParseDateTimeOffset_All(comprehensive: true);

  [TestCaseSource(nameof(YieldFullTestCases_Comprehensive_ParseDateTimeOffset))]
  public void Comprehensive_ParseDateTimeOffset(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, ISO8601DateTimeFormats.ParseDateTimeOffset(s));

  /*
   * ParseDateTime/TryParseDateTime
   */
  internal static IEnumerable YieldTestCases_ParseDateTime()
    => YieldTestCases_ParseDateTime_All(comprehensive: false);

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime(string s, DateTime expected)
    => Assert.AreEqual(expected, ISO8601DateTimeFormats.ParseDateTime(s));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime(string s, DateTime expected)
  {
    Assert.IsTrue(ISO8601DateTimeFormats.TryParseDateTime(s, out var result));
    Assert.AreEqual(expected, result);
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void ParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
    => Assert.AreEqual(expected, ISO8601DateTimeFormats.ParseDateTime(s.AsSpan()));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime))]
  public void TryParseDateTime_ReadOnlySpanOfChar(string s, DateTime expected)
  {
    Assert.IsTrue(ISO8601DateTimeFormats.TryParseDateTime(s.AsSpan(), out var result));
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
    => Assert.AreEqual(expected, ISO8601DateTimeFormats.ParseDateTimeOffset(s));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset(string s, DateTimeOffset expected)
  {
    Assert.IsTrue(ISO8601DateTimeFormats.TryParseDateTimeOffset(s, out var result));
    Assert.AreEqual(expected, result);
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, ISO8601DateTimeFormats.ParseDateTimeOffset(s.AsSpan()));

  [TestCaseSource(nameof(YieldTestCases_ParseDateTimeOffset))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar(string s, DateTimeOffset expected)
  {
    Assert.IsTrue(ISO8601DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out var result));
    Assert.AreEqual(expected, result);
  }
#endif

  /*
   * tests for invalid format
   */
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat_Comprehensive))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => ISO8601DateTimeFormats.ParseDateTime(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void ParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => ISO8601DateTimeFormats.ParseDateTime(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_InvalidFormat(string s)
    => Assert.IsFalse(ISO8601DateTimeFormats.TryParseDateTime(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(ISO8601DateTimeFormats.TryParseDateTime(s.AsSpan(), out _));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat_Comprehensive))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => ISO8601DateTimeFormats.ParseDateTimeOffset(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => ISO8601DateTimeFormats.ParseDateTimeOffset(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_InvalidFormat(string s)
    => Assert.IsFalse(ISO8601DateTimeFormats.TryParseDateTimeOffset(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_Parse_InvalidFormat))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(ISO8601DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out _));
#endif

  [Test]
  public void ParseDateTime_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ISO8601DateTimeFormats.ParseDateTime((string)null!));

  [Test]
  public void TryParseDateTime_ArgumentNull()
    => Assert.IsFalse(ISO8601DateTimeFormats.TryParseDateTime((string)null, out _));

  [Test]
  public void ParseDateTimeOffset_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ISO8601DateTimeFormats.ParseDateTimeOffset((string)null!));

  [Test]
  public void TryParseDateTimeOffset_ArgumentNull()
    => Assert.IsFalse(ISO8601DateTimeFormats.TryParseDateTimeOffset((string)null, out _));

  /*
   * ISO week
   */
#if SYSTEM_GLOBALIZATION_ISOWEEK
  private static IEnumerable YieldTestCases_ToWeekDateString(bool time, bool offset)
  {
    string t = time ? "T00:00:00.0000000" : string.Empty;
    string tz = time && offset ? "+00:00" : string.Empty;

    yield return new[] { $"2019-12-29{t}{tz}", "2019-W52-07" };
    yield return new[] { $"2019-12-30{t}{tz}", "2020-W01-01" };
    yield return new[] { $"2020-01-01{t}{tz}", "2020-W01-03" };
    yield return new[] { $"2020-06-30{t}{tz}", "2020-W27-02" };
    yield return new[] { $"2020-12-31{t}{tz}", "2020-W53-04" };
    yield return new[] { $"2021-01-03{t}{tz}", "2020-W53-07" };
    yield return new[] { $"2021-12-31{t}{tz}", "2021-W52-05" };
    yield return new[] { $"2022-05-01{t}{tz}", "2022-W17-07" };

    if (time && offset) {
      yield return new[] { "2020-01-01T00:00:00.0000000Z", "2020-W01-03" };
      yield return new[] { "2020-01-01T00:00:00.0000000+00:00", "2020-W01-03" };
      yield return new[] { "2020-01-01T00:00:00.0000000-04:00", "2020-W01-03" };
      yield return new[] { "2020-01-01T00:00:00.0000000+12:45", "2020-W01-03" };
      yield return new[] { "2020-01-01T00:00:00.0000000+09:00", "2020-W01-03" };
    }
  }

  private static IEnumerable YieldTestCases_ToWeekDateString_DateTime()
    => YieldTestCases_ToWeekDateString(time: true, offset: false);

  private static IEnumerable YieldTestCases_ToWeekDateString_DateTimeOffset()
    => YieldTestCases_ToWeekDateString(time: true, offset: true);

  [TestCaseSource(nameof(YieldTestCases_ToWeekDateString_DateTime))]
  public void ToWeekDateString_DateTime(string dateTime, string expected)
    => Assert.AreEqual(
      expected,
      ISO8601DateTimeFormats.ToWeekDateString(DateTime.ParseExact(dateTime, "o", null))
    );

  [TestCaseSource(nameof(YieldTestCases_ToWeekDateString_DateTimeOffset))]
  public void ToWeekDateString_DateTimeOffset(string dateTime, string expected)
    => Assert.AreEqual(
      expected,
      ISO8601DateTimeFormats.ToWeekDateString(DateTimeOffset.ParseExact(dateTime, "o", null))
    );

#if SYSTEM_DATEONLY
  private static IEnumerable YieldTestCases_ToWeekDateString_DateOnly()
    => YieldTestCases_ToWeekDateString(time: false, offset: false);

  [TestCaseSource(nameof(YieldTestCases_ToWeekDateString_DateOnly))]
  public void ToWeekDateString_DateOnly(string dateTime, string expected)
    => Assert.AreEqual(
      expected,
      ISO8601DateTimeFormats.ToWeekDateString(DateOnly.ParseExact(dateTime, "o", null))
    );
#endif

#endif
}
