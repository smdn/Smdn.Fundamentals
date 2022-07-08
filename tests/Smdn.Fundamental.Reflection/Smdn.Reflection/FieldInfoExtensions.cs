// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0649, CS0067, CS8597

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class FieldInfoExtensionsTests {
  public class BackingFieldAttribute : Attribute {
    public string DeclaringMemberName { get; }

    protected BackingFieldAttribute(string declaringMemberName)
    {
      DeclaringMemberName = declaringMemberName;
    }

    protected MemberInfo GetDeclaringMember(FieldInfo backingField)
    {
      var type = backingField.DeclaringType!;

      return type.GetMember(
        name: DeclaringMemberName,
        bindingAttr: BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
      ).FirstOrDefault();
    }
  }

  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class PropertyBackingFieldAttribute : BackingFieldAttribute {
    public PropertyBackingFieldAttribute(string declaringMemberName)
      : base(declaringMemberName) { }

    public PropertyInfo GetDeclaringProperty(FieldInfo backingField)
      => GetDeclaringMember(backingField) as PropertyInfo;
  }

  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class EventBackingFieldAttribute : BackingFieldAttribute {
    public EventBackingFieldAttribute(string declaringMemberName)
      : base(declaringMemberName) { }

    public EventInfo GetDeclaringEvent(FieldInfo backingField)
      => GetDeclaringMember(backingField) as EventInfo;
  }

  abstract class CAbstract {
    public int F0;

    protected abstract int PAbstract { get; set; }
    [field: PropertyBackingField(nameof(PVirtual))] public virtual int PVirtual { get; set; }

    protected abstract event EventHandler EAbstract;
    [field: EventBackingField(nameof(EVirtual))] public virtual event EventHandler EVirtual;
  }

  class COverride : CAbstract {
    public int F1;

    [field: PropertyBackingField(nameof(PAbstract))] protected override int PAbstract { get; set; }
    [field: PropertyBackingField(nameof(PVirtual))] public override int PVirtual { get; set; }

    [field: EventBackingField(nameof(EAbstract))] protected override event EventHandler EAbstract;
    [field: EventBackingField(nameof(EVirtual))] public override event EventHandler EVirtual;
  }

  class C {
    public int F0;
    public static int FS0;

    [field: PropertyBackingField(nameof(P0))] private int P0 { get; set; }
    public int P1 { get => 0; set => throw null; }
    public int P2 { get => 0; }
    [field: PropertyBackingField(nameof(P3))] public int P3 { get; } = 0;
    public int P4 { set => throw null; }

    [field: PropertyBackingField(nameof(PS0))] private static int PS0 { get; set; }
    public static int PS1 { get => 0; set => throw null; }
    public static int PS2 { get => 0; }
    [field: PropertyBackingField(nameof(PS3))] public static int PS3 { get; } = 0;
    public static int PS4 { set => throw null; }

    [field: EventBackingField(nameof(E0))] public event EventHandler E0;
    [field: EventBackingField(nameof(E1))] private event EventHandler E1;
    public event EventHandler E2 { add => throw null; remove => throw null; }

    [field: EventBackingField(nameof(ES0))] public static event EventHandler ES0;
    [field: EventBackingField(nameof(ES1))] private static event EventHandler ES1;
    public static event EventHandler ES2 { add => throw null; remove => throw null; }
  }

  private static IEnumerable<FieldInfo> GetTestTargetFields()
  {
    foreach (var type in new[] {
      typeof(CAbstract),
      typeof(COverride),
      typeof(C),
    }) {
      foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)) {
        yield return field;
      }
    }
  }

  private static System.Collections.IEnumerable YieldTestCases_PropertyBackingField()
  {
    foreach (var field in GetTestTargetFields()) {
      yield return new object[] { field, field.GetCustomAttribute<PropertyBackingFieldAttribute>() };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_PropertyBackingField))]
  public void IsPropertyBackingField(FieldInfo f, PropertyBackingFieldAttribute attr)
  {
    if (attr is null)
      Assert.IsFalse(f.IsPropertyBackingField());
    else
      Assert.IsTrue(f.IsPropertyBackingField());
  }

  [TestCase]
  public void IsPropertyBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsPropertyBackingField());

  [TestCaseSource(nameof(YieldTestCases_PropertyBackingField))]
  public void TryGetPropertyFromBackingField(FieldInfo f, PropertyBackingFieldAttribute attr)
  {
    if (attr is null) {
      Assert.IsFalse(f.TryGetPropertyFromBackingField(out var property));
      Assert.IsNull(property);
    }
    else {
      Assert.IsTrue(f.TryGetPropertyFromBackingField(out var property));
      Assert.IsNotNull(property);
      Assert.AreEqual(attr.GetDeclaringProperty(f), property);
    }
  }

  [TestCase]
  public void TryGetPropertyFromBackingField_ArgumentNull()
    => Assert.IsFalse(((FieldInfo)null!).TryGetPropertyFromBackingField(out _));

  private static System.Collections.IEnumerable YieldTestCases_EventBackingField()
  {
    foreach (var field in GetTestTargetFields()) {
      yield return new object[] { field, field.GetCustomAttribute<EventBackingFieldAttribute>() };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_EventBackingField))]
  public void IsEventBackingField(FieldInfo f, EventBackingFieldAttribute attr)
  {
    if (attr is null)
      Assert.IsFalse(f.IsEventBackingField());
    else
      Assert.IsTrue(f.IsEventBackingField());
  }

  [TestCase]
  public void IsEventBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsEventBackingField());

  [TestCaseSource(nameof(YieldTestCases_EventBackingField))]
  public void TryGetEventFromBackingField(FieldInfo f, EventBackingFieldAttribute attr)
  {
    if (attr is null) {
      Assert.IsFalse(f.TryGetEventFromBackingField(out var ev));
      Assert.IsNull(ev);
    }
    else {
      Assert.IsTrue(f.TryGetEventFromBackingField(out var ev));
      Assert.IsNotNull(ev);
      Assert.AreEqual(attr.GetDeclaringEvent(f), ev);
    }
  }

  [TestCase]
  public void TryGetEventFromBackingField_ArgumentNull()
    => Assert.IsFalse(((FieldInfo)null!).TryGetEventFromBackingField(out _));
}
