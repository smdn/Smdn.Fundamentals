using System;
using System.Collections;
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

      Assert.IsTrue((dic as IDictionary).IsReadOnly);

      try {
        (dic as IDictionary<string, string>).Add("newkey", "newvalue");
        Assert.Fail("NotSupportedException not thrown");
      }
      catch (NotSupportedException) {
      }

      var dic2 = dic.AsReadOnly();

      Assert.AreNotSame(dic2, dic);
      Assert.IsTrue((dic2 as IDictionary).IsReadOnly);
    }
  }
}