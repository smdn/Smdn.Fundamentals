using System;
using System.Text;
#if NET || NETCOREAPP2_0
using System.Security.Cryptography;
#else
using Smdn.Security.Cryptography;
#endif
using NUnit.Framework;

namespace Smdn.Security.Cryptography {
  [TestFixture]
  public class FromBase64TransformTests {
    [Test]
    public void TestProperties()
    {
      using (var t = new FromBase64Transform()) {
        Assert.IsTrue(t.CanReuseTransform);
        Assert.IsFalse(t.CanTransformMultipleBlocks);
        Assert.AreEqual(1, t.InputBlockSize);
        Assert.AreEqual(3, t.OutputBlockSize);
      }
    }

    [Test]
    public void TestDispose()
    {
      using (var t = new FromBase64Transform()) {
        t.Dispose();

        var input = new byte[1];
        var output = new byte[3];

        Assert.Throws<ObjectDisposedException>(() => t.TransformBlock(input, 0, input.Length, output, 0));
      }
    }

#if NET || NETCOREAPP2_0
    [Test]
    public void TestClear()
    {
      using (var t = new FromBase64Transform()) {
        t.Clear();

        var input = new byte[1];
        var output = new byte[3];

        Assert.Throws<ObjectDisposedException>(() => t.TransformBlock(input, 0, input.Length, output, 0));
      }
    }
#endif

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
        using (var t = new FromBase64Transform()) {
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
      using (var t = new FromBase64Transform()) {
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
      using (var t = new FromBase64Transform()) {
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
        using (var t = new FromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          Assert.Throws<FormatException>(() => t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_InvalidFormat_Chunked()
    {
      using (var t = new FromBase64Transform()) {
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
        using (var t = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces)) {
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
        using (var t = new FromBase64Transform(FromBase64TransformMode.DoNotIgnoreWhiteSpaces)) {
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
        using (var t = new FromBase64Transform(FromBase64TransformMode.DoNotIgnoreWhiteSpaces)) {
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
        using (var t = new FromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[3];

          var length = t.TransformBlock(inputBuffer, 0, inputBuffer.Length, outputBuffer, 0);

          Assert.AreEqual(0, length, $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferNull()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentNullException>(() => t.TransformBlock(null, 0, 1, new byte[3], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferInvalidLength()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[1], 0, 2, new byte[3], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferInvalidOffset()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[1], 1, 1, new byte[3], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferRangeNegative()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(new byte[1], -1, 1, new byte[3], 0));
      }
    }

    [Test]
    public void TestTransformBlock_InputBufferRangeOverflow()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[1], 1, int.MaxValue, new byte[3], 0));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferNull()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentNullException>(() => t.TransformBlock(new byte[4] { 0x41, 0x41, 0x41, 0x41 }, 0, 4, null, 0));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferInvalidOffset()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformBlock(new byte[4] { 0x41, 0x41, 0x41, 0x41 }, 0, 4, new byte[3], 4));
      }
    }

    [Test]
    public void TestTransformBlock_OutputBufferRangeNegative()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformBlock(new byte[4] { 0x41, 0x41, 0x41, 0x41 }, 0, 4, new byte[3], -1));
      }
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
        using (var t = new FromBase64Transform()) {
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
        using (var t = new FromBase64Transform()) {
          var inputBuffer = Encoding.ASCII.GetBytes(pattern.Input);
          var outputBuffer = new byte[8];

          Assert.Throws<FormatException>(() => t.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length), $"input: {pattern.Input}");
        }
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferEmpty()
    {
      using (var t = new FromBase64Transform()) {
        Assert.IsEmpty(t.TransformFinalBlock(new byte[0], 0, 0));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferNull()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentNullException>(() => t.TransformFinalBlock(null, 0, 1));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferInvalidLength()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformFinalBlock(new byte[1], 0, 2));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferInvalidOffset()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformFinalBlock(new byte[1], 1, 1));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferRangeNegative()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentOutOfRangeException>(() => t.TransformFinalBlock(new byte[1], -1, 1));
      }
    }

    [Test]
    public void TestTransformFinalBlock_InputBufferRangeOverflow()
    {
      using (var t = new FromBase64Transform()) {
        Assert.Throws<ArgumentException>(() => t.TransformFinalBlock(new byte[1], 1, int.MaxValue));
      }
    }
  }
}
