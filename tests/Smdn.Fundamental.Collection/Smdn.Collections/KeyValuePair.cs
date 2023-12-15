// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class KeyValuePairTests {
    [Test]
    public void Test()
    {
      var pair = KeyValuePair.Create(42, "foo");

      Assert.That(pair.GetType(), Is.EqualTo(typeof(global::System.Collections.Generic.KeyValuePair<int, string>)));
      Assert.That(pair.Key, Is.EqualTo(42));
      Assert.That(pair.Value, Is.EqualTo("foo"));
    }
  }
}
