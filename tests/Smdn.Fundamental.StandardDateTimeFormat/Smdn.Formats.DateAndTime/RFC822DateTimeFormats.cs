// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public class RFC822DateTimeFormatsTests {
  internal static IEnumerable YieldTestCases_ParseCommon_InvalidFormat()
  {
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
    => Assert.Throws<FormatException>(() => DateTimeFormat.FromRFC822DateTimeString(s)); // TODO: test RFC822DateTimeFormats.ParseDateTime

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_InvalidFormat))]
  public void ParseDateTimeOffset_InvalidFormat(string s)
    => Assert.Throws<FormatException>(() => DateTimeFormat.FromRFC822DateTimeOffsetString(s)); // TODO: test RFC822DateTimeFormats.ParseDateTimeOffset
}
