// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class ReadOnlyDictionaryTests {
    [Test]
    public void TestEmpty()
    {
      var empty = ReadOnlyDictionary<string, string>.Empty;

      Assert.That(empty.Count, Is.EqualTo(0));
    }
  }
}
