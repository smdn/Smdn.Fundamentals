// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0649, CS0067, CS8597

using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class FieldInfoExtensionsTests {
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  private class PropertyBackingFieldAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  private class EventBackingFieldAttribute : Attribute { }

  abstract class CAbstract {
    public int F0;

    protected abstract int PAbstract { get; set; }
    [field: PropertyBackingField] public virtual int PVirtual { get; set; }

    protected abstract event EventHandler EAbstract;
    [field: EventBackingField] public virtual event EventHandler EVirtual;
  }

  class COverride : CAbstract {
    public int F1;

    [field: PropertyBackingField] protected override int PAbstract { get; set; }
    [field: PropertyBackingField] public override int PVirtual { get; set; }

    [field: EventBackingField] protected override event EventHandler EAbstract;
    [field: EventBackingField] public override event EventHandler EVirtual;
  }

  class C {
    public int F0;
    public static int FS0;

    [field: PropertyBackingField] private int P0 { get; set; }
    public int P1 { get => 0; set => throw null; }
    public int P2 { get => 0; }
    [field: PropertyBackingField] public int P3 { get; } = 0;
    public int P4 { set => throw null; }

    [field: PropertyBackingField] private static int PS0 { get; set; }
    public static int PS1 { get => 0; set => throw null; }
    public static int PS2 { get => 0; }
    [field: PropertyBackingField] public static int PS3 { get; } = 0;
    public static int PS4 { set => throw null; }

    [field: EventBackingField] public event EventHandler E0;
    [field: EventBackingField] private event EventHandler E1;
    public event EventHandler E2 { add => throw null; remove => throw null; }

    [field: EventBackingField] public static event EventHandler ES0;
    [field: EventBackingField] private static event EventHandler ES1;
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

  private static System.Collections.IEnumerable YieldTestCases_IsPropertyBackingField()
  {
    foreach (var field in GetTestTargetFields()) {
      yield return new object[] { field, field.IsDefined(typeof(PropertyBackingFieldAttribute)) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsPropertyBackingField))]
  public void IsPropertyBackingField(FieldInfo f, bool isBackingField)
  {
    if (isBackingField)
      Assert.IsTrue(f.IsPropertyBackingField());
    else
      Assert.IsFalse(f.IsPropertyBackingField());
  }

  [TestCase]
  public void IsPropertyBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsPropertyBackingField());

  private static System.Collections.IEnumerable YieldTestCases_IsEventBackingField()
  {
    foreach (var field in GetTestTargetFields()) {
      yield return new object[] { field, field.IsDefined(typeof(EventBackingFieldAttribute)) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsEventBackingField))]
  public void IsEventBackingField(FieldInfo f, bool isBackingField)
  {
    if (isBackingField)
      Assert.IsTrue(f.IsEventBackingField());
    else
      Assert.IsFalse(f.IsEventBackingField());
  }

  [TestCase]
  public void IsEventBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsEventBackingField());
}
