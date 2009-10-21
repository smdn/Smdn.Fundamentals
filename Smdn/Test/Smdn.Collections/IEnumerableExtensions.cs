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
