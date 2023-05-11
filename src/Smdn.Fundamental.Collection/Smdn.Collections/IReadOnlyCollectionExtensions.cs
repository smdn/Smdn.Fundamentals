// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Collections;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class IReadOnlyCollectionExtensions {
#pragma warning disable CA1002
  public static List<TOutput> ConvertAll<TInput, TOutput>(
    this IReadOnlyCollection<TInput> collection,
#if SYSTEM_CONVERTER
    Converter<TInput, TOutput> converter
#else
    Func<TInput, TOutput> converter
#endif
  )
#pragma warning restore CA1002
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
