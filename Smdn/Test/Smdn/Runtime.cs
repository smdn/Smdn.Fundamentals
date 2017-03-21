using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
    [Test]
    public void TestVersionString()
    {
      // returns non-null value always
      Assert.IsNotNull(Runtime.VersionString);
      Assert.IsNotNull(Runtime.VersionString);
      Assert.IsNotNull(Runtime.VersionString);

      var version = Runtime.VersionString.ToLower();

      switch (Runtime.RuntimeEnvironment) {
        case RuntimeEnvironment.Mono:
          Assert.IsTrue(version.Contains("mono"));
          break;
        default:
          Assert.IsTrue(version.Contains(".net"));
          break;
      }
    }

    [Test]
    public void TestIsRunningOnUnix()
    {
      if (string.Empty.Equals(Shell.Execute("uname")))
        Assert.IsFalse(Runtime.IsRunningOnUnix);
      else
        Assert.IsTrue(Runtime.IsRunningOnUnix);
    }

    [Test]
    public void TestIsRunningOnWindows()
    {
      if (string.Empty.Equals(Shell.Execute("VER")))
        Assert.IsFalse(Runtime.IsRunningOnWindows);
      else
        Assert.IsTrue(Runtime.IsRunningOnWindows);
    }

    [Test]
    public void TestName()
    {
      // returns non-null value always
      Assert.IsNotNull(Runtime.Name);
      Assert.IsNotNull(Runtime.Name);
      Assert.IsNotNull(Runtime.Name);

      var name = Runtime.Name.ToLower();

      switch (Runtime.RuntimeEnvironment) {
        case RuntimeEnvironment.Mono:
          Assert.IsTrue(name.Contains("mono"));
          break;
        case RuntimeEnvironment.NetFx:
          Assert.IsTrue(name.Contains(".net"));
          break;
        default:
          Assert.IsTrue(name.Contains("unknown"));
          break;
      }
    }

    [Test]
    public void TestVersion()
    {
      if (Runtime.IsRunningOnMono) {
        var version = Runtime.Version;

        Assert.IsTrue(version.Major != 0 || version.Minor != 0);

        StringAssert.Contains(version.ToString(), Runtime.VersionString);
      }
      else {
        Assert.AreEqual(Environment.Version, Runtime.Version);
      }
    }
  }
}