// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smdn {
  public static class ArrayShim {
#if !SYSTEM_ARRAY_EMPTY
    public static T[] Empty<T>() => EmptyArray<T>.Instance;

    internal static class EmptyArray<T> {
      public static readonly T[] Instance = Enumerable.Empty<T>() as T[] ?? new T[0];
    }
#endif

#if !SYSTEM_ARRAY_CONVERTALL
    public static TOutput[] ConvertAll<TInput, TOutput>(
      this TInput[] array,
#if SYSTEM_CONVERTER
      Converter<TInput, TOutput> converter
#else
      Func<TInput, TOutput> converter
#endif
    )
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (converter == null)
        throw new ArgumentNullException(nameof(converter));

      if (array.Length == 0)
#if SYSTEM_ARRAY_EMPTY
        return Array.Empty<TOutput>();
#else
        return Empty<TOutput>();
#endif

      var ret = new TOutput[array.Length];

      for (var index = 0; index < array.Length; index++) {
        ret[index] = converter(array[index]);
      }

      return ret;
    }
#endif
  }
}