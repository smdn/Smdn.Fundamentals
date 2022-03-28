// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class StringShimTests {
  [Test]
  public void TestStartsWith()
  {
#if !SYSTEM_STRING_STARTSWITH_CHAR
    string nullString = null;

    Assert.Throws<ArgumentNullException>(() => nullString.StartsWith('a'));
#endif

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
#if !SYSTEM_STRING_ENDSWITH_CHAR
    string nullString = null;

    Assert.Throws<ArgumentNullException>(() => nullString.EndsWith('a'));
#endif

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
}
