// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0067, CS0169, CS0649, CS8597, IDE0044, IDE0051

using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

partial class MemberInfoExtensions {
  [AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Field |
    AttributeTargets.Method |
    AttributeTargets.Property |
    AttributeTargets.Event,
    AllowMultiple = false
  )]
  public class HidingInheritedMemberAttribute : Attribute
  {
    public bool HidingNonPublic { get; set; }
  }

  public class CParent {
    /*
     * nested types
     */
    private class CPrivate { }
    protected class CProtected { }
    protected class CProtectedHiddenByChildClass { }
    protected class CProtectedHiddenByGrandchildClass { }
    internal class CInternal { }
    internal class CInternalHiddenByChildClass { }
    internal class CInternalHiddenByGrandchildClass { }
    public class CPublic { }
    public class CPublicHiddenByChildClass { }
    public class CPublicHiddenByGrandchildClass { }

    /*
     * fields
     */
    private int FPrivate;
    protected int FProtected;
    protected int FProtectedHiddenByChildClass;
    protected int FProtectedHiddenByGrandchildClass;
    internal int FInternal;
    internal int FInternalHiddenByChildClass;
    internal int FInternalHiddenByGrandchildClass;
    public int FPublic;
    public int FPublicHiddenByChildClass;
    public int FPublicHiddenByGrandchildClass;
    static protected int FStaticProtectedHiddenByChildClass;
    static public int FStaticPublicHiddenByChildClass;
    static public int FStaticPublicHiddenByGrandchildClass;

    /*
     * methods
     */
    private void MPrivate() => throw null;
    protected void MProtected() => throw null;
    protected virtual void MProtectedOverriddenByChildClass() => throw null;
    protected virtual void MProtectedOverriddenByGrandchildClass() => throw null;
    protected void MProtectedHiddenByChildClass() => throw null;
    protected void MProtectedHiddenByGrandchildClass() => throw null;
    internal void MInternal() => throw null;
    internal void MInternalHiddenByChildClass() => throw null;
    internal void MInternalHiddenByGrandchildClass() => throw null;
    public void MPublic() => throw null;
    public virtual void MPublicOverriddenByChildClass() => throw null;
    public virtual void MPublicOverriddenByGrandchildClass() => throw null;
    public void MPublicHiddenByChildClass() => throw null;
    public void MPublicHiddenByGrandchildClass() => throw null;
    static protected void MStaticProtectedHiddenByChildClass() => throw null;
    static public void MStaticPublicHiddenByChildClass() => throw null;
    static public void MStaticPublicHiddenByGrandchildClass() => throw null;

    public int MOverloads() => throw null;
    public void MOverloads(int x, int y) => throw null;

    /*
     * properties
     */
    private int PPrivate { get; set; }
    protected int PProtected { get; set; }
    protected virtual int PProtectedOverriddenByChildClass { get; set; }
    protected virtual int PProtectedOverriddenByGrandchildClass { get; set; }
    protected int PProtectedHiddenByChildClass { get; set; }
    protected int PProtectedHiddenByGrandchildClass { get; set; }
    internal int PInternal { get; set; }
    internal int PInternalHiddenByChildClass { get; set; }
    internal int PInternalHiddenByGrandchildClass { get; set; }
    public int PPublic { get; set; }
    public virtual int PPublicOverriddenByChildClass { get; set; }
    public virtual int PPublicOverriddenByGrandchildClass { get; set; }
    public int PPublicHiddenByChildClass { get; set; }
    public int PPublicHiddenByGrandchildClass { get; set; }
    static protected int PStaticProtectedHiddenByChildClass { get; set; }
    static public int PStaticPublicHiddenByChildClass { get; set; }
    static public int PStaticPublicHiddenByGrandchildClass { get; set; }

    /*
     * events
     */
    private event EventHandler EPrivate;
    protected event EventHandler EProtected;
    protected virtual event EventHandler EProtectedOverriddenByChildClass;
    protected virtual event EventHandler EProtectedOverriddenByGrandchildClass;
    protected event EventHandler EProtectedHiddenByChildClass;
    protected event EventHandler EProtectedHiddenByGrandchildClass;
    internal event EventHandler EInternal;
    internal event EventHandler EInternalHiddenByChildClass;
    internal event EventHandler EInternalHiddenByGrandchildClass;
    public event EventHandler EPublic;
    public virtual event EventHandler EPublicOverriddenByChildClass;
    public virtual event EventHandler EPublicOverriddenByGrandchildClass;
    public event EventHandler EPublicHiddenByChildClass;
    public event EventHandler EPublicHiddenByGrandchildClass;
    static protected event EventHandler EStaticProtectedHiddenByChildClass;
    static public event EventHandler EStaticPublicHiddenByChildClass;
    static public event EventHandler EStaticPublicHiddenByGrandchildClass;

    /*
     * members hidden by different member type
     */
    public class CHiddenByChildClassField { }
    public class CHiddenByChildClassMethod { }
    public class CHiddenByChildClassProperty { }
    public class CHiddenByChildClassEvent { }

    public int FHiddenByChildClassType;
    public int FHiddenByChildClassMethod;
    public int FHiddenByChildClassProperty;
    public int FHiddenByChildClassEvent;

    public void MHiddenByChildClassType() => throw null;
    public void MHiddenByChildClassField() => throw null;
    public void MHiddenByChildClassProperty() => throw null;
    public void MHiddenByChildClassEvent() => throw null;

    public int PHiddenByChildClassType { get; set; }
    public int PHiddenByChildClassField { get; set; }
    public int PHiddenByChildClassMethod { get; set; }
    public int PHiddenByChildClassEvent { get; set; }

    public event EventHandler EHiddenByChildClassType;
    public event EventHandler EHiddenByChildClassField;
    public event EventHandler EHiddenByChildClassMethod;
    public event EventHandler EHiddenByChildClassProperty;
  }

  public class CChild : CParent {
    /*
     * nested types
     */
    private class CPrivate { } // cannot hide nested private types
    [HidingInheritedMember(HidingNonPublic = true)] new protected class CProtectedHiddenByChildClass { }
    [HidingInheritedMember(HidingNonPublic = true)] new public class CInternalHiddenByChildClass { }
    [HidingInheritedMember(HidingNonPublic = false)] new private class CPublicHiddenByChildClass { }

    /*
     * fields
     */
    private int FPrivate;
    [HidingInheritedMember(HidingNonPublic = true)] new protected int FProtectedHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new public int FInternalHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new private int FPublicHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new static protected int FStaticProtectedHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new static public int FStaticPublicHiddenByChildClass;

    /*
     * methods
     */
    private void MPrivate() => throw null;
    protected override void MProtectedOverriddenByChildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = true)] new protected void MProtectedHiddenByChildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = true)] new public void MInternalHiddenByChildClass() => throw null;
    public override void MPublicOverriddenByChildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new public void MPublicHiddenByChildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = true)] new static protected void MStaticProtectedHiddenByChildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new static public void MStaticPublicHiddenByChildClass() => throw null;

    [HidingInheritedMember(HidingNonPublic = false)] new public void MOverloads(int x, int y) => throw null;
    public void MOverloads(string x, string y) => throw null;

    /*
     * properties
     */
    private int PPrivate { get; set; }
    protected override int PProtectedOverriddenByChildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = true)] new protected int PProtectedHiddenByChildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = true)] new public int PInternalHiddenByChildClass { get; set; }
    public override int PPublicOverriddenByChildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new public int PPublicHiddenByChildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = true)] new static protected int PStaticProtectedHiddenByChildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new static public int PStaticPublicHiddenByChildClass { get; set; }

    /*
     * events
     */
    private event EventHandler EPrivate;
    protected override event EventHandler EProtectedOverriddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new protected event EventHandler EProtectedHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new public event EventHandler EInternalHiddenByChildClass;
    public override event EventHandler EPublicOverriddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new public event EventHandler EPublicHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new static protected event EventHandler EStaticProtectedHiddenByChildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new static public event EventHandler EStaticPublicHiddenByChildClass;

    /*
     * members hidden by different member type
     */
    [HidingInheritedMember(HidingNonPublic = false)] new public int CHiddenByChildClassField;
    [HidingInheritedMember(HidingNonPublic = false)] new public void CHiddenByChildClassMethod() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new public int CHiddenByChildClassProperty { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new public event EventHandler CHiddenByChildClassEvent;

    [HidingInheritedMember(HidingNonPublic = false)] new public class FHiddenByChildClassType { }
    [HidingInheritedMember(HidingNonPublic = false)] new public void FHiddenByChildClassMethod() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new public int FHiddenByChildClassProperty { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new public event EventHandler FHiddenByChildClassEvent;

    [HidingInheritedMember(HidingNonPublic = false)] new public class MHiddenByChildClassType { }
    [HidingInheritedMember(HidingNonPublic = false)] new public int MHiddenByChildClassField;
    [HidingInheritedMember(HidingNonPublic = false)] new public int MHiddenByChildClassProperty { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new public event EventHandler MHiddenByChildClassEvent;

    [HidingInheritedMember(HidingNonPublic = false)] new public class PHiddenByChildClassType { }
    [HidingInheritedMember(HidingNonPublic = false)] new public int PHiddenByChildClassField;
    [HidingInheritedMember(HidingNonPublic = false)] new public void PHiddenByChildClassMethod() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new public event EventHandler PHiddenByChildClassEvent;

    [HidingInheritedMember(HidingNonPublic = false)] new public class EHiddenByChildClassType { }
    [HidingInheritedMember(HidingNonPublic = false)] new public int EHiddenByChildClassField;
    [HidingInheritedMember(HidingNonPublic = false)] new public void EHiddenByChildClassMethod() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new public int EHiddenByChildClassProperty { get; set; }
  }

  public class CGrandchild : CChild {
    /*
     * nested types
     */
    [HidingInheritedMember(HidingNonPublic = true)] new protected class CProtectedHiddenByGrandchildClass { }
    [HidingInheritedMember(HidingNonPublic = true)] new public class CInternalHiddenByGrandchildClass { }
    [HidingInheritedMember(HidingNonPublic = false)] new private class CPublicHiddenByGrandchildClass { }

    [HidingInheritedMember(HidingNonPublic = true)] new public class CProtectedHiddenByChildClass { } // redeclared in CLevel1 as protected
    [HidingInheritedMember(HidingNonPublic = false)] new public class CInternalHiddenByChildClass { } // redeclared in CLevel1 as public

    /*
     * fields
     */
    [HidingInheritedMember(HidingNonPublic = true)] new protected int FProtectedHiddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new public int FInternalHiddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new private int FPublicHiddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new static public int FStaticPublicHiddenByGrandchildClass;

    [HidingInheritedMember(HidingNonPublic = true)] new public int FProtectedHiddenByChildClass; // redeclared in CLevel1 as protected
    [HidingInheritedMember(HidingNonPublic = false)] new public int FInternalHiddenByChildClass; // redeclared in CLevel1 as public

    /*
     * methods
     */
    protected override void MProtectedOverriddenByGrandchildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = true)] new protected void MProtectedHiddenByGrandchildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = true)] new internal void MInternalHiddenByGrandchildClass() => throw null;
    public override void MPublicOverriddenByGrandchildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new public void MPublicHiddenByGrandchildClass() => throw null;
    [HidingInheritedMember(HidingNonPublic = false)] new static public void MStaticPublicHiddenByGrandchildClass() => throw null;

    [HidingInheritedMember(HidingNonPublic = true)] new public void MProtectedHiddenByChildClass() => throw null; // redeclared in CLevel1 as protected
    [HidingInheritedMember(HidingNonPublic = false)] new public virtual void MInternalHiddenByChildClass() => throw null; // redeclared in CLevel1 as public

    [HidingInheritedMember(HidingNonPublic = false)] new public int MOverloads() => throw null;
    public void MOverloads(int x, int y, int z) => throw null;

    /*
     * properties
     */
    protected override int PProtectedOverriddenByGrandchildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = true)] new protected int PProtectedHiddenByGrandchildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = true)] new internal int PInternalHiddenByGrandchildClass { get; set; }
    public override int PPublicOverriddenByGrandchildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new public int PPublicHiddenByGrandchildClass { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new static public int PStaticPublicHiddenByGrandchildClass { get; set; }

    [HidingInheritedMember(HidingNonPublic = true)] new public int PProtectedHiddenByChildClass { get; set; } // redeclared in CLevel1 as protected
    [HidingInheritedMember(HidingNonPublic = false)] new public virtual int PInternalHiddenByChildClass { get; set; } // redeclared in CLevel1 as public

    /*
     * events
     */
    protected override event EventHandler EProtectedOverriddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new protected event EventHandler EProtectedHiddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = true)] new internal event EventHandler EInternalHiddenByGrandchildClass;
    public override event EventHandler EPublicOverriddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new public event EventHandler EPublicHiddenByGrandchildClass;
    [HidingInheritedMember(HidingNonPublic = false)] new static public event EventHandler EStaticPublicHiddenByGrandchildClass;

    [HidingInheritedMember(HidingNonPublic = true)] new public event EventHandler EProtectedHiddenByChildClass; // redeclared in CLevel1 as protected
    [HidingInheritedMember(HidingNonPublic = false)] new public virtual event EventHandler EInternalHiddenByChildClass; // redeclared in CLevel1 as public
  }

  public interface IParent0 {
    void M();
    void M(int x);
#if NET7_0_OR_GREATER
    static sealed void MStatic() => throw null;
    static virtual void MStaticVirtualHiddenByChildInterface() => throw null;
    static virtual void MStaticVirtualOverriddenByImplementation() => throw null;

    static abstract int PStaticAbstractHiddenByChildInterface { get; set; }
    static abstract event EventHandler EStaticAbstractHiddenByChildInterface;
#endif
  }

  public interface IParent1 {
    void M();
    void M(string x);
  }

  public interface IChild : IParent0, IParent1 {
    [HidingInheritedMember(HidingNonPublic = false)] new void M(int x);
    void M(object x);

#if NET7_0_OR_GREATER
    [HidingInheritedMember(HidingNonPublic = false)] new static virtual void MStaticVirtualHiddenByChildInterface() => throw null;

    [HidingInheritedMember(HidingNonPublic = false)] new static abstract string PStaticAbstractHiddenByChildInterface { get; set; }
    [HidingInheritedMember(HidingNonPublic = false)] new static abstract event EventHandler<int> EStaticAbstractHiddenByChildInterface;
#endif
  }

  public class CImplementation : IChild {
    public void M(int x) => throw null;
    public void M(string x) => throw null;

    // explicitly implemented interface members are not `new`
    void IParent0.M() => throw null;
    void IParent1.M() => throw null;
    void IChild.M(object x) => throw null;

#if NET7_0_OR_GREATER
    static void MStatic() => throw null; // does not hide IParent0.MStatic()
    static void MStaticVirtualHiddenByChildInterface() => throw null;
    static void MStaticVirtualOverriddenByChildInterface() => throw null;

    // explicitly implemented interface members are not `new`
    static int IParent0.PStaticAbstractHiddenByChildInterface { get => throw null; set => throw null; }
    static event EventHandler IParent0.EStaticAbstractHiddenByChildInterface { add => throw null; remove => throw null; }

    public static string PStaticAbstractHiddenByChildInterface { get => throw null; set => throw null; }
    public static event EventHandler<int> EStaticAbstractHiddenByChildInterface;
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_IsHidingInheritedMember()
  {
    const bool publicOnly = false;
    const bool publicAndNonPublic = true;

    foreach (var testTargetType in new[] {
      typeof(CParent),
      typeof(CChild),
      typeof(CGrandchild),
      typeof(IParent0),
      typeof(IParent1),
      typeof(IChild)
    }) {
      yield return new object[] { publicAndNonPublic, testTargetType, false };
      yield return new object[] { publicOnly, testTargetType, false };

      foreach (var m in testTargetType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
        var attr = m.GetCustomAttribute<HidingInheritedMemberAttribute>();

        if (m is MethodInfo mayBeAccessor) {
          if (mayBeAccessor.IsPropertyAccessorMethod())
            continue;
          if (mayBeAccessor.IsEventAccessorMethod())
            continue;
        }
        else if (m is FieldInfo mayBeBackingField) {
          if (mayBeBackingField.IsEventBackingField())
            continue;
        }

        var isHiding = attr is not null;
        var isHidingNonPublic = attr is not null && attr.HidingNonPublic;

        yield return new object[] { publicAndNonPublic, m, isHiding };
        yield return new object[] { publicOnly, m, isHiding && !isHidingNonPublic };
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsHidingInheritedMember))]
  public void IsHidingInheritedMember(
    bool nonPublic,
    MemberInfo m,
    bool expected
  )
    => Assert.That(m.IsHidingInheritedMember(nonPublic), Is.EqualTo(expected), $"Type: {m.DeclaringType}, Member: {m}");

  [TestCase(true)]
  [TestCase(false)]
  public void IsHidingInheritedMember_ArgumentNull(bool nonPublic)
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsHidingInheritedMember(nonPublic));
}
