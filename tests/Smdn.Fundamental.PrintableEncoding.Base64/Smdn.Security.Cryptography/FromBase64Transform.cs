// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Smdn.Formats;

namespace Smdn.Security.Cryptography {
  [TestFixture]
  public class FromBase64TransformTests {
    [Test]
    public void TestProperties()
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        Assert.IsTrue(t.CanReuseTransform);
#if NET5_0_OR_GREATER
        Assert.IsTrue(t.CanTransformMultipleBlocks);
        Assert.AreEqual(4, t.InputBlockSize);
#else
        Assert.IsFalse(t.CanTransformMultipleBlocks);
        Assert.AreEqual(1, t.InputBlockSize);
#endif
        Assert.AreEqual(3, t.OutputBlockSize);
      }
    }

    [Test]
    public void TestDispose()
    {
      using var t = Base64.CreateFromBase64Transform();

      t.Dispose();

      var input = new byte[1];
      var output = new byte[3];

      Assert.Throws<ObjectDisposedException>(() => t.TransformBlock(input, 0, input.Length, output, 0));
    }

    [Test]
    public void TestClear()
    {
      using var t = Base64.CreateFromBase64Transform();

      var clear = t.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes);

      if (clear is null) {
        Assert.Ignore("cannot test Clear()");
        return;
      }

      clear.Invoke(t, parameters: null);

      var input = new byte[1];
      var output = new byte[3];

      Assert.Throws<ObjectDisposedException>(() => t.TransformBlock(input, 0, input.Length, output, 0));
    }

#if false
      foreach (var pattern in new[] {
        new {Output = "A",     Input = "QQ=="},
        new {Output = "AS",    Input = "QVM="},
        new {Output = "ASC",   Input = "QVND"},
        new {Output = "ASCI",  Input = "QVNDSQ=="},
        new {Output = "ASCII", Input = "QVNDSQk="},
      }) {
      }
#endif

    [Test]
    public void TestTransformBlock()
    {
      foreach (var pattern in new[] {
        new {Output = "A",     Input = "QQ=="},
        new {Output = "AS",    Input = "QVM="},
        new {Output = "ASC",   Input = "QVND"},
        new {Output = "ASCI",  Input = "QVNDSQ=="},
        new {Output = "ASCII", Input = "QVNDSUk="},
      }) {
        using (var t = Base64.CreateFromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(pattern.Output.Length, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_ChunkedInput()
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        foreach (var pattern in new[] {
        //new {Output = "ASCII", Input = "QVNDSUk="},
          new {Output = "",      Input = "Q"},
          new {Output = "",      Input = "V"},
          new {Output = "",      Input = "N"},
          new {Output = "ASC",   Input = "D"},
          new {Output = "",      Input = "S"},
          new {Output = "",      Input = "U"},
          new {Output = "",      Input = "k"},
          new {Output = "II",    Input = "="},
        }) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(pattern.Output.Length, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_MultipleBlock()
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        foreach (var pattern in new[] {
          new {Output = "A",     Input = "QQ=="},
          new {Output = "A",     Input = "QQ=="},
        }) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(pattern.Output.Length, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_InvalidFormat()
    {
      foreach (var pattern in new[] {
        new {Output = "xxxx",  Input = "===="},
        new {Output = "xxxx",  Input = "Q==="},
      }) {
        using (var t = Base64.CreateFromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_InvalidFormat_Chunked()
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        byte[] inputBuffer;
        var outputBuffer = new byte[8];

        foreach (var pattern in new[] {
          new {Output = "",      Input = "="},
          new {Output = "",      Input = "="},
          new {Output = "",      Input = "="},
        }) {
          inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(pattern.Output.Length, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }

        inputBuffer = Encoding.ASCII.GetBytes("=");

        Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0));
      }
    }

    [Test]
    public void TestTransformBlock_IgnoreWhiteSpaces()
    {
      foreach (var pattern in new[] {
        new {Output = "ASCII", Input = " QVNDSUk="},
        new {Output = "ASCII", Input = "QVNDSUk= "},
        new {Output = "ASCII", Input = "QVND SUk="},
        new {Output = "ASCII", Input = "QVN DSUk="},
        new {Output = "ASCII", Input = "QVNDS Uk="},
        new {Output = "ASCII", Input = "QVND\tSUk="},
        new {Output = "ASCII", Input = "QVND\nSUk="},
        new {Output = "ASCII", Input = "QVND\rSUk="},
        new {Output = "ASCII", Input = "QVND\r\nSUk="},
      }) {
        using (var t = Base64.CreateFromBase64Transform(true)) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(pattern.Output.Length, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_DoNotIgnoreWhiteSpaces()
    {
      foreach (var pattern in new[] {
        new {Output = "ASCII", Input = " QVNDSUk="},
//        new {Output = "ASCII", Input = "QVNDSUk= "},
        new {Output = "ASCII", Input = "QVND SUk="},
        new {Output = "ASCII", Input = "QVN DSUk="},
        new {Output = "ASCII", Input = "QVNDS Uk="},
        new {Output = "ASCII", Input = "QVND\tSUk="},
        new {Output = "ASCII", Input = "QVND\nSUk="},
        new {Output = "ASCII", Input = "QVND\rSUk="},
        new {Output = "ASCII", Input = "QVND\r\nSUk="},
      }) {
        using (var t = Base64.CreateFromBase64Transform(false)) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_DoNotIgnoreWhiteSpaces_IgnoresTrailingWhiteSpaces()
    {
      foreach (var pattern in new[] {
        new {Output = "A",     Input = "QQ== "},
        new {Output = "AS",    Input = "QVM= "},
        new {Output = "ASC",   Input = "QVND "},
        new {Output = "ASCI",  Input = "QVNDSQ== "},
        new {Output = "ASCII", Input = "QVNDSUk= "},
        new {Output = "ASCII", Input = "QVNDSUk=\t"},
        new {Output = "ASCII", Input = "QVNDSUk=\r"},
        new {Output = "ASCII", Input = "QVNDSUk=\n"},
        new {Output = "ASCII", Input = "QVNDSUk=\r\n"},
      }) {
        using (var t = Base64.CreateFromBase64Transform(false)) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(pattern.Output.Length, transformedLength, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferShorterThanBlockSize()
    {
      foreach (var pattern in new[] {
        new {Output = "A",     Input = ""},
        new {Output = "A",     Input = "Q"},
        new {Output = "A",     Input = "QQ"},
        new {Output = "A",     Input = "QQ="},
      }) {
        using (var t = Base64.CreateFromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[3];

          var length = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(0, length, $"input: {pattern.Input}");
        }
      }
    }

    private static IEnumerable YieldTestCases_TransformBlock_InvalidArguments()
    {
      var inputBuffer = new byte[1];
      var outputBuffer = new byte[1];

      // buffer, offset, count, constraint
      yield return new object[] { null, 0, 1, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentNullException; System.Security.Cryptography.ToBase64Transform throws ArgumentOutOfRangeException
      yield return new object[] { inputBuffer, -1, 0, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
      yield return new object[] { inputBuffer, 0, -1, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
      yield return new object[] { inputBuffer, 1, 1, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
      yield return new object[] { inputBuffer, 0, 2, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
#if !SYSTEM_SECURITY_CRYPTOGRAPHY_FROMBASE64TRANSFORM
      // System.Security.Cryptography.FromBase64Transform does not throw any exceptions in these cases
      yield return new object[] { inputBuffer, 0, 1, null, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentNullException; System.Security.Cryptography.ToBase64Transform throws ArgumentOutOfRangeException
      yield return new object[] { inputBuffer, 0, 1, outputBuffer, -1, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
      yield return new object[] { inputBuffer, 0, 1, outputBuffer, 1, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
#endif
    }

    [TestCaseSource(nameof(YieldTestCases_TransformBlock_InvalidArguments))]
    public void TransformBlock_ArgumentException(
      byte[] inputBuffer,
      int inputOffset,
      int inputCount,
      byte[] outputBuffer,
      int outputOffset,
      IResolveConstraint constraint
    )
    {
      using var t = Base64.CreateFromBase64Transform();

      Assert.Throws(
        constraint,
        () => t.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset)
      );
    }

    [Test]
    public void TestTransformFinalBlock()
    {
      foreach (var pattern in new[] {
        new {Output = "A",     Input = "QQ=="},
        new {Output = "AS",    Input = "QVM="},
        new {Output = "ASC",   Input = "QVND"},
        new {Output = "ASCI",  Input = "QVNDSQ=="},
        new {Output = "ASCII", Input = "QVNDSUk="},
      }) {
        using (var t = Base64.CreateFromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          var ret = t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

          Assert.AreEqual(pattern.Output.Length, ret.Length, $"input: {pattern.Input}");
          Assert.AreEqual(pattern.Output, Encoding.ASCII.GetString(ret), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformFinalBlock_InvalidFormat()
    {
      foreach (var pattern in new[] {
        new {Output = "xxxx", Input = "QVN=SQ=="},
        new {Output = "xxxx", Input = "QV==SQ=="},
      }) {
        using (var t = Base64.CreateFromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          Assert.Throws<FormatException>(() => t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length), $"input: {pattern.Input}");
        }
      }
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
      using var t = Base64.CreateFromBase64Transform();

      Assert.Throws(
        constraint,
        () => t.TransformFinalBlock(inputBuffer, inputOffset, inputCount)
      );
    }
  }
}
