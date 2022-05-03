// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

partial class DateTimeFormatTests {
  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindUtc()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 +0000",
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindLocal()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindUnspecified()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToRFC822DateTimeString_DateTime_TimeSeparator()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 +0000",
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTimeOffset()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dto)
    );
  }

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToRFC822DateTimeString_DateTimeOffset_TimeSeparator()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dto)
    );
  }

  [TestCaseSource(typeof(RFC822DateTimeFormatsTests), nameof(RFC822DateTimeFormatsTests.YieldTestCases_ParseDateTime))]
  public void FromRFC822DateTimeString(string s, DateTime expected)
    => Assert.AreEqual(expected, DateTimeFormat.FromRFC822DateTimeString(s));

  [TestCaseSource(typeof(RFC822DateTimeFormatsTests), nameof(RFC822DateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset))]
  public void FromRFC822DateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, DateTimeFormat.FromRFC822DateTimeOffsetString(s));
}
