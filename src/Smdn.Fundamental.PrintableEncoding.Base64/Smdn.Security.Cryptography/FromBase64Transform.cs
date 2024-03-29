// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !SYSTEM_SECURITY_CRYPTOGRAPHY_FROMBASE64TRANSFORM
using System;
using System.Security.Cryptography;

namespace Smdn.Security.Cryptography;

internal sealed class FromBase64Transform : ICryptoTransform {
  public bool CanReuseTransform => true;
  public bool CanTransformMultipleBlocks => false;
  public int InputBlockSize => 1;
  public int OutputBlockSize => 3;

  public FromBase64Transform()
    : this(FromBase64TransformMode.IgnoreWhiteSpaces)
  {
  }

  public FromBase64Transform(FromBase64TransformMode mode)
  {
    ignoreWhiteSpaces =
      mode == FromBase64TransformMode.IgnoreWhiteSpaces;
  }

  public void Dispose()
  {
    disposed = true;
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
      outputOffset,
      considerBlockSize: false
    );

    return UncheckedTransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, false);
  }

  private int UncheckedTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset, bool transformFinalBlock)
  {
    var ret = 0;
    var paddedBlockDecoded = false;

    for (; ; ) {
      for (; bufferOffset < 4;) {
        if (inputCount <= 0)
          return ret;

        if (paddedBlockDecoded && transformFinalBlock)
          throw new FormatException("multiple base64 block");

        var octet = inputBuffer[inputOffset++];

        inputCount--;

        if (ignoreWhiteSpaces && char.IsWhiteSpace((char)octet))
          continue;

        buffer[bufferOffset++] = octet;
      }

      bufferOffset = 0;

      for (var b = 0; b < 4; b++) {
        var octet = buffer[b];

        if (0x80 <= octet)
          throw new FormatException($"invalid octet: 0x{octet:X2}");

        buffer[b] = FromBase64Table[octet];

        if (buffer[b] == NUL)
          throw new FormatException($"invalid octet: 0x{octet:X2}");
      }

      if (PAD == buffer[0] || PAD == buffer[1])
        throw new FormatException("incorrect padding");

      if (outputBuffer.Length <= outputOffset)
        throw new FormatException("attempt to access beyond end of array"); // NET46 spec

      outputBuffer[outputOffset++] = (byte)((buffer[0] << 2) | (buffer[1] >> 4));
      ret++;

      if (PAD == buffer[2]) {
        paddedBlockDecoded = true;
        continue;
      }

      if (outputBuffer.Length <= outputOffset)
        throw new FormatException("attempt to access beyond end of array"); // NET46 spec

      outputBuffer[outputOffset++] = (byte)((buffer[1] << 4) | (buffer[2] >> 2));
      ret++;

      if (PAD == buffer[3]) {
        paddedBlockDecoded = true;
        continue;
      }

      if (outputBuffer.Length <= outputOffset)
        throw new FormatException("attempt to access beyond end of array"); // NET46 spec

      outputBuffer[outputOffset++] = (byte)((buffer[2] << 6) | buffer[3]);
      ret++;
    }
  }

  public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
  {
    if (disposed)
      throw new ObjectDisposedException(GetType().FullName);

    CryptoTransformUtils.ValidateTransformFinalBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount,
      considerBlockSize: false
    );

    var ret = new byte[((inputCount / 4) + 1) * 3];
    var len = UncheckedTransformBlock(inputBuffer, inputOffset, inputCount, ret, 0, true);

    Array.Resize(ref ret, len);

    return ret;
  }

  private const byte NUL = 0xff;
  private const byte PAD = 0x40;

#pragma warning disable SA1137
  private static readonly byte[] FromBase64Table = new byte[] {
  /*0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,*/
     NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL, // 0x00
     NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL, // 0x10
     NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL,  NUL, 0x3e,  NUL,  NUL,  NUL, 0x3f, // 0x20
    0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d,  NUL,  NUL,  NUL,  PAD,  NUL,  NUL, // 0x30
     NUL, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, // 0x40
    0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,  NUL,  NUL,  NUL,  NUL,  NUL, // 0x50
     NUL, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, // 0x60
    0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33,  NUL,  NUL,  NUL,  NUL,  NUL, // 0x70
  };
#pragma warning restore SA1137

  private bool disposed = false;
  private readonly bool ignoreWhiteSpaces;
  private readonly byte[] buffer = new byte[4];
  private int bufferOffset = 0;
}
#endif
