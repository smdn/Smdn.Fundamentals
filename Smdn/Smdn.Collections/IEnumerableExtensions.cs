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

    public static IEnumerable<TResult> Cast<TResult>(this IEnumerable enumerable)
    {
      foreach (TResult e in enumerable)
        yield return e;
    }

    public static T Find<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException("match");

      if (enumerable is List<T>)
        return (enumerable as List<T>).Find(match);
      else if (enumerable is T[])
        return Array.Find(enumerable as T[], match);

      var enumerator = enumerable.GetEnumerator();

      while (enumerator.MoveNext()) {
        if (match(enumerator.Current))
          return enumerator.Current;
      }

      return default(T);
    }

    public static IEnumerable<T> FindAll<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException("match");

      /*
      if (enumerable is List<T>)
        return (enumerable as List<T>).FindAll(match);
      else if (enumerable is T[])
        return Array.FindAll(enumerable as T[], match);
      */

      var enumerator = enumerable.GetEnumerator();

      while (enumerator.MoveNext()) {
        if (match(enumerator.Current))
          yield return enumerator.Current;
      }
    }

    public static bool Exists<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException("match");

      if (enumerable is List<T>)
        return (enumerable as List<T>).Exists(match);
      else if (enumerable is T[])
        return Array.Exists(enumerable as T[], match);

      var enumerator = enumerable.GetEnumerator();

      while (enumerator.MoveNext()) {
        if (match(enumerator.Current))
          return true;
      }

      return false;
    }

    public static IEnumerable<T> Take<T>(this IEnumerable<T> enumerable, int count)
    {
      var enumerator = enumerable.GetEnumerator();

      while (0 < count-- && enumerator.MoveNext())
        yield return enumerator.Current;
    }

    public static IEnumerable<T> Reverse<T>(this IEnumerable<T> enumerable)
    {
      var list = (enumerable as IList<T>) ?? new List<T>(enumerable);

      for (var i = list.Count - 1; 0 <= i; i--)
        yield return list[i];
    }

    public static T[] ToArray<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable is List<T>)
        return (enumerable as List<T>).ToArray();

      var collection = enumerable as System.Collections.ICollection;

      if (collection == null) {
        return (new List<T>(enumerable)).ToArray();
      }
      else {
        var array = new T[collection.Count];

        collection.CopyTo(array, 0);

        return array;
      }
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

    public static T First<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable is IList<T>) {
        var list = enumerable as IList<T>;

        if (0 < list.Count)
          return list[0];
        else
          throw new InvalidOperationException("sequence is empty");
      }
      else {
        var enumerator = enumerable.GetEnumerator();

        if (enumerator.MoveNext())
          return enumerator.Current;
        else
          throw new InvalidOperationException("sequence is empty");
      }
    }

    public static T FirstOrDefault<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable is IList<T>) {
        var list = enumerable as IList<T>;

        if (0 < list.Count)
          return list[0];
        else
          return default(T);
      }
      else {
        var enumerator = enumerable.GetEnumerator();

        if (enumerator.MoveNext())
          return enumerator.Current;
        else
          return default(T);
      }
    }

    public static bool TrueForAll<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException("match");

      if (enumerable is List<T>)
        return (enumerable as List<T>).TrueForAll(match);
      else if (enumerable is T[])
        return Array.TrueForAll(enumerable as T[], match);

      var enumerator = enumerable.GetEnumerator();

      while (enumerator.MoveNext()) {
        if (!match(enumerator.Current))
          return false;
      }

      return true;
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
