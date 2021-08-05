// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Collections {
#if NET45 || NET452
  internal static class EmptyArray<T> {
    public static readonly T[] Instance = (System.Linq.Enumerable.Empty<T>() as T[]) ?? new T[0];
  }
#endif
}
