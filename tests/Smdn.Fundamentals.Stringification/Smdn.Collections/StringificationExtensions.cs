// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

#if SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
using _KeyValuePair = System.Collections.Generic.KeyValuePair;
#else
using Smdn.Collections;
using _KeyValuePair = Smdn.Collections.KeyValuePair;
#endif

namespace Smdn.Collections {
  [TestFixture]
  public class StringificationExtensionsTests {
    [Test]
    public void Stringify_KeyValuePair()
    {
      Assert.AreEqual("{key => value}", _KeyValuePair.Create("key", "value").Stringify());
      Assert.AreEqual("{key => 0}", _KeyValuePair.Create("key", 0).Stringify());
      Assert.AreEqual("{0 => value}", _KeyValuePair.Create(0, "value").Stringify());
      Assert.AreEqual("{ => }", _KeyValuePair.Create(string.Empty, string.Empty).Stringify());
      Assert.AreEqual("{ => }", _KeyValuePair.Create((string)null, (string)null).Stringify());
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_Single()
    {
      var pairs = new[] {_KeyValuePair.Create("key", "value")};

      Assert.AreEqual("{key => value}", pairs.Stringify());
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_Multiple()
    {
      var pairs = new[] {
        _KeyValuePair.Create("key1", "value1"),
        _KeyValuePair.Create("key2", "value2"),
      };

      Assert.AreEqual("{key1 => value1}, {key2 => value2}", pairs.Stringify());
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_KeyOrValueNull()
    {
      var pairs = new[] {
        _KeyValuePair.Create("key1", (string)null),
        _KeyValuePair.Create((string)null, "value2"),
        _KeyValuePair.Create((string)null, (string)null),
      };

      Assert.AreEqual("{key1 => }, { => value2}, { => }", pairs.Stringify());
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_ArgumentEmpty()
    {
      var pairs = new KeyValuePair<string, string>[] { };

      Assert.IsEmpty(pairs.Stringify());
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_ArgumentNull()
    {
      IEnumerable<KeyValuePair<string, string>> pairs = null;

      Assert.IsNull(pairs.Stringify());
    }
  }
}