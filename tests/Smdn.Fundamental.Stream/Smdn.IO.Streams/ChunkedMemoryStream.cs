// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

using Smdn;

namespace Smdn.IO.Streams {
  [TestFixture]
  public class ChunkedMemoryStreamTests {
    [Test]
    public void TestProperties()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.CanRead, Is.True, "can read");
        Assert.That(stream.CanWrite, Is.True, "can write");
        Assert.That(stream.CanSeek, Is.True, "can seek");
        Assert.That(stream.Length,Is.Zero);
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.ChunkSize, Is.EqualTo(8));
      }
    }

    private static void WriteLog(string format, params object[] arg)
    {
#if false
      TestContext.Out.WriteLine(format, arg);
#endif
    }

    private class TestChunk : ChunkedMemoryStream.Chunk {
      public TestChunk(int size, List<TestChunk> list)
      {
        this.list = list;
        this.index = list.Count;
        base.Data = new byte[size];
        list.Add(this);

        WriteLog("allocated [{0}]", index);
      }

      public override void Dispose()
      {
        list.Remove(this);

        WriteLog("disposed [{0}]", index);
      }

      private int index;
      private List<TestChunk> list;
    }

    [Test]
    public void TestAllocateDisposeChunkEmptyStream()
    {
      var allocated = new List<TestChunk>();

      using (var stream = new ChunkedMemoryStream(4, delegate(int size) {
        Assert.That(size, Is.EqualTo(4));
        return new TestChunk(size, allocated);
      })) {
        Assert.That(allocated.Count, Is.Zero, "first chunk");

        stream.Dispose();

        Assert.That(allocated.Count, Is.Zero, "closed");
      }
    }

    [Test]
    public void TestAllocateDisposeChunk()
    {
      var allocated = new List<TestChunk>();

      using (var stream = new ChunkedMemoryStream(4, delegate(int size) {
        Assert.That(size, Is.EqualTo(4));
        return new TestChunk(size, allocated);
      })) {
        Assert.That(allocated.Count, Is.Zero, "first chunk");

        var writer = new System.IO.BinaryWriter(stream);

        writer.Write(new byte[] {0x00, 0x01, 0x02});
        writer.Flush();

        Assert.That(stream.Length, Is.EqualTo(3L));
        Assert.That(allocated.Count, Is.EqualTo(1));

        writer.Write(new byte[] {0x03, 0x04, 0x05});
        writer.Flush();

        Assert.That(stream.Length, Is.EqualTo(6L));
        Assert.That(allocated.Count, Is.EqualTo(2), "extended by Write 1");

        writer.Write(new byte[] {0x06, 0x07, 0x08});
        writer.Flush();

        Assert.That(stream.Length, Is.EqualTo(9L));
        Assert.That(allocated.Count, Is.EqualTo(3), "extended by Write 2");

        stream.SetLength(stream.Length);
        Assert.That(stream.Length, Is.EqualTo(9L));
        Assert.That(allocated.Count, Is.EqualTo(3), "SetLength(stream.Length)");

        WriteLog("set length 12");
        stream.SetLength(12L);
        Assert.That(stream.Length, Is.EqualTo(12L));
        Assert.That(allocated.Count, Is.EqualTo(4), "extended by SetLength 1");

        WriteLog("set length 8");
        stream.SetLength(8L);
        Assert.That(stream.Length, Is.EqualTo(8L));
        Assert.That(allocated.Count, Is.EqualTo(3), "shorten by SetLength 1");

        WriteLog("set length 7");
        stream.SetLength(7L);
        Assert.That(stream.Length, Is.EqualTo(7L));
        Assert.That(allocated.Count, Is.EqualTo(2), "shorten by SetLength 2");

        WriteLog("set length 3");
        stream.SetLength(3L);
        Assert.That(stream.Length, Is.EqualTo(3L));
        Assert.That(allocated.Count, Is.EqualTo(1), "shorten by SetLength 3");

        WriteLog("set length 0");
        stream.SetLength(0L);
        Assert.That(stream.Length,Is.Zero);
        Assert.That(allocated.Count, Is.EqualTo(1), "shorten by SetLength 4");

        WriteLog("set length 12");
        stream.SetLength(12L);
        Assert.That(stream.Length, Is.EqualTo(12L));
        Assert.That(allocated.Count, Is.EqualTo(4), "extended by SetLength 2");

        WriteLog("set length 0");
        stream.SetLength(0L);
        Assert.That(stream.Length,Is.Zero);
        Assert.That(allocated.Count, Is.EqualTo(1), "shorten by SetLength 5");
      }

      Assert.That(allocated.Count, Is.Zero, "closed");
    }

    [Test]
    public void TestBehaveLikeMemoryStream()
    {
      var memoryStream = new MemoryStream();
      var chunkedStream = new ChunkedMemoryStream(4);
      var results = new List<byte[]>();

      foreach (var stream in new Stream[] {memoryStream, chunkedStream}) {
        Assert.That(stream.CanRead, Is.True, "CanRead");
        Assert.That(stream.CanWrite, Is.True, "CanWrite");
        Assert.That(stream.CanSeek, Is.True, "CanSeek");
        Assert.That(stream.CanTimeout, Is.False, "CanTimeout");

        var writer = new BinaryWriter(stream);
        var reader = new BinaryReader(stream);

        writer.Write(new byte[] {0x00, 0x01, 0x02, 0x03});
        writer.Flush();

        stream.Position = 2L;

        writer.Write(new byte[] {0x04, 0x05, 0x06, 0x07});
        writer.Write(new byte[] {0x08, 0x09, 0x0a, 0x0b});

        stream.Seek(-2L, SeekOrigin.Current);

        Assert.That(System.Net.IPAddress.HostToNetworkOrder(reader.ReadInt16()), Is.EqualTo(0x0a0b));

        stream.Seek(4L, SeekOrigin.Begin);

        writer.Write(new byte[] {0x0c, 0x0d, 0x0e, 0x0f});

        stream.Seek(-10L, SeekOrigin.End);

        Assert.That(System.Net.IPAddress.HostToNetworkOrder(reader.ReadInt32()), Is.EqualTo(0x00010405));

        Assert.That(stream.Position, Is.EqualTo(4L));
        Assert.That(stream.Length, Is.EqualTo(10L));

        stream.Position = 0L;

        results.Add(reader.ReadBytes((int)stream.Length));

        reader.Dispose();

        Assert.That(stream.CanRead, Is.False, "CanRead");
        Assert.That(stream.CanWrite, Is.False, "CanWrite");
        Assert.That(stream.CanSeek, Is.False, "CanSeek");
        Assert.That(stream.CanTimeout, Is.False, "CanTimeout");

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }

      Assert.That(results[1], Is.EqualTo(results[0]));
    }

    [Test]
    public void TestClose()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        stream.Dispose();

        Assert.That(stream.CanRead, Is.False, "CanRead");
        Assert.That(stream.CanWrite, Is.False, "CanWrite");
        Assert.That(stream.CanSeek, Is.False, "CanSeek");
        Assert.That(stream.CanTimeout, Is.False, "CanTimeout");

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());

        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00));

        stream.Dispose();
      }
    }

    [Test]
    public void TestPosition()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 12; i++) {
          Assert.That(stream.Position, Is.EqualTo((long)i));
          stream.WriteByte((byte)i);
          Assert.That(stream.Position, Is.EqualTo((long)i + 1));
        }

        Assert.Throws<ArgumentOutOfRangeException>(() => stream.Position = -1L);

        for (var expected = 0L; expected < 12L; expected++) {
          stream.Position = expected;

          Assert.That(stream.ReadByte(), Is.EqualTo(expected));
          Assert.That(stream.Position, Is.EqualTo(expected + 1));
        }

        stream.Position = 13; // no exception will be thrown
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));

        Assert.That(stream.Position, Is.EqualTo(13L));
        Assert.That(stream.Length, Is.EqualTo(13L));
      }
    }

    [Test]
    public void TestSetLength()
    {
      using (var stream = new ChunkedMemoryStream(4)) {
        for (var len = 0L; len < 12L; len++) {
          stream.SetLength(len);
          Assert.That(stream.Length, Is.EqualTo(len));
          Assert.That(stream.Position,Is.Zero);
        }

        stream.Position = stream.Length;

        for (var len = 12L; len <= 0L; len--) {
          stream.SetLength(len);
          Assert.That(stream.Length, Is.EqualTo(len));
          Assert.That(stream.Position, Is.EqualTo(len));
        }

        stream.SetLength(22L);
        Assert.That(stream.Length, Is.EqualTo(22L));

        stream.Position = 22L;

        stream.SetLength(14L);
        Assert.That(stream.Length, Is.EqualTo(14L));
        Assert.That(stream.Position, Is.EqualTo(14L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
        Assert.That(stream.Position, Is.EqualTo(14L));

        stream.SetLength(3L);
        Assert.That(stream.Length, Is.EqualTo(3L));
        Assert.That(stream.Position, Is.EqualTo(3L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
        Assert.That(stream.Position, Is.EqualTo(3L));

        stream.SetLength(13L);
        Assert.That(stream.Length, Is.EqualTo(13L));
        Assert.That(stream.Position, Is.EqualTo(3L));
        Assert.That(stream.ReadByte(), Is.Zero);
        Assert.That(stream.Position, Is.EqualTo(4L));
      }
    }

    [Test]
    public void TestSetLengthEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(4)) {
        Assert.That(stream.Length,Is.Zero);
        Assert.That(stream.Position,Is.Zero);

        stream.SetLength(2L);

        Assert.That(stream.Length, Is.EqualTo(2L));
        Assert.That(stream.Position,Is.Zero);
      }

      using (var stream = new ChunkedMemoryStream(4)) {
        Assert.That(stream.Length,Is.Zero);
        Assert.That(stream.Position,Is.Zero);

        stream.SetLength(6L);

        Assert.That(stream.Length, Is.EqualTo(6L));
        Assert.That(stream.Position,Is.Zero);
      }
    }

    [Test]
    public void TestSetLengthNegative()
    {
      using (var stream = new ChunkedMemoryStream(4)) {
        Assert.Throws<ArgumentOutOfRangeException>(() => stream.SetLength(-1));
      }
    }

    [Test]
    public void TestSeekBegin()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 0x20; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.That(stream.Seek(0x00, SeekOrigin.Begin), Is.Zero);
        Assert.That(stream.ReadByte(), Is.Zero);

        Assert.That(stream.Seek(0x18, SeekOrigin.Begin), Is.EqualTo(0x18));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x18));

        Assert.That(stream.Seek(0x0f, SeekOrigin.Begin), Is.EqualTo(0x0f));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x0f));

        Assert.That(stream.Seek(0x40, SeekOrigin.Begin), Is.EqualTo(0x40));

        Assert.Throws<IOException>(() => stream.Seek(-1, SeekOrigin.Begin));
      }
    }

    [Test]
    public void TestSeekCurrent()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 0x20; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.That(stream.Seek(-0x20, SeekOrigin.Current), Is.Zero);
        Assert.That(stream.ReadByte(), Is.Zero);

        Assert.That(stream.Seek(+0x17, SeekOrigin.Current), Is.EqualTo(0x18));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x18));

        Assert.That(stream.Seek(-0x0a, SeekOrigin.Current), Is.EqualTo(0x0f));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x0f));

        Assert.That(stream.Seek(+0x30, SeekOrigin.Current), Is.EqualTo(0x40));

        Assert.Throws<IOException>(() => stream.Seek(-0x41, SeekOrigin.Current));
      }
    }

    [Test]
    public void TestSeekEnd()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 0x20; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.That(stream.Seek(-0x20, SeekOrigin.End), Is.Zero);
        Assert.That(stream.ReadByte(), Is.Zero);

        Assert.That(stream.Seek(-0x08, SeekOrigin.End), Is.EqualTo(0x18));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x18));

        Assert.That(stream.Seek(-0x11, SeekOrigin.End), Is.EqualTo(0x0f));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x0f));

        Assert.That(stream.Seek(+0x20, SeekOrigin.End), Is.EqualTo(0x40));

        Assert.Throws<IOException>(() => stream.Seek(-0x41, SeekOrigin.End));
      }
    }

    [Test]
    public void TestSeekInternalStateNotChanged1()
    {
      using (var stream = new ChunkedMemoryStream()) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        Assert.That(stream.Seek(0L, SeekOrigin.Begin),Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        Assert.That(stream.Position, Is.EqualTo(4L));
        Assert.That(stream.Length, Is.EqualTo(4L));
      }
    }

    [Test]
    public void TestSeekInternalStateNotChanged2()
    {
      using (var stream = new ChunkedMemoryStream()) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        stream.Position = 0L;

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        Assert.That(stream.Position, Is.EqualTo(4L));
        Assert.That(stream.Length, Is.EqualTo(4L));
      }
    }

    [Test]
    public void TestReadByte()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 32; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.That(stream.Seek(0L, SeekOrigin.Begin),Is.Zero);

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length, Is.EqualTo(32L));

        for (var i = 0; i < 32; i++) {
          Assert.That(stream.Position, Is.EqualTo((long)i));
          Assert.That(stream.ReadByte(), Is.EqualTo(i));
          Assert.That(stream.Position, Is.EqualTo((long)i + 1));
        }

        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
      }
    }

    [Test]
    public void TestRead()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 32; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.That(stream.Seek(0, SeekOrigin.Begin),Is.Zero);

        var buffer = new byte[16];

        Assert.That(stream.Read(buffer, 0, 1), Is.EqualTo(1));
        Assert.That(buffer.Skip(0).Take(1).ToArray(), Is.EqualTo(new byte[] {0x00}));
        Assert.That(stream.Position, Is.EqualTo(1L));

        Assert.That(stream.Read(buffer, 0, 3), Is.EqualTo(3));
        Assert.That(buffer.Skip(0).Take(3).ToArray(), Is.EqualTo(new byte[] {0x01, 0x02, 0x03}));
        Assert.That(stream.Position, Is.EqualTo(4L));

        Assert.That(stream.Read(buffer, 0, 4), Is.EqualTo(4));
        Assert.That(buffer.Skip(0).Take(4).ToArray(), Is.EqualTo(new byte[] {0x04, 0x05, 0x06, 0x07}));
        Assert.That(stream.Position, Is.EqualTo(8L));

        Assert.That(stream.Read(buffer, 0, 7), Is.EqualTo(7));
        Assert.That(buffer.Skip(0).Take(7).ToArray(), Is.EqualTo(new byte[] {0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e}));
        Assert.That(stream.Position, Is.EqualTo(15L));

        Assert.That(stream.Read(buffer, 0, 2), Is.EqualTo(2));
        Assert.That(buffer.Skip(0).Take(2).ToArray(), Is.EqualTo(new byte[] {0x0f, 0x10}));
        Assert.That(stream.Position, Is.EqualTo(17L));

        Assert.That(stream.Read(buffer, 0, 16), Is.EqualTo(15));
        Assert.That(buffer.Skip(0).Take(15).ToArray(), Is.EqualTo(new byte[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f}));
        Assert.That(stream.Position, Is.EqualTo(32L));
      }
    }

    [Test]
    public void TestReadZeroBytes()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 32; i++) {
          stream.WriteByte((byte)i);
        }

        var buffer = new byte[4];

        Assert.That(stream.Seek(0, SeekOrigin.Begin),Is.Zero);

        Assert.That(stream.Read(buffer, 0, 0), Is.Zero);
        Assert.That(buffer, Is.EqualTo(new byte[4]));

        Assert.That(stream.Position,Is.Zero);

        Assert.That(stream.Seek(8, SeekOrigin.Begin), Is.EqualTo(8L));

        Assert.That(stream.Read(buffer, 0, 0), Is.Zero);
        Assert.That(buffer, Is.EqualTo(new byte[4]));

        Assert.That(stream.Position, Is.EqualTo(8L));
      }
    }

    [Test]
    public void TestReadByteFromEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        Assert.That(stream.ReadByte(), Is.EqualTo(-1));

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);
      }
    }

    [Test]
    public void TestReadFromEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        var buffer = new byte[4];

        Assert.That(stream.Read(buffer, 0, 4), Is.Zero);

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);
      }

      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        var buffer = new byte[12];

        Assert.That(stream.Read(buffer, 0, 12), Is.Zero);

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);
      }
    }

    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
    )]
    public void TestRead_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      using var stream = new ChunkedMemoryStream(8);

      Assert.Throws(expectedExceptionType, () => stream.Read(buffer, offset, count));
    }

#if false
    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
    )]
    public void TestReadAsync_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      using var stream = new ChunkedMemoryStream(8);

      Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count));
    }
#endif

    [Test]
    public void TestWriteByte()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        for (var i = 0; i < 32; i++) {
          Assert.That(stream.Position, Is.EqualTo((long)i));

          stream.WriteByte((byte)i);

          Assert.That(stream.Position, Is.EqualTo((long)(i + 1)));
          Assert.That(stream.Length, Is.EqualTo((long)(i + 1)));
        }

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
        }));
      }
    }

    [Test]
    public void TestWrite()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        stream.Write(new byte[] {0x00}, 0, 1);
        Assert.That(stream.Position, Is.EqualTo(1L));
        Assert.That(stream.Length, Is.EqualTo(1L));

        stream.Write(new byte[] {0x01, 0x02, 0x03}, 0, 3);
        Assert.That(stream.Position, Is.EqualTo(4L));
        Assert.That(stream.Length, Is.EqualTo(4L));

        stream.Write(new byte[] {0x04, 0x05, 0x06, 0x07}, 0, 4);
        Assert.That(stream.Position, Is.EqualTo(8L));
        Assert.That(stream.Length, Is.EqualTo(8L));

        stream.Write(new byte[] {0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e}, 0, 7);
        Assert.That(stream.Position, Is.EqualTo(15L));
        Assert.That(stream.Length, Is.EqualTo(15L));

        stream.Write(new byte[] {0x0f, 0x10}, 0, 2);
        Assert.That(stream.Position, Is.EqualTo(17L));
        Assert.That(stream.Length, Is.EqualTo(17L));

        stream.Write(new byte[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f}, 0, 15);
        Assert.That(stream.Position, Is.EqualTo(32L));
        Assert.That(stream.Length, Is.EqualTo(32L));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
        }));
      }
    }

    [Test]
    public void TestWriteOverwrite()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero, "position0");
        Assert.That(stream.Length,Is.Zero, "length0");

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);
        Assert.That(stream.Position, Is.EqualTo(4L), "position1");
        Assert.That(stream.Length, Is.EqualTo(4L), "length1");

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03,
        }));

        Assert.That(stream.Seek(-4L, SeekOrigin.Current),Is.Zero);
        Assert.That(stream.Position,Is.Zero, "position2");
        Assert.That(stream.Length, Is.EqualTo(4L), "length2");

        stream.Write(new byte[] {0x04, 0x05, 0x06, 0x07, 0x08, 0x09}, 0, 6);
        Assert.That(stream.Position, Is.EqualTo(6L), "position3");
        Assert.That(stream.Length, Is.EqualTo(6L), "length3");

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
        }));

        Assert.That(stream.Seek(0L, SeekOrigin.Begin),Is.Zero);
        Assert.That(stream.Position,Is.Zero, "position4");
        Assert.That(stream.Length, Is.EqualTo(6L), "length4");

        stream.Write(new byte[] {0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11}, 0, 8);
        Assert.That(stream.Position, Is.EqualTo(8L), "position5");
        Assert.That(stream.Length, Is.EqualTo(8L), "length5");

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11,
        }));

        Assert.That(stream.Seek(-2L, SeekOrigin.End), Is.EqualTo(6L));
        Assert.That(stream.Position, Is.EqualTo(6L), "position6");
        Assert.That(stream.Length, Is.EqualTo(8L), "length6");

        stream.Write(new byte[] {0x12, 0x13, 0x14, 0x15}, 0, 4);
        Assert.That(stream.Position, Is.EqualTo(10L), "position7");
        Assert.That(stream.Length, Is.EqualTo(10L), "length7");

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x12, 0x13,
          0x14, 0x15,
        }));

        Assert.That(stream.Seek(-2L, SeekOrigin.Current), Is.EqualTo(8L));
        Assert.That(stream.Position, Is.EqualTo(8L), "position8");
        Assert.That(stream.Length, Is.EqualTo(10L), "length8");

        stream.Write(new byte[] {0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b}, 0, 6);
        Assert.That(stream.Position, Is.EqualTo(14L), "position9");
        Assert.That(stream.Length, Is.EqualTo(14L), "length9");

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x12, 0x13,
          0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b,
        }));

        Assert.That(stream.Seek(-6L, SeekOrigin.Current), Is.EqualTo(8L));
        Assert.That(stream.Position, Is.EqualTo(8L), "position10");
        Assert.That(stream.Length, Is.EqualTo(14L), "length10");

        stream.Write(new byte[] {0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25}, 0, 10);
        Assert.That(stream.Position, Is.EqualTo(18L), "position11");
        Assert.That(stream.Length, Is.EqualTo(18L), "length11");

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x12, 0x13,
          0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23,
          0x24, 0x25,
        }));
      }
    }

    [Test]
    public void TestWriteZeroBytesToEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        stream.Write(new byte[0], 0, 0);

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        stream.Write(new byte[8], 0, 0);

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);
      }
    }

    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidWriteBufferArguments)
    )]
    public void TestWrite_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      using var stream = new ChunkedMemoryStream(8);

      Assert.Throws(expectedExceptionType, () => stream.Write(buffer, offset, count));
    }

#if false
    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidWriteBufferArguments)
    )]
    public void TestWriteAsync_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      using var stream = new ChunkedMemoryStream(8);

      Assert.Throws(expectedExceptionType, () => stream.WriteAsync(buffer, offset, count));
    }
#endif

    [Test]
    public void TestSeekAndReadRandom()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 12; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.That(stream.CanSeek, Is.True);
        Assert.That(stream.Position, Is.EqualTo(12L));
        Assert.That(stream.Length, Is.EqualTo(12L));

        Assert.That(stream.Seek(6L, SeekOrigin.Begin), Is.EqualTo(6L));
        Assert.That(stream.Position, Is.EqualTo(6L));

        var pair = new long[][] {
          // offset / position
          new long[] { 0, 6},
          new long[] {-2, 5},
          new long[] { 1, 7},
          new long[] {-4, 4},
          new long[] { 3, 8},
          new long[] {-6, 3},
          new long[] { 5, 9},
          new long[] {-8, 2},
          new long[] { 7,10},
          new long[] {-10, 1},
          new long[] { 9,11},
        };

        for (var index = 0; index < pair.Length; index++) {
          try {
            Assert.That(stream.Seek(pair[index][0], SeekOrigin.Current), Is.EqualTo(pair[index][1]), $"sought position {index}");
          }
          catch (IOException) {
            Assert.Fail($"IOException thrown while seeking ({index})");
          }

          Assert.That(stream.Position, Is.EqualTo(pair[index][1]));
          Assert.That(stream.ReadByte(), Is.EqualTo(pair[index][1]), $"read value {index}");
          Assert.That(stream.Position, Is.EqualTo(pair[index][1] + 1));
        }

        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
        Assert.That(stream.Seek(1, SeekOrigin.Current), Is.EqualTo(13));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
        Assert.That(stream.Position, Is.EqualTo(13));
      }
    }
    [Test]
    public void TestToArray()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[0]));

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        Assert.That(stream.Position, Is.EqualTo(4L));
        Assert.That(stream.Length, Is.EqualTo(4L));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03
        }));

        stream.Write(new byte[] {0x04, 0x05, 0x06, 0x07}, 0, 4);

        Assert.That(stream.Position, Is.EqualTo(8L));
        Assert.That(stream.Length, Is.EqualTo(8L));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
        }));

        stream.Write(new byte[] {0x08, 0x09, 0x0a, 0x0b}, 0, 4);

        Assert.That(stream.Position, Is.EqualTo(12L));
        Assert.That(stream.Length, Is.EqualTo(12L));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b,
        }));

        // shorten
        stream.SetLength(6L);

        Assert.That(stream.Position, Is.EqualTo(6L));
        Assert.That(stream.Length, Is.EqualTo(6L));

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
        }));

        stream.SetLength(0L);

        Assert.That(stream.Position,Is.Zero);
        Assert.That(stream.Length,Is.Zero);

        Assert.That(stream.ToArray(), Is.EqualTo(new byte[0]));
      }
    }
  }
}
