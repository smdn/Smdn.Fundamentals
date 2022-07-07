// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS8597

using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public partial class MethodInfoExtensionsTests {
  abstract class CAbstract {
    public abstract void MAbstract();
    public virtual void MVirtual() => throw new NotImplementedException();

    public abstract int PAbstract { get; }
    public virtual int PVirtual => throw new NotImplementedException();
  }

  class COverride : CAbstract {
    public override void MAbstract() => throw new NotImplementedException();
    public override void MVirtual() => throw new NotImplementedException();

    public override int PAbstract => throw new NotImplementedException();
    public override int PVirtual => throw new NotImplementedException();
  }

  class CSealed : COverride {
    public sealed override void MAbstract() => throw new NotImplementedException();
    public sealed override void MVirtual() => throw new NotSupportedException();

    public sealed override int PAbstract => throw new NotImplementedException();
    public sealed override int PVirtual => throw new NotImplementedException();
  }

  class CVirtual {
    public virtual void MVirtual() => throw new NotImplementedException();

    public virtual int PVirtual => throw new NotImplementedException();
  }

  abstract class CNew : CVirtual {
    public new void MVirtual() => throw new NotImplementedException();

    public new int PVirtual => throw new NotImplementedException();
  }

  [TestCase(typeof(CAbstract), nameof(CAbstract.MAbstract), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MVirtual), false)]
  [TestCase(typeof(COverride), nameof(COverride.MAbstract), true)]
  [TestCase(typeof(COverride), nameof(COverride.MVirtual), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MAbstract), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MVirtual), true)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.MVirtual), false)]
  [TestCase(typeof(CNew), nameof(CNew.MVirtual), false)]
  public void IsOverridden_Method(Type type, string methodName, bool isOverridden)
  {
    var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    Assert.AreEqual(isOverridden, method!.IsOverridden(), $"has override? {type}.{methodName}");
  }

  [TestCase(typeof(CAbstract), nameof(CAbstract.PAbstract), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.PVirtual), false)]
  [TestCase(typeof(COverride), nameof(COverride.PAbstract), true)]
  [TestCase(typeof(COverride), nameof(COverride.PVirtual), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.PAbstract), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.PVirtual), true)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.PVirtual), false)]
  [TestCase(typeof(CNew), nameof(CNew.PVirtual), false)]
  public void IsOverridden_AccessorMethod(Type type, string propertyName, bool isOverridden)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    var getter = property!.GetGetMethod();

    Assert.AreEqual(isOverridden, getter!.IsOverridden(), $"is overridden? {type}.{propertyName}");
  }

  [Test]
  public void IsOverridden_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsOverridden());

  class CDelegateSignatureMethod {
    public void M() => throw null;
    public virtual void Invoke() => throw null;

    private delegate void D0();
    public delegate void D1();

    public class NonDelegate {
      public virtual void Invoke() => throw null;
    }
  }

  private static System.Collections.IEnumerable YieldTestCases_IsDelegateSignatureMethod()
  {
    var testCaseType = typeof(CDelegateSignatureMethod);

    foreach (var m in testCaseType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
      yield return new object[] { m, false };
    }

    foreach (var nestedType in testCaseType.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
      foreach (var m in nestedType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
        yield return new object[] { m, nestedType.IsDelegate() && GetExpectedResult(m) };
      }
    }

    foreach (var (t, isConcreteDelegate) in new[] {
      (typeof(Delegate), false),
      (typeof(MulticastDelegate), false),
      (typeof(Action), true),
      (typeof(Action<int>), true),
      (typeof(Func<int, int>), true),
    }) {
      foreach (var m in t.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
        if (m.IsPropertyAccessorMethod())
          continue;

        yield return new object[] { m, isConcreteDelegate && GetExpectedResult(m) };
      }
    }

    static bool GetExpectedResult(MethodInfo m)
    {
      return !m.IsPropertyAccessorMethod() &&
        m.DeclaringType != typeof(object) &&
        m.DeclaringType != typeof(Delegate) &&
        m.DeclaringType != typeof(MulticastDelegate) &&
        !m.GetParameters().Concat(Enumerable.Repeat(m.ReturnParameter, 1)).Any(p => p.ParameterType == typeof(IAsyncResult)); // BeginInvoke/EndInvoke
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsDelegateSignatureMethod))]
  public void IsDelegateSignatureMethod(MethodInfo m, bool expected)
    => Assert.AreEqual(expected, m.IsDelegateSignatureMethod(), $"Type: {m.DeclaringType}, {m}");

  public void IsDelegateSignatureMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsDelegateSignatureMethod());
}
