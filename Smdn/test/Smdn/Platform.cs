using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Smdn {
  [TestFixture]
  public class PlatformTests {
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

#if NET || NETSTANDARD2_0
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
#endif
  }
}
