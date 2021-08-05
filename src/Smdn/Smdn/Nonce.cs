// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;
using System.Threading;

namespace Smdn {
  public static class Nonce {
    public static byte[] GetRandomBytes(int length)
    {
      var bytes = new byte[length];

      GetRandomBytes(bytes);

      return bytes;
    }

    private static RandomNumberGenerator defaultRng = null;

    public static void GetRandomBytes(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      LazyInitializer.EnsureInitialized(ref defaultRng, () => RandomNumberGenerator.Create());

      lock (defaultRng) {
        defaultRng.GetBytes(bytes);
      }
    }
  }
}
