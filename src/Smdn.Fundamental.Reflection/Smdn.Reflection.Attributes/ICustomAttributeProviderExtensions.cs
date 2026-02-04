// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Smdn.Reflection.Attributes;

public static class ICustomAttributeProviderExtensions {
  // TODO: return IReadOnlyList
  public static IList<CustomAttributeData> GetCustomAttributeDataList(this ICustomAttributeProvider attributeProvider)
    => attributeProvider switch {
      Assembly assm => CustomAttributeData.GetCustomAttributes(assm),
      Module module => CustomAttributeData.GetCustomAttributes(module),
      MemberInfo member => CustomAttributeData.GetCustomAttributes(member),
      ParameterInfo param => CustomAttributeData.GetCustomAttributes(param),

      null => throw new ArgumentNullException(nameof(attributeProvider)),
      _ => throw new ArgumentException($"unsupported type of {nameof(ICustomAttributeProvider)}", nameof(attributeProvider)),
    };

  public static bool HasCompilerGeneratedAttribute(this ICustomAttributeProvider attributeProvider)
    => GetCustomAttributeDataList(attributeProvider ?? throw new ArgumentNullException(nameof(attributeProvider)))
      .Any(static d =>
        string.Equals(
          d.AttributeType.FullName,
          "System.Runtime.CompilerServices.CompilerGeneratedAttribute",
          StringComparison.Ordinal
        )
      );

  internal static bool HasExtensionAttribute(this ICustomAttributeProvider attributeProvider)
    => GetCustomAttributeDataList(attributeProvider)
      .Any(static d =>
        string.Equals(
          d.AttributeType.FullName,
          "System.Runtime.CompilerServices.ExtensionAttribute",
          StringComparison.Ordinal
        )
      );

  internal static bool HasExtensionMarkerAttribute(this ICustomAttributeProvider attributeProvider)
    => GetCustomAttributeDataList(attributeProvider)
      .Any(static d =>
        string.Equals(
          d.AttributeType.FullName,
          "System.Runtime.CompilerServices.ExtensionMarkerAttribute",
          StringComparison.Ordinal
        )
      );

  internal static bool HasIsReadOnlyAttribute(this ICustomAttributeProvider attributeProvider)
    => GetCustomAttributeDataList(attributeProvider)
      .Any(static d =>
        string.Equals(
          d.AttributeType.FullName,
          "System.Runtime.CompilerServices.IsReadOnlyAttribute",
          StringComparison.Ordinal
        )
      );

  internal static bool HasRequiresLocationAttribute(this ICustomAttributeProvider attributeProvider)
    => GetCustomAttributeDataList(attributeProvider)
      .Any(static d =>
        string.Equals(
          d.AttributeType.FullName,
          "System.Runtime.CompilerServices.RequiresLocationAttribute",
          StringComparison.Ordinal
        )
      );
}
