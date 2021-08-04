// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if false
using System;
using NUnit.Framework;

using GenericCollections = System.Collections.Generic;

using System.Linq;

namespace Smdn.Collections {
  public class HashSet<T> : GenericCollections.HashSet<T> {
    public HashSet()
      : base() {}

    public HashSet(GenericCollections.IEqualityComparer<T> comparer)
      : base(comparer) {}

    public HashSet(GenericCollections.IEnumerable<T> collection)
      : base(collection) {}

    public HashSet(GenericCollections.IEnumerable<T> collection, GenericCollections.IEqualityComparer<T> comparer)
      : base(collection, comparer) {}
  }

  [TestFixture]
  public class HashSetTests {
    [Test]
    public void TestConstruct1()
    {
      var s = new HashSet<int>();

      Assert.AreEqual(0, s.Count);
      Assert.IsNotNull(s.Comparer);
    }

    [Test]
    public void TestConstruct2()
    {
      var comparer = GenericCollections.EqualityComparer<int>.Default;
      var s = new HashSet<int>(comparer);

      Assert.AreEqual(0, s.Count);
      Assert.AreSame(comparer, s.Comparer);

      comparer = null;
      s = new HashSet<int>(comparer);

      Assert.AreEqual(0, s.Count);
      Assert.IsNotNull(s.Comparer);
      Assert.AreEqual(GenericCollections.EqualityComparer<int>.Default, s.Comparer);
    }

    [Test]
    public void TestConstruct3()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 3));

      Assert.AreEqual(3, s.Count);
      Assert.AreEqual(GenericCollections.EqualityComparer<int>.Default, s.Comparer);
    }

    [Test]
    public void TestConstruct4()
    {
      var comparer = GenericCollections.EqualityComparer<int>.Default;
      var s = new HashSet<int>(Enumerable.Range(0, 3), comparer);

      Assert.AreEqual(3, s.Count);
      Assert.AreSame(comparer, s.Comparer);

      comparer = null;
      s = new HashSet<int>(comparer);

      Assert.AreEqual(0, s.Count);
      Assert.IsNotNull(s.Comparer);
      Assert.AreEqual(GenericCollections.EqualityComparer<int>.Default, s.Comparer);
    }

    [Test]
    public void TestConstructCollectionNull()
    {
      try {
        new HashSet<int>((int[])null);
        Assert.Fail("ArgumentNullException not thrown");
      }
      catch (ArgumentNullException) {
      }

      try {
        new HashSet<int>((int[])null, GenericCollections.EqualityComparer<int>.Default);
        Assert.Fail("ArgumentNullException not thrown");
      }
      catch (ArgumentNullException) {
      }
    }

    [Test]
    public void TestEnumerator()
    {
      var expected = new GenericCollections.List<int>(Enumerable.Range(0, 20));
      var s = new HashSet<int>(expected);

      Assert.AreEqual(20, s.Count, "Count initial");

      foreach (var e in s) {
        if (expected.Contains(e))
          expected.Remove(e);
      }

      CollectionAssert.IsEmpty(expected);
    }

    [Test]
    public void TestEnumeratorHashColliding()
    {
      var expected = new GenericCollections.List<int>(Enumerable.Range(0, 20));
      var s = new HashSet<int>(expected, new HashCollisionEqualityComparer());

      Assert.AreEqual(20, s.Count, "Count initial");

      foreach (var e in s) {
        if (expected.Contains(e))
          expected.Remove(e);
      }

      CollectionAssert.IsEmpty(expected);
    }

    [Test]
    public void TestEnumeratorWhileEnumeratingAdd()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 20));

      try {
        var count = 0;

        foreach (var e in s) {
          s.Add(20 + e);

          if (0 < count++)
            Assert.Fail("InvalidOperationException not thrown");
        }
      }
      catch (InvalidOperationException) {
      }
    }

    [Test]
    public void TestEnumeratorWhileEnumeratingRemove()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 20));

      try {
        var count = 0;

        foreach (var e in s) {
          s.Remove(e);

          if (0 < count++)
            Assert.Fail("InvalidOperationException not thrown");
        }
      }
      catch (InvalidOperationException) {
      }
    }

    [Test]
    public void TestEnumeratorWhileEnumeratingClear()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 20));

      try {
        var count = 0;

        foreach (var e in s) {
          s.Clear();

          if (0 < count++)
            Assert.Fail("InvalidOperationException not thrown");
        }
      }
      catch (InvalidOperationException) {
      }
    }

    [Test]
    public void TestEnumeratorEmpty()
    {
      var s = new HashSet<int>();

      Assert.AreEqual(0, s.Count, "Count initial");

      var count = 0;

      foreach (var e in s) {
        count++;
      }

      Assert.AreEqual(0, count, "count");
    }

    [Test]
    public void TestAdd()
    {
      var s = new HashSet<int>();

      Assert.AreEqual(0, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {}, s, "equivalent initial");

      Assert.IsTrue(s.Add(0), "Add #0");
      Assert.AreEqual(1, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {0}, s, "equivalent #0");

      Assert.IsTrue(s.Add(1), "Add #1");
      Assert.AreEqual(2, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new int[] {0, 1}, s, "equivalent #1");

      Assert.IsFalse(s.Add(0), "Add #2");
      Assert.AreEqual(2, s.Count, "Count #2");
      CollectionAssert.AreEquivalent(new int[] {0, 1}, s, "equivalent #2");
    }

    [Test]
    public void TestAddICollection()
    {
      var s = new HashSet<int>();

      Assert.AreEqual(0, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {}, s, "equivalent initial");

      GenericCollections.ICollection<int> collection = s;

      collection.Add(0);

      Assert.AreEqual(1, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {0}, s, "equivalent #0");

      collection.Add(0);

      Assert.AreEqual(1, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new int[] {0}, s, "equivalent #1");
    }

    [Test]
    public void TestAddCapacityChange()
    {
      var s = new HashSet<int>();

      Assert.AreEqual(0, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {}, s, "equivalent initial");

      var expected = new GenericCollections.List<int>();

      for (var i = 0; i < 1000; i++) {
        expected.Add(i);

        Assert.IsTrue(s.Add(i), "Add #{0}", i);
        Assert.AreEqual(i + 1, s.Count, "Count #{0}", i);
        CollectionAssert.AreEquivalent(expected, s, "equivalent #{0}", i);
      }
    }

    private class HashCollisionEqualityComparer : GenericCollections.EqualityComparer<int> {
      public override bool Equals(int x, int y)
      {
        return x == y;
      }

      public override int GetHashCode(int obj)
      {
        return 0;
      }
    }

    [Test]
    public void TestAddHashColliding()
    {
      var s = new HashSet<int>(new HashCollisionEqualityComparer());

      Assert.AreEqual(0, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {}, s, "equivalent initial");

      Assert.IsTrue(s.Add(0), "Add #0");
      Assert.AreEqual(1, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {0}, s, "equivalent #0");

      Assert.IsTrue(s.Add(1), "Add #1");
      Assert.AreEqual(2, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new int[] {0, 1}, s, "equivalent #1");

      Assert.IsFalse(s.Add(0), "Add #2");
      Assert.AreEqual(2, s.Count, "Count #2");
      CollectionAssert.AreEquivalent(new int[] {0, 1}, s, "equivalent #2");

      for (var i = 3; i < 20; i++) {
        Assert.IsTrue(s.Add(i - 1), "Add #{0}", i);
        Assert.AreEqual(i, s.Count, "Count #{0}", i);
        CollectionAssert.AreEquivalent(Enumerable.Range(0, i), s, "equivalent #{0}", i);
      }
    }

    [Test]
    public void TestAddNull()
    {
      var s = new HashSet<string>();

      Assert.IsTrue(s.Add(null), "Add #0");
      Assert.AreEqual(1, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new string[] {null}, s, "equivalent #0");

      Assert.IsTrue(s.Add("foo"), "Add #1");
      Assert.AreEqual(2, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new string[] {null, "foo"}, s, "equivalent #1");

      Assert.IsFalse(s.Add(null), "Add #2");
      Assert.AreEqual(2, s.Count, "Count #2");
      CollectionAssert.AreEquivalent(new string[] {null, "foo"}, s, "equivalent #2");

      Assert.IsTrue(s.Add(""), "Add #3");
      Assert.AreEqual(3, s.Count, "Count #3");
      CollectionAssert.AreEquivalent(new string[] {null, "foo", ""}, s, "equivalent #3");
    }

    [Test]
    public void TestRemove()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 3));

      Assert.AreEqual(3, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2}, s, "equivalent initial");

      Assert.IsTrue(s.Remove(0), "Remove #0");
      Assert.AreEqual(2, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {1, 2}, s, "equivalent #0");

      Assert.IsTrue(s.Remove(2), "Remove #1");
      Assert.AreEqual(1, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new int[] {1}, s, "equivalent #1");

      Assert.IsFalse(s.Remove(0), "Remove #2");
      Assert.AreEqual(1, s.Count, "Count #2");
      CollectionAssert.AreEquivalent(new int[] {1}, s, "equivalent #2");
    }

    [Test]
    public void TestRemoveHashColliding()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 3), new HashCollisionEqualityComparer());

      Assert.AreEqual(3, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2}, s, "equivalent initial");

      Assert.IsFalse(s.Remove(-1), "Remove #0");
      Assert.AreEqual(3, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2}, s, "equivalent #0");

      Assert.IsFalse(s.Remove(3), "Remove #1");
      Assert.AreEqual(3, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2}, s, "equivalent #1");

      Assert.IsTrue(s.Remove(1), "Remove #2");
      Assert.AreEqual(2, s.Count, "Count #2");
      CollectionAssert.AreEquivalent(new int[] {0, 2}, s, "equivalent #2");
    }

    [Test]
    public void TestRemoveNull()
    {
      var s = new HashSet<string>(new string[] {"foo", null, ""});

      Assert.AreEqual(3, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new string[] {"foo", null, ""}, s, "equivalent initial");

      Assert.IsTrue(s.Remove(null), "Remove #0");
      Assert.AreEqual(2, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new string[] {"foo", ""}, s.ToArray(), "equivalent #0");

      Assert.IsFalse(s.Remove(null), "Remove #1");
      Assert.AreEqual(2, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new string[] {"foo", ""}, s, "equivalent #1");
    }

    [Test]
    public void TestAddRemove()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 10));

      Assert.AreEqual(10, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
      }, s, "equivalent initial");

      Assert.IsTrue(s.Remove(5), "Remove #0");
      Assert.AreEqual(9, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {
        0, 1, 2, 3, 4, 6, 7, 8, 9,
      }, s, "equivalent #0");

      Assert.IsTrue(s.Add(5), "Add #1");
      Assert.AreEqual(10, s.Count, "Count #1");
      CollectionAssert.AreEquivalent(new int[] {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
      }, s, "equivalent #1");
    }

    [Test]
    public void TestRemoveWhere()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 20));

      Assert.AreEqual(20, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
      }, s, "equivalent initial");

      Assert.AreEqual(10, s.RemoveWhere(delegate(int e) {
        return (e % 2 == 0);
      }), "removed count");

      Assert.AreEqual(10, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {
        1, 3, 5, 7, 9,
        11, 13, 15, 17, 19
      }, s, "equivalent #0");
    }

    [Test]
    public void TestRemoveWherePredicateNull()
    {
      Predicate<int> pred = null;

      var s = new HashSet<int>(new[] {0, 1, 2, 3, 4, 5});

      Assert.AreEqual(6, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2, 3, 4, 5}, s, "equivalent initial");

      try {
        Assert.AreEqual(0, s.RemoveWhere(pred), "removed count");
        Assert.Fail("ArgumentNullException not thrown");
      }
      catch (ArgumentNullException) {
      }

      Assert.AreEqual(6, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2, 3, 4, 5}, s, "equivalent #0");
    }

    [Test]
    public void TestClear()
    {
      var s = new HashSet<int>(new[] {0, 1, 2});

      Assert.AreEqual(3, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(new int[] {0, 1, 2}, s, "equivalent initial");

      s.Clear();

      Assert.AreEqual(0, s.Count, "Count #0");
      CollectionAssert.AreEquivalent(new int[] {}, s, "equivalent #0");
    }

    [Test]
    public void TestContains()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 20));

      for (var i = 0; i < 20; i++) {
        Assert.IsTrue(s.Contains(i), "Contains #{0}", i);
      }

      foreach (var e in new[] {int.MinValue, -1, 20, int.MaxValue}) {
        Assert.IsFalse(s.Contains(e), "Contains({0})", e);
      }
    }

    [Test]
    public void TestContainsHashColliding()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 3), new HashCollisionEqualityComparer());

      Assert.IsTrue(s.Contains(0), "Contains #0");
      Assert.IsTrue(s.Contains(1), "Contains #1");
      Assert.IsTrue(s.Contains(2), "Contains #2");
      Assert.IsFalse(s.Contains(3), "Contains #3");
      Assert.IsFalse(s.Contains(-1), "Contains #4");
    }

    [Test]
    public void TestContainsNull()
    {
      var s = new HashSet<string>(new string[] {"foo", null, ""});

      Assert.IsTrue(s.Contains(null), "Contains #0");
      Assert.IsTrue(s.Contains(""), "Contains #1");
      Assert.IsTrue(s.Contains("foo"), "Contains #2");

      s = new HashSet<string>(new string[] {"foo", ""});

      Assert.IsFalse(s.Contains(null), "Contains #3");
      Assert.IsTrue(s.Contains(""), "Contains #4");
    }

    [Test]
    public void TestContainsEmpty()
    {
      var s = new HashSet<int>();

      Assert.IsFalse(s.Contains(0), "Contains #0");
      Assert.IsFalse(s.Contains(1), "Contains #1");
    }

    [Test]
    public void TestCopyTo()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 10));
      var array = new int[12];

      array[10] = -1;
      array[11] = 11;

      s.CopyTo(array);

      CollectionAssert.AreEquivalent(new int[] {
        -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11,
      }, array, "equivalent #0");

      Assert.AreEqual(-1, array[10], "array[10] #0");
      Assert.AreEqual(11, array[11], "array[11] #0");

      array[0] = -1;
      array[11] = 11;

      s.CopyTo(array, 1);

      Assert.AreEqual(-1, array[0], "array[0] #1");
      Assert.AreEqual(11, array[11], "array[11] #1");

      array[0] = -2;
      array[1] = -1;
      array[10] = 10;
      array[11] = 11;

      s.CopyTo(array, 2, 8);

      Assert.AreEqual(-2, array[0], "array[0] #2");
      Assert.AreEqual(-1, array[1], "array[1] #2");
      Assert.AreEqual(10, array[10], "array[10] #2");
      Assert.AreEqual(11, array[11], "array[11] #2");
    }

    [Test]
    public void TestCopyToEmpty()
    {
      var s = new HashSet<int>();
      var array = new int[] {-1, -1, -1};

      s.CopyTo(array);

      CollectionAssert.AreEqual(new int[] {-1, -1, -1}, array);

      array = new int[0];

      s.CopyTo(array);
    }

    [Test]
    public void TestCopyToArgumentException()
    {
      var s = new HashSet<int>(Enumerable.Range(0, 10));

      try {
        s.CopyTo(null, 0, 10);
        Assert.Fail("ArgumentNullException not thrown");
      }
      catch (ArgumentNullException) {
      }

      var array = new int[10];

      try {
        s.CopyTo(array, -1, 11);
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentException) {
      }

      try {
        s.CopyTo(array, 11, -1);
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        s.CopyTo(array, 0, 11);
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }

      try {
        s.CopyTo(array, 11, 0);
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestComparer()
    {
      var ss = new HashSet<string>(new[] {"A", "B", "C"}, StringComparer.Ordinal);
      var si = new HashSet<string>(new[] {"a", "b", "c"}, StringComparer.OrdinalIgnoreCase);

      Assert.IsTrue(ss.Contains("A"), "Contains #0");
      Assert.IsTrue(si.Contains("A"), "Contains #1");

      Assert.IsFalse(ss.Contains("b"), "Contains #2");
      Assert.IsTrue(si.Contains("b"), "Contains #3");

      Assert.IsFalse(ss.Remove("c"), "Remove #0");
      Assert.IsTrue(si.Remove("C"), "Remove #1");
    }

    [Test]
    public void TestComparerDifferent()
    {
      var si = new HashSet<string>(new[] {"a", "b", "c"}, StringComparer.OrdinalIgnoreCase);

      Assert.IsTrue(si.IsSupersetOf(new[] {"A", "B"}), "IsSupersetOf #0");
      Assert.IsTrue(si.IsSupersetOf(new HashSet<string>(new[] {"A", "B"}, StringComparer.Ordinal)), "IsSupersetOf #1");
      Assert.IsTrue(si.IsSupersetOf(new HashSet<string>(new[] {"A", "B"}, StringComparer.OrdinalIgnoreCase)), "IsSupersetOf #2");

      Assert.IsTrue(si.IsSubsetOf(new[] {"A", "B", "C", "D"}), "IsSubsetOf #0");
      Assert.IsTrue(si.IsSubsetOf(new HashSet<string>(new[] {"A", "B", "C", "D"}, StringComparer.Ordinal)), "IsSubsetOf #1");
      Assert.IsTrue(si.IsSubsetOf(new HashSet<string>(new[] {"A", "B", "C", "D"}, StringComparer.OrdinalIgnoreCase)), "IsSubsetOf #2");

      Assert.IsTrue(si.SetEquals(new[] {"A", "B", "C"}), "SetEquals #0");
      Assert.IsTrue(si.SetEquals(new HashSet<string>(new[] {"A", "B", "C"}, StringComparer.Ordinal)), "SetEquals #1");
      Assert.IsTrue(si.SetEquals(new HashSet<string>(new[] {"A", "B", "C"}, StringComparer.OrdinalIgnoreCase)), "SetEquals #2");

      Assert.IsTrue(si.Overlaps(new[] {"C", "D"}), "Overlaps #0");
      Assert.IsTrue(si.Overlaps(new HashSet<string>(new[] {"C", "D"}, StringComparer.Ordinal)), "Overlaps #1");
      Assert.IsTrue(si.Overlaps(new HashSet<string>(new[] {"C", "D"}, StringComparer.OrdinalIgnoreCase)), "Overlaps #2");

      si = new HashSet<string>(new[] {"a", "b", "c"}, StringComparer.OrdinalIgnoreCase);
      si.IntersectWith(new[] {"B", "C", "D"});

      CollectionAssert.AreEquivalent(new[] {"b", "c"}, si, "IntersectWith #0");

      si = new HashSet<string>(new[] {"a", "b", "c"}, StringComparer.OrdinalIgnoreCase);
      si.IntersectWith(new HashSet<string>(new[] {"B", "C", "D"}, StringComparer.Ordinal));

      CollectionAssert.AreEquivalent(new[] {"b", "c"}, si, "IntersectWith #1");

      si = new HashSet<string>(new[] {"a", "b", "c"}, StringComparer.OrdinalIgnoreCase);
      si.IntersectWith(new HashSet<string>(new[] {"B", "C", "D"}, StringComparer.OrdinalIgnoreCase));

      CollectionAssert.AreEquivalent(new[] {"b", "c"}, si, "IntersectWith #2");
    }

    [Test]
    public void TestExceptWith()
    {
      var setInitial = new int[] {6, 2, 0, 4, 8};
      var s = new HashSet<int>(setInitial);

      Assert.AreEqual(5, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent initial");

      s.ExceptWith(new int[] {});

      Assert.AreEqual(5, s.Count, "Count ExceptWith #0");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent ExceptWith #0");

      s = new HashSet<int>(setInitial);
      s.ExceptWith(new int[] {3, 1, 2, 0, 4});

      Assert.AreEqual(2, s.Count, "Count ExceptWith #1");
      CollectionAssert.AreEquivalent(new int[] {6, 8}, s, "equivalent ExceptWith #1");
    }

    [Test]
    public void TestIntersectWith()
    {
      var setInitial = new int[] {6, 2, 0, 4, 8};
      var s = new HashSet<int>(setInitial);

      Assert.AreEqual(5, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent initial");

      s.IntersectWith(new int[] {});

      Assert.AreEqual(0, s.Count, "Count IntersectWith #0");
      CollectionAssert.AreEquivalent(new int[] {}, s, "equivalent IntersectWith #0");

      s = new HashSet<int>(setInitial);
      s.IntersectWith(new int[] {3, 1, 2, 0, 4});

      Assert.AreEqual(3, s.Count, "Count IntersectWith #1");
      CollectionAssert.AreEquivalent(new int[] {2, 0, 4}, s, "equivalent IntersectWith #1");
    }

    [Test]
    public void TestSymmetricExceptWith()
    {
      var setInitial = new int[] {6, 2, 0, 4, 8};
      var s = new HashSet<int>(setInitial);

      Assert.AreEqual(5, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent initial");

      s.SymmetricExceptWith(new int[] {});

      Assert.AreEqual(5, s.Count, "Count SymmetricExceptWith #0");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent SymmetricExceptWith #0");

      s.SymmetricExceptWith(new int[] {3, 1, 2, 0, 4});

      Assert.AreEqual(4, s.Count, "Count SymmetricExceptWith #1");
      CollectionAssert.AreEquivalent(new int[] {6, 8, 3, 1}, s, "equivalent SymmetricExceptWith #1");
    }

    [Test]
    public void TestUnionWith()
    {
      var setInitial = new int[] {6, 2, 0, 4, 8};
      var s = new HashSet<int>(setInitial);

      Assert.AreEqual(5, s.Count, "Count initial");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent initial");

      s.UnionWith(new int[] {});

      Assert.AreEqual(5, s.Count, "Count UnionWith #0");
      CollectionAssert.AreEquivalent(setInitial, s, "equivalent UnionWith #0");

      s.UnionWith(new int[] {3, 1, 2, 0, 4});

      Assert.AreEqual(7, s.Count, "Count UnionWith #1");
      CollectionAssert.AreEquivalent(new int[] {6, 2, 0, 4, 8, 3, 1}, s, "equivalent UnionWith #1");
    }

    [Test]
    public void TestIsSubsetOf()
    {
      var s = new HashSet<int>(new int[] {3, 4, 1, 5, 2});

      Assert.IsTrue(s.IsSubsetOf(new int[] {0, 1, 2, 3, 4, 5, 6}), "IsSubsetOf #0");
      Assert.IsTrue(s.IsSubsetOf(new int[] {1, 2, 3, 4, 5}), "IsSubsetOf #1");
      Assert.IsFalse(s.IsSubsetOf(new int[] {0, 1, 2, 3, 4, 6}), "IsSubsetOf #2");
      Assert.IsFalse(s.IsSubsetOf(new int[] {1, 2, 3, 4, 6}), "IsSubsetOf #3");
    }

    [Test]
    public void TestIsSubsetOfEmpty()
    {
      var s = new HashSet<int>();

      Assert.IsTrue(s.IsSubsetOf(new int[] {0}), "IsSubsetOf #0");
      Assert.IsTrue(s.IsSubsetOf(new int[] {}), "IsSubsetOf #1");

      s = new HashSet<int>(new int[] {0});

      Assert.IsFalse(s.IsSubsetOf(new int[] {}), "IsSubsetOf #2");
    }

    [Test]
    public void TestIsProperSubsetOf()
    {
      var s = new HashSet<int>(new int[] {3, 4, 1, 5, 2});

      Assert.IsTrue(s.IsProperSubsetOf(new int[] {0, 1, 2, 3, 4, 5, 6}), "IsProperSubsetOf #0");
      Assert.IsFalse(s.IsProperSubsetOf(new int[] {1, 2, 3, 4, 5}), "IsProperSubsetOf #1");
      Assert.IsFalse(s.IsProperSubsetOf(new int[] {0, 1, 2, 3, 4, 6}), "IsProperSubsetOf #2");
      Assert.IsFalse(s.IsProperSubsetOf(new int[] {1, 2, 3, 4, 6}), "IsProperSubsetOf #3");
    }

    [Test]
    public void TestIsProperSubsetOfEmpty()
    {
      var s = new HashSet<int>();

      Assert.IsFalse(s.IsProperSubsetOf(new int[] {0}), "IsProperSubsetOf #0");
      Assert.IsTrue(s.IsProperSubsetOf(new int[] {}), "IsProperSubsetOf #1");

      s = new HashSet<int>(new int[] {0});

      Assert.IsFalse(s.IsProperSubsetOf(new int[] {}), "IsProperSubsetOf #2");
    }

    [Test]
    public void TestIsSupersetOf()
    {
      var s = new HashSet<int>(new int[] {3, 4, 1, 5, 2});

      Assert.IsTrue(s.IsSupersetOf(new int[] {2, 3, 4}), "IsSupersetOf #0");
      Assert.IsTrue(s.IsSupersetOf(new int[] {1, 2, 3, 4, 5}), "IsSupersetOf #1");
      Assert.IsFalse(s.IsSupersetOf(new int[] {1, 2, 3, 6}), "IsSupersetOf #2");
      Assert.IsFalse(s.IsSupersetOf(new int[] {1, 2, 3, 4, 6}), "IsSupersetOf #3");
    }

    [Test]
    public void TestIsSupersetOfEmpty()
    {
      var s = new HashSet<int>();

      Assert.IsFalse(s.IsSupersetOf(new int[] {0}), "IsSupersetOf #0");
      Assert.IsTrue(s.IsSupersetOf(new int[] {}), "IsSupersetOf #1");

      s = new HashSet<int>(new int[] {0});

      Assert.IsTrue(s.IsSupersetOf(new int[] {}), "IsSupersetOf #2");
    }

    [Test]
    public void TestIsProperSupersetOf()
    {
      var s = new HashSet<int>(new int[] {3, 4, 1, 5, 2});

      Assert.IsTrue(s.IsProperSupersetOf(new int[] {2, 3, 4}), "IsProperSupersetOf #0");
      Assert.IsFalse(s.IsProperSupersetOf(new int[] {1, 2, 3, 4, 5}), "IsProperSupersetOf #1");
      Assert.IsFalse(s.IsProperSupersetOf(new int[] {1, 2, 3, 6}), "IsProperSupersetOf #2");
      Assert.IsFalse(s.IsProperSupersetOf(new int[] {1, 2, 3, 4, 6}), "IsProperSupersetOf #3");
    }

    [Test]
    public void TestIsProperSupersetOfEmpty()
    {
      var s = new HashSet<int>();

      Assert.IsFalse(s.IsProperSupersetOf(new int[] {0}), "IsProperSupersetOf #0");
      Assert.IsFalse(s.IsProperSupersetOf(new int[] {}), "IsProperSupersetOf #1");

      s = new HashSet<int>(new int[] {0});

      Assert.IsTrue(s.IsProperSupersetOf(new int[] {}), "IsProperSupersetOf #2");
    }

    [Test]
    public void TestOverlaps()
    {
      var s = new HashSet<int>(new int[] {3, 4, 1, 5, 2});

      Assert.IsTrue(s.Overlaps(new int[] {2, 3, 5, 7}), "Overlaps #0");
      Assert.IsFalse(s.Overlaps(new int[] {0, 6}), "Overlaps #1");
      Assert.IsFalse(s.Overlaps(new int[] {}), "Overlaps #2");
    }

    [Test]
    public void TestOverlapsEmpty()
    {
      var s = new HashSet<int>();

      Assert.IsFalse(s.Overlaps(new int[] {0}), "Overlaps #1");
      Assert.IsFalse(s.Overlaps(new int[] {}), "Overlaps #2");
    }

    [Test]
    public void TestSetEquals()
    {
      var s = new HashSet<int>(new int[] {3, 4, 1, 5, 2});

      Assert.IsTrue(s.SetEquals(new int[] {1, 2, 3, 4, 5}), "SetEquals #0");
      Assert.IsFalse(s.SetEquals(new int[] {1, 2, 3, 4, 6}), "SetEquals #1");
      Assert.IsFalse(s.SetEquals(new int[] {1, 2, 3, 4}), "SetEquals #2");
      Assert.IsFalse(s.SetEquals(new int[] {}), "SetEquals #3");
    }
  }
}

#endif