// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_BUFFERS_ARRAYPOOL
using System.Buffers;
#endif
using System.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64;

// RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
// 5.1.3. Mailbox International Naming Convention
// http://tools.ietf.org/html/rfc3501#section-5.1.3
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
  public FromRFC3501ModifiedBase64Transform()
    : base()
  {
  }

#if SYSTEM_SECURITY_CRYPTOGRAPHY_FROMBASE64TRANSFORM
  public FromRFC3501ModifiedBase64Transform(FromBase64TransformMode mode)
    : base(mode)
  {
  }
#endif

  public FromRFC3501ModifiedBase64Transform(bool ignoreWhiteSpaces)
    : base(ignoreWhiteSpaces)
  {
  }

  private static void ModifyInput(byte[] inputBuffer, int inputOffset, int inputCount, byte[] destination)
  {
    Buffer.BlockCopy(inputBuffer, inputOffset, destination, 0, inputCount);

    // "," is used instead of "/"
    for (var i = 0; i < inputCount; i++) {
      if (destination[i] == 0x2c)
        // replace ',' 0x2c to '/' 0x2f
        destination[i] = 0x2f;
    }
  }

  public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
  {
    if (inputBuffer is null)
      throw new ArgumentNullException(nameof(inputBuffer));
    if (inputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
    if (inputCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);

    if (outputBuffer is null)
      throw new ArgumentNullException(nameof(outputBuffer));
    if (outputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(outputOffset), outputOffset);
    if (outputBuffer.Length <= outputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(outputOffset), outputBuffer, outputOffset, outputBuffer.Length);

    var modifiedInputBuffer =
#if SYSTEM_BUFFERS_ARRAYPOOL
      ArrayPool<byte>.Shared.Rent(inputCount);
#else
      new byte[inputCount];
#endif

    try {
      ModifyInput(inputBuffer, inputOffset, inputCount, modifiedInputBuffer);

      return base.TransformBlock(
        modifiedInputBuffer,
        0,
        inputCount,
        outputBuffer,
        outputOffset
      );
    }
    finally {
#if SYSTEM_BUFFERS_ARRAYPOOL
      ArrayPool<byte>.Shared.Return(modifiedInputBuffer);
#endif
    }
  }

  public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
  {
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

    var modifiedInputBuffer =
#if SYSTEM_BUFFERS_ARRAYPOOL
      ArrayPool<byte>.Shared.Rent(inputCount);
#else
      new byte[inputCount];
#endif

    try {
      ModifyInput(inputBuffer, inputOffset, inputCount, modifiedInputBuffer);

      return base.TransformFinalBlock(
        modifiedInputBuffer,
        0,
        inputCount
      );
    }
    finally {
#if SYSTEM_BUFFERS_ARRAYPOOL
      ArrayPool<byte>.Shared.Return(modifiedInputBuffer);
#endif
    }
  }
}
