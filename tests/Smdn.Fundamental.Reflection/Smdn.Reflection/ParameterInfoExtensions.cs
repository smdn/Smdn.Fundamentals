// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class ParameterInfoExtensionsTests {
  class C {
    public C(int x) => throw new NotImplementedException();
    public int M(int x, string y) => throw new NotImplementedException();
    public void M(object z) => throw new NotImplementedException();
  }

  private static System.Collections.IEnumerable YieldTestCases_IsReturnParameter()
  {
    foreach (var method in typeof(C).GetMethods()) {
      foreach (var para in method.GetParameters()) {
        yield return new object[] { para, false };
      }

      yield return new object[] { method.ReturnParameter, true };
    }

    foreach (var ctor in typeof(C).GetConstructors()) {
      foreach (var p in ctor.GetParameters()) {
        yield return new object[] { p, false };
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsReturnParameter))]
  public void IsReturnParameter(ParameterInfo para, bool expected)
    => Assert.AreEqual(expected, para.IsReturnParameter(), $"{para.Member} {para.Name}");
}
