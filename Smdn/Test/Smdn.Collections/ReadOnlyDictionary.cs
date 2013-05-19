using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

#if NET_4_5
using StringDictionary = System.Collections.ObjectModel.ReadOnlyDictionary<string, string>;
#else
using StringDictionary = Smdn.Collections.ReadOnlyDictionary<string, string>;
#endif

namespace Smdn.Collections {
  [TestFixture]
  public class ReadOnlyDictionaryTests {
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

#if false
    [Test]
    public void TestConstructFromKeyValuePairs()
    {
      var dic = new ReadOnlyDictionary<string, string>(new[] {
        new KeyValuePair<string, string>("key1", "val1"),
        new KeyValuePair<string, string>("key2", "val2"),
        new KeyValuePair<string, string>("key3", "val3"),
      });

      Assert.IsTrue((dic as IDictionary).IsReadOnly);
      Assert.AreEqual("val1", dic["key1"]);

      try {
        dic.Add("newkey", "newvalue");
        Assert.Fail("NotSupportedException");
      }
      catch (NotSupportedException) {
      }
    }

    [Test]
    public void TestConstructFromKeyValuePairsWithSpecifiedEqualityComaperer()
    {
      var dic = new ReadOnlyDictionary<string, string>(new[] {
        new KeyValuePair<string, string>("key1", "val1"),
        new KeyValuePair<string, string>("key2", "val2"),
        new KeyValuePair<string, string>("key3", "val3"),
      }, StringComparer.OrdinalIgnoreCase);

      Assert.IsTrue((dic as IDictionary).IsReadOnly);
      Assert.AreEqual("val1", dic["key1"]);
      Assert.AreEqual("val1", dic["Key1"]);
      Assert.AreEqual("val1", dic["KEY1"]);

      try {
        dic.Add("newkey", "newvalue");
        Assert.Fail("NotSupportedException");
      }
      catch (NotSupportedException) {
      }
    }
#endif

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

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestClear1()
    {
      CreateReadOnly().Clear();
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestClear2()
    {
      (CreateReadOnly() as System.Collections.IDictionary).Clear();
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestAdd1()
    {
      CreateReadOnly().Add("key", "val");
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestAdd2()
    {
      CreateReadOnly().Add(new KeyValuePair<string, string>("key", "val"));
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestAdd3()
    {
      (CreateReadOnly() as System.Collections.IDictionary).Add("key", "val");
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestRemove1()
    {
      CreateReadOnly().Remove("key1");
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestRemove2()
    {
      CreateReadOnly().Remove(new KeyValuePair<string, string>("key1", "val1"));
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestRemove3()
    {
      (CreateReadOnly() as System.Collections.IDictionary).Remove("key");
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestIndexer1()
    {
      CreateReadOnly()["key4"] = "val4";
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestIndexer2()
    {
      (CreateReadOnly() as System.Collections.IDictionary)["key4"] = "val4";
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
