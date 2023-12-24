// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Smdn.Reflection.Attributes;

namespace Smdn.Reflection;

public static class PropertyInfoExtensions {
  public static bool IsStatic(this PropertyInfo property)
  {
    if (property is null)
      throw new ArgumentNullException(nameof(property));

    if (property.CanRead && property.GetMethod is not null)
      return property.GetMethod.IsStatic;
    if (property.CanWrite && property.SetMethod is not null)
      return property.SetMethod.IsStatic;

    return false;
  }

  public static bool IsSetMethodInitOnly(this PropertyInfo property)
  {
    if (property is null)
      throw new ArgumentNullException(nameof(property));

    if (!property.CanWrite)
      throw new InvalidOperationException($"property {property.Name} is read-only");
    if (property.SetMethod is null)
      throw new InvalidOperationException($"cannot get {nameof(property.SetMethod)} from property {property.Name}");

    return property.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Any(
      static modreq => "System.Runtime.CompilerServices.IsExternalInit".Equals(modreq.FullName, StringComparison.Ordinal)
    );
  }

  // XXX: undocumented spec
  private const string PropertyBackingFieldPrefix = "<";
  private const string PropertyBackingFieldSuffix = ">k__BackingField";

  public static FieldInfo? GetBackingField(this PropertyInfo property)
  {
    if (property is null)
      throw new ArgumentNullException(nameof(property));

    if (!IsAccessorAutoGenerated(property))
      return null;

    if (property.DeclaringType is null)
      return null; // or throw exception?

    var backingField = property.DeclaringType.GetField(
      name: GetBackingFieldNameOf(property),
      bindingAttr: (IsStatic(property) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic
    );

    return backingField is not null && backingField.HasCompilerGeneratedAttribute()
      ? backingField
      : null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static string GetBackingFieldNameOf(PropertyInfo property)
    => string.Concat(PropertyBackingFieldPrefix, property.Name, PropertyBackingFieldSuffix);

  internal static string? GetPropertyNameFromBackingField(FieldInfo backingField)
  {
    if (!backingField.Name.StartsWith(PropertyBackingFieldPrefix, StringComparison.Ordinal))
      return null;

    var indexOfSuffix = backingField.Name.IndexOf(PropertyBackingFieldSuffix, StringComparison.Ordinal);

    if (indexOfSuffix < 0)
      return null;

    return backingField.Name.Substring(
      PropertyBackingFieldPrefix.Length,
      indexOfSuffix - PropertyBackingFieldPrefix.Length
    );
  }

  internal static bool IsAccessorAutoGenerated(PropertyInfo property)
    => property
      .GetAccessors(nonPublic: true)
      .Any(static accessor => accessor.HasCompilerGeneratedAttribute());

  internal static bool IsPrivate(PropertyInfo property)
    => property.GetMethod is null
      ? property.SetMethod is not null && property.SetMethod.IsPrivate
      : property.SetMethod is null
        ? property.GetMethod.IsPrivate
        : property.GetMethod.IsPrivate && property.SetMethod.IsPrivate;

  public static bool IsOverride(this PropertyInfo property)
    => property is null
      ? throw new ArgumentNullException(nameof(property))
      : property.GetMethod is null
        ? property.SetMethod is not null && property.SetMethod.IsOverride()
        : property.SetMethod is null
          ? property.GetMethod.IsOverride()
          : property.GetMethod.IsOverride() && property.SetMethod.IsOverride();

  public static bool IsAccessorReadOnly(this PropertyInfo property)
    => property is null
      ? throw new ArgumentNullException(nameof(property))
      : property.GetMethod?.IsReadOnly() ?? false;
}
