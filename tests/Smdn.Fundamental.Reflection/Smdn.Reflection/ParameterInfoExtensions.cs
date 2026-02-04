// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
}
