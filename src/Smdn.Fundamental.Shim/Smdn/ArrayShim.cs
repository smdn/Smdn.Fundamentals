// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;

namespace Smdn;

public static class ArrayShim {
  /*
   * SYSTEM_ARRAY_EMPTY
   */
  public static T[] Empty<T>() => EmptyArray<T>.Instance;

  internal static class EmptyArray<T> {
#pragma warning disable CA1825
    public static readonly T[] Instance = Enumerable.Empty<T>() as T[] ?? new T[0];
#pragma warning restore CA1825
  }

  /*
   * SYSTEM_ARRAY_CONVERTALL
   */
#if SYSTEM_CONVERTER
  public static TOutput[] ConvertAll<TInput, TOutput>(
    this TInput[] array,
    Converter<TInput, TOutput> converter
  )
    => ConvertAll(array, new Func<TInput, TOutput>(converter));
#endif

  public static TOutput[] ConvertAll<TInput, TOutput>(
    this TInput[] array,
    Func<TInput, TOutput> converter
  )
  {
    if (array == null)
      throw new ArgumentNullException(nameof(array));
    if (converter == null)
      throw new ArgumentNullException(nameof(converter));

    if (array.Length == 0)
      return ShimTypeSystemArrayEmpty.Empty<TOutput>();

    var ret = new TOutput[array.Length];

    for (var index = 0; index < array.Length; index++) {
      ret[index] = converter(array[index]);
    }

    return ret;
  }
}
