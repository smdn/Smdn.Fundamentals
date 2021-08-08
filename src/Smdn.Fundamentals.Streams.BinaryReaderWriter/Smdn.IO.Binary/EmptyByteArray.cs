// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.IO.Binary {
#if NET45 || NET452
  internal static class EmptyByteArray {
    public static readonly byte[] Instance = new byte[0];
  }
#endif
}