// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public class W3CDateTimeFormatsTests {
  internal static IEnumerable YieldTestCases_ParseCommon_InvalidFormat()
  {
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
        yield return new object[] { $"2022-05-01{T}" }; // lack of hour
        yield return new object[] { $"2022-05-" }; // lack of day
        yield return new object[] { $"2022-05" }; // lack of day
        yield return new object[] { $"2022-" }; // lack of month
        yield return new object[] { $"2022" }; // lack of month
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTime_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => DateTimeFormat.FromW3CDateTimeString(s)); // TODO: test W3CDateTimeFormats.ParseDateTime

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => DateTimeFormat.FromW3CDateTimeOffsetString(s)); // TODO: test W3CDateTimeFormats.ParseDateTimeOffset
}
