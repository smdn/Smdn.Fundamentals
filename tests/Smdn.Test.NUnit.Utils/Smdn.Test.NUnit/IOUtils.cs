// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.Test.NUnit;

[TestFixture]
public class IOUtilsTests {
  [Test]
  public void UsingCurrentDirectory()
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    Assert.AreNotEqual(path, Environment.CurrentDirectory, "pre");

    IOUtils.UsingDirectory(
      path: path,
      ensureDirectoryCreated: true,
      action: () => {
        IOUtils.UsingCurrentDirectory(
          path: path,
          () => Assert.AreEqual(path, Environment.CurrentDirectory, "action")
        );
      }
    );

    Assert.AreNotEqual(path, Environment.CurrentDirectory, "post");
  }

  [Test]
  public void UsingCurrentDirectoryAsync()
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    Assert.AreNotEqual(path, Environment.CurrentDirectory, "pre");

    IOUtils.UsingDirectory(
      path: path,
      ensureDirectoryCreated: true,
      action: () => {
        IOUtils.UsingCurrentDirectoryAsync(
          path: path,
          async () => {
            Assert.AreEqual(path, Environment.CurrentDirectory, "action");
            await Task.Delay(0);
          }
        );
      }
    );

    Assert.AreNotEqual(path, Environment.CurrentDirectory, "post");
  }

  [TestCase(true)]
  [TestCase(false)]
  public void UsingDirectory(bool ensureDirectoryCreated)
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    DirectoryAssert.DoesNotExist(path, "pre");

    IOUtils.UsingDirectory(
      path: path,
      ensureDirectoryCreated: ensureDirectoryCreated,
      () => {
        if (ensureDirectoryCreated) {
          DirectoryAssert.Exists(path, "action");
        }
        else {
          DirectoryAssert.DoesNotExist(path, "action");
          Directory.CreateDirectory(path);
        }
      }
    );

    DirectoryAssert.DoesNotExist(path, "post");
  }

  [TestCase(true)]
  [TestCase(false)]
  public async Task UsingDirectoryAsync(bool ensureDirectoryCreated)
  {
    var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test");

    DirectoryAssert.DoesNotExist(path, "pre");

    await IOUtils.UsingDirectoryAsync(
      path: path,
      ensureDirectoryCreated: ensureDirectoryCreated,
      async (string p) => {
        if (ensureDirectoryCreated) {
          DirectoryAssert.Exists(p, "action");
        }
        else {
          DirectoryAssert.DoesNotExist(p, "action");
          Directory.CreateDirectory(p);
        }

        await Task.Delay(0);
      }
    );

    DirectoryAssert.DoesNotExist(path, "post");
  }
}
