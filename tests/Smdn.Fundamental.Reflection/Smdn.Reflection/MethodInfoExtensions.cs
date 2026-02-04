// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS8597

using System;
using System.Linq;
using System.Threading.Tasks;
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
    public virtual void MVirtual<T>() => throw new NotImplementedException();

    public abstract int PAbstract { get; }
    public virtual int PVirtual => throw new NotImplementedException();
  }

  class COverride : CAbstract {
    public override void MAbstract() => throw new NotImplementedException();
    public override void MVirtual() => throw new NotImplementedException();
    public override void MVirtual<T>() => throw new NotImplementedException();

    public override int PAbstract => throw new NotImplementedException();
    public override int PVirtual => throw new NotImplementedException();
  }

  class CSealed : COverride {
    public sealed override void MAbstract() => throw new NotImplementedException();
    public sealed override void MVirtual() => throw new NotSupportedException();
    public sealed override void MVirtual<T>() => throw new NotImplementedException();

    public sealed override int PAbstract => throw new NotImplementedException();
    public sealed override int PVirtual => throw new NotImplementedException();
  }

  class CVirtual {
    public virtual void MVirtual() => throw new NotImplementedException();
    public virtual void MVirtual<T>() => throw new NotImplementedException();

    public virtual int PVirtual => throw new NotImplementedException();
  }

  abstract class CNew : CVirtual {
    public new void MVirtual() => throw new NotImplementedException();
    public new void MVirtual<T>() => throw new NotImplementedException();

    public new int PVirtual => throw new NotImplementedException();
  }

  abstract class CNewVirtual : CVirtual {
    public new virtual void MVirtual() => throw new NotImplementedException();
    public new void MVirtual<T>() => throw new NotImplementedException();

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

  [TestCase(typeof(CAbstract), nameof(CAbstract.M), 0, false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MAbstract), 0, false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MVirtual), 0, false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MVirtual), 1, false)]
  [TestCase(typeof(COverride), nameof(COverride.MAbstract), 0, true)]
  [TestCase(typeof(COverride), nameof(COverride.MVirtual), 0, true)]
  [TestCase(typeof(COverride), nameof(COverride.MVirtual), 1, true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MAbstract), 0, true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MVirtual), 0, true)]
  [TestCase(typeof(CSealed), nameof(CSealed.MVirtual), 1, true)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.MVirtual), 0, false)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.MVirtual), 1, false)]
  [TestCase(typeof(CNew), nameof(CNew.MVirtual), 0, false)]
  [TestCase(typeof(CNew), nameof(CNew.MVirtual), 1, false)]
  [TestCase(typeof(CNewVirtual), nameof(CNewVirtual.MVirtual), 0, false)]
  [TestCase(typeof(CNewVirtual), nameof(CNewVirtual.MVirtual), 1, false)]
#if NET7_0_OR_GREATER
  [TestCase(typeof(IStaticAbstract), nameof(IStaticAbstract.MStaticAbstract), 0, false)]
  [TestCase(typeof(IStaticVirtual), nameof(IStaticVirtual.MStaticVirtual), 0, false)]
  [TestCase(typeof(CImplementationOfIStaticAbstract), nameof(CImplementationOfIStaticAbstract.MStaticAbstract), 0, false)]
  //[TestCase(typeof(CImplementationOfIStaticVirtual), $"Smdn.Reflection.{nameof(MethodInfoExtensionsTests)}.{nameof(IStaticVirtual)}.{nameof(IStaticVirtual.MStaticVirtual)}", 0, false)]
  [TestCase(typeof(CImplementationOfIStaticVirtual), nameof(CImplementationOfIStaticVirtual.MStaticVirtualToBeReimplemented), 0, false)]
  [TestCase(typeof(IStaticNew), nameof(IStaticNew.MStaticAbstract), 0, false)]
  [TestCase(typeof(IStaticNew), nameof(IStaticNew.MStaticVirtual), 0, false)]
  [TestCase(typeof(CImplementationOfIStaticNew), nameof(CImplementationOfIStaticNew.MStaticAbstract), 0, false)]
  [TestCase(typeof(CImplementationOfIStaticNew), nameof(CImplementationOfIStaticNew.MStaticVirtual), 0, false)]
#endif
  public void IsOverride_Method(Type type, string methodName, int genericParameterCount, bool isOverride)
  {
    var method = type
      .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
      .First(
        m =>
          string.Equals(m.Name, methodName, StringComparison.Ordinal) &&
          m.GetGenericArguments().Length == genericParameterCount
      );

    Assert.That(method.IsOverride(), Is.EqualTo(isOverride), $"is override? {type}.{methodName}");
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
  public void IsOverride_Method_IgnoreReflectedType(Type type, string methodName, Type declaringType, bool isOverride)
  {
    var method = type.GetMethod(
      name: methodName,
      bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
      binder: null,
      types: Type.EmptyTypes,
      modifiers: null
    );

    Assert.That(type, Is.EqualTo(method!.ReflectedType), nameof(method.ReflectedType));
    Assert.That(declaringType, Is.EqualTo(method!.DeclaringType), nameof(method.DeclaringType));
    Assert.That(method!.IsOverride(), Is.EqualTo(isOverride), $"is override? {type}.{methodName}");
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

    Assert.That(getter!.IsOverride(), Is.EqualTo(isOverride), $"is override? {type}.{propertyName}");
  }

  [Test]
  public void IsOverride_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsOverride());

  // cspell:ignore reoverride
  class CReoverride : COverride {
    public override void MAbstract() => throw new NotImplementedException();
    public override void MVirtual() => throw new NotSupportedException();
    public override void MVirtual<T>() => throw new NotImplementedException();
  }

  class CInheritWithoutOverride : COverride {
  }

  class COverrideBaseOfBase : CInheritWithoutOverride {
    public override void MAbstract() => throw new NotImplementedException();
    public override void MVirtual() => throw new NotSupportedException();
    public override void MVirtual<T>() => throw new NotImplementedException();
  }

  class CVirtualOverloads {
    public virtual void M(int x) => throw new NotImplementedException();
    public virtual void M<T>(int x) => throw new NotImplementedException();
  }

  class COverrideOverloads : CVirtualOverloads {
    public override void M(int x) => throw new NotImplementedException();
    public override void M<T>(int x) => throw new NotImplementedException();
  }

  interface IGetImmediateOverriddenMethod {
    void M();
  }

  class CExplicitInterfaceImplementation : IGetImmediateOverriddenMethod {
    void IGetImmediateOverriddenMethod.M() => throw new NotImplementedException();
  }

  class CGenericBase<T> {
    public virtual void M(T arg) => throw new NotImplementedException();
  }

  class CDerivedGenericDefinition<T> : CGenericBase<T> {
    public override void M(T arg) => throw new NotImplementedException();
  }

  class CDerivedConstructedGeneric : CGenericBase<int> {
    public override void M(int arg) => throw new NotImplementedException();
  }

#nullable enable
  [TestCase(typeof(CAbstract), nameof(CAbstract.M), 0, null)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MAbstract), 0, null)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MVirtual), 0, null)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.MVirtual), 1, null)]
  [TestCase(typeof(COverride), nameof(COverride.MAbstract), 0, typeof(CAbstract))]
  [TestCase(typeof(COverride), nameof(COverride.MVirtual), 0, typeof(CAbstract))]
  [TestCase(typeof(COverride), nameof(COverride.MVirtual), 1, typeof(CAbstract))]
  [TestCase(typeof(CSealed), nameof(CSealed.MAbstract), 0, typeof(COverride))]
  [TestCase(typeof(CSealed), nameof(CSealed.MVirtual), 0, typeof(COverride))]
  [TestCase(typeof(CSealed), nameof(CSealed.MVirtual), 1, typeof(COverride))]
  [TestCase(typeof(CNew), nameof(CSealed.MVirtual), 0, null)]
  [TestCase(typeof(CReoverride), nameof(CReoverride.MAbstract), 0, typeof(COverride))]
  [TestCase(typeof(CReoverride), nameof(CReoverride.MVirtual), 0, typeof(COverride))]
  [TestCase(typeof(CReoverride), nameof(CReoverride.MVirtual), 1, typeof(COverride))]
  [TestCase(typeof(COverrideBaseOfBase), nameof(COverrideBaseOfBase.MAbstract), 0, typeof(COverride))]
  [TestCase(typeof(COverrideBaseOfBase), nameof(COverrideBaseOfBase.MVirtual), 0, typeof(COverride))]
  [TestCase(typeof(COverrideBaseOfBase), nameof(COverrideBaseOfBase.MVirtual), 1, typeof(COverride))]
  [TestCase(typeof(COverrideOverloads), nameof(COverrideOverloads.M), 0, typeof(CVirtualOverloads))]
  [TestCase(typeof(COverrideOverloads), nameof(COverrideOverloads.M), 1, typeof(CVirtualOverloads))]
  [TestCase(typeof(CExplicitInterfaceImplementation), $"Smdn.Reflection.MethodInfoExtensionsTests.{nameof(IGetImmediateOverriddenMethod)}.{nameof(IGetImmediateOverriddenMethod.M)}", 0, null)]
  [TestCase(typeof(CDerivedGenericDefinition<>), nameof(CDerivedGenericDefinition<>.M), 0, typeof(CGenericBase<>))]
  [TestCase(typeof(CDerivedConstructedGeneric), nameof(CDerivedConstructedGeneric.M), 0, typeof(CGenericBase<int>))]
  public void GetImmediateOverriddenMethod(Type type, string methodName, int genericParameterCount, Type? expectedBaseType)
  {
    var method = type
      .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly )
      .Where(m => methodName.Equals(m.Name, StringComparison.Ordinal))
      .First(
        m => genericParameterCount == 0
          ? !m.IsGenericMethod
          : m.IsGenericMethod && m.GetGenericArguments().Length == genericParameterCount
      );

    var overriddenMethod = method.GetImmediateOverriddenMethod();

    if (expectedBaseType is null) {
      Assert.That(overriddenMethod, Is.Null);
    }
    else {
      Assert.That(overriddenMethod, Is.Not.Null);
      if (expectedBaseType.IsGenericTypeDefinition)
        Assert.That(overriddenMethod.DeclaringType?.ToString(), Is.EqualTo(expectedBaseType.ToString())); // ???
      else
        Assert.That(overriddenMethod.DeclaringType, Is.EqualTo(expectedBaseType));
      Assert.That(overriddenMethod.Name, Is.EqualTo(methodName));
      Assert.That(overriddenMethod.GetParameters().Length, Is.EqualTo(method.GetParameters().Length));

      if (method.IsGenericMethod) {
        Assert.That(overriddenMethod.IsGenericMethod, Is.True);
        Assert.That(overriddenMethod.GetGenericArguments().Count, Is.EqualTo(genericParameterCount));
      }
      else {
        Assert.That(overriddenMethod.IsGenericMethod, Is.False);
      }
    }
  }
#nullable restore

  [Test]
  public void GetImmediateOverriddenMethod_ArgumentNull()
    => Assert.That(
      () => ((MethodInfo)null!).GetImmediateOverriddenMethod(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("m")
    );

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
    => Assert.That(m.IsDelegateSignatureMethod(), Is.EqualTo(expected), $"Type: {m.DeclaringType}, {m}");

  [Test]
  public void IsDelegateSignatureMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsDelegateSignatureMethod());

  // ref: https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/proposals/csharp-8.0/readonly-instance-members
  struct SReadOnlyMethod {
    public readonly int MReadOnly() => throw new NotImplementedException();
    public static void MStatic() => throw new NotImplementedException();
  }

  [TestCase(typeof(SReadOnlyMethod), nameof(SReadOnlyMethod.MReadOnly), true)]
  [TestCase(typeof(SReadOnlyMethod), nameof(SReadOnlyMethod.MStatic), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.M), false)]
  public void IsReadOnly_Method(Type type, string methodName, bool expected)
  {
    var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(method!.IsReadOnly(), Is.EqualTo(expected), $"is readonly? {type}.{methodName}");
  }

  [Test]
  public void IsReadOnly_Method_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsReadOnly());

  class CAsyncStateMachine {
    public virtual void M() => throw new NotImplementedException();
    public virtual Task MTask() => Task.Delay(0);
    public virtual Task<int> MTaskOfInt() => Task.FromResult(0);
    public async void MAsyncVoid() { await Task.Delay(0); }
    public async Task MAsyncTask() { await Task.Delay(0); }
    public async Task<int> MAsyncTaskOfInt() { await Task.Delay(0); return 0; }

    public virtual ValueTask MValueTask() => new ValueTask();
    public virtual ValueTask<int> MValueTaskOfInt() => new ValueTask<int>();
    public async ValueTask MAsyncValueTask() { await Task.Delay(0); }
    public async ValueTask<int> MAsyncValueTaskOfInt() { await Task.Delay(0); return 0; }
  }

  class CAsyncStateMachineOverride : CAsyncStateMachine {
    public override async void M() { await Task.Delay(0); }
    public override async Task MTask() { await Task.Delay(0); }
    public override async Task<int> MTaskOfInt() { await Task.Delay(0); return 0; }

    public override async ValueTask MValueTask() { await Task.Delay(0); }
    public override async ValueTask<int> MValueTaskOfInt() { await Task.Delay(0); return 0; }
  }

  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.M), false)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MTask), false)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MTaskOfInt), false)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MAsyncVoid), true)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MAsyncTask), true)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MAsyncTaskOfInt), true)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MValueTask), false)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MValueTaskOfInt), false)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MAsyncValueTask), true)]
  [TestCase(typeof(CAsyncStateMachine), nameof(CAsyncStateMachine.MAsyncValueTaskOfInt), true)]
  [TestCase(typeof(CAsyncStateMachineOverride), nameof(CAsyncStateMachineOverride.M), true)]
  [TestCase(typeof(CAsyncStateMachineOverride), nameof(CAsyncStateMachineOverride.MTask), true)]
  [TestCase(typeof(CAsyncStateMachineOverride), nameof(CAsyncStateMachineOverride.MTaskOfInt), true)]
  [TestCase(typeof(CAsyncStateMachineOverride), nameof(CAsyncStateMachineOverride.MValueTask), true)]
  [TestCase(typeof(CAsyncStateMachineOverride), nameof(CAsyncStateMachineOverride.MValueTaskOfInt), true)]
  public void IsAsyncStateMachine(Type type, string methodName, bool isAsyncStateMachine)
  {
    var method = type.GetMethod(
      methodName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly
    );

    Assert.That(
      method!.IsAsyncStateMachine(),
      Is.EqualTo(isAsyncStateMachine),
      $"{type}.{methodName}"
    );
  }

  [Test]
  public void IsAsyncStateMachine_ArgumentNull()
    => Assert.That(
      () => ((MethodInfo)null!).IsAsyncStateMachine(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("m")
    );
}
