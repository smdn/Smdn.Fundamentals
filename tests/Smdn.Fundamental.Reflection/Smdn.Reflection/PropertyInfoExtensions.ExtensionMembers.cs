// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using PropertyInfoExtensionsExtensionMembersTestTypes;

namespace Smdn.Reflection;

#pragma warning disable IDE0040
partial class PropertyInfoExtensionsTests {
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

  private static System.Collections.IEnumerable YieldTestCases_TryGetExtensionMarkerType()
  {
    foreach (var type in EnumerateNestedTypes(typeof(EnclosingClassForUriExtensionMembers))) {
      foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
        var isExtensionProperty = property.GetCustomAttribute<ExtensionPropertyAttribute>(inherit: false) is not null;

        yield return new object?[] { property, isExtensionProperty };
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_TryGetExtensionMarkerType))]
  public void TryGetExtensionMarkerType(PropertyInfo p, bool isExtensionProperty)
  {
    if (isExtensionProperty) {
      Assert.That(p.TryGetExtensionMarkerType(out var extensionMarkerType), Is.True);
      Assert.That(extensionMarkerType, Is.Not.Null);
      Assert.That(extensionMarkerType.IsExtensionMarkerType(), Is.True);
      Assert.That(extensionMarkerType.DeclaringType, Is.EqualTo(p.DeclaringType));
    }
    else {
      Assert.That(p.TryGetExtensionMarkerType(out var extensionMarkerType), Is.False);
      Assert.That(extensionMarkerType, Is.Null);
    }
  }

  [TestCaseSource(nameof(YieldTestCases_TryGetExtensionMarkerType))]
  public void TryGetExtensionParameter(PropertyInfo p, bool isExtensionProperty)
  {
    if (isExtensionProperty) {
      Assert.That(p.TryGetExtensionParameter(out var extensionParameter), Is.True);
      Assert.That(extensionParameter, Is.Not.Null);
      Assert.That(
        // extension marker method -> extension marker type -> extension grouping type
        extensionParameter.Member.DeclaringType?.DeclaringType,
        Is.EqualTo(p.DeclaringType)
      );
    }
    else {
      Assert.That(p.TryGetExtensionParameter(out var extensionParameter), Is.False);
      Assert.That(extensionParameter, Is.Null);
    }
  }
}
