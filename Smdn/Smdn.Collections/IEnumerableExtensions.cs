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
using System.Collections;
using System.Collections.Generic;

namespace Smdn.Collections {
  public static class IEnumerableExtensions {
    public static bool EqualsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> other) where T : IEquatable<T>
    {
      if (enumerable == null && other == null)
        return true;
      else if (enumerable == null || other == null)
        return false;

      var enumeratorThis  = enumerable.GetEnumerator();
      var enumeratorOther = other.GetEnumerator();

      while (enumeratorThis.MoveNext()) {
        if (!enumeratorOther.MoveNext())
          return false;
        else if (!enumeratorThis.Current.Equals(enumeratorOther.Current))
          return false;
      }

      return !enumeratorOther.MoveNext();
    }

    public static bool EqualsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> other, IEqualityComparer<T> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException("comparer");

      if (enumerable == null && other == null)
        return true;
      else if (enumerable == null || other == null)
        return false;

      var enumeratorThis  = enumerable.GetEnumerator();
      var enumeratorOther = other.GetEnumerator();

      while (enumeratorThis.MoveNext()) {
        if (!enumeratorOther.MoveNext())
          return false;
        else if (!comparer.Equals(enumeratorThis.Current, enumeratorOther.Current))
          return false;
      }

      return !enumeratorOther.MoveNext();
    }

    public static IEnumerable<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> enumerable, Converter<TInput, TOutput> converter)
    {
      var enumerator = enumerable.GetEnumerator();

      while (enumerator.MoveNext())
        yield return converter(enumerator.Current);
    }

    public static int Count(this IEnumerable enumerable)
    {
      if (enumerable is System.Collections.ICollection)
        return (enumerable as System.Collections.ICollection).Count;

      // XXX
      var count = 0;
      var enumerator = enumerable.GetEnumerator();

      while (enumerator.MoveNext())
        count++;

      return count;
    }
  }
}
