// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;

namespace Smdn.Reflection.Attributes {
  public static class CustomAttributeTypedArgumentExtensions {
    // CustomAttributeTypedArgument.Value holds an underlying typed value if the ArgumentType is enum
    public static object GetTypedValue(this CustomAttributeTypedArgument typedArg)
      => typedArg.ArgumentType.IsEnum
        ? Enum.ToObject(typedArg.ArgumentType, typedArg.Value)
        : typedArg.Value;
  }
}
