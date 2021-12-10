// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;

namespace Smdn.Collections {
  public static class StringificationExtensions {
    public static string Stringify<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
      => string.Concat("{", pair.Key, " => ", pair.Value, "}");

    public static string Stringify<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
      const string separator = ", ";

      if (pairs == null)
        return null;

      return string.Join(separator, pairs.Select(Stringify));
    }
  }
}
