// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_READONLYSPAN && SYSTEM_READONLYMEMORY
using System.Buffers;
#endif

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040

#if SYSTEM_READONLYSPAN
  public static string ToUpperCaseString(ReadOnlySpan<byte> dataSequence)
  {
#if false && SYSTEM_STRING_CREATE
    // XXX: string.Create does not accept ReadOnlySpan<T>, dotnet/runtime#30175
    return string.Create(
      dataSequence.Length * 2,
      dataSequence,
      static (destination, sequence) => TryEncodeUpperCase(sequence, destination, out _)
    );
#else
    char[] destination = null;

    try {
      var length = dataSequence.Length * 2;

      destination = ArrayPool<char>.Shared.Rent(length);

      TryEncodeUpperCase(dataSequence, destination.AsSpan(0, length), out _);

#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
      return new string(destination.AsSpan(0, length));
#else
      return new string(destination, 0, length);
#endif
    }
    finally {
      if (destination is not null)
        ArrayPool<char>.Shared.Return(destination);
    }
#endif
  }
#endif

#if SYSTEM_READONLYSPAN
  public static string ToLowerCaseString(ReadOnlySpan<byte> dataSequence)
  {
#if false && SYSTEM_STRING_CREATE
    // XXX: string.Create does not accept ReadOnlySpan<T>, dotnet/runtime#30175
    return string.Create(
      dataSequence.Length * 2,
      dataSequence,
      static (destination, sequence) => TryEncodeLowerCase(sequence, destination, out _)
    );
#else
    char[] destination = null;

    try {
      var length = dataSequence.Length * 2;

      destination = ArrayPool<char>.Shared.Rent(length);

      TryEncodeLowerCase(dataSequence, destination.AsSpan(0, length), out _);

#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
      return new string(destination.AsSpan(0, length));
#else
      return new string(destination, 0, length);
#endif
    }
    finally {
      if (destination is not null)
        ArrayPool<char>.Shared.Return(destination);
    }
#endif
  }
#endif
}
