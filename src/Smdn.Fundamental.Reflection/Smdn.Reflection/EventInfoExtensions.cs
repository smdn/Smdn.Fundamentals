// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Smdn.Reflection {
  public static class EventInfoExtensions {
    public static bool IsStatic(this EventInfo ev)
      => GetMethods(ev, nonPublic: true).Any(static m => m.IsStatic);

    public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev)
    {
      return GetMethods(ev, false);
    }

    public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev, bool nonPublic)
    {
      var methodAdd = ev.GetAddMethod(nonPublic);       if (methodAdd != null) yield return methodAdd;
      var methodRemove = ev.GetRemoveMethod(nonPublic); if (methodRemove != null) yield return methodRemove;
      var methodRaise = ev.GetRaiseMethod(nonPublic);   if (methodRaise != null) yield return methodRaise;

      IEnumerable<MethodInfo> otherMethods = null;

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
}
