// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_BUFFERS_ARRAYPOOL
using System.Buffers;
#endif
using System.Security.Cryptography;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64;

// RFC 2152 - UTF-7 A Mail-Safe Transformation Format of Unicode
// http://tools.ietf.org/html/rfc2152
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class FromRFC2152ModifiedBase64Transform : ICryptoTransform {
  public bool CanReuseTransform => true;
  public bool CanTransformMultipleBlocks => false;
  public int InputBlockSize => 4;
  public int OutputBlockSize => 3;

  public FromRFC2152ModifiedBase64Transform()
    : this(ignoreWhiteSpaces: true)
  {
  }

#if SYSTEM_SECURITY_CRYPTOGRAPHY_FROMBASE64TRANSFORM
  public FromRFC2152ModifiedBase64Transform(FromBase64TransformMode mode)
    : this(mode == FromBase64TransformMode.IgnoreWhiteSpaces)
  {
  }
#endif

  public FromRFC2152ModifiedBase64Transform(bool ignoreWhiteSpaces)
  {
    this.fromBase64Transform = Base64.CreateFromBase64Transform(ignoreWhiteSpaces);
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (disposing) {
      fromBase64Transform?.Dispose();
      fromBase64Transform = null;
    }
  }

  public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
  {
    if (fromBase64Transform == null)
      throw new ObjectDisposedException(GetType().FullName);

    CryptoTransformUtils.ValidateTransformBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount,
      outputBuffer,
      outputOffset
    );

    count += inputCount;

    return fromBase64Transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
  }

  private static readonly byte[] PaddingBuffer = new byte[] { 0x3d, 0x3d }; // '=' 0x3d

  public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
  {
    if (fromBase64Transform == null)
      throw new ObjectDisposedException(GetType().FullName);

    CryptoTransformUtils.ValidateTransformFinalBlockArguments(
      this,
      inputBuffer,
      inputOffset,
      inputCount
    );

    // The pad character "=" is not used when encoding
    // Modified Base64 because of the conflict with its use as an escape
    // character for the Q content transfer encoding in RFC 2047 header
    // fields, as mentioned above.
    var paddingCount = 4 - ((count + inputCount) & 3);

    count = 0; // initialize

    switch (paddingCount) {
      case 1:
      case 2:
        var paddedInputBufferLength = inputCount + paddingCount;
        var paddedInputBuffer =
#if SYSTEM_BUFFERS_ARRAYPOOL
          ArrayPool<byte>.Shared.Rent(paddedInputBufferLength);
#else
          new byte[paddedInputBufferLength];
#endif

        try {
          Buffer.BlockCopy(inputBuffer, inputOffset, paddedInputBuffer, 0, inputCount);
          Buffer.BlockCopy(PaddingBuffer, 0, paddedInputBuffer, inputCount, paddingCount);

          return fromBase64Transform.TransformFinalBlock(paddedInputBuffer, 0, paddedInputBufferLength);
        }
        finally {
          ArrayPool<byte>.Shared.Return(paddedInputBuffer);
        }

      case 3:
        throw new FormatException("incorrect form");

      default: // case 4
#if SYSTEM_ARRAY_EMPTY
        return Array.Empty<byte>();
#else
        return EmptyByteArray;
#endif
    }
  }

#if !SYSTEM_ARRAY_EMPTY
  private static readonly byte[] EmptyByteArray = new byte[0];
#endif

  private int count = 0;
  private ICryptoTransform? fromBase64Transform;
}
