using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class ShellStringTests {
    [Test]
    public void TestConstructFromString()
    {
      var s1 = new ShellString("aaa");

      Assert.IsFalse(s1.IsEmpty);
      Assert.AreEqual("aaa", s1.Raw);
      Assert.AreEqual("aaa", s1.Expanded);
    }

    [Test]
    public void TestIsNullOrEmpty()
    {
      Assert.IsTrue(ShellString.IsNullOrEmpty(null));
      Assert.IsTrue(ShellString.IsNullOrEmpty(new ShellString(null)));
      Assert.IsTrue(ShellString.IsNullOrEmpty(new ShellString(string.Empty)));
      Assert.IsFalse(ShellString.IsNullOrEmpty(new ShellString("a")));
    }

    [Test]
    public void TestExpand()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        Assert.AreEqual(null, ShellString.Expand(null));
        Assert.AreEqual(null, ShellString.Expand(new ShellString(null)));
        Assert.AreEqual(string.Empty, ShellString.Expand(new ShellString(string.Empty)));
        Assert.AreEqual("foo", ShellString.Expand(new ShellString("%Smdn.Tests.TestValue1%")));
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

        Assert.AreEqual(s2.Raw, "%Smdn.Tests.TestValue1%");
        Assert.AreEqual(s2.Expanded, "foo");
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

        Assert.IsTrue(str1.Equals("%Smdn.Tests.TestValue1%"));
        Assert.IsTrue(str1.Equals(new ShellString("foo")));
        Assert.IsTrue(str1.Equals("foo"));

        Assert.IsFalse(str1.Equals((ShellString)null));
        Assert.IsFalse(str1.Equals((string)null));
        Assert.IsFalse(str1.Equals((object)1));

        Assert.IsTrue(str1.Equals(str1));
        Assert.IsFalse(str1.Equals(str2));
        Assert.IsTrue(str1.Equals(str3));
        Assert.IsFalse(str1.Equals(str4));

        Assert.IsFalse(str2.Equals(str1));
        Assert.IsTrue(str2.Equals(str2));
        Assert.IsFalse(str2.Equals(str3));
        Assert.IsTrue(str2.Equals(str4));
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

        Assert.IsTrue(x == y);
        Assert.IsTrue(x == new ShellString("%Smdn.Tests.TestValue1%"));
        Assert.IsTrue(x == new ShellString("foo"));
        Assert.IsTrue(new ShellString("%Smdn.Tests.TestValue1%") == x);
        Assert.IsTrue(new ShellString("foo") == x);
        Assert.IsFalse(x == new ShellString("%Smdn.Tests.TestValue2%"));
        Assert.IsFalse(x == new ShellString("bar"));
        Assert.IsFalse(x == null);
        Assert.IsFalse(null == x);
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }

    [Test]
    public void TestOperatorInquality()
    {
      try {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", "foo");

        var x = new ShellString("%Smdn.Tests.TestValue1%");
        var y = x;

        Assert.IsFalse(x != y);
        Assert.IsFalse(x != new ShellString("%Smdn.Tests.TestValue1%"));
        Assert.IsFalse(x != new ShellString("foo"));
        Assert.IsFalse(new ShellString("%Smdn.Tests.TestValue1%") != x);
        Assert.IsFalse(new ShellString("foo") != x);
        Assert.IsTrue(x != new ShellString("%Smdn.Tests.TestValue2%"));
        Assert.IsTrue(x != new ShellString("bar"));
        Assert.IsTrue(x != null);
        Assert.IsTrue(null != x);
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

        Assert.AreEqual(str.ToString(), "%Smdn.Tests.TestValue1%");
      }
      finally {
        Environment.SetEnvironmentVariable("Smdn.Tests.TestValue1", null);
      }
    }
  }
}