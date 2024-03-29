// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Smdn.Formats;

namespace Smdn.Security.Cryptography {
  [TestFixture]
  public class ToBase64TransformTests {
    [Test]
    public void TestProperties()
    {
      using var t = Base64.CreateToBase64Transform();

      Assert.That(t.CanReuseTransform, Is.True, nameof(t.CanReuseTransform));
#if NET6_0_OR_GREATER
      Assert.That(t.CanTransformMultipleBlocks, Is.True, nameof(t.CanTransformMultipleBlocks));
#else
      Assert.That(t.CanTransformMultipleBlocks, Is.False, nameof(t.CanTransformMultipleBlocks));
#endif
      Assert.That(t.InputBlockSize, Is.EqualTo(3), nameof(t.InputBlockSize));
      Assert.That(t.OutputBlockSize, Is.EqualTo(4), nameof(t.OutputBlockSize));
    }

    [Test]
    public void TestDispose()
    {
      using var t = Base64.CreateToBase64Transform();

      t.Dispose();

      var input = new byte[3];
      var output = new byte[4];

      Assert.DoesNotThrow(() => t.TransformBlock(input, 0, input.Length, output, 0));
    }

    [Test]
    public void TestClear()
    {
      using var t = Base64.CreateToBase64Transform();

      var clear = t.GetType().GetMethod(
        "Clear",
        bindingAttr: BindingFlags.Public | BindingFlags.Instance,
        binder: null,
        types: Type.EmptyTypes,
        modifiers: null
      );

      if (clear is null) {
        Assert.Ignore("cannot test Clear()");
        return;
      }

      clear.Invoke(t, parameters: null);

      var input = new byte[3];
      var output = new byte[4];

      Assert.DoesNotThrow(() => t.TransformBlock(input, 0, input.Length, output, 0));
    }

#if NET6_0_OR_GREATER
    [TestCase("A",      null)]
    [TestCase("AS",     null)]
    [TestCase("ASC",    "QVND")]
    [TestCase("ASCI",   null)]
    [TestCase("ASCII",  null)]
    [TestCase("ASCII_", "QVNDSUlf")]
#else
    [TestCase("A",      null)]
    [TestCase("AS",     null)]
    [TestCase("ASC",    "QVND")]
    [TestCase("ASCI",   "QVND")]
    [TestCase("ASCII",  "QVND")]
    [TestCase("ASCII_", "QVND")]
#endif
    public void TestTransformBlock(string input, string output)
    {
      using var t = Base64.CreateToBase64Transform();

      var inputBuffer = Encoding.ASCII.GetBytes(input);
#if NET6_0_OR_GREATER
      var inputBlocks = 1 + (input.Length - 1) / t.InputBlockSize;
#endif
      var outputBlocks =
#if NET6_0_OR_GREATER
        inputBlocks;
#else
        1;
#endif
      var outputBuffer = new byte[outputBlocks * t.OutputBlockSize];

      if (output is null) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0));
      }
      else {
        var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

        Assert.That(transformedLength, Is.EqualTo(outputBlocks * t.OutputBlockSize), $"input: {input}");
        Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(output), $"input: {input}");
      }
    }

    [TestCase("A",  "QQ==")]
    [TestCase("AS", "QVM=")]
    public void TestTransformBlock_InputBufferTooShort(string input, string output)
    {
      using var t = Base64.CreateToBase64Transform();

      var inputBuffer = Encoding.ASCII.GetBytes(input);
      var outputBuffer = new byte[4];

      Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {input}");
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
    {
      using var t = Base64.CreateToBase64Transform();

      Assert.Throws(
        constraint,
        () => t.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset)
      );
    }

    [TestCase("A",   "QQ==")]
    [TestCase("AS",  "QVM=")]
    [TestCase("ASC", "QVND")]
    public void TestTransformFinalBlock(string input, string output)
    {
      using var t = Base64.CreateToBase64Transform();

      var inputBuffer = Encoding.ASCII.GetBytes(input);

      var ret = t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

      Assert.That(ret.Length, Is.EqualTo(output.Length), $"input: {input}");
      Assert.That(Encoding.ASCII.GetString(ret), Is.EqualTo(output), $"input: {input}");
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferEmpty()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.That(t.TransformFinalBlock(new byte[0], 0, 0), Is.Empty);
      }
    }

    [TestCase("ASCI",  "QVNDSQ==")]
    [TestCase("ASCII", "QVNDSUk=")]
    public void TestTransformFinalBlock_InputBufferTooLong(string input, string output)
    {
      using var t = Base64.CreateToBase64Transform();

      var inputBuffer = Encoding.ASCII.GetBytes(input);

#if NET6_0_OR_GREATER
      var ret = t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

      Assert.That(ret.Length, Is.EqualTo(output.Length), $"input: {input}");
      Assert.That(Encoding.ASCII.GetString(ret), Is.EqualTo(output), $"input: {input}");
#else
      Assert.Throws<ArgumentOutOfRangeException>(
        () => t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length),
        $"input: {input}"
      );
#endif
    }

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
    {
      using var t = Base64.CreateToBase64Transform();

      Assert.Throws(
        constraint,
        () => t.TransformFinalBlock(inputBuffer, inputOffset, inputCount)
      );
    }
  }
}
