// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cspell:ignore retval
#pragma warning disable IDE0051, CS0067, CS8597

using System;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class ParameterInfoExtensionsTests {
  public abstract class AccessorParameterAttribute : Attribute {
    public string DeclaringMemberName { get; }

    protected AccessorParameterAttribute(string declaringMemberName)
    {
      DeclaringMemberName = declaringMemberName;
    }

    protected MemberInfo GetDeclaringMember(ParameterInfo attributeProvider)
    {
      var type = attributeProvider.Member.DeclaringType!;

      return type.GetMember(
        name: DeclaringMemberName,
        bindingAttr: BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
      ).FirstOrDefault();
    }
  }

  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
  public class PropertyAccessorParameterAttribute : AccessorParameterAttribute
  {
    public PropertyAccessorParameterAttribute(string declaringMemberName)
      :  base(declaringMemberName)
    {
    }

    public PropertyInfo GetDeclaringProperty(ParameterInfo attributeProvider)
      => GetDeclaringMember(attributeProvider) as PropertyInfo;
  }

  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
  public class EventAccessorParameterAttribute : AccessorParameterAttribute
  {
    public EventAccessorParameterAttribute(string declaringMemberName)
      :  base(declaringMemberName)
    {
    }

    public EventInfo GetDeclaringEvent(ParameterInfo attributeProvider)
      => GetDeclaringMember(attributeProvider) as EventInfo;
  }

  class C {
    public C(int x) => throw new NotImplementedException();
    public int M(int x, string y) => throw new NotImplementedException();
    protected int MProtected(int x, string y) => throw new NotImplementedException();
    private int MPrivate(int x, string y) => throw new NotImplementedException();
    public void M(object z) => throw new NotImplementedException();

    public static int MStatic(int x, string y) => throw new NotImplementedException();

    public int P {
      [return: PropertyAccessorParameter(nameof(P))] get;
      [param: PropertyAccessorParameter(nameof(P))] set;
    }

    private int PPrivate {
      [return: PropertyAccessorParameter(nameof(PPrivate))] get;
      [param: PropertyAccessorParameter(nameof(PPrivate))] set;
    }

    public static int PStatic {
      [return: PropertyAccessorParameter(nameof(PStatic))] get;
      [param: PropertyAccessorParameter(nameof(PStatic))] set;
    }

    public event EventHandler E {
      [param: EventAccessorParameter(nameof(E))] add => throw null;
      [param: EventAccessorParameter(nameof(E))] remove => throw null;
    }

    private event EventHandler EPrivate {
      [param: EventAccessorParameter(nameof(EPrivate))] add => throw null;
      [param: EventAccessorParameter(nameof(EPrivate))] remove => throw null;
    }

    public static event EventHandler EStatic {
      [param: EventAccessorParameter(nameof(EStatic))] add => throw null;
      [param: EventAccessorParameter(nameof(EStatic))] remove => throw null;
    }
  }

  private static System.Collections.IEnumerable YieldTestCases_IsReturnParameter()
  {
    const BindingFlags bindingFlags =
      BindingFlags.Instance |
      BindingFlags.Static |
      BindingFlags.Public |
      BindingFlags.NonPublic;

    foreach (var method in typeof(C).GetMethods(bindingFlags)) {
      foreach (var para in method.GetParameters()) {
        yield return new object[] { para, false };
      }

      yield return new object[] { method.ReturnParameter, true };
    }

    foreach (var ctor in typeof(C).GetConstructors(bindingFlags)) {
      foreach (var p in ctor.GetParameters()) {
        yield return new object[] { p, false };
      }
    }

    foreach (var property in typeof(C).GetProperties(bindingFlags)) {
      if (property.SetMethod is not null)
        yield return new object[] { property.SetMethod.GetParameters()[0], false };
      if (property.GetMethod is not null)
        yield return new object[] { property.GetMethod.ReturnParameter, true };
    }

    foreach (var ev in typeof(C).GetEvents(bindingFlags)) {
      if (ev.AddMethod is not null)
        yield return new object[] { ev.AddMethod.GetParameters()[0], false };
      if (ev.RemoveMethod is not null)
        yield return new object[] { ev.RemoveMethod.GetParameters()[0], false };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsReturnParameter))]
  public void IsReturnParameter(ParameterInfo para, bool expected)
    => Assert.That(para.IsReturnParameter(), Is.EqualTo(expected), $"{para.Member} {para.Name}");

  [TestCase]
  public void IsReturnParameter_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((ParameterInfo)null!).IsReturnParameter());

  private static System.Collections.IEnumerable YieldTestCases_GetDeclaringProperty()
  {
    foreach (var args in YieldTestCases_IsReturnParameter()) {
      yield return new object[] { ((object[])args)![0]! };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_GetDeclaringProperty))]
  public void GetDeclaringProperty(ParameterInfo para)
  {
    var property = para.GetDeclaringProperty();
    var attr =
#if NETFRAMEWORK
      // ???
      para.GetCustomAttributes(typeof(PropertyAccessorParameterAttribute), inherit: false).FirstOrDefault() as PropertyAccessorParameterAttribute;
#else
      para.GetCustomAttribute<PropertyAccessorParameterAttribute>();
#endif

    if (attr is null)
      Assert.That(property, Is.Null);
    else
      Assert.That(property, Is.EqualTo(attr.GetDeclaringProperty(para)));
  }

  [TestCase]
  public void GetDeclaringProperty_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((ParameterInfo)null!).GetDeclaringProperty());

  private static System.Collections.IEnumerable YieldTestCases_GetDeclaringEvent()
  {
    foreach (var args in YieldTestCases_IsReturnParameter()) {
      yield return new object[] { ((object[])args)![0]! };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_GetDeclaringEvent))]
  public void GetDeclaringEvent(ParameterInfo para)
  {
    var ev = para.GetDeclaringEvent();
    var attr =
#if NETFRAMEWORK
      // ???
      para.GetCustomAttributes(typeof(EventAccessorParameterAttribute), inherit: false).FirstOrDefault() as EventAccessorParameterAttribute;
#else
      para.GetCustomAttribute<EventAccessorParameterAttribute>();
#endif

    if (attr is null)
      Assert.That(ev, Is.Null);
    else
      Assert.That(ev, Is.EqualTo(attr.GetDeclaringEvent(para)));
  }

  [TestCase]
  public void GetDeclaringEvent_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((ParameterInfo)null!).GetDeclaringEvent());

  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MInt), false)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MString), false)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MIEnumerableOfT), false)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MIEnumerableOfString), false)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MExtensionForInt), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MExtensionForString), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MExtensionForIEnumerableOfT), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CExtensionMethods), nameof(ParameterInfoExtensionsTestTypes.CExtensionMethods.MExtensionForIEnumerableOfString), true)]
  public void IsExtensionMethodFirstParameter(Type type, string methodName, bool isExtensionMethodFirstParameter)
  {
    var method = type.GetMethod(methodName);

    Assert.That(
      method!.GetParameters()[0].IsExtensionMethodFirstParameter(),
      Is.EqualTo(isExtensionMethodFirstParameter)
    );
    Assert.That(
      method.GetParameters()[1].IsExtensionMethodFirstParameter(),
      Is.False
    );
    Assert.That(
      method.ReturnParameter.IsExtensionMethodFirstParameter(),
      Is.False
    );
  }

  [TestCase]
  public void IsExtensionMethodFirstParameter_ArgumentNull()
    => Assert.That(
      () => ((ParameterInfo)null!).IsExtensionMethodFirstParameter(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("param")
    );

  class ParameterModifiers {
    public void None(int x) => throw new NotImplementedException();
    public void In(in int x) => throw new NotImplementedException();
    public void Out(out int x) => throw new NotImplementedException();
    public void Ref(ref int x) => throw new NotImplementedException();
    public void RefReadOnly(ref readonly ReadOnlySpan<int> x) => throw new NotImplementedException();
    public int RefVal() => throw new NotImplementedException();
    public ref int RefRetval() => throw new NotImplementedException();
    public ref readonly int RefReadOnlyRetval() => throw new NotImplementedException();
    public ref ReadOnlySpan<int> RefOfRefStructRetval() => throw new NotImplementedException();
    public ref readonly ReadOnlySpan<int> RefReadOnlyOfRefStructRetval() => throw new NotImplementedException();

    public void Scoped(scoped ReadOnlySpan<int> x) => throw new NotImplementedException();
    public void ScopedIn(scoped in int x) => throw new NotImplementedException();
    public void ScopedOut(scoped out int x) => throw new NotImplementedException();
    public void ScopedOutOfRefStruct(scoped out ReadOnlySpan<int> x) => throw new NotImplementedException();
    public void ScopedRef(scoped ref int x) => throw new NotImplementedException();
    public void ScopedRefReadOnly(scoped ref readonly ReadOnlySpan<int> x) => throw new NotImplementedException();

    public void ParamsArray(params int[] x) => throw new NotImplementedException();
    public void ParamsRefStruct(params ReadOnlySpan<int> x) => throw new NotImplementedException();
    public void ParamsScopedRefStruct(params scoped ReadOnlySpan<int> x) => throw new NotImplementedException();
  }

  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.None), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.In), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.Out), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.Ref), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.RefReadOnly), true)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.RefVal), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.RefRetval), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.RefReadOnlyRetval), true)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.RefOfRefStructRetval), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.RefReadOnlyOfRefStructRetval), true)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.Scoped), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ScopedIn), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ScopedOut), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ScopedOutOfRefStruct), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ScopedRef), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ScopedRefReadOnly), true)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ParamsArray), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ParamsRefStruct), false)]
  [TestCase(typeof(ParameterModifiers), nameof(ParameterModifiers.ParamsScopedRefStruct), false)]
  public void IsRefReadOnly(Type type, string methodName, bool isRefReadOnly)
  {
    var method = type.GetMethod(methodName);
    var parameter = method!.GetParameters().FirstOrDefault() ?? method.ReturnParameter;

    Assert.That(
      parameter.IsRefReadOnly(),
      Is.EqualTo(isRefReadOnly)
    );
  }

  [TestCase]
  public void IsRefReadOnly_ArgumentNull()
    => Assert.That(
      () => ((ParameterInfo)null!).IsRefReadOnly(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("param")
    );

  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MOneParam), false)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MTwoParam), false)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MParamsArrayOfInt), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MOneParamAndParamsArrayOfInt), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MTwoParamAndParamsArrayOfInt), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MStaticParamsArrayOfInt), true)]
#if NET8_0_OR_GREATER
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MParamsReadOnlySpanOfChar), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MOneParamAndParamsReadOnlySpanOfChar), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MTwoParamAndParamsReadOnlySpanOfChar), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MStaticParamsReadOnlySpanOfChar), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MParamsIReadOnlyListOfString), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MOneParamAndParamsIReadOnlyListOfString), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MTwoParamAndParamsIReadOnlyListOfString), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MStaticParamsIReadOnlyListOfString), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MParamsNonGenericArrayList), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MOneParamAndParamsNonGenericArrayList), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MTwoParamAndParamsNonGenericArrayList), true)]
  [TestCase(typeof(ParameterInfoExtensionsTestTypes.CParams), nameof(ParameterInfoExtensionsTestTypes.CParams.MStaticParamsNonGenericArrayList), true)]
#endif
  public void CanTakeArbitraryLengthOfArgs(Type type, string methodName, bool canTakeArbitraryLengthOfArgs)
  {
    var method = type.GetMethod(methodName);
    var parameters = method!.GetParameters();

    Assert.That(
      parameters.Last().CanTakeArbitraryLengthOfArgs(),
      Is.EqualTo(canTakeArbitraryLengthOfArgs)
    );

    for (var i = 0; i < parameters.Length - 1; i++) {
      Assert.That(
        parameters[i].CanTakeArbitraryLengthOfArgs(),
        Is.False,
        $"param #{i}"
      );
    }

    Assert.That(
      method.ReturnParameter.CanTakeArbitraryLengthOfArgs(),
      Is.False
    );
  }

  [TestCase]
  public void CanTakeArbitraryLengthOfArgs_ArgumentNull()
    => Assert.That(
      () => ((ParameterInfo)null!).CanTakeArbitraryLengthOfArgs(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("param")
    );
}
