using System;
using System.IO;
using NUnit.Framework;

using Smdn.Formats;

namespace Smdn.IO {
  [TestFixture]
  public class StreamExtensionsTests {
    [Test]
    public void TestCopyToStreamGreaterThanBufferSize()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      StreamExtensions.CopyTo(inputStream, outputStream, 3);

      Assert.AreEqual(8, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(inputData, outputStream.ToArray());
    }

    [Test]
    public void TestCopyToStreamLessThanBufferSize()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      StreamExtensions.CopyTo(inputStream, outputStream, 16);

      Assert.AreEqual(8, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(inputData, outputStream.ToArray());
    }

    [Test]
    public void TestCopyToPositionedStream()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      inputStream.Seek(4, SeekOrigin.Begin);
      outputStream.Seek(8, SeekOrigin.Begin);

      StreamExtensions.CopyTo(inputStream, outputStream);

      Assert.AreEqual(12, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x06, 0x07}, outputStream.ToArray());
    }

    [Test, ExpectedException(typeof(NotSupportedException)), Ignore("to be written")]
    public void TestCopyToReadFromUnreadableStream()
    {
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestCopyToWriteToUnwritableStream()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream(new byte[0], false);

      StreamExtensions.CopyTo(inputStream, outputStream);
    }

    [Test]
    public void TestCopyToBinaryWriter()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();
      var writer = new System.IO.BinaryWriter(outputStream);

      StreamExtensions.CopyTo(inputStream, writer, 3);

      writer.Flush();

      Assert.AreEqual(8, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(inputData, outputStream.ToArray());
    }

    [Test]
    public void TestReadToEnd()
    {
      using (var stream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07})) {
        stream.Seek(4, SeekOrigin.Begin);

        Assert.AreEqual(new byte[] {0x04, 0x05, 0x06, 0x07}, StreamExtensions.ReadToEnd(stream, 2, 2));
      }
    }
  }
}