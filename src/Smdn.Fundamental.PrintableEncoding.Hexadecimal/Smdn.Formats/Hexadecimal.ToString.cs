// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_READONLYSPAN && SYSTEM_READONLYMEMORY && !SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT
using System.Buffers;
#endif

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
  public static string ToUpperCaseString(ArraySegment<byte> dataSequence)
  {
    if (dataSequence.Count == 0)
      return string.Empty;

#if SYSTEM_READONLYSPAN
    return ToUpperCaseString(dataSequence.AsSpan());
#else
    var destination = new char[dataSequence.Count * 2];

    TryEncodeUpperCase(dataSequence, new ArraySegment<char>(destination), out _);

    return new string(destination);
#endif // SYSTEM_READONLYSPAN
  }

#if SYSTEM_READONLYSPAN
  public static string ToUpperCaseString(ReadOnlySpan<byte> dataSequence)
  {
    if (dataSequence.Length == 0)
      return string.Empty;

#if SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT
    return string.Create(
      dataSequence.Length * 2,
      dataSequence,
      static (destination, sequence) => TryEncodeUpperCase(sequence, destination, out _)
    );
#else
    char[]? destination = null;

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

  public static string ToLowerCaseString(ArraySegment<byte> dataSequence)
  {
    if (dataSequence.Count == 0)
      return string.Empty;

#if SYSTEM_READONLYSPAN
    return ToLowerCaseString(dataSequence.AsSpan());
#else
    var destination = new char[dataSequence.Count * 2];

    TryEncodeLowerCase(dataSequence, new ArraySegment<char>(destination), out _);

    return new string(destination);
#endif
  }

#if SYSTEM_READONLYSPAN
  public static string ToLowerCaseString(ReadOnlySpan<byte> dataSequence)
  {
    if (dataSequence.Length == 0)
      return string.Empty;

#if SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT
    return string.Create(
      dataSequence.Length * 2,
      dataSequence,
      static (destination, sequence) => TryEncodeLowerCase(sequence, destination, out _)
    );
#else
    char[]? destination = null;

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
