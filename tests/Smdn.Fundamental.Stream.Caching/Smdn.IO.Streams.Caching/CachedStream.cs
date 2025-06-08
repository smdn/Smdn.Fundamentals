// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
          Assert.That(stream.CanRead, Is.True, "can read");
          Assert.That(stream.CanWrite, Is.False, "can write");
          Assert.That(stream.CanSeek, Is.True, "can seek");
          Assert.That(stream.Length, Is.EqualTo(8L), "length");
          Assert.That(stream.Position, Is.EqualTo(4L), "position");
          Assert.That(stream.BlockSize, Is.EqualTo(4), "block size");
          Assert.That(stream.LeaveInnerStreamOpen, Is.True, "leave inner stream open");
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

          Assert.That(stream.CanRead, Is.False, "CanRead");
          Assert.That(stream.CanWrite, Is.False, "CanWrite");
          Assert.That(stream.CanSeek, Is.False, "CanSeek");
          Assert.That(stream.CanTimeout, Is.False, "CanTimeout");

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
          Assert.That(stream.Seek(4, SeekOrigin.Begin), Is.EqualTo(4L));
          Assert.That(stream.Position, Is.EqualTo(4L));
          Assert.That(stream.ReadByte(), Is.EqualTo(0x04));

          Assert.That(stream.Seek(2, SeekOrigin.Current), Is.EqualTo(7L));
          Assert.That(stream.Position, Is.EqualTo(7L));
          Assert.That(stream.ReadByte(), Is.EqualTo(0x07));

          Assert.That(stream.Seek(-2, SeekOrigin.End), Is.EqualTo(14L));
          Assert.That(stream.Position, Is.EqualTo(14L));
          Assert.That(stream.ReadByte(), Is.EqualTo(0x0e));

          Assert.That(stream.Seek(4, SeekOrigin.Current), Is.EqualTo(19L));
          Assert.That(stream.Position, Is.EqualTo(19L));
          Assert.That(stream.ReadByte(), Is.EqualTo(-1));

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
          Assert.That(stream.Position, Is.EqualTo(0L));
          Assert.That(stream.ReadByte(), Is.EqualTo(0x00));
          Assert.That(stream.Position, Is.EqualTo(1L));

          Assert.That(stream.ReadByte(), Is.EqualTo(0x01));

          Assert.That(stream.ReadByte(), Is.EqualTo(0x02));

          Assert.That(stream.ReadByte(), Is.EqualTo(0x03));

          Assert.That(stream.ReadByte(), Is.EqualTo(0x04));
          Assert.That(stream.Position, Is.EqualTo(5L));

          GC.Collect();

          Assert.That(stream.ReadByte(), Is.EqualTo(0x05));
          Assert.That(stream.Position, Is.EqualTo(6L));

          stream.Position = 0L;

          Assert.That(stream.ReadByte(), Is.EqualTo(0x00));
          Assert.That(stream.Position, Is.EqualTo(1L));

          stream.Position = stream.Length;

          Assert.That(stream.ReadByte(), Is.EqualTo(-1));
          Assert.That(stream.Position, Is.EqualTo(stream.Length));
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

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(6));
          Assert.That(stream.Position, Is.EqualTo(6L));
          Assert.That(buffer.Skip(0).Take(6).ToArray(), Is.EqualTo(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05}));

          GC.Collect();

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(6));
          Assert.That(stream.Position, Is.EqualTo(12L));
          Assert.That(buffer.Skip(0).Take(6).ToArray(), Is.EqualTo(new byte[] {0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b}));

          stream.Position = 0L;

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(6));
          Assert.That(stream.Position, Is.EqualTo(6L));
          Assert.That(buffer.Skip(0).Take(6).ToArray(), Is.EqualTo(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05}));

          stream.Position = 12L;

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(5));
          Assert.That(stream.Position, Is.EqualTo(17L));
          Assert.That(buffer.Skip(0).Take(5).ToArray(), Is.EqualTo(new byte[] {0x0c, 0x0d, 0x0e, 0x0f, 0x10}));

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(0));
          Assert.That(stream.Position, Is.EqualTo(17L));

          stream.Position = 16L;

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(1));
          Assert.That(stream.Position, Is.EqualTo(17L));

          stream.Position = stream.Length;

          Assert.That(stream.Read(buffer, 0, 6), Is.EqualTo(0));
          Assert.That(stream.Position, Is.EqualTo(17L));
        }
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
      using var stream = CreateCachedStream(new MemoryStream(8), 4, true);

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
      using var stream = CreateCachedStream(new MemoryStream(8), 4, true);

      Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count));
    }
#endif

    [Test]
    public void TestSeekAndReadRandom()
    {
      using (var innerStream = new MemoryStream(16)) {
        innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b}, 0, 12);
        innerStream.Position = 0L;

        using (var stream = CreateCachedStream(innerStream, 4, true)) {
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

            if (index % 3 == 0)
              GC.Collect();
          }

          Assert.That(stream.ReadByte(), Is.EqualTo(-1));
          Assert.That(stream.Seek(1, SeekOrigin.Current), Is.EqualTo(13));
          Assert.That(stream.ReadByte(), Is.EqualTo(-1));
          Assert.That(stream.Position, Is.EqualTo(13));
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

        Assert.That(stream.Position, Is.EqualTo(pos));
        Assert.That(stream.Length, Is.EqualTo(len));
      }
    }
  }
}
