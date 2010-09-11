using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
    private int GetSimdRuntimeAccelMode()
    {
      try {
        Assembly.Load("Mono.Simd, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");

#if true
        const string code = @"
namespace Smdn {
  public static class SimdRuntimeTest {
    public static int GetAccelMode() {
      return (int)Mono.Simd.SimdRuntime.AccelMode;
    }
  }
}";

        using (var provider = new Microsoft.CSharp.CSharpCodeProvider(new System.Collections.Generic.Dictionary<string, string>() {{"CompilerVersion", "v2.0"}})) {
          var options = new System.CodeDom.Compiler.CompilerParameters(new[] {"Mono.Simd"});

          options.GenerateInMemory = true;
          options.IncludeDebugInformation = false;

          var result = provider.CompileAssemblyFromSource(options, code);

          var simdRuntimeType = result.CompiledAssembly.GetType("Smdn.SimdRuntimeTest", true);

          return (int)simdRuntimeType.GetMethod("GetAccelMode", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
        }
#else
        var simdRuntimeType = assm.GetType("Mono.Simd.SimdRuntime", true);

        // this will not return actual acceleration mode
        // (Mono does not emit replaced intrinsics, so this returns AccelMode.None always)
        return (int)simdRuntimeType.GetProperty("AccelMode", BindingFlags.Static | BindingFlags.Public).GetValue(null, null));
#endif
      }
      catch {
        return 0;
      }
    }

    [Test]
    public void TestSimdRuntimeAccelMode()
    {
      Assert.AreEqual(GetSimdRuntimeAccelMode(), Runtime.SimdRuntimeAccelMode);
    }

    [Test]
    public void TestIsSimdRuntimeAvailable()
    {
      // no exception must be thrown
      var available = Runtime.IsSimdRuntimeAvailable;

      Assert.AreEqual(0 < GetSimdRuntimeAccelMode(), Runtime.IsSimdRuntimeAvailable);
    }

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

        Assert.AreNotEqual(0, version.Major);
        Assert.AreNotEqual(0, version.Minor);

        Assert.IsTrue(Runtime.VersionString.Contains(version.ToString()));
      }
      else {
        Assert.AreEqual(Environment.Version, Runtime.Version);
      }
    }
  }
}