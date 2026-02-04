// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq;
using System.Reflection;

using Smdn.Reflection.Attributes;

namespace Smdn.Reflection;

internal static class ExtensionMembersUtils {
  public static bool TryGetExtensionMarkerType(
    MemberInfo member,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out Type? extensionMarkerType
  )
  {
    extensionMarkerType = null;

    if (member.DeclaringType is not { } extensionGroupType)
      return false;

    if (!TryGetExtensionMarkerTypeName(member, out var extensionMarkerTypeName))
      return false;

    extensionMarkerType = extensionGroupType.GetNestedType(extensionMarkerTypeName, BindingFlags.Public);

    return extensionMarkerType is not null;
  }

  public static bool TryGetExtensionParameter(
    MemberInfo member,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out ParameterInfo? extensionParameter
  )
  {
    extensionParameter = default;

    if (!TryGetExtensionMarkerType(member, out var extensionMarkerType))
      return false;

#pragma warning disable SA1114
    _ = TypeExtensions.IsExtensionMarkerTypeUnchecked(
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
      extensionMarkerType,
#else
      extensionMarkerType!,
#endif
      out extensionParameter
    );
#pragma warning restore SA1114

    return extensionParameter is not null;
  }

  internal static bool TryGetExtensionMarkerTypeName(
    MemberInfo member,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out string? extensionMarkerTypeName
  )
  {
    extensionMarkerTypeName = null;

    var extensionMarkerAttributeData = member
      .GetCustomAttributeDataList()
      .FirstOrDefault(
        static d => "System.Runtime.CompilerServices.ExtensionMarkerAttribute".Equals(d.AttributeType.FullName, StringComparison.Ordinal)
      );

    if (extensionMarkerAttributeData is null)
      return false;

    extensionMarkerTypeName = extensionMarkerAttributeData
      .ConstructorArguments
      .FirstOrDefault()
      .Value
      as string;

    extensionMarkerTypeName ??= extensionMarkerAttributeData
      .NamedArguments
      .FirstOrDefault(
        static namedArg => "Name".Equals(namedArg.MemberName, StringComparison.Ordinal)
      )
      .TypedValue
      .Value
      as string;

    return extensionMarkerTypeName is not null;
  }
}
