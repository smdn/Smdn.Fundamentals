using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using Smdn;

namespace Smdn.IO {
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

    private class TestChunk : ChunkedMemoryStream.Chunk {
      public TestChunk(int size, List<TestChunk> list)
      {
        this.list = list;
        this.index = list.Count;
        base.Data = new byte[size];
        list.Add(this);

        Console.WriteLine("allocated [{0}]", index);
      }

      public override void Dispose()
      {
        list.Remove(this);

        Console.WriteLine("disposed [{0}]", index);
      }

      private int index;
      private List<TestChunk> list;
    }

    [Test]
    public void TestAllocateDisposeChunk()
    {
      var allocated = new List<TestChunk>();

      using (var stream = new ChunkedMemoryStream(4, delegate(int size) {
        Assert.AreEqual(4, size);
        return new TestChunk(size, allocated);
      })) {
        Assert.AreEqual(1, allocated.Count, "first chunk");

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

        Console.WriteLine("set length 12");
        stream.SetLength(12L);
        Assert.AreEqual(12L, stream.Length);
        Assert.AreEqual(4, allocated.Count, "extended by SetLength 1");

        Console.WriteLine("set length 8");
        stream.SetLength(8L);
        Assert.AreEqual(8L, stream.Length);
        Assert.AreEqual(3, allocated.Count, "shorten by SetLength 1");

        Console.WriteLine("set length 7");
        stream.SetLength(7L);
        Assert.AreEqual(7L, stream.Length);
        Assert.AreEqual(2, allocated.Count, "shorten by SetLength 2");

        Console.WriteLine("set length 3");
        stream.SetLength(3L);
        Assert.AreEqual(3L, stream.Length);
        Assert.AreEqual(1, allocated.Count, "shorten by SetLength 3");

        Console.WriteLine("set length 0");
        stream.SetLength(0L);
        Assert.AreEqual(0L, stream.Length);
        Assert.AreEqual(1, allocated.Count, "shorten by SetLength 4");

        Console.WriteLine("set length 12");
        stream.SetLength(12L);
        Assert.AreEqual(12L, stream.Length);
        Assert.AreEqual(4, allocated.Count, "extended by SetLength 2");

        Console.WriteLine("set length 0");
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
        var writer = new BigEndianBinaryWriter(stream);
        var reader = new BigEndianBinaryReader(stream);

        writer.Write(0x00010203);
        writer.Flush();

        stream.Position = 2L;

        writer.Write(0x04050607);
        writer.Write(0x08090a0b);

        stream.Seek(-2L, SeekOrigin.Current);

        Assert.AreEqual(0x0a0b, reader.ReadInt16());

        stream.Seek(4L, SeekOrigin.Begin);

        writer.Write(0x0c0d0e0f);

        stream.Seek(-10L, SeekOrigin.End);

        Assert.AreEqual(0x00010405, reader.ReadInt32());

        Assert.AreEqual(4L, stream.Position);
        Assert.AreEqual(10L, stream.Length);

        stream.Position = 0L;

        results.Add(reader.ReadToEnd());
      }

      Assert.AreEqual(results[0], results[1]);
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

        try {
          stream.Position = -1L;
          Assert.Fail("ArgumentOutOfRangeException not thrown");
        }
        catch (ArgumentOutOfRangeException) {
        }

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

    [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestSetLengthNegative()
    {
      using (var stream = new ChunkedMemoryStream(4)) {
        stream.SetLength(-1);
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

        try {
          stream.Seek(-1, SeekOrigin.Begin);
          Assert.Fail("IOException not thrown");
        }
        catch (IOException) {
        }
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

        try {
          stream.Seek(-0x41, SeekOrigin.Current);
          Assert.Fail("IOException not thrown");
        }
        catch (IOException) {
        }
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

        try {
          stream.Seek(-0x41, SeekOrigin.End);
          Assert.Fail("IOException not thrown");
        }
        catch (IOException) {
        }
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
        Assert.AreEqual(new byte[] {0x00}, buffer.Slice(0, 1));
        Assert.AreEqual(1L, stream.Position);

        Assert.AreEqual(3, stream.Read(buffer, 0, 3));
        Assert.AreEqual(new byte[] {0x01, 0x02, 0x03}, buffer.Slice(0, 3));
        Assert.AreEqual(4L, stream.Position);

        Assert.AreEqual(4, stream.Read(buffer, 0, 4));
        Assert.AreEqual(new byte[] {0x04, 0x05, 0x06, 0x07}, buffer.Slice(0, 4));
        Assert.AreEqual(8L, stream.Position);

        Assert.AreEqual(7, stream.Read(buffer, 0, 7));
        Assert.AreEqual(new byte[] {0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e}, buffer.Slice(0, 7));
        Assert.AreEqual(15L, stream.Position);

        Assert.AreEqual(2, stream.Read(buffer, 0, 2));
        Assert.AreEqual(new byte[] {0x0f, 0x10}, buffer.Slice(0, 2));
        Assert.AreEqual(17L, stream.Position);

        Assert.AreEqual(15, stream.Read(buffer, 0, 16));
        Assert.AreEqual(new byte[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f}, buffer.Slice(0, 15));
        Assert.AreEqual(32L, stream.Position);
      }
    }

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
            Assert.Fail("IOException thrown while seeking ({0})", index);
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
  }
}