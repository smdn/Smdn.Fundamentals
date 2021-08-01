// 
// Copyright (c) 2019 smdn <smdn@smdn.jp>
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
using System.Linq;

namespace Smdn.Collections {
  public static class IReadOnlyListExtensions {
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index)
    {
      if (list == null)
        throw new ArgumentNullException(nameof(list));

      return Slice(list, index, list.Count - index);
    }

    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index, int count)
    {
      if (list == null)
        throw new ArgumentNullException(nameof(list));
      if (index < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (list.Count - count < index)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfCollection(nameof(index), list, index, count);

      if (count == 0)
#if NET45 || NET452
        return ArrayExtensions.Empty<T>();
#else
        return Array.Empty<T>();
#endif

      if (list is T[] arr)
        return new ArraySegment<T>(arr, index, count);
      if (list is ArraySegment<T> seg)
        return new ArraySegment<T>(seg.Array, seg.Offset + index, count);
        // #if .NET Standard 2.1, .NET Core 2.0
        // return seg.Slice(start, count);

      return new ReadOnlyListSegment<T>(list, index, count);
    }

    private class ReadOnlyListSegment<T> : IReadOnlyList<T> {
      private readonly IReadOnlyList<T> list;
      private readonly int offset;
      public int Count { get; }

      public T this[int index] {
        get {
          if (index < 0 || Count <= index)
            throw ExceptionUtils.CreateArgumentMustBeInRange(0, nameof(Count), nameof(index), index);

          return list[offset + index];
        }
      }

      public ReadOnlyListSegment(IReadOnlyList<T> list, int offset, int count)
      {
        this.list = list;
        this.offset = offset;
        Count = count;
      }

      public IEnumerator<T> GetEnumerator()
      {
        // XXX
        foreach (var e in list.Skip(offset).Take(Count))
          yield return e;
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T> equalityComparer = default)
      => IndexOf(list ?? throw new ArgumentNullException(nameof(list)), item, 0, list.Count, equalityComparer);

    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, IEqualityComparer<T> equalityComparer = default)
      => IndexOf(list ?? throw new ArgumentNullException(nameof(list)), item, index, list.Count - index, equalityComparer);

    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, int count, IEqualityComparer<T> equalityComparer = default)
    {
      if (list == null)
        throw new ArgumentNullException(nameof(list));

      if (equalityComparer == null && list is List<T> l)
        return l.IndexOf(item, index, count);

      if (index < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (list.Count - count < index)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfCollection(nameof(index), list, index, count);

      equalityComparer ??= EqualityComparer<T>.Default;

      var end = index + count;

      for (var i = index; i < end; i++) {
        if (equalityComparer.Equals(list[i], item))
          return i;
      }

      return -1;
    }
  }
}
