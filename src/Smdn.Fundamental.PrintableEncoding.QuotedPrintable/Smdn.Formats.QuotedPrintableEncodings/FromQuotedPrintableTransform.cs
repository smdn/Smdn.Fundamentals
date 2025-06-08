// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:words dequote
using System;
using System.Security.Cryptography;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.QuotedPrintableEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class FromQuotedPrintableTransform : ICryptoTransform {
  public bool CanTransformMultipleBlocks => true;
  public bool CanReuseTransform => true;
  public int InputBlockSize => 1;
  public int OutputBlockSize => 1;

  public FromQuotedPrintableTransform(FromQuotedPrintableTransformMode mode)
  {
    dequoteUnderscore = mode switch {
      FromQuotedPrintableTransformMode.MimeEncoding => true,
      FromQuotedPrintableTransformMode.ContentTransferEncoding => false,
      _ => throw ExceptionUtils.CreateNotSupportedEnumValue(mode),
    };
  }

  public void Clear()
    => Dispose();

  public void Dispose()
  {
    disposed = true;

    GC.SuppressFinalize(this);
  }

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

    const byte CR = 0x0d;
    const byte LF = 0x0a;
    var ret = 0;

    while (0 < inputCount--) {
      var octet = inputBuffer[inputOffset++];

      if (bufferOffset == 0) {
        if (octet == 0x3d) { // '=' 0x3d
          buffer[bufferOffset++] = octet;
        }
        else if (dequoteUnderscore && octet == 0x5f) { // '_' 0x5f
          outputBuffer[outputOffset++] = 0x20; // ' ' 0x20
          ret++;
        }
        else {
          outputBuffer[outputOffset++] = octet;
          ret++;
        }
      }
      else {
        // quoted char
        buffer[bufferOffset++] = octet;
      }

      if (bufferOffset == 3) {
        // dequote
        if (buffer[1] == CR && buffer[2] == LF) {
          // soft newline (CRLF)
          bufferOffset = 0;
        }
        else if (buffer[1] is CR or LF) {
          // soft newline (CR, LF)
          if (buffer[2] == 0x3d) {
            bufferOffset = 1;
          }
          else {
            outputBuffer[outputOffset++] = buffer[2];
            ret++;

            bufferOffset = 0;
          }
        }
        else {
          if (!Hexadecimal.TryDecode(buffer.AsSpan(1, 2), out var d))
            throw new FormatException("incorrect form");

          outputBuffer[outputOffset++] = d;
          ret++;

          bufferOffset = 0;
        }
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

    var outputBuffer = new byte[inputCount/* * OutputBlockSize */];
    var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);

    Array.Resize(ref outputBuffer, len);

    return outputBuffer;
  }

  private readonly byte[] buffer = new byte[3];
  private int bufferOffset;
  private readonly bool dequoteUnderscore;
  private bool disposed;
}
