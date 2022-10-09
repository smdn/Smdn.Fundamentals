// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class RuntimeTests {
  [Test]
  public void OutputRuntimeEnvironment()
  {
    TestContext.Out.WriteLine("[Runtime environment information]");
    TestContext.Out.WriteLine($"{nameof(RuntimeInformation)}.{nameof(RuntimeInformation.FrameworkDescription)}: {RuntimeInformation.FrameworkDescription}");

#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
    TestContext.Out.WriteLine(
      "ReferencedAssemblies(entry assembly): {0}",
      string.Join(
        ", ",
        Assembly
          .GetEntryAssembly()
          ?.GetReferencedAssemblies()
          ?.Select(static a => a.Name) ?? Enumerable.Empty<string>()
      )
    );
    TestContext.Out.WriteLine(
      "ReferencedAssemblies(Runtime class): {0}",
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
        TestContext.Out.WriteLine($"[Test: {nameof(TestVersionString)}]");
        TestContext.Out.WriteLine($"{nameof(Runtime.VersionString)}: {Runtime.VersionString}");
        Assert.Inconclusive("see output");
        break;
    }
  }

  [Test]
  public void TestName()
  {
    // returns non-null value always
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
        if (Runtime.Version is null) {
          TestContext.Out.WriteLine($"[Test: {nameof(TestName)}]");
          TestContext.Out.WriteLine($"{nameof(Runtime.Name)}: {Runtime.Name}");

          Assert.Inconclusive("see output");
        }
        else {
          if (5 <= Runtime.Version.Major)
            StringAssert.Contains(".net", name);
          else
            StringAssert.Contains(".net core", name);
        }
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
#if SYSTEM_ENVIRONMENT_VERSION
    Assert.IsNotNull(Runtime.Version);
#else
    TestContext.Out.WriteLine($"[Test: {nameof(TestVersion)}]");
    TestContext.Out.WriteLine($"{nameof(Runtime.Version)}: {Runtime.Version}");

    Assert.Inconclusive("see output");

    return;
#endif

    var version = Runtime.Version!;

    Assert.AreNotEqual(0, version.Major, nameof(version.Major));
    Assert.AreNotEqual(-1, version.Minor, nameof(version.Minor));

    switch (Runtime.RuntimeEnvironment) {
      case RuntimeEnvironment.Mono:
        StringAssert.Contains(version.ToString(), Runtime.VersionString, "Mono verion string");
        break;

      case RuntimeEnvironment.NetFx:
        Assert.Less(version.Major, 5, ".NET Framework major verion must be less than 5");
        StringAssert.Contains(version.ToString(), Runtime.VersionString, ".NET Framework verion string");
        break;

      case RuntimeEnvironment.NetCore:
        if (!Runtime.Name.Contains(".NET Core"))
          Assert.GreaterOrEqual(version.Major, 5, ".NET major verion must be greater than or equal to 5");
#if SYSTEM_ENVIRONMENT_VERSION
        Assert.AreEqual(Environment.Version, version, "CoreCLR version must be equal to Environment.Version");
#endif
        StringAssert.Contains(version.ToString(), Runtime.VersionString, "CoreCLR version string");
        break;

      default:
        TestContext.Out.WriteLine($"[Test: {nameof(TestVersion)}]");
        TestContext.Out.WriteLine($"{nameof(Runtime.Version)}: {Runtime.Version}");

        Assert.Inconclusive("see output");
        break;
    }
  }
}
