// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.OperatingSystem {
  [TestFixture]
  public class ShellStringTests {
    [Test]
    public void TestConstructFromString()
    {
      var s1 = new ShellString("aaa");

      Assert.That(s1.IsEmpty, Is.False);
      Assert.That(s1.Raw, Is.EqualTo("aaa"));
      Assert.That(s1.Expanded, Is.EqualTo("aaa"));
    }

    [Test]
    public void TestClone()
    {
      var str = new ShellString("aaa");
      var cloned = str.Clone();

      Assert.That(object.ReferenceEquals(str, cloned), Is.False);
      Assert.That(cloned.Raw, Is.EqualTo(str.Raw));

      str.Raw = "hoge";

      Assert.That(cloned.Raw, Is.Not.EqualTo(str.Raw));
    }

    [Test]
    public void TestIsNullOrEmpty()
    {
      Assert.That(ShellString.IsNullOrEmpty(null), Is.True);
      Assert.That(ShellString.IsNullOrEmpty(new ShellString(null)), Is.True);
      Assert.That(ShellString.IsNullOrEmpty(new ShellString(string.Empty)), Is.True);
      Assert.That(ShellString.IsNullOrEmpty(new ShellString("a")), Is.False);
    }

    [Test]
    public void TestExpand()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        Assert.That(ShellString.Expand(null), Is.EqualTo(null));
        Assert.That(ShellString.Expand(new ShellString(null)), Is.EqualTo(null));
        Assert.That(ShellString.Expand(new ShellString(string.Empty)), Is.EqualTo(string.Empty));
        Assert.That(ShellString.Expand(new ShellString("%Smdn.Tests.TestValue1%")), Is.EqualTo("foo"));
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }

    [Test]
    public void TestExpanded()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        var s2 = new ShellString("%Smdn.Tests.TestValue1%");

        Assert.That(s2.Raw, Is.EqualTo("%Smdn.Tests.TestValue1%"));
        Assert.That(s2.Expanded, Is.EqualTo("foo"));
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }

    [Test]
    public void TestEquals()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue2", "bar");

        var str1 = new ShellString("%Smdn.Tests.TestValue1%");
        var str2 = new ShellString("%Smdn.Tests.TestValue2%");
        var str3 = new ShellString("foo");
        var str4 = new ShellString("bar");

        Assert.That(str1.Equals("%Smdn.Tests.TestValue1%"), Is.True);
        Assert.That(str1.Equals(new ShellString("foo")), Is.True);
        Assert.That(str1.Equals("foo"), Is.True);

        Assert.That(str1.Equals((ShellString)null!), Is.False);
        Assert.That(str1!.Equals((string)null!), Is.False);
        Assert.That(str1!.Equals((object)1), Is.False);

        Assert.That(str1.Equals(str1), Is.True);
        Assert.That(str1.Equals(str2), Is.False);
        Assert.That(str1.Equals(str3), Is.True);
        Assert.That(str1.Equals(str4), Is.False);

        Assert.That(str2.Equals(str1), Is.False);
        Assert.That(str2.Equals(str2), Is.True);
        Assert.That(str2.Equals(str3), Is.False);
        Assert.That(str2.Equals(str4), Is.True);
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue2", null);
      }
    }

    [Test]
    public void TestOperatorEquality()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        var x = new ShellString("%Smdn.Tests.TestValue1%");
        var y = x;

        Assert.That(x == y, Is.True);
        Assert.That(x == new ShellString("%Smdn.Tests.TestValue1%"), Is.True);
        Assert.That(x == new ShellString("foo"), Is.True);
        Assert.That(new ShellString("%Smdn.Tests.TestValue1%") == x, Is.True);
        Assert.That(new ShellString("foo") == x, Is.True);
        Assert.That(x == new ShellString("%Smdn.Tests.TestValue2%"), Is.False);
        Assert.That(x == new ShellString("bar"), Is.False);
        Assert.That(x == null, Is.False);
        Assert.That(null == x, Is.False);
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }

    [Test]
    public void TestOperatorInequality()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        var x = new ShellString("%Smdn.Tests.TestValue1%");
        var y = x;

        Assert.That(x != y, Is.False);
        Assert.That(x != new ShellString("%Smdn.Tests.TestValue1%"), Is.False);
        Assert.That(x != new ShellString("foo"), Is.False);
        Assert.That(new ShellString("%Smdn.Tests.TestValue1%") != x, Is.False);
        Assert.That(new ShellString("foo") != x, Is.False);
        Assert.That(x != new ShellString("%Smdn.Tests.TestValue2%"), Is.True);
        Assert.That(x != new ShellString("bar"), Is.True);
        Assert.That(x != null, Is.True);
        Assert.That(null != x, Is.True);
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }

    [Test]
    public void TestToString()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        var str = new ShellString("%Smdn.Tests.TestValue1%");

        Assert.That(str.ToString(), Is.EqualTo("%Smdn.Tests.TestValue1%"));
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }
  }
}
