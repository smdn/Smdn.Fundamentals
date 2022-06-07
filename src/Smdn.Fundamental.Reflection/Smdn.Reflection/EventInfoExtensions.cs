// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
}
