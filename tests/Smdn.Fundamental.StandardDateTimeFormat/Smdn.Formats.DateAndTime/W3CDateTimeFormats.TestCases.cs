// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections;

namespace Smdn.Formats.DateAndTime;

#pragma warning disable IDE0040
partial class W3CDateTimeFormatsTests {
#pragma warning restore IDE0040
  private static IEnumerable YieldTestCases_Parse_InvalidFormat_Comprehensive()
  {
    foreach (var (_, input) in ISO8601DateTimeFormatsTests.YieldTestCases_Parse_InvalidFormat_All(iso8601: false)) {
      yield return new object[] { input };
    }
  }

  private static IEnumerable YieldTestCases_Parse_InvalidFormat()
  {
    foreach (var (level, input) in ISO8601DateTimeFormatsTests.YieldTestCases_Parse_InvalidFormat_All(iso8601: false)) {
      if (level == ISO8601DateTimeFormatsTests.TestCaseLevel.Essential)
        yield return new object[] { input };
    }
  }
}
