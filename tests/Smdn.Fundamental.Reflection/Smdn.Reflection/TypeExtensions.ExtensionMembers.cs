// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using Smdn.Reflection.Attributes;

namespace Smdn.Reflection;

#pragma warning disable IDE0040
partial class TypeExtensionsTests {
#pragma warning restore IDE0040
  private static IEnumerable<Type> EnumerateNestedTypes(Type t)
  {
    foreach (var nestedType in t.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
      yield return nestedType;

      foreach (var childNestedType in EnumerateNestedTypes(nestedType)) {
        yield return childNestedType;
      }
    }
  }

  private static System.Collections.IEnumerable YieldTestCases_IsExtensionEnclosingClass()
  {
    const bool IsExtensionEnclosingType = true;

    yield return new object?[] { typeof(object), !IsExtensionEnclosingType };
    yield return new object?[] { typeof(Guid), !IsExtensionEnclosingType };
    yield return new object?[] { typeof(DateTimeKind), !IsExtensionEnclosingType };
    yield return new object?[] { typeof(Action), !IsExtensionEnclosingType };
    yield return new object?[] { typeof(System.Linq.Enumerable), !IsExtensionEnclosingType };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), !IsExtensionEnclosingType };

    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers), IsExtensionEnclosingType };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers), IsExtensionEnclosingType };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers), IsExtensionEnclosingType };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName), IsExtensionEnclosingType };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName), IsExtensionEnclosingType };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers), !IsExtensionEnclosingType };
  }

  [TestCaseSource(nameof(YieldTestCases_IsExtensionEnclosingClass))]
  public void IsExtensionEnclosingClass(Type t, bool expected)
    => Assert.That(t.IsExtensionEnclosingClass(), Is.EqualTo(expected));

  [Test]
  public void IsExtensionEnclosingClass_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).IsExtensionEnclosingClass(),
      Throws.ArgumentNullException
    );

  private static System.Collections.IEnumerable YieldTestCases_EnumerateExtensionGroupingTypes()
  {
    yield return new object?[] { typeof(object), 0 };
    yield return new object?[] { typeof(Guid), 0 };
    yield return new object?[] { typeof(DateTimeKind), 0 };
    yield return new object?[] { typeof(Action), 0 };
    yield return new object?[] { typeof(System.Linq.Enumerable), 0 };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), 0 };

    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers), 1 };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers), 2 };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers), 1 };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName), 1 };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName), 1 };
    yield return new object?[] { typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers), 0 };
  }

  [TestCaseSource(nameof(YieldTestCases_EnumerateExtensionGroupingTypes))]
  public void EnumerateExtensionGroupingTypes(Type t, int expectedNumberOfExtensionGroupingTypes)
    => Assert.That(
      t.EnumerateExtensionGroupingTypes().Count(),
      Is.EqualTo(expectedNumberOfExtensionGroupingTypes)
    );

  [Test]
  public void EnumerateExtensionGroupingTypes_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).EnumerateExtensionGroupingTypes(),
      Throws.ArgumentNullException
    );

  private static System.Collections.IEnumerable YieldTestCases_HasExtensionMembers()
  {
    static bool IsExtensionGroupingTypeName(Type t)
      => t.Name.StartsWith("<G>$", StringComparison.Ordinal); // the type which has a name starts with "<G>$" must be extension grouping type

    yield return new object?[] { typeof(object), false };
    yield return new object?[] { typeof(Guid), false };
    yield return new object?[] { typeof(DateTimeKind), false };
    yield return new object?[] { typeof(Action), false };
    yield return new object?[] { typeof(System.Linq.Enumerable), false };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), false };

    foreach (var type in EnumerateNestedTypes(typeof(System.Linq.Enumerable))) {
      yield return new object?[] { type, false };
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, IsExtensionGroupingTypeName(type) };
      }
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName)
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, false }; // has grouping type but no extension members
      }
    }

    foreach (var type in EnumerateNestedTypes(typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers))) {
      yield return new object?[] { type, false }; // must not determine that the fake types have extension members
    }
  }

  [TestCaseSource(nameof(YieldTestCases_HasExtensionMembers))]
  public void HasExtensionMembers(Type t, bool expected)
    => Assert.That(t.HasExtensionMembers(), Is.EqualTo(expected));

  [Test]
  public void HasExtensionMembers_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).HasExtensionMembers(),
      Throws.ArgumentNullException
    );

  private static System.Collections.IEnumerable YieldTestCases_IsExtensionGroupingType()
  {
    static bool IsExtensionGroupingTypeName(Type t)
      => t.Name.StartsWith("<G>$", StringComparison.Ordinal); // the type which has a name starts with "<G>$" must be extension grouping type

    yield return new object?[] { typeof(object), false };
    yield return new object?[] { typeof(Guid), false };
    yield return new object?[] { typeof(DateTimeKind), false };
    yield return new object?[] { typeof(Action), false };
    yield return new object?[] { typeof(System.Linq.Enumerable), false };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), false };

    foreach (var type in EnumerateNestedTypes(typeof(System.Linq.Enumerable))) {
      yield return new object?[] { type, IsExtensionGroupingTypeName(type) };
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, IsExtensionGroupingTypeName(type) };
      }
    }

    foreach (var type in EnumerateNestedTypes(typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers))) {
      yield return new object?[] { type, false }; // must not determine as extension grouping type for fake types
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsExtensionGroupingType))]
  public void IsExtensionGroupingType(Type t, bool expected)
    => Assert.That(t.IsExtensionGroupingType(), Is.EqualTo(expected));

  [Test]
  public void IsExtensionGroupingType_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).IsExtensionGroupingType(),
      Throws.ArgumentNullException
    );

  private static System.Collections.IEnumerable YieldTestCases_EnumerateExtensionMarkerTypeAndParameterPairs()
  {
    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
          var extensionMarkerAttributeData = member
            .GetCustomAttributeDataList()
            .FirstOrDefault(static d => "System.Runtime.CompilerServices.ExtensionMarkerAttribute".Equals(d.AttributeType.FullName, StringComparison.Ordinal));

          if (extensionMarkerAttributeData is null)
            continue;

          var extensionMarkerName = (string)extensionMarkerAttributeData.ConstructorArguments[0].Value!;
          var extensionMarkerType = member
            .DeclaringType!
            .GetNestedTypes(BindingFlags.Public | BindingFlags.DeclaredOnly)
            .FirstOrDefault(t => t.Name.Equals(extensionMarkerName, StringComparison.Ordinal))
            ?? throw new InvalidOperationException($"could not find extension marker type named '{extensionMarkerName}'");

          var expectedExtensionParameterAttribute = member.GetCustomAttribute<TypeExtensionsExtensionMembersTestTypes.ExpectedExtensionParameterAttribute>();

          if (expectedExtensionParameterAttribute is null)
            continue;

          yield return new object?[] {
            extensionMarkerType.DeclaringType!,
            extensionMarkerType,
            expectedExtensionParameterAttribute.Type,
            expectedExtensionParameterAttribute.Name
          };
        }
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_EnumerateExtensionMarkerTypeAndParameterPairs))]
  public void EnumerateExtensionMarkerTypeAndParameterPairs(Type t, Type expectedMarkerType, Type expectedTypeOfParameter, string? expectedNameOfParameter)
  {
    IReadOnlyList<(Type, ParameterInfo?)>? markerTypesAndParameters = null;

    Assert.That(
      () => markerTypesAndParameters = t.EnumerateExtensionMarkerTypeAndParameterPairs().ToList(),
      Throws.Nothing
    );
    Assert.That(markerTypesAndParameters, Is.Not.Null);
    Assert.That(markerTypesAndParameters.Count, Is.GreaterThan(0));

    Assert.That(
      markerTypesAndParameters.Select(static pair => pair.Item1),
      Does.Contain(expectedMarkerType)
    );

    var (_, extensionParameter) = markerTypesAndParameters.First(pair => pair.Item1 == expectedMarkerType);

    Assert.That(extensionParameter, Is.Not.Null);

    if (expectedTypeOfParameter.IsGenericTypeDefinition) {
      Assert.That(
        extensionParameter.ParameterType.GetGenericTypeDefinition(),
        Is.EqualTo(expectedTypeOfParameter)
      );
    }
    else {
      Assert.That(extensionParameter.ParameterType, Is.EqualTo(expectedTypeOfParameter));
    }

    Assert.That(
      extensionParameter.Name, // on Mono, this may result in an empty string rather than null
      expectedNameOfParameter is null
        ? Is.Null.Or.Empty
        : Is.EqualTo(expectedNameOfParameter)
    );
  }

  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName))]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName))]
  public void EnumerateExtensionMarkerTypeAndParameterPairs_NoExtensionMembers(Type extensionEnclosingType)
  {
    var extensionGroupingType = EnumerateNestedTypes(extensionEnclosingType).First(static t => t.IsExtensionGroupingType());

    IReadOnlyList<(Type, ParameterInfo?)>? markerTypesAndParameters = null;

    Assert.That(
      () => markerTypesAndParameters = extensionGroupingType.EnumerateExtensionMarkerTypeAndParameterPairs().ToList(),
      Throws.Nothing
    );
    Assert.That(markerTypesAndParameters, Is.Not.Null);
    Assert.That(markerTypesAndParameters.Count, Is.GreaterThan(0));

    foreach (var (extensionMarkerType, extensionParameter) in markerTypesAndParameters) {
      Assert.That(extensionMarkerType, Is.Not.Null);
      Assert.That(extensionParameter, Is.Null);
    }
  }

  private static System.Collections.IEnumerable YieldTestCases_EnumerateExtensionMarkerTypeAndParameterPairs_InvalidType()
  {
    static bool IsExtensionGroupingTypeName(Type t)
      => t.Name.StartsWith("<G>$", StringComparison.Ordinal); // the type which has a name starts with "<G>$" must be extension grouping type

    const bool IsExtensionGroupingType = true;
    const bool HasExtensionMembers = true;
    const bool NotEvaluated = false;

    yield return new object?[] { typeof(object), !IsExtensionGroupingType, NotEvaluated };
    yield return new object?[] { typeof(Guid), !IsExtensionGroupingType, NotEvaluated };
    yield return new object?[] { typeof(DateTimeKind), !IsExtensionGroupingType, NotEvaluated };
    yield return new object?[] { typeof(Action), !IsExtensionGroupingType, NotEvaluated };
    yield return new object?[] { typeof(System.Linq.Enumerable), !IsExtensionGroupingType, NotEvaluated };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), !IsExtensionGroupingType, NotEvaluated };

    foreach (var type in EnumerateNestedTypes(typeof(System.Linq.Enumerable))) {
      yield return new object?[] { type, IsExtensionGroupingTypeName(type), HasExtensionMembers };
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, IsExtensionGroupingTypeName(type), HasExtensionMembers };
      }
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, IsExtensionGroupingTypeName(type), !HasExtensionMembers }; // has no extension members
      }
    }

    foreach (var type in EnumerateNestedTypes(typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers))) {
      yield return new object?[] { type, !IsExtensionGroupingType, NotEvaluated }; // must not determine as extension marker type for fake types
    }
  }

  [TestCaseSource(nameof(YieldTestCases_EnumerateExtensionMarkerTypeAndParameterPairs_InvalidType))]
  public void EnumerateExtensionMarkerTypeAndParameterPairs_InvalidType(Type t, bool isExtensionGroupingType, bool hasExtensionMembers)
  {
    Assert.That(
      t.EnumerateExtensionMarkerTypeAndParameterPairs().ToList(),
      isExtensionGroupingType
        ? Has.Count.GreaterThanOrEqualTo(0) // zero or more
        : Is.Empty
    );

    var (extensionMarkerType, extensionParameter) = t.EnumerateExtensionMarkerTypeAndParameterPairs().FirstOrDefault();

    Assert.That(
      extensionParameter,
      (isExtensionGroupingType && hasExtensionMembers)
        ? Is.Not.Null
        : Is.Null
    );
  }

  [Test]
  public void EnumerateExtensionMarkerTypeAndParameterPairs_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).EnumerateExtensionMarkerTypeAndParameterPairs(),
      Throws.ArgumentNullException
    );

  private static System.Collections.IEnumerable YieldTestCases_IsExtensionMarkerType()
  {
    static bool IsNestedTypeDeclaredInExtensionGroupingTypeName(Type t)
      => t.DeclaringType!.Name.StartsWith("<G>$", StringComparison.Ordinal); // the type which has a name starts with "<G>$" must be extension grouping type

    yield return new object?[] { typeof(object), false };
    yield return new object?[] { typeof(Guid), false };
    yield return new object?[] { typeof(DateTimeKind), false };
    yield return new object?[] { typeof(Action), false };
    yield return new object?[] { typeof(System.Linq.Enumerable), false };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), false };

    foreach (var type in EnumerateNestedTypes(typeof(System.Linq.Enumerable))) {
      yield return new object?[] { type, IsNestedTypeDeclaredInExtensionGroupingTypeName(type) };
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, IsNestedTypeDeclaredInExtensionGroupingTypeName(type) };
      }
    }

    foreach (var type in EnumerateNestedTypes(typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers))) {
      yield return new object?[] { type, false }; // must not determine as extension grouping type for fake types
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsExtensionMarkerType))]
  public void IsExtensionMarkerType(Type t, bool expected)
    => Assert.That(t.IsExtensionMarkerType(), Is.EqualTo(expected));

  [Test]
  public void IsExtensionMarkerType_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).IsExtensionMarkerType(),
      Throws.ArgumentNullException
    );

  private static System.Collections.IEnumerable YieldTestCases_GetExtensionParameter()
  {
    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
          var extensionMarkerAttributeData = member
            .GetCustomAttributeDataList()
            .FirstOrDefault(static d => "System.Runtime.CompilerServices.ExtensionMarkerAttribute".Equals(d.AttributeType.FullName, StringComparison.Ordinal));

          if (extensionMarkerAttributeData is null)
            continue;

          var extensionMarkerName = (string)extensionMarkerAttributeData.ConstructorArguments[0].Value!;
          var extensionMarkerType = EnumerateNestedTypes(enclosingType)
            .FirstOrDefault(t => t.Name.Equals(extensionMarkerName, StringComparison.Ordinal))
            ?? throw new InvalidOperationException($"could not find extension marker type named '{extensionMarkerName}'");

          var expectedExtensionParameterAttribute = member.GetCustomAttribute<TypeExtensionsExtensionMembersTestTypes.ExpectedExtensionParameterAttribute>();

          if (expectedExtensionParameterAttribute is null)
            continue;

          yield return new object?[] {
            extensionMarkerType,
            expectedExtensionParameterAttribute.Type,
            expectedExtensionParameterAttribute.Name
          };
        }
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_GetExtensionParameter))]
  public void GetExtensionParameter(Type t, Type expectedTypeOfParameter, string? expectedNameOfParameter)
  {
    var param = t.GetExtensionParameter();

    Assert.That(param, Is.Not.Null);
    Assert.That(
      expectedTypeOfParameter.IsGenericTypeDefinition
        ? param.ParameterType.GetGenericTypeDefinition()
        : param.ParameterType,
      Is.EqualTo(expectedTypeOfParameter)
    );

    Assert.That(
      param.Name, // on Mono, this may result in an empty string rather than null
      expectedNameOfParameter is null
        ? Is.Null.Or.Empty
        : Is.EqualTo(expectedNameOfParameter)
    );
  }

  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName))]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName))]
  public void GetExtensionParameter_NoExtensionMembers(Type t)
  {
    var extensionMarkerType = EnumerateNestedTypes(t).First(static t => t.IsExtensionMarkerType());

    Assert.That(
      extensionMarkerType.GetExtensionParameter(),
      Is.Null
    );
  }

  private static System.Collections.IEnumerable YieldTestCases_GetExtensionParameter_InvalidType()
  {
    static bool IsExtensionMarkerType(Type t)
      => t.Name.StartsWith("<M>$", StringComparison.Ordinal);

    const bool ThrowsInvalidOperationException = true;

    yield return new object?[] { typeof(object), ThrowsInvalidOperationException };
    yield return new object?[] { typeof(Guid), ThrowsInvalidOperationException };
    yield return new object?[] { typeof(DateTimeKind), ThrowsInvalidOperationException };
    yield return new object?[] { typeof(Action), ThrowsInvalidOperationException };
    yield return new object?[] { typeof(System.Linq.Enumerable), ThrowsInvalidOperationException };
    yield return new object?[] { typeof(System.Collections.Generic.IEnumerable<string>), ThrowsInvalidOperationException };

    foreach (var type in EnumerateNestedTypes(typeof(System.Linq.Enumerable))) {
      yield return new object?[] { type, !IsExtensionMarkerType(type) };
    }

    foreach (var enclosingType in new[] {
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName),
      typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName),
    }) {
      foreach (var type in EnumerateNestedTypes(enclosingType)) {
        yield return new object?[] { type, !IsExtensionMarkerType(type) };
      }
    }

    foreach (var type in EnumerateNestedTypes(typeof(TypeExtensionsExtensionMembersTestTypes.FakeEnclosingClassForNonGenericExtensionMembers))) {
      yield return new object?[] { type, ThrowsInvalidOperationException }; // must not determine as extension marker type for fake types
    }
  }

  [TestCaseSource(nameof(YieldTestCases_GetExtensionParameter_InvalidType))]
  public void GetExtensionParameter_InvalidType(Type t, bool throwsInvalidOperationException)
    => Assert.That(
      () => t.GetExtensionParameter(),
      throwsInvalidOperationException
        ? Throws.InvalidOperationException
        : Throws.Nothing
    );

  [Test]
  public void GetExtensionParameter_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).GetExtensionParameter(),
      Throws.ArgumentNullException
    );

  const int CountOfAssignmentOperatorsInTestClass =
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    1;
#else
    0;
#endif

  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForNonGenericExtensionMembers), 3)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForOpenGenericExtensionMembers), 2)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForCloseGenericExtensionMembers), 2)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithParameterName), 0)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.EnclosingClassForEmptyExtensionMembersWithoutParameterName), 0)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.ExtensionAndImplementation_NonGenericParameter), 8 + CountOfAssignmentOperatorsInTestClass )]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.ExtensionAndImplementation_ConstructedGenericParameter), 8 + CountOfAssignmentOperatorsInTestClass)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.ExtensionAndImplementation_GenericTypeDefinitionParameter), 8 + CountOfAssignmentOperatorsInTestClass)]
  [TestCase(typeof(TypeExtensionsExtensionMembersTestTypes.ExtensionAndImplementation_MultipleExtensionGroupingTypes), (8 + CountOfAssignmentOperatorsInTestClass) * 3)]
  public void EnumerateExtensionMemberAndImplementationPairs(Type t, int expectedCountOfEnumeratingPairs)
  {
    var pairs = t.EnumerateExtensionMemberAndImplementationPairs().ToList();

    Assert.That(pairs.Count, Is.EqualTo(expectedCountOfEnumeratingPairs));

    for (var i = 0; i < pairs.Count; i++) {
      var impl = pairs[i].ImplementationMethod;
      var ext = pairs[i].ExtensionMember;

      Assert.That(impl, Is.Not.Null);
      Assert.That(ext, Is.Not.Null);

      Assert.That(impl.DeclaringType, Is.EqualTo(t), $"{nameof(impl)}.{nameof(impl.DeclaringType)} #{i}");
      Assert.That(ext.DeclaringType, Is.Not.EqualTo(t), $"{nameof(ext)}.{nameof(ext.DeclaringType)} #{i}");
      Assert.That(ext.DeclaringType.DeclaringType, Is.EqualTo(t), $"{nameof(ext)}.{nameof(ext.DeclaringType)}.{nameof(ext.DeclaringType)} #{i}");
      Assert.That(impl.Name, Is.EqualTo(ext.Name), $"{nameof(impl.Name)} #{i}");

      if (impl.IsGenericMethodDefinition) {
        // TODO: compare IEnumerable`1[T] (impl) and IEnumerable`1[$T0] (ext)
      }
      else {
        Assert.That(
          impl.ReturnType,
          Is.EqualTo(ext.ReturnType),
          $"{nameof(impl.ReturnType)} #{i}"
        );
      }

      Assert.That(
        impl.GetParameters().Length,
        Is.EqualTo(
          ext.GetParameters().Length + (ext.IsStatic ? 0 : 1 /* extension parameter */)
        ),
        $"{nameof(impl.GetParameters)} #{i}"
      );

      if (!ext.TryGetExtensionParameter(out var extParameter)) {
        Assert.Fail("could not get extension parameter from extension member");
        return;
      }

      var implParameters = impl.GetParameters();
      var extParameters = ext.GetParameters();

      if (!ext.IsStatic)
        extParameters = [extParameter, .. extParameters];

      for (var j = 0; j < implParameters.Length; j++) {
        Assert.That(
          implParameters[j].Name,
          Is.EqualTo(extParameters[j].Name),
          $"{nameof(ParameterInfo.Name)} #{i}-parameter#{j}"
        );
        // TODO: test parameter types
      }
    }
  }

  [Test]
  public void EnumerateExtensionMemberAndImplementationPairs_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).EnumerateExtensionMemberAndImplementationPairs(),
      Throws.ArgumentNullException
    );
}
