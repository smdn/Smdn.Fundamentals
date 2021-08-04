// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP3_1
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

    [Test]
    public void TestToString_IEnumerable()
    {
      var str = StringConversion.ToString(typeof(void), new (string, object)[] {
        ("e1", new[] { 0, 1, 2 }),
        ("e2", new List<int> { 0, 1, 2 }),
      });

      Assert.AreEqual("{Void: e1=['0', '1', '2'], e2=['0', '1', '2']}", str);
    }

    [Test]
    public void TestToString_IEnumerable_Empty()
    {
      var str = StringConversion.ToString(typeof(void), new (string, object)[] {
        ("e", new List<int>()),
      });

      Assert.AreEqual("{Void: e=[]}", str);
    }

    [Test]
    public void TestToString_KeyValuePair()
    {
      var str = StringConversion.ToString(typeof(void), new (string, object)[] {
        ("p1", KeyValuePair.Create("x", 1)),
        ("p2", KeyValuePair.Create(2, "y")),
        ("p3", KeyValuePair.Create((string)null, KeyValuePair.Create("key", (string)null))),
      });

      Assert.AreEqual("{Void: p1={'x' => '1'}, p2={'2' => 'y'}, p3={(null) => {'key' => (null)}}}", str);
    }

    [Test]
    public void TestToString_IEnumerableOfKeyValuePair()
    {
      var str = StringConversion.ToString(typeof(void), new (string, object)[] {
        ("p1", new[] {
          KeyValuePair.Create("x", 1),
          KeyValuePair.Create("y", 2),
          KeyValuePair.Create("z", 3),
        }),
        ("p2", new Dictionary<string, int> {
          { "x", 1 },
          { "y", 2 },
          { "z", 3 },
        }),
      });

      Assert.AreEqual("{Void: p1=[{'x' => '1'}, {'y' => '2'}, {'z' => '3'}], p2=[{'x' => '1'}, {'y' => '2'}, {'z' => '3'}]}", str);
    }

    [Test]
    public void TestToString_IEnumerable_ContainsNullOrEmpty()
    {
      var str = StringConversion.ToString(typeof(void), new (string, object)[] {
        ("e", new[] { "x", null, "" }),
        ("p", new Dictionary<int, string> {
          { 0, "x" },
          { 1, null },
          { 2, "" },
        }),
      });

      Assert.AreEqual("{Void: e=['x', (null), ''], p=[{'0' => 'x'}, {'1' => (null)}, {'2' => ''}]}", str);
    }
  }
}
