using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class CharwiseStringComparerTests {
    private void Compare(CharwiseStringComparer comparerActual)
    {
      var comparerExpected = StringComparer.InvariantCultureIgnoreCase;

      Assert.AreEqual(comparerExpected.Compare(null, null),
                      comparerActual.Compare(null, null));

      Assert.AreEqual(comparerExpected.Compare(null, string.Empty),
                      comparerActual.Compare(null, string.Empty));

      Assert.AreEqual(comparerExpected.Compare(string.Empty, null),
                      comparerActual.Compare(string.Empty, null));

      Assert.AreEqual(comparerExpected.Compare(string.Empty, string.Empty),
                      comparerActual.Compare(string.Empty, string.Empty));

      Assert.AreEqual(comparerExpected.Compare("a", "ab"),
                      comparerActual.Compare("a", "ab"));

      Assert.AreEqual(comparerExpected.Compare("ab", "a"),
                      comparerActual.Compare("ab", "a"));

      Assert.AreEqual(comparerExpected.Compare("abc", "abc"),
                      comparerActual.Compare("abc", "abc"));

      Assert.AreEqual(comparerExpected.Compare("ab", "ABC"),
                      comparerActual.Compare("ab", "ABC"));
    }

    [Test]
    public void TestCompareIgnoreCase()
    {
      Compare(CharwiseStringComparer.IgnoreCase);

      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("ABC", "ABC"));
      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("abc", "abc"));

      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("abc", "ABC"));
      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("ABC", "abc"));
      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("abC", "abc"));
      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("abc", "abC"));
    }

    [Test]
    public void TestCompareConsiderCase()
    {
      Compare(CharwiseStringComparer.ConsiderCase);

      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("ABC", "ABC"));
      Assert.AreEqual(0, CharwiseStringComparer.IgnoreCase.Compare("abc", "abc"));

      Assert.Greater (CharwiseStringComparer.ConsiderCase.Compare("abc", "ABC"), 0);
      Assert.Less    (CharwiseStringComparer.ConsiderCase.Compare("ABC", "abc"), 0);
      Assert.Less    (CharwiseStringComparer.ConsiderCase.Compare("abC", "abc"), 0);
      Assert.Greater (CharwiseStringComparer.ConsiderCase.Compare("abc", "abC"), 0);
    }

    [Test]
    public void TestGetHashCodeIgnoreCase()
    {
      Assert.AreEqual(CharwiseStringComparer.IgnoreCase.GetHashCode(string.Empty),
                      CharwiseStringComparer.IgnoreCase.GetHashCode(string.Empty));
      Assert.AreEqual(CharwiseStringComparer.IgnoreCase.GetHashCode("abc"),
                      CharwiseStringComparer.IgnoreCase.GetHashCode("abc"));
      Assert.AreEqual(CharwiseStringComparer.IgnoreCase.GetHashCode("abc"),
                      CharwiseStringComparer.IgnoreCase.GetHashCode("Abc"));
      Assert.AreEqual(CharwiseStringComparer.IgnoreCase.GetHashCode("abc"),
                      CharwiseStringComparer.IgnoreCase.GetHashCode("ABC"));
    }

    [Test]
    public void TestGetHashCodeConsiderCase()
    {
      Assert.AreEqual(CharwiseStringComparer.ConsiderCase.GetHashCode(string.Empty),
                      CharwiseStringComparer.ConsiderCase.GetHashCode(string.Empty));
      Assert.AreEqual(CharwiseStringComparer.ConsiderCase.GetHashCode("abc"),
                      CharwiseStringComparer.ConsiderCase.GetHashCode("abc"));
      Assert.AreNotEqual(CharwiseStringComparer.ConsiderCase.GetHashCode("abc"),
                         CharwiseStringComparer.ConsiderCase.GetHashCode("Abc"));
      Assert.AreNotEqual(CharwiseStringComparer.ConsiderCase.GetHashCode("abc"),
                         CharwiseStringComparer.ConsiderCase.GetHashCode("ABC"));
    }

    private void Equals(CharwiseStringComparer comparerActual)
    {
      var comparerExpected = StringComparer.InvariantCultureIgnoreCase;

      Assert.AreEqual(comparerExpected.Equals(null, null),
                      comparerActual.Equals(null, null));

      Assert.AreEqual(comparerExpected.Equals(null, string.Empty),
                      comparerActual.Equals(null, string.Empty));

      Assert.AreEqual(comparerExpected.Equals(string.Empty, null),
                      comparerActual.Equals(string.Empty, null));

      Assert.AreEqual(comparerExpected.Equals(string.Empty, string.Empty),
                      comparerActual.Equals(string.Empty, string.Empty));

      Assert.AreEqual(comparerExpected.Equals("a", "ab"),
                      comparerActual.Equals("a", "ab"));

      Assert.AreEqual(comparerExpected.Equals("ab", "a"),
                      comparerActual.Equals("ab", "a"));

      Assert.AreEqual(comparerExpected.Equals("abc", "abc"),
                      comparerActual.Equals("abc", "abc"));

      Assert.AreEqual(comparerExpected.Equals("ab", "ABC"),
                      comparerActual.Equals("ab", "ABC"));
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
      Equals(CharwiseStringComparer.IgnoreCase);

      Assert.IsTrue(CharwiseStringComparer.IgnoreCase.Equals("ABC", "ABC"));
      Assert.IsTrue(CharwiseStringComparer.IgnoreCase.Equals("abc", "abc"));

      Assert.IsTrue(CharwiseStringComparer.IgnoreCase.Equals("abc", "ABC"));
      Assert.IsTrue(CharwiseStringComparer.IgnoreCase.Equals("ABC", "abc"));
      Assert.IsTrue(CharwiseStringComparer.IgnoreCase.Equals("abC", "abc"));
      Assert.IsTrue(CharwiseStringComparer.IgnoreCase.Equals("abc", "abC"));
    }

    [Test]
    public void TestEqualsConsiderCase()
    {
      Equals(CharwiseStringComparer.ConsiderCase);

      Assert.IsTrue(CharwiseStringComparer.ConsiderCase.Equals("ABC", "ABC"));
      Assert.IsTrue(CharwiseStringComparer.ConsiderCase.Equals("abc", "abc"));

      Assert.IsFalse(CharwiseStringComparer.ConsiderCase.Equals("abc", "ABC"));
      Assert.IsFalse(CharwiseStringComparer.ConsiderCase.Equals("ABC", "abc"));
      Assert.IsFalse(CharwiseStringComparer.ConsiderCase.Equals("abC", "abc"));
      Assert.IsFalse(CharwiseStringComparer.ConsiderCase.Equals("abc", "abC"));
    }
  }
}
