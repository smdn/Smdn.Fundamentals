// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS8597

using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class PropertyInfoExtensionsTests {
  class C {
    public int P0 { get; init; }
    public int P1 { get; set; }
    public int P2 { get; } = 0;
    public int P3 { get => 0; }

    public static int SP0 { get; } = 0;
    public static int SP1 { set { } }
    public static int SP2 { get; set; }
  }

  public abstract class CBase {
    public virtual int PVirtual0 { get; }
    public virtual int PVirtual1 { get; }
    public virtual int PVirtual2 { get => 0; }
    public abstract int PAbstract0 { get; }
    public abstract int PAbstract1 { get; }
    public virtual int PVirtualHidden { get; }
    public virtual int PVirtualInherited { get; }
    public virtual int PVirtualGet { get; }
    public virtual int PVirtualSet { set => throw null; }
    public virtual int PVirtualGetSet { get; set; }
  }

  public class CEx : CBase {
    public override int PVirtual0 { get; }
    public override int PVirtual1 { get => 0; }
    public override int PVirtual2 { get; }
    public override int PAbstract0 { get; }
    public override int PAbstract1 { get => 0; }
    public new int PVirtualHidden { get; }
    public override int PVirtualGet { get; }
    public override int PVirtualSet { set => throw null; }
    public override int PVirtualGetSet { get; set; }
  }

  static class SC {
    public static int SP0 { get; } = 0;
  }

  struct S {
    public int P0 { get; init; }
    public int P1 { get; set; }
    public int P2 { get => 0; }
    public readonly int P3 { get; init; }

    public static int SP0 { get; set; }
    public static int SP1 { get => 0; }
  }

  readonly struct ROS {
    public int P0 { get; init; }
    //public int P1 { get; set; }
    public int P2 { get => 0; }
  }

  [TestCase(typeof(C), nameof(C.P0), false)]
  [TestCase(typeof(C), nameof(C.P1), false)]
  [TestCase(typeof(C), nameof(C.P2), false)]
  [TestCase(typeof(C), nameof(C.SP0), true)]
  [TestCase(typeof(C), nameof(C.SP1), true)]
  [TestCase(typeof(C), nameof(C.SP2), true)]
  [TestCase(typeof(SC), nameof(SC.SP0), true)]
  public void IsStatic(Type type, string propertyName, bool expected)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(property!.IsStatic(), Is.EqualTo(expected), $"{type.Name}.{property!.Name}");
  }

  [Test]
  public void IsStatic_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).IsStatic());

  [TestCase(typeof(C), nameof(C.P0), true)]
  [TestCase(typeof(C), nameof(C.P1), false)]
  [TestCase(typeof(S), nameof(S.P0), true)]
  [TestCase(typeof(S), nameof(S.P1), false)]
  [TestCase(typeof(S), nameof(S.P3), true)]
  [TestCase(typeof(S), nameof(ROS.P0), true)]
  public void IsSetMethodInitOnly(Type type, string propertyName, bool expected)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    Assert.That(property!.IsSetMethodInitOnly(), Is.EqualTo(expected), $"{type.Name}.{property!.Name}");
  }

  [TestCase(typeof(C), nameof(C.P2))]
  [TestCase(typeof(S), nameof(S.P2))]
  [TestCase(typeof(ROS), nameof(ROS.P2))]
  public void IsSetMethodInitOnly_ReadOnly(Type type, string propertyName)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    Assert.Throws<InvalidOperationException>(() => property!.IsSetMethodInitOnly(), $"{type.Name}.{property!.Name}");
  }

  [Test]
  public void IsSetMethodInitOnly_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).IsSetMethodInitOnly());

  [TestCase(typeof(C), nameof(C.P0), true)]
  [TestCase(typeof(C), nameof(C.P1), true)]
  [TestCase(typeof(C), nameof(C.P2), true)]
  [TestCase(typeof(C), nameof(C.P3), false)]
  [TestCase(typeof(C), nameof(C.SP0), true)]
  [TestCase(typeof(C), nameof(C.SP1), false)]
  [TestCase(typeof(C), nameof(C.SP2), true)]
  [TestCase(typeof(S), nameof(S.P0), true)]
  [TestCase(typeof(S), nameof(S.P1), true)]
  [TestCase(typeof(S), nameof(S.P2), false)]
  [TestCase(typeof(S), nameof(S.P3), true)]
  [TestCase(typeof(S), nameof(S.SP0), true)]
  [TestCase(typeof(S), nameof(S.SP1), false)]
  [TestCase(typeof(CBase), nameof(CBase.PVirtual0), true)]
  [TestCase(typeof(CBase), nameof(CBase.PVirtual1), true)]
  [TestCase(typeof(CBase), nameof(CBase.PVirtual2), false)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtual0), true)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtual1), false)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtual2), true)]
  [TestCase(typeof(CBase), nameof(CBase.PAbstract0), false)]
  [TestCase(typeof(CBase), nameof(CBase.PAbstract1), false)]
  [TestCase(typeof(CEx), nameof(CEx.PAbstract0), true)]
  [TestCase(typeof(CEx), nameof(CEx.PAbstract1), false)]
  public void GetBackingField(Type type, string propertyName, bool hasBackingField)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    if (hasBackingField)
      Assert.That(property!.GetBackingField(), Is.Not.Null);
    else
      Assert.That(property!.GetBackingField(), Is.Null);
  }

  [Test]
  public void GetBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).GetBackingField());

  [TestCase(typeof(C), nameof(C.P0), false)]
  [TestCase(typeof(C), nameof(C.SP0), false)]
  [TestCase(typeof(S), nameof(S.P0), false)]
  [TestCase(typeof(S), nameof(S.SP0), false)]
  [TestCase(typeof(CBase), nameof(CBase.PAbstract0), false)]
  [TestCase(typeof(CBase), nameof(CBase.PVirtual0), false)]
  [TestCase(typeof(CBase), nameof(CBase.PVirtualHidden), false)]
  [TestCase(typeof(CEx), nameof(CEx.PAbstract0), true)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtual0), true)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtualHidden), false)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtualGet), true)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtualSet), true)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtualGetSet), true)]
  public void IsOverride(Type type, string propertyName, bool expected)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(property!.IsOverride(), Is.EqualTo(expected), $"{type.Name}.{property!.Name}");
  }

  [TestCase(typeof(System.IO.TextWriter), nameof(System.IO.TextWriter.NewLine), typeof(System.IO.TextWriter), false)]
  [TestCase(typeof(System.IO.StreamWriter), nameof(System.IO.StreamWriter.NewLine), typeof(System.IO.TextWriter), false)] // = TextWriter.NewLine
  [TestCase(typeof(CBase), nameof(CBase.PVirtualInherited), typeof(CBase), false)]
  [TestCase(typeof(CEx), nameof(CEx.PVirtualInherited), typeof(CBase), false)] // = CBase.PVirtualInherited
  public void IsOverride_IgnoreReflectedType(Type type, string propertyName, Type declaringType, bool expected)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(type, Is.EqualTo(property!.ReflectedType), nameof(property.ReflectedType));
    Assert.That(declaringType, Is.EqualTo(property!.DeclaringType), nameof(property.DeclaringType));
    Assert.That(property!.IsOverride(), Is.EqualTo(expected), $"{type.Name}.{property!.Name}");
  }

  [Test]
  public void IsOverride_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).IsOverride());

  // ref: https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/proposals/csharp-8.0/readonly-instance-members
  struct SReadOnlyProperty {
    public int PGetSet {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public int PGetOnly {
      get => throw new NotImplementedException();
    }

    public readonly int PGetOnly_ReadOnlyGetMethod {
      get => throw new NotImplementedException();
    }

    public int PGetSet_ReadOnlyGetMethod {
      readonly get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
  }

  [TestCase(typeof(SReadOnlyProperty), nameof(SReadOnlyProperty.PGetSet), false)]
  [TestCase(typeof(SReadOnlyProperty), nameof(SReadOnlyProperty.PGetOnly), false)]
  [TestCase(typeof(SReadOnlyProperty), nameof(SReadOnlyProperty.PGetOnly_ReadOnlyGetMethod), true)]
  [TestCase(typeof(SReadOnlyProperty), nameof(SReadOnlyProperty.PGetSet_ReadOnlyGetMethod), true)]
  public void IsAccessorReadOnly(Type type, string propertyName, bool expected)
  {
    var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(property!.IsAccessorReadOnly(), Is.EqualTo(expected), $"{type.Name}.{property!.Name}");
  }

  [Test]
  public void IsAccessorReadOnly_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).IsAccessorReadOnly());
}
