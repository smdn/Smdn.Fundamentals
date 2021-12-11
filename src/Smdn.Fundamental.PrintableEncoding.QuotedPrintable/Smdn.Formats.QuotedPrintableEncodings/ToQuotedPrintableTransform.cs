// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.QuotedPrintableEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class ToQuotedPrintableTransform : ICryptoTransform {
  public bool CanTransformMultipleBlocks => true;
  public bool CanReuseTransform => true;
  public int InputBlockSize => 1;
  public int OutputBlockSize => 3;

  public ToQuotedPrintableTransform(ToQuotedPrintableTransformMode mode)
  {
    quoteWhitespaces = mode switch {
      ToQuotedPrintableTransformMode.MimeEncoding => true,
      ToQuotedPrintableTransformMode.ContentTransferEncoding => false,
      _ => throw ExceptionUtils.CreateNotSupportedEnumValue(mode),
    };
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
      var quote = octet switch {
        0x09 /* HT */   => quoteWhitespaces,
        0x20 /* SP */   => quoteWhitespaces,
        0x3f /* '?' */  => quoteWhitespaces,
        0x5f /* '_' */  => quoteWhitespaces,
        0x3d /* '=' */  => true,
        < 0x21 => true, // quote non-printable chars
        > 0x7f => true, // quote non-printable chars
        _ => false,
      };

      if (quote) {
        // '=' 0x3d or non printable char
        outputBuffer[outputOffset++] = 0x3d; // '=' 0x3d

        Hexadecimal.TryEncodeUpperCase(octet, outputBuffer.AsSpan(outputOffset, 2), out var bytesEncoded);

        outputOffset += bytesEncoded;

        ret += 3;
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
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputOffset);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);
    if (InputBlockSize < inputCount)
      throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(InputBlockSize), nameof(inputCount), inputCount);

    var outputBuffer = new byte[inputCount * OutputBlockSize];
    var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputBuffer.Length);

    Array.Resize(ref outputBuffer, len);

    return outputBuffer;
  }

  private readonly bool quoteWhitespaces = true;
  private bool disposed = false;
}
