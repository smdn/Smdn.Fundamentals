// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class StringConversionTests {
    [Test]
    public void TestToEnum()
    {
      Assert.That(StringConversion.ToEnum<DayOfWeek>("Sunday"), Is.EqualTo(DayOfWeek.Sunday));
      Assert.That(StringConversion.ToEnum<DayOfWeek>("Monday"), Is.EqualTo(DayOfWeek.Monday));
      Assert.That(StringConversion.ToEnum<DayOfWeek>("Tuesday"), Is.EqualTo(DayOfWeek.Tuesday));
      Assert.That(StringConversion.ToEnum<DayOfWeek>("Wednesday"), Is.EqualTo(DayOfWeek.Wednesday));

      Assert.That(StringConversion.ToEnum<DayOfWeek>("sUndaY", true), Is.EqualTo(DayOfWeek.Sunday));
      Assert.That(StringConversion.ToEnumIgnoreCase<DayOfWeek>("sUndaY"), Is.EqualTo(DayOfWeek.Sunday));

      Assert.That(() => StringConversion.ToEnum<DayOfWeek>("sUndaY"), Throws.ArgumentException, "#1");
      Assert.That(() => StringConversion.ToEnum<DayOfWeek>("sUndaY", false), Throws.ArgumentException, "#2");
    }
  }
}
