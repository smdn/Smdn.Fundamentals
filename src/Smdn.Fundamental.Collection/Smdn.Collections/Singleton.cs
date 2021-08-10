// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Smdn.Collections {
  public static class Singleton {
    public static IReadOnlyList<T> CreateList<T>(T element) => new SingletonList<T>(element);

    [Serializable]
    private class SingletonList<T> :
      IReadOnlyList<T>,
      ISerializable
    {
      public T this[int index] => index == 0 ? element : throw ExceptionUtils.CreateArgumentMustBeInRange(0, 0, nameof(index), index);
      public int Count => 1;

      public SingletonList(T element)
      {
        this.element = element;
      }

      protected SingletonList(SerializationInfo info, StreamingContext context)
      {
        this.element = (T)info.GetValue(nameof(element), typeof(T));
      }

      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
        info.AddValue(nameof(element), element);
      }

      public IEnumerator<T> GetEnumerator()
      {
        yield return element;
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      private readonly T element;
    }

#if false
    public static IReadOnlyDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(TKey key, TValue value)
    {
      throw new NotImplementedException();
    }
#endif
  }
}

