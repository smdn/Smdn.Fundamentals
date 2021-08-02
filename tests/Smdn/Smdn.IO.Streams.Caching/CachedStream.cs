using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using Smdn;

namespace Smdn.IO.Streams.Caching {
  [TestFixture]
  public class PersistentCachedStreamTests : CachedStreamBaseTests {
    protected override CachedStreamBase CreateCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen)
    {
      return new PersistentCachedStream(innerStream, blockSize, leaveInnerStreamOpen);
    }
  }

  [TestFixture]
  public class NonPersistentCachedStreamTests : CachedStreamBaseTests {
    protected override CachedStreamBase CreateCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen)
    {
      return new NonPersistentCachedStream(innerStream, blockSize, leaveInnerStreamOpen);
    }
  }

  public abstract class CachedStreamBaseTests {
    protected abstract CachedStreamBase CreateCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen);

    [Test]
    public void TestProperties()
    {
      using (var innerStream = new MemoryStream(8)) {
        innerStream.SetLength(8);
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
          Assert.IsTrue(stream.CanRead, "can read");
          Assert.IsFalse(stream.CanWrite, "can write");
          Assert.IsTrue(stream.CanSeek, "can seek");
          Assert.AreEqual(8L, stream.Length, "length");
          Assert.AreEqual(4L, stream.Position, "position");
          Assert.AreEqual(4, stream.BlockSize, "block size");
          Assert.IsTrue(stream.LeaveInnerStreamOpen, "leave inner stream open");
        }
      }
    }

    [Test]
    public void TestClose()
    {
      using (var innerStream = new MemoryStream(8)) {
        innerStream.SetLength(8);
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
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
    }

    [Test]
    public void TestSeek()
    {
      using (var innerStream = new MemoryStream(16)) {
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f}, 0, 16);
        innerStream.Position = 0L;

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
          Assert.AreEqual(4L, stream.Seek(4, SeekOrigin.Begin));
          Assert.AreEqual(4L, stream.Position);
          Assert.AreEqual(0x04, stream.ReadByte());

          Assert.AreEqual(7L, stream.Seek(2, SeekOrigin.Current));
          Assert.AreEqual(7L, stream.Position);
          Assert.AreEqual(0x07, stream.ReadByte());

          Assert.AreEqual(14L, stream.Seek(-2, SeekOrigin.End));
          Assert.AreEqual(14L, stream.Position);
          Assert.AreEqual(0x0e, stream.ReadByte());

          Assert.AreEqual(19L, stream.Seek(4, SeekOrigin.Current));
          Assert.AreEqual(19L, stream.Position);
          Assert.AreEqual(-1, stream.ReadByte());

          Assert.Throws<IOException>(() => stream.Seek(-17L, SeekOrigin.End));
        }
      }
    }

    [Test]
    public void TestReadByte()
    {
      using (var innerStream = new MemoryStream(16)) {
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f}, 0, 16);
        innerStream.Position = 0L;

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
          Assert.AreEqual(0L, stream.Position);
          Assert.AreEqual(0x00, stream.ReadByte());
          Assert.AreEqual(1L, stream.Position);

          Assert.AreEqual(0x01, stream.ReadByte());

          Assert.AreEqual(0x02, stream.ReadByte());

          Assert.AreEqual(0x03, stream.ReadByte());

          Assert.AreEqual(0x04, stream.ReadByte());
          Assert.AreEqual(5L, stream.Position);

          GC.Collect();

          Assert.AreEqual(0x05, stream.ReadByte());
          Assert.AreEqual(6L, stream.Position);

          stream.Position = 0L;

          Assert.AreEqual(0x00, stream.ReadByte());
          Assert.AreEqual(1L, stream.Position);

          stream.Position = stream.Length;

          Assert.AreEqual(-1, stream.ReadByte());
          Assert.AreEqual(stream.Length, stream.Position);
        }
      }
    }

    [Test]
    public void TestRead()
    {
      using (var innerStream = new MemoryStream(16)) {
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10}, 0, 17);
        innerStream.Position = 0L;

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
          var buffer = new byte[stream.Length];

          Assert.AreEqual(6, stream.Read(buffer, 0, 6));
          Assert.AreEqual(6L, stream.Position);
          Assert.AreEqual(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05}, buffer.Slice(0, 6));

          GC.Collect();

          Assert.AreEqual(6, stream.Read(buffer, 0, 6));
          Assert.AreEqual(12L, stream.Position);
          Assert.AreEqual(new byte[] {0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b}, buffer.Slice(0, 6));

          stream.Position = 0L;

          Assert.AreEqual(6, stream.Read(buffer, 0, 6));
          Assert.AreEqual(6L, stream.Position);
          Assert.AreEqual(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05}, buffer.Slice(0, 6));

          stream.Position = 12L;

          Assert.AreEqual(5, stream.Read(buffer, 0, 6));
          Assert.AreEqual(17L, stream.Position);
          Assert.AreEqual(new byte[] {0x0c, 0x0d, 0x0e, 0x0f, 0x10}, buffer.Slice(0, 5));

          Assert.AreEqual(0, stream.Read(buffer, 0, 6));
          Assert.AreEqual(17L, stream.Position);

          stream.Position = 16L;

          Assert.AreEqual(1, stream.Read(buffer, 0, 6));
          Assert.AreEqual(17L, stream.Position);

          stream.Position = stream.Length;

          Assert.AreEqual(0, stream.Read(buffer, 0, 6));
          Assert.AreEqual(17L, stream.Position);
        }
      }
    }


    [Test]
    public void TestSeekAndReadRandom()
    {
      using (var innerStream = new MemoryStream(16)) {
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b}, 0, 12);
        innerStream.Position = 0L;

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
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

            if (index % 3 == 0)
              GC.Collect();
          }

          Assert.AreEqual(-1, stream.ReadByte());
          Assert.AreEqual(13, stream.Seek(1, SeekOrigin.Current));
          Assert.AreEqual(-1, stream.ReadByte());
          Assert.AreEqual(13, stream.Position);
        }
      }
    }

    private MemoryStream CreateStream()
    {
      var stream = new MemoryStream(8);

      stream.SetLength(8);
      stream.Position = 0L;

      return stream;
    }

    [Test]
    public void TestWriteByte()
    {
      using (var stream = CreateCachedStream(CreateStream(), 4, false)) {
        Assert.Throws<NotSupportedException>(() => stream.WriteByte(0x00));
      }
    }

    [Test]
    public void TestWrite()
    {
      using (var stream = CreateCachedStream(CreateStream(), 4, false)) {
        Assert.Throws<NotSupportedException>(() => stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4));
      }
    }

    [Test]
    public void TestSetLength()
    {
      using (var stream = CreateCachedStream(CreateStream(), 4, false)) {
        Assert.Throws<NotSupportedException>(() => stream.SetLength(32L));
      }
    }

    [Test]
    public void TestFlush()
    {
      using (var stream = CreateCachedStream(CreateStream(), 4, false)) {
        var pos = stream.Position;
        var len = stream.Length;

        Assert.DoesNotThrow(() => stream.Flush());

        Assert.AreEqual(pos, stream.Position);
        Assert.AreEqual(len, stream.Length);
      }
    }
  }
}
