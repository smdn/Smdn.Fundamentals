using System;
using System.Collections.Generic;
using Smdn.Collections;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class ConvertUtilsTests {
    [Test]
    public void TestToJoinedStringArgumentSingle()
    {
      var pairs = new[] {KeyValuePair.Create("key", "value")};

      Assert.AreEqual("{key => value}", ConvertUtils.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringArgumentMultiple()
    {
      var pairs = new[] {
        KeyValuePair.Create("key1", "value1"),
        KeyValuePair.Create("key2", "value2"),
      };

      Assert.AreEqual("{key1 => value1}, {key2 => value2}", ConvertUtils.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringKeyOrValueNull()
    {
      var pairs = new[] {
        KeyValuePair.Create("key1", (string)null),
        KeyValuePair.Create((string)null, "value2"),
        KeyValuePair.Create((string)null, (string)null),
      };

      Assert.AreEqual("{key1 => }, { => value2}, { => }", ConvertUtils.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringArgumentEmpty()
    {
      var pairs = new KeyValuePair<string, string>[] { };

      Assert.IsEmpty(ConvertUtils.ToJoinedString(pairs));
    }

    [Test]
    public void TestToJoinedStringArgumentNull()
    {
      IEnumerable<KeyValuePair<string, string>> pairs = null;

      Assert.IsNull(ConvertUtils.ToJoinedString(pairs));
    }
  }
}