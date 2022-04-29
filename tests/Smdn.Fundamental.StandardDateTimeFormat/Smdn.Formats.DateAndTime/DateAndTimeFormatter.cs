// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.DateAndTime;

[TestFixture()]
public class DateAndTimeFormatterTests {

  private static IEnumerable YieldTestCases_FormatOffset()
  {
    foreach (var delimiter in new[] { true, false }) {
      var d = delimiter ? ":" : "";

      yield return new object[] { TimeSpan.Zero, delimiter, $"+00{d}00" };
      yield return new object[] { TimeSpan.FromHours(+0.0), delimiter, $"+00{d}00" };
      yield return new object[] { TimeSpan.FromHours(-0.0), delimiter, $"+00{d}00" };
      yield return new object[] { TimeSpan.FromTicks(-1), delimiter, $"-00{d}00" };
      yield return new object[] { TimeSpan.FromHours(-12.0), delimiter, $"-12{d}00" };
      yield return new object[] { TimeSpan.FromHours(+14.0), delimiter, $"+14{d}00" };
      yield return new object[] { TimeSpan.FromHours(8.0).Add(TimeSpan.FromMinutes(45.0)), delimiter, $"+08{d}45" };
      yield return new object[] { TimeSpan.FromHours(-3.0).Subtract(TimeSpan.FromMinutes(30.0)), delimiter, $"-03{d}30" };
      yield return new object[] { TimeSpan.FromHours(9.0), delimiter, $"+09{d}00" };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FormatOffset))]
  public void FormatOffset(TimeSpan offset, bool delimiter, string expected)
    => Assert.AreEqual(expected, DateAndTimeFormatter.FormatOffset(offset, delimiter));
}
