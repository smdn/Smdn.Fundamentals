using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.IO.Streams {
  [TestFixture]
  public class ExtendStreamTests {
    [Test]
    public void TestConstruct()
    {
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, (byte[])null, (byte[])null)).Length, "construct null/null");
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, new byte[] { }, null)).Length, "construct byte[]/null");
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, null, new byte[] { })).Length, "construct null/byte[]");
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, new byte[] { }, new byte[] { })).Length, "construct byte[]/byte[]");
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, Stream.Null, null)).Length, "construct Stream/null");
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, null, Stream.Null)).Length, "construct null/Stream");
      Assert.AreEqual(0L, (new ExtendStream(Stream.Null, Stream.Null, Stream.Null)).Length, "construct Stream/Stream");
    }

    [Test]
    public void TestConstruct_StreamNull()
    {
      Assert.Throws<ArgumentNullException>(() => new ExtendStream(null, Stream.Null, Stream.Null));
      Assert.Throws<ArgumentNullException>(() => new ExtendStream(null, new byte[0], new byte[0]));
    }

    [Test]
    public void TestLeaveStreamOpen()
    {
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03});

      using (var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: true)) {
        Assert.IsTrue(stream.LeaveInnerStreamOpen);
      }

      Assert.DoesNotThrow(() => baseStream.ReadByte());
    }

    [Test]
    public void TestLeaveStreamNotOpen()
    {
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03});

      using (var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: false)) {
        Assert.IsFalse(stream.LeaveInnerStreamOpen);
      }

      Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte());
    }

    [Test]
    public void TestClose_LeaveStreamOpen()
    {
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03});

      using (var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: true)) {
        Assert.IsTrue(stream.LeaveInnerStreamOpen);
        TestClose(stream);
      }
    }

    [Test]
    public void TestClose_LeaveStreamNotOpen()
    {
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03});

      using (var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: false)) {
        Assert.IsFalse(stream.LeaveInnerStreamOpen);
        TestClose(stream);
      }
    }

    private void TestClose(ExtendStream stream)
    {
      stream.Dispose();

      Assert.IsFalse(stream.CanRead, nameof(stream.CanRead));
      Assert.IsFalse(stream.CanWrite, nameof(stream.CanWrite));
      Assert.IsFalse(stream.CanSeek, nameof(stream.CanSeek));
      Assert.IsFalse(stream.CanTimeout, nameof(stream.CanTimeout));

      Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.InnerStream), nameof(stream.InnerStream));
      Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.LeaveInnerStreamOpen), nameof(stream.LeaveInnerStreamOpen));
      Assert.Throws<ObjectDisposedException>(() => stream.ReadByte(), nameof(stream.ReadByte));
      Assert.Throws<ObjectDisposedException>(() => stream.Read(Array.Empty<byte>(), 0, 0), nameof(stream.Read));
      Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(Array.Empty<byte>(), 0, 0), nameof(stream.ReadAsync));
      Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00), nameof(stream.WriteByte));
      Assert.Throws<ObjectDisposedException>(() => stream.Write(Array.Empty<byte>(), 0, 0), nameof(stream.Write));
      Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(Array.Empty<byte>(), 0, 0), nameof(stream.WriteAsync));
      Assert.Throws<ObjectDisposedException>(() => stream.Flush(), nameof(stream.Flush));
      Assert.Throws<ObjectDisposedException>(() => stream.FlushAsync(), nameof(stream.FlushAsync));
      Assert.Throws<ObjectDisposedException>(() => stream.Seek(0, SeekOrigin.Begin), nameof(stream.Seek));
      Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L), nameof(stream.SetLength));
      Assert.Throws<ObjectDisposedException>(() => { Assert.AreEqual(-1, stream.Length); }, nameof(stream.Length));
      Assert.Throws<ObjectDisposedException>(() => { Assert.AreEqual(-1, stream.Position); }, nameof(stream.Length));

      Assert.DoesNotThrow(() => stream.Close(), nameof(stream.Close));
    }

    [Test]
    public void TestLength()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 })) {
        Assert.AreEqual(2L, (new ExtendStream(innerStream, new byte[] { }, new byte[] { }, true)).Length);
        Assert.AreEqual(4L, (new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { }, true)).Length);
        Assert.AreEqual(4L, (new ExtendStream(innerStream, new byte[] { }, new byte[] { 0x00, 0x00 }, true)).Length);
        Assert.AreEqual(6L, (new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, true)).Length);
      }
    }

    [Test]
    public void TestWritingOperation()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 })) {
          Assert.IsFalse(extended.CanWrite);

          var len = extended.Length;
          var pos = extended.Position;

          Assert.Throws<NotSupportedException>(() => extended.SetLength(10L));
          Assert.Throws<NotSupportedException>(() => extended.WriteByte(0xff));
          Assert.Throws<NotSupportedException>(() => extended.Write(new byte[] { 0xff, 0xff }, 0, 2));
          Assert.Throws<NotSupportedException>(() => extended.WriteAsync(new byte[] { 0xff, 0xff }, 0, 2));

          Assert.AreEqual(len, extended.Length);
          Assert.AreEqual(pos, extended.Position);
        }
      }
    }

    [Test]
    public void TestFlush()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 })) {
          Assert.IsFalse(extended.CanWrite);

          var len = extended.Length;
          var pos = extended.Position;

          Assert.Throws<NotSupportedException>(() => extended.Flush());
          Assert.Throws<NotSupportedException>(() => extended.FlushAsync());

          Assert.AreEqual(len, extended.Length);
          Assert.AreEqual(pos, extended.Position);
        }
      }
    }

    [Test] public void TestRead_ArgumentOutOfRange() => TestRead_ArgumentException(runAsync: false);
    [Test] public void TestReadAsync_ArgumentOutOfRange() => TestRead_ArgumentException(runAsync: true);

    private void TestRead_ArgumentException(bool runAsync)
    {
      using (var stream = new ExtendStream(Stream.Null, Stream.Null, Stream.Null)) {
        var buffer = new byte[2];

        if (runAsync)
          Assert.Throws<ArgumentNullException>(() => stream.ReadAsync(buffer: null, 0, 2));
        else
          Assert.Throws<ArgumentNullException>(() => stream.Read(buffer: null, 0, 2));

        if (runAsync)
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, -1, 0));
        else
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, -1, 0));

        if (runAsync)
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, 0, -1));
        else
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, 0, -1));

        if (runAsync)
          Assert.Throws<ArgumentException>(() => stream.ReadAsync(buffer, 1, 2));
        else
          Assert.Throws<ArgumentException>(() => stream.Read(buffer, 1, 2));

        if (runAsync)
          Assert.Throws<ArgumentException>(() => stream.ReadAsync(buffer, 2, 1));
        else
          Assert.Throws<ArgumentException>(() => stream.Read(buffer, 2, 1));
      }
    }

    [Test] public Task TestRead_ExtendedStream() => TestRead_ExtendedStream(runAsync: false);
    [Test] public Task TestReadAsync_ExtendedStream() => TestRead_ExtendedStream(runAsync: true);

    private async Task TestRead_ExtendedStream(bool runAsync)
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 13; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
          using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b })) {
            var ret = runAsync
                ? await extended.ReadAsync(buffer, 0, len)
                : extended.Read(buffer, 0, len);

            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test] public Task TestRead_PrependedStream() => TestRead_PrependedStream(runAsync: false);
    [Test] public Task TestReadAsync_PrependedStream() => TestRead_PrependedStream(runAsync: true);

    private async Task TestRead_PrependedStream(bool runAsync)
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 9; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
          using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, null)) {
            var ret = runAsync
                ? await extended.ReadAsync(buffer, 0, len)
                : extended.Read(buffer, 0, len);

            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test] public Task TestRead_AppendedStream() => TestRead_AppendedStream(runAsync: false);
    [Test] public Task TestReadAsync_AppendedStream() => TestRead_AppendedStream(runAsync: true);

    private async Task TestRead_AppendedStream(bool runAsync)
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 9; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 })) {
          using (var extended = new ExtendStream(innerStream, null, new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
            var ret = runAsync
              ? await extended.ReadAsync(buffer, 0, len)
              : extended.Read(buffer, 0, len);

            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test] public Task TestRead_NonExtendedStream() => TestRead_NonExtendedStream(runAsync: false);
    [Test] public Task TestReadAsync_NonExtendedStream() => TestRead_NonExtendedStream(runAsync: true);

    private async Task TestRead_NonExtendedStream(bool runAsync)
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 5; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 })) {
          using (var extended = new ExtendStream(innerStream, (Stream)null, (Stream)null)) {
            var ret = runAsync
              ? await extended.ReadAsync(buffer, 0, len)
              : extended.Read(buffer, 0, len);

            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, ret, "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test]
    public void TestReadByte()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b })) {
          Assert.AreEqual(12L, extended.Length);
          Assert.AreEqual(0L, extended.Position);
          Assert.IsTrue(extended.CanRead);

          for (var expected = 0L; expected < 12L; expected++) {
            Assert.AreEqual(expected, extended.ReadByte(), "offset: {0}", expected);
            Assert.AreEqual(expected + 1, extended.Position, "position {0}", expected);
          }

          Assert.AreEqual(-1, extended.ReadByte(), "end of stream");
        }
      }
    }

    [Test]
    public void TestSetPosition()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b })) {
          Assert.Throws<ArgumentOutOfRangeException>(() => extended.Position = -1L);

          foreach (var expected in new long[] {
            0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
          }) {
            extended.Position = expected;

            Assert.AreEqual(expected, extended.Position);
            Assert.AreEqual(expected, extended.ReadByte());
            Assert.AreEqual(expected + 1L, extended.Position);
          }

          Assert.DoesNotThrow(() => extended.Position = 13L);
          Assert.AreEqual(13L, extended.Position);
        }
      }
    }

    [Test]
    public void TestSeek_SeekOriginCurrent()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b })) {
          Assert.IsTrue(extended.CanSeek);

          extended.Position = 6L;

          Assert.Throws<IOException>(() => extended.Seek(-7, SeekOrigin.Current));

          Assert.AreEqual(6L, extended.Position);

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
            Assert.AreEqual(pair[index][1], extended.Seek(pair[index][0], SeekOrigin.Current), "seeked position {0}", index);

            Assert.AreEqual(pair[index][1], extended.ReadByte(), "read value {0}", index);
          }

          Assert.AreEqual(-1, extended.ReadByte());
          Assert.AreEqual(13, extended.Seek(1, SeekOrigin.Current));
          Assert.AreEqual(-1, extended.ReadByte());
        }
      }
    }

    [Test]
    public void TestSeek_SeekOriginBegin()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b })) {
          Assert.IsTrue(extended.CanSeek);

          foreach (var offset in new long[] {
            0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
          }) {
            Assert.AreEqual(offset, extended.Seek(offset, SeekOrigin.Begin), "seeking to {0}", offset);
            Assert.AreEqual(offset, extended.Position);
            Assert.AreEqual(offset, extended.ReadByte());
          }

          extended.Position = 6L;

          Assert.Throws<IOException>(() => extended.Seek(-1L, SeekOrigin.Begin));

          Assert.AreEqual(6L, extended.Position);

          Assert.AreEqual(12L, extended.Seek(12L, SeekOrigin.Begin));
          Assert.AreEqual(-1, extended.ReadByte());
        }
      }
    }

    [Test]
    public void TestSeek_SeekOriginEnd()
    {
      using (var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 })) {
        using (var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b })) {
          Assert.IsTrue(extended.CanSeek);

          foreach (var offset in new long[] {
            0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
          }) {
            Assert.AreEqual(offset, extended.Seek(-extended.Length + offset, SeekOrigin.End), "seeking to {0}", offset);
            Assert.AreEqual(offset, extended.Position);
            Assert.AreEqual(offset, extended.ReadByte());
          }

          extended.Position = 6L;

          Assert.Throws<IOException>(() => extended.Seek(-13L, SeekOrigin.End));

          Assert.AreEqual(6L, extended.Position);

          Assert.AreEqual(12L, extended.Seek(0L, SeekOrigin.End));
          Assert.AreEqual(-1, extended.ReadByte());
        }
      }
    }
  }
}
