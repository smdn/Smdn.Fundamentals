// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

partial class NodeTests {
  [Test]
  public void TestToString()
  {
    var regexFormat_X = new Regex("^[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");
    var regexFormat_x = new Regex("^[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}$");
    var regexFormat_Default = regexFormat_X;

    for (var n = 0; n < 1000; n++) {
      var node = Node.CreateRandom();

      Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString()), node.ToString());
      Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString(null)), node.ToString(null));
      Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString(string.Empty)), node.ToString(string.Empty));
      Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString(null, formatProvider: null)), node.ToString(null, formatProvider: null));
      Assert.IsTrue(regexFormat_X.IsMatch(node.ToString("X")), node.ToString("X"));
      Assert.IsTrue(regexFormat_x.IsMatch(node.ToString("x")), node.ToString("x"));
    }
  }

  [Test]
  public void TestToString_InvalidFormat()
  {
    var node = Node.CreateRandom();

    Assert.Throws<FormatException>(() => node.ToString("n"));
    Assert.Throws<FormatException>(() => node.ToString("xx"));
    Assert.Throws<FormatException>(() => node.ToString("XX"));
  }

#if SYSTEM_ISPANFORMATTABLE
  [TestCase("X", "^[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$")]
  [TestCase("x", "^[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}$")]
  public void TestTryFormat(string format, string expectedPattern)
  {
    var expected = new Regex(expectedPattern);

    for (var n = 0; n < 10; n++) {
      var node = Node.CreateRandom();
      var destination = new char[20];

      Assert.IsTrue(node.TryFormat(destination, out var charsWritten, format, provider: null), $"TryFormat {node}");
      Assert.AreEqual(17, charsWritten, $"{nameof(charsWritten)} {node}");
      Assert.IsTrue(expected.IsMatch(new string(destination, 0, charsWritten)), $"destination {node}");
    }
  }

  [Test]
  public void TestTryFormat_DestinationTooShort([Values(0, 1, 16)] int length)
  {
    Assert.IsFalse(Node.CreateRandom().TryFormat(new char[length], out var charsWritten1, "X", provider: null), "format X");
    Assert.AreEqual(0, charsWritten1, "format X");

    Assert.IsFalse(Node.CreateRandom().TryFormat(new char[length], out var charsWritten2, "x", provider: null), "format x");
    Assert.AreEqual(0, charsWritten2, "format x");
  }

  [TestCase("n")]
  [TestCase("xx")]
  [TestCase("XX")]
  public void TestTryFormat_InvalidFormat(string format)
  {
    var node = Node.CreateRandom();
    var destination = new char[17];

    Assert.IsTrue(node.TryFormat(destination, out _, format, provider: null));
    Assert.AreEqual(new string(destination), node.ToString("X"));
  }
#endif
}
