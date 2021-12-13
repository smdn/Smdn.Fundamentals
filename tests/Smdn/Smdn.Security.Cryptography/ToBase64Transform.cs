// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;
using Smdn.Formats;

#if NET5_0_OR_GREATER
using ExpectedInputBufferRangeException = System.ArgumentOutOfRangeException;
using ExpectedOutputBufferRangeException = System.ArgumentOutOfRangeException;
#else
using ExpectedInputBufferRangeException = System.ArgumentException;
using ExpectedOutputBufferRangeException = System.ArgumentException;
#endif

namespace Smdn.Security.Cryptography {
  [TestFixture]
  public class ToBase64TransformTests {
    [Test]
    public void TestProperties()
    {
      using var t = Base64.CreateToBase64Transform();

      Assert.IsTrue(t.CanReuseTransform, nameof(t.CanReuseTransform));
#if NET6_0_OR_GREATER
      Assert.IsTrue(t.CanTransformMultipleBlocks, nameof(t.CanTransformMultipleBlocks));
#else
      Assert.IsFalse(t.CanTransformMultipleBlocks, nameof(t.CanTransformMultipleBlocks));
#endif
      Assert.AreEqual(3, t.InputBlockSize, nameof(t.InputBlockSize));
      Assert.AreEqual(4, t.OutputBlockSize, nameof(t.OutputBlockSize));
    }

    [Test]
    public void TestDispose()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        t.Dispose();

        var input = new byte[3];
        var output = new byte[4];

        Assert.DoesNotThrow(() => t.TransformBlock(input, 0, input.Length, output, 0));
      }
    }

    // cannot test Clear() with ICryptoTransfrom
#if false// NETFRAMEWORK || NETCOREAPP2_0
    [Test]
    public void TestClear()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        t.Clear();

        var input = new byte[3];
        var output = new byte[4];

        Assert.DoesNotThrow(() => t.TransformBlock(input, 0, input.Length, output, 0));
      }
    }
#endif

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

        Assert.AreEqual(outputBlocks * t.OutputBlockSize, transformedLength, $"input: {input}");
        Assert.AreEqual(output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {input}");
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

    [Test]
    public void TestTransformBlock_InputBufferNull()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentNullException>(() => t.TransformBlock(null, 0, 3, new byte[4], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferInvalidLength()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ExpectedInputBufferRangeException>(() => t.TransformBlock(new byte[3], 0, 4, new byte[4], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferInvalidOffset()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[3], 1, 3, new byte[4], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferRangeNegative()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(new byte[3], -1, 3, new byte[4], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferRangeOverflow()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ExpectedInputBufferRangeException>(() => t.TransformBlock(new byte[3], 1, int.MaxValue, new byte[4], 0));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferNull()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentNullException>(() => t.TransformBlock(new byte[3], 0, 3, null, 0));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferInvalidOffset1()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ExpectedOutputBufferRangeException>(() => t.TransformBlock(new byte[3], 0, 3, new byte[4], 4));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferInvalidOffset2()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ExpectedOutputBufferRangeException>(() => t.TransformBlock(new byte[3], 0, 3, new byte[4], 1));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferRangeNegative()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(new byte[3], 0, 3, new byte[4], -1));
      }
    }

    [TestCase("A",   "QQ==")]
    [TestCase("AS",  "QVM=")]
    [TestCase("ASC", "QVND")]
    public void TestTransformFinalBlock(string input, string output)
    {
      using var t = Base64.CreateToBase64Transform();

      var inputBuffer = Encoding.ASCII.GetBytes(input);

      var ret = t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

      Assert.AreEqual(output.Length, ret.Length, $"input: {input}");
      Assert.AreEqual(output, Encoding.ASCII.GetString(ret), $"input: {input}");
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferEmpty()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.IsEmpty(t.TransformFinalBlock(new byte[0], 0, 0));
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

      Assert.AreEqual(output.Length, ret.Length, $"input: {input}");
      Assert.AreEqual(output, Encoding.ASCII.GetString(ret), $"input: {input}");
#else
      Assert.Throws<ArgumentOutOfRangeException>(
        () => t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length),
        $"input: {input}"
      );
#endif
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferNull()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentNullException>(() => t.TransformFinalBlock(null, 0, 3));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferInvalidLength()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ExpectedInputBufferRangeException>(() => t.TransformFinalBlock(new byte[3], 0, 4));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferInvalidOffset()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformFinalBlock(new byte[3], 1, 3));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferRangeNegative()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformFinalBlock(new byte[3], -1, 3));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferRangeOverflow()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ExpectedInputBufferRangeException>(() => t.TransformFinalBlock(new byte[3], 1, int.MaxValue));
      }
    }
  }
}
