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

    Assert.That(str.Remove(new string[] {}), Is.EqualTo(str), "remove no strings");

    Assert.That(str.Remove("abc", "mno", "xyz"), Is.EqualTo("defghijklpqrstuvw"), "remove strings");
  }

  [Test]
  public void TestRemoveChars()
  {
    var str = "abcdefghijklmnopqrstuvwxyz";

    Assert.That(str.RemoveChars(new char[] {}), Is.EqualTo(str), "remove no chars");

    Assert.That(str.RemoveChars('a', 'h', 'p', 'z'), Is.EqualTo("bcdefgijklmnoqrstuvwxy"), "remove chars");
  }

  [Test]
  public void TestReplace()
  {
    var str = "abcdefghijklmnopqrstuvwxyz";

    Assert.That(str.Replace(new[] {'a', 'h', 'p', 'z'}, delegate(char c, string s, int i) {
      return string.Format(null, "{0}{1}", i, Char.ToUpperInvariant(c));
    }), Is.EqualTo("0Abcdefg7Hijklmno15Pqrstuvwxy25Z"), "replace chars");

    Assert.That(str.Replace(new[] {"abc", "mno", "xyz"}, delegate(string matched, string s, int i) {
      return string.Format(null, "{0}{1}", i, matched.ToUpperInvariant());
    }), Is.EqualTo("0ABCdefghijkl13MNOpqrstuvw26XYZ"), "replace strings");
  }

  [Test]
  public void TestReplace_InputEmpty()
  {
    Assert.That(string.Empty.Replace(new[] {'a', 'h', 'p', 'z'}, delegate(char c, string s, int i) {
      return string.Format(null, "{0}{1}", i, Char.ToUpperInvariant(c));
    }), Is.EqualTo(string.Empty), "replace chars");

    Assert.That(string.Empty.Replace(new[] {"abc", "mno", "xyz"}, delegate(string matched, string s, int i) {
      return string.Format(null, "{0}{1}", i, matched.ToUpperInvariant());
    }), Is.EqualTo(string.Empty), "replace strings");
  }

  [Test]
  public void TestReplace_OldValuesEmpty()
  {
    var str = "abcdefghijklmnopqrstuvwxyz";

    Assert.That(str.Replace(new char[0], delegate(char c, string s, int i) {
      return string.Format(null, "{0}{1}", i, Char.ToUpperInvariant(c));
    }), Is.EqualTo(str), "replace chars");

    Assert.That(str.Replace(new string[0], delegate(string matched, string s, int i) {
      return string.Format(null, "{0}{1}", i, matched.ToUpperInvariant());
    }), Is.EqualTo(str), "replace strings");
  }

  [Test]
  public void TestReplace_ReplaceToNull()
  {
    var str = "abcdefghijklmnopqrstuvwxyz";

    Assert.That(str.Replace(new[] {"abc", "mno", "xyz"}, delegate(string matched, string s, int i) {
      return null;
    }), Is.EqualTo("defghijklpqrstuvw"), "replace strings");
  }

  [Test]
  public void TestReplace_ReplaceToEmpty()
  {
    var str = "abcdefghijklmnopqrstuvwxyz";

    Assert.That(str.Replace(new[] {"abc", "mno", "xyz"}, delegate(string matched, string s, int i) {
      return string.Empty;
    }), Is.EqualTo("defghijklpqrstuvw"), "replace strings");
  }
}
