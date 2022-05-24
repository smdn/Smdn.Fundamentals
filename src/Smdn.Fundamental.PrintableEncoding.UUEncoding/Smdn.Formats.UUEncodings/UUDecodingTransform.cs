// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.UUEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class UUDecodingTransform : ICryptoTransform {
  public bool CanTransformMultipleBlocks => true;
  public bool CanReuseTransform => true;
  public int InputBlockSize => 1;
  public int OutputBlockSize => 3;

  public UUDecodingTransform()
  {
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

    const byte CR = 0x0d;
    const byte LF = 0x0a;
    var ret = 0;

    for (; ; ) {
      if (bufferOffset == 4)
        ret += WriteBlock(outputBuffer, ref outputOffset);

      if (inputCount-- == 0)
        break;

      var octet = inputBuffer[inputOffset++];

      if (octet is CR or LF) {
        /*
         * <newline>
         */
        if (0 < lineLength)
          ret += WriteBlock(outputBuffer, ref outputOffset);

        lineLength = -1;

        continue;
      }

      if (lineLength == -1) {
        /*
         * <length character>
         */
        if (octet is >= 0x21 and <= 0x5f)
          // '!' 0x21 to '_' 0x5f
          lineLength = octet - 0x20;
        else if (octet is 0x20 or 0x60)
          // SP 0x20 or '`' 0x60
          lineLength = 0;
        else
          throw new FormatException("incorrect form (line length)");
      }
      else {
        /*
         * <formatted characters>
         */
        if (octet is >= 0x21 and <= 0x5f) {
          // '!' 0x21 to '_' 0x5f
          buffer |= (long)(octet - 0x20) << (6 * (3 - bufferOffset));
          bufferOffset++;
        }
        else if (octet is 0x20 or 0x60) {
          // SP 0x20 or '`' 0x60
          bufferOffset++;
          // buffer |= 0x00;
        }
        else {
          throw new FormatException("incorrect form");
        }
      }
    }

    return ret;
  }

  private int WriteBlock(byte[] outputBuffer, ref int outputOffset)
  {
    var ret = 0;

    for (var shift = 16; 0 <= shift; shift -= 8) {
      if (lineLength <= 0)
        break;

      outputBuffer[outputOffset++] = (byte)((buffer >> shift) & 0xff);
      ret++;
      lineLength--;
    }

    buffer = 0L;
    bufferOffset = 0;

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
      return EmptyByteArray;
#endif

    var outputBuffer = new byte[inputCount * OutputBlockSize];
    var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);

    if (outputBuffer.Length < len + OutputBlockSize)
      Array.Resize(ref outputBuffer, len + OutputBlockSize); // XXX

    var outputOffset = len;

    len += WriteBlock(outputBuffer, ref outputOffset);

    Array.Resize(ref outputBuffer, len);

    return outputBuffer;
  }

#if !SYSTEM_ARRAY_EMPTY
  private static readonly byte[] EmptyByteArray = new byte[0];
#endif

  private long buffer = 0L;
  private int bufferOffset = 0;
  private int lineLength = -1;
  private bool disposed = false;
}
