// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text.RegularExpressions;

using NUnit.Framework;

namespace Smdn.Text.RegularExpressions {
  [TestFixture]
  public class RegexExtensionsTests {
    [Test]
    public void TestIsMatch_ArgumentNull()
    {
      Regex r = null!;
      Match m = null!;

      Assert.Throws<ArgumentNullException>(() => r.IsMatch("input", out m));

      Assert.That(m, Is.Null);
    }

    [Test]
    public void TestIsMatch_Success()
    {
      var r = new Regex("x+");

      Assert.That(r.IsMatch("yyyxxxxzzz", out Match m), Is.True);
      Assert.That(m, Is.Not.Null);
      Assert.That(m.Success, Is.True);
      Assert.That(m.Value, Is.EqualTo("xxxx"));
    }

    [Test]
    public void TestIsMatch_NotSuccess()
    {
      var r = new Regex("x+");

      Assert.That(r.IsMatch("yyyzzz", out Match m), Is.False);
      Assert.That(m, Is.Not.Null);
      Assert.That(m.Success, Is.False);
    }
  }
}
