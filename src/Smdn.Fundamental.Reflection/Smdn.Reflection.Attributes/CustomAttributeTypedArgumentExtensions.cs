// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;

namespace Smdn.Reflection.Attributes;

public static class CustomAttributeTypedArgumentExtensions {
  // CustomAttributeTypedArgument.Value holds an underlying typed value if the ArgumentType is enum
  public static object? GetTypedValue(this CustomAttributeTypedArgument typedArg)
    => typedArg.ArgumentType.IsEnum
      ? GetTypedEnumValue(typedArg)
      : typedArg.Value;

  private static object? GetTypedEnumValue(CustomAttributeTypedArgument typedArg)
  {
    if (typedArg.Value is null)
      return null;

    // try to convert a raw value to corresponding enum value
    try {
      return Enum.ToObject(enumType: typedArg.ArgumentType, typedArg.Value);
    }
    catch (ArgumentException ex) when (string.Equals(ex.ParamName, "enumType", StringComparison.Ordinal)) {
      var isRuntimeType = string.Equals(
        typedArg.ArgumentType.GetType().FullName,
        "System.RuntimeType",
        StringComparison.Ordinal
      );

      if (isRuntimeType)
        throw;

      // return the value as it is without conversion to enum since
      // the type may have been loaded in a reflection-only context and
      // the value may not be able to convert to the actual enum type
      return typedArg.Value;
    }
  }
}
