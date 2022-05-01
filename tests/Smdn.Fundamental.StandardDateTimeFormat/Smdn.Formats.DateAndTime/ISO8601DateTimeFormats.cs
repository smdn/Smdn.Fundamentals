// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_GLOBALIZATION_ISOWEEK
#endif
#if NET6_0_OR_GREATER
#define SYSTEM_DATEONLY
#endif

using System;
using System.Collections;
#if SYSTEM_GLOBALIZATION_ISOWEEK
using System.Globalization;
#endif

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public class ISO8601DateTimeFormatsTests {
  internal static IEnumerable YieldTestCases_ParseCommon_InvalidFormat()
  {
    foreach (var testCase in W3CDateTimeFormatsTests.YieldTestCases_ParseCommon_InvalidFormat()) {
      yield return testCase;
    }

    foreach (var tz in new[] { "Z", "+00:00", string.Empty }) {
      foreach (var T in new[] { "T", " " }) {
        yield return new object[] { $"2022-W17-07{T}23:17:14.0123456{tz}" }; // not supported (ISO week)
        yield return new object[] { $"令04.05.01{T}23:17:14.0123456{tz}" }; // not supported (JIS X 0301)
        yield return new object[] { $"R04.05.01{T}23:17:14.0123456{tz}" }; // not supported (JIS X 0301)
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => DateTimeFormat.FromISO8601DateTimeString(s)); // TODO: test ISO8601DateTimeFormats.ParseDateTime

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => DateTimeFormat.FromISO8601DateTimeOffsetString(s)); // TODO: test ISO8601DateTimeFormats.ParseDateTimeOffset

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
