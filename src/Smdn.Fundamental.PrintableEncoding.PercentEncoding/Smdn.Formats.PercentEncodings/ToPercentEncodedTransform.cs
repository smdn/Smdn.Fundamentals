// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.PercentEncodings;

/*
 * http://tools.ietf.org/html/rfc3986
 * RFC 3986 - Uniform Resource Identifier (URI): Generic Syntax
 * 2.1. Percent-Encoding
 *
 * http://tools.ietf.org/html/rfc2396
 * RFC 2396 - Uniform Resource Identifiers (URI): Generic Syntax
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class ToPercentEncodedTransform : ICryptoTransform {
  //                                    "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
  // RFC 2396 unreserved characters:    "!      '()*  -. 0123456789     ?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[  ] _ abcdefghijklmnopqrstuvwxyz   ~";
  // RFC 3986 unreserved characters:    "             -. 0123456789       ABCDEFGHIJKLMNOPQRSTUVWXYZ     _ abcdefghijklmnopqrstuvwxyz   ~";
  // RFC 3986   reserved characters:    "!  #$ &'()*+,  /          :; = ?@                          [  ]                                 ";
  private const string
    Rfc2396UriEscapeChars             = " \"  %                      < >                            [\\]^ `                          {|} ";
  private const string
    Rfc3986UriEscapeChars             = " \"  %                      < >                             \\ ^ `                          {|} ";
  private const string
    Rfc2396DataEscapeChars            = " \"#$%&    +,  /          :;<=>?@                          [\\]^ `                          {|} ";
  private const string
    Rfc3986DataEscapeChars            = "!\"#$%&'()*+,  /          :;<=>?@                          [\\]^ `                          {|} ";

  private const string
    Rfc5092AChars                     = " \"# %         /          :;< >?@                          [\\]^ `                          {|} ";
  private const string
    Rfc5092BChars                     = " \"# %                     ;< >?                           [\\]^ `                          {|} ";

  private static byte[] GetEscapeOctets(string str)
  {
    const byte SP = (byte)' ';
    var octets = new byte[0x80 - 0x20];
    var count = 0;

    octets[count++] = SP;

    foreach (var c in str) {
      if (c != SP)
        octets[count++] = (byte)c;
    }

    Array.Resize(ref octets, count);

    return octets;
  }

  public bool CanTransformMultipleBlocks => true;
  public bool CanReuseTransform => true;
  public int InputBlockSize => 1;
  public int OutputBlockSize => 3;

  public ToPercentEncodedTransform(ToPercentEncodedTransformMode mode)
  {
#pragma warning disable IDE0055
    escapeOctets = (mode & ToPercentEncodedTransformMode.ModeMask) switch {
      ToPercentEncodedTransformMode.Rfc2396Uri  => GetEscapeOctets(Rfc2396UriEscapeChars),
      ToPercentEncodedTransformMode.Rfc2396Data => GetEscapeOctets(Rfc2396DataEscapeChars),
      ToPercentEncodedTransformMode.Rfc3986Uri  => GetEscapeOctets(Rfc3986UriEscapeChars),
      ToPercentEncodedTransformMode.Rfc3986Data => GetEscapeOctets(Rfc3986DataEscapeChars),
      ToPercentEncodedTransformMode.Rfc5092Uri  => GetEscapeOctets(Rfc5092AChars),
      ToPercentEncodedTransformMode.Rfc5092Path => GetEscapeOctets(Rfc5092BChars),
      _ => throw ExceptionUtils.CreateNotSupportedEnumValue(mode),
    };
#pragma warning restore IDE0055

    escapeSpaceToPlus = (mode & ToPercentEncodedTransformMode.EscapeSpaceToPlus) != 0;
  }

  public void Clear()
  {
    disposed = true;
  }

  void IDisposable.Dispose() => Clear();

  public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
  {
    if (disposed)
      throw new ObjectDisposedException(GetType().FullName);

    if (inputBuffer == null)
      throw new ArgumentNullException(nameof(inputBuffer));
    if (inputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
    if (inputCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);

    if (outputBuffer == null)
      throw new ArgumentNullException(nameof(outputBuffer));
    if (outputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(outputOffset), outputOffset);
    if (outputBuffer.Length - inputCount < outputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(outputOffset), outputBuffer, outputOffset, inputCount);

    var ret = 0;

    for (var i = 0; i < inputCount; i++) {
      var octet = inputBuffer[inputOffset++];

      var isPrintable = octet is >= 0x20 and < 0x80;
      var isDigit = isPrintable && octet is >= 0x30 and <= 0x39; // DIGIT
      var isUpAlpha = isPrintable && octet is >= 0x41 and <= 0x5a; // UPALPHA
      var isLowAlpha = isPrintable && octet is >= 0x61 and <= 0x7a; // UPALPHA
      var escape = !isPrintable && !(isDigit || isUpAlpha || isLowAlpha);

      escape |= 0 <= Array.BinarySearch(escapeOctets, octet);

      if (escape) {
        if (octet == 0x20 && escapeSpaceToPlus) {
          outputBuffer[outputOffset++] = 0x2b; // '+' 0x2b

          ret += 1;
        }
        else {
          outputBuffer[outputOffset++] = 0x25; // '%' 0x25

          Hexadecimal.TryEncodeUpperCase(octet, outputBuffer.AsSpan(outputOffset, 2), out var bytesEncoded);

          outputOffset += bytesEncoded;

          ret += 3;
        }
      }
      else {
        outputBuffer[outputOffset++] = octet;

        ret += 1;
      }
    }

    return ret;
  }

  public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
  {
    if (disposed)
      throw new ObjectDisposedException(GetType().FullName);
    if (inputBuffer == null)
      throw new ArgumentNullException(nameof(inputBuffer));
    if (inputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
    if (inputCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);
    if (InputBlockSize < inputCount)
      throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(InputBlockSize), nameof(inputCount), inputCount);

    var outputBuffer = new byte[inputCount * OutputBlockSize];
    var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputBuffer.Length);

    Array.Resize(ref outputBuffer, len);

    return outputBuffer;
  }

  private bool disposed = false;
  private readonly byte[] escapeOctets;
  private readonly bool escapeSpaceToPlus;
}
