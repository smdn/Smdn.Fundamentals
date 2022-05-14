// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_ARRAYSEGMENT_EMPTY
#endif

using System;
using NUnit.Framework;

namespace Smdn.Formats;

[TestFixture]
public partial class HexadecimalTests {
  private static ArraySegment<T> CreateEmptyArraySegment<T>()
#if SYSTEM_ARRAYSEGMENT_EMPTY
    => ArraySegment<T>.Empty;
#else
    => default;
#endif
}
