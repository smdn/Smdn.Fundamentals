using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#if NETCOREAPP2_0
using KeyValuePair = System.Collections.Generic.KeyValuePair;
#else
using KeyValuePair = Smdn.Collections.KeyValuePair;
#endif

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

      Assert.Throws<ArgumentException>(() => StringConversion.ToEnum<DayOfWeek>("sUndaY"), "#1");
      Assert.Throws<ArgumentException>(() => StringConversion.ToEnum<DayOfWeek>("sUndaY", false), "#2");
    }

    private class TestObject1 {
      public int MI = 42;
      public string MS1 = null;
      public string MS2 = string.Empty;
      public string MS3 = "foo";

      public override string ToString()
      {
        return StringConversion.ToString(GetType(), new(string, object)[] {
          (nameof(MI), MI),
          (nameof(MS1), MS1),
          (nameof(MS2), MS2),
          (nameof(MS3), MS3),
        });
      }
    }

    private class TestObject_Empty {
      public override string ToString()
      {
        return StringConversion.ToString(GetType(), Enumerable.Empty<(string, object)>());
      }
    }

    private class TestObject_Null {
      public override string ToString()
      {
        return StringConversion.ToString(GetType(), null);
      }
    }

    [Test]
    public void TestToString_Object()
    {
      Assert.AreEqual("{TestObject1: MI='42', MS1=(null), MS2='', MS3='foo'}", new TestObject1().ToString());
      Assert.AreEqual("{TestObject_Empty: }", new TestObject_Empty().ToString());
    }

    [Test]
    public void TestToString_Object_NameAndValuePairsNull()
    {
      Assert.AreEqual("{TestObject_Null}", new TestObject_Null().ToString());
    }
  }
}
