// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#if SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
using _KeyValuePair = System.Collections.Generic.KeyValuePair;
#else
using Smdn.Collections;
using _KeyValuePair = Smdn.Collections.KeyValuePair;
#endif

namespace Smdn {
  [TestFixture]
  public class StringificationTests {
    private class TestObject1 {
      public int MI = 42;
      public string MS1 = null;
      public string MS2 = string.Empty;
      public string MS3 = "foo";

      public override string ToString()
      {
        return Stringification.Stringify(GetType(), new(string, object)[] {
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
        return Stringification.Stringify(GetType(), Enumerable.Empty<(string, object)>());
      }
    }

    private class TestObject_Null {
      public override string ToString()
      {
        return Stringification.Stringify(GetType(), null);
      }
    }

    [Test]
    public void Stringify_Object()
    {
      Assert.AreEqual("{TestObject1: MI='42', MS1=(null), MS2='', MS3='foo'}", new TestObject1().ToString());
      Assert.AreEqual("{TestObject_Empty: }", new TestObject_Empty().ToString());
    }

    [Test]
    public void Stringify_Object_NameAndValuePairsNull()
    {
      Assert.AreEqual("{TestObject_Null}", new TestObject_Null().ToString());
    }

    [Test]
    public void Stringify_IEnumerable()
    {
      var str = Stringification.Stringify(typeof(void), new (string, object)[] {
        ("e1", new[] { 0, 1, 2 }),
        ("e2", new List<int> { 0, 1, 2 }),
      });

      Assert.AreEqual("{Void: e1=['0', '1', '2'], e2=['0', '1', '2']}", str);
    }

    [Test]
    public void Stringify_IEnumerable_Empty()
    {
      var str = Stringification.Stringify(typeof(void), new (string, object)[] {
        ("e", new List<int>()),
      });

      Assert.AreEqual("{Void: e=[]}", str);
    }

    [Test]
    public void Stringify_KeyValuePair()
    {
      var str = Stringification.Stringify(typeof(void), new (string, object)[] {
        ("p1", _KeyValuePair.Create("x", 1)),
        ("p2", _KeyValuePair.Create(2, "y")),
        ("p3", _KeyValuePair.Create((string)null, _KeyValuePair.Create("key", (string)null))),
      });

      Assert.AreEqual("{Void: p1={'x' => '1'}, p2={'2' => 'y'}, p3={(null) => {'key' => (null)}}}", str);
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair()
    {
      var str = Stringification.Stringify(typeof(void), new (string, object)[] {
        ("p1", new[] {
          _KeyValuePair.Create("x", 1),
          _KeyValuePair.Create("y", 2),
          _KeyValuePair.Create("z", 3),
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
    public void Stringify_IEnumerable_ContainsNullOrEmpty()
    {
      var str = Stringification.Stringify(typeof(void), new (string, object)[] {
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