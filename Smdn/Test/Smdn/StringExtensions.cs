using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class StringExtensionsTests {
    [Test]
    public void TestRemove()
    {
      var str = "abcdefghijklmnopqrstuvwxyz";

      Assert.AreEqual(str, str.Remove(new string[] {}), "remove no strings");

      Assert.AreEqual("defghijklpqrstuvw", str.Remove("abc", "mno", "xyz"), "remove strings");
    }

    [Test]
    public void TestRemoveChars()
    {
      var str = "abcdefghijklmnopqrstuvwxyz";

      Assert.AreEqual(str, str.RemoveChars(new char[] {}), "remove no chars");

      Assert.AreEqual("bcdefgijklmnoqrstuvwxy", str.RemoveChars('a', 'h', 'p', 'z'), "remove chars");
    }

    [Test]
    public void TestReplace()
    {
      var str = "abcdefghijklmnopqrstuvwxyz";

      Assert.AreEqual("0Abcdefg7Hijklmno15Pqrstuvwxy25Z", str.Replace(new[] {'a', 'h', 'p', 'z'}, delegate(char c, string s, int i) {
        return string.Format("{0}{1}", i, Char.ToUpper(c));
      }), "replace chars");

      Assert.AreEqual("0ABCdefghijkl13MNOpqrstuvw26XYZ", str.Replace(new[] {"abc", "mno", "xyz"}, delegate(string matched, string s, int i) {
        return string.Format("{0}{1}", i, matched.ToUpper());
      }), "replace strings");
    }

    [Test]
    public void TestStartsWith()
    {
      string nullString = null;

      Assert.Throws<ArgumentNullException>(() => nullString.StartsWith('a'));

      Assert.IsFalse(string.Empty.StartsWith('a'));
      Assert.IsTrue("a".StartsWith('a'));
      Assert.IsTrue("abc".StartsWith('a'));
      Assert.IsFalse("abc".StartsWith('c'));

      Assert.AreEqual(string.Empty.StartsWith("a", StringComparison.Ordinal),
                      string.Empty.StartsWith('a'),
                      "same as StartsWith(string) #1");

      Assert.AreEqual("a".StartsWith("a", StringComparison.Ordinal),
                      "a".StartsWith('a'),
                      "same as StartsWith(string) #2");

      Assert.AreEqual("abc".StartsWith("a", StringComparison.Ordinal),
                      "abc".StartsWith('a'),
                      "same as StartsWith(string) #3");

      Assert.AreEqual("abc".StartsWith("c", StringComparison.Ordinal),
                      "abc".StartsWith('c'),
                      "same as StartsWith(string) #4");
    }

    [Test]
    public void TestEndsWith()
    {
      string nullString = null;

      Assert.Throws<ArgumentNullException>(() => nullString.EndsWith('a'));

      Assert.IsFalse(string.Empty.EndsWith('a'));
      Assert.IsTrue("a".EndsWith('a'));
      Assert.IsTrue("abc".EndsWith('c'));
      Assert.IsFalse("abc".EndsWith('a'));

      Assert.AreEqual(string.Empty.EndsWith("a", StringComparison.Ordinal),
                      string.Empty.EndsWith('a'),
                      "same as EndsWith(string) #1");

      Assert.AreEqual("a".EndsWith("a", StringComparison.Ordinal),
                      "a".EndsWith('a'),
                      "same as EndsWith(string) #2");

      Assert.AreEqual("abc".EndsWith("c", StringComparison.Ordinal),
                      "abc".EndsWith('c'),
                      "same as EndsWith(string) #3");

      Assert.AreEqual("abc".EndsWith("a", StringComparison.Ordinal),
                      "abc".EndsWith('a'),
                      "same as EndsWith(string) #4");
    }

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

      Assert.AreEqual("from", ex.ParamName, "#1");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(3, 4), "#2");

      Assert.AreEqual("from", ex.ParamName, "#2");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(1, 0), "#3");

      Assert.AreEqual("to", ex.ParamName, "#3");

      ex = Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Slice(0, 4), "#4");

      Assert.AreEqual("to", ex.ParamName, "#4");
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

      Assert.AreEqual("startIndex", ex.ParamName, "#1");

      Assert.Throws<ArgumentException>(() => "abc".IndexOfNot('a', 4));
    }
  }
}