// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class StringReplacementExtensionsTests {
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
}
