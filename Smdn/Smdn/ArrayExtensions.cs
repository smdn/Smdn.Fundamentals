// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

namespace Smdn {
  /// <summary>
  /// extension methods for System.Array
  /// </summary>
  public static class ArrayExtensions {
    public static T[] Append<T>(this T[] array, params T[] elements)
    {
      return Concat(array, new T[][] {elements});
      //return Concat(array, elements);
    }

    public static T[] Prepend<T>(this T[] array, params T[] elements)
    {
      return Concat(elements, new T[][] {array});
      //return Concat(elements, array);
    }

    public static T[] Concat<T>(this T[] array, params T[][] arrays)
    {
      if (array == null)
        throw new ArgumentNullException("array");

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
        throw new ArgumentNullException("array");

      return Slice(array, start, array.Length - start);
    }

    public static T[] Slice<T>(this T[] array, int start, int count)
    {
      if (array == null)
        throw new ArgumentNullException("array");

      var cut = new T[count];

      Array.Copy(array, start, cut, 0, count);

      return cut;
    }

    public static bool EqualsAll<T>(this T[] array, T[] other) where T : IEquatable<T>
    {
      if (array == null || other == null)
        return false;

      if (array.Length != other.Length)
        return false;

      for (var i = 0; i < array.Length; i++) {
        if (!array[i].Equals(other[i]))
          return false;
      }

      return true;
    }

    public static bool EqualsAll<T>(this T[] array, T[] other, IEqualityComparer<T> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException("comparer");

      if (array == null || other == null)
        return false;

      if (array.Length != other.Length)
        return false;

      for (var i = 0; i < array.Length; i++) {
        if (!comparer.Equals(array[i], other[i]))
          return false;
      }

      return true;
    }
  }
}
