// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn.IO {
  [TestFixture]
  public class PathUtilsTests {
    public static bool IsRunningOnWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [Test]
    public void TestDefaultPathStringComparison()
    {
      if (IsRunningOnWindows)
        Assert.IsTrue(string.Equals("C:\\path", "C:\\Path", PathUtils.DefaultPathStringComparison));
      else
        Assert.IsFalse(string.Equals("/path", "/Path", PathUtils.DefaultPathStringComparison));

#if SYSTEM_STRINGCOMPARER_FROMCOMPARISON
      StringComparer comparer = null;

      Assert.DoesNotThrow(() => {
        comparer = StringComparer.FromComparison(
          PathUtils.DefaultPathStringComparison
        );
      });

#if SYSTEM_STRINGCOMPARER_ISWELLKNOWNORDINALCOMPARER
      Assert.IsTrue(
        StringComparer.IsWellKnownOrdinalComparer(
          PathUtils.DefaultPathStringComparer,
          out var isIgnoreCase
        ),
        "IsWellKnownOrdinalComparer"
      );

      if (IsRunningOnWindows)
        Assert.IsTrue(isIgnoreCase, nameof(isIgnoreCase));
      else
        Assert.IsFalse(isIgnoreCase, nameof(isIgnoreCase));
#endif
#endif
    }

    [Test]
    public void TestDefaultPathStringComparer()
    {
      Assert.IsNotNull(PathUtils.DefaultPathStringComparer);

      if (IsRunningOnWindows)
        Assert.IsTrue(PathUtils.DefaultPathStringComparer.Equals("C:\\path", "C:\\Path"));
      else
        Assert.IsFalse(PathUtils.DefaultPathStringComparer.Equals("/path", "/Path"));

#if SYSTEM_STRINGCOMPARER_ISWELLKNOWNORDINALCOMPARER
      Assert.IsTrue(
        StringComparer.IsWellKnownOrdinalComparer(
          PathUtils.DefaultPathStringComparer,
          out var isIgnoreCase
        ),
        "IsWellKnownOrdinalComparer"
      );

      if (IsRunningOnWindows)
        Assert.IsTrue(isIgnoreCase, nameof(isIgnoreCase));
      else
        Assert.IsFalse(isIgnoreCase, nameof(isIgnoreCase));
#endif
    }

    [Test]
    public void TestChangeFileName()
    {
      if (IsRunningOnWindows) {
        Assert.AreEqual(@"C:\WINDOWS\renamed.ini", PathUtils.ChangeFileName(@"C:\WINDOWS\boot.ini", "renamed"));
        Assert.AreEqual(@"..\renamed.ini", PathUtils.ChangeFileName(@"..\boot.ini", "renamed"));
        Assert.AreEqual(@"C:\WINDOWS\renamed", PathUtils.ChangeFileName(@"C:\WINDOWS\boot", "renamed"));
      }
      else {
        Assert.AreEqual("/var/log/renamed.log", PathUtils.ChangeFileName("/var/log/test.log", "renamed"));
        Assert.AreEqual("../renamed.log", PathUtils.ChangeFileName("../test.log", "renamed"));
        Assert.AreEqual("/var/log/renamed", PathUtils.ChangeFileName("/var/log/test", "renamed"));
      }

      Assert.AreEqual(@"renamed.ini", PathUtils.ChangeFileName(@"boot.ini", "renamed"));
      Assert.AreEqual("renamed", PathUtils.ChangeFileName("test", "renamed"));
    }

    [Test]
    public void TestChangeDirectoryName()
    {
      if (IsRunningOnWindows) {
        Assert.AreEqual(@"C:\renamed\boot.ini", PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", @"C:\renamed"));
        Assert.AreEqual(@"..\renamed\boot.ini", PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", @"..\renamed"));
        Assert.AreEqual("boot.ini", PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", string.Empty));
        Assert.AreEqual(@".\boot.ini", PathUtils.ChangeDirectoryName(@"boot.ini", @".\"));
      }
      else {
        Assert.AreEqual("/renamed/test.log", PathUtils.ChangeDirectoryName("/var/log/test.log", "/renamed"));
        Assert.AreEqual("../renamed/test.log", PathUtils.ChangeDirectoryName("/var/log/test.log", "../renamed"));
        Assert.AreEqual("test.log", PathUtils.ChangeDirectoryName("/var/log/test.log", string.Empty));
        Assert.AreEqual("./test.log", PathUtils.ChangeDirectoryName("test.log", "./"));
      }
    }

    [Test]
    public void TestArePathEqual()
    {
      if (IsRunningOnWindows) {
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\Windows\"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\Windows"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\windows\"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\windows"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/Windows/"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/Windows"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/windows/"));
        Assert.IsTrue(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/windows"));
      }
      else {
        Assert.IsTrue(PathUtils.ArePathEqual("/var/log/", "/var/log/"));
        Assert.IsTrue(PathUtils.ArePathEqual("/var/log/", "/var/log"));
        Assert.IsFalse(PathUtils.ArePathEqual("/var/log/", "/var/Log/"));
        Assert.IsFalse(PathUtils.ArePathEqual("/var/log/", "/var/Log"));
      }
    }

    [Test]
    public void TestAreExtensionEqual()
    {
      if (IsRunningOnWindows) {
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
      else {
        Assert.IsTrue(PathUtils.AreExtensionEqual("/etc/conf.ini", ".ini"));
        Assert.IsFalse(PathUtils.AreExtensionEqual("/etc/CONF.INI", ".ini"));
        Assert.IsFalse(PathUtils.AreExtensionEqual("/etc/Conf.Ini", ".ini"));

        Assert.IsFalse(PathUtils.AreExtensionEqual("/etc/conf.ini", ".txt"));

        Assert.IsTrue(PathUtils.AreExtensionEqual(@"test.jpeg", "test.jpeg"));
        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "TEST.JPEG"));
        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "Test.Jpeg"));

        Assert.IsFalse(PathUtils.AreExtensionEqual(@"test.jpeg", "test.png"));
      }
    }

    [Test]
    public void TestContainsShellEscapeChar()
    {
      var shift_jis = Encodings.ShiftJis;
      var ngchars = "?????????????????????????????????????????????????????????????????????????????????????????????????????????????????"; // XXX: "????????????"

      foreach (var c in ngchars.ToCharArray()) {
        var s = c.ToString();

        Assert.IsTrue(PathUtils.ContainsShellEscapeChar(s, shift_jis),
                      string.Format("char: {0}, bytes: {1}", s, BitConverter.ToString(shift_jis.GetBytes(s))));
      }

      Assert.IsTrue(PathUtils.ContainsShellEscapeChar("?????????", shift_jis));
      Assert.IsTrue(PathUtils.ContainsShellEscapeChar("?????????????????????????????????", shift_jis));
    }

    [Test]
    public void TestContainsShellPipeChar()
    {
      var shift_jis = Encodings.ShiftJis;
      var ngchars = "?????????????????????????????????????????????????????????????????????????????????????????????????????"; // XXX: ????????????

      foreach (var c in ngchars.ToCharArray()) {
        var s = c.ToString();

        Assert.IsTrue(PathUtils.ContainsShellPipeChar(s, shift_jis),
                      string.Format("char: {0}, bytes: {1}", s, BitConverter.ToString(shift_jis.GetBytes(s))));
      }

      Assert.IsTrue(PathUtils.ContainsShellPipeChar("????????????", shift_jis));
      Assert.IsTrue(PathUtils.ContainsShellPipeChar("????????????", shift_jis));
    }

    [Test]
    public void TestContainsShellSpecialChars()
    {
      var shift_jis = Encodings.ShiftJis;
      var ngchars = new byte[] {0x5c, 0x7c};

      Assert.IsTrue(PathUtils.ContainsShellSpecialChars("?????????", shift_jis, ngchars));
      Assert.IsTrue(PathUtils.ContainsShellSpecialChars("?????????????????????????????????", shift_jis, ngchars));
      Assert.IsTrue(PathUtils.ContainsShellSpecialChars("????????????", shift_jis, ngchars));
      Assert.IsTrue(PathUtils.ContainsShellSpecialChars("????????????", shift_jis, ngchars));
    }

#if !SYSTEM_IO_PATH_GETRELATIVEPATH
    [Test]
    public void TestGetRelativePath()
    {
      if (IsRunningOnWindows)
        GetRelativePathWin();
      else
        GetRelativePathUnix();
    }

    private void GetRelativePathWin()
    {
      Assert.AreEqual(@"child",
                      PathUtils.GetRelativePath(@"C:\", @"C:\child"),
                      "child #1");
      Assert.AreEqual(@"child\",
                      PathUtils.GetRelativePath(@"C:\", @"C:\child\"),
                      "child #2");
      Assert.AreEqual(@"child\file",
                      PathUtils.GetRelativePath(@"C:\", @"C:\child\file"),
                      "child #3");

      Assert.AreEqual(@"..\",
                      PathUtils.GetRelativePath(@"C:\child\", @"C:\"),
                      "parent #1");
      Assert.AreEqual(@"..\file",
                      PathUtils.GetRelativePath(@"C:\child\", @"C:\file"),
                      "parent #2");
      Assert.AreEqual(@"..\dir\",
                      PathUtils.GetRelativePath(@"C:\child\", @"C:\dir\"),
                      "parent #3");

      Assert.AreEqual(@"file2",
                      PathUtils.GetRelativePath(@"C:\file1", @"C:\file2"),
                      "sibling #1");
      Assert.AreEqual(@"dir\",
                      PathUtils.GetRelativePath(@"C:\file1", @"C:\dir\"),
                      "sibling #2");
      Assert.AreEqual(@"dir\file2",
                      PathUtils.GetRelativePath(@"C:\file1", @"C:\dir\file2"),
                      "sibling #3");

      Assert.AreEqual(@"..\file",
                      PathUtils.GetRelativePath(@"C:\dir1\", @"C:\file"),
                      "cousin #1");
      Assert.AreEqual(@"..\dir2\",
                      PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir2\"),
                      "cousin #2");
      Assert.AreEqual(@"..\dir2\file",
                      PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir2\file"),
                      "cousin #3");

      Assert.AreEqual(@"..\..\dir2\",
                      PathUtils.GetRelativePath(@"C:\dir1\subdir\", @"C:\dir2\"),
                      "#1");
      Assert.AreEqual(@"..\..\..\dir2\",
                      PathUtils.GetRelativePath(@"C:\dir1\subdir1\subdir2\", @"C:\dir2\"),
                      "#2");
      Assert.AreEqual(@"..\dir2\",
                      PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir2\"),
                      "#3");
      Assert.AreEqual(@"subdir\",
                      PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir1\subdir\"),
                      "#4");

      Assert.AreEqual(@"..\%sibling\",
                      PathUtils.GetRelativePath(@"C:\dir\", @"C:\%sibling\"),
                      "contains '%' #1");
      Assert.AreEqual(@"..\%73ibling\",
                      PathUtils.GetRelativePath(@"C:\dir\", @"C:\%73ibling\"),
                      "contains '%' #2");

      Assert.AreEqual(@"..\foo:bar\",
                      PathUtils.GetRelativePath(@"C:\dir\", @"C:\foo:bar\"),
                      "contains ':' #1");
      Assert.AreEqual(@".\foo:bar", // XXX
                      PathUtils.GetRelativePath(@"C:\dir\", @"C:\dir\foo:bar"),
                      "contains ':' #2");
      Assert.AreEqual(@"foo",
                      PathUtils.GetRelativePath(@"C:\dir\foo:bar", @"C:\dir\foo"),
                      "contains ':' #3");

      Assert.AreEqual(@"..\??????\",
                      PathUtils.GetRelativePath(@"C:\dir\", @"C:\??????\"),
                      "contains non ascii #1");

      Assert.AreEqual(@"D:\",
                      PathUtils.GetRelativePath(@"C:\", @"D:\"),
                      "rooted path #1");
      Assert.AreEqual(@"D:\dir\",
                      PathUtils.GetRelativePath(@"C:\", @"D:\dir\"),
                      "rooted path #2");

      Assert.AreEqual(@"C:\child",
                      Path.GetFullPath(Path.Combine(@"C:\", PathUtils.GetRelativePath(@"C:\", @"C:\child"))),
                      "re-combine #1");
      Assert.AreEqual(@"C:\dir\",
                      Path.GetFullPath(Path.Combine(@"C:\child\", PathUtils.GetRelativePath(@"C:\child\", @"C:\dir\"))),
                      "re-combine #2");
      Assert.AreEqual(@"C:\dir2\",
                      Path.GetFullPath(Path.Combine(@"C:\dir1\subdir1\subdir2\", PathUtils.GetRelativePath(@"C:\dir1\subdir1\subdir2\", @"C:\dir2\"))),
                      "re-combine #3");
    }

    private void GetRelativePathUnix()
    {
      Assert.AreEqual("root",
                      PathUtils.GetRelativePath("/", "/root"),
                      "child #1");
      Assert.AreEqual("root/",
                      PathUtils.GetRelativePath("/", "/root/"),
                      "child #2");
      Assert.AreEqual("root/file",
                      PathUtils.GetRelativePath("/", "/root/file"),
                      "child #3");

      Assert.AreEqual("../",
                      PathUtils.GetRelativePath("/root/", "/"),
                      "parent #1");
      Assert.AreEqual("../usr",
                      PathUtils.GetRelativePath("/root/", "/usr"),
                      "parent #2");
      Assert.AreEqual("../usr/",
                      PathUtils.GetRelativePath("/root/", "/usr/"),
                      "parent #3");

      Assert.AreEqual("usr",
                      PathUtils.GetRelativePath("/file", "/usr"),
                      "sibling #1");
      Assert.AreEqual("usr/",
                      PathUtils.GetRelativePath("/file", "/usr/"),
                      "sibling #2");
      Assert.AreEqual("usr/file",
                      PathUtils.GetRelativePath("/file", "/usr/file"),
                      "sibling #3");

      Assert.AreEqual("../usr",
                      PathUtils.GetRelativePath("/root/", "/usr"),
                      "cousin #1");
      Assert.AreEqual("../usr/",
                      PathUtils.GetRelativePath("/root/", "/usr/"),
                      "cousin #2");
      Assert.AreEqual("../usr/file",
                      PathUtils.GetRelativePath("/root/", "/usr/file"),
                      "cousin #3");

      Assert.AreEqual("../../usr/",
                      PathUtils.GetRelativePath("/root/dir/", "/usr/"),
                      "#1");
      Assert.AreEqual("../../../usr/",
                      PathUtils.GetRelativePath("/root/dir/subdir/", "/usr/"),
                      "#2");
      Assert.AreEqual("../dir2/",
                      PathUtils.GetRelativePath("/root/dir1/", "/root/dir2/"),
                      "#3");
      Assert.AreEqual("subdir/",
                      PathUtils.GetRelativePath("/root/dir1/", "/root/dir1/subdir/"),
                      "#4");

      Assert.AreEqual("../%sibling/",
                      PathUtils.GetRelativePath("/root/dir/", "/root/%sibling/"),
                      "contains '%' #1");
      Assert.AreEqual("../%73ibling/",
                      PathUtils.GetRelativePath("/root/dir/", "/root/%73ibling/"),
                      "contains '%' #2");

      Assert.AreEqual(@"../foo:bar/",
                      PathUtils.GetRelativePath(@"/root/dir/", @"/root/foo:bar/"),
                      "contains ':' #1");
      Assert.AreEqual(@"foo:bar",
                      PathUtils.GetRelativePath(@"/root/", @"/root/foo:bar"),
                      "contains ':' #2");
      Assert.AreEqual(@"foo",
                      PathUtils.GetRelativePath(@"/root/foo:bar", @"/root/foo"),
                      "contains ':' #3");

      Assert.AreEqual("../??????/",
                      PathUtils.GetRelativePath("/root/dir/", "/root/??????/"),
                      "contains non ascii #1");
    }
#endif
  }
}
