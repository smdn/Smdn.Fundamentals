// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Smdn.Test.NUnit;

[TestFixture]
public class IOUtilsTests {
  [Test]
  public void UsingCurrentDirectory()
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    Assert.That(Environment.CurrentDirectory, Is.Not.EqualTo(path), "pre");

    IOUtils.UsingDirectory(
      path: path,
      ensureDirectoryCreated: true,
      action: _ => {
        IOUtils.UsingCurrentDirectory(
          path: path,
          () => Assert.That(Environment.CurrentDirectory, Is.EqualTo(path), "action")
        );
      }
    );

    Assert.That(Environment.CurrentDirectory, Is.Not.EqualTo(path), "post");
  }

  [Test]
  public void UsingCurrentDirectoryAsync()
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    Assert.That(Environment.CurrentDirectory, Is.Not.EqualTo(path), "pre");

    IOUtils.UsingDirectory(
      path: path,
      ensureDirectoryCreated: true,
      action: _ => {
        IOUtils.UsingCurrentDirectoryAsync(
          path: path,
          async () => {
            Assert.That(Environment.CurrentDirectory, Is.EqualTo(path), "action");
            await Task.Delay(0);
          }
        );
      }
    );

    Assert.That(Environment.CurrentDirectory, Is.Not.EqualTo(path), "post");
  }

  [Repeat(10)]
  [TestCase(true)]
  [TestCase(false)]
  public void UsingDirectory(bool ensureDirectoryCreated)
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    DirectoryAssert.DoesNotExist(path, "pre");

    IOUtils.UsingDirectory(
      path: path,
      ensureDirectoryCreated: ensureDirectoryCreated,
      dir => {
        Assert.That(dir.FullName, Is.EqualTo(Path.GetFullPath(path)), "path");

        if (ensureDirectoryCreated) {
          DirectoryAssert.Exists(dir.FullName, "action");
        }
        else {
          DirectoryAssert.DoesNotExist(dir.FullName, "action");
          dir.Create();
        }
      }
    );

    DirectoryAssert.DoesNotExist(path, "post");
  }

  [Repeat(10)]
  [TestCase(true)]
  [TestCase(false)]
  public async Task UsingDirectoryAsync(bool ensureDirectoryCreated)
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    DirectoryAssert.DoesNotExist(path, "pre");

    await IOUtils.UsingDirectoryAsync(
      path: path,
      ensureDirectoryCreated: ensureDirectoryCreated,
      async dir => {
        Assert.That(dir.FullName, Is.EqualTo(Path.GetFullPath(path)), "path");

        if (ensureDirectoryCreated) {
          DirectoryAssert.Exists(dir.FullName, "action");
        }
        else {
          DirectoryAssert.DoesNotExist(dir.FullName, "action");
          dir.Create();
        }

        await Task.Delay(0);
      }
    );

    DirectoryAssert.DoesNotExist(path, "post");
  }

  [Repeat(10)]
  public void UsingFile()
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test.txt");

    FileAssert.DoesNotExist(path, "pre");

    File.WriteAllText(path, "test");

    IOUtils.UsingFile(
      path: path,
      file => {
        Assert.That(file.FullName, Is.EqualTo(Path.GetFullPath(path)), "path");

        FileAssert.DoesNotExist(file.FullName, "action");

        File.WriteAllText(file.FullName, "test");
      }
    );

    FileAssert.DoesNotExist(path, "post");
  }

  [Repeat(10)]
  public async Task UsingFileAsync()
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test.txt");

    FileAssert.DoesNotExist(path, "pre");

    File.WriteAllText(path, "test");

    await IOUtils.UsingFileAsync(
      path: path,
      async file => {
        Assert.That(file.FullName, Is.EqualTo(Path.GetFullPath(path)), "path");

        FileAssert.DoesNotExist(file.FullName, "action");

        File.WriteAllText(file.FullName, "test");

        await Task.Delay(0);
      }
    );

    FileAssert.DoesNotExist(path, "post");
  }
}
