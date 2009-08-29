using System;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class PathUtilsTests {
    [Test]
    public void TestArePathEqual()
    {
      if (Runtime.IsRunningOnUnix) {
        Assert.IsTrue(PathUtils.ArePathEqual("/var/log/", "/var/log/"));
        Assert.IsTrue(PathUtils.ArePathEqual("/var/log/", "/var/log"));
        Assert.IsFalse(PathUtils.ArePathEqual("/var/log/", "/var/Log/"));
        Assert.IsFalse(PathUtils.ArePathEqual("/var/log/", "/var/Log"));
      }
      else {
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\Windows\"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\Windows"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\windows\"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\windows"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/Windows/"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/Windows"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/windows/"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/windows"));
      }
    }

    [Test]
    public void TestAreExtensionEqual()
    {
      if (Runtime.IsRunningOnUnix) {
        Assert.IsTrue(PathUtils.AreExtensionEqual("/etc/conf.ini", ".ini"));
        Assert.IsFalse(PathUtils.AreExtensionEqual("/etc/CONF.INI", ".ini"));
        Assert.IsFalse(PathUtils.AreExtensionEqual("/etc/Conf.Ini", ".ini"));

        Assert.IsFalse(PathUtils.AreExtensionEqual("/etc/conf.ini", ".txt"));

        Assert.IsTrue(PathUtils.AreExtensionEqual(@"test.jpeg", "test.jpeg"));
        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "TEST.JPEG"));
        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "Test.Jpeg"));

        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "test.png"));
      }
      else {
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".ini"));
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"C:\WINDOWS\BOOT.INI", ".ini"));
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"C:\WINDOWS\Boot.Ini", ".ini"));

        Assert.IsFalse(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".txt"));

        Assert.IsTrue(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".ini"));
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".INI"));
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".Ini"));

        Assert.IsTrue(PathUtils.AreExtensionEqual(@"test.jpeg", "test.jpeg"));
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"test.jpeg", "TEST.JPEG"));
        Assert.IsTrue(PathUtils.AreExtensionEqual(@"test.jpeg", "Test.Jpeg"));

        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "test.png"));
      }
    }
  }
}