// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
    [Test]
    public void OutputRuntimeEnvironment()
    {
      TestContext.Out.WriteLine($"{nameof(RuntimeInformation)}.{nameof(RuntimeInformation.FrameworkDescription)}: {RuntimeInformation.FrameworkDescription}");

#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
      TestContext.Out.WriteLine(
        "ReferencedAssemblies: {0}",
        string.Join(
          ", ",
          typeof(Runtime)
            .GetTypeInfo()
            .Assembly
            .GetReferencedAssemblies()
            .Select(static a => a.Name)
        )
      );
#endif

#if SYSTEM_ENVIRONMENT_VERSION
      TestContext.Out.WriteLine($"{nameof(Environment)}.{nameof(Environment.Version)}: {Environment.Version}");
#endif

      TestContext.Out.WriteLine($"{nameof(Runtime.RuntimeEnvironment)}: {Runtime.RuntimeEnvironment}");
      TestContext.Out.WriteLine($"{nameof(Runtime.Name)}: {Runtime.Name}");
      TestContext.Out.WriteLine($"{nameof(Runtime.Version)}: {Runtime.Version}");
      TestContext.Out.WriteLine($"{nameof(Runtime.VersionString)}: {Runtime.VersionString}");

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

      TestContext.Out.WriteLine($"{nameof(Runtime.IsRunningOnNetFx)}: {Runtime.IsRunningOnNetFx}");
      TestContext.Out.WriteLine($"{nameof(Runtime.IsRunningOnNetCore)}: {Runtime.IsRunningOnNetCore}");
      TestContext.Out.WriteLine($"{nameof(Runtime.IsRunningOnMono)}: {Runtime.IsRunningOnMono}");

      Assert.Inconclusive("see output");
    }

    [Test]
    public void TestVersionString()
    {
      // returns non-null value always
      Assert.IsNotNull(Runtime.VersionString);

      var version = Runtime.VersionString.ToLower();

      switch (Runtime.RuntimeEnvironment) {
        case RuntimeEnvironment.Mono:
          StringAssert.Contains("mono", version);
          break;
        case RuntimeEnvironment.NetFx:
          StringAssert.Contains(".net framework", version);
          break;
        case RuntimeEnvironment.NetCore:
          StringAssert.Contains(".net", version); // .NET Core, .NET
          break;
        default:
          StringAssert.Contains(".net", version);
          break;
      }
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
          StringAssert.Contains(".net framework", name);
          break;
        case RuntimeEnvironment.NetCore:
          StringAssert.Contains(".net", name);
          break;
        default:
#if NETFRAMEWORK
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
