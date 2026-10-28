// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS8597

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

partial class MethodInfoExtensionsTests {
  public enum AccessorType {
    PropertyGet,
    PropertySet,
    EventAdd,
    EventRemove,
  }

  public abstract class AccessorAttribute : Attribute {
    public string DeclaringMemberName { get; }
    public AccessorType AccessorType { get; }

    protected AccessorAttribute(string declaringMemberName, AccessorType accessorType)
    {
      DeclaringMemberName = declaringMemberName;
      AccessorType = accessorType;
    }

    protected MemberInfo GetDeclaringMember(MethodInfo accessor)
    {
      var type = accessor.DeclaringType!;

      return type.GetMember(
        name: DeclaringMemberName,
        bindingAttr: BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
      ).FirstOrDefault();
    }
  }

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class PropertyAccessorAttribute : AccessorAttribute
  {
    public PropertyAccessorAttribute(string declaringMemberName, AccessorType accessorType)
      :  base(declaringMemberName, accessorType)
    {
    }

    public PropertyInfo GetDeclaringProperty(MethodInfo accessor)
      => GetDeclaringMember(accessor) as PropertyInfo;
  }

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class EventAccessorAttribute : AccessorAttribute
  {
    public EventAccessorAttribute(string declaringMemberName, AccessorType accessorType)
      :  base(declaringMemberName, accessorType)
    {
    }

    public EventInfo GetDeclaringEvent(MethodInfo accessor)
      => GetDeclaringMember(accessor) as EventInfo;
  }

  private class CAccessors {
    public void M0() => throw new NotImplementedException();
    public static void M1() => throw new NotImplementedException();

    public virtual int P0 {
      [PropertyAccessor(nameof(P0), AccessorType.PropertyGet)] get;
      [PropertyAccessor(nameof(P0), AccessorType.PropertySet)] set;
    }
    public int P1 {
      [PropertyAccessor(nameof(P1), AccessorType.PropertyGet)] private get;
      [PropertyAccessor(nameof(P1), AccessorType.PropertySet)] set;
    }
    public int P2 {
      [PropertyAccessor(nameof(P2), AccessorType.PropertyGet)] get;
      [PropertyAccessor(nameof(P2), AccessorType.PropertySet)] private set;
    }
    private int P3 {
      [PropertyAccessor(nameof(P3), AccessorType.PropertyGet)] get;
      [PropertyAccessor(nameof(P3), AccessorType.PropertySet)] set;
    }
    protected internal static int P4 {
      [PropertyAccessor(nameof(P4), AccessorType.PropertyGet)] get;
      [PropertyAccessor(nameof(P4), AccessorType.PropertySet)] private set;
    }

    public virtual event EventHandler E0 {
      [EventAccessor(nameof(E0), AccessorType.EventAdd)] add => throw null;
      [EventAccessor(nameof(E0), AccessorType.EventRemove)] remove => throw null;
    }
    private event EventHandler E1 {
      [EventAccessor(nameof(E1), AccessorType.EventAdd)] add => throw null;
      [EventAccessor(nameof(E1), AccessorType.EventRemove)] remove => throw null;
    }
    public static event EventHandler E2 {
      [EventAccessor(nameof(E2), AccessorType.EventAdd)] add => throw null;
      [EventAccessor(nameof(E2), AccessorType.EventRemove)] remove => throw null;
    }
  }

  private class CAccessorsOverridingBaseAccessor : CAccessors {
    public override int P0 {
      [PropertyAccessor(nameof(P0), AccessorType.PropertyGet)] get;
      [PropertyAccessor(nameof(P0), AccessorType.PropertySet)] set;
    }

    public override event EventHandler E0 {
      [EventAccessor(nameof(E0), AccessorType.EventAdd)] add => throw null;
      [EventAccessor(nameof(E0), AccessorType.EventRemove)] remove => throw null;
    }
  }

  private class CAccessorsHidingBaseAccessor : CAccessors {
    public new int P0 {
      [PropertyAccessor(nameof(P0), AccessorType.PropertyGet)] get;
      [PropertyAccessor(nameof(P0), AccessorType.PropertySet)] set;
    }

    public new event EventHandler E0 {
      [EventAccessor(nameof(E0), AccessorType.EventAdd)] add => throw null;
      [EventAccessor(nameof(E0), AccessorType.EventRemove)] remove => throw null;
    }
  }

  private static IEnumerable<MethodInfo> GetMethods()
    => new[] {
      typeof(CAccessors),
      typeof(CAccessorsOverridingBaseAccessor),
      typeof(CAccessorsHidingBaseAccessor),
    }.SelectMany(static t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly));

  private static System.Collections.IEnumerable YieldTestCases_PropertyAccessors()
  {
    foreach (var method in GetMethods()) {
      yield return new object[] { method, method.GetCustomAttribute<PropertyAccessorAttribute>() };
    }
  }

  [TestCaseSourceAttribute(nameof(YieldTestCases_PropertyAccessors))]
  public void IsPropertyGetMethod(MethodInfo method, PropertyAccessorAttribute attr)
  {
    if (attr is null || attr.AccessorType != AccessorType.PropertyGet)
      Assert.That(method.IsPropertyGetMethod(), Is.False);
    else
      Assert.That(method.IsPropertyGetMethod(), Is.True);
  }

  [Test]
  public void IsPropertyGetMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsPropertyGetMethod());

  [TestCaseSourceAttribute(nameof(YieldTestCases_PropertyAccessors))]
  public void IsPropertySetMethod(MethodInfo method, PropertyAccessorAttribute attr)
  {
    if (attr is null || attr.AccessorType != AccessorType.PropertySet)
      Assert.That(method.IsPropertySetMethod(), Is.False);
    else
      Assert.That(method.IsPropertySetMethod(), Is.True);
  }

  [Test]
  public void IsPropertySetMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsPropertySetMethod());

  [TestCaseSourceAttribute(nameof(YieldTestCases_PropertyAccessors))]
  public void IsPropertyAccessorMethod(MethodInfo method, PropertyAccessorAttribute attr)
  {
    if (attr is null)
      Assert.That(method.IsPropertyAccessorMethod(), Is.False);
    else
      Assert.That(method.IsPropertyAccessorMethod(), Is.True);
  }

  [Test]
  public void IsPropertyAccessorMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsPropertyAccessorMethod());

  [TestCaseSourceAttribute(nameof(YieldTestCases_PropertyAccessors))]
  public void TryGetPropertyFromAccessorMethod(MethodInfo method, PropertyAccessorAttribute attr)
  {
    if (attr is null) {
      Assert.That(method.TryGetPropertyFromAccessorMethod(out var property), Is.False);
      Assert.That(property, Is.Null);
    }
    else {
      Assert.That(method.TryGetPropertyFromAccessorMethod(out var property), Is.True);
      Assert.That(property, Is.Not.Null);
      Assert.That(property, Is.EqualTo(attr.GetDeclaringProperty(method)));
    }
  }

  [Test]
  public void TryGetPropertyFromAccessorMethod_ArgumentNull()
  {
    Assert.That(((MethodInfo)null!).TryGetPropertyFromAccessorMethod(out var p), Is.False);
    Assert.That(p, Is.Null);
  }

  private static System.Collections.IEnumerable YieldTestCases_EventAccessors()
  {
    foreach (var method in GetMethods()) {
      yield return new object[] { method, method.GetCustomAttribute<EventAccessorAttribute>() };
    }
  }

  [TestCaseSourceAttribute(nameof(YieldTestCases_EventAccessors))]
  public void IsEventAddMethod(MethodInfo method, EventAccessorAttribute attr)
  {
    if (attr is null || attr.AccessorType != AccessorType.EventAdd)
      Assert.That(method.IsEventAddMethod(), Is.False);
    else
      Assert.That(method.IsEventAddMethod(), Is.True);
  }

  [Test]
  public void IsEventAddMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsEventAddMethod());

  [TestCaseSourceAttribute(nameof(YieldTestCases_EventAccessors))]
  public void IsEventRemoveMethod(MethodInfo method, EventAccessorAttribute attr)
  {
    if (attr is null || attr.AccessorType != AccessorType.EventRemove)
      Assert.That(method.IsEventRemoveMethod(), Is.False);
    else
      Assert.That(method.IsEventRemoveMethod(), Is.True);
  }

  [Test]
  public void IsEventRemoveMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsEventRemoveMethod());

  [TestCaseSourceAttribute(nameof(YieldTestCases_EventAccessors))]
  public void IsEventAccessorMethod(MethodInfo method, EventAccessorAttribute attr)
  {
    if (attr is null)
      Assert.That(method.IsEventAccessorMethod(), Is.False);
    else
      Assert.That(method.IsEventAccessorMethod(), Is.True);
  }

  [Test]
  public void IsEventAccessorMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodInfo)null!).IsEventAccessorMethod());

  [TestCaseSourceAttribute(nameof(YieldTestCases_EventAccessors))]
  public void TryGetEventFromAccessorMethod(MethodInfo method, EventAccessorAttribute attr)
  {
    if (attr is null) {
      Assert.That(method.TryGetEventFromAccessorMethod(out var ev), Is.False);
      Assert.That(ev, Is.Null);
    }
    else {
      Assert.That(method.TryGetEventFromAccessorMethod(out var ev), Is.True);
      Assert.That(ev, Is.Not.Null);
      Assert.That(ev, Is.EqualTo(attr.GetDeclaringEvent(method)));
    }
  }

  [Test]
  public void TryGetEventFromAccessorMethod_ArgumentNull()
  {
    Assert.That(((MethodInfo)null!).TryGetEventFromAccessorMethod(out var ev), Is.False);
    Assert.That(ev, Is.Null);
  }
}
