// SPDX-FileCopyrightText: 2013 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using NUnit.Framework;

namespace Smdn.OperatingSystem;

[TestFixture()]
public class ShellTests {
#if SYSTEM_DIAGNOSTICS_PROCESS
  [Test]
  public void TestCreateProcessStartInfo()
  {
    Assert.That(() => Shell.CreateProcessStartInfo(null, (string)null), Throws.ArgumentNullException, "#1");
    Assert.That(() => Shell.CreateProcessStartInfo(null, (string[])null), Throws.ArgumentNullException, "#2");

    Assert.That(() => Shell.CreateProcessStartInfo("foo", (string)null), Throws.Nothing, "#3");
    Assert.That(() => Shell.CreateProcessStartInfo("foo", (string[])null), Throws.Nothing, "#4");
  }
#endif
}
