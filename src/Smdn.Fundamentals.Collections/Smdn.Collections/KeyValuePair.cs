// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections.Generic;

namespace Smdn.Collections {
#if !SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
  public static class KeyValuePair {
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue @value)
    {
      return new KeyValuePair<TKey, TValue>(key, @value);
    }
  }
#endif
}
