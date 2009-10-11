using System;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class PathUtilsTests {
    [Test]
    public void TestChangeFileName()
    {
      if (Runtime.IsRunningOnUnix) {
        Assert.AreEqual("/var/log/renamed.log", PathUtils.ChangeFileName("/var/log/test.log", "renamed"));
        Assert.AreEqual("../renamed.log", PathUtils.ChangeFileName("../test.log", "renamed"));
        Assert.AreEqual("/var/log/renamed", PathUtils.ChangeFileName("/var/log/test", "renamed"));
      }
      else {
        Assert.AreEqual(@"C:\WINDOWS\renamed.ini", PathUtils.ChangeFileName(@"C:\WINDOWS\boot.ini", "renamed"));
        Assert.AreEqual(@"..\renamed.ini", PathUtils.ChangeFileName(@"..\boot.ini", "renamed"));
        Assert.AreEqual(@"C:\WINDOWS\renamed", PathUtils.ChangeFileName(@"C:\WINDOWS\boot", "renamed"));
      }

      Assert.AreEqual(@"renamed.ini", PathUtils.ChangeFileName(@"boot.ini", "renamed"));
      Assert.AreEqual("renamed", PathUtils.ChangeFileName("test", "renamed"));
    }

    [Test]
    public void TestChangeDirectoryName()
    {
      if (Runtime.IsRunningOnUnix) {
        Assert.AreEqual("/renamed/test.log", PathUtils.ChangeDirectoryName("/var/log/test.log", "/renamed"));
        Assert.AreEqual("../renamed/test.log", PathUtils.ChangeDirectoryName("/var/log/test.log", "../renamed"));
        Assert.AreEqual("test.log", PathUtils.ChangeDirectoryName("/var/log/test.log", string.Empty));
        Assert.AreEqual("./test.log", PathUtils.ChangeDirectoryName("test.log", "./"));
      }
      else {
        Assert.AreEqual(@"C:\renamed\boot.ini", PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", @"C:\renamed"));
        Assert.AreEqual(@"..\renamed\boot.ini", PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", @"..\renamed"));
        Assert.AreEqual("boot.ini", PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", string.Empty));
        Assert.AreEqual(@".\boot.ini", PathUtils.ChangeDirectoryName(@"boot.ini", @".\"));
      }
    }

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