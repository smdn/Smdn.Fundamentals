// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class StringConversionTests {
    [Test]
    public void TestToEnum()
    {
      Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnum<DayOfWeek>("Sunday"));
      Assert.AreEqual(DayOfWeek.Monday, StringConversion.ToEnum<DayOfWeek>("Monday"));
      Assert.AreEqual(DayOfWeek.Tuesday, StringConversion.ToEnum<DayOfWeek>("Tuesday"));
      Assert.AreEqual(DayOfWeek.Wednesday, StringConversion.ToEnum<DayOfWeek>("Wednesday"));

      Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnum<DayOfWeek>("sUndaY", true));
      Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnumIgnoreCase<DayOfWeek>("sUndaY"));

      Assert.Throws<ArgumentException>(() => StringConversion.ToEnum<DayOfWeek>("sUndaY"), "#1");
      Assert.Throws<ArgumentException>(() => StringConversion.ToEnum<DayOfWeek>("sUndaY", false), "#2");
    }
  }
}
