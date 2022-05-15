// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET20_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_TEXT_ENCODING_DECODERFALLBACK
#define SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
#endif

using System;
using System.Text;

namespace Smdn.Text.Encodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class OctetEncoding : Encoding {
  // for string which contains 7-bit characters
  public static readonly Encoding SevenBits;

  // for string which contains 8-bit characters
  public static readonly Encoding EightBits;

  static OctetEncoding()
  {
#if SYSTEM_TEXT_ENCODING_CTOR_ENCODERFALLBACK_DECODERFALLBACK
    SevenBits = new OctetEncoding(7);
    EightBits = new OctetEncoding(8);
#elif SYSTEM_TEXT_ENCODING_DECODERFALLBACK && SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
    SevenBits = (Encoding)new OctetEncoding(7).Clone();
    SevenBits.DecoderFallback = new DecoderExceptionFallback();
    SevenBits.EncoderFallback = new EncoderExceptionFallback();

    EightBits = (Encoding)new OctetEncoding(8).Clone();
    EightBits.DecoderFallback = new DecoderExceptionFallback();
    EightBits.EncoderFallback = new EncoderExceptionFallback();
#else
    SevenBits = new OctetEncoding(7, encoderReplacement: null /* throw exception */);
    EightBits = new OctetEncoding(8, encoderReplacement: null /* throw exception */);
#endif
  }

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

  /*public*/ private OctetEncoding(int bits, byte? encoderReplacement)
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

  public override int GetByteCount(char[] chars, int index, int count)
  {
    if (index < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);

#if SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
    if (EncoderFallback == null)
      return count - index;

    EncoderFallbackBuffer? buffer = null;
    var byteCount = 0;

    for (; index < count; index++) {
      if (chars[index] < maxValue) {
        byteCount++;
      }
      else {
        if (buffer == null)
          buffer = EncoderFallback.CreateFallbackBuffer();

        buffer.Fallback(chars[index], index);

        var fallbackChars = new char[buffer.Remaining];

        for (var r = 0; r < fallbackChars.Length; r++) {
          fallbackChars[r] = buffer.GetNextChar();
        }

        byteCount += GetByteCount(fallbackChars, 0, fallbackChars.Length);
      }
    }

    return byteCount;
#else
    if (encoderReplacement.HasValue)
      return count - index;

    var byteCount = 0;

    for (; index < count; index++) {
      if (chars[index] < maxValue)
        byteCount++;
      else
        throw new EncoderFallbackException();
    }

    return byteCount;
#endif
  }

  public override int GetCharCount(byte[] bytes, int index, int count)
  {
    if (index < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);

    return count;
  }

  public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
  {
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

    var byteCount = 0;

    for (var i = 0; i < charCount; i++, charIndex++, byteIndex++, byteCount++) {
      if (chars[charIndex] < maxValue) {
        bytes[byteIndex] = (byte)chars[charIndex];
        byteCount++;
      }
      else {
#if SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
        if (EncoderFallback == null) {
          bytes[byteIndex] = (byte)'?';
          byteCount++;
        }
        else {
          var buffer = EncoderFallback.CreateFallbackBuffer();

          if (buffer.Fallback(chars[charIndex], charIndex)) {
            var fallbackChars = new char[buffer.Remaining];

            for (var r = 0; r < fallbackChars.Length; r++) {
              fallbackChars[r] = buffer.GetNextChar();
            }

            var c = GetBytes(fallbackChars, 0, fallbackChars.Length, bytes, byteIndex);

            byteIndex += c - 1;
            byteCount += c;
          }
          else {
            bytes[byteIndex] = (byte)'?';
            byteCount++;
          }
        }
#else
        if (encoderReplacement.HasValue) {
          bytes[byteIndex] = encoderReplacement.Value;
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

  private readonly char maxValue;
#if !SYSTEM_TEXT_ENCODING_ENCODERFALLBACK
  private readonly byte? encoderReplacement;
#endif
}
