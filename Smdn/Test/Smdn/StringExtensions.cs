using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class StringExtensionsTests {
    [Test]
    public void TestRemove()
    {
      var str = "abcdefghijklmnopqrstuvwxyz";

      Assert.AreEqual(str, str.Remove(new char[] {}), "remove no chars");
      Assert.AreEqual(str, str.Remove(new string[] {}), "remove no strings");

      Assert.AreEqual("bcdefgijklmnoqrstuvwxy", str.Remove('a', 'h', 'p', 'z'), "remove chars");
      Assert.AreEqual("defghijklpqrstuvw", str.Remove("abc", "mno", "xyz"), "remove strings");
    }

    [Test]
    public void TestReplace()
    {
      var str = "abcdefghijklmnopqrstuvwxyz";

      Assert.AreEqual("0Abcdefg8Hijklmno17Pqrstuvwxy29Z", str.Replace(new[] {'a', 'h', 'p', 'z'}, delegate(char c, string s, int i) {
        return string.Format("{0}{1}", i, Char.ToUpper(c));
      }), "replace chars");

      Assert.AreEqual("0ABCdefghijkl13MNOpqrstuvw26XYZ", str.Replace(new[] {"abc", "mno", "xyz"}, delegate(string matched, string s, int i) {
        return string.Format("{0}{1}", i, matched.ToUpper());
      }), "replace strings");
    }
  }
}