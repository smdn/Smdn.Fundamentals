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
        Assert.That(t.CanReuseTransform, Is.True);
#if NET5_0_OR_GREATER
        Assert.That(t.CanTransformMultipleBlocks, Is.True);
        Assert.That(t.InputBlockSize, Is.EqualTo(4));
#else
        Assert.That(t.CanTransformMultipleBlocks, Is.False);
        Assert.That(t.InputBlockSize, Is.EqualTo(1));
#endif
        Assert.That(t.OutputBlockSize, Is.EqualTo(3));
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

    [TestCase("A",     "QQ==")]
    [TestCase("AS",    "QVM=")]
    [TestCase("ASC",   "QVND")]
    [TestCase("ASCI",  "QVNDSQ==")]
    [TestCase("ASCII", "QVNDSUk=")]
    public void TestTransformBlock(string output, string input)
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

        var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

        Assert.That(transformedLength, Is.EqualTo(output.Length), $"input: {input}");
        Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(output), $"input: {input}");
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

          Assert.That(transformedLength, Is.EqualTo(pattern.Output.Length), $"input: {pattern.Input}");
          Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(pattern.Output), $"input: {pattern.Input}");
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

          Assert.That(transformedLength, Is.EqualTo(pattern.Output.Length), $"input: {pattern.Input}");
          Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(pattern.Output), $"input: {pattern.Input}");
        }
      }
    }

    [TestCase("====")]
    [TestCase("Q===")]
    public void TestTransformBlock_InvalidFormat(string input)
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

        Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {input}");
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

          Assert.That(transformedLength, Is.EqualTo(pattern.Output.Length), $"input: {pattern.Input}");
          Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(pattern.Output), $"input: {pattern.Input}");
        }

        inputBuffer = Encoding.ASCII.GetBytes("=");

        Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0));
      }
    }

    [TestCase("ASCII", " QVNDSUk=")]
    [TestCase("ASCII", "QVNDSUk= ")]
    [TestCase("ASCII", "QVND SUk=")]
    [TestCase("ASCII", "QVN DSUk=")]
    [TestCase("ASCII", "QVNDS Uk=")]
    [TestCase("ASCII", "QVND\tSUk=")]
    [TestCase("ASCII", "QVND\nSUk=")]
    [TestCase("ASCII", "QVND\rSUk=")]
    [TestCase("ASCII", "QVND\r\nSUk=")]
    public void TestTransformBlock_IgnoreWhiteSpaces(string output, string input)
    {
      using (var t = Base64.CreateFromBase64Transform(true)) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

        var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

        Assert.That(transformedLength, Is.EqualTo(output.Length), $"input: {input}");
        Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(output), $"input: {input}");
      }
    }

    [TestCase(" QVNDSUk=")]
    // [TestCase("QVNDSUk= ")]
    [TestCase("QVND SUk=")]
    [TestCase("QVN DSUk=")]
    [TestCase("QVNDS Uk=")]
    [TestCase("QVND\tSUk=")]
    [TestCase("QVND\nSUk=")]
    [TestCase("QVND\rSUk=")]
    [TestCase("QVND\r\nSUk=")]
    public void TestTransformBlock_DoNotIgnoreWhiteSpaces(string input)
    {
      using (var t = Base64.CreateFromBase64Transform(false)) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

        Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {input}");
      }
    }

    [TestCase("A",     "QQ== ")]
    [TestCase("AS",    "QVM= ")]
    [TestCase("ASC",   "QVND ")]
    [TestCase("ASCI",  "QVNDSQ== ")]
    [TestCase("ASCII", "QVNDSUk= ")]
    [TestCase("ASCII", "QVNDSUk=\t")]
    [TestCase("ASCII", "QVNDSUk=\r")]
    [TestCase("ASCII", "QVNDSUk=\n")]
    [TestCase("ASCII", "QVNDSUk=\r\n")]
    public void TestTransformBlock_DoNotIgnoreWhiteSpaces_IgnoresTrailingWhiteSpaces(string output, string input)
    {
      using (var t = Base64.CreateFromBase64Transform(false)) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

#if NET8_0_OR_GREATER
        Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {input}");
#else
        var transformedLength = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

        Assert.That(transformedLength, Is.EqualTo(output.Length), $"input: {input}");
        Assert.That(Encoding.ASCII.GetString(outputBuffer, 0, transformedLength), Is.EqualTo(output), $"input: {input}");
#endif
      }
    }

    [TestCase("")]
    [TestCase("Q")]
    [TestCase("QQ")]
    [TestCase("QQ=")]
    public void TestTransformBlock_InputBufferShorterThanBlockSize(string input)
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[3];

        var length = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

        Assert.That(length, Is.Zero, $"input: {input}");
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

    [TestCase("A",     "QQ==")]
    [TestCase("AS",    "QVM=")]
    [TestCase("ASC",   "QVND")]
    [TestCase("ASCI",  "QVNDSQ==")]
    [TestCase("ASCII", "QVNDSUk=")]
    public void TestTransformFinalBlock(string output, string input)
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

        var ret = t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

        Assert.That(ret.Length, Is.EqualTo(output.Length), $"input: {input}");
        Assert.That(Encoding.ASCII.GetString(ret), Is.EqualTo(output), $"input: {input}");
      }
    }

    [TestCase("QVN=SQ==")]
    [TestCase("QV==SQ==")]
    public void TestTransformFinalBlock_InvalidFormat(string input)
    {
      using (var t = Base64.CreateFromBase64Transform()) {
        var inputBuffer = Encoding.ASCII.GetBytes(input);
        var outputBuffer = new byte[8];

        Assert.Throws<FormatException>(() => t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length), $"input: {input}");
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
