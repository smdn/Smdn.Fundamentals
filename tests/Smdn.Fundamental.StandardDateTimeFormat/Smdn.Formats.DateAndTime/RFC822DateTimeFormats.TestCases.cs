// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;

namespace Smdn.Formats.DateAndTime;

#pragma warning disable IDE0040
partial class RFC822DateTimeFormatsTests {
#pragma warning restore IDE0040
  private enum TestCaseLevel {
    Essential,
    Complemental,
  }

  private static IEnumerable YieldTestCases_ParseDateTimeOffset_All(bool comprehensive)
  {
    foreach (var (level, input, expectedDateTime, _) in YieldTestCases_Parse_All()) {
      if (!(level == TestCaseLevel.Essential || comprehensive))
        continue;
      yield return new object[] { input, expectedDateTime };
    }
  }

  private static IEnumerable YieldTestCases_ParseDateTime_All(bool comprehensive)
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
    foreach (var testCase in YieldTestCases_Parse_MilitaryTimeZones())
      yield return testCase;
    foreach (var testCase in YieldTestCases_Parse_NorthAmericanTimeZones())
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

    foreach (var (level, dayOfWeek) in new[] {
      (TestCaseLevel.Essential, "Fri, "),
      (TestCaseLevel.Complemental, string.Empty),
    }) {
      foreach (var (lv, year) in new[] {
        (level, "2001"),
        (TestCaseLevel.Complemental, "01"),
      }) {
        foreach (var zone in new[] { "GMT", "UT" }) {
          yield return (lv,                         $"{dayOfWeek}13 Apr {year} 19:23:02.1234567 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.Zero).AddTicks(4567), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:02.123456 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.Zero).AddTicks(4560), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:02.12345 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.Zero).AddTicks(4500), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:02.1234 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.Zero).AddTicks(4000), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:02.123 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:02.12 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 120, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:02.1 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 100, TimeSpan.Zero), expectedKind);
          yield return (lv,                         $"{dayOfWeek}13 Apr {year} 19:23:02 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23:2 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:23 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 0, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 19:2 {zone}", new DateTimeOffset(2001, 4, 13, 19, 2, 0, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 01:2 {zone}", new DateTimeOffset(2001, 4, 13, 1, 2, 0, TimeSpan.Zero), expectedKind);
          yield return (TestCaseLevel.Complemental, $"{dayOfWeek}13 Apr {year} 1:02 {zone}", new DateTimeOffset(2001, 4, 13, 1, 2, 0, TimeSpan.Zero), expectedKind);
          yield return (lv,                         $"{dayOfWeek}13 Apr {year} 1:2 {zone}", new DateTimeOffset(2001, 4, 13, 1, 2, 0, TimeSpan.Zero), expectedKind);
        }
      }
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset ExpectedDateTime,
    DateTimeKind ExpectedKind
  )> YieldTestCases_Parse_LocalTimes()
  {
    const DateTimeKind expectedKind = DateTimeKind.Local;

    foreach (var (level, dayOfWeek) in new[] {
      (TestCaseLevel.Essential, "Tue, "),
      (TestCaseLevel.Complemental, string.Empty),
    }) {
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.1234567 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4567), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.1234567 +09:00", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4567), expectedKind); // invalid but acceptable
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.123456 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4560), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.12345 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4500), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.1234 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4000), expectedKind);
      yield return (level,                      $"{dayOfWeek}10 Jun 2003 09:41:01.1234567 -0400", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(-4)).AddTicks(4567), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.1234567 +1245", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(12) + TimeSpan.FromMinutes(45)).AddTicks(4567), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.123 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.12 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 120, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:01.1 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 100, TimeSpan.FromHours(+9)), expectedKind);
      yield return (level,                      $"{dayOfWeek}10 Jun 2003 09:41:01 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41:1 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:41 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 0, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 09:4 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)), expectedKind);
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}10 Jun 2003 9:04 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)), expectedKind);
      yield return (level,                      $"{dayOfWeek}10 Jun 2003 9:4 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)), expectedKind);
      yield return (level,                      $"{dayOfWeek}10 Jun 03 9:4 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)), expectedKind);
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset ExpectedDateTime,
    DateTimeKind ExpectedKind
  )> YieldTestCases_Parse_MilitaryTimeZones()
  {
    var timeZoneAndTestCaseLevels = new[] {
      (TestCaseLevel.Essential,    "A"),
      (TestCaseLevel.Complemental, "B"),
      (TestCaseLevel.Complemental, "C"),
      (TestCaseLevel.Complemental, "D"),
      (TestCaseLevel.Complemental, "E"),
      (TestCaseLevel.Complemental, "F"),
      (TestCaseLevel.Complemental, "G"),
      (TestCaseLevel.Complemental, "H"),
      (TestCaseLevel.Complemental, "I"),
      (TestCaseLevel.Complemental, "K"),
      (TestCaseLevel.Complemental, "L"),
      (TestCaseLevel.Complemental, "M"),
      (TestCaseLevel.Complemental, "N"),
      (TestCaseLevel.Complemental, "O"),
      (TestCaseLevel.Complemental, "P"),
      (TestCaseLevel.Complemental, "Q"),
      (TestCaseLevel.Complemental, "R"),
      (TestCaseLevel.Complemental, "S"),
      (TestCaseLevel.Complemental, "T"),
      (TestCaseLevel.Complemental, "U"),
      (TestCaseLevel.Complemental, "V"),
      (TestCaseLevel.Complemental, "W"),
      (TestCaseLevel.Complemental, "X"),
      (TestCaseLevel.Complemental, "Y"),
      (TestCaseLevel.Essential,    "Z"),
      (TestCaseLevel.Complemental, "a"),
      (TestCaseLevel.Complemental, "b"),
      (TestCaseLevel.Complemental, "c"),
      (TestCaseLevel.Complemental, "d"),
      (TestCaseLevel.Complemental, "e"),
      (TestCaseLevel.Complemental, "f"),
      (TestCaseLevel.Complemental, "g"),
      (TestCaseLevel.Complemental, "h"),
      (TestCaseLevel.Complemental, "i"),
      (TestCaseLevel.Complemental, "k"),
      (TestCaseLevel.Complemental, "l"),
      (TestCaseLevel.Complemental, "m"),
      (TestCaseLevel.Complemental, "n"),
      (TestCaseLevel.Complemental, "o"),
      (TestCaseLevel.Complemental, "p"),
      (TestCaseLevel.Complemental, "q"),
      (TestCaseLevel.Complemental, "r"),
      (TestCaseLevel.Complemental, "s"),
      (TestCaseLevel.Complemental, "t"),
      (TestCaseLevel.Complemental, "u"),
      (TestCaseLevel.Complemental, "v"),
      (TestCaseLevel.Complemental, "w"),
      (TestCaseLevel.Complemental, "x"),
      (TestCaseLevel.Complemental, "y"),
      (TestCaseLevel.Essential,    "z"),
    };

    const DateTimeKind expectedKind = DateTimeKind.Unspecified;
    var expectedDateTime = new DateTimeOffset(2022, 4, 27, 20, 54, 1, 123, TimeSpan.FromHours(-0.0)).AddTicks(4567);

    foreach (var (level, tz) in timeZoneAndTestCaseLevels) {
      foreach (var (lv, dayOfWeek) in new[] {
        (level, "Wed, "),
        (TestCaseLevel.Complemental, string.Empty),
      }) {
        yield return (lv, $"{dayOfWeek}27 Apr 2022 20:54:01.1234567 {tz}", expectedDateTime, expectedKind);
      }
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input,
    DateTimeOffset Expected,
    DateTimeKind ExpectedKind
  )> YieldTestCases_Parse_NorthAmericanTimeZones()
  {
    static IEnumerable<(string Input, string Expected)> YieldTestCases()
    {
      yield return ("12 Jun 2006 11:00:00.0000000 EDT", "2006-06-12T11:00:00.0000000-04:00");
      yield return ("04 Nov 2007 01:00:00.0000000 EST", "2007-11-04T01:00:00.0000000-05:00");
      yield return ("10 Dec 2006 15:00:00.0000000 EST", "2006-12-10T15:00:00.0000000-05:00");
      yield return ("11 Mar 2007 02:30:00.0000000 EST", "2007-03-11T02:30:00.0000000-05:00");
      yield return ("12 Jun 2006 11:00:00.0000000 CDT", "2006-06-12T11:00:00.0000000-05:00");
      yield return ("04 Nov 2007 01:00:00.0000000 CST", "2007-11-04T01:00:00.0000000-06:00");
      yield return ("10 Dec 2006 15:00:00.0000000 CST", "2006-12-10T15:00:00.0000000-06:00");
      yield return ("11 Mar 2007 02:30:00.0000000 CST", "2007-03-11T02:30:00.0000000-06:00");
      yield return ("12 Jun 2006 11:00:00.0000000 MDT", "2006-06-12T11:00:00.0000000-06:00");
      yield return ("04 Nov 2007 01:00:00.0000000 MST", "2007-11-04T01:00:00.0000000-07:00");
      yield return ("10 Dec 2006 15:00:00.0000000 MST", "2006-12-10T15:00:00.0000000-07:00");
      yield return ("11 Mar 2007 02:30:00.0000000 MST", "2007-03-11T02:30:00.0000000-07:00");
      yield return ("12 Jun 2006 11:00:00.0000000 PDT", "2006-06-12T11:00:00.0000000-07:00");
      yield return ("04 Nov 2007 01:00:00.0000000 PST", "2007-11-04T01:00:00.0000000-08:00");
      yield return ("10 Dec 2006 15:00:00.0000000 PST", "2006-12-10T15:00:00.0000000-08:00");
      yield return ("11 Mar 2007 02:30:00.0000000 PST", "2007-03-11T02:30:00.0000000-08:00");
    }

    foreach (var (input, expected) in YieldTestCases()) {
      yield return (TestCaseLevel.Essential, input, DateTimeOffset.ParseExact(expected, "o", null), DateTimeKind.Utc);
    }
  }

  private static IEnumerable YieldTestCases_Parse_InvalidFormat_Comprehensive()
  {
    foreach (var (_, input) in YieldTestCases_Parse_InvalidFormat_All()) {
      yield return new object[] { input };
    }
  }

  private static IEnumerable YieldTestCases_Parse_InvalidFormat()
  {
    foreach (var (level, input) in YieldTestCases_Parse_InvalidFormat_All()) {
      if (level == TestCaseLevel.Essential)
        yield return new object[] { input };
    }
  }

  private static IEnumerable<(
    TestCaseLevel Level,
    string Input
  )> YieldTestCases_Parse_InvalidFormat_All()
  {
    yield return (TestCaseLevel.Essential, string.Empty); // empty string
    yield return (TestCaseLevel.Essential, "!"); // non-datetime string

    yield return (TestCaseLevel.Essential,    "Thu, 01 May 2022 23:17:14.0123456 +0000"); // day of week / mismatch
    yield return (TestCaseLevel.Complemental, "Nul, 01 May 2022 23:17:14.0123456 +0000"); // day of week / invalid
    yield return (TestCaseLevel.Complemental, "Su, 01 May 2022 23:17:14.0123456 +0000"); // day of week / invalid
    foreach (var (level, dayOfWeek) in new [] {
      (TestCaseLevel.Essential, "Sun, "),
      (TestCaseLevel.Complemental, string.Empty),
    }) {
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}0 May 2022 23:17:14.0123456 +0000"); // day / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}32 May 2022 23:17:14.0123456 +0000"); // day / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 Nul 2022 23:17:14.0123456 +0000"); // month / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 March 2022 23:17:14.0123456 +0000"); // month / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 022 23:17:14.0123456 +0000"); // year / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022 25:17:14.0123456 +0000"); // hour / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022 23:60:14.0123456 +0000"); // minute / invalid
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022 23:17:61.0123456 +0000"); // second / invalid
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14. +0000"); // second / invalid
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14.01234567 +0000"); // precision / overflow
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14.0123456 J"); // time zone / non-existent
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14.0123456 GM"); // time zone / invalid
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14.0123456 UTC"); // time zone / invalid
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14.0123456 JST"); // time zone / invalid
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:17:14.0123456 +00"); // time zone / invalid
#if false
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022 23:17:14.0123456 +00:00"); // time zone / invalid but acceptable
#endif
      yield return (level,                      $"{dayOfWeek}01 May 2022 23:"); // lack of minute
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022 23"); // lack of minute
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022 "); // lack of hour
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May 2022"); // lack of hour
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May "); // lack of year
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 May"); // lack of year
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01 "); // lack of month
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}01"); // lack of month
      yield return (TestCaseLevel.Complemental, $"{dayOfWeek}"); // lack of day
    }
  }
}
