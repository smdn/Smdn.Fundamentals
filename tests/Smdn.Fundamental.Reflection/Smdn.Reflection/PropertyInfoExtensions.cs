// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
  }

  public class CEx : CBase {
    public override int PVirtual0 { get; }
    public override int PVirtual1 { get => 0; }
    public override int PVirtual2 { get; }
    public override int PAbstract0 { get; }
    public override int PAbstract1 { get => 0; }
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

    Assert.AreEqual(expected, property!.IsStatic(), $"{type.Name}.{property!.Name}");
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

    Assert.AreEqual(expected, property!.IsSetMethodInitOnly(), $"{type.Name}.{property!.Name}");
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
      Assert.IsNotNull(property!.GetBackingField());
    else
      Assert.IsNull(property!.GetBackingField());
  }

  [Test]
  public void GetBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).GetBackingField());
}
