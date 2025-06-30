// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;

namespace Smdn.Formats.DateAndTime;

#pragma warning disable IDE0040
partial class ISO8601DateTimeFormatsTests {
#pragma warning restore IDE0040
  internal enum TestCaseLevel {
    Essential,
    Complemental,
  }

  internal static IEnumerable YieldTestCases_ParseDateTimeOffset_All(bool comprehensive)
  {
    foreach (var (level, input, expectedDateTime, _) in YieldTestCases_Parse_All()) {
      if (!(level == TestCaseLevel.Essential || comprehensive))
        continue;
      yield return new object[] { input, expectedDateTime };
    }
  }

  internal static IEnumerable YieldTestCases_ParseDateTime_All(bool comprehensive)
  {
    static DateTime ToDateTime(DateTimeOffset dto, DateTimeKind kind)
      => kind switch {
        DateTimeKind.Utc => dto.UtcDateTime,
        DateTimeKind.Local => dto.LocalDateTime,
        _ => dto.DateTime, // DateTimeKind.Unspecified
      };

    foreach (var (level, input, expectedDateTime, expectedKind) in YieldTestCases_Parse_All()) {
      if (!(level == TestCaseLevel.Essential || comprehensive))
        continue;
      yield return new object[] { input, ToDateTime(expectedDateTime, expectedKind) };
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset ExpectedDateTime,
    DateTimeKind ExpectedKind
  )> YieldTestCases_Parse_All()
  {
    foreach (var testCase in YieldTestCases_Parse_UniversalTimes())
      yield return testCase;
    foreach (var testCase in YieldTestCases_Parse_LocalTimes())
      yield return testCase;
    foreach (var testCase in YieldTestCases_Parse_DateOnlyFormats())
      yield return testCase;
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset ExpectedDateTime,
    DateTimeKind ExpectedKind
  )>
  YieldTestCases_Parse_UniversalTimes()
  {
    const DateTimeKind expectedKind = DateTimeKind.Utc;

    foreach (var T in new[] { "T", " "}) {
      yield return (TestCaseLevel.Essential,    $"2008-04-11{T}12:34:56.7893333 Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3333), expectedKind);
      yield return (TestCaseLevel.Essential,    $"2008-04-11{T}12:34:56.7893333Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3333), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.789333Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3330), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.78933Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3300), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3000), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.789Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.78Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 780, TimeSpan.Zero), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 700, TimeSpan.Zero), expectedKind);
      yield return (TestCaseLevel.Essential,    $"2008-04-11{T}12:34:56Z", new DateTimeOffset(2008, 4, 11, 12, 34, 56, TimeSpan.Zero), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34Z", new DateTimeOffset(2008, 4, 11, 12, 34, 0, TimeSpan.Zero), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12Z", new DateTimeOffset(2008, 4, 11, 12, 0, 0, TimeSpan.Zero), expectedKind);
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset ExpectedDateTime,
    DateTimeKind ExpectedKind
  )>
  YieldTestCases_Parse_LocalTimes()
  {
    const DateTimeKind expectedKind = DateTimeKind.Local;

    foreach (var (level, T) in new[] {
      (TestCaseLevel.Essential, "T"),
      (TestCaseLevel.Complemental, " "),
    }) {
      yield return (level,                      $"2008-04-11{T}12:34:56.7893333-0400", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333), expectedKind); // invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893333 -0400", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333), expectedKind); // invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893333 +00:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3333), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893333 -00:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.Zero).AddTicks(3333), expectedKind); // invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893333-04:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893333 -04:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333), expectedKind);
      yield return (level,                      $"2008-04-11{T}12:34:56.7893333 +12:45", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(12) + TimeSpan.FromMinutes(45)).AddTicks(3333), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893333 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3333), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.789333 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3330), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.78933 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3300), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7893 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3000), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.789 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.78 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 780, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:56.7 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 700, TimeSpan.FromHours(+9)), expectedKind);
      yield return (level,                      $"2008-04-11{T}12:34:56 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"2008-04-11{T}12:34:05 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 5, TimeSpan.FromHours(+9)), expectedKind);
      yield return (level,                      $"2008-04-11{T}12:34 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 0, TimeSpan.FromHours(+9)), expectedKind);
      yield return (level,                      $"2008-04-11{T}12 +09:00", new DateTimeOffset(2008, 4, 11, 12, 0, 0, TimeSpan.FromHours(+9)), expectedKind);
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset ExpectedDateTime,
    DateTimeKind ExpectedKind
  )>
  YieldTestCases_Parse_DateOnlyFormats()
  {
    const DateTimeKind expectedKind = DateTimeKind.Unspecified;

    yield return (TestCaseLevel.Complemental, "2008-04-11 ", new DateTimeOffset(2008, 4, 11, 0, 0, 0, TimeSpan.Zero), expectedKind);
    yield return (TestCaseLevel.Essential,    "2008-04-11", new DateTimeOffset(2008, 4, 11, 0, 0, 0, TimeSpan.Zero), expectedKind);
    yield return (TestCaseLevel.Essential,    "2008-04", new DateTimeOffset(2008, 4, 1, 0, 0, 0, TimeSpan.Zero), expectedKind);
    yield return (TestCaseLevel.Essential,    "2008", new DateTimeOffset(2008, 1, 1, 0, 0, 0, TimeSpan.Zero), expectedKind);
  }

  private static IEnumerable YieldTestCases_Parse_InvalidFormat_Comprehensive()
  {
    foreach (var (_, input) in YieldTestCases_Parse_InvalidFormat_All(iso8601: true)) {
      yield return new object[] { input };
    }
  }

  private static IEnumerable YieldTestCases_Parse_InvalidFormat()
  {
    foreach (var (level, input) in YieldTestCases_Parse_InvalidFormat_All(iso8601: true)) {
      if (level == TestCaseLevel.Essential)
        yield return new object[] { input };
    }
  }

  internal static IEnumerable<(
    TestCaseLevel Level,
    string Input
  )>
  YieldTestCases_Parse_InvalidFormat_All(bool iso8601)
  {
    yield return (TestCaseLevel.Essential, string.Empty); // empty string
    yield return (TestCaseLevel.Essential, "!"); // non-datetime string

#if false
    yield return (TestCaseLevel.Complemental, "2022-05-01T23:17:14.0123456+00:00"); // VALID
#endif
    foreach (var T in new[] { "T", " " }) {
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456X"); // time zone / invalid
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456U"); // time zone / invalid
      yield return (TestCaseLevel.Essential,    $"2022-05-01{T}23:17:14.0123456UT"); // time zone / invalid
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456UTC"); // time zone / invalid
      yield return (TestCaseLevel.Essential,    $"2022-05-01{T}23:17:14.0123456+00"); // time zone / invalid
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456 +00"); // time zone / invalid
#if false
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456-00:00"); // time zone / invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456 -00:00"); // time zone / invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456+0000"); // time zone / invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:14.0123456 +0000"); // time zone / invalid but acceptable
#endif
    }

    foreach (var (level, tz) in new[] {
      (TestCaseLevel.Complemental, "Z"),
      (TestCaseLevel.Essential, "+00:00"),
      (TestCaseLevel.Complemental, string.Empty),
    }) {
      yield return (TestCaseLevel.Essential,    $"2022-05-01_23:17:14.0123456{tz}"); // date and time delimiter / invalid
      yield return (TestCaseLevel.Complemental, $"2022-05-01/23:17:14.0123456{tz}"); // date and time delimiter / invalid

      foreach (var (lv, T) in new[] {
        (level, "T"),
        (TestCaseLevel.Complemental, " "),
      }) {
        yield return (TestCaseLevel.Complemental, $"222-05-01{T}23:17:14.0123456{tz}"); // year / invalid
        yield return (TestCaseLevel.Complemental, $"22-05-01{T}23:17:14.0123456{tz}"); // year / invalid
        yield return (TestCaseLevel.Complemental, $"2-05-01{T}23:17:14.0123456{tz}"); // year / invalid
        yield return (TestCaseLevel.Complemental, $"2022-0-01{T}23:17:14.0123456{tz}"); // month / invalid
        yield return (TestCaseLevel.Complemental, $"2022-5-01{T}23:17:14.0123456{tz}"); // month / invalid
        yield return (TestCaseLevel.Complemental, $"2022-13-01{T}23:17:14.0123456{tz}"); // month / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-0{T}23:17:14.0123456{tz}"); // day / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-1{T}23:17:14.0123456{tz}"); // day / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-32{T}23:17:14.0123456{tz}"); // day / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}25:17:14.0123456{tz}"); // hour / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T} 3:17:14.0123456{tz}"); // hour / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}3:17:14.0123456{tz}"); // hour / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23: 7:14.0123456{tz}"); // minute / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:7:14.0123456{tz}"); // minute / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:60:14.0123456{tz}"); // minute / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:1.0123456{tz}"); // second / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17: 1.0123456{tz}"); // second / invalid
        yield return (TestCaseLevel.Complemental, $"2022-05-01{T}23:17:61.0123456{tz}"); // second / invalid
        yield return (lv,                         $"2022-05-01{T}23:17:14.01234567{tz}"); // precision / overflow
        yield return (lv,                         $"2022-05-01{T}23:17:14.{tz}"); // lack of millisecond
        yield return (lv,                         $"2022-05-01{T}23:17:{tz}"); // lack of second
        yield return (lv,                         $"2022-05-01{T}23:{tz}"); // lack of minute
      }
    }

    if (iso8601) {
      foreach (var (level, tz) in new[] {
        (TestCaseLevel.Complemental, "Z"),
        (TestCaseLevel.Essential, "+00:00"),
        (TestCaseLevel.Complemental, string.Empty),
      }) {
        foreach (var (lv, T) in new[] {
          (level, "T"),
          (TestCaseLevel.Complemental, " "),
        }) {
          yield return (lv, $"2022-W17-07{T}23:17:14.0123456{tz}"); // not supported (ISO week)
          yield return (lv, $"令04.05.01{T}23:17:14.0123456{tz}"); // not supported (JIS X 0301)
          yield return (lv, $"R04.05.01{T}23:17:14.0123456{tz}"); // not supported (JIS X 0301)
        }
      }
    }

    yield return (TestCaseLevel.Essential, "2022-05-01T"); // lack of hour
    yield return (TestCaseLevel.Essential, "2022-05-"); // lack of day
    yield return (TestCaseLevel.Essential, "2022-"); // lack of month
  }
}
