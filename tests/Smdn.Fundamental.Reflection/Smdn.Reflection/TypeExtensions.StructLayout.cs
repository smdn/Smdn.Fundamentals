// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Smdn.Reflection;

partial class TypeExtensionsTests {
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.S0), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.S1), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.S2), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SLayoutKindAuto), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SLayoutKindSequential), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SLayoutKindExplicit), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SPack0), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SPack1), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SPack2), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SPack4), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SCharSetNotSpecified), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SCharSetAuto), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SCharSetAnsi), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SCharSetUnicode), false)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SCharSetNone), true)]
  public void IsStructLayoutDefault(Type type, bool expected)
    => Assert.That(type.IsStructLayoutDefault(), Is.EqualTo(expected), type.FullName);

  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SSizeNotSpecified), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SSize1), true)]
  [TestCase(typeof(TypeExtensionsStructLayoutTestTypes.SSize2), true)]
  public void IsStructLayoutDefault_SizeMustNotBetConsidered(Type type, bool expected)
    => Assert.That(type.IsStructLayoutDefault(), Is.EqualTo(expected), type.FullName);

  [Test]
  public void IsStructLayoutDefault_ArgumentNull()
  {
    Type type = null;

    Assert.Throws<ArgumentNullException>(() => type.IsStructLayoutDefault());
  }

  [TestCase(typeof(ValueType))]
  [TestCase(typeof(object))]
  [TestCase(typeof(Func<>))]
  [TestCase(typeof(LayoutKind))]
  public void IsStructLayoutDefault_InvalidType(Type type)
    => Assert.Throws<ArgumentException>(() => type.IsStructLayoutDefault(), type.FullName);
}
