// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection {
  [TestFixture()]
  public class ParameterInfoExtensionsTests {
    class C {
      public C(int x) => throw new NotImplementedException();
      public int M(int x, string y) => throw new NotImplementedException();
      public void M(object z) => throw new NotImplementedException();
    }

    public static System.Collections.IEnumerable YieldIsReturnParameterTestCases()
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

    [TestCaseSource(nameof(YieldIsReturnParameterTestCases))]
    public void IsReturnParameter(ParameterInfo para, bool expected)
    {
      Assert.AreEqual(expected, para.IsReturnParameter(), $"{para.Member} {para.Name}");
    }
  }
}