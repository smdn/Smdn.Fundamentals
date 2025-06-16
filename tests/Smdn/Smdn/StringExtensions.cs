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
      Assert.That("abcdefg".Count("abcdefgh"), Is.Zero);
      Assert.That("abcdefg".Count("abcdefg"), Is.EqualTo(1));
      Assert.That("abcdefg".Count("abcdef"), Is.EqualTo(1));

      Assert.That("xxyyxyyxx".Count("xx"), Is.EqualTo(2));
      Assert.That("xxyyxyyxx".Count("xy"), Is.EqualTo(2));
      Assert.That("xxyyxyyxx".Count("xxx"), Is.Zero);

      Assert.That("xxyyxyyxx".Count('x'), Is.EqualTo(5));
      Assert.That("xxyyxyyxx".Count('y'), Is.EqualTo(4));
    }

    [Test]
    public void TestSlice()
    {
      Assert.That("abcdef".Slice(0, 3), Is.EqualTo("abc"));
      Assert.That("abcdef".Slice(2, 4), Is.EqualTo("cd"));
      Assert.That("abcdef".Slice(3, 5), Is.EqualTo("de"));
      Assert.That("abcdef".Slice(0, 0), Is.EqualTo(""));
      Assert.That("abcdef".Slice(0, 6), Is.EqualTo("abcdef"));
      Assert.That("abcdef".Slice(5, 6), Is.EqualTo("f"));
    }

    [Test]
    public void TestSlice_ArgumentOutOfRange()
    {
      ArgumentOutOfRangeException ex;

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(-1, 0), "#1");

      Assert.That(ex!.ParamName, Is.EqualTo("from"), "#1");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(3, 4), "#2");

      Assert.That(ex!.ParamName, Is.EqualTo("from"), "#2");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(1, 0), "#3");

      Assert.That(ex!.ParamName, Is.EqualTo("to"), "#3");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(0, 4), "#4");

      Assert.That(ex!.ParamName, Is.EqualTo("to"), "#4");
    }

    [Test]
    public void TestIndexOfNot()
    {
      Assert.That("aabbcc".IndexOfNot('a'), Is.EqualTo(2));
      Assert.That("aabbcc".IndexOfNot('b'), Is.Zero);
      Assert.That("cccccc".IndexOfNot('c'), Is.EqualTo(-1));

      Assert.That("aabb".IndexOfNot('a', 0), Is.EqualTo(2));
      Assert.That("aabb".IndexOfNot('a', 1), Is.EqualTo(2));
      Assert.That("aabb".IndexOfNot('a', 2), Is.EqualTo(2));
      Assert.That("aabb".IndexOfNot('a', 3), Is.EqualTo(3));

      Assert.That("aaaa".IndexOfNot('a', 2), Is.EqualTo(-1));
      Assert.That("aaaa".IndexOfNot('a', 4), Is.EqualTo(-1));

      var ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".IndexOfNot('a', -1));

      Assert.That(ex!.ParamName, Is.EqualTo("startIndex"), "#1");

      Assert.Throws<ArgumentException>(() => "abc".IndexOfNot('a', 4));
    }
  }
}
