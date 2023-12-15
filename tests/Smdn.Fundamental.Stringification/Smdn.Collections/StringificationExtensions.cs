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
      Assert.That(_KeyValuePair.Create("key", "value").Stringify(), Is.EqualTo("{key => value}"));
      Assert.That(_KeyValuePair.Create("key", 0).Stringify(), Is.EqualTo("{key => 0}"));
      Assert.That(_KeyValuePair.Create(0, "value").Stringify(), Is.EqualTo("{0 => value}"));
      Assert.That(_KeyValuePair.Create(string.Empty, string.Empty).Stringify(), Is.EqualTo("{ => }"));
      Assert.That(_KeyValuePair.Create((string)null, (string)null).Stringify(), Is.EqualTo("{ => }"));
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_Single()
    {
      var pairs = new[] {_KeyValuePair.Create("key", "value")};

      Assert.That(pairs.Stringify(), Is.EqualTo("{key => value}"));
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_Multiple()
    {
      var pairs = new[] {
        _KeyValuePair.Create("key1", "value1"),
        _KeyValuePair.Create("key2", "value2"),
      };

      Assert.That(pairs.Stringify(), Is.EqualTo("{key1 => value1}, {key2 => value2}"));
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_KeyOrValueNull()
    {
      var pairs = new[] {
        _KeyValuePair.Create("key1", (string)null!),
        _KeyValuePair.Create((string)null!, "value2"),
        _KeyValuePair.Create((string)null!, (string)null!),
      };

      Assert.That(pairs.Stringify(), Is.EqualTo("{key1 => }, { => value2}, { => }"));
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_ArgumentEmpty()
    {
      var pairs = new KeyValuePair<string, string>[] { };

      Assert.That(pairs.Stringify(), Is.Empty);
    }

    [Test]
    public void Stringify_IEnumerableOfKeyValuePair_ArgumentNull()
    {
      IEnumerable<KeyValuePair<string, string>> pairs = null;

      Assert.That(pairs.Stringify(), Is.Null);
    }
  }
}
