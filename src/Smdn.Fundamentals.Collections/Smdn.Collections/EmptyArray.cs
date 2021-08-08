// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Collections {
#if !(NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET5_0_OR_GREATER)
  internal static class EmptyArray<T> {
    public static readonly T[] Instance = (System.Linq.Enumerable.Empty<T>() as T[]) ?? new T[0];
  }
#endif
}
