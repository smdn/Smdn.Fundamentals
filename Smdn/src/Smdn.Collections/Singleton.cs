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
#if SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace Smdn.Collections {
  public static class Singleton {
    public static IReadOnlyList<T> CreateList<T>(T element) => new SingletonList<T>(element);

#if SERIALIZATION
    [Serializable]
#endif
    private class SingletonList<T> :
      IReadOnlyList<T>
#if SERIALIZATION
      , ISerializable
#endif
    {
      public T this[int index] => index == 0 ? element : throw ExceptionUtils.CreateArgumentMustBeInRange(0, 0, nameof(index), index);
      public int Count => 1;

      public SingletonList(T element)
      {
        this.element = element;
      }

#if SERIALIZATION
      protected SingletonList(SerializationInfo info, StreamingContext context)
      {
        this.element = (T)info.GetValue(nameof(element), typeof(T));
      }

      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
        info.AddValue(nameof(element), element);
      }
#endif

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

