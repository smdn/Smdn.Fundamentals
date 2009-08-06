using System;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class PathUtilsTests {
    [Test]
    public void TestEquals()
    {
      if (Runtime.IsRunningOnUnix) {
        Assert.IsTrue(PathUtils.Equals("/var/log/", "/var/log/"));
        Assert.IsTrue(PathUtils.Equals("/var/log/", "/var/log"));
        Assert.IsFalse(PathUtils.Equals("/var/log/", "/var/Log/"));
        Assert.IsFalse(PathUtils.Equals("/var/log/", "/var/Log"));
      }
      else {
        Assert.IsTrue(PathUtils.Equals(@"C:\Windows\", @"C:\Windows\"));
        Assert.IsTrue(PathUtils.Equals(@"C:\Windows\", @"C:\Windows"));
        Assert.IsTrue(PathUtils.Equals(@"C:\Windows\", @"C:\windows\"));
        Assert.IsTrue(PathUtils.Equals(@"C:\Windows\", @"C:\windows"));
        Assert.IsTrue(PathUtils.Equals(@"C:/Windows/", @"C:/Windows/"));
        Assert.IsTrue(PathUtils.Equals(@"C:/Windows/", @"C:/Windows"));
        Assert.IsTrue(PathUtils.Equals(@"C:/Windows/", @"C:/windows/"));
        Assert.IsTrue(PathUtils.Equals(@"C:/Windows/", @"C:/windows"));
      }
    }
  }
}