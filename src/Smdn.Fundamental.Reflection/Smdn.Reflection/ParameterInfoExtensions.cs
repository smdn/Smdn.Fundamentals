// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
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

  public static PropertyInfo? GetDeclaringProperty(this ParameterInfo param)
  {
    if (param is null)
      throw new ArgumentNullException(nameof(param));
    if (param.Member is not MethodInfo accessor)
      return null;

    var isReturnParameter = IsReturnParameter(param);

    MethodInfoExtensions.TryGetPropertyFromAccessorMethod(
      accessor: accessor,
      expectGet: isReturnParameter,
      expectSet: !isReturnParameter,
      property: out var property
    );

    return property;
  }

  public static EventInfo? GetDeclaringEvent(this ParameterInfo param)
  {
    if (param is null)
      throw new ArgumentNullException(nameof(param));
    if (param.Member is not MethodInfo accessor)
      return null;
    if (IsReturnParameter(param)) // event accessor methods never have return value
      return null;
    if (!string.Equals("value", param.Name, StringComparison.Ordinal)) // parameter name of event accessor method must be `value`
      return null;

    MethodInfoExtensions.TryGetEventFromAccessorMethod(
      accessor: accessor,
      expectAdd: true,
      expectRemove: true,
      ev: out var ev
    );

    return ev;
  }
}
