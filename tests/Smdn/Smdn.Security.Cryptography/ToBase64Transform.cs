// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;
using Smdn.Formats;

namespace Smdn.Security.Cryptography {
  [TestFixture]
  public class ToBase64TransformTests {
    [Test]
    public void TestProperties()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.IsTrue(t.CanReuseTransform);
        Assert.IsFalse(t.CanTransformMultipleBlocks);
        Assert.AreEqual(3, t.InputBlockSize);
        Assert.AreEqual(4, t.OutputBlockSize);
      }
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

#if false
      foreach (var pattern in new[] {
        new {Input = "A",     Output = "QQ=="},
        new {Input = "AS",    Output = "QVM="},
        new {Input = "ASC",   Output = "QVND"},
        new {Input = "ASCI",  Output = "QVNDSQ=="},
        new {Input = "ASCII", Output = "QVNDSUk="},
      }) {
      }
#endif

    [Test]
    public void TestTransformBlock()
    {
      foreach (var pattern in new[] {
        new {Input = "ASC",    Output = "QVND"},
        new {Input = "ASCI",   Output = "QVND"},
        new {Input = "ASCII",  Output = "QVND"},
        new {Input = "ASCII_", Output = "QVND"},
      }) {
        using (var t = Base64.CreateToBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[4];

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(4, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferTooShort()
    {
      foreach (var pattern in new[] {
        new {Input = "A",     Output = "QQ=="},
        new {Input = "AS",    Output = "QVM="},
      }) {
        using (var t = Base64.CreateToBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[4];

          Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {pattern.Input}");
        }
      }
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
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[3], 0, 4, new byte[4], 0));
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
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[3], 1, int.MaxValue, new byte[4], 0));
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
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[3], 0, 3, new byte[4], 4));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferInvalidOffset2()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[3], 0, 3, new byte[4], 1));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferRangeNegative()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(new byte[3], 0, 3, new byte[4], -1));
      }
    }

    [Test]
    public void TestTransformFinalBlock()
    {
      foreach (var pattern in new[] {
        new {Input = "A",     Output = "QQ=="},
        new {Input = "AS",    Output = "QVM="},
        new {Input = "ASC",   Output = "QVND"},
      }) {
        using (var t = Base64.CreateToBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);

          var ret = t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

          Assert.AreEqual(pattern.Output.Length, ret.Length, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(ret), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferEmpty()
    {
      using (var t = Base64.CreateToBase64Transform()) {
        Assert.IsEmpty(t.TransformFinalBlock(new byte[0], 0, 0));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferTooLong()
    {
      foreach (var pattern in new[] {
        new {Input = "ASCI",  Output = "QVNDSQ=="},
        new {Input = "ASCII", Output = "QVNDSUk="},
      }) {
        using (var t = Base64.CreateToBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);

          Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length), $"input: {pattern.Input}");
        }
      }
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
        Assert.Throws<ArgumentException>(() => t.TransformFinalBlock(new byte[3], 0, 4));
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
        Assert.Throws<ArgumentException>(() => t.TransformFinalBlock(new byte[3], 1, int.MaxValue));
      }
    }
  }
}