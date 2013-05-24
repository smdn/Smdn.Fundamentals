using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class ShellTests {
    [Test]
    public void TestCreateProcessStartInfo()
    {
      Assert.Throws<ArgumentNullException>(() => Shell.CreateProcessStartInfo(null, (string)null), "#1");
      Assert.Throws<ArgumentNullException>(() => Shell.CreateProcessStartInfo(null, (string[])null), "#2");

      Assert.DoesNotThrow(() => Shell.CreateProcessStartInfo("foo", (string)null), "#3");
      Assert.DoesNotThrow(() => Shell.CreateProcessStartInfo("foo", (string[])null), "#4");
    }
  }
}