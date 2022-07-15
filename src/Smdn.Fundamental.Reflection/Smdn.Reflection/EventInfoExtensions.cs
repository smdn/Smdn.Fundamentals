// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Smdn.Reflection;

public static class EventInfoExtensions {
  public static bool IsStatic(this EventInfo ev)
    => GetMethodsCore(
      ev ?? throw new ArgumentNullException(nameof(ev)),
      nonPublic: true
    ).Any(static m => m.IsStatic);

  public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev)
    => GetMethodsCore(
      ev ?? throw new ArgumentNullException(nameof(ev)),
      nonPublic: false
    );

  public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev, bool nonPublic)
    => GetMethodsCore(
      ev ?? throw new ArgumentNullException(nameof(ev)),
      nonPublic: nonPublic
    );

  private static IEnumerable<MethodInfo> GetMethodsCore(EventInfo ev, bool nonPublic)
  {
    var methodAdd = ev.GetAddMethod(nonPublic);
    var methodRemove = ev.GetRemoveMethod(nonPublic);
    var methodRaise = ev.GetRaiseMethod(nonPublic);

    if (methodAdd != null)
      yield return methodAdd;

    if (methodRemove != null)
      yield return methodRemove;

    if (methodRaise != null)
      yield return methodRaise;

    IEnumerable<MethodInfo>? otherMethods = null;

    try {
      otherMethods = ev.GetOtherMethods(nonPublic);
    }
    catch (NullReferenceException) { // MonoEvent.GetOtherMethods throws NullReferenceException
      // ignore exceptions
    }

    if (otherMethods == null)
      yield break;

    foreach (var m in otherMethods) {
      if (m != null)
        yield return m;
    }
  }

  public static FieldInfo? GetBackingField(this EventInfo ev)
  {
    if (ev is null)
      throw new ArgumentNullException(nameof(ev));

    if (!IsAccessorAutoGenerated(ev))
      return null;

    if (ev.DeclaringType is null)
      return null; // or throw exception?

    var backingField = ev.DeclaringType.GetField(
      name: GetBackingFieldNameOf(ev),
      bindingAttr: (IsStatic(ev) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic
    );

    return backingField?.GetCustomAttribute<CompilerGeneratedAttribute>() is null
      ? null
      : backingField;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsAccessorAutoGenerated(EventInfo ev)
    =>
      ev.AddMethod?.GetCustomAttribute<CompilerGeneratedAttribute>() is not null ||
      ev.RemoveMethod?.GetCustomAttribute<CompilerGeneratedAttribute>() is not null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static string GetBackingFieldNameOf(EventInfo ev)
    => ev.Name; // XXX: undocumented spec

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static string GetEventNameFromBackingField(FieldInfo backingField)
    => backingField.Name; // XXX: undocumented spec

  internal static bool IsPrivate(EventInfo ev)
    => ev.AddMethod is null
      ? ev.RemoveMethod is not null && ev.RemoveMethod.IsPrivate
      : ev.RemoveMethod is null
        ? ev.AddMethod.IsPrivate
        : ev.AddMethod.IsPrivate && ev.RemoveMethod.IsPrivate;

  public static bool IsOverride(this EventInfo ev)
    => ev is null
      ? throw new ArgumentNullException(nameof(ev))
      : ev.AddMethod is null
        ? ev.RemoveMethod is not null && ev.RemoveMethod.IsOverride()
        : ev.RemoveMethod is null
          ? ev.AddMethod.IsOverride()
          : ev.AddMethod.IsOverride() && ev.RemoveMethod.IsOverride();
}
