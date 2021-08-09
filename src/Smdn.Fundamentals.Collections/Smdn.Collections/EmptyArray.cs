// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Collections {
#if !SYSTEM_ARRAY_EMPTY
  internal static class EmptyArray<T> {
    public static readonly T[] Instance = (System.Linq.Enumerable.Empty<T>() as T[]) ?? new T[0];
  }
#endif
}
