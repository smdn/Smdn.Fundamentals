// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using MethodInfoExtensionsExtensionMembersTestTypes;

namespace Smdn.Reflection;

#pragma warning disable IDE0040
partial class MethodInfoExtensionsTests {
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
      foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
        var isExtensionMethod = method.GetCustomAttribute<ExtensionMethodAttribute>(inherit: false) is not null;

        yield return new object?[] { method, isExtensionMethod };
      }

      foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
        foreach (var accessor in property.GetAccessors(nonPublic: true)) {
          var isExtensionMethod = accessor.GetCustomAttribute<ExtensionMethodAttribute>(inherit: false) is not null;

          yield return new object?[] { accessor, isExtensionMethod };
        }
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_TryGetExtensionMarkerType))]
  public void TryGetExtensionMarkerType(MethodInfo m, bool isExtensionMethod)
  {
    if (isExtensionMethod) {
      Assert.That(m.TryGetExtensionMarkerType(out var extensionMarkerType), Is.True);
      Assert.That(extensionMarkerType, Is.Not.Null);
      Assert.That(extensionMarkerType.IsExtensionMarkerType(), Is.True);
      Assert.That(extensionMarkerType.DeclaringType, Is.EqualTo(m.DeclaringType));
    }
    else {
      Assert.That(m.TryGetExtensionMarkerType(out var extensionMarkerType), Is.False);
      Assert.That(extensionMarkerType, Is.Null);
    }
  }

  [TestCaseSource(nameof(YieldTestCases_TryGetExtensionMarkerType))]
  public void TryGetExtensionParameter(MethodInfo m, bool isExtensionMethod)
  {
    if (isExtensionMethod) {
      Assert.That(m.TryGetExtensionParameter(out var extensionParameter), Is.True);
      Assert.That(extensionParameter, Is.Not.Null);
      Assert.That(
        // extension marker method -> extension marker type -> extension grouping type
        extensionParameter.Member.DeclaringType?.DeclaringType,
        Is.EqualTo(m.DeclaringType)
      );
    }
    else {
      Assert.That(m.TryGetExtensionParameter(out var extensionParameter), Is.False);
      Assert.That(extensionParameter, Is.Null);
    }
  }
}
