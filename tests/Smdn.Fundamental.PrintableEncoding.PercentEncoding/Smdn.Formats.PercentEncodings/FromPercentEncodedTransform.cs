// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using NUnit.Framework;
using NUnit.Framework.Constraints;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.PercentEncodings;

[TestFixture]
public class FromPercentEncodedTransformTests {
  [TestCaseSource(
    typeof(ICryptoTransformTestCaseSources),
    nameof(ICryptoTransformTestCaseSources.YieldTestCases_TransformBlock_InvalidArguments)
  )]
  public void TransformBlock_ArgumentException(
    byte[] inputBuffer,
    int inputOffset,
    int inputCount,
    byte[] outputBuffer,
    int outputOffset,
    IResolveConstraint constraint
  )
    => Assert.Throws(
      constraint,
      () => new FromPercentEncodedTransform().TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset)
    );

  [TestCaseSource(
    typeof(ICryptoTransformTestCaseSources),
    nameof(ICryptoTransformTestCaseSources.YieldTestCases_TransformFinalBlock_InvalidArguments)
  )]
  public void TransformFinalBlock_ArgumentException(
    byte[] inputBuffer,
    int inputOffset,
    int inputCount,
    IResolveConstraint constraint
  )
    => Assert.Throws(
      constraint,
      () => new FromPercentEncodedTransform().TransformFinalBlock(inputBuffer, inputOffset, inputCount)
    );
}
