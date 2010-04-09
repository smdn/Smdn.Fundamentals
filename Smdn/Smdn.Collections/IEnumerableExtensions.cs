// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
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
    [Obsolete("use Enumerable.SequenceEqual instead")]
    public static bool EqualsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> other) where T : IEquatable<T>
    {
      return Enumerable.SequenceEqual(enumerable, other);
    }

    [Obsolete("use Enumerable.SequenceEqual instead")]
    public static bool EqualsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> other, IEqualityComparer<T> comparer)
    {
      return Enumerable.SequenceEqual(enumerable, other, comparer);
    }

    [Obsolete("use Enumerable.Select instead")]
    public static IEnumerable<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> enumerable, Converter<TInput, TOutput> converter)
    {
      return Enumerable.Select(enumerable, delegate(TInput input) { return converter(input); });
    }

    [Obsolete("use Enumerable.SingleOrDefault instead")]
    public static T Find<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      return Enumerable.SingleOrDefault(enumerable, match);
    }

    [Obsolete("use Enumerable.Where instead")]
    public static IEnumerable<T> FindAll<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      return Enumerable.Where(enumerable, delegate(T val) { return match(val); });
    }

    [Obsolete("use Enumerable.Any instead")]
    public static bool Exists<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      return Enumerable.Any(enumerable, delegate(T val) { return match(val); });
    }

    [Obsolete("use Enumerable.All instead")]
    public static bool TrueForAll<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      return Enumerable.All(enumerable, match);
    }

    public static IEnumerable<T> EnumerateDepthFirst<T>(this IEnumerable<T> nestedEnumerable)
      where T : IEnumerable<T>
    {
      if (nestedEnumerable == null)
        throw new ArgumentNullException("nestedEnumerable");

      var stack = new Stack<IEnumerator<T>>();
      var enumerator = nestedEnumerable.GetEnumerator();

      for (;;) {
        if (enumerator.MoveNext()) {
          stack.Push(enumerator);

          yield return enumerator.Current;

          enumerator = enumerator.Current.GetEnumerator();
        }
        else {
          if (stack.Count == 0)
            yield break;

          enumerator = stack.Pop();
        }
      }
    }
  }
}
