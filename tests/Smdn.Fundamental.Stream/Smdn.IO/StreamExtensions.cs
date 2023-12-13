// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

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

      Assert.That(outputStream.Length, Is.EqualTo(8));

      outputStream.Dispose();

      Assert.That(outputStream.ToArray(), Is.EqualTo(inputData));
    }

    [Test]
    public void TestCopyToStreamLessThanBufferSize()
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();

      inputStream.CopyTo(outputStream, 16);

      Assert.That(outputStream.Length, Is.EqualTo(8));

      outputStream.Dispose();

      Assert.That(outputStream.ToArray(), Is.EqualTo(inputData));
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

      Assert.That(outputStream.Length, Is.EqualTo(12));

      outputStream.Dispose();

      Assert.That(outputStream.ToArray(), Is.EqualTo(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x06, 0x07}));
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

    [TestCase(false)]
    [TestCase(true)]
    public void TestCopyToBinaryWriter(bool runAsync)
    {
      var inputData = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};
      var inputStream = new MemoryStream(inputData);
      var outputStream = new MemoryStream();
      var writer = new System.IO.BinaryWriter(outputStream);

      if (runAsync)
        Assert.DoesNotThrowAsync(async () => await StreamExtensions.CopyToAsync(inputStream, writer, 3));
      else
        Assert.DoesNotThrow(() => StreamExtensions.CopyTo(inputStream, writer, 3));

      writer.Flush();

      Assert.That(outputStream.Length, Is.EqualTo(8));

      outputStream.Dispose();

      Assert.That(outputStream.ToArray(), Is.EqualTo(inputData));
    }

    [Test]
    public void TestCopyToBinaryWriter_StreamNull()
    {
      var writer = new System.IO.BinaryWriter(Stream.Null);

      Assert.Throws<ArgumentNullException>(() => StreamExtensions.CopyTo(null, writer));
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.CopyToAsync(null, writer));
    }

    [Test]
    public void TestCopyToBinaryWriter_WriterNull()
    {
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.CopyTo(Stream.Null, null));
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.CopyToAsync(Stream.Null, null));
    }

    [Test]
    public void TestCopyToBinaryWriter_OutOfRange_BufferSize()
    {
      var writer = new System.IO.BinaryWriter(Stream.Null);

      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.CopyTo(Stream.Null, writer, bufferSize: 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.CopyToAsync(Stream.Null, writer, bufferSize: 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.CopyTo(Stream.Null, writer, bufferSize: -1));
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.CopyToAsync(Stream.Null, writer, bufferSize: -1));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestReadToEnd(bool runAsync)
    {
      using (var stream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07})) {
        stream.Seek(4, SeekOrigin.Begin);

        byte[] ret = null;

        if (runAsync)
          Assert.DoesNotThrowAsync(async () => ret = await StreamExtensions.ReadToEndAsync(stream, 2, 2));
        else
          Assert.DoesNotThrow(() => ret = StreamExtensions.ReadToEnd(stream, 2, 2));

        Assert.That(ret, Is.EqualTo(new byte[] {0x04, 0x05, 0x06, 0x07}));
      }
    }

    [Test]
    public void TestReadToEnd_StreamNull()
    {
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadToEnd(null));
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadToEndAsync(null));
    }

    [Test]
    public void TestReadToEnd_OutOfRange_ReadBufferSize()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.ReadToEnd(Stream.Null, readBufferSize: 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.ReadToEndAsync(Stream.Null, readBufferSize: 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.ReadToEnd(Stream.Null, readBufferSize: -1));
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.ReadToEndAsync(Stream.Null, readBufferSize: -1));
    }

    [Test]
    public void TestReadToEnd_OutOfRange_InitialCapacity()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.ReadToEnd(Stream.Null, initialCapacity: -1));
      Assert.Throws<ArgumentOutOfRangeException>(() => StreamExtensions.ReadToEndAsync(Stream.Null, initialCapacity: -1));
    }

    [Test]
    public void TestWrite_ArraySegment()
    {
      using (var stream = new MemoryStream()) {
        var segment = new ArraySegment<byte>(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04}, 1, 3);

        StreamExtensions.Write(stream, segment);

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {0x01, 0x02, 0x03}));
      }
    }

#if SYSTEM_BUFFERS_READONLYSEQUENCE
    private class SequenceSegment : ReadOnlySequenceSegment<byte> {
      public SequenceSegment(Memory<byte> memory, int runningIndex, SequenceSegment next)
      {
        this.Memory = memory;
        this.RunningIndex = runningIndex;
        this.Next = next;
      }
    }

    [Test]
    public void TestWriteAsync_ReadOnlySequence_StreamNull()
    {
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.Write(stream: null, sequence: default));
      Assert.Throws<ArgumentNullException>(() => StreamExtensions.WriteAsync(stream: null, sequence: default));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestWrite_ReadOnlySequence_SingleSegment(bool runAsync)
    {
      using (var stream = new MemoryStream()) {
        var seg1 = new SequenceSegment(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 0, null);
        var sequence = new ReadOnlySequence<byte>(seg1, 0, seg1, seg1.Memory.Length);

        Assert.That(sequence.IsSingleSegment, Is.True);

        if (runAsync)
          Assert.DoesNotThrowAsync(async () => await StreamExtensions.WriteAsync(stream, sequence));
        else
          Assert.DoesNotThrow(() => StreamExtensions.Write(stream, sequence));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }));
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestWrite_ReadOnlySequence_MultipleSegment(bool runAsync)
    {
      using (var stream = new MemoryStream()) {
        var seg2 = new SequenceSegment(new byte[] { 0x02, 0x03, 0x04 }, 2, null);
        var seg1 = new SequenceSegment(new byte[] { 0x00, 0x01 }, 0, seg2);
        var sequence = new ReadOnlySequence<byte>(seg1, 0, seg2, seg2.Memory.Length);

        Assert.That(sequence.IsSingleSegment, Is.False);

        if (runAsync)
          Assert.DoesNotThrowAsync(async () => await StreamExtensions.WriteAsync(stream, sequence));
        else
          Assert.DoesNotThrow(() => StreamExtensions.Write(stream, sequence));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }));
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestWriteAsync_ReadOnlySequence_Empty(bool runAsync)
    {
      using (var stream = new MemoryStream()) {
        var sequence = new ReadOnlySequence<byte>();

        Assert.That(sequence.IsEmpty, Is.True);

        if (runAsync)
          Assert.DoesNotThrowAsync(async () => await StreamExtensions.WriteAsync(stream, sequence));
        else
          Assert.DoesNotThrow(() => StreamExtensions.Write(stream, sequence));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[0]));
      }
    }
#endif // SYSTEM_BUFFERS_READONLYSEQUENCE
  }
}
