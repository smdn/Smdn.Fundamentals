// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;


namespace Smdn {
  /// <summary>
  /// extension methods for System.Array
  /// </summary>
  public static class ArrayExtensions {
    public static T[] Append<T>(this T[] array, T element, params T[] elements)
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (elements == null)
        throw new ArgumentNullException(nameof(elements));

      if (array.Length == 0 && elements.Length == 0)
        return new T[] { element };

      var appended = new T[array.Length + 1 + elements.Length];

      Array.Copy(array, 0, appended, 0, array.Length);

      appended[array.Length] = element;

      if (0 < elements.Length)
        Array.Copy(elements, 0, appended, 1 + array.Length, elements.Length);

      return appended;
    }

    public static T[] Prepend<T>(this T[] array, T element, params T[] elements)
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (elements == null)
        throw new ArgumentNullException(nameof(elements));

      if (array.Length == 0 && elements.Length == 0)
        return new T[] { element };

      var prepended = new T[array.Length + 1 + elements.Length];

      prepended[0] = element;

      if (0 < elements.Length)
        Array.Copy(elements, 0, prepended, 1, elements.Length);

      Array.Copy(array, 0, prepended, 1 + elements.Length, array.Length);

      return prepended;
    }

    public static T[] Concat<T>(this T[] array, params T[][] arrays)
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));

      if (arrays == null || arrays.Length == 0)
        return array;

      var length = array.Length;

      for (var i = 0; i < arrays.Length; i++) {
        length += arrays[i].Length;
      }

      var concat = new T[length];
      var index = 0;

      Array.Copy(array, 0, concat, index, array.Length);

      index += array.Length;

      for (var i = 0; i < arrays.Length; i++) {
        Array.Copy(arrays[i], 0, concat, index, arrays[i].Length);

        index += arrays[i].Length;
      }

      return concat;
    }

    public static T[] Slice<T>(this T[] array, int start)
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));

      return Slice(array, start, array.Length - start);
    }

    public static T[] Slice<T>(this T[] array, int start, int count)
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (start < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(start), start);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (array.Length - count < start)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(start), array, start, count);

      var cut = new T[count];

      Array.Copy(array, start, cut, 0, count);

      return cut;
    }

    public static T[] Shuffle<T>(this T[] array)
    {
      return Shuffle(array, new Random());
    }

    public static T[] Shuffle<T>(this T[] array, Random random)
    {
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (random == null)
        throw new ArgumentNullException(nameof(random));

      var shuffled = (T[])array.Clone();

      if (shuffled.Length < 2)
        return shuffled;

      // http://ray.sakura.ne.jp/tips/shaffle.html
      for (var i = 1; i < shuffled.Length; i++) {
        var j = random.Next(0, i + 1);

        var temp = shuffled[i];
        shuffled[i] = shuffled[j];
        shuffled[j] = temp;
      }

      return shuffled;
    }

    public static TOutput[] Convert<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter)
    {
#if NETFRAMEWORK || NETSTANDARD2_0
      return Array.ConvertAll<TInput, TOutput>(array, converter);
#else
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (converter == null)
        throw new ArgumentNullException(nameof(converter));

      if (array.Length == 0)
        return Array.Empty<TOutput>();

      var ret = new TOutput[array.Length];

      for (var index = 0; index < array.Length; index++) {
        ret[index] = converter(array[index]);
      }

      return ret;
#endif
    }

#if NET45 || NET452
    public static T[] Empty<T>()
    {
      return EmptyArray<T>.Instance;
    }

    internal static class EmptyArray<T> {
      public static readonly T[] Instance = (
        Runtime.RuntimeEnvironment == RuntimeEnvironment.NetFx ? System.Linq.Enumerable.Empty<T>() as T[] :
        Runtime.RuntimeEnvironment == RuntimeEnvironment.Mono  ? System.Linq.Enumerable.Empty<T>() as T[] :
        null) ?? new T[0];
    }
#endif
  }
}
