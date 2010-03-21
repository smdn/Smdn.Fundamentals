using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class IDictionaryExtensionsTests {
    [Test]
    public void TestAsReadOnly()
    {
      var dic = (new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      }).AsReadOnly();

      Assert.IsTrue(dic.IsReadOnly);

      try {
        dic.Add("newkey", "newvalue");
        Assert.Fail("NotSupportedException not thrown");
      }
      catch (NotSupportedException) {
      }
    }

    [Test]
    public void TestAsReadOnlyWithSpecifiedComparer()
    {
      var dic = (new Dictionary<string, string>() {
        {"key1", "val1"},
        {"key2", "val2"},
        {"key3", "val3"},
      }).AsReadOnly(StringComparer.OrdinalIgnoreCase);

      Assert.IsTrue(dic.IsReadOnly);
      Assert.AreEqual("val1", dic["key1"]);
      Assert.AreEqual("val1", dic["Key1"]);
      Assert.AreEqual("val1", dic["KEY1"]);

      try {
        dic.Add("newkey", "newvalue");
        Assert.Fail("NotSupportedException not thrown");
      }
      catch (NotSupportedException) {
      }
    }
  }
}