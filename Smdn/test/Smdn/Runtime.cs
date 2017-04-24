using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

#if NET46
using Smdn.OperatingSystem;
#endif

#if !NET46
using System.Runtime.InteropServices;
#endif

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
#if NET46
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
          StringAssert.Contains("mono", version);
          break;
        default:
          StringAssert.Contains(".net", version);
          break;
      }
    }
#endif

    [Test]
    public void TestIsRunningOnUnix()
    {
#if NET46
      if (string.Empty.Equals(Shell.Execute("uname")))
        Assert.IsFalse(Runtime.IsRunningOnUnix);
      else
        Assert.IsTrue(Runtime.IsRunningOnUnix);
#else
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        Assert.IsTrue(Runtime.IsRunningOnUnix);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        Assert.IsTrue(Runtime.IsRunningOnUnix);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Assert.IsFalse(Runtime.IsRunningOnUnix);
      else
        Assert.Ignore("unknown OSPlatform");
#endif
    }

    [Test]
    public void TestIsRunningOnWindows()
    {
#if NET46
      if (string.Empty.Equals(Shell.Execute("VER")))
        Assert.IsFalse(Runtime.IsRunningOnWindows);
      else
        Assert.IsTrue(Runtime.IsRunningOnWindows);
#else
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        Assert.IsFalse(Runtime.IsRunningOnWindows);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        Assert.IsFalse(Runtime.IsRunningOnWindows);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Assert.IsTrue(Runtime.IsRunningOnWindows);
      else
        Assert.Ignore("unknown OSPlatform");
#endif
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
          StringAssert.Contains("mono", name);
          break;
        case RuntimeEnvironment.NetFx:
          StringAssert.Contains(".net", name);
          break;
        case RuntimeEnvironment.NetCore:
          StringAssert.Contains(".net core", name);
          break;
        default:
#if NET46
          StringAssert.Contains("compatible", name);
#else
          StringAssert.Contains(".net", name);
#endif
          break;
      }
    }

#if NET46
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
#endif
  }
}