// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

using Smdn.Security.Cryptography;

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

    CryptoTransformUtils.ValidateTransformBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount,
      outputBuffer,
      outputOffset
    );

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

    CryptoTransformUtils.ValidateTransformFinalBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount
    );

    if (inputCount == 0)
#if SYSTEM_ARRAY_EMPTY
      return Array.Empty<byte>();
#else
      return QuotedPrintableEncoding.EmptyByteArray;
#endif

    var outputBuffer = new byte[inputCount * OutputBlockSize];
    var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputBuffer.Length);

    Array.Resize(ref outputBuffer, len);

    return outputBuffer;
  }

  private readonly bool quoteWhitespaces = true;
  private bool disposed = false;
}
