// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore vtable
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Smdn.Reflection.Attributes;

namespace Smdn.Reflection;

public static class MethodInfoExtensions {
  [Obsolete($"use {nameof(IsOverride)} instead")]
  public static bool IsOverridden(this MethodInfo m)
    => IsOverride(m);

  private const MethodAttributes MethodAttributesIsOverrideMask = MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask | MethodAttributes.Static;
  private const MethodAttributes MethodAttributesIsOverride = MethodAttributes.Virtual | MethodAttributes.ReuseSlot; /* except static methods */

  public static bool IsOverride(this MethodInfo m)
    => m is null
      ? throw new ArgumentNullException(nameof(m))
      : (m.Attributes & MethodAttributesIsOverrideMask) == MethodAttributesIsOverride;

  /// <summary>
  /// Gets the <see cref="MethodInfo"/> for the method on the immediate base class that the current method overrides.
  /// </summary>
  /// <param name="m">
  /// The <see cref="MethodInfo"/> of the method to find the overridden base definition for.
  /// </param>
  /// <returns>
  /// The <see cref="MethodInfo"/> of the overridden method on the immediate base class if it exists;
  /// otherwise, <see langword="null"/>.
  /// This method returns <see langword="null"/> if the method is not an override (e.g., it is a new slot or the root virtual definition).
  /// </returns>
  /// <exception cref="ArgumentNullException">
  /// Thrown when <paramref name="m"/> is <see langword="null"/>.
  /// </exception>
  /// <remarks>
  /// Unlike <see cref="MethodInfo.GetBaseDefinition"/>, which jumps to the root of the inheritance chain,
  /// this method only moves one level up. It correctly handles generic methods by verifying generic parameter counts
  /// and ensures that methods hidden by the 'new' keyword are not incorrectly identified as overridden.
  /// </remarks>
  public static MethodInfo? GetImmediateOverriddenMethod(this MethodInfo m)
  {
    if (m is null)
      throw new ArgumentNullException(nameof(m));

    if (m.DeclaringType?.BaseType is not { } baseType)
      return null; // has no base type

    if (m.IsHidingInheritedMember(nonPublic: true))
      return null; // not override

    if (m.GetBaseDefinition() is not { } baseMethodDefinition)
      return null; // is base definition

    var genericArgumentCount = m.GetGenericArguments().Length;

    return baseType
      .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
      .FirstOrDefault(
        baseMethod =>
          m.IsGenericMethod == baseMethod.IsGenericMethod &&
          genericArgumentCount == baseMethod.GetGenericArguments().Length && // 0 if non-generic method
          baseMethodDefinition == baseMethod.GetBaseDefinition() &&
          string.Equals(m.Name, baseMethod.Name, StringComparison.Ordinal)
      );
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static BindingFlags GetBindingFlagsForAccessorOwner(MethodInfo accessor)
    => (accessor.IsStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic;

  public static bool IsPropertyGetMethod(this MethodInfo m)
    => TryGetPropertyFromAccessorMethod(
      accessor: m ?? throw new ArgumentNullException(nameof(m)),
      expectGet: true,
      expectSet: false,
      property: out _
    );

  public static bool IsPropertySetMethod(this MethodInfo m)
    => TryGetPropertyFromAccessorMethod(
      accessor: m ?? throw new ArgumentNullException(nameof(m)),
      expectGet: false,
      expectSet: true,
      property: out _
    );

  public static bool IsPropertyAccessorMethod(this MethodInfo m)
    => TryGetPropertyFromAccessorMethod(
      accessor: m ?? throw new ArgumentNullException(nameof(m)),
      expectGet: true,
      expectSet: true,
      property: out _
    );

  public static bool TryGetPropertyFromAccessorMethod(
    this MethodInfo? accessor,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out PropertyInfo? property
  )
    => TryGetPropertyFromAccessorMethod(
      accessor: accessor,
      expectGet: true,
      expectSet: true,
      property: out property
    );

  internal static bool TryGetPropertyFromAccessorMethod(
    MethodInfo? accessor,
    bool expectGet,
    bool expectSet,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out PropertyInfo? property
  )
  {
    if (
      accessor is null ||
      !accessor.IsSpecialName || // property accessor methods must have special name
      accessor.DeclaringType is null
    ) {
      property = null;
      return false;
    }

    var properties = accessor.DeclaringType.GetProperties(GetBindingFlagsForAccessorOwner(accessor));

#if false
    // In the reflection-only context, this will always be false,
    // since a comparison of System.RuntimeType and System.Reflection.TypeLoading.RoType will
    // always be false.
    var isSetAccessor = accessor.ReturnType == typeof(void);

    expectGet &= !isSetAccessor;
    expectSet &= isSetAccessor;
#endif

    property = expectGet ? properties.FirstOrDefault(prop => accessor == prop.GetMethod) : null;
    property ??= expectSet ? properties.FirstOrDefault(prop => accessor == prop.SetMethod) : null;

    return property is not null;
  }

  public static bool IsEventAddMethod(this MethodInfo m)
    => TryGetEventFromAccessorMethod(
      accessor: m ?? throw new ArgumentNullException(nameof(m)),
      expectAdd: true,
      expectRemove: false,
      ev: out _
    );

  public static bool IsEventRemoveMethod(this MethodInfo m)
    => TryGetEventFromAccessorMethod(
      accessor: m ?? throw new ArgumentNullException(nameof(m)),
      expectAdd: false,
      expectRemove: true,
      ev: out _
    );

  public static bool IsEventAccessorMethod(this MethodInfo m)
    => TryGetEventFromAccessorMethod(
      accessor: m ?? throw new ArgumentNullException(nameof(m)),
      expectAdd: true,
      expectRemove: true,
      ev: out _
    );

  public static bool TryGetEventFromAccessorMethod(
    this MethodInfo? accessor,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out EventInfo? ev
  )
    => TryGetEventFromAccessorMethod(
      accessor: accessor,
      expectAdd: true,
      expectRemove: true,
      ev: out ev
    );

  internal static bool TryGetEventFromAccessorMethod(
    MethodInfo? accessor,
    bool expectAdd,
    bool expectRemove,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out EventInfo? ev
  )
  {
    if (
      accessor is null ||
      accessor.ReturnType != typeof(void) || // event accessor methods never have return value
      !accessor.IsSpecialName || // event accessor methods must have special name
      accessor.DeclaringType is null
    ) {
      ev = null;
      return false;
    }

    var events = accessor.DeclaringType.GetEvents(GetBindingFlagsForAccessorOwner(accessor));

    ev = expectAdd ? events.FirstOrDefault(e => accessor == e.AddMethod) : null;
    ev ??= expectRemove ? events.FirstOrDefault(e => accessor == e.RemoveMethod) : null;

    return ev is not null;
  }

  public static bool IsDelegateSignatureMethod(this MethodInfo m)
    => m is null
      ? throw new ArgumentNullException(nameof(m))
      : m.DeclaringType is not null &&
        m.DeclaringType != typeof(Delegate) &&
        m.DeclaringType != typeof(MulticastDelegate) &&
        m == TypeExtensions.GetDelegateSignatureMethod(m.DeclaringType);

  internal static bool SignatureEqual(MethodInfo x, MethodInfo y)
  {
    if (x.ReturnParameter.ParameterType != y.ReturnParameter.ParameterType)
      return false;

    var paramsX = x.GetParameters();
    var paramsY = y.GetParameters();

    if (paramsX.Length != paramsY.Length)
      return false;

    for (var i = 0; i < paramsX.Length; i++) {
      if (paramsX[i].ParameterType != paramsY[i].ParameterType)
        return false;
    }

    return true;
  }

  public static bool IsReadOnly(this MethodInfo m)
    => m is null
      ? throw new ArgumentNullException(nameof(m))
      : m.HasIsReadOnlyAttribute();

  /// <summary>
  /// Attempts to get the <see cref="Type"/> represents the marker type corresponding to the
  /// extension member when <paramref name="m"/> is an extension method.
  /// </summary>
  /// <param name="m">The <see cref="MethodInfo"/> that represents the extension method.</param>
  /// <param name="extensionMarkerType">
  /// The <see cref="Type"/> that represents the retrieved marker type
  /// corresponding to <paramref name="m"/>.
  /// </param>
  /// <returns>
  /// <see langword="true"/> if the marker type corresponding to <paramref name="m"/> was found,
  /// otherwise <see langword="false"/>.
  /// </returns>
  /// <exception cref="ArgumentNullException"><paramref name="m"/> is <see langword="null"/>.</exception>
  public static bool TryGetExtensionMarkerType(
    this MethodInfo m,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out Type? extensionMarkerType
  )
    => ExtensionMembersUtils.TryGetExtensionMarkerType(
      m ?? throw new ArgumentNullException(nameof(m)),
      out extensionMarkerType
    );

  /// <summary>
  /// Attempts to get the <see cref="ParameterInfo"/> represents the extension parameter corresponding to the
  /// extension member when <paramref name="m"/> is an extension method.
  /// </summary>
  /// <param name="m">The <see cref="MethodInfo"/> that represents the extension method.</param>
  /// <param name="extensionParameter">
  /// The <see cref="ParameterInfo"/> that represents the retrieved extension parameter
  /// corresponding to <paramref name="m"/>.
  /// </param>
  /// <returns>
  /// <see langword="true"/> if the extension parameter corresponding to <paramref name="m"/> was found,
  /// otherwise <see langword="false"/>.
  /// </returns>
  /// <exception cref="ArgumentNullException"><paramref name="m"/> is <see langword="null"/>.</exception>
  public static bool TryGetExtensionParameter(
    this MethodInfo m,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out ParameterInfo? extensionParameter
  )
    => ExtensionMembersUtils.TryGetExtensionParameter(
      m ?? throw new ArgumentNullException(nameof(m)),
      out extensionParameter
    );

  public static bool IsAsyncStateMachine(this MethodInfo m)
    => m is null
      ? throw new ArgumentNullException(nameof(m))
      : m.HasAsyncStateMachineAttribute();
}
