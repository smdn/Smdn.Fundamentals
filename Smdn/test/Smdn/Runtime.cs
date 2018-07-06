using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

#if NET
using Smdn.OperatingSystem;
#endif

#if !NET
using System.Runtime.InteropServices;
#endif

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
    [Test]
    public void TestRuntimeEnvironment()
    {
      Console.WriteLine("RuntimeEnvironment: {0} ({1})\r\nRuntimeVersion: {2}\r\n",
                        Runtime.RuntimeEnvironment,
                        Runtime.VersionString,
                        Runtime.Version);

      Assert.Inconclusive("see output");
    }

#if NET
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
#if NET
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
#if NET
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
#if NET
          StringAssert.Contains("compatible", name);
#else
          StringAssert.Contains(".net", name);
#endif
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
#if NET || NETSTANDARD2_0
        Assert.AreEqual(Environment.Version, Runtime.Version);
#endif
      }
    }
  }
}