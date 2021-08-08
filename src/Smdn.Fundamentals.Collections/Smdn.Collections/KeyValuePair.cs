// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections.Generic;

namespace Smdn.Collections {
#if !(NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER)
  public static class KeyValuePair {
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue @value)
    {
      return new KeyValuePair<TKey, TValue>(key, @value);
    }
  }
#endif
}
