// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Smdn.Reflection;

public static class ParameterInfoExtensions {
  public static bool IsReturnParameter(this ParameterInfo param)
  {
    if (param is null)
      throw new ArgumentNullException(nameof(param));

#if NETFRAMEWORK
    return param.Position < 0 && param.ParameterType.Equals((param.Member as MethodInfo)?.ReturnType);
#else
    return param == (param.Member as MethodInfo)?.ReturnParameter;
#endif
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static BindingFlags GetBindingFlagsForAccessorOwner(MethodInfo accessor)
    => (accessor.IsStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic;

  public static PropertyInfo? GetDeclaringProperty(this ParameterInfo param)
  {
    if (param is null)
      throw new ArgumentNullException(nameof(param));
    if (param.Member is not MethodInfo accessor)
      return null;
    if (!accessor.IsSpecialName) // property accessor methods must have special name
      return null;
    if (accessor.DeclaringType is null)
      return null;

    var properties = accessor.DeclaringType.GetProperties(GetBindingFlagsForAccessorOwner(accessor));

    return IsReturnParameter(param)
      ? properties.FirstOrDefault(property => accessor == property.GetMethod)
      : properties.FirstOrDefault(property => accessor == property.SetMethod);
  }

  public static EventInfo? GetDeclaringEvent(this ParameterInfo param)
  {
    if (param is null)
      throw new ArgumentNullException(nameof(param));
    if (IsReturnParameter(param)) // event accessor methods never have return value
      return null;
    if (!string.Equals("value", param.Name, StringComparison.Ordinal)) // parameter name of event accessor method must be `value`
      return null;
    if (param.Member is not MethodInfo accessor)
      return null;
    if (!accessor.IsSpecialName) // event accessor methods must have special name
      return null;
    if (accessor.DeclaringType is null)
      return null;

    return accessor
      .DeclaringType
      .GetEvents(GetBindingFlagsForAccessorOwner(accessor))
      .FirstOrDefault(ev => accessor == ev.AddMethod || accessor == ev.RemoveMethod);
  }
}
