// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public partial class TypeExtensionsTests {
  private delegate void D();

  [TestCase(typeof(D), true)]
  [TestCase(typeof(Guid), false)]
  [TestCase(typeof(System.Delegate), true)]
  [TestCase(typeof(System.MulticastDelegate), true)]
  public void IsDelegate(Type type, bool expected)
    => Assert.AreEqual(expected, type.IsDelegate());

  [Test]
  public void IsDelegate_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsDelegate());

  [TestCase(typeof(D), true)]
  [TestCase(typeof(Guid), false)]
  [TestCase(typeof(System.Delegate), false)]
  [TestCase(typeof(System.MulticastDelegate), false)]
  public void IsConcreteDelegate(Type type, bool expected)
    => Assert.AreEqual(expected, type.IsConcreteDelegate());

  [Test]
  public void IsConcreteDelegate_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsConcreteDelegate());

  enum E1 { }
  [Flags] enum E2 { }

  [TestCase(typeof(E1), false)]
  [TestCase(typeof(E2), true)]
  [TestCase(typeof(System.IO.FileAttributes), true)]
  [TestCase(typeof(System.DateTimeKind), false)]
  public void IsEnumFlags(Type type, bool expected)
    => Assert.AreEqual(expected, type.IsEnumFlags());

  [Test]
  public void IsEnumFlags_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsEnumFlags());

  struct SReadOnly1 {}
  readonly struct SReadOnly2 { }

  [TestCase(typeof(SReadOnly1), false)]
  [TestCase(typeof(SReadOnly2), true)]
  public void IsReadOnlyValueType(Type type, bool expected)
    => Assert.AreEqual(expected, type.IsReadOnlyValueType());

  [Test]
  public void IsReadOnlyValueType_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsReadOnlyValueType());

  struct SByRefLike1 { }
  ref struct SByRefLike2 { }

  [TestCase(typeof(SByRefLike1), false)]
  [TestCase(typeof(SByRefLike2), true)]
  public void IsByRefLikeValueType(Type type, bool expected)
    => Assert.AreEqual(expected, type.IsByRefLikeValueType());

  [Test]
  public void IsByRefLikeValueType_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsByRefLikeValueType());

  [TestCase(typeof(Action), new Type[] { }, typeof(void))]
  [TestCase(typeof(Action<int>), new[] { typeof(int) }, typeof(void))]
  [TestCase(typeof(Func<int, string>), new[] { typeof(int) }, typeof(string))]
  public void GetDelegateSignatureMethod(Type type, Type[] expectedParameterTypes, Type expectedReturnType)
  {
    var m = type.GetDelegateSignatureMethod();

    Assert.AreEqual(expectedReturnType, m!.ReturnType, "return type");
    CollectionAssert.AreEqual(expectedParameterTypes, m.GetParameters().Select(p => p.ParameterType), "parameter types");
  }

  [Test]
  public void GetDelegateSignatureMethod_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).GetDelegateSignatureMethod());

  struct STest1 {}

  struct STest2 : IDisposable {
    public void Dispose() => throw new NotImplementedException();
  }

  class CTestBase : ICloneable {
    public object Clone() => throw new NotImplementedException();
  }

  class CTest : CTestBase, IDisposable {
    void IDisposable.Dispose() => throw new NotImplementedException();
  }

  [TestCase(typeof(DayOfWeek), new Type[] { })]
  [TestCase(typeof(Action), new Type[] { })]
  [TestCase(typeof(STest1), new Type[] { })]
  [TestCase(typeof(STest2), new[] { typeof(IDisposable) })]
  [TestCase(typeof(CTestBase), new[] { typeof(ICloneable) })]
  [TestCase(typeof(CTest), new[] { typeof(CTestBase), typeof(IDisposable) })]
  public void GetExplicitBaseTypeAndInterfaces(Type type, Type[] expectedBaseTypes)
    => CollectionAssert.AreEquivalent(
      expectedBaseTypes,
      type.GetExplicitBaseTypeAndInterfaces()
    );

  [Test]
  public void GetExplicitBaseTypeAndInterfaces_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).GetExplicitBaseTypeAndInterfaces());

  [TestCase(typeof(int), new[] { "System" })]
  [TestCase(typeof(int*), new[] { "System" })]
  [TestCase(typeof(int[]), new[] { "System" })]
  [TestCase(typeof(int?), new[] { "System" })]
  [TestCase(typeof((int, int)), new[] { "System" })]
  [TestCase(typeof((int, System.IO.Stream)), new[] { "System", "System.IO" })]
  [TestCase(typeof(Action), new[] { "System" })]
  [TestCase(typeof(List<>), new[] { "System.Collections.Generic" })]
  [TestCase(typeof(List<int>), new[] { "System", "System.Collections.Generic" })]
  [TestCase(typeof(List<int?>), new[] { "System", "System.Collections.Generic" })]
  [TestCase(typeof(List<int[]>), new[] { "System", "System.Collections.Generic" })]
  [TestCase(typeof(List<KeyValuePair<int, int>>), new[] { "System", "System.Collections.Generic" })]
  public void GetNamespaces(Type type, string[] expected)
    => CollectionAssert.AreEquivalent(
      expected,
      type.GetNamespaces()
    );

  [TestCase(typeof(int), new string[] { })]
  [TestCase(typeof(int[]), new string[] { })]
  [TestCase(typeof(List<>), new[] { "System.Collections.Generic" })]
  [TestCase(typeof(List<int>), new[] { "System.Collections.Generic" })]
  public void GetNamespacesWithPrimitiveType(Type type, string[] expected)
    => CollectionAssert.AreEquivalent(
      expected,
      type.GetNamespaces(t => t == typeof(int))
    );

  [Test]
  public void GetNamespaces_ArgumentNull()
  {
    Type t = null!;

    Assert.Throws<ArgumentNullException>(() => t.GetNamespaces());
    Assert.Throws<ArgumentNullException>(() => t.GetNamespaces(static ty => true));
    Assert.Throws<ArgumentNullException>(() => typeof(object).GetNamespaces(isLanguagePrimitive: null!));
  }

  [TestCase(typeof(List<>), "List")]
  [TestCase(typeof(List<int>), "List")]
  [TestCase(typeof(List<KeyValuePair<int, int>>), "List")]
  [TestCase(typeof(Dictionary<,>), "Dictionary")]
  [TestCase(typeof(Dictionary<int, int>), "Dictionary")]
  [TestCase(typeof(Dictionary<,>.KeyCollection), "KeyCollection")]
  [TestCase(typeof(Dictionary<int, int>.KeyCollection), "KeyCollection")]
  public void GetGenericTypeName(Type type, string expected)
    => Assert.AreEqual(expected, type.GetGenericTypeName());

  [TestCase(typeof(void))]
  [TestCase(typeof(int))]
  [TestCase(typeof(System.Collections.IList))]
  public void GetGenericTypeName_NonGenericTypes(Type type)
    => Assert.Throws<ArgumentException>(() => type.GetGenericTypeName());

  [Test]
  public void GetGenericTypeName_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).GetGenericTypeName());
}
