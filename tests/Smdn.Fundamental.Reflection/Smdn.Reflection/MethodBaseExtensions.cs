// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class MethodBaseExtensionsTests {
  class C1 {
    public void M1() => throw new NotImplementedException();
    public int M2() => throw new NotImplementedException();
    public void M3(int p1) => throw new NotImplementedException();
    public int M4(int p1) => throw new NotImplementedException();
    public int M5(int p1, int p2) => throw new NotImplementedException();
  }

  [TestCase(typeof(C1), nameof(C1.M1), new[] { typeof(void) })]
  [TestCase(typeof(C1), nameof(C1.M2), new[] { typeof(int) })]
  [TestCase(typeof(C1), nameof(C1.M3), new[] { typeof(int), typeof(void) })]
  [TestCase(typeof(C1), nameof(C1.M4), new[] { typeof(int), typeof(int) })]
  [TestCase(typeof(C1), nameof(C1.M5), new[] { typeof(int), typeof(int), typeof(int) })]
  public void GetSignatureTypes(Type type, string methodName, Type[] expected)
    => Assert.That(type.GetMethod(methodName)!.GetSignatureTypes(), Is.EqualTo(expected).AsCollection);

  [Test]
  public void GetSignatureTypes_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodBase)null!).GetSignatureTypes());

  class C2 : ICloneable, IDisposable {
    static C2() {}
    public C2() {}

    public void M() => throw new NotImplementedException();
    public object Clone() => throw new NotImplementedException();
    void IDisposable.Dispose() => throw new NotImplementedException();
  }

  [TestCase(typeof(C2), ".ctor", null, null)]
  [TestCase(typeof(C2), ".cctor", null, null)]
  [TestCase(typeof(C2), nameof(C2.M), null, null)]
  [TestCase(typeof(C2), nameof(C2.Clone), null, null)]
  [TestCase(typeof(C2), "System.IDisposable.Dispose", typeof(IDisposable), nameof(IDisposable.Dispose))]
#if NET7_0_OR_GREATER
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    ".cctor",
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.M),
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.MStaticAbstract),
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.MStaticVirtual),
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CExplicitlyImplementedStaticInterfaceMembers),
    "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticAbstract),
    typeof(MethodBaseExtensionsTestTypes.IStaticMembers),
    nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticAbstract)
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CExplicitlyImplementedStaticInterfaceMembers),
    "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticVirtual),
    typeof(MethodBaseExtensionsTestTypes.IStaticMembers),
    nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticVirtual)
  )]
#endif
  public void FindExplicitInterfaceMethod(Type type, string methodName, Type expectedInterface, string expectedMethodName)
  {
    var method = type.GetMember(
      methodName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    ).First() as MethodBase;
    var expectedMethod = expectedInterface?.GetMethod(expectedMethodName);

    Assert.That(method!.FindExplicitInterfaceMethod(), Is.EqualTo(expectedMethod));
  }

  [Test]
  public void FindExplicitInterfaceMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodBase)null!).FindExplicitInterfaceMethod());

  [TestCase(typeof(C2), ".ctor", false, null, null)]
  [TestCase(typeof(C2), ".cctor", false, null, null)]
  [TestCase(typeof(C2), nameof(C2.M), false, null, null)]
  [TestCase(typeof(C2), nameof(C2.Clone), false, null, null)]
  [TestCase(typeof(C2), "System.IDisposable.Dispose", true, typeof(IDisposable), nameof(IDisposable.Dispose))]
#if NET7_0_OR_GREATER
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    ".cctor",
    false,
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.M),
    false,
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.MStaticAbstract),
    false,
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.MStaticVirtual),
    false,
    null,
    null
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CExplicitlyImplementedStaticInterfaceMembers),
    "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticAbstract),
    true,
    typeof(MethodBaseExtensionsTestTypes.IStaticMembers),
    nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticAbstract)
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CExplicitlyImplementedStaticInterfaceMembers),
    "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticVirtual),
    true,
    typeof(MethodBaseExtensionsTestTypes.IStaticMembers),
    nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticVirtual)
  )]
#endif
  public void TryFindExplicitInterfaceMethod(Type type, string methodName, bool expectedResult, Type expectedInterface, string expectedMethodName)
  {
    var method = type.GetMember(
      methodName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    ).First() as MethodBase;
    var expectedMethod = expectedInterface?.GetMethod(expectedMethodName);

    Assert.That(method!.TryFindExplicitInterfaceMethod(out var actualMethod), Is.EqualTo(expectedResult), "result");
    Assert.That(actualMethod, Is.EqualTo(expectedMethod), "actual method");
  }

  [Test]
  public void TryFindExplicitInterfaceMethod_ArgumentNull()
    => Assert.DoesNotThrow(() => Assert.That(((MethodBase)null!).TryFindExplicitInterfaceMethod(out _), Is.False));

  [TestCase(typeof(C2), ".ctor", false)]
  [TestCase(typeof(C2), ".cctor", false)]
  [TestCase(typeof(C2), nameof(C2.M), false)]
  [TestCase(typeof(C2), nameof(C2.Clone), false)]
  [TestCase(typeof(C2), "System.IDisposable.Dispose", true)]
#if NET7_0_OR_GREATER
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    ".cctor",
    false
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.M),
    false
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.MStaticAbstract),
    false
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers),
    nameof(MethodBaseExtensionsTestTypes.CImplicitlyImplementedStaticInterfaceMembers.MStaticVirtual),
    false
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CExplicitlyImplementedStaticInterfaceMembers),
    "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticAbstract),
    true
  )]
  [TestCase(
    typeof(MethodBaseExtensionsTestTypes.CExplicitlyImplementedStaticInterfaceMembers),
    "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembers.MStaticVirtual),
    true
  )]
#endif
  public void IsExplicitlyImplemented(Type type, string methodName, bool expected)
  {
    var method = type.GetMember(
      methodName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    ).First() as MethodBase;;

    Assert.That(method!.IsExplicitlyImplemented(), Is.EqualTo(expected));
  }

  [Test]
  public void IsExplicitlyImplemented_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodBase)null!).IsExplicitlyImplemented());

  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.I1.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.I2.M", null)]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.C1.I3.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.C1.I4.M", null)]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.C1.I5.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.C1.I6.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.C1.I7.M", null)]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.C1), "MethodBaseExtensionsTestTypes.C1.I8.M", null)]
#if NET7_0_OR_GREATER
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembersPublic) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembersPublic.M), "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.IStaticMembersInternal) + "." + nameof(MethodBaseExtensionsTestTypes.IStaticMembersInternal.M), null)]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.CStaticMembers) + ".IPublic.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.CStaticMembers) + ".IInternal.M", null)]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.CStaticMembers) + ".IProtected.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.CStaticMembers) + ".IProtectedInternal.M", "M")]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.CStaticMembers) + ".IPrivateProtected.M", null)]
  [TestCase(typeof(MethodBaseExtensionsTestTypes.CStaticMembers), "MethodBaseExtensionsTestTypes." + nameof(MethodBaseExtensionsTestTypes.CStaticMembers) + ".IPrivate.M", null)]
#endif
  public void FindExplicitInterfaceMethod_PublicInterfaceOnly(Type type, string methodName, string expectedMethodName)
  {
    var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    var explicitInterfaceMethod = method!.FindExplicitInterfaceMethod(findOnlyPublicInterfaces: true);

    Assert.That(explicitInterfaceMethod?.Name, Is.EqualTo(expectedMethodName), methodName);
  }

  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
  class ExpectedMethodSpecialNameAttribute : Attribute {
    public MethodSpecialName Expected { get; }

    public ExpectedMethodSpecialNameAttribute(MethodSpecialName expected)
    {
      Expected = expected;
    }
  }

  class SpecialMethods {
    public class V { }
    public class W { }

    public class C : IDisposable {
      [ExpectedMethodSpecialName(MethodSpecialName.Constructor)] public C() { }
      [ExpectedMethodSpecialName(MethodSpecialName.None)] ~C() { }

      [ExpectedMethodSpecialName(MethodSpecialName.None)] public void Deconstruct(out int x, out int y, out int z) => throw new NotImplementedException();

      [ExpectedMethodSpecialName(MethodSpecialName.None)] void M() => throw new NotImplementedException();
#pragma warning disable CA1816 // Change to call GC.SuppressFinalize(object)
      [ExpectedMethodSpecialName(MethodSpecialName.None)] void IDisposable.Dispose() => throw new NotImplementedException();
#pragma warning restore CA1816

      // unary operators
      [ExpectedMethodSpecialName(MethodSpecialName.UnaryPlus)] public static C operator +(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.UnaryNegation)] public static C operator -(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedUnaryNegation)] public static C operator checked -(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.LogicalNot)] public static C operator !(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.OnesComplement)] public static C operator ~(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.True)] public static bool operator true(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.False)] public static bool operator false(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Increment)] public static C operator ++(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedIncrement)] public static C operator checked ++(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Decrement)] public static C operator --(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedDecrement)] public static C operator checked --(C c) => throw new NotImplementedException();

      // binary operators
      [ExpectedMethodSpecialName(MethodSpecialName.Addition)] public static C operator +(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedAddition)] public static C operator checked +(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Subtraction)] public static C operator -(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedSubtraction)] public static C operator checked -(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Multiply)] public static C operator *(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedMultiply)] public static C operator checked *(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Division)] public static C operator /(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedDivision)] public static C operator checked /(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Modulus)] public static C operator %(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.BitwiseAnd)] public static C operator &(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.BitwiseOr)] public static C operator |(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.ExclusiveOr)] public static C operator ^(C x, C y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.RightShift)] public static C operator >>(C x, int y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.UnsignedRightShift)] public static C operator >>>(C x, int y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.LeftShift)] public static C operator <<(C x, int y) => throw new NotImplementedException();

      // type cast
      [ExpectedMethodSpecialName(MethodSpecialName.Explicit)] public static explicit operator C(V v) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedExplicit)] public static explicit operator checked C(V v) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Explicit)] public static explicit operator V(C c) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.CheckedExplicit)] public static explicit operator checked V(C c) => throw new NotImplementedException();

      [ExpectedMethodSpecialName(MethodSpecialName.Implicit)] public static implicit operator C(W w) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Implicit)] public static implicit operator W(C c) => throw new NotImplementedException();
    }

    public class P : IEquatable<P>, IComparable<P> {
      // comparison
      [ExpectedMethodSpecialName(MethodSpecialName.Equality)] public static bool operator ==(P x, P y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.Inequality)] public static bool operator !=(P x, P y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.LessThan)] public static bool operator <(P x, P y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.GreaterThan)] public static bool operator >(P x, P y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.LessThanOrEqual)] public static bool operator <=(P x, P y) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.GreaterThanOrEqual)] public static bool operator >=(P x, P y) => throw new NotImplementedException();

      [ExpectedMethodSpecialName(MethodSpecialName.None)] public bool Equals(P other) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.None)] public int CompareTo(P other) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.None)] public override bool Equals(object obj) => throw new NotImplementedException();
      [ExpectedMethodSpecialName(MethodSpecialName.None)] public override int GetHashCode() => throw new NotImplementedException();
    }
  }

  [TestCase(typeof(SpecialMethods.C))]
  [TestCase(typeof(SpecialMethods.P))]
  public void GetNameType(Type type)
  {
    foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
      if (member is MethodBase method) {
        var attr = method.GetCustomAttribute<ExpectedMethodSpecialNameAttribute>();

        if (attr == null)
          continue;

        var expected = method!.GetCustomAttribute<ExpectedMethodSpecialNameAttribute>()!.Expected;

        Assert.That(method.GetNameType(), Is.EqualTo(expected), $"{type.FullName} : {method.Name}");
      }
    }
  }

  [Test]
  public void GetNameType_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((MethodBase)null!).GetNameType());
}
