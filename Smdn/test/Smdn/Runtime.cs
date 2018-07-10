using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
    [Test]
    public void TestRuntimeEnvironment()
    {
      TestContext.Out.WriteLine("RuntimeEnvironment: {0}", Runtime.RuntimeEnvironment);
      TestContext.Out.WriteLine("Name: {0}", Runtime.Name);
      TestContext.Out.WriteLine("Version: {0}", Runtime.Version);
      TestContext.Out.WriteLine("VersionString: {0}", Runtime.VersionString);

      Assert.Inconclusive("see output");
    }

    [Test]
    public void TestIsRunningOnRuntime()
    {
      switch (Runtime.RuntimeEnvironment) {
        case RuntimeEnvironment.NetFx:
          Assert.IsTrue(Runtime.IsRunningOnNetFx);
          Assert.IsFalse(Runtime.IsRunningOnNetCore);
          Assert.IsFalse(Runtime.IsRunningOnMono);
          break;
        case RuntimeEnvironment.NetCore:
          Assert.IsTrue(Runtime.IsRunningOnNetCore);
          Assert.IsFalse(Runtime.IsRunningOnNetFx);
          Assert.IsFalse(Runtime.IsRunningOnMono);
          break;
        case RuntimeEnvironment.Mono:
          Assert.IsTrue(Runtime.IsRunningOnMono);
          Assert.IsFalse(Runtime.IsRunningOnNetFx);
          Assert.IsFalse(Runtime.IsRunningOnNetCore);
          break;

        default:
          Assert.IsFalse(Runtime.IsRunningOnMono);
          Assert.IsFalse(Runtime.IsRunningOnNetFx);
          Assert.IsFalse(Runtime.IsRunningOnNetCore);
          break;
      }

      TestContext.Out.WriteLine("IsRunningOnNetFx: {0}", Runtime.IsRunningOnNetFx);
      TestContext.Out.WriteLine("IsRunningOnNetCore: {0}", Runtime.IsRunningOnNetCore);
      TestContext.Out.WriteLine("IsRunningOnMono: {0}", Runtime.IsRunningOnMono);

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
      Assert.IsNotNull(Runtime.Version);

      if (Runtime.IsRunningOnMono) {
        var version = Runtime.Version;

        Assert.IsTrue(version.Major != 0 || version.Minor != 0);

        StringAssert.Contains(version.ToString(), Runtime.VersionString);
      }
    }
  }
}
