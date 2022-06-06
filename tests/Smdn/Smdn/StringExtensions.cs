// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class StringExtensionsTests {
    [Test]
    public void TestCount()
    {
      Assert.AreEqual(0, "abcdefg".Count("abcdefgh"));
      Assert.AreEqual(1, "abcdefg".Count("abcdefg"));
      Assert.AreEqual(1, "abcdefg".Count("abcdef"));

      Assert.AreEqual(2, "xxyyxyyxx".Count("xx"));
      Assert.AreEqual(2, "xxyyxyyxx".Count("xy"));
      Assert.AreEqual(0, "xxyyxyyxx".Count("xxx"));

      Assert.AreEqual(5, "xxyyxyyxx".Count('x'));
      Assert.AreEqual(4, "xxyyxyyxx".Count('y'));
    }

    [Test]
    public void TestSlice()
    {
      Assert.AreEqual("abc", "abcdef".Slice(0, 3));
      Assert.AreEqual("cd", "abcdef".Slice(2, 4));
      Assert.AreEqual("de", "abcdef".Slice(3, 5));
      Assert.AreEqual("", "abcdef".Slice(0, 0));
      Assert.AreEqual("abcdef", "abcdef".Slice(0, 6));
      Assert.AreEqual("f", "abcdef".Slice(5, 6));
    }

    [Test]
    public void TestSlice_ArgumentOutOfRange()
    {
      ArgumentOutOfRangeException ex;

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(-1, 0), "#1");

      Assert.AreEqual("from", ex!.ParamName, "#1");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(3, 4), "#2");

      Assert.AreEqual("from", ex!.ParamName, "#2");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(1, 0), "#3");

      Assert.AreEqual("to", ex!.ParamName, "#3");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(0, 4), "#4");

      Assert.AreEqual("to", ex!.ParamName, "#4");
    }

    [Test]
    public void TestIndexOfNot()
    {
      Assert.AreEqual(2, "aabbcc".IndexOfNot('a'));
      Assert.AreEqual(0, "aabbcc".IndexOfNot('b'));
      Assert.AreEqual(-1, "cccccc".IndexOfNot('c'));

      Assert.AreEqual(2, "aabb".IndexOfNot('a', 0));
      Assert.AreEqual(2, "aabb".IndexOfNot('a', 1));
      Assert.AreEqual(2, "aabb".IndexOfNot('a', 2));
      Assert.AreEqual(3, "aabb".IndexOfNot('a', 3));

      Assert.AreEqual(-1, "aaaa".IndexOfNot('a', 2));
      Assert.AreEqual(-1, "aaaa".IndexOfNot('a', 4));

      var ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".IndexOfNot('a', -1));

      Assert.AreEqual("startIndex", ex!.ParamName, "#1");

      Assert.Throws<ArgumentException>(() => "abc".IndexOfNot('a', 4));
    }
  }
}
