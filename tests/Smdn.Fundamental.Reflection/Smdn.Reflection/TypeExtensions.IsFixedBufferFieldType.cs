// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public partial class TypeExtensionsTests {
  public unsafe struct US0 {
    public fixed int FFixed4Int[4];
    public int FInt;
  }

  public unsafe struct US1 {
    public fixed byte FFixed1Byte[1];
    public fixed byte FFixed2Byte[2];
    public byte FByte;
  }

  [TestCase(typeof(US0))]
  [TestCase(typeof(US1))]
  public void IsFixedBufferFieldType(Type type)
  {
    foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) {
      Assert.That(nestedType.IsFixedBufferFieldType(), Is.True, $"type = {type}");
    }
  }

  [TestCase(typeof(US0), nameof(US0.FFixed4Int), true)]
  [TestCase(typeof(US0), nameof(US0.FInt), false)]
  [TestCase(typeof(US1), nameof(US1.FFixed1Byte), true)]
  [TestCase(typeof(US1), nameof(US1.FFixed2Byte), true)]
  [TestCase(typeof(US1), nameof(US1.FByte), false)]
  public void IsFixedBufferFieldType_FieldType(Type type, string fieldName, bool isFixedField)
    => Assert.That(
      type.GetField(fieldName)!.FieldType!.IsFixedBufferFieldType(),
      Is.EqualTo(isFixedField)
    );

  [TestCase(typeof(void))]
  [TestCase(typeof(int))]
  [TestCase(typeof(Enum))]
  [TestCase(typeof(DateTimeKind))]
  [TestCase(typeof(Guid))]
  [TestCase(typeof(Delegate))]
  [TestCase(typeof(IDisposable))]
  [TestCase(typeof(Dictionary<,>.Enumerator))]
  [TestCase(typeof(Dictionary<string, int>.Enumerator))]
  public void IsFixedBufferFieldType_False(Type type)
    => Assert.That(type.IsFixedBufferFieldType(), Is.False);

  [Test]
  public void IsFixedBufferFieldType_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).IsFixedBufferFieldType(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("t")
    );
}
