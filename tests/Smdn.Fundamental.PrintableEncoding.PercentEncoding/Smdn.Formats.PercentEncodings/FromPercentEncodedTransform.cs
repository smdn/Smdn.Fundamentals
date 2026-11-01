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

    Assert.That(() => t.TransformBlock(input, 0, input.Length, output, 0), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => t.TransformFinalBlock(input, 0, input.Length), Throws.TypeOf<ObjectDisposedException>());
  }

  [Test]
  public void Dispose()
  {
    using var t = new FromPercentEncodedTransform();

    t.Dispose();

    var input = new byte[t.InputBlockSize];
    var output = new byte[t.OutputBlockSize];

    Assert.That(() => t.TransformBlock(input, 0, input.Length, output, 0), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => t.TransformFinalBlock(input, 0, input.Length), Throws.TypeOf<ObjectDisposedException>());
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
    Constraint throwsConstraint
  )
    => Assert.That(
      () => new FromPercentEncodedTransform().TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset),
      throwsConstraint
    );

  [TestCaseSource(
    typeof(ICryptoTransformTestCaseSources),
    nameof(ICryptoTransformTestCaseSources.YieldTestCases_TransformFinalBlock_InvalidArguments)
  )]
  public void TransformFinalBlock_ArgumentException(
    byte[] inputBuffer,
    int inputOffset,
    int inputCount,
    Constraint throwsConstraint
  )
    => Assert.That(
      () => new FromPercentEncodedTransform().TransformFinalBlock(inputBuffer, inputOffset, inputCount),
      throwsConstraint
    );
}
