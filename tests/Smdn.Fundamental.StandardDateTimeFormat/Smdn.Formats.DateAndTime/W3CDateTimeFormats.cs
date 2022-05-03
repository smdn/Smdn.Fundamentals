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
public class W3CDateTimeFormatsTests {
  private static IEnumerable YieldTestCases_ParseDateTime()
    => DateTimeFormatTests.YieldTestCases_FromW3CDateTimeString_UniversalTime();

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

  private static IEnumerable YieldTestCases_ParseDateTime_Local()
    => DateTimeFormatTests.YieldTestCases_FromW3CDateTimeString_Local();

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void ParseDateTime_Local(string s, DateTime expected)
  {
    var result = W3CDateTimeFormats.ParseDateTime(s);

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void TryParseDateTime_Local(string s, DateTime expected)
  {
    Assert.IsTrue(W3CDateTimeFormats.TryParseDateTime(s, out var result));
    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void ParseDateTime_ReadOnlySpanOfChar_Local(string s, DateTime expected)
  {
    var result = W3CDateTimeFormats.ParseDateTime(s.AsSpan());

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }

  [TestCaseSource(nameof(YieldTestCases_ParseDateTime_Local))]
  public void TryParseDateTime_ReadOnlySpanOfChar_Local(string s, DateTime expected)
  {
    Assert.IsTrue(W3CDateTimeFormats.TryParseDateTime(s.AsSpan(), out var result));
    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(result),
      result
    );
  }
#endif

  private static IEnumerable YieldTestCases_ParseDateTimeOffset()
    => DateTimeFormatTests.YieldTestCases_FromW3CDateTimeOffsetString();

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

  internal static IEnumerable YieldTestCases_ParseCommon_InvalidFormat()
  {
    yield return new object[] { string.Empty }; // empty string
    yield return new object[] { "!" }; // non-datetime string

#if false
    yield return new object[] { "2022-05-01T23:17:14.0123456+00:00" }; // VALID
#endif
    foreach (var T in new[] { "T", " " }) {
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456X" }; // time zone / invalid
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456U" }; // time zone / invalid
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456UT" }; // time zone / invalid
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456UTC" }; // time zone / invalid
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456+00" }; // time zone / invalid
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456 +00" }; // time zone / invalid
#if false
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456-00:00" }; // time zone / invalid but acceptable
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456 -00:00" }; // time zone / invalid but acceptable
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456+0000" }; // time zone / invalid but acceptable
      yield return new object[] { $"2022-05-01{T}23:17:14.0123456 +0000" }; // time zone / invalid but acceptable
#endif
    }

    foreach (var tz in new[] { "Z", "+00:00", string.Empty }) {
      yield return new object[] { $"2022-05-01_23:17:14.0123456{tz}" }; // date and time delimiter / invalid
      yield return new object[] { $"2022-05-01/23:17:14.0123456{tz}" }; // date and time delimiter / invalid

      foreach (var T in new[] { "T", " " }) {
        yield return new object[] { $"222-05-01{T}23:17:14.0123456{tz}" }; // year / invalid
        yield return new object[] { $"22-05-01{T}23:17:14.0123456{tz}" }; // year / invalid
        yield return new object[] { $"2-05-01{T}23:17:14.0123456{tz}" }; // year / invalid
        yield return new object[] { $"2022-0-01{T}23:17:14.0123456{tz}" }; // month / invalid
        yield return new object[] { $"2022-5-01{T}23:17:14.0123456{tz}" }; // month / invalid
        yield return new object[] { $"2022-13-01{T}23:17:14.0123456{tz}" }; // month / invalid
        yield return new object[] { $"2022-05-0{T}23:17:14.0123456{tz}" }; // day / invalid
        yield return new object[] { $"2022-05-1{T}23:17:14.0123456{tz}" }; // day / invalid
        yield return new object[] { $"2022-05-32{T}23:17:14.0123456{tz}" }; // day / invalid
        yield return new object[] { $"2022-05-01{T}25:17:14.0123456{tz}" }; // hour / invalid
        yield return new object[] { $"2022-05-01{T} 3:17:14.0123456{tz}" }; // hour / invalid
        yield return new object[] { $"2022-05-01{T}3:17:14.0123456{tz}" }; // hour / invalid
        yield return new object[] { $"2022-05-01{T}23: 7:14.0123456{tz}" }; // minute / invalid
        yield return new object[] { $"2022-05-01{T}23:7:14.0123456{tz}" }; // minute / invalid
        yield return new object[] { $"2022-05-01{T}23:60:14.0123456{tz}" }; // minute / invalid
        yield return new object[] { $"2022-05-01{T}23:17:1.0123456{tz}" }; // second / invalid
        yield return new object[] { $"2022-05-01{T}23:17: 1.0123456{tz}" }; // second / invalid
        yield return new object[] { $"2022-05-01{T}23:17:61.0123456{tz}" }; // second / invalid
        yield return new object[] { $"2022-05-01{T}23:17:14.01234567{tz}" }; // precision / overflow
        yield return new object[] { $"2022-05-01{T}23:17:14.{tz}" }; // lack of millisecond
        yield return new object[] { $"2022-05-01{T}23:17:{tz}" }; // lack of second
        yield return new object[] { $"2022-05-01{T}23:{tz}" }; // lack of minute
      }
    }

    yield return new object[] { "2022-05-01T" }; // lack of hour
    yield return new object[] { "2022-05-" }; // lack of day
    yield return new object[] { "2022-" }; // lack of month
  }

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTime(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTime(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTime_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTime(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTime_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTime(s.AsSpan(), out _));
#endif

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTimeOffset(s));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_ReadOnlySpanOfChar_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => W3CDateTimeFormats.ParseDateTimeOffset(s.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void TryParseDateTimeOffset_InvalidFormat(string s)
    => Assert.IsFalse(W3CDateTimeFormats.TryParseDateTimeOffset(s, out _));

#if SYSTEM_READONLYSPAN
  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
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
