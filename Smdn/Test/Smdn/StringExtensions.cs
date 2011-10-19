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

      try {
        "abc".Slice(-1, 0);
        Assert.Fail("ArgumentOutOfRangeException not thrown #1");
      }
      catch (ArgumentOutOfRangeException ex) {
        Assert.AreEqual("from", ex.ParamName, "#1");
      }

      try {
        "abc".Slice(3, 4);
        Assert.Fail("ArgumentOutOfRangeException not thrown #2");
      }
      catch (ArgumentOutOfRangeException ex) {
        Assert.AreEqual("from", ex.ParamName, "#2");
      }

      try {
        "abc".Slice(1, 0);
        Assert.Fail("ArgumentOutOfRangeException not thrown #3");
      }
      catch (ArgumentOutOfRangeException ex) {
        Assert.AreEqual("to", ex.ParamName, "#3");
      }

      try {
        "abc".Slice(0, 4);
        Assert.Fail("ArgumentOutOfRangeException not thrown #4");
      }
      catch (ArgumentOutOfRangeException ex) {
        Assert.AreEqual("to", ex.ParamName, "#4");
      }
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

      try {
        "abc".IndexOfNot('a', -1);
        Assert.Fail("ArgumentOutOfRangeException not thrown #1");
      }
      catch (ArgumentOutOfRangeException ex) {
        Assert.AreEqual("startIndex", ex.ParamName, "#1");
      }

      try {
        "abc".IndexOfNot('a', 4);
        Assert.Fail("ArgumentException not thrown #2");
      }
      catch (ArgumentException) {
      }
    }
  }
}