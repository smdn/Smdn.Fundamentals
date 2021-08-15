// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smdn {
#if !SYSTEM_ARRAY_EMPTY
  public static class ArrayShim {
    public static T[] Empty<T>() => EmptyArray<T>.Instance;

    internal static class EmptyArray<T> {
      public static readonly T[] Instance = Enumerable.Empty<T>() as T[] ?? new T[0];
    }
  }
#endif
}
