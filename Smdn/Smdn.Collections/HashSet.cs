// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2013 smdn
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

#if false
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Smdn.Collections {
  /*
   * System.Collections.Generic.HashSet<T> is available from .NET Framework 3.5
   * Starting with the .NET Framework version 4, the HashSet<T> class implements the ISet<T> interface.
   */
#if NET_4_0
#elif NET_3_5
  [Serializable]
  public partial class HashSet<T> :
    System.Collections.Generic.HashSet<T>,
    ISet<T>
  {
    public HashSet()
      : base()
    {
    }

    public HashSet(IEqualityComparer<T> comparer)
      : base(comparer)
    {
    }

    public HashSet(IEnumerable<T> collection)
      : base(collection)
    {
    }

    public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
      : base(collection, comparer)
    {
    }
  }
#else
  [Serializable]
  public partial class HashSet<T> :
    ISerializable,
    IDeserializationCallback,
    ISet<T>
  {
    public HashSet()
    {
      Initialize(null, EqualityComparer<T>.Default);
    }

    public HashSet(IEqualityComparer<T> comparer)
    {
      Initialize(null, comparer);
    }

    public HashSet(IEnumerable<T> collection)
    {
      if (collection == null)
        throw new ArgumentNullException("collection");

      Initialize(collection, null);
    }

    public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
    {
      if (collection == null)
        throw new ArgumentNullException("collection");

      Initialize(collection, comparer);
    }
  }
#endif

#if !NET_3_5
  partial class HashSet<T> {
    public struct Enumerator :
      IEnumerator<T>,
      IDisposable
    {
      object IEnumerator.Current {
        get
        {
          CheckState();

          return currentBucket.Item;
        }
      }

      public T Current {
        get
        {
          CheckState();

          return currentBucket.Item;
        }
      }

      internal Enumerator(HashSet<T> hashset)
      {
        this.hashset = hashset;
        this.currentBucket = null;
        this.bucketIndex = 0;
        this.generation = hashset.generation;
      }

      public bool MoveNext()
      {
        CheckState();

        if (currentBucket != null) {
          currentBucket = currentBucket.Next;

          if (currentBucket != null)
            return true;
        }

        for (; bucketIndex < hashset.buckets.Length;) {
          currentBucket = hashset.buckets[bucketIndex];

          bucketIndex++;

          if (currentBucket != null)
            break;
        }

        return currentBucket != null;
      }

      public void Reset()
      {
        CheckState();

        bucketIndex = 0;
        currentBucket = null;
      }

      public void Dispose()
      {
        hashset = null;
      }

      private void CheckState()
      {
        if (hashset == null)
          throw new ObjectDisposedException(GetType().FullName);
        if (this.generation != hashset.generation)
          throw new InvalidOperationException("collection changed while enumerating");
      }

      private HashSet<T> hashset;
      private HashSet<T>.Bucket currentBucket;
      private int bucketIndex;
      private readonly int generation;
    }

    private const int InitialCapacity = 17;
    private const int CapacityThresholdPercent = 90; // %

    private class Bucket {
      public readonly T Item;
      public readonly int Hash;
      public Bucket Next;

      public Bucket(T @item, int hash)
      {
        this.Item = @item;
        this.Hash = hash;
        this.Next = null;
      }
    }

    public static IEqualityComparer<HashSet<T>> CreateSetComparer()
    {
      throw new NotImplementedException();
    }

    /*
     * instance members
     */
    public IEqualityComparer<T> Comparer {
      get { return comparer; }
    }

    public int Count {
      get { return itemCount; }
    }

    bool ICollection<T>.IsReadOnly {
      get { return false; }
    }

    protected HashSet(SerializationInfo info, StreamingContext context)
    {
      throw new NotImplementedException();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      throw new NotImplementedException();
    }

    public void OnDeserialization(object sender)
    {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return new Enumerator(this);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return new Enumerator(this);
    }

    private void Initialize(IEnumerable<T> collection, IEqualityComparer<T> comparer)
    {
      this.comparer = comparer ?? EqualityComparer<T>.Default;

      GrowCapacity();

      if (collection == null)
        return;

      foreach (var item in collection) {
        Add(item);
      }
    }

    private void GrowCapacity()
    {
      if (buckets == null) {
        this.buckets = new Bucket[InitialCapacity];
        this.threshold = InitialCapacity * CapacityThresholdPercent / 100;
        return;
      }

      /*
       * http://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx
       * 
       * As elements are added to a Hashtable, the actual load factor of the Hashtable increases.
       * When the actual load factor reaches the specified load factor,
       * the number of buckets in the Hashtable is automatically increased to 
       * the smallest prime number that is larger than twice the current number of 
       * Hashtable buckets.
       */
      var newCapacity = (int)MathUtils.NextPrimeNumber(buckets.Length * 2 + 1);
      var newBuckets = new Bucket[newCapacity];

      foreach (var bucket in buckets) {
        if (bucket == null)
          continue;

        var index = bucket.Hash % newCapacity;
        var next = bucket.Next;

        if (newBuckets[index] == null) {
          newBuckets[index] = bucket;
          newBuckets[index].Next = null;
        }
        else {
          var last = newBuckets[index];

          for (;last.Next != null; last = last.Next);

          last.Next = bucket;
          last.Next.Next = null;
        }

        for (;next != null;) {
          var n = next.Next;

          index = next.Hash % newCapacity;

          if (newBuckets[index] == null) {
            newBuckets[index] = next;
            newBuckets[index].Next = null;
          }
          else {
            var last = newBuckets[index];

            for (;last.Next != null; last = last.Next);

            last.Next = next;
            last.Next.Next = null;
          }

          next = n;
        }
      }

      this.buckets = newBuckets;
      this.threshold = newCapacity * CapacityThresholdPercent / 100;
    }

    private int GetHashAndIndex(T item, out int bucketIndex)
    {
      // XXX
      var hash = (item == null)
          ? 0
          : comparer.GetHashCode(item);

      bucketIndex = (hash & 0x7fffffff) % buckets.Length;

      return hash;
    }

    void ICollection<T>.Add(T item)
    {
      Add(item);
    }

    public bool Add(T item)
    {
      if (threshold <= itemCount)
        GrowCapacity();

      int index;
      var hash = GetHashAndIndex(item, out index);

      if (buckets[index] == null) {
        buckets[index] = new Bucket(item, hash);
        itemCount++;
        generation++;
        return true;
      }
      else {
        var bucket = buckets[index];

        for (;;) {
          if (comparer.Equals(bucket.Item, item)) {
            return false; // already contains
          }
          else if (bucket.Next == null) {
            bucket.Next = new Bucket(item, hash);
            itemCount++;
            generation++;
            return true;
          }
          else {
            bucket = bucket.Next;
          }
        }
      }
    }

    public bool Remove(T item)
    {
      int index;
      var hash = GetHashAndIndex(item, out index);

      if (buckets[index] == null)
        return false; // not exist

      var bucket = buckets[index];

      if (bucket.Next == null) {
        if (item == null ? bucket.Item == null : bucket.Hash == hash) {
          buckets[index] = null;
          itemCount--;
          generation++;
          return true;
        }
        else {
          return false; // not exist
        }
      }

      for (var prev = bucket;;) {
        if (comparer.Equals(bucket.Item, item)) {
          if (bucket == buckets[index])
            buckets[index] = bucket.Next;
          else
            prev.Next = bucket.Next;

          itemCount--;
          generation++;
          return true;
        }
        else {
          prev = bucket;
          bucket = bucket.Next;

          if (bucket == null)
            return false; // not exist
        }
      }
    }

    public int RemoveWhere(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException("match");

      // XXX
      var matched = new List<T>();

      for (var index = 0; index < buckets.Length; index++) {
        if (buckets[index] == null)
          continue;

        if (match(buckets[index].Item))
          matched.Add(buckets[index].Item);

        for (var b = buckets[index].Next; b != null; b = b.Next) {
          if (match(b.Item))
            matched.Add(b.Item);
        }
      }

      foreach (var e in matched) {
        Remove(e);
      }

      return matched.Count;
    }

    public void Clear()
    {
      Array.Clear(buckets, 0, buckets.Length);

      itemCount = 0;
      generation++;
    }

    public bool Contains(T item)
    {
      int index;
      var hash = GetHashAndIndex(item, out index);

      if (buckets[index] == null)
        return false;
      else if (buckets[index].Next == null)
        return (item == null ? buckets[index].Item == null : buckets[index].Hash == hash);

      for (var b = buckets[index]; b != null; b = b.Next) {
        if (comparer.Equals(b.Item, item))
          return true;
      }

      return false;
    }

    public void CopyTo(T[] array)
    {
      if (array == null)
        throw new ArgumentNullException("array");

      CopyTo(array, 0, array.Length);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException("array");

      CopyTo(array, arrayIndex, array.Length - arrayIndex);
    }

    public void CopyTo(T[] array, int arrayIndex, int count)
    {
      if (array == null)
        throw new ArgumentNullException("array");
      if (arrayIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arrayIndex", arrayIndex);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (array.Length - count < arrayIndex)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("arrayIndex", array, arrayIndex, count);

      for (var index = 0; index < buckets.Length; index++) {
        if (buckets[index] == null)
          continue;

        if (count-- == 0)
          break;

        array[arrayIndex++] = buckets[index].Item;

        for (var b = buckets[index].Next; b != null; b = b.Next) {
          if (count-- == 0)
            break;

          array[arrayIndex++] = b.Item;
        }
      }
    }

    public void TrimExcess()
    {
      throw new NotImplementedException();
    }

    private HashSet<T> ToHashSet(IEnumerable<T> other)
    {
      var h = other as HashSet<T>;

      if (h == null || !h.comparer.Equals(this.comparer))
        return new HashSet<T>(other, this.comparer);
      else
        return h;
    }

    public void ExceptWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      foreach (var item in other) {
        Remove(item);
      }
    }

    public void IntersectWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      RemoveWhere(delegate(T item) {
        return !otherSet.Contains(item);
      });
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      foreach (var item in otherSet) {
        if (!Add(item))
          Remove(item);
      }
    }

    public void UnionWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      foreach (var item in other) {
        Add(item);
      }
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      if (itemCount == 0)
        return (otherSet.Count == 0);

      if (otherSet.Count <= itemCount)
        return false;

      foreach (var item in this) {
        if (!otherSet.Contains(item))
          return false;
      }

      return true;
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      if (otherSet.Count < itemCount)
        return false;

      foreach (var item in this) {
        if (!otherSet.Contains(item))
          return false;
      }

      return true;
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      if (itemCount <= otherSet.Count)
        return false;

      foreach (var item in otherSet) {
        if (!Contains(item))
          return false;
      }

      return true;
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      if (itemCount < otherSet.Count)
        return false;

      foreach (var item in otherSet) {
        if (!Contains(item))
          return false;
      }

      return true;
    }

    public bool Overlaps(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      foreach (var item in otherSet) {
        if (Contains(item))
          return true;
      }

      return false;
    }

    public bool SetEquals(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");

      var otherSet = ToHashSet(other);

      if (itemCount != otherSet.Count)
        return false;

      foreach (var item in otherSet) {
        if (!Contains(item))
          return false;
      }

      return true;
    }

    private int itemCount;
    private int threshold;
    private int generation;
    private IEqualityComparer<T> comparer;
    private Bucket[] buckets;
  }
#endif
}
#endif