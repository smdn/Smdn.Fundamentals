using System;
using System.IO;
using NUnit.Framework;

using Smdn.Formats;

namespace Smdn.IO {
  [TestFixture]
  public class StreamExtensionsTests {
    [Test]
    public void TestWriteToEndStreamGreaterThanBufferSize()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      StreamExtensions.WriteToEnd(inputStream, outputStream, 3);

      Assert.AreEqual(8, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(inputData, outputStream.ToArray());
    }

    [Test]
    public void TestWriteToEndStreamLessThanBufferSize()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      StreamExtensions.WriteToEnd(inputStream, outputStream, 16);

      Assert.AreEqual(8, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(inputData, outputStream.ToArray());
    }

    [Test]
    public void TestWriteToEndPositionedStream()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      inputStream.Seek(4, SeekOrigin.Begin);
      outputStream.Seek(8, SeekOrigin.Begin);

      StreamExtensions.WriteToEnd(inputStream, outputStream);

      Assert.AreEqual(12, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x06, 0x07}, outputStream.ToArray());
    }

    [Test]
    public void TestWriteToBinaryWriter()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();
      var writer = new System.IO.BinaryWriter(outputStream);

      StreamExtensions.WriteToEnd(inputStream, writer, 3);

      writer.Flush();

      Assert.AreEqual(8, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(inputData, outputStream.ToArray());
    }
  }
}