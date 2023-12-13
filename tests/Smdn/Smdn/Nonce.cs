// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class NonceTests {
    [Test]
    public void TestGetRandomBytes1()
    {
      var bytes = Nonce.GetRandomBytes(16);

      Assert.That(bytes.Length, Is.EqualTo(16));
      Assert.That(bytes, Is.Not.EqualTo(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0}));
    }

    [Test]
    public void TestGetRandomBytes2()
    {
      var bytes = new byte[16];

      Nonce.GetRandomBytes(bytes);

      Assert.That(bytes, Is.Not.EqualTo(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0}));
    }
  }
}