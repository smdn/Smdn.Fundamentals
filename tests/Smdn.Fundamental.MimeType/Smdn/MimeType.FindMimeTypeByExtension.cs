// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn;

partial class MimeTypeTests {
  public static bool IsRunningOnWindows =>
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  public static bool IsRunningOnUnix =>
    RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
    RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

  [Test]
  public void FindMimeTypeByExtension()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && !File.Exists("/etc/mime.types")) {
      Assert.Ignore("/etc/mime.types not found");
      return;
    }

    Assert.AreEqual(MimeType.TextPlain, MimeType.FindMimeTypeByExtension("hoge.txt"));
    Assert.AreEqual(MimeType.TextPlain, MimeType.FindMimeTypeByExtension("hoge.TXT"));
    Assert.AreEqual(MimeType.CreateTextType("html"), MimeType.FindMimeTypeByExtension("index.html"));
    Assert.AreEqual(MimeType.CreateImageType("png"), MimeType.FindMimeTypeByExtension("image.png"));
    Assert.AreEqual(null, MimeType.FindMimeTypeByExtension(".hoge"));
    Assert.AreEqual(null, MimeType.FindMimeTypeByExtension("hoge"));
    Assert.AreEqual(null, MimeType.FindMimeTypeByExtension(string.Empty));
    Assert.AreEqual(null, MimeType.FindMimeTypeByExtension("."));

    Assert.Throws<ArgumentNullException>(() => MimeType.FindMimeTypeByExtension(null!));

    if (IsRunningOnUnix)
      Assert.Throws<ArgumentNullException>(() => MimeType.FindMimeTypeByExtension("hoge.txt", mimeTypesFile: null!));

    if (IsRunningOnWindows)
      Assert.DoesNotThrow(() => MimeType.FindMimeTypeByExtension("hoge.txt", mimeTypesFile: null!));
  }

  [Test]
  public void FindMimeTypeByExtension_WithMimeTypesFile()
  {
    if (!IsRunningOnUnix)
      return;

    var pseudoMimeTypesFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, ".mime.types");

    IOUtils.UsingFile(pseudoMimeTypesFile, f => {
      File.WriteAllText(f.FullName, "application/x-foo-bar\tfoo-bar");

      Assert.AreEqual(
        new MimeType("application/x-foo-bar"),
        MimeType.FindMimeTypeByExtension(".foo-bar", mimeTypesFile: f.FullName)
      );

      Assert.IsNull(
        MimeType.FindMimeTypeByExtension(".hoge", mimeTypesFile: f.FullName)
      );
    });
  }

  [Test]
  public void FindMimeTypeByExtension_WithMimeTypesFile_FileNotFound()
  {
    const string nonExistentFile = ".nonexistent.mime.types";

    Assert.DoesNotThrow(() => {
      Assert.AreEqual(null, MimeType.FindMimeTypeByExtension(string.Empty, mimeTypesFile: nonExistentFile));
    });

    if (IsRunningOnUnix)
      Assert.Throws<FileNotFoundException>(() => MimeType.FindMimeTypeByExtension("hoge.txt", mimeTypesFile: nonExistentFile));

    if (IsRunningOnWindows)
      Assert.DoesNotThrow(() => MimeType.FindMimeTypeByExtension("hoge.txt", mimeTypesFile: nonExistentFile));
  }

  [Test]
  public void FindExtensionsByMimeType()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && !File.Exists("/etc/mime.types")) {
      Assert.Ignore("/etc/mime.types not found");
      return;
    }

    CollectionAssert.Contains(MimeType.FindExtensionsByMimeType("text/plain"),
                              ".txt");
    CollectionAssert.Contains(MimeType.FindExtensionsByMimeType("TEXT/PLAIN"),
                              ".txt");
    CollectionAssert.Contains(MimeType.FindExtensionsByMimeType(MimeType.TextPlain),
                              ".txt");

    CollectionAssert.Contains(MimeType.FindExtensionsByMimeType("text/html"),
                              ".html");

    CollectionAssert.Contains(MimeType.FindExtensionsByMimeType("image/jpeg"),
                              ".jpg");

    Assert.IsEmpty(MimeType.FindExtensionsByMimeType("application/x-hogemoge"));

    Assert.Throws<ArgumentNullException>(() => MimeType.FindExtensionsByMimeType((string)null!));
    Assert.Throws<ArgumentNullException>(() => MimeType.FindExtensionsByMimeType((MimeType)null!));

    if (IsRunningOnUnix)
      Assert.Throws<ArgumentNullException>(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: null!));

    if (IsRunningOnWindows)
      Assert.DoesNotThrow(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: null!));
  }

  [Test]
  public void FindExtensionsByMimeType_WithMimeTypesFile()
  {
    if (!IsRunningOnUnix)
      return;

    var pseudoMimeTypesFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, ".mime.types");

    IOUtils.UsingFile(pseudoMimeTypesFile, f => {
      File.WriteAllText(f.FullName, "application/x-foo-bar\tfoo-bar");

      CollectionAssert.Contains(
        MimeType.FindExtensionsByMimeType("application/x-foo-bar", mimeTypesFile: f.FullName),
        ".foo-bar"
      );
      Assert.IsEmpty(MimeType.FindExtensionsByMimeType("application/x-hogemoge", mimeTypesFile: f.FullName));
    });
  }

  [Test]
  public void FindExtensionsByMimeType_WithMimeTypesFile_FileNotFound()
  {
    const string nonExistentFile = ".nonexistent.mime.types";

    if (IsRunningOnUnix)
      Assert.Throws<FileNotFoundException>(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: nonExistentFile));

    if (IsRunningOnWindows)
      Assert.DoesNotThrow(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: nonExistentFile));
  }
}
