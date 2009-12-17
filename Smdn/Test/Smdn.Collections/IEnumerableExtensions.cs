using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture()]
  public class IEnumerableExtensionsTests {
    [Test]
    public void TestEqualsAll()
    {
      Assert.IsTrue(((IEnumerable<int>)(new int[] {})).EqualsAll(new int[] {}));
      Assert.IsTrue(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).EqualsAll(new[] {0, 1, 2, 3, 4}));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).EqualsAll(null));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).EqualsAll(new int[] {}));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).EqualsAll(new[] {0, 1, 2, 3}));
      Assert.IsFalse(((IEnumerable<int>)(new[] {0, 1, 2, 3, 4})).EqualsAll(new[] {0, 1, 2, 3, 4, 5}));
    }

    [Test]
    public void TestEqualsAllWithIEqualityComparer()
    {
      Assert.IsTrue(((IEnumerable<string>)(new string[] {})).EqualsAll(new string[] {}, StringComparer.InvariantCultureIgnoreCase));
      Assert.IsTrue(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).EqualsAll(new[] {"A", "b", "C", "d", "E"}, StringComparer.InvariantCultureIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).EqualsAll(null, StringComparer.InvariantCultureIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).EqualsAll(new string[] {}, StringComparer.InvariantCultureIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).EqualsAll(new[] {"A", "b", "C", "d"}, StringComparer.InvariantCultureIgnoreCase));
      Assert.IsFalse(((IEnumerable<string>)(new[] {"a", "b", "c", "d", "e"})).EqualsAll(new[] {"A", "b", "C", "d", "E", "f"}, StringComparer.InvariantCultureIgnoreCase));
    }

    [Test]
    public void TestConvertAll()
    {
      Assert.IsTrue(((IEnumerable<string>)(new string[] {})).EqualsAll((new int[] {}).ConvertAll(delegate(int i) {
        return i.ToString();
      })));
      Assert.IsTrue(((IEnumerable<string>)(new string[] {"0", "1", "2", "3", "4"})).EqualsAll((new int[] {0, 1, 2, 3, 4}).ConvertAll(delegate(int i) {
        return i.ToString();
      })));
    }

    [Test]
    public void TestCast()
    {
      // TODO
      Assert.Ignore("no tests");
    }

    [Test]
    public void TestCount()
    {
      Assert.AreEqual(5, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Count());
      Assert.AreEqual(5, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Count());
      Assert.AreEqual(5, ((IEnumerable)new ArrayList(new[] {0, 1, 2, 3, 4})).Count());
    }

    private static IEnumerable<int> GetEnumerator()
    {
      yield return 0;
      yield return 1;
      yield return 2;
      yield return 3;
      yield return 4;
    }

    private static IEnumerable<int> GetEmptyEnumerator()
    {
      yield break;
    }

    [Test]
    public void TestFirst()
    {
      Assert.AreEqual(0, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).First());
      Assert.AreEqual(0, GetEnumerator().First());

      try {
        ((IEnumerable<int>)new int[] {}).First();
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }

      try {
        GetEmptyEnumerator().First();
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }
    }

    [Test]
    public void TestFirstOrDefault()
    {
      Assert.AreEqual(0, ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).FirstOrDefault());
      Assert.AreEqual(0, GetEnumerator().FirstOrDefault());
      Assert.AreEqual(0, ((IEnumerable<int>)new int[] {}).FirstOrDefault());
      Assert.AreEqual(0, GetEmptyEnumerator().FirstOrDefault());
    }

    [Test]
    public void TestFind()
    {
      Assert.AreEqual("a",   (new[] {"a", "aa", "aaa"}).Find(delegate(string s) {return s.Length == 1;}));
      Assert.AreEqual("aa",  (new[] {"a", "aa", "aaa"}).Find(delegate(string s) {return s.Length == 2;}));
      Assert.AreEqual("aaa", (new[] {"a", "aa", "aaa"}).Find(delegate(string s) {return s.Length == 3;}));
      Assert.AreEqual(null,  (new[] {"a", "aa", "aaa"}).Find(delegate(string s) {return s.Length == 4;}));

      Assert.AreEqual(3, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Find(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Find(delegate(int i){ return i == 9; }));
      Assert.AreEqual(3, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Find(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Find(delegate(int i){ return i == 9; }));
      Assert.AreEqual(3, ((IEnumerable<int>)GetEnumerator()).Find(delegate(int i){ return i == 3; }));
      Assert.AreEqual(0, ((IEnumerable<int>)GetEnumerator()).Find(delegate(int i){ return i == 9; }));
    }

    [Test]
    public void TestFindAll()
    {
      CollectionAssert.AreEquivalent(new int[0], ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).FindAll(delegate(int i){ return i == 9; }));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).FindAll(delegate(int i){ return i < 3; }));
    }

    [Test]
    public void TestExists()
    {
      Assert.IsTrue (((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Exists(delegate(int i){ return i == 3; }));
      Assert.IsFalse(((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Exists(delegate(int i){ return i == 9; }));
      Assert.IsTrue (((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Exists(delegate(int i){ return i == 3; }));
      Assert.IsFalse(((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Exists(delegate(int i){ return i == 9; }));
      Assert.IsTrue (((IEnumerable<int>)GetEnumerator()).Exists(delegate(int i){ return i == 3; }));
      Assert.IsFalse(((IEnumerable<int>)GetEnumerator()).Exists(delegate(int i){ return i == 9; }));
    }

    [Test]
    public void TestTake()
    {
      CollectionAssert.AreEquivalent(new int[0], ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(-1));
      CollectionAssert.AreEquivalent(new int[0], ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(0));
      CollectionAssert.AreEquivalent(new[] {0}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(1));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(3));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2, 3, 4}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(5));
      CollectionAssert.AreEquivalent(new[] {0, 1, 2, 3, 4}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Take(10));
    }

    [Test]
    public void TestReverse()
    {
      CollectionAssert.AreEquivalent(new[] {4, 3, 2, 1, 0}, ((IEnumerable<int>)new int[] {0, 1, 2, 3, 4}).Reverse());
      CollectionAssert.AreEquivalent(new[] {4, 3, 2, 1, 0}, ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).Reverse());
      CollectionAssert.AreEquivalent(new[] {4, 3, 2, 1, 0}, ((IEnumerable<int>)GetEnumerator()).Reverse());
    }

    [Test]
    public void TestToArray()
    {
      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4}, 
                                              ((IEnumerable<int>)new[] {0, 1, 2, 3, 4}).ToArray()));
      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4}, 
                                              ((IEnumerable<int>)new List<int>(new[] {0, 1, 2, 3, 4})).ToArray()));
      Assert.IsTrue(ArrayExtensions.EqualsAll(new[] {0, 1, 2, 3, 4}, 
                                              GetEnumerator().ToArray()));
    }
  }
}
