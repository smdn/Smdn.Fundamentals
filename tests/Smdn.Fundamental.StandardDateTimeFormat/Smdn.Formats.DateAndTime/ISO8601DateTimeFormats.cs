// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

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
}
