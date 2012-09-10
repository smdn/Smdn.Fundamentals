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
using System.Collections.Generic;

namespace Smdn.Collections {
  public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
    public TValue this[TKey key] {
      get { return dictionary[key]; }
      set { throw ReadOnlyException(); }
    }

    public ICollection<TKey> Keys {
      get { return dictionary.Keys; }
    }

    public ICollection<TValue> Values {
      get { return dictionary.Values; }
    }

    public int Count {
      get { return dictionary.Count; }
    }

    public bool IsReadOnly {
      get { return true; }
    }

    public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
    {
      if (dictionary == null)
        throw new ArgumentNullException("dictionary");

      this.dictionary = dictionary;
    }

    public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
      : this(pairs, null)
    {
    }

    public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs, IEqualityComparer<TKey> comparer)
    {
      if (pairs == null)
        throw new ArgumentNullException("pairs");

      this.dictionary = new Dictionary<TKey, TValue>(comparer);

      foreach (var pair in pairs) {
        dictionary.Add(pair);
      }
    }

    /*
     * read operations
     */
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      return dictionary.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
      return dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      dictionary.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      return dictionary.GetEnumerator();
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
    public void Add(KeyValuePair<TKey, TValue> item)
    {
      throw ReadOnlyException();
    }

    public void Add(TKey key, TValue @value)
    {
      throw ReadOnlyException();
    }

    public void Clear()
    {
      throw ReadOnlyException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      throw ReadOnlyException();
    }

    public bool Remove(TKey key)
    {
      throw ReadOnlyException();
    }

    private Exception ReadOnlyException()
    {
      return new NotSupportedException("dictionary is read-only");
    }

    private IDictionary<TKey, TValue> dictionary;
  }
}
