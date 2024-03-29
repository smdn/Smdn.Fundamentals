// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Smdn.Collections;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
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
#if SYSTEM_ARRAY_EMPTY
      return Array.Empty<T>();
#else
      return ArrayShim.Empty<T>();
#endif

    if (list is T[] arr)
      return new ArraySegment<T>(arr, index, count);

    if (list is ArraySegment<T> seg) {
#if SYSTEM_ARRAYSEGMENT_SLICE
      return seg.Slice(index, count);
#else
      return new ArraySegment<T>(seg.Array, seg.Offset + index, count);
#endif
    }

    return new ReadOnlyListSegment<T>(list, index, count);
  }

  private sealed class ReadOnlyListSegment<T> : IReadOnlyList<T> {
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

  public static int IndexOf<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T>? equalityComparer = default)
    => IndexOf(list ?? throw new ArgumentNullException(nameof(list)), item, 0, list.Count, equalityComparer);

  public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, IEqualityComparer<T>? equalityComparer = default)
    => IndexOf(list ?? throw new ArgumentNullException(nameof(list)), item, index, list.Count - index, equalityComparer);

  public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, int count, IEqualityComparer<T>? equalityComparer = default)
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
