using System;
using NUnit.Framework;

namespace Smdn.OperatingSystem {
  [TestFixture()]
  public class ShellTests {
#if NET46
    [Test]
    public void TestCreateProcessStartInfo()
    {
      Assert.Throws<ArgumentNullException>(() => Shell.CreateProcessStartInfo(null, (string)null), "#1");
      Assert.Throws<ArgumentNullException>(() => Shell.CreateProcessStartInfo(null, (string[])null), "#2");

      Assert.DoesNotThrow(() => Shell.CreateProcessStartInfo("foo", (string)null), "#3");
      Assert.DoesNotThrow(() => Shell.CreateProcessStartInfo("foo", (string[])null), "#4");
    }
#endif
  }
}