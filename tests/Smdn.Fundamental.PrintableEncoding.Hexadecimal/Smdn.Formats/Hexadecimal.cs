// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
