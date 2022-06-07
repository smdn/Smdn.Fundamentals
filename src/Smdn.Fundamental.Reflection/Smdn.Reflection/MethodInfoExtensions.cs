// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;

namespace Smdn.Reflection;

public static class MethodInfoExtensions {
  public static bool IsOverridden(this MethodInfo m)
    => m is null
      ? throw new ArgumentNullException(nameof(m))
      : (m.Attributes & MethodAttributes.Virtual) != 0 && (m.Attributes & MethodAttributes.NewSlot) == 0;
}
