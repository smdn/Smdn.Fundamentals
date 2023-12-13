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
        Assert.That(string.Equals("C:\\path", "C:\\Path", PathUtils.DefaultPathStringComparison), Is.True);
      else
        Assert.That(string.Equals("/path", "/Path", PathUtils.DefaultPathStringComparison), Is.False);

#if SYSTEM_STRINGCOMPARER_FROMCOMPARISON
      StringComparer comparer = null;

      Assert.DoesNotThrow(() => {
        comparer = StringComparer.FromComparison(
          PathUtils.DefaultPathStringComparison
        );
      });

#if SYSTEM_STRINGCOMPARER_ISWELLKNOWNORDINALCOMPARER
      Assert.That(
        StringComparer.IsWellKnownOrdinalComparer(
          PathUtils.DefaultPathStringComparer,
          out var isIgnoreCase
        ), Is.True,
        "IsWellKnownOrdinalComparer"
      );

      if (IsRunningOnWindows)
        Assert.That(isIgnoreCase, Is.True, nameof(isIgnoreCase));
      else
        Assert.That(isIgnoreCase, Is.False, nameof(isIgnoreCase));
#endif
#endif
    }

    [Test]
    public void TestDefaultPathStringComparer()
    {
      Assert.That(PathUtils.DefaultPathStringComparer, Is.Not.Null);

      if (IsRunningOnWindows)
        Assert.That(PathUtils.DefaultPathStringComparer.Equals("C:\\path", "C:\\Path"), Is.True);
      else
        Assert.That(PathUtils.DefaultPathStringComparer.Equals("/path", "/Path"), Is.False);

#if SYSTEM_STRINGCOMPARER_ISWELLKNOWNORDINALCOMPARER
      Assert.That(
        StringComparer.IsWellKnownOrdinalComparer(
          PathUtils.DefaultPathStringComparer,
          out var isIgnoreCase
        ), Is.True,
        "IsWellKnownOrdinalComparer"
      );

      if (IsRunningOnWindows)
        Assert.That(isIgnoreCase, Is.True, nameof(isIgnoreCase));
      else
        Assert.That(isIgnoreCase, Is.False, nameof(isIgnoreCase));
#endif
    }

    [Test]
    public void TestChangeFileName()
    {
      if (IsRunningOnWindows) {
        Assert.That(PathUtils.ChangeFileName(@"C:\WINDOWS\boot.ini", "renamed"), Is.EqualTo(@"C:\WINDOWS\renamed.ini"));
        Assert.That(PathUtils.ChangeFileName(@"..\boot.ini", "renamed"), Is.EqualTo(@"..\renamed.ini"));
        Assert.That(PathUtils.ChangeFileName(@"C:\WINDOWS\boot", "renamed"), Is.EqualTo(@"C:\WINDOWS\renamed"));
      }
      else {
        Assert.That(PathUtils.ChangeFileName("/var/log/test.log", "renamed"), Is.EqualTo("/var/log/renamed.log"));
        Assert.That(PathUtils.ChangeFileName("../test.log", "renamed"), Is.EqualTo("../renamed.log"));
        Assert.That(PathUtils.ChangeFileName("/var/log/test", "renamed"), Is.EqualTo("/var/log/renamed"));
      }

      Assert.That(PathUtils.ChangeFileName(@"boot.ini", "renamed"), Is.EqualTo(@"renamed.ini"));
      Assert.That(PathUtils.ChangeFileName("test", "renamed"), Is.EqualTo("renamed"));
    }

    [Test]
    public void TestChangeDirectoryName()
    {
      if (IsRunningOnWindows) {
        Assert.That(PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", @"C:\renamed"), Is.EqualTo(@"C:\renamed\boot.ini"));
        Assert.That(PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", @"..\renamed"), Is.EqualTo(@"..\renamed\boot.ini"));
        Assert.That(PathUtils.ChangeDirectoryName(@"C:\WINDOWS\boot.ini", string.Empty), Is.EqualTo("boot.ini"));
        Assert.That(PathUtils.ChangeDirectoryName(@"boot.ini", @".\"), Is.EqualTo(@".\boot.ini"));
      }
      else {
        Assert.That(PathUtils.ChangeDirectoryName("/var/log/test.log", "/renamed"), Is.EqualTo("/renamed/test.log"));
        Assert.That(PathUtils.ChangeDirectoryName("/var/log/test.log", "../renamed"), Is.EqualTo("../renamed/test.log"));
        Assert.That(PathUtils.ChangeDirectoryName("/var/log/test.log", string.Empty), Is.EqualTo("test.log"));
        Assert.That(PathUtils.ChangeDirectoryName("test.log", "./"), Is.EqualTo("./test.log"));
      }
    }

    [Test]
    public void TestArePathEqual()
    {
      if (IsRunningOnWindows) {
        Assert.That(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\Windows\"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\Windows"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\windows\"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:\Windows\", @"C:\windows"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/Windows/"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/Windows"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/windows/"), Is.True);
        Assert.That(PathUtils.ArePathEqual(@"C:/Windows/", @"C:/windows"), Is.True);
      }
      else {
        Assert.That(PathUtils.ArePathEqual("/var/log/", "/var/log/"), Is.True);
        Assert.That(PathUtils.ArePathEqual("/var/log/", "/var/log"), Is.True);
        Assert.That(PathUtils.ArePathEqual("/var/log/", "/var/Log/"), Is.False);
        Assert.That(PathUtils.ArePathEqual("/var/log/", "/var/Log"), Is.False);
      }
    }

    [Test]
    public void TestAreExtensionEqual()
    {
      if (IsRunningOnWindows) {
        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".ini"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\BOOT.INI", ".ini"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\Boot.Ini", ".ini"), Is.True);

        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".txt"), Is.False);

        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".ini"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".INI"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"C:\WINDOWS\boot.ini", ".Ini"), Is.True);

        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "test.jpeg"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "TEST.JPEG"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "Test.Jpeg"), Is.True);

        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "test.png"), Is.False);
      }
      else {
        Assert.That(PathUtils.AreExtensionEqual("/etc/conf.ini", ".ini"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual("/etc/CONF.INI", ".ini"), Is.False);
        Assert.That(PathUtils.AreExtensionEqual("/etc/Conf.Ini", ".ini"), Is.False);

        Assert.That(PathUtils.AreExtensionEqual("/etc/conf.ini", ".txt"), Is.False);

        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "test.jpeg"), Is.True);
        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "TEST.JPEG"), Is.False);
        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "Test.Jpeg"), Is.False);

        Assert.That(PathUtils.AreExtensionEqual(@"test.jpeg", "test.png"), Is.False);
      }
    }

    [Test]
    public void TestContainsShellEscapeChar()
    {
      var shift_jis = Encodings.ShiftJis;
      var ngchars = "―ソЫⅨ噂浬欺圭構蚕十申曾箪貼能表暴予禄兔喀媾彌拿杤歃濬畚秉綵臀藹觸軆鐔饅鷭"; // XXX: "偆砡纊犾"

      foreach (var c in ngchars.ToCharArray()) {
        var s = c.ToString();

        Assert.That(
          PathUtils.ContainsShellEscapeChar(s, shift_jis),
          Is.True,
          $"char: {s}, bytes: {BitConverter.ToString(shift_jis.GetBytes(s))}"
        );
      }

      Assert.That(PathUtils.ContainsShellEscapeChar("六十年", shift_jis), Is.True);
      Assert.That(PathUtils.ContainsShellEscapeChar("明治十七年の上海アリス", shift_jis), Is.True);
    }

    [Test]
    public void TestContainsShellPipeChar()
    {
      var shift_jis = Encodings.ShiftJis;
      var ngchars = "ポл榎掛弓芸鋼旨楯酢竹倒培怖翻慾處嘶斈忿掟桍毫烟痞窩縹艚蛞諫轎閖驂黥"; // XXX: 埈蒴僴礰

      foreach (var c in ngchars.ToCharArray()) {
        var s = c.ToString();

        Assert.That(
          PathUtils.ContainsShellPipeChar(s, shift_jis),
          Is.True,
          $"char: {s}, bytes: {BitConverter.ToString(shift_jis.GetBytes(s))}"
        );
      }

      Assert.That(PathUtils.ContainsShellPipeChar("竹取物語", shift_jis), Is.True);
      Assert.That(PathUtils.ContainsShellPipeChar("ポケモン", shift_jis), Is.True);
    }

    [Test]
    public void TestContainsShellSpecialChars()
    {
      var shift_jis = Encodings.ShiftJis;
      var ngchars = new byte[] {0x5c, 0x7c};

      Assert.That(PathUtils.ContainsShellSpecialChars("六十年", shift_jis, ngchars), Is.True);
      Assert.That(PathUtils.ContainsShellSpecialChars("明治十七年の上海アリス", shift_jis, ngchars), Is.True);
      Assert.That(PathUtils.ContainsShellSpecialChars("竹取物語", shift_jis, ngchars), Is.True);
      Assert.That(PathUtils.ContainsShellSpecialChars("ポケモン", shift_jis, ngchars), Is.True);
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
      Assert.That(
        PathUtils.GetRelativePath(@"C:\", @"C:\child"),
        Is.EqualTo(@"child"),
        "child #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\", @"C:\child\"),
        Is.EqualTo(@"child\"),
        "child #2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\", @"C:\child\file"),
        Is.EqualTo(@"child\file"),
        "child #3"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\child\", @"C:\"),
        Is.EqualTo(@"..\"),
        "parent #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\child\", @"C:\file"),
        Is.EqualTo(@"..\file"),
        "parent #2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\child\", @"C:\dir\"),
        Is.EqualTo(@"..\dir\"),
        "parent #3"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\file1", @"C:\file2"),
        Is.EqualTo(@"file2"),
        "sibling #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\file1", @"C:\dir\"),
        Is.EqualTo(@"dir\"),
        "sibling #2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\file1", @"C:\dir\file2"),
        Is.EqualTo(@"dir\file2"),
        "sibling #3"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\", @"C:\file"),
        Is.EqualTo(@"..\file"),
        "cousin #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir2\"),
        Is.EqualTo(@"..\dir2\"),
        "cousin #2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir2\file"),
        Is.EqualTo(@"..\dir2\file"),
        "cousin #3"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\subdir\", @"C:\dir2\"),
        Is.EqualTo(@"..\..\dir2\"),
        "#1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\subdir1\subdir2\", @"C:\dir2\"),
        Is.EqualTo(@"..\..\..\dir2\"),
        "#2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir2\"),
        Is.EqualTo(@"..\dir2\"),
        "#3"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir1\", @"C:\dir1\subdir\"),
        Is.EqualTo(@"subdir\"),
        "#4"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir\", @"C:\%sibling\"),
        Is.EqualTo(@"..\%sibling\"),
        "contains '%' #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir\", @"C:\%73ibling\"),
        Is.EqualTo(@"..\%73ibling\"),
        "contains '%' #2"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir\", @"C:\foo:bar\"),
        Is.EqualTo(@"..\foo:bar\"),
        "contains ':' #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir\", @"C:\dir\foo:bar"),
        Is.EqualTo(@".\foo:bar"), // XXX
        "contains ':' #2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir\foo:bar", @"C:\dir\foo"),
        Is.EqualTo(@"foo"),
        "contains ':' #3"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\dir\", @"C:\兄弟\"),
        Is.EqualTo(@"..\兄弟\"),
        "contains non ascii #1"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"C:\", @"D:\"),
        Is.EqualTo(@"D:\"),
        "rooted path #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"C:\", @"D:\dir\"),
        Is.EqualTo(@"D:\dir\"),
        "rooted path #2"
      );

      Assert.That(
        Path.GetFullPath(Path.Combine(@"C:\", PathUtils.GetRelativePath(@"C:\", @"C:\child"))),
        Is.EqualTo(@"C:\child"),
        "re-combine #1"
      );
      Assert.That(
        Path.GetFullPath(Path.Combine(@"C:\child\", PathUtils.GetRelativePath(@"C:\child\", @"C:\dir\"))),
        Is.EqualTo(@"C:\dir\"),
        "re-combine #2"
      );
      Assert.That(
        Path.GetFullPath(Path.Combine(@"C:\dir1\subdir1\subdir2\", PathUtils.GetRelativePath(@"C:\dir1\subdir1\subdir2\", @"C:\dir2\"))),
        Is.EqualTo(@"C:\dir2\"),
        "re-combine #3"
      );
    }

    private void GetRelativePathUnix()
    {
      Assert.That(
        PathUtils.GetRelativePath("/", "/root"),
        Is.EqualTo("root"),
        "child #1"
      );
      Assert.That(
        PathUtils.GetRelativePath("/", "/root/"),
        Is.EqualTo("root/"),
        "child #2"
      );
      Assert.That(
        PathUtils.GetRelativePath("/", "/root/file"),
        Is.EqualTo("root/file"),
        "child #3"
      );

      Assert.That(
        PathUtils.GetRelativePath("/root/", "/"),
        Is.EqualTo("../"),
        "parent #1"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/", "/usr"),
        Is.EqualTo("../usr"),
        "parent #2"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/", "/usr/"),
        Is.EqualTo("../usr/"),
        "parent #3"
      );

      Assert.That(
        PathUtils.GetRelativePath("/file", "/usr"),
        Is.EqualTo("usr"),
        "sibling #1"
      );
      Assert.That(
        PathUtils.GetRelativePath("/file", "/usr/"),
        Is.EqualTo("usr/"),
        "sibling #2"
      );
      Assert.That(
        PathUtils.GetRelativePath("/file", "/usr/file"),
        Is.EqualTo("usr/file"),
        "sibling #3"
      );

      Assert.That(
        PathUtils.GetRelativePath("/root/", "/usr"),
        Is.EqualTo("../usr"),
        "cousin #1"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/", "/usr/"),
        Is.EqualTo("../usr/"),
        "cousin #2"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/", "/usr/file"),
        Is.EqualTo("../usr/file"),
        "cousin #3"
      );

      Assert.That(
        PathUtils.GetRelativePath("/root/dir/", "/usr/"),
        Is.EqualTo("../../usr/"),
        "#1"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/dir/subdir/", "/usr/"),
        Is.EqualTo("../../../usr/"),
        "#2"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/dir1/", "/root/dir2/"),
        Is.EqualTo("../dir2/"),
        "#3"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/dir1/", "/root/dir1/subdir/"),
        Is.EqualTo("subdir/"),
        "#4"
      );

      Assert.That(
        PathUtils.GetRelativePath("/root/dir/", "/root/%sibling/"),
        Is.EqualTo("../%sibling/"),
        "contains '%' #1"
      );
      Assert.That(
        PathUtils.GetRelativePath("/root/dir/", "/root/%73ibling/"),
        Is.EqualTo("../%73ibling/"),
        "contains '%' #2"
      );

      Assert.That(
        PathUtils.GetRelativePath(@"/root/dir/", @"/root/foo:bar/"),
        Is.EqualTo(@"../foo:bar/"),
        "contains ':' #1"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"/root/", @"/root/foo:bar"),
        Is.EqualTo(@"foo:bar"),
        "contains ':' #2"
      );
      Assert.That(
        PathUtils.GetRelativePath(@"/root/foo:bar", @"/root/foo"),
        Is.EqualTo(@"foo"),
        "contains ':' #3"
      );

      Assert.That(
        PathUtils.GetRelativePath("/root/dir/", "/root/兄弟/"),
        Is.EqualTo("../兄弟/"),
        "contains non ascii #1"
      );
    }
#endif
  }
}
