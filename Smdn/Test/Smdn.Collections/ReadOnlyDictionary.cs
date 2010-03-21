using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class ReadOnlyDictionaryTests {
    [Test]
    public void TestConstruct()
    {
      var dic = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      });

      Assert.IsTrue(dic.IsReadOnly);
    }

    [Test]
    public void TestConstructBaseDictionaryChanged()
    {
      var basedic = new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      };
      var dic = new ReadOnlyDictionary<string, string>(basedic);

      Assert.IsTrue(dic.IsReadOnly);
      Assert.IsTrue(dic.ContainsKey("key1"));
      Assert.IsFalse(dic.ContainsKey("key4"));

      basedic.Add("key4", "val4");

      Assert.IsTrue(dic.ContainsKey("key4"));
    }

    [Test]
    public void TestConstructFromKeyValuePairs()
    {
      var dic = new ReadOnlyDictionary<string, string>(new[] {
        new KeyValuePair<string, string>("key1", "val1"),
        new KeyValuePair<string, string>("key2", "val2"),
        new KeyValuePair<string, string>("key3", "val3"),
      });

      Assert.IsTrue(dic.IsReadOnly);
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

      Assert.IsTrue(dic.IsReadOnly);
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

    [Test]
    public void TestReadOperations()
    {
      var dic = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      });

      Assert.AreEqual(3, dic.Count);

      Assert.AreEqual("val1", dic["key1"]);
      Assert.IsTrue(dic.Contains(new KeyValuePair<string, string>("key2", "val2")));
      Assert.IsTrue(dic.ContainsKey("key3"));

      foreach (var pair in dic) {
      }

      foreach (var key in dic.Keys) {
      }

      foreach (var val in dic.Values) {
      }

      var pairs = new KeyValuePair<string, string>[3];

      dic.CopyTo(pairs, 0);

      string outval = null;

      Assert.IsTrue(dic.TryGetValue("key1", out outval));
      Assert.AreEqual(outval, "val1");
    }

    private IDictionary<string, string> CreateReadOnly()
    {
      return new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      });
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestClear()
    {
      CreateReadOnly().Clear();
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
    public void TestRemove1()
    {
      CreateReadOnly().Remove("key1");
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestRemove2()
    {
      CreateReadOnly().Remove(new KeyValuePair<string, string>("key1", "val1"));
    }
  }
}
