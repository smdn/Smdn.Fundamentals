// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !SYSTEM_SECURITY_CRYPTOGRAPHY_TOBASE64TRANSFORM
using System;
using System.Security.Cryptography;

namespace Smdn.Security.Cryptography;

internal sealed class ToBase64Transform : ICryptoTransform {
  public bool CanReuseTransform => true;
  public bool CanTransformMultipleBlocks => false;
  public int InputBlockSize => 3;
  public int OutputBlockSize => 4;

  public ToBase64Transform()
  {
  }

  public void Dispose()
  {
  }

  public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
  {
    CryptoTransformUtils.ValidateTransformBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount,
      outputBuffer,
      outputOffset
    );

    return UncheckedTransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
  }

  private static int UncheckedTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
  {
    const byte Padding = 0x3d; // '='

    var ret = 0;

    var b = (inputBuffer[inputOffset] & 0xfc) >> 2;

    outputBuffer[outputOffset++] = ToBase64Table[b];
    ret++;

    b = (inputBuffer[inputOffset++] & 0x03) << 4;
    inputCount--;

    if (0 < inputCount) {
      b |= (inputBuffer[inputOffset] & 0xf0) >> 4;

      outputBuffer[outputOffset++] = ToBase64Table[b];
      ret++;

      b = (inputBuffer[inputOffset++] & 0x0f) << 2;
      inputCount--;

      if (0 < inputCount) {
        b |= (inputBuffer[inputOffset] & 0xc0) >> 6;

        outputBuffer[outputOffset++] = ToBase64Table[b];
        ret++;

        b = inputBuffer[inputOffset++] & 0x3f;
        // inputCount--;

        outputBuffer[outputOffset++] = ToBase64Table[b];
        ret++;
      }
      else {
        outputBuffer[outputOffset++] = ToBase64Table[b];
        outputBuffer[outputOffset++] = Padding;
        ret += 2;
      }
    }
    else {
      outputBuffer[outputOffset++] = ToBase64Table[b];
      outputBuffer[outputOffset++] = Padding;
      outputBuffer[outputOffset++] = Padding;
      ret += 3;
    }

    return ret;
  }

  public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
  {
    CryptoTransformUtils.ValidateTransformFinalBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount
    );

    if (inputCount == 0)
      return Array.Empty<byte>();

    var ret = new byte[OutputBlockSize];
    var len = UncheckedTransformBlock(inputBuffer, inputOffset, inputCount, ret, 0);

    Array.Resize(ref ret, len);

    return ret;
  }

  private static readonly byte[] ToBase64Table = new byte[] {
  /*0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,*/
    0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50, // 0x00
    0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, // 0x10
    0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, // 0x20
    0x77, 0x78, 0x79, 0x7a, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x2b, 0x2f, // 0x30
  /*0x3d,*/
  };
}
#endif
