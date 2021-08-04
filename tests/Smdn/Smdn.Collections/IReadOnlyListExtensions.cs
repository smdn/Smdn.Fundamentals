// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class IReadOnlyListExtensionsTests {
    [Test]
    public void TestSlice_Array()
    {
      TestSlice(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
    }

    [Test]
    public void TestSlice_ArraySegment()
    {
      TestSlice(new ArraySegment<int>(new[] { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 1, 10));
    }

    [Test]
    public void TestSlice_List()
    {
      TestSlice(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
    }

    private void TestSlice(IReadOnlyList<int> list)
    {
      Assert.AreEqual(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, list.Slice(0));
      Assert.AreEqual(new[] { 0, 1, 2 }, list.Slice(0, 3));
      Assert.AreEqual(new[] { 7, 8, 9 }, list.Slice(7, 3));
      Assert.AreEqual(new[] { 2, 3, 4, 5, 6 }, list.Slice(2, 5));
      Assert.AreEqual(new[] { 6, 7, 8, 9 }, list.Slice(6));
      Assert.AreEqual(new int[] { }, list.Slice(0, 0));
      Assert.AreEqual(new int[] { }, list.Slice(10));
      Assert.AreEqual(new int[] { }, list.Slice(10, 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => list.Slice(-1, 1), "argument exception #1");
      Assert.Throws<ArgumentOutOfRangeException>(() => list.Slice(1, -1), "argument exception #2");
      Assert.Throws<ArgumentException>(() => list.Slice(10, 1), "argument exception #3");
      Assert.Throws<ArgumentException>(() => list.Slice(1, 10), "argument exception #4");
      Assert.Throws<ArgumentException>(() => list.Slice(11, 0), "argument exception #5");
      Assert.Throws<ArgumentException>(() => list.Slice(0, 11), "argument exception #6");

      // test IReadOnlyList.Count
      Assert.AreEqual(10, list.Slice(0).Count);
      Assert.AreEqual(3, list.Slice(0, 3).Count);
      Assert.AreEqual(4, list.Slice(6).Count);
      Assert.AreEqual(0, list.Slice(10).Count);

      // test IReadOnlyList.this[]
      Assert.AreEqual(2, list.Slice(2, 3)[0]);
      Assert.AreEqual(3, list.Slice(2, 3)[1]);
      Assert.AreEqual(4, list.Slice(2, 3)[2]);
      Assert.Throws<ArgumentOutOfRangeException>(() => Assert.IsNotNull(list.Slice(2, 3)[-1]));
      Assert.Throws<ArgumentOutOfRangeException>(() => Assert.IsNotNull(list.Slice(2, 3)[3]));

      Assert.Throws<ArgumentOutOfRangeException>(() => Assert.IsNotNull(list.Slice(0, 0)[0]));

      // test enumerator
      Assert.AreEqual(new[] { 0, 1, 2 }, list.Slice(0, 3).ToArray());
      Assert.AreEqual(new[] { 7, 8, 9 }, list.Slice(7, 3).ToArray());
      Assert.AreEqual(new int[] { }, list.Slice(0, 0).ToArray());
      Assert.AreEqual(new int[] { }, list.Slice(10).ToArray());
    }

    [Test]
    public void TestIndexOf_Array() => TestIndexOf<ArgumentException>(new int[] { 1, 1, 2, 1 });

    [Test]
    public void TestIndexOf_List() => TestIndexOf<ArgumentOutOfRangeException>(new List<int> { 1, 1, 2, 1 });

    private void TestIndexOf<TExpectedArgumentException> (IReadOnlyList<int> list) where TExpectedArgumentException : ArgumentException
    {
      Assert.AreEqual(0, list.IndexOf(item: 1));
      Assert.AreEqual(2, list.IndexOf(item: 2));
      Assert.AreEqual(-1, list.IndexOf(item: 0));
      Assert.AreEqual(0, list.IndexOf(item: 1, 0));
      Assert.AreEqual(1, list.IndexOf(item: 1, 1));
      Assert.AreEqual(3, list.IndexOf(item: 1, 2));
      Assert.AreEqual(3, list.IndexOf(item: 1, 3));
      Assert.AreEqual(2, list.IndexOf(item: 2, 0));
      Assert.AreEqual(2, list.IndexOf(item: 2, 1));
      Assert.AreEqual(2, list.IndexOf(item: 2, 2));
      Assert.AreEqual(-1, list.IndexOf(item: 2, 3));
      Assert.AreEqual(-1, list.IndexOf(item: 2, 0, 1));
      Assert.AreEqual(-1, list.IndexOf(item: 2, 0, 2));
      Assert.AreEqual(2, list.IndexOf(item: 2, 0, 3));
      Assert.AreEqual(2, list.IndexOf(item: 2, 0, 4));
      Assert.AreEqual(-1, list.IndexOf(item: 2, 4, 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(default, -1));
      Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(default, 5));

      Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(default, 0, -1));
      Assert.Throws<TExpectedArgumentException>(() => list.IndexOf(default, 0, 5)); // List<T>.IndexOf() throws ArgumentOutOfRangeException
      Assert.Throws<TExpectedArgumentException>(() => list.IndexOf(default, 1, 4)); // List<T>.IndexOf() throws ArgumentOutOfRangeException
      Assert.Throws<TExpectedArgumentException>(() => list.IndexOf(default, 4, 1)); // List<T>.IndexOf() throws ArgumentOutOfRangeException
    }

    [Test]
    public void TestIndexOf_WithEqualityComparer_Array() => TestIndexOf_WithEqualityComparer(new string[] { "A", "B", "c", "C" });

    [Test]
    public void TestIndexOf_WithEqualityComparer_List() => TestIndexOf_WithEqualityComparer(new List<string> { "A", "B", "c", "C" });

    private void TestIndexOf_WithEqualityComparer(IReadOnlyList<string> list)
    {
      var equalityComparer = StringComparer.OrdinalIgnoreCase;

      Assert.AreEqual(0, list.IndexOf(item: "A", equalityComparer));
      Assert.AreEqual(0, list.IndexOf(item: "a", equalityComparer));
      Assert.AreEqual(1, list.IndexOf(item: "B", equalityComparer));
      Assert.AreEqual(1, list.IndexOf(item: "b", equalityComparer));
      Assert.AreEqual(2, list.IndexOf(item: "C", equalityComparer));
      Assert.AreEqual(2, list.IndexOf(item: "c", equalityComparer));
      Assert.AreEqual(3, list.IndexOf(item: "C", index: 3, equalityComparer));
      Assert.AreEqual(3, list.IndexOf(item: "c", index: 3, equalityComparer));
      Assert.AreEqual(-1, list.IndexOf(item: "X", equalityComparer));
      Assert.AreEqual(-1, list.IndexOf(item: "x", equalityComparer));

      Assert.AreEqual(1, list.IndexOf(item: "B", StringComparer.Ordinal));
      Assert.AreEqual(-1, list.IndexOf(item: "b", StringComparer.Ordinal));
    }
  }
}
