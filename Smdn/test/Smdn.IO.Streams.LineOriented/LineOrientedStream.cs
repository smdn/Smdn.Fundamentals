using System;
using System.Buffers;
using System.IO;
using System.Threading;
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

        Assert.IsNull(ret);
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

        Assert.IsNull(ret);
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public async Task TestReadLineAsync_SingleSegment(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF};

      using (var stream = CreateStream(type, new MemoryStream(data), bufferSize: 8)) {
        var ret = await stream.ReadLineAsync();

        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Value.IsEmpty);
        Assert.IsTrue(ret.Value.SequenceWithNewLine.IsSingleSegment);
        Assert.IsTrue(ret.Value.Sequence.IsSingleSegment);
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43 },
                                  ret.Value.Sequence.ToArray());
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

        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Value.IsEmpty);
        Assert.IsFalse(ret.Value.SequenceWithNewLine.IsSingleSegment);
        Assert.IsFalse(ret.Value.Sequence.IsSingleSegment);
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f },
                                  ret.Value.Sequence.ToArray());
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadByte(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
      var stream = CreateStream(type, new MemoryStream(data), 8);
      var index = 0;

      for (; ; ) {
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
    public void TestReadToStream_ArgumentNull_TargetStream(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws<ArgumentNullException>(() => stream.Read(null, 16L));
      Assert.Throws<ArgumentNullException>(() => stream.ReadAsync(null, 16L));
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStream_ArgumentOutOfRange_Length(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(Stream.Null, -1L));
      Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(Stream.Null, -1L));
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStream_LengthZero(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);
      var copyStream = new MemoryStream();

      Assert.AreEqual(0L, stream.Read(copyStream, 0L));
      Assert.AreEqual(0L, stream.Position, "Position");

      copyStream.Dispose();

      Assert.AreEqual(new byte[0], copyStream.ToArray());
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStreamAsync_LengthZero(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);
      var copyStream = new MemoryStream();

      var t = stream.ReadAsync(copyStream, 0L);

      Assert.IsTrue(t.IsCompleted);
      Assert.AreEqual(0L, t.GetAwaiter().GetResult());
      Assert.AreEqual(0L, stream.Position, "Position");

      copyStream.Dispose();

      Assert.AreEqual(new byte[0], copyStream.ToArray());
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStreamAsync_CancelledToken(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);

      using (var cts = new CancellationTokenSource()) {
        cts.Cancel();

        var t = stream.ReadAsync(Stream.Null, 0L, cts.Token);

        Assert.IsTrue(t.IsCanceled);
        Assert.That(async () => await t, Throws.InstanceOf<OperationCanceledException>());

        Assert.AreEqual(0L, stream.Position, "Position");
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadToStream_BufferEmpty(StreamType type)
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
    public void TestRead_BufferEmpty(StreamType type)
    {
      var data = new byte[] {0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF, 0x42, 0x43, 0x44, Ascii.Octets.CR, Ascii.Octets.LF, 0x45, 0x46, 0x47};
      var stream = CreateStream(type, new MemoryStream(data), 8);

      var buffer = new byte[12];

      Assert.AreEqual(12L, stream.Read(buffer, 0, 12));

      Assert.AreEqual(12L, stream.Position, "Position");

      Assert.AreEqual(data, buffer);
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestRead_LessThanBuffered(StreamType type)
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
    public void TestRead_LongerThanBuffered(StreamType type)
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
    public void TestReadToStream_LessThanBuffered(StreamType type)
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
    public void TestReadToStream_LongerThanBuffered(StreamType type)
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
    public void TestRead_LengthZero(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);
      var buffer = new byte[1];

      Assert.AreEqual(0, stream.Read(buffer, 0, 0));

      Assert.AreEqual(0L, stream.Position, "Position");
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadAsync_LengthZero(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);
      var buffer = new byte[1];

      var t = stream.ReadAsync(buffer, 0, 0);

      Assert.IsTrue(t.IsCompleted);
      Assert.AreEqual(0L, t.GetAwaiter().GetResult());
      Assert.AreEqual(0L, stream.Position, "Position");
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestReadAsync_CancelledToken(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);

      using (var cts = new CancellationTokenSource()) {
        cts.Cancel();

        var t = stream.ReadAsync(new byte[1], 0, 1, cts.Token);

        Assert.IsTrue(t.IsCanceled);
        Assert.That(async () => await t, Throws.InstanceOf<OperationCanceledException>());

        Assert.AreEqual(0L, stream.Position, "Position");
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public async Task TestCopyToAsync(StreamType type)
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
      };

      using (var stream = CreateStream(type, new MemoryStream(data), 8)) {
        Assert.AreEqual(data[0], stream.ReadByte());

        var targetStream = new MemoryStream();

        await stream.CopyToAsync(targetStream, 0, cancellationToken: default);

        CollectionAssert.AreEqual(data.Slice(1), targetStream.ToArray());

        Assert.AreEqual(-1, stream.ReadByte());
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public async Task TestCopyToAsync_NothingBuffered(StreamType type)
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
      };

      using (var stream = CreateStream(type, new MemoryStream(data), 8)) {
        var targetStream = new MemoryStream();

        await stream.CopyToAsync(targetStream, 0, cancellationToken: default);

        CollectionAssert.AreEqual(data, targetStream.ToArray());

        Assert.AreEqual(-1, stream.ReadByte());
      }
    }

    [TestCase(StreamType.Strict)]
    [TestCase(StreamType.Loose)]
    public void TestCopyToAsync_ArgumentNull_Destination(StreamType type)
    {
      var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null));
      Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null, bufferSize: 0));
      Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null, bufferSize: 0, cancellationToken: default));
    }

    [TestCase(StreamType.Strict, -1)]
    [TestCase(StreamType.Loose, -1)]
    [TestCase(StreamType.Strict, 0)]
    [TestCase(StreamType.Loose, 0)]
    [TestCase(StreamType.Strict, int.MaxValue)]
    [TestCase(StreamType.Loose, int.MaxValue)]
    public async Task TestCopyToAsync_BufferSizeShouldBeIgnored(StreamType type, int bufferSize)
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
      };

      using (var stream = CreateStream(type, new MemoryStream(data), 8)) {
        var targetStream = new MemoryStream();

        await stream.CopyToAsync(targetStream, bufferSize: bufferSize, cancellationToken: default);

        CollectionAssert.AreEqual(data, targetStream.ToArray());

        Assert.AreEqual(-1, stream.ReadByte());
      }
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
        Assert.Throws<ObjectDisposedException>(() => Assert.IsFalse(stream.NewLine.IsEmpty));
        Assert.Throws<ObjectDisposedException>(() => stream.ReadLine());
        Assert.Throws<ObjectDisposedException>(() => stream.ReadLineAsync());
        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
        Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(buffer, 0, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.Read(Stream.Null, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(Stream.Null, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.Flush());
        Assert.Throws<ObjectDisposedException>(() => stream.FlushAsync());
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00));
        Assert.Throws<ObjectDisposedException>(() => stream.Write(buffer, 0, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(buffer, 0, 8));
        Assert.Throws<ObjectDisposedException>(() => stream.CopyToAsync(Stream.Null));

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
