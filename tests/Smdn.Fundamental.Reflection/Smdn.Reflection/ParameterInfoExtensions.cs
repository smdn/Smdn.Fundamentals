// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0067

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
    public int P { get; set; }
    public event EventHandler E;
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

    foreach (var property in typeof(C).GetProperties()) {
      if (property.SetMethod is not null)
        yield return new object[] { property.SetMethod.GetParameters()[0], false };
      if (property.GetMethod is not null)
        yield return new object[] { property.GetMethod.ReturnParameter, true };
    }

    foreach (var ev in typeof(C).GetEvents()) {
      if (ev.AddMethod is not null)
        yield return new object[] { ev.AddMethod.GetParameters()[0], false };
      if (ev.RemoveMethod is not null)
        yield return new object[] { ev.RemoveMethod.GetParameters()[0], false };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsReturnParameter))]
  public void IsReturnParameter(ParameterInfo para, bool expected)
    => Assert.AreEqual(expected, para.IsReturnParameter(), $"{para.Member} {para.Name}");
}
