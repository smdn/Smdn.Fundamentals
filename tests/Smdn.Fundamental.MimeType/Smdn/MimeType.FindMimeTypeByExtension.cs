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

    Assert.That(MimeType.FindMimeTypeByExtension("hoge.txt"), Is.EqualTo(MimeType.TextPlain));
    Assert.That(MimeType.FindMimeTypeByExtension("hoge.TXT"), Is.EqualTo(MimeType.TextPlain));
    Assert.That(MimeType.FindMimeTypeByExtension("index.html"), Is.EqualTo(MimeType.CreateTextType("html")));
    Assert.That(MimeType.FindMimeTypeByExtension("image.png"), Is.EqualTo(MimeType.CreateImageType("png")));
    Assert.That(MimeType.FindMimeTypeByExtension(".hoge"), Is.EqualTo(null));
    Assert.That(MimeType.FindMimeTypeByExtension("hoge"), Is.EqualTo(null));
    Assert.That(MimeType.FindMimeTypeByExtension(string.Empty), Is.EqualTo(null));
    Assert.That(MimeType.FindMimeTypeByExtension("."), Is.EqualTo(null));

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

      Assert.That(
        MimeType.FindMimeTypeByExtension(".foo-bar", mimeTypesFile: f.FullName),
        Is.EqualTo(new MimeType("application/x-foo-bar"))
      );

      Assert.That(
        MimeType.FindMimeTypeByExtension(".hoge", mimeTypesFile: f.FullName),
        Is.Null
      );
    });
  }

  [Test]
  public void FindMimeTypeByExtension_WithMimeTypesFile_FileNotFound()
  {
    const string nonExistentFile = ".nonexistent.mime.types";

    Assert.DoesNotThrow(() => {
      Assert.That(MimeType.FindMimeTypeByExtension(string.Empty, mimeTypesFile: nonExistentFile), Is.EqualTo(null));
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

    Assert.That(MimeType.FindExtensionsByMimeType("text/plain"), Has.Member(".txt"));
    Assert.That(MimeType.FindExtensionsByMimeType("TEXT/PLAIN"), Has.Member(".txt"));
    Assert.That(MimeType.FindExtensionsByMimeType(MimeType.TextPlain), Has.Member(".txt"));

    Assert.That(MimeType.FindExtensionsByMimeType("text/html"), Has.Member(".html"));

    Assert.That(MimeType.FindExtensionsByMimeType("image/jpeg"), Has.Member(".jpg"));

    Assert.That(MimeType.FindExtensionsByMimeType("application/x-hogemoge"), Is.Empty);

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

      Assert.That(
        MimeType.FindExtensionsByMimeType("application/x-foo-bar", mimeTypesFile: f.FullName), Has.Member(".foo-bar"
));
      Assert.That(MimeType.FindExtensionsByMimeType("application/x-hogemoge", mimeTypesFile: f.FullName), Is.Empty);
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
