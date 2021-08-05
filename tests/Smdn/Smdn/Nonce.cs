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

      Assert.AreEqual(16, bytes.Length);
      Assert.AreNotEqual(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0}, bytes);
    }

    [Test]
    public void TestGetRandomBytes2()
    {
      var bytes = new byte[16];

      Nonce.GetRandomBytes(bytes);

      Assert.AreNotEqual(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0}, bytes);
    }
  }
}