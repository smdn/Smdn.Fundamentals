// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_RUNTIME_VERSIONING_FRAMEWORKNAME
using System;
using System.Runtime.Versioning;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class FrameworkNameUtilsTests {
  private static System.Collections.IEnumerable YieldTestCases_TryGetMoniker_String()
  {
    yield return new object[] { ".NETCoreApp,Version=v7.0", true, "net7.0" };
    yield return new object[] { ".NETCoreApp,Version=v6.0", true, "net6.0" };
    yield return new object[] { ".NETCoreApp,Version=v5.0", true, "net5.0" };
    yield return new object[] { ".NETCoreApp,Version=v3.1", true, "netcoreapp3.1" };
    yield return new object[] { ".NETStandard,Version=v2.1", true, "netstandard2.1" };
    yield return new object[] { ".NETStandard,Version=v1.6", true, "netstandard1.6" };
    yield return new object[] { ".NETFramework,Version=v4.7.1", true, "net471" };
    yield return new object[] { ".NETFramework,Version=v4.5", true, "net45" };
    yield return new object[] { ".NETFramework,Version=v4.0", true, "net40" };
    yield return new object[] { ".NETFramework,Version=v4.0,Profile=Compact", true, "net40" };
    yield return new object[] { ".NETUnknown,Version=v4.0", false, null };
    yield return new object[] { ".NETFramework,Format=invalid", false, null };
    yield return new object[] { string.Empty, false, null };
    yield return new object[] { null, false, null };
  }

  [TestCaseSource(nameof(YieldTestCases_TryGetMoniker_String))]
  public void TryGetMoniker_String(string input, bool expectedResult, string expectedOutput)
  {
    Assert.AreEqual(expectedResult, FrameworkNameUtils.TryGetMoniker(input, out var moniker));

    if (expectedResult)
      Assert.AreEqual(expectedOutput, moniker, nameof(moniker));
  }

  private static System.Collections.IEnumerable YieldTestCases_TryGetMoniker_FrameworkName()
  {
    yield return new object[] { new FrameworkName(".NETCoreApp", new Version(6, 0)), true, "net6.0" };
    yield return new object[] { new FrameworkName(".NETCoreApp", new Version(5, 0)), true, "net5.0" };
    yield return new object[] { new FrameworkName(".NETCoreApp", new Version(3, 1)), true, "netcoreapp3.1" };
    yield return new object[] { new FrameworkName(".NETStandard", new Version(2, 1)), true, "netstandard2.1" };
    yield return new object[] { new FrameworkName(".NETFramework", new Version(4, 7, 1)), true, "net471" };
    yield return new object[] { new FrameworkName(".NETFramework", new Version(4, 0)), true, "net40" };
    yield return new object[] { new FrameworkName(".NETFramework", new Version(4, 0), "Compact"), true, "net40" };
    yield return new object[] { new FrameworkName(".NETUnknown", new Version(4, 0)), false, null };
    yield return new object[] { null, false, null };
  }

  [TestCaseSource(nameof(YieldTestCases_TryGetMoniker_FrameworkName))]
  public void TryGetMoniker_FrameworkName(FrameworkName input, bool expectedResult, string expectedOutput)
  {
    Assert.AreEqual(expectedResult, FrameworkNameUtils.TryGetMoniker(input, out var moniker));

    if (expectedResult)
      Assert.AreEqual(expectedOutput, moniker, nameof(moniker));
  }
}
#endif // SYSTEM_RUNTIME_VERSIONING_FRAMEWORKNAME
