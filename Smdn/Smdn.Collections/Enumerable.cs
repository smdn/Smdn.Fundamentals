// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2012 smdn
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
#if !NET_3_5
  public static class Enumerable {
    private static class EmptyOf<T> {
      public static readonly T[] Enumerable = new T[0];
    }

    private static void CheckArgs(object source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
    }

    private static void CheckArgsPredicate(object source, object predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
    }

    private static void CheckArgsSelector(object source, object selector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (selector == null)
        throw new ArgumentNullException("selector");
    }

    public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      CheckArgsPredicate(source, predicate);

      foreach (var item in source) {
        if (!predicate(item))
          return false;
      }

      return true;
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source)
    {
      CheckArgs(source);

      using (var enumerator = source.GetEnumerator()) {
        return enumerator.MoveNext();
      }
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      CheckArgsPredicate(source, predicate);

      foreach (var item in source) {
        if (predicate(item))
          return true;
      }

      return false;
    }

    public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
    {
      CheckArgs(source);

      foreach (TResult castedItem in source) { // this might throw InvalidCastException
        yield return castedItem;
      }
    }

    public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource @value)
    {
      var collection = source as ICollection<TSource>;

      if (collection == null)
        return Contains(source, @value, null);
      else
        return collection.Contains(@value);
    }

    public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource @value, IEqualityComparer<TSource> comparer)
    {
      CheckArgs(source);

      if (comparer == null)
        comparer = EqualityComparer<TSource>.Default;

      foreach (var item in source) {
        if (comparer.Equals(item, @value))
          return true;
      }

      return false;
    }

    public static int Count<TSource>(this IEnumerable<TSource> source)
    {
      CheckArgs(source);

      var collection = source as System.Collections.ICollection;

      if (collection != null)
        return collection.Count;

      // XXX
      var count = 0;

      using (var enumerator = source.GetEnumerator()) {
        while (enumerator.MoveNext())
          count++;

        // TODO: throw OverflowException
        return count;
      }
    }

    public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      CheckArgsPredicate(source, predicate);

      var count = 0;

      foreach (var item in source) {
        if (predicate(item))
          count++;
      }

      // TODO: throw OverflowException
      return count;
    }

    public static IEnumerable<TSource> Empty<TSource>()
    {
      return EmptyOf<TSource>.Enumerable;
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source)
    {
      CheckArgs(source);

      var list = source as IList<TSource>;

      if (list == null) {
        foreach (var item in source) {
          return item;
        }

        throw new InvalidOperationException("sequence is empty");
      }
      else {
        if (0 < list.Count)
          return list[0];
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
      CheckArgs(source);

      var list = source as IList<TSource>;

      if (list == null) {
        foreach (var item in source) {
          return item;
        }

        return default(TSource);
      }
      else {
        if (0 < list.Count)
          return list[0];
        else
          return default(TSource);
      }
    }

    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      CheckArgsPredicate(source, predicate);

      foreach (var item in source) {
        if (predicate(item))
          return item;
      }

      return default(TSource);
    }

    public static IEnumerable<int> Range(int start, int count)
    {
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (int.MaxValue - count < start - 1)
        throw new ArgumentOutOfRangeException("count");

      for (var i = 0; i < count; i++)
        yield return start++;
    }

    public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
    {
      CheckArgs(source);

      var list = (source as IList<TSource>) ?? new List<TSource>(source);

      for (var i = list.Count - 1; 0 <= i; i--)
        yield return list[i];
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      CheckArgsSelector(source, selector);

      foreach (var item in source) {
        yield return selector(item);
      }
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

      using (IEnumerator<TSource> enumeratorFirst = first.GetEnumerator(), enumeratorSecond = second.GetEnumerator()) {
        while (enumeratorFirst.MoveNext()) {
          if (!enumeratorSecond.MoveNext())
            return false;
          else if (!comparer.Equals(enumeratorFirst.Current, enumeratorSecond.Current))
            return false;
        }

        return !enumeratorSecond.MoveNext();
      }
    }

    public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
    {
      CheckArgs(source);

      foreach (var item in source) {
        if (0 < count--)
          yield return item;
      }
    }

    public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
    {
      CheckArgs(source);

      var list = source as List<TSource>;

      if (list != null)
        return list.ToArray();

      var collection = source as System.Collections.ICollection;

      if (collection == null) {
        return (new List<TSource>(source)).ToArray();
      }
      else {
        var array = new TSource[collection.Count];

        collection.CopyTo(array, 0);

        return array;
      }
    }

    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
    {
      CheckArgs(source);

      return new List<TSource>(source);
    }

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      CheckArgsPredicate(source, predicate);

      foreach (var item in source) {
        if (predicate(item))
          yield return item;
      }
    }

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      CheckArgsPredicate(source, predicate);

      var index = 0;

      foreach (var item in source) {
        if (predicate(item, index++))
          yield return item;
      }
    }
  }
#endif
}
