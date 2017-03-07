using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

using StringDictionary = System.Collections.ObjectModel.ReadOnlyDictionary<string, string>;

namespace Smdn.Collections {
  [TestFixture]
  public class ReadOnlyDictionaryTests {
    [Test]
    public void TestEmpty()
    {
      var empty = Smdn.Collections.ReadOnlyDictionary<string, string>.Empty;

      Assert.AreEqual(0, empty.Count);
    }

    [Test]
    public void TestConstruct()
    {
      var dic = new StringDictionary(new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      });

      Assert.IsTrue((dic as IDictionary).IsReadOnly);
    }

    [Test]
    public void TestConstructBaseDictionaryChanged()
    {
      var basedic = new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      };
      var dic = new StringDictionary(basedic);

      Assert.IsTrue((dic as IDictionary).IsReadOnly);
      Assert.IsTrue(dic.ContainsKey("key1"));
      Assert.IsFalse(dic.ContainsKey("key4"));

      basedic.Add("key4", "val4");

      Assert.IsTrue(dic.ContainsKey("key4"));
    }

    [Test]
    public void TestReadOperations()
    {
      var dic = new StringDictionary(new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      });

      Assert.AreEqual(3, dic.Count, "Count");

      Assert.AreEqual("val1", dic["key1"], "indexer");
      Assert.IsTrue(dic.ContainsKey("key2"), "ContainsKey");

      var count = 0;

      foreach (var pair in dic) {
        count++;
      }

      Assert.AreEqual(3, count, "GetEnumerator");

      var pairs = new KeyValuePair<string, string>[3];

      (dic as ICollection<KeyValuePair<string, string>>).CopyTo(pairs, 0);

      string outval = null;

      Assert.IsTrue(dic.TryGetValue("key1", out outval), "TryGetValue");
      Assert.AreEqual(outval, "val1", "TryGetValue out value");
    }

    private IDictionary<string, string> CreateReadOnly()
    {
      return new StringDictionary(new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      });
    }

    [Test]
    public void TestClear1()
    {
      var dic = CreateReadOnly();

      Assert.Throws<NotSupportedException>(() => dic.Clear());
    }

    [Test]
    public void TestClear2()
    {
      var dic = CreateReadOnly() as System.Collections.IDictionary;

      Assert.Throws<NotSupportedException>(() => dic.Clear());
    }

    [Test]
    public void TestAdd1()
    {
      var dic = CreateReadOnly();

      Assert.Throws<NotSupportedException>(() => dic.Add("key", "val"));
    }

    [Test]
    public void TestAdd2()
    {
      var dic = CreateReadOnly();

      Assert.Throws<NotSupportedException>(() => dic.Add(new KeyValuePair<string, string>("key", "val")));
    }

    [Test]
    public void TestAdd3()
    {
      var dic = CreateReadOnly() as System.Collections.IDictionary;

      Assert.Throws<NotSupportedException>(() => dic.Add("key", "val"));
    }

    [Test]
    public void TestRemove1()
    {
      var dic = CreateReadOnly();

      Assert.Throws<NotSupportedException>(() => dic.Remove("key1"));
    }

    [Test]
    public void TestRemove2()
    {
      var dic = CreateReadOnly();

      Assert.Throws<NotSupportedException>(() => dic.Remove(new KeyValuePair<string, string>("key1", "val1")));
    }

    [Test]
    public void TestRemove3()
    {
      var dic = CreateReadOnly() as System.Collections.IDictionary;

      Assert.Throws<NotSupportedException>(() => dic.Remove("key"));
    }

    [Test]
    public void TestIndexer1()
    {
      var dic = CreateReadOnly();

      Assert.Throws<NotSupportedException>(() => dic["key4"] = "val4");
    }

    [Test]
    public void TestIndexer2()
    {
      var dic = CreateReadOnly() as System.Collections.IDictionary;

      Assert.Throws<NotSupportedException>(() => dic["key4"] = "val4");
    }

    [Test]
    public void TestKeyCollection()
    {
      var keys = CreateReadOnly().Keys;

      Assert.AreEqual(3, keys.Count, "Count");
      Assert.IsTrue(keys.IsReadOnly, "IsReadOnly");
      Assert.IsTrue(keys.Contains("key1"), "Contains");

      var keyArray = new string[3];

      keys.CopyTo(keyArray, 0);

      CollectionAssert.AreEqual(keys, keyArray, "CopyTo");

      var count = 0;

      foreach (var e in keys) {
        count++;
      }

      Assert.AreEqual(3, count, "GetEnumerator");

      Assert.Throws<NotSupportedException>(() => keys.Clear(), "Clear");
      Assert.Throws<NotSupportedException>(() => keys.Add("foo"), "Add");
      Assert.Throws<NotSupportedException>(() => keys.Remove("foo"), "Remove");
    }

    [Test]
    public void TestValueCollection()
    {
      var values = CreateReadOnly().Values;

      Assert.AreEqual(3, values.Count, "Count");
      Assert.IsTrue(values.IsReadOnly, "IsReadOnly");
      Assert.IsTrue(values.Contains("val1"), "Contains");

      var valArray = new string[3];

      values.CopyTo(valArray, 0);

      CollectionAssert.AreEqual(values, valArray, "CopyTo");

      var count = 0;

      foreach (var e in values) {
        count++;
      }

      Assert.AreEqual(3, count, "GetEnumerator");

      Assert.Throws<NotSupportedException>(() => values.Clear(), "Clear");
      Assert.Throws<NotSupportedException>(() => values.Add("foo"), "Add");
      Assert.Throws<NotSupportedException>(() => values.Remove("foo"), "Remove");
    }
  }
}
