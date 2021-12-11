// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  internal static class Nonce {
#if !(NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
    private static readonly RandomNumberGenerator defaultRng = RandomNumberGenerator.Create();
#endif

    public static void Fill(Span<byte> span) =>
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
      RandomNumberGenerator.Fill(span);
#else
      Fill(span, defaultRng);
#endif

    public static void Fill(Span<byte> span, RandomNumberGenerator rng)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
      rng.GetBytes(span);
#else
      var buffer = new byte[span.Length];

      rng.GetBytes(buffer);

      buffer.CopyTo(span);
#endif
    }
  }
}
