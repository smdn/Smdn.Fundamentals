// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0618 // [Obsolete]

using System;
using NUnit.Framework;

using System.Runtime.InteropServices;

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
        Assert.That(Platform.Endianness, Is.EqualTo(Endianness.LittleEndian));
      else
        Assert.That(Platform.Endianness, Is.Not.EqualTo(Endianness.LittleEndian)); // XXX
    }

    [Test]
    public void TestIsRunningOnUnix()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        Assert.That(Platform.IsRunningOnUnix, Is.True);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        Assert.That(Platform.IsRunningOnUnix, Is.True);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Assert.That(Platform.IsRunningOnUnix, Is.False);
      else
        Assert.Ignore("unknown OSPlatform");
    }

    [Test]
    public void TestIsRunningOnWindows()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        Assert.That(Platform.IsRunningOnWindows, Is.False);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        Assert.That(Platform.IsRunningOnWindows, Is.False);
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Assert.That(Platform.IsRunningOnWindows, Is.True);
      else
        Assert.Ignore("unknown OSPlatform");
    }

    [Test]
    public void TestPathStringComparison()
    {
      if (Platform.IsRunningOnWindows)
        Assert.That(string.Equals("C:\\path", "C:\\Path", Platform.PathStringComparison), Is.True);
      else
        Assert.That(string.Equals("/path", "/Path", Platform.PathStringComparison), Is.False);
    }

    [Test]
    public void TestPathStringComparer()
    {
      Assert.That(Platform.PathStringComparer, Is.Not.Null);

      if (Platform.IsRunningOnWindows)
        Assert.That(Platform.PathStringComparer.Equals("C:\\path", "C:\\Path"), Is.True);
      else
        Assert.That(Platform.PathStringComparer.Equals("/path", "/Path"), Is.False);
    }

    [Test]
    public void TestDistributionName()
    {
      // returns non-null value always
      Assert.That(Platform.DistributionName, Is.Not.Null);
      Assert.That(Platform.DistributionName, Is.Not.Null);
      Assert.That(Platform.DistributionName, Is.Not.Null);

      var dist = Platform.DistributionName.ToLowerInvariant();

      if (Platform.IsRunningOnUnix)
        Assert.That(dist, Does.Not.Contain("windows"));
      else
        Assert.That(dist, Does.Contain("windows"));
    }

    [Test]
    public void TestKernelName()
    {
      // returns non-null value always
      Assert.That(Platform.KernelName, Is.Not.Null);
      Assert.That(Platform.KernelName, Is.Not.Null);
      Assert.That(Platform.KernelName, Is.Not.Null);
    }

    [Test]
    public void TestProcessorName()
    {
      // returns non-null value always
      Assert.That(Platform.ProcessorName, Is.Not.Null);
      Assert.That(Platform.ProcessorName, Is.Not.Null);
      Assert.That(Platform.ProcessorName, Is.Not.Null);
    }
  }
}
