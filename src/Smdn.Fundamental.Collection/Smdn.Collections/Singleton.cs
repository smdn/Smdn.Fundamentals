// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_SERIALIZABLEATTRIBUTE
#define SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONINFO
#endif

using System;
using System.Collections;
using System.Collections.Generic;
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE || SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONINFO
using System.Runtime.Serialization;
#endif

namespace Smdn.Collections;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class Singleton {
  public static IReadOnlyList<T> CreateList<T>(T element) => new SingletonList<T>(element);

#if SYSTEM_SERIALIZABLEATTRIBUTE
  [Serializable]
#endif
#pragma warning disable IDE0055
  private class SingletonList<T> :
    IReadOnlyList<T>
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
#pragma warning disable SA1001
    ,
    ISerializable
#pragma warning restore SA1001
#endif
  {
#pragma warning restore IDE0055
    public T this[int index] => index == 0 ? element! : throw ExceptionUtils.CreateArgumentMustBeInRange(0, 0, nameof(index), index);
    public int Count => 1;

    public SingletonList(T element)
    {
      this.element = element;
    }

#if SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONINFO
    protected SingletonList(SerializationInfo info, StreamingContext context)
    {
      this.element = (T?)info.GetValue(nameof(element), typeof(T));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue(nameof(element), element);
    }
#endif

    public IEnumerator<T> GetEnumerator()
    {
      yield return element!;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private readonly T? element;
  }

#if false
  public static IReadOnlyDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(TKey key, TValue value)
  {
    throw new NotImplementedException();
  }
#endif
}
