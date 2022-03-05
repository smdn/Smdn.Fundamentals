// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn {
  [TestFixture()]
  public class MimeTypeTests {
    public static bool IsRunningOnWindows =>
      RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsRunningOnUnix =>
      RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
      RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    [Test]
    public void TestConstructor()
    {
      var mime = new MimeType("text", "plain");

      Assert.AreEqual("text", mime.Type);
      Assert.AreEqual("plain", mime.SubType);
    }

    [Test]
    public void TestConstructor_Tuple()
    {
      var m = ("text", "plain");
      var mime = new MimeType(m);

      Assert.AreEqual("text", mime.Type);
      Assert.AreEqual("plain", mime.SubType);
    }

    [Test]
    public void TestConstructorInvalidArgument()
    {
      Assert.Throws<ArgumentNullException>(() => new MimeType((string)null), "#1");
      Assert.Throws<ArgumentException>(() => new MimeType(string.Empty), "#2");
      Assert.Throws<ArgumentException>(() => new MimeType("text"), "#3");
      Assert.Throws<ArgumentException>(() => new MimeType("text/"), "#4");
      Assert.Throws<ArgumentException>(() => new MimeType("/plain"), "#5");
      Assert.Throws<ArgumentException>(() => new MimeType("text/plain/hoge"), "#6");

      Assert.Throws<ArgumentNullException>(() => new MimeType(null, "foo"), "#7");
      Assert.Throws<ArgumentException>(() => new MimeType(string.Empty, "foo"), "#8");

      Assert.Throws<ArgumentNullException>(() => new MimeType("foo", null), "#9");
      Assert.Throws<ArgumentException>(() => new MimeType("foo", string.Empty), "#10");

      Assert.Throws<ArgumentNullException>(() => new MimeType((null, "foo")), "#11");
      Assert.Throws<ArgumentException>(() => new MimeType((string.Empty, "foo")), "#12");

      Assert.Throws<ArgumentNullException>(() => new MimeType(("foo", null)), "#13");
      Assert.Throws<ArgumentException>(() => new MimeType(("foo", string.Empty)), "#14");
    }

    [Test]
    public void TestDeconstruct()
    {
      var (type, subType) = MimeType.TextPlain;

      Assert.AreEqual("text", type);
      Assert.AreEqual("plain", subType);
    }

    [Test]
    public void TestEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("text", "plain")).Equals(new MimeType("text", "PLAIN")));
      Assert.IsFalse((new MimeType("text", "plain")).Equals(new MimeType("TEXT", "plain")));
      Assert.IsFalse((new MimeType("text", "plain")).Equals(new MimeType("TEXT", "PLAIN")));

      Assert.IsFalse((new MimeType("text", "html")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("image", "plain")).Equals(new MimeType("text", "plain")));
      Assert.IsFalse((new MimeType("application", "octet-stream")).Equals(new MimeType("text", "plain")));
    }

    [Test]
    public void TestTypeEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain").TypeEquals(new MimeType("text", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").TypeEquals(new MimeType("TEXT", "plain"))));

      Assert.IsTrue((new MimeType("text", "plain").TypeEquals(new MimeType("text", "html"))));
      Assert.IsFalse((new MimeType("text", "plain").TypeEquals(new MimeType("image", "x-icon"))));
    }

    [Test]
    public void TestSubTypeEquals()
    {
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "PLAIN"))));

      Assert.IsTrue((new MimeType("text", "plain").SubTypeEquals(new MimeType("image", "plain"))));
      Assert.IsFalse((new MimeType("text", "plain").SubTypeEquals(new MimeType("text", "html"))));
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("text", "plain")));
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("text", "PLAIN")));
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("TEXT", "plain")));
      Assert.IsTrue((new MimeType("text", "plain")).EqualsIgnoreCase(new MimeType("TEXT", "PLAIN")));
    }

    [Test]
    public void TestTypeEqualsIgnoreCase()
    {
      Assert.IsTrue((new MimeType("text", "plain").TypeEqualsIgnoreCase(new MimeType("text", "plain"))));
      Assert.IsTrue((new MimeType("text", "plain").TypeEqualsIgnoreCase(new MimeType("TEXT", "plain"))));
    }

    [Test]
    public void TestSubTypeEqualsIgnoreCase()
    {
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEqualsIgnoreCase(new MimeType("text", "plain"))));
      Assert.IsTrue((new MimeType("text", "plain").SubTypeEqualsIgnoreCase(new MimeType("text", "PLAIN"))));
    }

    [Test]
    public void TestFindMimeTypeByExtension()
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

      Assert.Throws<ArgumentNullException>(() => MimeType.FindMimeTypeByExtension(null));

      if (IsRunningOnUnix)
        Assert.Throws<ArgumentNullException>(() => MimeType.FindMimeTypeByExtension("hoge.txt", mimeTypesFile: null));

      if (IsRunningOnWindows)
        Assert.DoesNotThrow(() => MimeType.FindMimeTypeByExtension("hoge.txt", mimeTypesFile: null));
    }

    [Test]
    public void TestFindMimeTypeByExtension_WithMimeTypesFile()
    {
      if (!IsRunningOnUnix)
        return;

      var pseudoMimeTypesFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, ".mime.types");

      TestUtils.UsingFile(pseudoMimeTypesFile, () => {
        File.WriteAllText(pseudoMimeTypesFile, "application/x-foo-bar\tfoo-bar");

        Assert.AreEqual(
          new MimeType("application/x-foo-bar"),
          MimeType.FindMimeTypeByExtension(".foo-bar", mimeTypesFile: pseudoMimeTypesFile)
        );

        Assert.IsNull(
          MimeType.FindMimeTypeByExtension(".hoge", mimeTypesFile: pseudoMimeTypesFile)
        );
      });
    }

    [Test]
    public void TestFindMimeTypeByExtension_WithMimeTypesFile_FileNotFound()
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
    public void TestFindExtensionsByMimeType()
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

      Assert.Throws<ArgumentNullException>(() => MimeType.FindExtensionsByMimeType((string)null));
      Assert.Throws<ArgumentNullException>(() => MimeType.FindExtensionsByMimeType((MimeType)null));

      if (IsRunningOnUnix)
        Assert.Throws<ArgumentNullException>(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: null));

      if (IsRunningOnWindows)
        Assert.DoesNotThrow(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: null));
    }

    [Test]
    public void TestFindExtensionsByMimeType_WithMimeTypesFile()
    {
      if (!IsRunningOnUnix)
        return;

      var pseudoMimeTypesFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, ".mime.types");

      TestUtils.UsingFile(pseudoMimeTypesFile, () => {
        File.WriteAllText(pseudoMimeTypesFile, "application/x-foo-bar\tfoo-bar");

        CollectionAssert.Contains(
          MimeType.FindExtensionsByMimeType("application/x-foo-bar", mimeTypesFile: pseudoMimeTypesFile),
          ".foo-bar"
        );
        Assert.IsEmpty(MimeType.FindExtensionsByMimeType("application/x-hogemoge", mimeTypesFile: pseudoMimeTypesFile));
      });
    }

    [Test]
    public void TestFindExtensionsByMimeType_WithMimeTypesFile_FileNotFound()
    {
      const string nonExistentFile = ".nonexistent.mime.types";

      if (IsRunningOnUnix)
        Assert.Throws<FileNotFoundException>(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: nonExistentFile));

      if (IsRunningOnWindows)
        Assert.DoesNotThrow(() => MimeType.FindExtensionsByMimeType("text/plain", mimeTypesFile: nonExistentFile));
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("text/plain", MimeType.TextPlain.ToString());
      Assert.AreEqual("application/octet-stream", MimeType.ApplicationOctetStream.ToString());
      Assert.AreEqual("text/html", MimeType.CreateTextType("html").ToString());
    }

    [Test]
    public void TestExplicitToStringCoversion()
    {
      Assert.AreEqual("text/plain", (string)MimeType.TextPlain);
      Assert.AreEqual("application/octet-stream", (string)MimeType.ApplicationOctetStream);
      Assert.AreEqual("text/html", (string)MimeType.CreateTextType("html"));

      Assert.IsNull((string)((MimeType)null));
    }

    [Test]
    public void TestTryParse()
    {
      MimeType result;

      Assert.IsFalse(MimeType.TryParse(null, out result), "#1");
      Assert.IsNull(result, "#1");

      Assert.IsFalse(MimeType.TryParse(string.Empty, out result), "#2");
      Assert.IsNull(result, "#2");

      Assert.IsFalse(MimeType.TryParse("text", out result), "#3");
      Assert.IsNull(result, "#3");

      Assert.IsFalse(MimeType.TryParse("text/", out result), "#4");
      Assert.IsNull(result, "#4");

      Assert.IsFalse(MimeType.TryParse("/plain", out result), "#5");
      Assert.IsNull(result, "#5");

      Assert.IsFalse(MimeType.TryParse("text/plain/hoge", out result), "#6");
      Assert.IsNull(result, "#6");

      Assert.IsTrue(MimeType.TryParse("text/plain", out result), "#7");
      Assert.AreEqual(MimeType.TextPlain, result, "#7");
    }

    [Test]
    public void TestTryParse_Tuple()
    {
      Assert.IsTrue(MimeType.TryParse("text/plain", out (string type, string subType) result));
      Assert.AreEqual("text", result.type);
      Assert.AreEqual("plain", result.subType);
    }

    [Test]
    public void TestParse_Tuple()
    {
      (var type, var subType) = MimeType.Parse("text/plain");

      Assert.AreEqual("text", type);
      Assert.AreEqual("plain", subType);

      Assert.Throws<ArgumentNullException>(() => MimeType.Parse(null));
      Assert.Throws<ArgumentException>(() => MimeType.Parse(string.Empty));
      Assert.Throws<ArgumentException>(() => MimeType.Parse("text/"));
      Assert.Throws<ArgumentException>(() => MimeType.Parse("/plain"));
    }
  }
}
