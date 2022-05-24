// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.PercentEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class FromPercentEncodedTransform : ICryptoTransform {
  public bool CanTransformMultipleBlocks => true;
  public bool CanReuseTransform => true;
  public int InputBlockSize => 1;
  public int OutputBlockSize => 1;

  public FromPercentEncodedTransform()
    : this(false)
  {
  }

  public FromPercentEncodedTransform(bool decodePlusToSpace)
  {
    this.decodePlusToSpace = decodePlusToSpace;
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

    const byte SP = (byte)' ';
    var ret = 0;

    while (0 < inputCount--) {
      var octet = inputBuffer[inputOffset++];

      if (bufferOffset == 0) {
        if (decodePlusToSpace && octet == 0x2b) { // '+' 0x2b
          outputBuffer[outputOffset++] = SP;
          ret++;
        }
        else if (octet == 0x25) { // '%' 0x25
          buffer[bufferOffset++] = octet;
        }
        else {
          outputBuffer[outputOffset++] = octet;
          ret++;
        }
      }
      else {
        // encoded char
        buffer[bufferOffset++] = octet;
      }

      if (bufferOffset == 3) {
        // decode
        if (!Hexadecimal.TryDecode(buffer.AsSpan(1, 2), out var d))
          throw new FormatException("incorrect form");

        outputBuffer[outputOffset++] = d;
        ret++;

        bufferOffset = 0;
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

    var outputBuffer = new byte[inputCount/* * OutputBlockSize */];
    var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);

    Array.Resize(ref outputBuffer, len);

    return outputBuffer;
  }

  private readonly byte[] buffer = new byte[3];
  private int bufferOffset = 0;
  private bool disposed = false;
  private readonly bool decodePlusToSpace;
}
