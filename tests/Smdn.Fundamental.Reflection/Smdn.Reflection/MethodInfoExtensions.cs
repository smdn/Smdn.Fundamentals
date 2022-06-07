// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class MethodInfoExtensionsTests {
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

    Assert.AreEqual(isOverridden, method.IsOverridden(), $"has override? {type}.{methodName}");
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

    Assert.AreEqual(isOverridden, getter.IsOverridden(), $"is overridden? {type}.{propertyName}");
  }
}

