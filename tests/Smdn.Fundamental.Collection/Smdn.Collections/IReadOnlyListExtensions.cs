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
      Assert.That(list.Slice(0), Is.EqualTo(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
      Assert.That(list.Slice(0, 3), Is.EqualTo(new[] { 0, 1, 2 }));
      Assert.That(list.Slice(7, 3), Is.EqualTo(new[] { 7, 8, 9 }));
      Assert.That(list.Slice(2, 5), Is.EqualTo(new[] { 2, 3, 4, 5, 6 }));
      Assert.That(list.Slice(6), Is.EqualTo(new[] { 6, 7, 8, 9 }));
      Assert.That(list.Slice(0, 0), Is.EqualTo(new int[] { }));
      Assert.That(list.Slice(10), Is.EqualTo(new int[] { }));
      Assert.That(list.Slice(10, 0), Is.EqualTo(new int[] { }));

      Assert.Throws<ArgumentOutOfRangeException>(() => list.Slice(-1, 1), "argument exception #1");
      Assert.Throws<ArgumentOutOfRangeException>(() => list.Slice(1, -1), "argument exception #2");
      Assert.Throws<ArgumentException>(() => list.Slice(10, 1), "argument exception #3");
      Assert.Throws<ArgumentException>(() => list.Slice(1, 10), "argument exception #4");
      Assert.Throws<ArgumentException>(() => list.Slice(11, 0), "argument exception #5");
      Assert.Throws<ArgumentException>(() => list.Slice(0, 11), "argument exception #6");

      // test IReadOnlyList.Count
      Assert.That(list.Slice(0).Count, Is.EqualTo(10));
      Assert.That(list.Slice(0, 3).Count, Is.EqualTo(3));
      Assert.That(list.Slice(6).Count, Is.EqualTo(4));
      Assert.That(list.Slice(10).Count, Is.Zero);

      // test IReadOnlyList.this[]
      Assert.That(list.Slice(2, 3)[0], Is.EqualTo(2));
      Assert.That(list.Slice(2, 3)[1], Is.EqualTo(3));
      Assert.That(list.Slice(2, 3)[2], Is.EqualTo(4));
      Assert.Throws<ArgumentOutOfRangeException>(() => Assert.That(list.Slice(2, 3)[-1], Is.Zero));
      Assert.Throws<ArgumentOutOfRangeException>(() => Assert.That(list.Slice(2, 3)[3], Is.Zero));

      Assert.Throws<ArgumentOutOfRangeException>(() => Assert.That(list.Slice(0, 0)[0], Is.Zero));

      // test enumerator
      Assert.That(list.Slice(0, 3).ToArray(), Is.EqualTo(new[] { 0, 1, 2 }));
      Assert.That(list.Slice(7, 3).ToArray(), Is.EqualTo(new[] { 7, 8, 9 }));
      Assert.That(list.Slice(0, 0).ToArray(), Is.EqualTo(new int[] { }));
      Assert.That(list.Slice(10).ToArray(), Is.EqualTo(new int[] { }));
    }

    [Test]
    public void TestIndexOf_Array() => TestIndexOf<ArgumentException>(new int[] { 1, 1, 2, 1 });

    [Test]
    public void TestIndexOf_List() => TestIndexOf<ArgumentOutOfRangeException>(new List<int> { 1, 1, 2, 1 });

    private void TestIndexOf<TExpectedArgumentException> (IReadOnlyList<int> list) where TExpectedArgumentException : ArgumentException
    {
      Assert.That(list.IndexOf(item: 1), Is.Zero);
      Assert.That(list.IndexOf(item: 2), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: 0), Is.EqualTo(-1));
      Assert.That(list.IndexOf(item: 1, 0), Is.Zero);
      Assert.That(list.IndexOf(item: 1, 1), Is.EqualTo(1));
      Assert.That(list.IndexOf(item: 1, 2), Is.EqualTo(3));
      Assert.That(list.IndexOf(item: 1, 3), Is.EqualTo(3));
      Assert.That(list.IndexOf(item: 2, 0), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: 2, 1), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: 2, 2), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: 2, 3), Is.EqualTo(-1));
      Assert.That(list.IndexOf(item: 2, 0, 1), Is.EqualTo(-1));
      Assert.That(list.IndexOf(item: 2, 0, 2), Is.EqualTo(-1));
      Assert.That(list.IndexOf(item: 2, 0, 3), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: 2, 0, 4), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: 2, 4, 0), Is.EqualTo(-1));

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

      Assert.That(list.IndexOf(item: "A", equalityComparer), Is.Zero);
      Assert.That(list.IndexOf(item: "a", equalityComparer), Is.Zero);
      Assert.That(list.IndexOf(item: "B", equalityComparer), Is.EqualTo(1));
      Assert.That(list.IndexOf(item: "b", equalityComparer), Is.EqualTo(1));
      Assert.That(list.IndexOf(item: "C", equalityComparer), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: "c", equalityComparer), Is.EqualTo(2));
      Assert.That(list.IndexOf(item: "C", index: 3, equalityComparer), Is.EqualTo(3));
      Assert.That(list.IndexOf(item: "c", index: 3, equalityComparer), Is.EqualTo(3));
      Assert.That(list.IndexOf(item: "X", equalityComparer), Is.EqualTo(-1));
      Assert.That(list.IndexOf(item: "x", equalityComparer), Is.EqualTo(-1));

      Assert.That(list.IndexOf(item: "B", StringComparer.Ordinal), Is.EqualTo(1));
      Assert.That(list.IndexOf(item: "b", StringComparer.Ordinal), Is.EqualTo(-1));
    }
  }
}
