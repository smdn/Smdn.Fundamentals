// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

partial class NodeTests {
  [Test]
  public void TestParse_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => Node.Parse(null, provider: null));

#if FEATURE_GENERIC_MATH
    Assert.Throws<ArgumentNullException>(() => Parse<Node>(null, provider: null), "IParseable");

    static T Parse<T>(string s, IFormatProvider provider) where T : IParseable<T> => T.Parse(s, provider);
#endif
  }

  [TestCase("00:00:00:00:00:00", true, "00:00:00:00:00:00")]
  [TestCase("01:23:45:67:89:AB", true, "01:23:45:67:89:AB")]
  [TestCase("FF:FF:FF:FF:FF:FF", true, "FF:FF:FF:FF:FF:FF")]
  [TestCase("ab:cd:ef:AB:CD:EF", true, "ab:cd:ef:AB:CD:EF")]
  [TestCase("0:1:2:3:4:F", true, "00:01:02:03:04:0F")]
  [TestCase("000:001:002:003:004:00F", true, "00:01:02:03:04:0F")]
  [TestCase("00:00:00:00:00", false, null)]
  [TestCase("00", false, null)]
  [TestCase("", false, null)]
  [TestCase("100:00:00:00:00:00", false, null)]
  [TestCase("00:00:00:00:00:100", false, null)]
  [TestCase("00:00:00:00:00:0X", false, null)]
  [TestCase("00-00-00-00-00-00", false, null)]
  [TestCase("00:00:00:00:00-00", false, null)]
  public void TestParse(string s, bool expectValid, string expectedString)
  {
    Node n = default;

    if (expectValid)
      Assert.DoesNotThrow(() => n = Node.Parse(s, provider: null));
    else
      Assert.Throws<FormatException>(() => n = Node.Parse(s, provider: null));

    if (expectValid) {
      Assert.AreEqual(expectedString.ToUpperInvariant(), n.ToString("X"));
      Assert.AreEqual(expectedString.ToLowerInvariant(), n.ToString("x"));
    }

#if FEATURE_GENERIC_MATH
    if (expectValid)
      Assert.DoesNotThrow(() => n = Parse<Node>(s, provider: null), "IParseable");
    else
      Assert.Throws<FormatException>(() => n = Parse<Node>(s, provider: null), "IParseable");

    if (expectValid) {
      Assert.AreEqual(expectedString.ToUpperInvariant(), n.ToString("X"), "IParseable");
      Assert.AreEqual(expectedString.ToLowerInvariant(), n.ToString("x"), "IParseable");
    }

    static T Parse<T>(string s, IFormatProvider provider) where T : IParseable<T> => T.Parse(s, provider);
#endif
  }

  [TestCase("00:00:00:00:00:00", true, "00:00:00:00:00:00")]
  [TestCase("01:23:45:67:89:AB", true, "01:23:45:67:89:AB")]
  [TestCase("FF:FF:FF:FF:FF:FF", true, "FF:FF:FF:FF:FF:FF")]
  [TestCase("ab:cd:ef:AB:CD:EF", true, "ab:cd:ef:AB:CD:EF")]
  [TestCase("0:1:2:3:4:F", true, "00:01:02:03:04:0F")]
  [TestCase("000:001:002:003:004:00F", true, "00:01:02:03:04:0F")]
  [TestCase("00:00:00:00:00", false, null)]
  [TestCase("00", false, null)]
  [TestCase("", false, null)]
  [TestCase(null, false, null)]
  [TestCase("100:00:00:00:00:00", false, null)]
  [TestCase("00:00:00:00:00:100", false, null)]
  [TestCase("00:00:00:00:00:0X", false, null)]
  [TestCase("00-00-00-00-00-00", false, null)]
  [TestCase("00:00:00:00:00-00", false, null)]
  public void TestTryParse(string s, bool expectValid, string expectedString)
  {
    Assert.AreEqual(expectValid, Node.TryParse(s, out var node));

    if (expectValid) {
      Assert.AreEqual(expectedString.ToUpperInvariant(), node.ToString("X"));
      Assert.AreEqual(expectedString.ToLowerInvariant(), node.ToString("x"));
    }

#if FEATURE_GENERIC_MATH
    Assert.AreEqual(expectValid, TryParse<Node>(s, out var node2), "IParseable");

    if (expectValid) {
      Assert.AreEqual(expectedString.ToUpperInvariant(), node2.ToString("X"), "IParseable");
      Assert.AreEqual(expectedString.ToLowerInvariant(), node2.ToString("x"), "IParseable");
    }

    static bool TryParse<T>(string s, out T result) where T : IParseable<T> => T.TryParse(s, provider: null, out result);
#endif
  }
}
