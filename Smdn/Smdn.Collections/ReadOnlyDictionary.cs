// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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
  /*
   * System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> is available from .NET Framework 4.5
   */
#if !NET_4_5
  public class ReadOnlyDictionary<TKey, TValue> :
    IDictionary<TKey, TValue>,
    IDictionary
  {
    public sealed class KeyCollection : ICollection<TKey> {
      public int Count {
        get { return keys.Count; }
      }

      bool ICollection<TKey>.IsReadOnly {
        get { return true; }
      }

      internal KeyCollection(ICollection<TKey> keys)
      {
        this.keys = keys;
      }

      void ICollection<TKey>.Add(TKey item)
      {
        throw GetReadOnlyException();
      }

      void ICollection<TKey>.Clear()
      {
        throw GetReadOnlyException();
      }

      bool ICollection<TKey>.Contains(TKey item)
      {
        return keys.Contains(item);
      }

      public void CopyTo(TKey[] array, int arrayIndex)
      {
        keys.CopyTo(array, arrayIndex);
      }

      bool ICollection<TKey>.Remove(TKey item)
      {
        throw GetReadOnlyException();
      }

      public IEnumerator<TKey> GetEnumerator()
      {
        return keys.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return keys.GetEnumerator();
      }

      private static Exception GetReadOnlyException()
      {
        return new NotSupportedException("dictionary is read-only");
      }

      private readonly ICollection<TKey> keys;
    }

    public sealed class ValueCollection : ICollection<TValue> {
      public int Count {
        get { return values.Count; }
      }

      bool ICollection<TValue>.IsReadOnly {
        get { return true; }
      }

      internal ValueCollection(ICollection<TValue> values)
      {
        this.values = values;
      }

      void ICollection<TValue>.Add(TValue item)
      {
        throw GetReadOnlyException();
      }

      void ICollection<TValue>.Clear()
      {
        throw GetReadOnlyException();
      }

      bool ICollection<TValue>.Contains(TValue item)
      {
        return values.Contains(item);
      }

      public void CopyTo(TValue[] array, int arrayIndex)
      {
        values.CopyTo(array, arrayIndex);
      }

      bool ICollection<TValue>.Remove(TValue item)
      {
        throw GetReadOnlyException();
      }

      public IEnumerator<TValue> GetEnumerator()
      {
        return values.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return values.GetEnumerator();
      }

      private static Exception GetReadOnlyException()
      {
        return new NotSupportedException("dictionary is read-only");
      }

      private readonly ICollection<TValue> values;
    }

    protected IDictionary<TKey, TValue> Dictionary {
      get { return dictionary; }
    }

    public TValue this[TKey key] {
      get { return dictionary[key]; }
    }

    public KeyCollection Keys {
      get { return new KeyCollection(dictionary.Keys); }
    }

    public ValueCollection Values {
      get { return new ValueCollection(dictionary.Values); }
    }

    public int Count {
      get { return dictionary.Count; }
    }

    TValue IDictionary<TKey, TValue>.this[TKey key] {
      get { return dictionary[key]; }
      set { throw GetReadOnlyException(); }
    }

    object IDictionary.this[object key] {
      get { return (dictionary as IDictionary)[key]; }
      set { throw GetReadOnlyException(); }
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys {
      get { return dictionary.Keys; }
    }

    ICollection<TValue> IDictionary<TKey, TValue>.Values {
      get { return dictionary.Values; }
    }

    ICollection IDictionary.Keys {
      get { return (dictionary as IDictionary).Keys; }
    }

    ICollection IDictionary.Values {
      get { return (dictionary as IDictionary).Values; }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
      get { return true; }
    }

    bool IDictionary.IsReadOnly {
      get { return true; }
    }

    bool IDictionary.IsFixedSize {
      get { return (dictionary as IDictionary).IsFixedSize; }
    }

    bool ICollection.IsSynchronized {
      get { return (dictionary as IDictionary).IsSynchronized; }
    }

    object ICollection.SyncRoot {
      get { return (dictionary as IDictionary).SyncRoot; }
    }

    public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
    {
      if (dictionary == null)
        throw new ArgumentNullException("dictionary");

      this.dictionary = dictionary;
    }

    /*
     * read operations
     */
    public bool ContainsKey(TKey key)
    {
      return dictionary.ContainsKey(key);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
      return dictionary.Contains(item);
    }

    bool IDictionary.Contains(object key)
    {
      return (dictionary as IDictionary).Contains(key);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      dictionary.CopyTo(array, arrayIndex);
    }

    void ICollection.CopyTo(Array array, int index)
    {
      (dictionary as IDictionary).CopyTo(array, index);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      return dictionary.GetEnumerator();
    }

    System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
    {
      return (dictionary as IDictionary).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return dictionary.GetEnumerator();
    }

    public bool TryGetValue(TKey key, out TValue @value)
    {
      return dictionary.TryGetValue(key, out @value);
    }

    /*
     * write operations
     */
    void IDictionary<TKey, TValue>.Add(TKey key, TValue @value)
    {
      throw GetReadOnlyException();
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
      throw GetReadOnlyException();
    }

    void IDictionary.Add(object key, object value)
    {
      throw GetReadOnlyException();
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
    {
      throw GetReadOnlyException();
    }

    void IDictionary.Clear()
    {
      throw GetReadOnlyException();
    }

    bool IDictionary<TKey, TValue>.Remove(TKey key)
    {
      throw GetReadOnlyException();
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
      throw GetReadOnlyException();
    }

    void IDictionary.Remove(object key)
    {
      throw GetReadOnlyException();
    }

    private static Exception GetReadOnlyException()
    {
      return new NotSupportedException("dictionary is read-only");
    }

    private IDictionary<TKey, TValue> dictionary;
  }
#endif
}
