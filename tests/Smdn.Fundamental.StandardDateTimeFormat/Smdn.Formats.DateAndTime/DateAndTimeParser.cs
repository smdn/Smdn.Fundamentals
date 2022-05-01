// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public class DateAndTimeParserTests {
  private static IEnumerable YieldTestCases_ParseCommon_Invalid()
  {
    yield return new object[] { null, typeof(ArgumentNullException) };
    yield return new object[] { string.Empty, typeof(FormatException) };
    yield return new object[] { "x", typeof(FormatException) };
  }

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_Invalid))]
  public void ParseDateTime_Invalid(string s, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => DateTimeFormat.FromISO8601DateTimeString(s));

  [TestCaseSource(nameof(YieldTestCases_ParseCommon_Invalid))]
  public void ParseDateTimeOffset_Invalid(string s, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => DateTimeFormat.FromISO8601DateTimeOffsetString(s));
}
