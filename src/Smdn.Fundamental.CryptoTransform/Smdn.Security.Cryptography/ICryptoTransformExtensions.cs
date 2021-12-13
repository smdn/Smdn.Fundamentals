// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;
using System.Text;

using Smdn.Text.Encodings;

namespace Smdn.Security.Cryptography;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class ICryptoTransformExtensions {
  public static string TransformStringTo(this ICryptoTransform transform, string str, Encoding encoding)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    var bytes = encoding.GetBytes(str);

    return OctetEncoding.EightBits.GetString(TransformBytes(transform, bytes, 0, bytes.Length));
  }

  public static string TransformStringFrom(this ICryptoTransform transform, string str, Encoding encoding)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    var bytes = OctetEncoding.EightBits.GetBytes(str);

    return encoding.GetString(TransformBytes(transform, bytes, 0, bytes.Length));
  }

  public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer)
  {
    if (inputBuffer == null)
      throw new ArgumentNullException(nameof(inputBuffer));

    return TransformBytes(transform, inputBuffer, 0, inputBuffer.Length);
  }

  public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer, int inputOffset, int inputCount)
  {
    if (transform == null)
      throw new ArgumentNullException(nameof(transform));

    if (inputBuffer == null)
      throw new ArgumentNullException(nameof(inputBuffer));
    if (inputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
    if (inputCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);


    var blockCountWithoutRemainder =
#if SYSTEM_MATH_DIVREM
      Math.DivRem(inputCount, transform.InputBlockSize, out var inputBlockRemainder);
#else
      MathUtils.DivRem(inputCount, transform.InputBlockSize, out var inputBlockRemainder);
#endif
    var outputBuffer = new byte[
      (blockCountWithoutRemainder + (inputBlockRemainder == 0 ? 0 : 1)) * transform.OutputBlockSize
    ];
    var outputOffset = 0;

    if (transform.CanTransformMultipleBlocks && 0 < blockCountWithoutRemainder) {
      var bytesToTransform = blockCountWithoutRemainder * transform.InputBlockSize;

      outputOffset += transform.TransformBlock(inputBuffer, inputOffset, bytesToTransform, outputBuffer, outputOffset);
      inputOffset += bytesToTransform;
      inputCount -= bytesToTransform;
    }

    var inputBlockSize = transform.InputBlockSize;

    while (inputBlockSize <= inputCount) {
      outputOffset += transform.TransformBlock(inputBuffer, inputOffset, inputBlockSize, outputBuffer, outputOffset);

      inputOffset += inputBlockSize;
      inputCount -= inputBlockSize;
    }

    var finalBlock = transform.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

    if (outputBuffer.Length != outputOffset + finalBlock.Length)
      Array.Resize(ref outputBuffer, outputOffset + finalBlock.Length);

    Buffer.BlockCopy(finalBlock, 0, outputBuffer, outputOffset, finalBlock.Length);

    return outputBuffer;
  }
}
