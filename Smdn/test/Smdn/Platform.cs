using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Smdn {
  [TestFixture]
  public class PlatformTests {
    [Test]
    public void Test()
    {
      TestContext.Out.WriteLine("DistributionName: {0}", Platform.DistributionName);
      TestContext.Out.WriteLine("KernelName: {0}", Platform.KernelName);
      TestContext.Out.WriteLine("ProcessorName: {0}", Platform.ProcessorName);

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
    public void TestDistributionName()
    {
      // returns non-null value always
      Assert.IsNotNull(Platform.DistributionName);
      Assert.IsNotNull(Platform.DistributionName);
      Assert.IsNotNull(Platform.DistributionName);

      var dist = Platform.DistributionName.ToLower();

      if (Runtime.IsRunningOnUnix)
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
