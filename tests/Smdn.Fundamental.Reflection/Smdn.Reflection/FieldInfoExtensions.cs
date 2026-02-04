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

    public readonly int FReadOnly = 1;
    public const int FConst = 1;
    public static readonly int FStaticReadOnly = 1;

#if SYSTEM_RUNTIME_COMPILERSERVICES_REQUIREDMEMBERATTRIBUTE
    public required int FRequired;
#endif

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

  struct S(int f) {
    public int F = f;
    public static int FStatic;

    public const int FConst = 1;
    public readonly int FReadOnly = 1;
    public static readonly int FStaticReadOnly = 1;

#if SYSTEM_RUNTIME_COMPILERSERVICES_REQUIREDMEMBERATTRIBUTE
    public required int FRequired;
#endif
  }

#if NET
  ref struct SRef {
#pragma warning disable CS9265
    public ref int FRef;
    public ref readonly int FRefReadOnly;
#pragma warning restore CS9265
  }

  readonly ref struct SReadOnlyRef {
#pragma warning disable CS9265
    public readonly ref int FReadOnlyRef;
    public readonly ref readonly int FReadOnlyRefReadOnly;
#pragma warning restore CS9265
  }
#endif

  unsafe struct SFixedSizeBuffers {
    public int Int;
    public fixed int Fixed4Int[4];
    public fixed byte Fixed1Byte[1];
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
      Assert.That(f.IsPropertyBackingField(), Is.False);
    else
      Assert.That(f.IsPropertyBackingField(), Is.True);
  }

  [TestCase]
  public void IsPropertyBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsPropertyBackingField());

  [TestCaseSource(nameof(YieldTestCases_PropertyBackingField))]
  public void TryGetPropertyFromBackingField(FieldInfo f, PropertyBackingFieldAttribute attr)
  {
    if (attr is null) {
      Assert.That(f.TryGetPropertyFromBackingField(out var property), Is.False);
      Assert.That(property, Is.Null);
    }
    else {
      Assert.That(f.TryGetPropertyFromBackingField(out var property), Is.True);
      Assert.That(property, Is.Not.Null);
      Assert.That(property, Is.EqualTo(attr.GetDeclaringProperty(f)));
    }
  }

  [TestCase]
  public void TryGetPropertyFromBackingField_ArgumentNull()
    => Assert.That(((FieldInfo)null!).TryGetPropertyFromBackingField(out _), Is.False);

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
      Assert.That(f.IsEventBackingField(), Is.False);
    else
      Assert.That(f.IsEventBackingField(), Is.True);
  }

  [TestCase]
  public void IsEventBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsEventBackingField());

  [TestCaseSource(nameof(YieldTestCases_EventBackingField))]
  public void TryGetEventFromBackingField(FieldInfo f, EventBackingFieldAttribute attr)
  {
    if (attr is null) {
      Assert.That(f.TryGetEventFromBackingField(out var ev), Is.False);
      Assert.That(ev, Is.Null);
    }
    else {
      Assert.That(f.TryGetEventFromBackingField(out var ev), Is.True);
      Assert.That(ev, Is.Not.Null);
      Assert.That(ev, Is.EqualTo(attr.GetDeclaringEvent(f)));
    }
  }

  [TestCase]
  public void TryGetEventFromBackingField_ArgumentNull()
    => Assert.That(((FieldInfo)null!).TryGetEventFromBackingField(out _), Is.False);

  private struct NonRefFieldsReadOnlyModifier {
    public readonly int ReadOnly;
  }

#if NET7_0_OR_GREATER
#pragma warning disable CS9265
  private ref struct RefFieldsReadOnlyModifier {
    public ref int Ref;
    public ref readonly int RefReadOnly;
  }

  private readonly ref struct RefReadOnlyFieldsReadOnlyModifier {
    public readonly ref int ReadOnlyRef;
    public readonly ref readonly int ReadOnlyRefReadOnly;
  }
#pragma warning restore CS9265
#endif

  [TestCase(typeof(NonRefFieldsReadOnlyModifier), nameof(NonRefFieldsReadOnlyModifier.ReadOnly), false)]
#if NET7_0_OR_GREATER
  [TestCase(typeof(RefFieldsReadOnlyModifier), nameof(RefFieldsReadOnlyModifier.Ref), false)]
  [TestCase(typeof(RefFieldsReadOnlyModifier), nameof(RefFieldsReadOnlyModifier.RefReadOnly), true)]
  [TestCase(typeof(RefReadOnlyFieldsReadOnlyModifier), nameof(RefReadOnlyFieldsReadOnlyModifier.ReadOnlyRef), false)]
  [TestCase(typeof(RefReadOnlyFieldsReadOnlyModifier), nameof(RefReadOnlyFieldsReadOnlyModifier.ReadOnlyRefReadOnly), true)]
#endif
  public void IsReadOnly(Type t, string fieldName, bool expected)
  {
    var f = t.GetField(
      fieldName,
      BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
    )!;

    Assert.That(f.IsReadOnly(), Is.EqualTo(expected));
  }

  [TestCase]
  public void IsReadOnly_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((FieldInfo)null!).IsReadOnly());

#if SYSTEM_RUNTIME_COMPILERSERVICES_REQUIREDMEMBERATTRIBUTE
  [TestCase(typeof(C), nameof(C.FRequired), true)]
  [TestCase(typeof(S), nameof(S.FRequired), true)]
#endif
  [TestCase(typeof(C), nameof(C.F0), false)]
  [TestCase(typeof(C), nameof(C.FS0), false)]
  [TestCase(typeof(C), nameof(C.FReadOnly), false)]
  [TestCase(typeof(C), nameof(C.FConst), false)]
  [TestCase(typeof(C), nameof(C.FStaticReadOnly), false)]
  [TestCase(typeof(S), nameof(S.F), false)]
  [TestCase(typeof(S), nameof(S.FStatic), false)]
  [TestCase(typeof(S), nameof(S.FConst), false)]
  [TestCase(typeof(S), nameof(S.FReadOnly), false)]
  [TestCase(typeof(S), nameof(S.FStaticReadOnly), false)]
#if NET
  [TestCase(typeof(SRef), nameof(SRef.FRef), false)]
  [TestCase(typeof(SRef), nameof(SRef.FRefReadOnly), false)]
  [TestCase(typeof(SReadOnlyRef), nameof(SReadOnlyRef.FReadOnlyRef), false)]
  [TestCase(typeof(SReadOnlyRef), nameof(SReadOnlyRef.FReadOnlyRefReadOnly), false)]
#endif
  public void IsRequired(Type type, string fieldName, bool isRequired)
  {
    var field = type.GetField(
      fieldName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    );

    Assert.That(
      field!.IsRequired(),
      Is.EqualTo(isRequired),
      $"{type.Name}.{field!.Name}"
    );
  }

  [Test]
  public void IsRequired_ArgumentNull()
    => Assert.That(
      () => ((FieldInfo)null!).IsRequired(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("f")
    );

  [TestCase(typeof(C), nameof(C.F0), false)]
  [TestCase(typeof(S), nameof(S.F), false)]
  [TestCase(typeof(S), nameof(S.FStatic), false)]
  [TestCase(typeof(S), nameof(S.FConst), false)]
  [TestCase(typeof(S), nameof(S.FReadOnly), false)]
  [TestCase(typeof(S), nameof(S.FStaticReadOnly), false)]
  [TestCase(typeof(SFixedSizeBuffers), nameof(SFixedSizeBuffers.Int), false)]
  [TestCase(typeof(SFixedSizeBuffers), nameof(SFixedSizeBuffers.Fixed4Int), true)]
  [TestCase(typeof(SFixedSizeBuffers), nameof(SFixedSizeBuffers.Fixed1Byte), true)]
  public void IsFixedBuffer(Type type, string fieldName, bool isFixedBuffer)
  {
    var field = type.GetField(
      fieldName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    );

    Assert.That(
      field!.IsFixedBuffer(),
      Is.EqualTo(isFixedBuffer),
      $"{type.Name}.{field!.Name}"
    );
  }

  [Test]
  public void IsFixedBuffer_ArgumentNull()
    => Assert.That(
      () => ((FieldInfo)null!).IsFixedBuffer(),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("f")
    );

#nullable enable
  [TestCase(typeof(C), nameof(C.F0), false, null, 0)]
  [TestCase(typeof(S), nameof(S.F), false, null, 0)]
  [TestCase(typeof(S), nameof(S.FStatic), false, null, 0)]
  [TestCase(typeof(S), nameof(S.FConst), false, null, 0)]
  [TestCase(typeof(S), nameof(S.FReadOnly), false, null, 0)]
  [TestCase(typeof(S), nameof(S.FStaticReadOnly), false, null, 0)]
  [TestCase(typeof(SFixedSizeBuffers), nameof(SFixedSizeBuffers.Int), false, null, 0)]
  [TestCase(typeof(SFixedSizeBuffers), nameof(SFixedSizeBuffers.Fixed4Int), true, typeof(int), 4)]
  [TestCase(typeof(SFixedSizeBuffers), nameof(SFixedSizeBuffers.Fixed1Byte), true, typeof(byte), 1)]
  public void IsFixedBuffer(Type type, string fieldName, bool isFixedBuffer, Type? expectedElementType, int expectedLength)
  {
    var field = type.GetField(
      fieldName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    );

    Assert.That(
      field!.TryGetFixedBufferElementTypeAndLength(out var elementType, out var length),
      Is.EqualTo(isFixedBuffer),
      $"{type.Name}.{field!.Name}"
    );

    if (isFixedBuffer) {
      Assert.That(elementType, Is.Not.Null, $"{type.Name}.{field!.Name}");
      Assert.That(elementType, Is.EqualTo(expectedElementType), $"{type.Name}.{field!.Name}");
      Assert.That(length, Is.EqualTo(expectedLength), $"{type.Name}.{field!.Name}");
    }
  }

  [Test]
  public void TryGetFixedBufferElementTypeAndLength_ArgumentNull()
    => Assert.That(
      () => ((FieldInfo)null!).TryGetFixedBufferElementTypeAndLength(out _, out _),
      Throws
        .ArgumentNullException
        .With
        .Property(nameof(ArgumentNullException.ParamName))
        .EqualTo("f")
    );
#nullable restore
}
