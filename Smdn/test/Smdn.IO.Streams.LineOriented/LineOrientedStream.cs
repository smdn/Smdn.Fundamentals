using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

using Smdn.Text;

namespace Smdn.IO.Streams.LineOriented {
  [TestFixture]
  public class LineOrientedStreamTests {
    public enum StreamType {
      Strict,
      Loose,
    }

    private static LineOrientedStream CreateStream(StreamType type, Stream baseStream, int bufferSize)
    {
      return CreateStream(type, baseStream, bufferSize, false);
    }

    private static LineOrientedStream CreateStream(StreamType type, Stream baseStream, int bufferSize, bool leaveStreamOpen)
    {
      switch (type) {
        case StreamType.Loose:
          return new LooseLineOrientedStream(baseStream, bufferSize, leaveStreamOpen);
        case StreamType.Strict:
          return new StrictLineOrientedStream(baseStream, bufferSize, leaveStreamOpen);
        default:
          return new LooseLineOrientedStream(baseStream, bufferSize, leaveStreamOpen); // XXX
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestConstructFromMemoryStream(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};

      using (var stream = CreateStream(type, new MemoryStream(data), 8)) {
        Assert.IsTrue(stream.CanRead, "can read");
        Assert.IsTrue(stream.CanWrite, "can write");
        Assert.IsTrue(stream.CanSeek, "can seek");
        Assert.IsFalse(stream.CanTimeout, "can timeout");
        Assert.AreEqual(8L, stream.Length);
        Assert.AreEqual(0L, stream.Position);
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public async Task TestReadLineAsync_EndOfStream(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43};

      using (var stream = CreateStream(type, new MemoryStream(data), 8)) {
        stream.ReadToEnd();

        var ret = await stream.ReadLineAsync();

        Assert.IsTrue(ret.IsEndOfStream);
        Assert.Throws<InvalidOperationException>(() => Assert.IsNotNull(ret.IsEmptyLine));
        Assert.Throws<InvalidOperationException>(() => Assert.IsNotNull(ret.LineWithNewLine));
        Assert.Throws<InvalidOperationException>(() => Assert.IsNotNull(ret.Line));
        Assert.AreEqual(0, ret.LengthOfNewLine);
      }
    }

    [TestCase(StreamType.Strict, false)]
    [TestCase(StreamType.Strict, true)]
    [TestCase(StreamType.Loose, false)]
    [TestCase(StreamType.Loose, true)]
    public async Task TestReadLineAsync_EndOfStream_NothingBuffered(StreamType type, bool keepEOL)
    {
      using (var stream = CreateStream(type, Stream.Null, 8)) {
        var ret = await stream.ReadLineAsync();

        Assert.IsTrue(ret.IsEndOfStream);
        Assert.Throws<InvalidOperationException>(() => Assert.IsNotNull(ret.IsEmptyLine));
        Assert.Throws<InvalidOperationException>(() => Assert.IsNotNull(ret.LineWithNewLine));
        Assert.Throws<InvalidOperationException>(() => Assert.IsNotNull(ret.Line));
        Assert.AreEqual(0, ret.LengthOfNewLine);
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public async Task TestReadLineAsync_SingleSegment(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF};

      using (var stream = CreateStream(type, new MemoryStream(data), bufferSize: 8)) {
        var ret = await stream.ReadLineAsync();

        Assert.IsFalse(ret.IsEndOfStream);
        Assert.IsFalse(ret.IsEmptyLine);
        Assert.IsTrue(ret.LineWithNewLine.IsSingleSegment);
        Assert.IsTrue(ret.Line.IsSingleSegment);
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.LineWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43 },
                                  ret.Line.ToArray());
      }
    }

    [TestCase(StreamType.Strict, false)]
    [TestCase(StreamType.Strict, true)]
    [TestCase(StreamType.Loose, false)]
    [TestCase(StreamType.Loose, true)]
    public async Task TestReadLineAsync_MultipleSegment(StreamType type, bool keepEOL)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, Ascii.Octets.CR, Ascii.Octets.LF};

      using (var stream = CreateStream(type, new MemoryStream(data), bufferSize: 8)) {
        var ret = await stream.ReadLineAsync();

        Assert.IsFalse(ret.IsEndOfStream);
        Assert.IsFalse(ret.IsEmptyLine);
        Assert.IsFalse(ret.LineWithNewLine.IsSingleSegment);
        Assert.IsFalse(ret.Line.IsSingleSegment);
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.LineWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f },
                                  ret.Line.ToArray());
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadByte(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
      var stream = CreateStream(type, new MemoryStream(data), 8);
      var index = 0;

      for (;;) {
        var val = stream.ReadByte();

        if (index == data.Length) {
          Assert.AreEqual(-1, val);
        }
        else {
          Assert.AreEqual(data[index++], val, "data");
          Assert.AreEqual(index, stream.Position, "Position");
        }

        if (val == -1)
          break;
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStreamBufferEmpty(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF, 0x42, 0x43, 0x44, Ascii.Octets.CR, Ascii.Octets.LF, 0x45, 0x46, 0x47};
      var stream = CreateStream(type, new MemoryStream(data), 8);

      var copyStream = new MemoryStream();

      Assert.AreEqual(12L, stream.Read(copyStream, 12L));

      Assert.AreEqual(12L, stream.Position, "Position");

      copyStream.Dispose();

      Assert.AreEqual(data, copyStream.ToArray());
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadLessThanBuffered(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF, 0x42, 0x43, 0x44, Ascii.Octets.CR, Ascii.Octets.LF, 0x45, 0x46, 0x47};
      var stream = CreateStream(type, new MemoryStream(data), 16);

      var line = stream.ReadLine(true);

      Assert.AreEqual(4L, stream.Position, "Position");

      Assert.AreEqual(data.Slice(0, 4), line);

      var buffer = new byte[4];

      Assert.AreEqual(4, stream.Read(buffer, 0, 4));

      Assert.AreEqual(8L, stream.Position, "Position");

      Assert.AreEqual(data.Slice(4, 4), buffer);
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadLongerThanBuffered(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF, 0x42, 0x43, 0x44, Ascii.Octets.CR, Ascii.Octets.LF, 0x45, 0x46, 0x47};
      var stream = CreateStream(type, new MemoryStream(data), 8);
      
      var line = stream.ReadLine(true);

      Assert.AreEqual(4L, stream.Position, "Position");

      Assert.AreEqual(data.Slice(0, 4), line);

      var buffer = new byte[10];

      Assert.AreEqual(8, stream.Read(buffer, 0, 10));

      Assert.AreEqual(12L, stream.Position, "Position");

      Assert.AreEqual(data.Slice(4, 8), buffer.Slice(0, 8));
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStreamLessThanBuffered(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF, 0x42, 0x43, 0x44, Ascii.Octets.CR, Ascii.Octets.LF, 0x45, 0x46, 0x47};
      var stream = CreateStream(type, new MemoryStream(data), 16);

      var line = stream.ReadLine(true);

      Assert.AreEqual(4L, stream.Position, "Position");

      Assert.AreEqual(data.Slice(0, 4), line);

      var copyStream = new MemoryStream();

      Assert.AreEqual(4L, stream.Read(copyStream, 4L));

      Assert.AreEqual(8L, stream.Position, "Position");

      copyStream.Dispose();

      Assert.AreEqual(data.Slice(4, 4), copyStream.ToArray());
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStreamLongerThanBuffered(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF, 0x42, 0x43, 0x44, Ascii.Octets.CR, Ascii.Octets.LF, 0x45, 0x46, 0x47};
      var stream = CreateStream(type, new MemoryStream(data), 8);

      var line = stream.ReadLine(true);

      Assert.AreEqual(4L, stream.Position, "Position");

      Assert.AreEqual(data.Slice(0, 4), line);

      var copyStream = new MemoryStream();

      Assert.AreEqual(8L, stream.Read(copyStream, 10L));

      Assert.AreEqual(12L, stream.Position, "Position");

      copyStream.Dispose();

      Assert.AreEqual(data.Slice(4, 8), copyStream.ToArray());
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestClose(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};

      using (var stream = CreateStream(type, new MemoryStream(data), 8)) {
        stream.Dispose();

        Assert.IsFalse(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

        var buffer = new byte[8];

        Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(0, stream.Position));
        Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(8, stream.Length));
        Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(8, stream.BufferSize));
        Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.InnerStream));
        Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.NewLine));
        Assert.Throws<ObjectDisposedException>(() => stream.ReadLine());
        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
        Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00));
        Assert.Throws<ObjectDisposedException>(() => stream.Write(buffer, 0, 8));

        stream.Dispose();
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestCloseLeaveStreamOpen(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};

      using (var baseStream = new MemoryStream(data)) {
        var stream = CreateStream(type, baseStream, 8, true);

        stream.Dispose();

        Assert.IsFalse(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00));

        Assert.DoesNotThrow(() => baseStream.ReadByte());
        Assert.DoesNotThrow(() => baseStream.WriteByte(0x00));

        stream.Dispose();
      }
    }
  }
}
