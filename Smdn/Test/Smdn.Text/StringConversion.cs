using System;
using System.Collections.Generic;
using Smdn.Collections;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class StringConversionTests {
    [Test]
    public void TestToJoinedStringArgumentSingle()
    {
      var pairs = new[] {KeyValuePair.Create("key", "value")};

      Assert.AreEqual("{key => value}", StringConversion.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringArgumentMultiple()
    {
      var pairs = new[] {
        KeyValuePair.Create("key1", "value1"),
        KeyValuePair.Create("key2", "value2"),
      };

      Assert.AreEqual("{key1 => value1}, {key2 => value2}", StringConversion.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringKeyOrValueNull()
    {
      var pairs = new[] {
        KeyValuePair.Create("key1", (string)null),
        KeyValuePair.Create((string)null, "value2"),
        KeyValuePair.Create((string)null, (string)null),
      };

      Assert.AreEqual("{key1 => }, { => value2}, { => }", StringConversion.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringArgumentEmpty()
    {
      var pairs = new KeyValuePair<string, string>[] { };

      Assert.IsEmpty(StringConversion.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringArgumentNull()
    {
      IEnumerable<KeyValuePair<string, string>> pairs = null;

      Assert.IsNull(StringConversion.ToJoinedString(pairs));
    }

    [Test]
    public void TestToEnum()
    {
      Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnum<DayOfWeek>("Sunday"));
      Assert.AreEqual(DayOfWeek.Monday, StringConversion.ToEnum<DayOfWeek>("Monday"));
      Assert.AreEqual(DayOfWeek.Tuesday, StringConversion.ToEnum<DayOfWeek>("Tuesday"));
      Assert.AreEqual(DayOfWeek.Wednesday, StringConversion.ToEnum<DayOfWeek>("Wednesday"));

      Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnum<DayOfWeek>("sUndaY", true));
      Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnumIgnoreCase<DayOfWeek>("sUndaY"));

      try {
        Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnum<DayOfWeek>("sUndaY"));
        Assert.Fail("exception not thrown");
      }
      catch {
      }

      try {
        Assert.AreEqual(DayOfWeek.Sunday, StringConversion.ToEnum<DayOfWeek>("sUndaY", false));
        Assert.Fail("exception not thrown");
      }
      catch {
      }
    }
  }
}