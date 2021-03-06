// SPDX-FileCopyrightText: 2013 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.OperatingSystem {
  [TestFixture()]
  public class ShellTests {
#if SYSTEM_DIAGNOSTICS_PROCESS
    [Test]
    public void TestCreateProcessStartInfo()
    {
      Assert.Throws<ArgumentNullException>(() => Shell.CreateProcessStartInfo(null, (string)null), "#1");
      Assert.Throws<ArgumentNullException>(() => Shell.CreateProcessStartInfo(null, (string[])null), "#2");

      Assert.DoesNotThrow(() => Shell.CreateProcessStartInfo("foo", (string)null), "#3");
      Assert.DoesNotThrow(() => Shell.CreateProcessStartInfo("foo", (string[])null), "#4");
    }
#endif
  }
}