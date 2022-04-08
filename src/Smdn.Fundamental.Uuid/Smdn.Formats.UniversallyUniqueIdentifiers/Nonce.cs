// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

internal static class Nonce {
#if !SYSTEM_SECURITY_CRYPTOGRAPHY_RANDOMNUMBERGENERATOR_FILL
  private static readonly RandomNumberGenerator defaultRng = RandomNumberGenerator.Create();
#endif

  public static void Fill(Span<byte> span) =>
#if SYSTEM_SECURITY_CRYPTOGRAPHY_RANDOMNUMBERGENERATOR_FILL
    RandomNumberGenerator.Fill(span);
#else
    Fill(span, defaultRng);
#endif

  public static void Fill(Span<byte> span, RandomNumberGenerator rng)
  {
#if SYSTEM_SECURITY_CRYPTOGRAPHY_RANDOMNUMBERGENERATOR_GETBYTES_SPAN_OF_BYTE
    rng.GetBytes(span);
#else
    var buffer = new byte[span.Length];

    rng.GetBytes(buffer);

    buffer.CopyTo(span);
#endif
  }
}
