// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;

namespace Smdn.Reflection {
  public static class PropertyInfoExtensions {
    public static bool IsSetMethodInitOnly(this PropertyInfo property)
    {
      if (!property.CanWrite)
        throw new InvalidOperationException($"property {property.Name} is read-only");

      return property.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Any(
        modreq => "System.Runtime.CompilerServices.IsExternalInit".Equals(modreq.FullName, StringComparison.Ordinal)
      );
    }
  }
}


