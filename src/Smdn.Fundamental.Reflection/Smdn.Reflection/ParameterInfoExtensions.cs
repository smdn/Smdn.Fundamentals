// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;

namespace Smdn.Reflection {
  public static class ParameterInfoExtensions {
    public static bool IsReturnParameter(this ParameterInfo param)
    {
      if (param is null)
        throw new ArgumentNullException(nameof(param));

#if NETFRAMEWORK
      return param.Position < 0 && param.ParameterType.Equals((param.Member as MethodInfo)?.ReturnType);
#else
      return (param == (param.Member as MethodInfo)?.ReturnParameter);
#endif
    }
  }
}
