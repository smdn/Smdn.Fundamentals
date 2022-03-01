// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [TestFixture]
  public class NodeTests {
    [Test]
    public void TestCreateRandom()
    {
      var regexRandomNode = new Regex("^[0-9A-F][13579BDF]:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");

      for (var n = 0; n < 1000; n++) {
        var node = Node.CreateRandom();

        Assert.IsTrue(regexRandomNode.IsMatch(node.ToString()), node.ToString());
      }
    }

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
  }
}