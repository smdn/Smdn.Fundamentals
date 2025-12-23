// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT || (SYSTEM_STRING_CREATE && SYSTEM_RUNTIME_COMPILERSERVICES_UNSAFE && SYSTEM_RUNTIME_INTEROPSERVICES_MEMORYMARSHAL)
#define SYSTEM_STRING_CREATE_WITH_STATE_OF_READONLYSPAN
#endif

using System;
#if SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT
#elif SYSTEM_STRING_CREATE_WITH_STATE_OF_READONLYSPAN
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#else
using System.Buffers;
#endif
using System.Text.Unicode;

namespace Smdn.Text.Unicode.ControlPictures;

public static class ReadOnlySpanExtensions {
  public static bool TryPicturizeControlChars(this ReadOnlySpan<byte> span, Span<char> destination)
  {
    if (destination.Length < span.Length)
      return false;

    for (var i = 0; i < span.Length; i++) {
      // replace control characters to control pictures
      destination[i] = span[i] switch {
        >= 0x00 and <= 0x20 => (char)(UnicodeRanges.ControlPictures.FirstCodePoint + span[i]), // CO Control chars + SP -> U+2400-U+2420
        0x7F => (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x21), // DEL -> U+2421 SYMBOL FOR DELETE
        0x85 => (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x24), // C1 NEL -> U+2424 SYMBOL FOR NEWLINE
        _ => (char)span[i],
      };
    }

    return true;
  }

  public static bool TryPicturizeControlChars(this ReadOnlySpan<char> span, Span<char> destination)
  {
    if (destination.Length < span.Length)
      return false;

    for (var i = 0; i < span.Length; i++) {
      if (char.IsSurrogate(span[i])) {
        destination[i] = span[i];
        continue;
      }

      // replace control characters to control pictures
      destination[i] = span[i] switch {
        >= (char)0x00 and <= (char)0x20 => (char)(UnicodeRanges.ControlPictures.FirstCodePoint + span[i]), // CO Control chars + SP -> U+2400-U+2420
        (char)0x7F => (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x21), // DEL -> U+2421 SYMBOL FOR DELETE
        (char)0x85 => (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x24), // C1 NEL -> U+2424 SYMBOL FOR NEWLINE
        _ => span[i],
      };
    }

    return true;
  }

  public static string ToControlCharsPicturizedString(this ReadOnlySpan<byte> span)
  {
    if (span.IsEmpty)
      return string.Empty;

#if SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT
    return string.Create(
      span.Length,
      span,
      static (destination, input) => _ = TryPicturizeControlChars(
        span: input,
        destination: destination
      )
    );
#elif SYSTEM_STRING_CREATE_WITH_STATE_OF_READONLYSPAN
    // ref: https://github.com/dotnet/runtime/issues/30175#issuecomment-1343179127
    unsafe {
      ref var refInput = ref MemoryMarshal.GetReference(span);
      var ptrInput = (IntPtr)Unsafe.AsPointer(ref refInput);

      return string.Create(
        span.Length,
        (Pointer: ptrInput, span.Length),
        static (destination, input) => _ = TryPicturizeControlChars(
          span: new ReadOnlySpan<byte>(
            pointer: input.Pointer.ToPointer(),
            length: input.Length
          ),
          destination: destination
        )
      );
    }
#else
    char[]? buffer = null;

    try {
      buffer = ArrayPool<char>.Shared.Rent(span.Length);

#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
      var buf = buffer.AsSpan(0, span.Length);

      TryPicturizeControlChars(span, buf);

      // string.Create(ReadOnlySpan) https://github.com/dotnet/runtime/issues/30175
      return new string(buf);
#else
      TryPicturizeControlChars(span, buffer.AsSpan(0, span.Length));

      return new string(buffer, 0, span.Length);
#endif
    }
    finally {
      if (buffer is not null)
        ArrayPool<char>.Shared.Return(buffer);
    }
#endif
  }

  public static string ToControlCharsPicturizedString(this ReadOnlySpan<char> span)
  {
    if (span.IsEmpty)
      return string.Empty;

#if SYSTEM_STRING_CREATE_OF_TSTATE_ALLOWS_REF_STRUCT
    return string.Create(
      span.Length,
      span,
      static (destination, input) => _ = TryPicturizeControlChars(
        span: input,
        destination: destination
      )
    );
#elif SYSTEM_STRING_CREATE_WITH_STATE_OF_READONLYSPAN
    // ref: https://github.com/dotnet/runtime/issues/30175#issuecomment-1343179127
    unsafe {
      ref var refInput = ref MemoryMarshal.GetReference(span);
      var ptrInput = (IntPtr)Unsafe.AsPointer(ref refInput);

      return string.Create(
        span.Length,
        (Pointer: ptrInput, span.Length),
        static (destination, input) => _ = TryPicturizeControlChars(
          span: new ReadOnlySpan<char>(
            pointer: input.Pointer.ToPointer(),
            length: input.Length
          ),
          destination: destination
        )
      );
    }
#else
    char[]? buffer = null;

    try {
      buffer = ArrayPool<char>.Shared.Rent(span.Length);

#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
      var buf = buffer.AsSpan(0, span.Length);

      TryPicturizeControlChars(span, buf);

      // string.Create(ReadOnlySpan) https://github.com/dotnet/runtime/issues/30175
      return new string(buf);
#else
      TryPicturizeControlChars(span, buffer.AsSpan(0, span.Length));

      return new string(buffer, 0, span.Length);
#endif
    }
    finally {
      if (buffer is not null)
        ArrayPool<char>.Shared.Return(buffer);
    }
#endif
  }
}
