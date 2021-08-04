// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Collections {
  public static class IReadOnlyCollectionExtensions {
    public static List<TOutput> ConvertAll<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof(collection));
      if (converter == null)
        throw new ArgumentNullException(nameof(converter));

      var ret = new List<TOutput>(collection.Count);

      foreach (var e in collection) {
        ret.Add(converter(e));
      }

      return ret;
    }
  }
}
