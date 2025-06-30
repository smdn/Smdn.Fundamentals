// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormatTests {
#pragma warning restore IDE0040
  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindUtc()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.That(
      DateTimeFormat.ToRFC822DateTimeString(dtm),
      Is.EqualTo("Mon, 25 Feb 2008 15:01:12 +0000")
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindLocal()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

    Assert.That(
      DateTimeFormat.ToRFC822DateTimeString(dtm),
      Is.EqualTo("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindUnspecified()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

    Assert.That(
      DateTimeFormat.ToRFC822DateTimeString(dtm),
      Is.EqualTo("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim)
    );
  }

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToRFC822DateTimeString_DateTime_TimeSeparator()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.That(
      DateTimeFormat.ToRFC822DateTimeString(dtm),
      Is.EqualTo("Mon, 25 Feb 2008 15:01:12 +0000")
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTimeOffset()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

    Assert.That(
      DateTimeFormat.ToRFC822DateTimeString(dto),
      Is.EqualTo("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim)
    );
  }

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToRFC822DateTimeString_DateTimeOffset_TimeSeparator()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

    Assert.That(
      DateTimeFormat.ToRFC822DateTimeString(dto),
      Is.EqualTo("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim)
    );
  }

  [TestCaseSource(typeof(RFC822DateTimeFormatsTests), nameof(RFC822DateTimeFormatsTests.YieldTestCases_ParseDateTime))]
  public void FromRFC822DateTimeString(string s, DateTime expected)
    => Assert.That(DateTimeFormat.FromRFC822DateTimeString(s), Is.EqualTo(expected));

  [TestCaseSource(typeof(RFC822DateTimeFormatsTests), nameof(RFC822DateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset))]
  public void FromRFC822DateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.That(DateTimeFormat.FromRFC822DateTimeOffsetString(s), Is.EqualTo(expected));
}
