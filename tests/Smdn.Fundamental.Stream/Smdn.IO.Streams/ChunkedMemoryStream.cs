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
        Assert.IsTrue(stream.CanRead, "can read");
        Assert.IsTrue(stream.CanWrite, "can write");
        Assert.IsTrue(stream.CanSeek, "can seek");
        Assert.AreEqual(0L, stream.Length);
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(8, stream.ChunkSize);
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
        Assert.AreEqual(4, size);
        return new TestChunk(size, allocated);
      })) {
        Assert.AreEqual(0, allocated.Count, "first chunk");

        stream.Dispose();

        Assert.AreEqual(0, allocated.Count, "closed");
      }
    }

    [Test]
    public void TestAllocateDisposeChunk()
    {
      var allocated = new List<TestChunk>();

      using (var stream = new ChunkedMemoryStream(4, delegate(int size) {
        Assert.AreEqual(4, size);
        return new TestChunk(size, allocated);
      })) {
        Assert.AreEqual(0, allocated.Count, "first chunk");

        var writer = new System.IO.BinaryWriter(stream);

        writer.Write(new byte[] {0x00, 0x01, 0x02});
        writer.Flush();

        Assert.AreEqual(3L, stream.Length);
        Assert.AreEqual(1, allocated.Count);

        writer.Write(new byte[] {0x03, 0x04, 0x05});
        writer.Flush();

        Assert.AreEqual(6L, stream.Length);
        Assert.AreEqual(2, allocated.Count, "extended by Write 1");

        writer.Write(new byte[] {0x06, 0x07, 0x08});
        writer.Flush();

        Assert.AreEqual(9L, stream.Length);
        Assert.AreEqual(3, allocated.Count, "extended by Write 2");

        stream.SetLength(stream.Length);
        Assert.AreEqual(9L, stream.Length);
        Assert.AreEqual(3, allocated.Count, "SetLength(stream.Length)");

        WriteLog("set length 12");
        stream.SetLength(12L);
        Assert.AreEqual(12L, stream.Length);
        Assert.AreEqual(4, allocated.Count, "extended by SetLength 1");

        WriteLog("set length 8");
        stream.SetLength(8L);
        Assert.AreEqual(8L, stream.Length);
        Assert.AreEqual(3, allocated.Count, "shorten by SetLength 1");

        WriteLog("set length 7");
        stream.SetLength(7L);
        Assert.AreEqual(7L, stream.Length);
        Assert.AreEqual(2, allocated.Count, "shorten by SetLength 2");

        WriteLog("set length 3");
        stream.SetLength(3L);
        Assert.AreEqual(3L, stream.Length);
        Assert.AreEqual(1, allocated.Count, "shorten by SetLength 3");

        WriteLog("set length 0");
        stream.SetLength(0L);
        Assert.AreEqual(0L, stream.Length);
        Assert.AreEqual(1, allocated.Count, "shorten by SetLength 4");

        WriteLog("set length 12");
        stream.SetLength(12L);
        Assert.AreEqual(12L, stream.Length);
        Assert.AreEqual(4, allocated.Count, "extended by SetLength 2");

        WriteLog("set length 0");
        stream.SetLength(0L);
        Assert.AreEqual(0L, stream.Length);
        Assert.AreEqual(1, allocated.Count, "shorten by SetLength 5");
      }

      Assert.AreEqual(0, allocated.Count, "closed");
    }

    [Test]
    public void TestBehaveLikeMemoryStream()
    {
      var memoryStream = new MemoryStream();
      var chunkedStream = new ChunkedMemoryStream(4);
      var results = new List<byte[]>();

      foreach (var stream in new Stream[] {memoryStream, chunkedStream}) {
        Assert.IsTrue(stream.CanRead, "CanRead");
        Assert.IsTrue(stream.CanWrite, "CanWrite");
        Assert.IsTrue(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

        var writer = new BinaryWriter(stream);
        var reader = new BinaryReader(stream);

        writer.Write(new byte[] {0x00, 0x01, 0x02, 0x03});
        writer.Flush();

        stream.Position = 2L;

        writer.Write(new byte[] {0x04, 0x05, 0x06, 0x07});
        writer.Write(new byte[] {0x08, 0x09, 0x0a, 0x0b});

        stream.Seek(-2L, SeekOrigin.Current);

        Assert.AreEqual(0x0a0b, System.Net.IPAddress.HostToNetworkOrder(reader.ReadInt16()));

        stream.Seek(4L, SeekOrigin.Begin);

        writer.Write(new byte[] {0x0c, 0x0d, 0x0e, 0x0f});

        stream.Seek(-10L, SeekOrigin.End);

        Assert.AreEqual(0x00010405, System.Net.IPAddress.HostToNetworkOrder(reader.ReadInt32()));

        Assert.AreEqual(4L, stream.Position);
        Assert.AreEqual(10L, stream.Length);

        stream.Position = 0L;

        results.Add(reader.ReadBytes((int)stream.Length));

        reader.Dispose();

        Assert.IsFalse(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }

      Assert.AreEqual(results[0], results[1]);
    }

    [Test]
    public void TestClose()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        stream.Dispose();

        Assert.IsFalse(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

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
          Assert.AreEqual((long)i, stream.Position);
          stream.WriteByte((byte)i);
          Assert.AreEqual((long)i + 1, stream.Position);
        }

        Assert.Throws<ArgumentOutOfRangeException>(() => stream.Position = -1L);

        for (var expected = 0L; expected < 12L; expected++) {
          stream.Position = expected;

          Assert.AreEqual(expected, stream.ReadByte());
          Assert.AreEqual(expected + 1, stream.Position);
        }

        stream.Position = 13; // no exception will be thrown
        Assert.AreEqual(-1, stream.ReadByte());

        Assert.AreEqual(13L, stream.Position);
        Assert.AreEqual(13L, stream.Length);
      }
    }

    [Test]
    public void TestSetLength()
    {
      using (var stream = new ChunkedMemoryStream(4)) {
        for (var len = 0L; len < 12L; len++) {
          stream.SetLength(len);
          Assert.AreEqual(len, stream.Length);
          Assert.AreEqual(0L, stream.Position);
        }

        stream.Position = stream.Length;

        for (var len = 12L; len <= 0L; len--) {
          stream.SetLength(len);
          Assert.AreEqual(len, stream.Length);
          Assert.AreEqual(len, stream.Position);
        }

        stream.SetLength(22L);
        Assert.AreEqual(22L, stream.Length);

        stream.Position = 22L;

        stream.SetLength(14L);
        Assert.AreEqual(14L, stream.Length);
        Assert.AreEqual(14L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());
        Assert.AreEqual(14L, stream.Position);

        stream.SetLength(3L);
        Assert.AreEqual(3L, stream.Length);
        Assert.AreEqual(3L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());
        Assert.AreEqual(3L, stream.Position);

        stream.SetLength(13L);
        Assert.AreEqual(13L, stream.Length);
        Assert.AreEqual(3L, stream.Position);
        Assert.AreEqual(0, stream.ReadByte());
        Assert.AreEqual(4L, stream.Position);
      }
    }

    [Test]
    public void TestSetLengthEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(4)) {
        Assert.AreEqual(0L, stream.Length);
        Assert.AreEqual(0L, stream.Position);

        stream.SetLength(2L);

        Assert.AreEqual(2L, stream.Length);
        Assert.AreEqual(0L, stream.Position);
      }

      using (var stream = new ChunkedMemoryStream(4)) {
        Assert.AreEqual(0L, stream.Length);
        Assert.AreEqual(0L, stream.Position);

        stream.SetLength(6L);

        Assert.AreEqual(6L, stream.Length);
        Assert.AreEqual(0L, stream.Position);
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

        Assert.AreEqual(0x00, stream.Seek(0x00, SeekOrigin.Begin));
        Assert.AreEqual(0x00, stream.ReadByte());

        Assert.AreEqual(0x18, stream.Seek(0x18, SeekOrigin.Begin));
        Assert.AreEqual(0x18, stream.ReadByte());

        Assert.AreEqual(0x0f, stream.Seek(0x0f, SeekOrigin.Begin));
        Assert.AreEqual(0x0f, stream.ReadByte());

        Assert.AreEqual(0x40, stream.Seek(0x40, SeekOrigin.Begin));

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

        Assert.AreEqual(0x00, stream.Seek(-0x20, SeekOrigin.Current));
        Assert.AreEqual(0x00, stream.ReadByte());

        Assert.AreEqual(0x18, stream.Seek(+0x17, SeekOrigin.Current));
        Assert.AreEqual(0x18, stream.ReadByte());

        Assert.AreEqual(0x0f, stream.Seek(-0x0a, SeekOrigin.Current));
        Assert.AreEqual(0x0f, stream.ReadByte());

        Assert.AreEqual(0x40, stream.Seek(+0x30, SeekOrigin.Current));

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

        Assert.AreEqual(0x00, stream.Seek(-0x20, SeekOrigin.End));
        Assert.AreEqual(0x00, stream.ReadByte());

        Assert.AreEqual(0x18, stream.Seek(-0x08, SeekOrigin.End));
        Assert.AreEqual(0x18, stream.ReadByte());

        Assert.AreEqual(0x0f, stream.Seek(-0x11, SeekOrigin.End));
        Assert.AreEqual(0x0f, stream.ReadByte());

        Assert.AreEqual(0x40, stream.Seek(+0x20, SeekOrigin.End));

        Assert.Throws<IOException>(() => stream.Seek(-0x41, SeekOrigin.End));
      }
    }

    [Test]
    public void TestSeekInternalStateNotChanged1()
    {
      using (var stream = new ChunkedMemoryStream()) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        Assert.AreEqual(0L, stream.Seek(0L, SeekOrigin.Begin));
        Assert.AreEqual(0L, stream.Length);

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        Assert.AreEqual(4L, stream.Position);
        Assert.AreEqual(4L, stream.Length);
      }
    }

    [Test]
    public void TestSeekInternalStateNotChanged2()
    {
      using (var stream = new ChunkedMemoryStream()) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        stream.Position = 0L;

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        Assert.AreEqual(4L, stream.Position);
        Assert.AreEqual(4L, stream.Length);
      }
    }

    [Test]
    public void TestReadByte()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 32; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.AreEqual(0L, stream.Seek(0L, SeekOrigin.Begin));

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(32L, stream.Length);

        for (var i = 0; i < 32; i++) {
          Assert.AreEqual((long)i, stream.Position);
          Assert.AreEqual(i, stream.ReadByte());
          Assert.AreEqual((long)i + 1, stream.Position);
        }

        Assert.AreEqual(-1, stream.ReadByte());
      }
    }

    [Test]
    public void TestRead()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        for (var i = 0; i < 32; i++) {
          stream.WriteByte((byte)i);
        }

        Assert.AreEqual(0L, stream.Seek(0, SeekOrigin.Begin));

        var buffer = new byte[16];

        Assert.AreEqual(1, stream.Read(buffer, 0, 1));
        Assert.AreEqual(new byte[] {0x00}, buffer.Skip(0).Take(1).ToArray());
        Assert.AreEqual(1L, stream.Position);

        Assert.AreEqual(3, stream.Read(buffer, 0, 3));
        Assert.AreEqual(new byte[] {0x01, 0x02, 0x03}, buffer.Skip(0).Take(3).ToArray());
        Assert.AreEqual(4L, stream.Position);

        Assert.AreEqual(4, stream.Read(buffer, 0, 4));
        Assert.AreEqual(new byte[] {0x04, 0x05, 0x06, 0x07}, buffer.Skip(0).Take(4).ToArray());
        Assert.AreEqual(8L, stream.Position);

        Assert.AreEqual(7, stream.Read(buffer, 0, 7));
        Assert.AreEqual(new byte[] {0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e}, buffer.Skip(0).Take(7).ToArray());
        Assert.AreEqual(15L, stream.Position);

        Assert.AreEqual(2, stream.Read(buffer, 0, 2));
        Assert.AreEqual(new byte[] {0x0f, 0x10}, buffer.Skip(0).Take(2).ToArray());
        Assert.AreEqual(17L, stream.Position);

        Assert.AreEqual(15, stream.Read(buffer, 0, 16));
        Assert.AreEqual(new byte[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f}, buffer.Skip(0).Take(15).ToArray());
        Assert.AreEqual(32L, stream.Position);
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

        Assert.AreEqual(0L, stream.Seek(0, SeekOrigin.Begin));

        Assert.AreEqual(0, stream.Read(buffer, 0, 0));
        Assert.AreEqual(new byte[4], buffer);

        Assert.AreEqual(0L, stream.Position);

        Assert.AreEqual(8L, stream.Seek(8, SeekOrigin.Begin));

        Assert.AreEqual(0, stream.Read(buffer, 0, 0));
        Assert.AreEqual(new byte[4], buffer);

        Assert.AreEqual(8L, stream.Position);
      }
    }

    [Test]
    public void TestReadByteFromEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        Assert.AreEqual(-1, stream.ReadByte());

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);
      }
    }

    [Test]
    public void TestReadFromEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        var buffer = new byte[4];

        Assert.AreEqual(0, stream.Read(buffer, 0, 4));

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);
      }

      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        var buffer = new byte[12];

        Assert.AreEqual(0, stream.Read(buffer, 0, 12));

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);
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
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        for (var i = 0; i < 32; i++) {
          Assert.AreEqual((long)i, stream.Position);

          stream.WriteByte((byte)i);

          Assert.AreEqual((long)(i + 1), stream.Position);
          Assert.AreEqual((long)(i + 1), stream.Length);
        }

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
        }, stream.ToArray());
      }
    }

    [Test]
    public void TestWrite()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        stream.Write(new byte[] {0x00}, 0, 1);
        Assert.AreEqual(1L, stream.Position);
        Assert.AreEqual(1L, stream.Length);

        stream.Write(new byte[] {0x01, 0x02, 0x03}, 0, 3);
        Assert.AreEqual(4L, stream.Position);
        Assert.AreEqual(4L, stream.Length);

        stream.Write(new byte[] {0x04, 0x05, 0x06, 0x07}, 0, 4);
        Assert.AreEqual(8L, stream.Position);
        Assert.AreEqual(8L, stream.Length);

        stream.Write(new byte[] {0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e}, 0, 7);
        Assert.AreEqual(15L, stream.Position);
        Assert.AreEqual(15L, stream.Length);

        stream.Write(new byte[] {0x0f, 0x10}, 0, 2);
        Assert.AreEqual(17L, stream.Position);
        Assert.AreEqual(17L, stream.Length);

        stream.Write(new byte[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f}, 0, 15);
        Assert.AreEqual(32L, stream.Position);
        Assert.AreEqual(32L, stream.Length);

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
        }, stream.ToArray());
      }
    }

    [Test]
    public void TestWriteOverwrite()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position, "position0");
        Assert.AreEqual(0L, stream.Length, "length0");

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);
        Assert.AreEqual(4L, stream.Position, "position1");
        Assert.AreEqual(4L, stream.Length, "length1");

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03,
        }, stream.ToArray());

        Assert.AreEqual(0L, stream.Seek(-4L, SeekOrigin.Current));
        Assert.AreEqual(0L, stream.Position, "position2");
        Assert.AreEqual(4L, stream.Length, "length2");

        stream.Write(new byte[] {0x04, 0x05, 0x06, 0x07, 0x08, 0x09}, 0, 6);
        Assert.AreEqual(6L, stream.Position, "position3");
        Assert.AreEqual(6L, stream.Length, "length3");

        Assert.AreEqual(new byte[] {
          0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
        }, stream.ToArray());

        Assert.AreEqual(0L, stream.Seek(0L, SeekOrigin.Begin));
        Assert.AreEqual(0L, stream.Position, "position4");
        Assert.AreEqual(6L, stream.Length, "length4");

        stream.Write(new byte[] {0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11}, 0, 8);
        Assert.AreEqual(8L, stream.Position, "position5");
        Assert.AreEqual(8L, stream.Length, "length5");

        Assert.AreEqual(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11,
        }, stream.ToArray());

        Assert.AreEqual(6L, stream.Seek(-2L, SeekOrigin.End));
        Assert.AreEqual(6L, stream.Position, "position6");
        Assert.AreEqual(8L, stream.Length, "length6");

        stream.Write(new byte[] {0x12, 0x13, 0x14, 0x15}, 0, 4);
        Assert.AreEqual(10L, stream.Position, "position7");
        Assert.AreEqual(10L, stream.Length, "length7");

        Assert.AreEqual(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x12, 0x13,
          0x14, 0x15,
        }, stream.ToArray());

        Assert.AreEqual(8L, stream.Seek(-2L, SeekOrigin.Current));
        Assert.AreEqual(8L, stream.Position, "position8");
        Assert.AreEqual(10L, stream.Length, "length8");

        stream.Write(new byte[] {0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b}, 0, 6);
        Assert.AreEqual(14L, stream.Position, "position9");
        Assert.AreEqual(14L, stream.Length, "length9");

        Assert.AreEqual(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x12, 0x13,
          0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b,
        }, stream.ToArray());

        Assert.AreEqual(8L, stream.Seek(-6L, SeekOrigin.Current));
        Assert.AreEqual(8L, stream.Position, "position10");
        Assert.AreEqual(14L, stream.Length, "length10");

        stream.Write(new byte[] {0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25}, 0, 10);
        Assert.AreEqual(18L, stream.Position, "position11");
        Assert.AreEqual(18L, stream.Length, "length11");

        Assert.AreEqual(new byte[] {
          0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x12, 0x13,
          0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23,
          0x24, 0x25,
        }, stream.ToArray());
      }
    }

    [Test]
    public void TestWriteZeroBytesToEmptyStream()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        stream.Write(new byte[0], 0, 0);

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        stream.Write(new byte[8], 0, 0);

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);
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

        Assert.IsTrue(stream.CanSeek);
        Assert.AreEqual(12L, stream.Position);
        Assert.AreEqual(12L, stream.Length);

        Assert.AreEqual(6L, stream.Seek(6L, SeekOrigin.Begin));
        Assert.AreEqual(6L, stream.Position);

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
            Assert.AreEqual(pair[index][1], stream.Seek(pair[index][0], SeekOrigin.Current), "seeked position {0}", index);
          }
          catch (IOException) {
            Assert.Fail($"IOException thrown while seeking ({index})");
          }

          Assert.AreEqual(pair[index][1], stream.Position);
          Assert.AreEqual(pair[index][1], stream.ReadByte(), "read value {0}", index);
          Assert.AreEqual(pair[index][1] + 1, stream.Position);
        }

        Assert.AreEqual(-1, stream.ReadByte());
        Assert.AreEqual(13, stream.Seek(1, SeekOrigin.Current));
        Assert.AreEqual(-1, stream.ReadByte());
        Assert.AreEqual(13, stream.Position);
      }
    }
    [Test]
    public void TestToArray()
    {
      using (var stream = new ChunkedMemoryStream(8)) {
        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        Assert.AreEqual(new byte[0], stream.ToArray());

        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        Assert.AreEqual(4L, stream.Position);
        Assert.AreEqual(4L, stream.Length);

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03
        }, stream.ToArray());

        stream.Write(new byte[] {0x04, 0x05, 0x06, 0x07}, 0, 4);

        Assert.AreEqual(8L, stream.Position);
        Assert.AreEqual(8L, stream.Length);

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
        }, stream.ToArray());

        stream.Write(new byte[] {0x08, 0x09, 0x0a, 0x0b}, 0, 4);

        Assert.AreEqual(12L, stream.Position);
        Assert.AreEqual(12L, stream.Length);

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b,
        }, stream.ToArray());

        // shorten
        stream.SetLength(6L);

        Assert.AreEqual(6L, stream.Position);
        Assert.AreEqual(6L, stream.Length);

        Assert.AreEqual(new byte[] {
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
        }, stream.ToArray());

        stream.SetLength(0L);

        Assert.AreEqual(0L, stream.Position);
        Assert.AreEqual(0L, stream.Length);

        Assert.AreEqual(new byte[0], stream.ToArray());
      }
    }
  }
}
