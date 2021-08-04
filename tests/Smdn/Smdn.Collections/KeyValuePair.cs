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

      Assert.AreEqual(typeof(global::System.Collections.Generic.KeyValuePair<int, string>), pair.GetType());
      Assert.AreEqual(42, pair.Key);
      Assert.AreEqual("foo", pair.Value);
    }
  }
}
