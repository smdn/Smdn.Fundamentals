// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_TEXT_ENCODING_ENCODERFALLBACK && SYSTEM_BUFFERS_ARRAYPOOL
using System.Buffers;
#endif
using System.Text;

namespace Smdn.Text.Encodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class OctetEncoding : Encoding {
  // for string which contains 7-bit characters
  public static readonly Encoding SevenBits = Create(7);

  // for string which contains 8-bit characters
  public static readonly Encoding EightBits = Create(8);

#pragma warning disable CA1859
  private static Encoding Create(int bit)
  {
#if SYSTEM_TEXT_ENCODING_CTOR_ENCODERFALLBACK_DECODERFALLBACK
    return new OctetEncoding(bit);
#elif SYSTEM_TEXT_ENCODING_DECODERFALLBACK && SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
    var e = (Encoding)new OctetEncoding(bit).Clone();

    e.DecoderFallback = new DecoderExceptionFallback();
    e.EncoderFallback = new EncoderExceptionFallback();

    return e;
#else
    return new OctetEncoding(bit, encoderReplacement: null /* throw exception */);
#endif
  }
#pragma warning restore CA1859

  private static char ValidateMaxValue(int bits, string nameOfBitsParameter)
  {
    if (bits is < 1 or > 8)
      throw ExceptionUtils.CreateArgumentMustBeInRange(1, 8, nameOfBitsParameter, bits);

    return (char)(1 << bits);
  }

#if SYSTEM_TEXT_ENCODING_CTOR_ENCODERFALLBACK_DECODERFALLBACK
  public OctetEncoding(int bits)
    : this(bits, new EncoderExceptionFallback(), new DecoderExceptionFallback())
  {
  }

  public OctetEncoding(int bits, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
    : base(0, encoderFallback, decoderFallback)
  {
    maxValue = ValidateMaxValue(bits, nameof(bits));
  }
#elif SYSTEM_TEXT_ENCODING_DECODERFALLBACK && SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
  public OctetEncoding(int bits)
  {
    maxValue = ValidateMaxValue(bits, nameof(bits));
  }
#else
  public OctetEncoding(int bits)
    : this(bits, encoderReplacement: null)
  {
  }

  // public
  private OctetEncoding(int bits, byte? encoderReplacement)
  {
    maxValue = ValidateMaxValue(bits, nameof(bits));
    this.encoderReplacement = encoderReplacement;
  }
#endif

  public override int GetMaxCharCount(int byteCount)
  {
    if (byteCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(byteCount), byteCount);

    return byteCount;
  }

  public override int GetMaxByteCount(int charCount)
  {
    if (charCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(charCount), charCount);

    return charCount;
  }

  public override int GetByteCount(string s)
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
    => GetByteCount((s ?? throw new ArgumentNullException(nameof(s))).AsSpan());
#else
    => GetByteCount((s ?? throw new ArgumentNullException(nameof(s))).ToCharArray());
#endif

  public override int GetByteCount(char[] chars)
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
    => GetByteCount((chars ?? throw new ArgumentNullException(nameof(chars))).AsSpan());
#else
    => GetByteCount(
      chars ?? throw new ArgumentNullException(nameof(chars)),
      index: 0,
      count: chars.Length
    );
#endif

#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
  public override int GetByteCount(char[] chars, int index, int count)
    => GetByteCount(
      (chars ?? throw new ArgumentNullException(nameof(chars))).AsSpan(index, count)
    );

  public override int GetByteCount(ReadOnlySpan<char> chars)
#else
  public override int GetByteCount(char[] chars, int index, int count)
#endif
  {
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
    var index = 0;
    var count = chars.Length;
#else
    if (chars is null)
      throw new ArgumentNullException(nameof(chars));
    if (index < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
#endif

#if SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
    EncoderFallbackBuffer? buffer = null;
    var byteCount = 0;

    for (var i = 0; i < count; i++) {
      if (chars[index] < maxValue) {
        byteCount++;
        index++;
      }
      else {
        if (buffer == null)
          buffer = EncoderFallback.CreateFallbackBuffer();
        else
          buffer.Reset();

        var (fallback, isSurrogatePair) =
          2 <= (count - index) && char.IsSurrogatePair(chars[index], chars[index + 1])
            ? (buffer.Fallback(chars[index], chars[index + 1], index), true)
            : char.IsSurrogate(chars[index])
              ? (false, false)
              : (buffer.Fallback(chars[index], index), false);

        if (fallback) {
          byteCount += buffer.Remaining;
          index += isSurrogatePair ? 2 : 1;
          i += isSurrogatePair ? 1 : 0;
        }
        else {
          byteCount++;
          index++;
        }
      }
    }

    return byteCount;
#else
    if (encoderReplacement.HasValue)
      return count;

    var byteCount = 0;

    for (var i = 0; i < count; i++) {
      if (chars[index++] < maxValue)
        byteCount++;
      else
        throw new EncoderFallbackException();
    }

    return byteCount;
#endif
  }

  public override int GetCharCount(byte[] bytes)
#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
    => GetCharCount((bytes ?? throw new ArgumentNullException(nameof(bytes))).AsSpan());
#else
    => GetCharCount(
      bytes: bytes ?? throw new ArgumentNullException(nameof(bytes)),
      index: 0,
      count: bytes.Length
    );
#endif

  public override int GetCharCount(byte[] bytes, int index, int count)
#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
    => GetCharCount((bytes ?? throw new ArgumentNullException(nameof(bytes))).AsSpan(index, count));
#else
  {
    if (bytes is null)
      throw new ArgumentNullException(nameof(bytes));
    if (index < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);

    return count;
  }
#endif

#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
  public override int GetCharCount(ReadOnlySpan<byte> bytes)
    => bytes.Length;
#endif

#if false
  public override byte[] GetBytes(string s)
    => base.GetBytes(s);

  public override byte[] GetBytes(char[] chars)
    => GetBytes(
      chars: chars ?? throw new ArgumentNullException(nameof(chars)),
      index: 0,
      count: chars.Length
    );

  public override byte[] GetBytes(char[] chars, int index, int count)
    => base.GetBytes(chars, index, count);
#endif

  public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
    => GetBytes(
      chars: (s ?? throw new ArgumentNullException(nameof(s))).AsSpan(charIndex, charCount),
      bytes: (bytes ?? throw new ArgumentNullException(nameof(bytes))).AsSpan(byteIndex)
    );
#else
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));
    if (bytes is null)
      throw new ArgumentNullException(nameof(bytes));

    var chars = s.ToCharArray(charIndex, charCount);

    return GetBytes(
      chars: chars,
      charIndex: 0,
      charCount: chars.Length,
      bytes: bytes,
      byteIndex: byteIndex
    );
  }
#endif

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    => GetBytes(
      (chars ?? throw new ArgumentNullException(nameof(chars))).AsSpan(charIndex, charCount),
      (bytes ?? throw new ArgumentNullException(nameof(bytes))).AsSpan(byteIndex)
    );

  public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
#else
  public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
#endif
  {
#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
    var charIndex = 0;
    var charCount = chars.Length;
    var byteIndex = 0;
#else
    if (chars == null)
      throw new ArgumentNullException(nameof(chars));
    if (bytes == null)
      throw new ArgumentNullException(nameof(chars));
    if (charIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(charIndex), charIndex);
    if (charCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(charCount), charCount);
    if (chars.Length - charCount < charIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(charIndex), chars, charIndex, charCount);
    if (byteIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(byteIndex), byteIndex);
#endif

#if SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
    EncoderFallbackBuffer? buffer = null;
#endif
    var byteCount = 0;

    for (var i = 0; i < charCount; i++) {
      if (chars[charIndex] < maxValue) {
        bytes[byteIndex++] = (byte)chars[charIndex++];
        byteCount++;
      }
      else {
#if SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
        if (buffer is null)
          buffer = EncoderFallback.CreateFallbackBuffer();
        else
          buffer.Reset();

        var (fallback, isSurrogatePair) =
          2 <= (charCount - charIndex) && char.IsSurrogatePair(chars[charIndex], chars[charIndex + 1])
            ? (buffer.Fallback(chars[charIndex], chars[charIndex + 1], charIndex), true)
            : char.IsSurrogate(chars[charIndex])
              ? (false, false)
              : (buffer.Fallback(chars[charIndex], charIndex), false);

        if (fallback) {
          var fallbackCharsLength = buffer.Remaining;
          var fallbackChars =
#if SYSTEM_BUFFERS_ARRAYPOOL
            ArrayPool<char>.Shared.Rent(fallbackCharsLength);
#else
            new char[fallbackCharsLength];
#endif

          try {
            for (var r = 0; r < fallbackCharsLength; r++) {
              var fallbackChar = buffer.GetNextChar();

              if (maxValue <= fallbackChar)
                throw new EncoderFallbackException($"The value of fallback character must be less than 'U+{(int)maxValue:X4}'.");

              fallbackChars[r] = fallbackChar;
            }

            var c =
#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
              GetBytes(fallbackChars.AsSpan(0, fallbackCharsLength), bytes.Slice(byteIndex));
#else
              GetBytes(fallbackChars, 0, fallbackCharsLength, bytes, byteIndex);
#endif

            byteIndex += c;
            byteCount += c;

            charIndex += isSurrogatePair ? 2 : 1;
            i += isSurrogatePair ? 1 : 0;
          }
          finally {
#if SYSTEM_BUFFERS_ARRAYPOOL
            ArrayPool<char>.Shared.Return(fallbackChars);
#endif
          }
        }
        else {
          bytes[byteIndex++] = (byte)'?';
          charIndex++;
          byteCount++;
        }
#else
        if (encoderReplacement.HasValue) {
          bytes[byteIndex++] = encoderReplacement.Value;
          charIndex++;
          byteCount++;
        }
        else {
          throw new EncoderFallbackException();
        }
#endif
      }
    }

    return byteCount;
  }

  public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof(bytes));
    if (chars == null)
      throw new ArgumentNullException(nameof(chars));
    if (byteIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(byteIndex), byteIndex);
    if (byteCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(byteCount), byteCount);
    if (bytes.Length - byteCount < byteIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(byteIndex), bytes, byteIndex, byteCount);
    if (charIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(charIndex), charIndex);

    var charCount = 0;

    for (var i = 0; i < byteCount; i++, byteIndex++, charIndex++, charCount++) {
      chars[charIndex] = (char)bytes[byteIndex];
    }

    return charCount;
  }

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
  public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
  {
    // TODO: vectorize
    for (var i = 0; i < bytes.Length; i++) {
      chars[i] = (char)bytes[i];
    }

    return bytes.Length;
  }
#endif

  private readonly char maxValue;
#if !SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
  private readonly byte? encoderReplacement;
#endif
}
