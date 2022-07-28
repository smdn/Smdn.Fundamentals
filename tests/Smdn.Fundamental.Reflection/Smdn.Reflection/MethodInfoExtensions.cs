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
    public void M() => throw new NotImplementedException();
    public int P => throw new NotImplementedException();

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

  abstract class CNewVirtual : CVirtual {
    public new virtual void MVirtual() => throw new NotImplementedException();

    public new virtual int PVirtual => throw new NotImplementedException();
  }

#if NET7_0_OR_GREATER
  interface IStaticAbstract {
    static abstract void MStaticAbstract();
  }

  interface IStaticVirtual {
    static virtual void MStaticVirtual() => throw new NotImplementedException();
    static virtual void MStaticVirtualToBeReimplemented() => throw new NotImplementedException();
  }

  class CImplementationOfIStaticAbstract : IStaticAbstract {
    public static void MStaticAbstract() => throw new NotImplementedException();
  }

  class CImplementationOfIStaticVirtual : IStaticVirtual {
    public static void MStaticVirtualToBeReimplemented() => throw new NotImplementedException();
  }

  interface IStaticNew : IStaticAbstract, IStaticVirtual {
    new static void MStaticAbstract() => throw new NotImplementedException();
    new static void MStaticVirtual() => throw new NotImplementedException();
  }

  class CImplementationOfIStaticNew : IStaticNew {
    public static void MStaticAbstract() => throw new NotImplementedException();
    public static void MStaticVirtual() => throw new NotImplementedException();
  }
#endif

  [TestCase(typeof(CAbstract), nameof(CAbstract.M), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MAbstract), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MVirtual), false)]
  [TestCase(typeof(COverride), nameof(COverride.MAbstract), true)]
  [TestCase(typeof(COverride), nameof(COverride.MVirtual), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MAbstract), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MVirtual), true)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.MVirtual), false)]
  [TestCase(typeof(CNew), nameof(CNew.MVirtual), false)]
  [TestCase(typeof(CNewVirtual), nameof(CNewVirtual.MVirtual), false)]
#if NET7_0_OR_GREATER
  [TestCase(typeof(IStaticAbstract), nameof(IStaticAbstract.MStaticAbstract), false)]
  [TestCase(typeof(IStaticVirtual), nameof(IStaticVirtual.MStaticVirtual), false)]
  [TestCase(typeof(CImplementationOfIStaticAbstract), nameof(CImplementationOfIStaticAbstract.MStaticAbstract), false)]
  //[TestCase(typeof(CImplementationOfIStaticVirtual), $"Smdn.Reflection.{nameof(MethodInfoExtensionsTests)}.{nameof(IStaticVirtual)}.{nameof(IStaticVirtual.MStaticVirtual)}", false)]
  [TestCase(typeof(CImplementationOfIStaticVirtual), nameof(CImplementationOfIStaticVirtual.MStaticVirtualToBeReimplemented), false)]
  [TestCase(typeof(IStaticNew), nameof(IStaticNew.MStaticAbstract), false)]
  [TestCase(typeof(IStaticNew), nameof(IStaticNew.MStaticVirtual), false)]
  [TestCase(typeof(CImplementationOfIStaticNew), nameof(CImplementationOfIStaticNew.MStaticAbstract), false)]
  [TestCase(typeof(CImplementationOfIStaticNew), nameof(CImplementationOfIStaticNew.MStaticVirtual), false)]
#endif
  public void IsOverride_Method(Type type, string methodName, bool isOverride)
  {
    var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.AreEqual(isOverride, method!.IsOverride(), $"is override? {type}.{methodName}");
  }

  private class CObject : object { }

  [TestCase(typeof(object), nameof(object.ToString), typeof(object), false)]
  [TestCase(typeof(CObject), nameof(CObject.ToString), typeof(object), false)] // = object.ToString
  [TestCase(typeof(Convert), nameof(Convert.ToString), typeof(object), false)] // = object.ToString
  [TestCase(typeof(object), nameof(object.GetHashCode), typeof(object), false)]
  [TestCase(typeof(CObject), nameof(CObject.GetHashCode), typeof(object), false)] // = object.GetHashCode
  [TestCase(typeof(System.IO.Stream), nameof(System.IO.Stream.Close), typeof(System.IO.Stream), false)]
  [TestCase(typeof(System.IO.MemoryStream), nameof(System.IO.MemoryStream.Close), typeof(System.IO.Stream), false)] // = Stream.Close
  [TestCase(typeof(System.IO.Stream), nameof(System.IO.Stream.Dispose), typeof(System.IO.Stream), false)]
  [TestCase(typeof(System.IO.MemoryStream), nameof(System.IO.MemoryStream.Dispose), typeof(System.IO.Stream), false)] // = Stream.Dispose
  public void IsOverride_Method_IgnoreRelectedType(Type type, string methodName, Type declaringType, bool isOverride)
  {
    var method = type.GetMethod(
      name: methodName,
      bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
      binder: null,
      types: Type.EmptyTypes,
      modifiers: null
    );

    Assert.AreEqual(method!.ReflectedType, type, nameof(method.ReflectedType));
    Assert.AreEqual(method!.DeclaringType, declaringType, nameof(method.DeclaringType));
    Assert.AreEqual(isOverride, method!.IsOverride(), $"is override? {type}.{methodName}");
  }

  [TestCase(typeof(CAbstract), nameof(CAbstract.P), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.PAbstract), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.PVirtual), false)]
  [TestCase(typeof(COverride), nameof(COverride.PAbstract), true)]
  [TestCase(typeof(COverride), nameof(COverride.PVirtual), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.PAbstract), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.PVirtual), true)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.PVirtual), false)]
  [TestCase(typeof(CNew), nameof(CNew.PVirtual), false)]
  [TestCase(typeof(CNewVirtual), nameof(CNewVirtual.PVirtual), false)]
  public void IsOverride_AccessorMethod(Type type, string propertyName, bool isOverride)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    var getter = property!.GetGetMethod();

    Assert.AreEqual(isOverride, getter!.IsOverride(), $"is override? {type}.{propertyName}");
  }

  [Test]
  public void IsOverride_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsOverride());

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
