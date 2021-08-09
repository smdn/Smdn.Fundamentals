// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

#if !SYSTEM_DIAGNOSTICS_PROCESS
using System.Runtime.InteropServices;
#endif

namespace Smdn {
  [TestFixture]
  public class PlatformTests {
    [Test]
    public void Test()
    {
      TestContext.Out.WriteLine("DistributionName: {0}", Platform.DistributionName);
      TestContext.Out.WriteLine("KernelName: {0}", Platform.KernelName);
      TestContext.Out.WriteLine("ProcessorName: {0}", Platform.ProcessorName);

      TestContext.Out.WriteLine("IsRunningOnWindows: {0}", Platform.IsRunningOnWindows);
      TestContext.Out.WriteLine("IsRunningOnUnix: {0}", Platform.IsRunningOnUnix);

      Assert.Inconclusive("see output");
    }

    [Test]
    public void TestEndianness()
    {
      if (BitConverter.IsLittleEndian)
        Assert.AreEqual(Endianness.LittleEndian, Platform.Endianness);
      else
        Assert.AreNotEqual(Endianness.LittleEndian, Platform.Endianness); // XXX
    }

    [Test]
    public void TestIsRunningOnUnix()
    {
#if SYSTEM_DIAGNOSTICS_PROCESS
      if (string.Empty.Equals(Smdn.OperatingSystem.Shell.Execute("uname")))
        Assert.IsFalse(Platform.IsRunningOnUnix);
      else
        Assert.IsTrue(Platform.IsRunningOnUnix);
#else
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        Assert.IsTrue(Platform.IsRunningOnUnix);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        Assert.IsTrue(Platform.IsRunningOnUnix);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Assert.IsFalse(Platform.IsRunningOnUnix);
      else
        Assert.Ignore("unknown OSPlatform");
#endif
    }

    [Test]
    public void TestIsRunningOnWindows()
    {
#if SYSTEM_DIAGNOSTICS_PROCESS
      if (string.Empty.Equals(Smdn.OperatingSystem.Shell.Execute("VER")))
        Assert.IsFalse(Platform.IsRunningOnWindows);
      else
        Assert.IsTrue(Platform.IsRunningOnWindows);
#else
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        Assert.IsFalse(Platform.IsRunningOnWindows);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        Assert.IsFalse(Platform.IsRunningOnWindows);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Assert.IsTrue(Platform.IsRunningOnWindows);
      else
        Assert.Ignore("unknown OSPlatform");
#endif
    }

    [Test]
    public void TestPathStringComparison()
    {
      if (Platform.IsRunningOnWindows)
        Assert.IsTrue(string.Equals("C:\\path", "C:\\Path", Platform.PathStringComparison));
      else
        Assert.IsFalse(string.Equals("/path", "/Path", Platform.PathStringComparison));
    }

    [Test]
    public void TestPathStringComparer()
    {
      Assert.IsNotNull(Platform.PathStringComparer);

      if (Platform.IsRunningOnWindows)
        Assert.IsTrue(Platform.PathStringComparer.Equals("C:\\path", "C:\\Path"));
      else
        Assert.IsFalse(Platform.PathStringComparer.Equals("/path", "/Path"));
    }

    [Test]
    public void TestDistributionName()
    {
      // returns non-null value always
      Assert.IsNotNull(Platform.DistributionName);
      Assert.IsNotNull(Platform.DistributionName);
      Assert.IsNotNull(Platform.DistributionName);

      var dist = Platform.DistributionName.ToLower();

      if (Platform.IsRunningOnUnix)
        Assert.That(dist, Does.Not.Contain("windows"));
      else
        StringAssert.Contains("windows", dist);
    }

    [Test]
    public void TestKernelName()
    {
      // returns non-null value always
      Assert.IsNotNull(Platform.KernelName);
      Assert.IsNotNull(Platform.KernelName);
      Assert.IsNotNull(Platform.KernelName);
    }

    [Test]
    public void TestProcessorName()
    {
      // returns non-null value always
      Assert.IsNotNull(Platform.ProcessorName);
      Assert.IsNotNull(Platform.ProcessorName);
      Assert.IsNotNull(Platform.ProcessorName);
    }
  }
}
