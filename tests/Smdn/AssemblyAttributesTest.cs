// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using NUnit.Framework;

[TestFixture()]
public class AssemblyAttributesTests {
  [Test]
  public void Test()
  {
    var assm = typeof(Smdn.Runtime).GetTypeInfo().Assembly;

    TestContext.Out.WriteLine("AssemblyProduct: '{0}'", assm.GetCustomAttribute<AssemblyProductAttribute>()?.Product);
    TestContext.Out.WriteLine("AssemblyInformationalVersion: '{0}'", assm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
    TestContext.Out.WriteLine("AssemblyConfiguration: '{0}'", assm.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration);

    Assert.Inconclusive("see output");
  }
}
