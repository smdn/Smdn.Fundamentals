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
public class RFC822DateTimeFormatsTests {
  private static IEnumerable YieldTestCases_ParseDateTime()
  {
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeString_UniversalTime())
      yield return testCase;
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeString_MilitaryTimeZones())
      yield return testCase;
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeString_NorthAmericanTimeZones())
      yield return testCase;
  }

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

  private static IEnumerable YieldTestCases_ParseDateTime_Local()
    => DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeString_Local();

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void ParseDateTime_Local(string s, DateTime expected)
  {
    var result = RFC822DateTimeFormats.ParseDateTime(s);

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void TryParseDateTime_Local(string s, DateTime expected)
  {
    Assert.IsTrue(RFC822DateTimeFormats.TryParseDateTime(s, out var result));
    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void ParseDateTime_ReadOnlySpanOfChar_Local(string s, DateTime expected)
  {
    var result = RFC822DateTimeFormats.ParseDateTime(s.AsSpan());

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void TryParseDateTime_ReadOnlySpanOfChar_Local(string s, DateTime expected)
  {
    Assert.IsTrue(RFC822DateTimeFormats.TryParseDateTime(s.AsSpan(), out var result));
    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }
#endif

  private static IEnumerable YieldTestCases_ParseDateTimeOffset()
  {
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeOffsetString())
      yield return testCase;
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeOffsetString_UniversalTime())
      yield return testCase;
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeOffsetString_MilitaryTimeZones())
      yield return testCase;
    foreach (var testCase in DateTimeFormatTests.YieldTestCases_FromRFC822DateTimeOffsetString_NorthAmericanTimeZones())
      yield return testCase;
  }

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

  internal static IEnumerable YieldTestCases_ParseCommon_InvalidFormat()
  {
    yield return new object[] { string.Empty }; // empty string
    yield return new object[] { "!" }; // non-datetime string


    yield return new object[] { "Thu, 01 May 2022 23:17:14.0123456 +0000" }; // day of week / mismatch
    yield return new object[] { "Nul, 01 May 2022 23:17:14.0123456 +0000" }; // day of week / invalid
    yield return new object[] { "Su, 01 May 2022 23:17:14.0123456 +0000" }; // day of week / invalid
    foreach (var dayOfWeek in new [] { "Sun, ", string.Empty }) {
      yield return new object[] { $"{dayOfWeek}0 May 2022 23:17:14.0123456 +0000" }; // day / invalid
      yield return new object[] { $"{dayOfWeek}32 May 2022 23:17:14.0123456 +0000" }; // day / invalid
      yield return new object[] { $"{dayOfWeek}01 Nul 2022 23:17:14.0123456 +0000" }; // month / invalid
      yield return new object[] { $"{dayOfWeek}01 March 2022 23:17:14.0123456 +0000" }; // month / invalid
      yield return new object[] { $"{dayOfWeek}01 May 022 23:17:14.0123456 +0000" }; // year / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 25:17:14.0123456 +0000" }; // hour / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:60:14.0123456 +0000" }; // minute / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:61.0123456 +0000" }; // second / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14. +0000" }; // second / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.01234567 +0000" }; // precision / overflow
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.0123456 J" }; // time zone / non-existent
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.0123456 GM" }; // time zone / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.0123456 UTC" }; // time zone / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.0123456 JST" }; // time zone / invalid
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.0123456 +00" }; // time zone / invalid
#if false
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:17:14.0123456 +00:00" }; // time zone / invalid but acceptable
#endif
      yield return new object[] { $"{dayOfWeek}01 May 2022 23:" }; // lack of minute
      yield return new object[] { $"{dayOfWeek}01 May 2022 23" }; // lack of minute
      yield return new object[] { $"{dayOfWeek}01 May 2022 " }; // lack of hour
      yield return new object[] { $"{dayOfWeek}01 May 2022" }; // lack of hour
      yield return new object[] { $"{dayOfWeek}01 May " }; // lack of year
      yield return new object[] { $"{dayOfWeek}01 May" }; // lack of year
      yield return new object[] { $"{dayOfWeek}01 " }; // lack of month
      yield return new object[] { $"{dayOfWeek}01" }; // lack of month
      yield return new object[] { $"{dayOfWeek}" }; // lack of day
    }
  }

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTime(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTime(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTime_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTime(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTime(s.AsSpan(), out _));
#endif

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTimeOffset(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => RFC822DateTimeFormats.ParseDateTimeOffset(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTimeOffset_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTimeOffset(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTimeOffset(s.AsSpan(), out _));
#endif

  [Test]
  public void ParseDateTime_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => RFC822DateTimeFormats.ParseDateTime((string)null));

  [Test]
  public void TryParseDateTime_ArgumentNull()
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTime((string)null, out _));

  [Test]
  public void ParseDateTimeOffset_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => RFC822DateTimeFormats.ParseDateTimeOffset((string)null));

  [Test]
  public void TryParseDateTimeOffset_ArgumentNull()
    => Assert.IsFalse(RFC822DateTimeFormats.TryParseDateTimeOffset((string)null, out _));
}
