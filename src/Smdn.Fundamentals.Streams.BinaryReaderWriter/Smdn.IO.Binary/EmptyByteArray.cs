// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.IO.Binary {
#if !(NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET5_0_OR_GREATER)
  internal static class EmptyByteArray {
    public static readonly byte[] Instance = new byte[0];
  }
#endif
}