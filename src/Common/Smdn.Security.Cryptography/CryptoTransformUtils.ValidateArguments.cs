// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Security.Cryptography;

namespace Smdn.Security.Cryptography;

internal static partial class CryptoTransformUtils {
  public static void ValidateTransformBlockArguments(
    this ICryptoTransform transform,
    byte[] inputBuffer,
    int inputOffset,
    int inputCount,
    byte[] outputBuffer,
    int outputOffset,
    bool considerBlockSize = true
  )
  {
    if (transform is null)
      throw new ArgumentNullException(nameof(transform));

    if (inputBuffer is null)
      throw new ArgumentNullException(nameof(inputBuffer));
    if (inputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
    if (inputCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);
    if (considerBlockSize && inputCount < transform.InputBlockSize)
      throw ExceptionUtils.CreateArgumentMustBeGreaterThan(transform.InputBlockSize, nameof(inputCount), inputCount);

    if (outputBuffer is null)
      throw new ArgumentNullException(nameof(outputBuffer));
    if (outputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(outputOffset), outputOffset);
    if (outputBuffer.Length <= outputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(outputOffset), outputBuffer, outputOffset, outputBuffer.Length);
    if (considerBlockSize && outputBuffer.Length < outputOffset + transform.OutputBlockSize)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(outputBuffer), outputBuffer, outputOffset, outputBuffer.Length);
  }

  public static void ValidateTransformFinalBlockArguments(
    this ICryptoTransform transform,
    byte[] inputBuffer,
    int inputOffset,
    int inputCount,
    bool considerBlockSize = true
  )
  {
    if (transform is null)
      throw new ArgumentNullException(nameof(transform));

    if (inputBuffer == null)
      throw new ArgumentNullException(nameof(inputBuffer));
    if (inputOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
    if (inputCount < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
    if (inputBuffer.Length - inputCount < inputOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);
    if (considerBlockSize && transform.InputBlockSize < inputCount)
      throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(transform.InputBlockSize, nameof(inputCount), inputCount);
  }
}
