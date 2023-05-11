// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.PercentEncodings;

[TestFixture]
public class FromPercentEncodedTransformTests {
  [Test]
  public void Clear()
  {
    using var t = new FromPercentEncodedTransform();

    t.Clear();

    var input = new byte[t.InputBlockSize];
    var output = new byte[t.OutputBlockSize];

    Assert.Throws<ObjectDisposedException>(() => t.TransformBlock(input, 0, input.Length, output, 0));
    Assert.Throws<ObjectDisposedException>(() => t.TransformFinalBlock(input, 0, input.Length));
  }

  [Test]
  public void Dispose()
  {
    using var t = new FromPercentEncodedTransform();

    t.Dispose();

    var input = new byte[t.InputBlockSize];
    var output = new byte[t.OutputBlockSize];

    Assert.Throws<ObjectDisposedException>(() => t.TransformBlock(input, 0, input.Length, output, 0));
    Assert.Throws<ObjectDisposedException>(() => t.TransformFinalBlock(input, 0, input.Length));
  }

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
