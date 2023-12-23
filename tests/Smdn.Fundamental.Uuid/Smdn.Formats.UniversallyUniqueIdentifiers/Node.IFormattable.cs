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
#pragma warning disable CA1305
    var regexFormat_X = new Regex("^[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");
    var regexFormat_x = new Regex("^[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}$");
    var regexFormat_Default = regexFormat_X;

    for (var n = 0; n < 1000; n++) {
      var node = Node.CreateRandom();

      Assert.That(regexFormat_Default.IsMatch(node.ToString()), Is.True, node.ToString());
      Assert.That(regexFormat_Default.IsMatch(node.ToString(null)), Is.True, node.ToString(null));
      Assert.That(regexFormat_Default.IsMatch(node.ToString(string.Empty)), Is.True, node.ToString(string.Empty));
      Assert.That(regexFormat_Default.IsMatch(node.ToString(null, formatProvider: null)), Is.True, node.ToString(null, formatProvider: null));
      Assert.That(regexFormat_X.IsMatch(node.ToString("X")), Is.True, node.ToString("X"));
      Assert.That(regexFormat_x.IsMatch(node.ToString("x")), Is.True, node.ToString("x"));
    }
#pragma warning restore CA1305
  }

  [Test]
  public void TestToString_InvalidFormat()
  {
#pragma warning disable CA1305
    var node = Node.CreateRandom();

    Assert.Throws<FormatException>(() => node.ToString("n"));
    Assert.Throws<FormatException>(() => node.ToString("xx"));
    Assert.Throws<FormatException>(() => node.ToString("XX"));
#pragma warning restore CA1305
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

      Assert.That(node.TryFormat(destination, out var charsWritten, format, provider: null), Is.True, $"TryFormat {node}");
      Assert.That(charsWritten, Is.EqualTo(17), $"{nameof(charsWritten)} {node}");
      Assert.That(expected.IsMatch(new string(destination, 0, charsWritten)), Is.True, $"destination {node}");
    }
  }

  [Test]
  public void TestTryFormat_DestinationTooShort([Values(0, 1, 16)] int length)
  {
    Assert.That(Node.CreateRandom().TryFormat(new char[length], out var charsWritten1, "X", provider: null), Is.False, "format X");
    Assert.That(charsWritten1, Is.EqualTo(0), "format X");

    Assert.That(Node.CreateRandom().TryFormat(new char[length], out var charsWritten2, "x", provider: null), Is.False, "format x");
    Assert.That(charsWritten2, Is.EqualTo(0), "format x");
  }

  [TestCase("n")]
  [TestCase("xx")]
  [TestCase("XX")]
  public void TestTryFormat_InvalidFormat(string format)
  {
#pragma warning disable CA1305
    var node = Node.CreateRandom();
    var destination = new char[17];

    Assert.That(node.TryFormat(destination, out _, format, provider: null), Is.True);
    Assert.That(node.ToString("X"), Is.EqualTo(new string(destination)));
#pragma warning restore CA1305
  }
#endif
}
