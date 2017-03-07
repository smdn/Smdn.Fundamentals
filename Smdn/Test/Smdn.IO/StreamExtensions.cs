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

      inputStream.CopyTo(outputStream, 3);

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

      inputStream.CopyTo(outputStream, 16);

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

      inputStream.CopyTo(outputStream);

      Assert.AreEqual(12, outputStream.Length);

      outputStream.Close();

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x06, 0x07}, outputStream.ToArray());
    }

    private class UnreadableMemoryStream : MemoryStream
    {
      public override bool CanRead {
        get { return false; }
      }

      public UnreadableMemoryStream(byte[] buffer)
        : base(buffer)
      {
      }
    }

    [Test]
    public void TestCopyToReadFromUnreadableStream()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new UnreadableMemoryStream(inputData);
      var outputStream = new MemoryStream();

      Assert.Throws<NotSupportedException>(() => inputStream.CopyTo(outputStream));
    }

    [Test]
    public void TestCopyToWriteToUnwritableStream()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream(new byte[0], false);

      Assert.Throws<NotSupportedException>(() => inputStream.CopyTo(outputStream));
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

    [Test]
    public void TestWriteArraySegment()
    {
      using (var stream = new MemoryStream()) {
        var segment = new ArraySegment<byte>(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04}, 1, 3);

        StreamExtensions.Write(stream, segment);

        Assert.AreEqual(new byte[] {0x01, 0x02, 0x03}, stream.ToArray());
      }
    }
  }
}