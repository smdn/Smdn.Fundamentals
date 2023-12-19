// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

partial class NodeTests {
  [Test]
  public void TestParse_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => Node.Parse((string)null!, provider: null));

#if FEATURE_GENERIC_MATH
    Assert.Throws<ArgumentNullException>(() => Parse<Node>(null, provider: null), "IParsable");

    static T Parse<T>(string s, IFormatProvider provider) where T : IParsable<T> => T.Parse(s, provider);
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
      Assert.That(n.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()));
      Assert.That(n.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()));
    }

#if FEATURE_GENERIC_MATH
    if (expectValid)
      Assert.DoesNotThrow(() => n = Parse<Node>(s, provider: null), "IParsable");
    else
      Assert.Throws<FormatException>(() => n = Parse<Node>(s, provider: null), "IParsable");

    if (expectValid) {
      Assert.That(n.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()), "IParsable");
      Assert.That(n.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()), "IParsable");
    }

    static T Parse<T>(string s, IFormatProvider provider) where T : IParsable<T> => T.Parse(s, provider);
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
  public void TestParse_ISpanParsable(string s, bool expectValid, string expectedString)
  {
    Node n = default;

    if (expectValid)
      Assert.DoesNotThrow(() => n = Node.Parse(s.AsSpan(), provider: null));
    else
      Assert.Throws<FormatException>(() => n = Node.Parse(s.AsSpan(), provider: null));

    if (expectValid) {
      Assert.That(n.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()));
      Assert.That(n.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()));
    }

#if FEATURE_GENERIC_MATH
    if (expectValid)
      Assert.DoesNotThrow(() => n = Parse<Node>(s.AsSpan(), provider: null), "IParsable");
    else
      Assert.Throws<FormatException>(() => n = Parse<Node>(s.AsSpan(), provider: null), "IParsable");

    if (expectValid) {
      Assert.That(n.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()), "IParsable");
      Assert.That(n.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()), "IParsable");
    }

    static T Parse<T>(ReadOnlySpan<char> s, IFormatProvider provider) where T : ISpanParsable<T> => T.Parse(s, provider);
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
    Assert.That(Node.TryParse(s, out var node), Is.EqualTo(expectValid));

    if (expectValid) {
      Assert.That(node.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()));
      Assert.That(node.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()));
    }

#if FEATURE_GENERIC_MATH
    Assert.That(TryParse<Node>(s, out var node2), Is.EqualTo(expectValid), "IParsable");

    if (expectValid) {
      Assert.That(node2.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()), "IParsable");
      Assert.That(node2.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()), "IParsable");
    }

    static bool TryParse<T>(string s, out T result) where T : IParsable<T> => T.TryParse(s, provider: null, out result);
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
  public void TestTryParse_ISpanParsable(string s, bool expectValid, string expectedString)
  {
    Assert.That(Node.TryParse(s.AsSpan(), out var node), Is.EqualTo(expectValid));

    if (expectValid) {
      Assert.That(node.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()));
      Assert.That(node.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()));
    }

#if FEATURE_GENERIC_MATH
    Assert.That(TryParse<Node>(s.AsSpan(), out var node2), Is.EqualTo(expectValid), "IParsable");

    if (expectValid) {
      Assert.That(node2.ToString("X"), Is.EqualTo(expectedString.ToUpperInvariant()), "IParsable");
      Assert.That(node2.ToString("x"), Is.EqualTo(expectedString.ToLowerInvariant()), "IParsable");
    }

    static bool TryParse<T>(ReadOnlySpan<char> s, out T result) where T : ISpanParsable<T> => T.TryParse(s, provider: null, out result);
#endif
  }
}
