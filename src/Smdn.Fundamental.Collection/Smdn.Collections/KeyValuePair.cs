// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
using System.Collections.Generic;
#endif

namespace Smdn.Collections {
#if !SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class KeyValuePair {
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue @value) => new(key, @value);
  }
#endif
}
