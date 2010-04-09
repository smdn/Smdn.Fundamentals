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

#if !LINQ
using System;
using System.Collections;
using System.Collections.Generic;

namespace Smdn.Collections {
  public static class Enumerable {
    private static IEnumerator<TSource> GetEnumerator<TSource>(IEnumerable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");

      return source.GetEnumerator();
    }

    public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext()) {
        if (!predicate(enumerator.Current))
          return false;
      }

      return true;
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source)
    {
      return GetEnumerator(source).MoveNext();
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext()) {
        if (predicate(enumerator.Current))
          return true;
      }

      return false;
    }

    public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
    {
      foreach (TResult e in source) // this might throw InvalidCastException
        yield return e;
    }

    public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource @value)
    {
      if (source is ICollection<TSource>)
        return (source as ICollection<TSource>).Contains(@value);

      return Contains(source, @value, null);
    }

    public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource @value, IEqualityComparer<TSource> comparer)
    {
      var enumerator = GetEnumerator(source);

      if (comparer == null)
        comparer = EqualityComparer<TSource>.Default;

      while (enumerator.MoveNext()) {
        if (comparer.Equals(enumerator.Current, @value))
          return true;
      }

      return false;
    }

    public static int Count<TSource>(this IEnumerable<TSource> source)
    {
      if (source is System.Collections.ICollection)
        return (source as System.Collections.ICollection).Count;

      // XXX
      var count = 0;
      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext())
        count++;

      // TODO: throw OverflowException
      return count;
    }

    public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var count = 0;
      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext()) {
        if (predicate(enumerator.Current))
          count++;
      }

      // TODO: throw OverflowException
      return count;
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source)
    {
      if (source is IList<TSource>) {
        var list = source as IList<TSource>;

        if (0 < list.Count)
          return list[0];
        else
          throw new InvalidOperationException("sequence is empty");
      }
      else {
        var enumerator = GetEnumerator(source);

        if (enumerator.MoveNext())
          return enumerator.Current;
        else
          throw new InvalidOperationException("sequence is empty");
      }
    }

    /*
    public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      throw new NotImplementedException();
    }
    */

    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
    {
      if (source is IList<TSource>) {
        var list = source as IList<TSource>;

        if (0 < list.Count)
          return list[0];
        else
          return default(TSource);
      }
      else {
        var enumerator = GetEnumerator(source);

        if (enumerator.MoveNext())
          return enumerator.Current;
        else
          return default(TSource);
      }
    }

    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext()) {
        if (predicate(enumerator.Current))
          return enumerator.Current;
      }

      return default(TSource);
    }

    public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");

      var list = (source as IList<TSource>) ?? new List<TSource>(source);

      for (var i = list.Count - 1; 0 <= i; i--)
        yield return list[i];
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      if (selector == null)
        throw new ArgumentNullException("selector");

      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext())
        yield return selector(enumerator.Current);
    }

    /*
    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
    {
      if (selector == null)
        throw new ArgumentNullException("selector");

      throw new NotImplementedException();
    }
    */

    public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      return SequenceEqual(first, second, null);
    }

    public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      if (first == null && second == null)
        return true;
      else if (first == null)
        throw new ArgumentNullException("first");
      else if (second == null)
        throw new ArgumentNullException("second");

      if (comparer == null)
        comparer = EqualityComparer<TSource>.Default;

      var enumeratorFirst  = first.GetEnumerator();
      var enumeratorSecond = second.GetEnumerator();

      while (enumeratorFirst.MoveNext()) {
        if (!enumeratorSecond.MoveNext())
          return false;
        else if (!comparer.Equals(enumeratorFirst.Current, enumeratorSecond.Current))
          return false;
      }

      return !enumeratorSecond.MoveNext();
    }

    public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
    {
      var enumerator = GetEnumerator(source);

      while (0 < count-- && enumerator.MoveNext())
        yield return enumerator.Current;
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

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var enumerator = GetEnumerator(source);

      while (enumerator.MoveNext()) {
        if (predicate(enumerator.Current))
          yield return enumerator.Current;
      }
    }

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var enumerator = GetEnumerator(source);
      var index = 0;

      while (enumerator.MoveNext()) {
        if (predicate(enumerator.Current, index))
          yield return enumerator.Current;
      }
    }
  }
}
#endif // !LINQ
