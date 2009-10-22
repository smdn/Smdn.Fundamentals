using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class PlatformTests {
    [Test]
    public void TestDistributionName()
    {
      // returns non-null value always
      Assert.IsNotNull(Platform.DistributionName);
      Assert.IsNotNull(Platform.DistributionName);
      Assert.IsNotNull(Platform.DistributionName);

      var dist = Platform.DistributionName.ToLower();

      if (Runtime.IsRunningOnUnix)
        Assert.IsFalse(dist.Contains("windows"));
      else
        Assert.IsTrue(dist.Contains("windows"));
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
