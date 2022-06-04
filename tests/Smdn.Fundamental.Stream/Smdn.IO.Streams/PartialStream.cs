// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

#if SYSTEM_ARRAY_EMPTY
using ArrayEmptyShim = System.Array; // System.Array.Empty
#else
using ArrayEmptyShim = Smdn.ArrayShim; // Smdn.ArrayShim.Empty
#endif

namespace Smdn.IO.Streams {
  [TestFixture]
  public class PartialStreamTests {
    [Test]
    public void TestLeaveInnerStreamOpen()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});

      using (var stream = new PartialStream(inner, 2, 4, true)) {
        Assert.AreEqual(2, inner.Position);
        Assert.IsTrue(stream.LeaveInnerStreamOpen);
      }

      Assert.DoesNotThrow(() => inner.ReadByte());
    }

    [Test]
    public void TestLeaveInnerStreamClose()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});

      using (var stream = new PartialStream(inner, 2, 4, false)) {
        Assert.AreEqual(2, inner.Position);
        Assert.IsFalse(stream.LeaveInnerStreamOpen);
      }

      Assert.Throws<ObjectDisposedException>(() => inner.ReadByte());
    }

    [Test]
    public void TestConstructReadOnlyWithWritableStream()
    {
      var readOnly = true;
      var inner = new MemoryStream(8);
      var stream = new PartialStream(inner, 4, 4, readOnly /*readonly*/, true);

      Assert.AreEqual(4, inner.Position);

      if (!inner.CanWrite)
        Assert.Fail("inner stream is not writable");

      Assert.IsFalse(stream.CanWrite);

      Assert.Throws<NotSupportedException>(() => stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4));
      Assert.Throws<NotSupportedException>(() => stream.WriteAsync(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4));
      Assert.Throws<NotSupportedException>(() => stream.WriteByte(0x00));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.ThrowsAsync<NotSupportedException>(async () => await stream.WriteAsync(ReadOnlyMemory<byte>.Empty));
#endif

      var len = stream.Length;
      var pos = stream.Position;

      Assert.DoesNotThrow(() => stream.Flush());

      Assert.AreEqual(len, stream.Length);
      Assert.AreEqual(pos, stream.Position);
    }

    [Test]
    public void TestConstructNotSeekToBegin()
    {
      var inner = new MemoryStream(8);

      inner.Position = 0;

      var stream = new PartialStream(inner, 4, 4, false, true, false);

      Assert.AreEqual(0, inner.Position);
      Assert.AreEqual(-4, stream.Position);
    }

    [Test]
    public void TestConstructNested()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var nestOuter = new PartialStream(inner, 1, 6);
      var nestInner = new PartialStream(nestOuter, 1, 4);

      Assert.AreEqual(2, inner.Position);
      Assert.AreEqual(1, nestOuter.Position);
      Assert.AreEqual(0, nestInner.Position);

      Assert.IsInstanceOf<PartialStream>(nestInner.InnerStream);

      var buffer = new byte[inner.Length];

      nestInner.Position = 0;
      Assert.AreEqual(4, nestInner.Read(buffer, 0, buffer.Length));
      Assert.AreEqual(new byte[] {0x02, 0x03, 0x04, 0x05, 0x00, 0x00, 0x00, 0x00}, buffer);

      nestOuter.Position = 0;
      Assert.AreEqual(6, nestOuter.Read(buffer, 0, buffer.Length));
      Assert.AreEqual(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x00, 0x00}, buffer);
    }

    [Test]
    public void TestConstructNonNested()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var nestOuter = new PartialStream(inner, 1, 6);
      var nestInner = PartialStream.CreateNonNested(nestOuter, 1, 4);

      Assert.AreEqual(2, inner.Position);
      Assert.AreEqual(1, nestOuter.Position);
      Assert.AreEqual(0, nestInner.Position);

      Assert.IsNotInstanceOf<PartialStream>(nestInner.InnerStream);

      var buffer = new byte[inner.Length];

      nestInner.Position = 0;
      Assert.AreEqual(4, nestInner.Read(buffer, 0, buffer.Length));
      Assert.AreEqual(new byte[] {0x02, 0x03, 0x04, 0x05, 0x00, 0x00, 0x00, 0x00}, buffer);

      nestOuter.Position = 0;
      Assert.AreEqual(6, nestOuter.Read(buffer, 0, buffer.Length));
      Assert.AreEqual(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x00, 0x00}, buffer);
    }

    [Test]
    public void TestCloseLeaveInnerStreamOpen()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});

      using (var stream = new PartialStream(inner, 2, 4, true)) {
        TestClose(stream);
      }
    }

    [Test]
    public void TestCloseLeaveInnerStreamClose()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});

      using (var stream = new PartialStream(inner, 2, 4, false)) {
        TestClose(stream);
      }
    }

    private void TestClose(PartialStream stream)
    {
      stream.Dispose();

      Assert.IsFalse(stream.CanRead, "CanRead");
      Assert.IsFalse(stream.CanWrite, "CanWrite");
      Assert.IsFalse(stream.CanSeek, "CanSeek");
      Assert.IsFalse(stream.CanTimeout, "CanTimeout");

      Assert.Throws<ObjectDisposedException>(() =>stream.ReadByte());
      Assert.Throws<ObjectDisposedException>(() => stream.Read(ArrayEmptyShim.Empty<byte>(), 0, 0));
      Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(ArrayEmptyShim.Empty<byte>(), 0, 0));
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.ReadAsync(Memory<byte>.Empty));
#endif
      Assert.Throws<ObjectDisposedException>(() =>stream.WriteByte(0x00));
      Assert.Throws<ObjectDisposedException>(() => stream.Write(ArrayEmptyShim.Empty<byte>(), 0, 0));
      Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(ArrayEmptyShim.Empty<byte>(), 0, 0));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.WriteAsync(ReadOnlyMemory<byte>.Empty));
#endif

      stream.Dispose();
    }

    enum ReadMethod {
      Read,
      ReadAsync,
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      ReadAsyncToMemory
#endif
    }

    [Test] public Task TestRead_LengthSpecified() => TestRead_LengthSpecified(ReadMethod.Read);
    [Test] public Task TestReadAsync_LengthSpecified() => TestRead_LengthSpecified(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_LengthSpecified() => TestRead_LengthSpecified(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_LengthSpecified(ReadMethod readMethod)
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, 2, 4);

      Assert.AreEqual(8, stream.InnerStream.Length);
      Assert.AreEqual(2, stream.InnerStream.Position);

      Assert.AreEqual(4, stream.Length);
      Assert.AreEqual(0, stream.Position);

      var buffer = new byte[3];

      Assert.AreEqual(
        2,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 2),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 2)),
#endif
          _ => stream.Read(buffer, 0, 2),
        }
      );
      Assert.AreEqual(new byte[] {0x02, 0x03, 0x00}, buffer);

      Assert.AreEqual(4, stream.InnerStream.Position);
      Assert.AreEqual(2, stream.Position);

      Assert.AreEqual(0x04, stream.ReadByte());

      Assert.AreEqual(
        1,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 2),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 2)),
#endif
          _ => stream.Read(buffer, 0, 2),
        }
      );
      Assert.AreEqual(new byte[] {0x05, 0x03, 0x00}, buffer);

      Assert.AreEqual(6, stream.InnerStream.Position);
      Assert.AreEqual(4, stream.Position);

      Assert.AreEqual(
        0,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 3),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 3)),
#endif
          _ => stream.Read(buffer, 0, 3),
        }
      );
      Assert.AreEqual(-1, stream.ReadByte());
    }

    [Test] public Task TestRead_AfterEndOfInnerStream() => TestRead_AfterEndOfInnerStream(ReadMethod.Read);
    [Test] public Task TestReadAsync_AfterEndOfInnerStream() => TestRead_AfterEndOfInnerStream(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_AfterEndOfInnerStream() => TestRead_AfterEndOfInnerStream(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_AfterEndOfInnerStream(ReadMethod readMethod)
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, inner.Length, 8);

      Assert.AreEqual(8, stream.Length);
      Assert.AreEqual(0, stream.Position);

      var buffer = new byte[2];

      Assert.AreEqual(
        0,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 2),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 2)),
#endif
          _ => stream.Read(buffer, 0, 2),
        }
      );
      Assert.AreEqual(0, stream.Position);
    }

    [Test] public Task TestRead_LengthNotSpecified() => TestRead_LengthNotSpecified(ReadMethod.Read);
    [Test] public Task TestReadAsync_LengthNotSpecified() => TestRead_LengthNotSpecified(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_LengthNotSpecified() => TestRead_LengthNotSpecified(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_LengthNotSpecified(ReadMethod readMethod)
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, 2);

      Assert.AreEqual(8, stream.InnerStream.Length);
      Assert.AreEqual(2, stream.InnerStream.Position);

      Assert.AreEqual(6, stream.Length);
      Assert.AreEqual(0, stream.Position);

      var buffer = new byte[3];

      Assert.AreEqual(
        3,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 3),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 3)),
#endif
          _ => stream.Read(buffer, 0, 3),
        }
      );
      Assert.AreEqual(new byte[] {0x02, 0x03, 0x04}, buffer);

      Assert.AreEqual(5, stream.InnerStream.Position);
      Assert.AreEqual(3, stream.Position);

      Assert.AreEqual(0x05, stream.ReadByte());

      Assert.AreEqual(6, stream.InnerStream.Position);
      Assert.AreEqual(4, stream.Position);

      Assert.AreEqual(
        2,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 3),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 3)),
#endif
          _ => stream.Read(buffer, 0, 3),
        }
      );
      Assert.AreEqual(new byte[] {0x06, 0x07, 0x04}, buffer);

      Assert.AreEqual(8, stream.InnerStream.Position);
      Assert.AreEqual(6, stream.Position);

      Assert.AreEqual(
        0,
        readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, 3),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, 3)),
#endif
          _ => stream.Read(buffer, 0, 3),
        }
      );
      Assert.AreEqual(-1, stream.ReadByte());
    }

    [Test]
    public void TestReadByte_LengthSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, 2, 4);

      foreach (var expected in new int[] {
        0x02, 0x03, 0x04, 0x05, -1,
      }) {
        Assert.AreEqual(expected, stream.ReadByte());
      }

      Assert.AreEqual(4, stream.Position);
      Assert.AreEqual(6, stream.InnerStream.Position);

      Assert.AreEqual(-1, stream.ReadByte());

      Assert.AreEqual(4, stream.Position);
      Assert.AreEqual(6, stream.InnerStream.Position);
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
      using var stream = new PartialStream(new MemoryStream(new byte[8]), 2, 4);

      Assert.Throws(expectedExceptionType, () => stream.Read(buffer, offset, count));
    }

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
      using var stream = new PartialStream(new MemoryStream(new byte[8]), 2, 4);

      Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count));
    }

    [Test]
    public void TestReadByte_LengthNotSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, 2);

      foreach (var expected in new int[] {
        0x02, 0x03, 0x04, 0x05, 0x06, 0x07, -1,
      }) {
        Assert.AreEqual(expected, stream.ReadByte());
      }

      Assert.AreEqual(6, stream.Position);
      Assert.AreEqual(8, stream.InnerStream.Position);

      Assert.AreEqual(-1, stream.ReadByte());

      Assert.AreEqual(6, stream.Position);
      Assert.AreEqual(8, stream.InnerStream.Position);
    }

    enum WriteMethod {
      Write,
      WriteAsync,
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      WriteAsyncFromReadOnlyMemory,
#endif
    }

    [Test] public Task TestWrite_LengthSpecified() => TestWrite_LengthSpecified(WriteMethod.Write);
    [Test] public Task TestWriteAsync_LengthSpecified() => TestWrite_LengthSpecified(WriteMethod.WriteAsync);
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    [Test] public Task TestWriteAsync_FromReadOnlyMemory_LengthSpecified() => TestWrite_LengthSpecified(WriteMethod.WriteAsyncFromReadOnlyMemory);
#endif

    private async Task TestWrite_LengthSpecified(WriteMethod writeMethod)
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});

      using (var stream = new PartialStream(inner, 2, 4, false)) {
        Assert.AreEqual(8, stream.InnerStream.Length);
        Assert.AreEqual(2, stream.InnerStream.Position);

        Assert.AreEqual(4, stream.Length);
        Assert.AreEqual(0, stream.Position);

        switch (writeMethod) {
          case WriteMethod.WriteAsync: await stream.WriteAsync(new byte[] {0x02, 0x03}, 0, 2); break;
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
          case WriteMethod.WriteAsyncFromReadOnlyMemory: await stream.WriteAsync(new ReadOnlyMemory<byte>(new byte[] {0x02, 0x03})); break;
#endif
          default: stream.Write(new byte[] {0x02, 0x03}, 0, 2); break;
        };

        Assert.AreEqual(2, stream.Position);

        stream.WriteByte(0x04);

        Assert.AreEqual(3, stream.Position);

        Assert.ThrowsAsync<IOException>(async () => {
          switch (writeMethod) {
            case WriteMethod.WriteAsync: await stream.WriteAsync(new byte[] {0x05, 0x06}, 0, 2); break;
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
            case WriteMethod.WriteAsyncFromReadOnlyMemory: await stream.WriteAsync(new ReadOnlyMemory<byte>(new byte[] {0x05, 0x06})); break;
#endif
            default: stream.Write(new byte[] {0x05, 0x06}, 0, 2); break;
          }
        });

        Assert.AreEqual(3, stream.Position);

        switch (writeMethod) {
          case WriteMethod.WriteAsync: await stream.WriteAsync(new byte[] {0x05}, 0, 1); break;
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
          case WriteMethod.WriteAsyncFromReadOnlyMemory: await stream.WriteAsync(new ReadOnlyMemory<byte>(new byte[] {0x05})); break;
#endif
          default: stream.Write(new byte[] {0x05}, 0, 1); break;
        };

        Assert.AreEqual(4, stream.Position);

        Assert.Throws<IOException>(() => stream.WriteByte(0x06));

        Assert.AreEqual(4, stream.Position);
      }

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x02, 0x03, 0x04, 0x05, 0x00, 0x00}, inner.ToArray());
    }

    [Test] public Task TestWrite_LengthNotSpecified() => TestWrite_LengthNotSpecified(WriteMethod.Write);
    [Test] public Task TestWriteAsync_LengthNotSpecified() => TestWrite_LengthNotSpecified(WriteMethod.WriteAsync);
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    [Test] public Task TestWriteAsync_FromReadOnlyMemory_LengthNotSpecified() => TestWrite_LengthNotSpecified(WriteMethod.WriteAsyncFromReadOnlyMemory);
#endif

    private async Task TestWrite_LengthNotSpecified(WriteMethod writeMethod)
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});

      using (var stream = new PartialStream(inner, 2, false)) {
        Assert.AreEqual(8, stream.InnerStream.Length);
        Assert.AreEqual(2, stream.InnerStream.Position);

        Assert.AreEqual(6, stream.Length);
        Assert.AreEqual(0, stream.Position);

        switch (writeMethod) {
          case WriteMethod.WriteAsync: await stream.WriteAsync(new byte[] {0x02, 0x03, 0x04}, 0, 3); break;
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
          case WriteMethod.WriteAsyncFromReadOnlyMemory: await stream.WriteAsync(new ReadOnlyMemory<byte>(new byte[] {0x02, 0x03, 0x04})); break;
#endif
          default: stream.Write(new byte[] {0x02, 0x03, 0x04}, 0, 3); break;
        };

        Assert.AreEqual(3, stream.Position);

        stream.WriteByte(0x05);

        Assert.AreEqual(4, stream.Position);

        // cannot expand MemoryStream
        Assert.ThrowsAsync<NotSupportedException>(async () => {
          switch (writeMethod) {
            case WriteMethod.WriteAsync: await stream.WriteAsync(new byte[] {0x06, 0x07, 0x08}, 0, 3); break;
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
            case WriteMethod.WriteAsyncFromReadOnlyMemory: await stream.WriteAsync(new ReadOnlyMemory<byte>(new byte[] {0x06, 0x07, 0x08})); break;
#endif
            default: stream.Write(new byte[] {0x06, 0x07, 0x08}, 0, 3); break;
          }
        });

        Assert.AreEqual(4, stream.Position);

        switch (writeMethod) {
          case WriteMethod.WriteAsync: await stream.WriteAsync(new byte[] {0x06, 0x07}, 0, 2); break;
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
          case WriteMethod.WriteAsyncFromReadOnlyMemory: await stream.WriteAsync(new ReadOnlyMemory<byte>(new byte[] {0x06, 0x07})); break;
#endif
          default: stream.Write(new byte[] {0x06, 0x07}, 0, 2); break;
        };

        Assert.AreEqual(6, stream.Position);

        // cannot expand MemoryStream
        Assert.Throws<NotSupportedException>(() => stream.WriteByte(0x08));

        Assert.AreEqual(6, stream.Position);
      }

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, inner.ToArray());
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
      using var stream = new PartialStream(new MemoryStream(new byte[8]), 2, 4);

      Assert.Throws(expectedExceptionType, () => stream.Write(buffer, offset, count));
    }

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
      using var stream = new PartialStream(new MemoryStream(new byte[8]), 2, 4);

      Assert.Throws(expectedExceptionType, () => stream.WriteAsync(buffer, offset, count));
    }

    [Test]
    public void TestSeekBegin()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, false);
      var stream = new PartialStream(inner, 4);

      // in range
      Assert.AreEqual(2L, stream.Seek(2, SeekOrigin.Begin));
      Assert.AreEqual(2L, stream.Position);
      Assert.AreEqual(6L, stream.InnerStream.Position);

      // beyond the length
      Assert.AreEqual(10L, stream.Seek(10, SeekOrigin.Begin));
      Assert.AreEqual(10L, stream.Position);
      Assert.AreEqual(14L, stream.InnerStream.Position);

      // before start of stream
      Assert.Throws<IOException>(() => stream.Seek(-1, SeekOrigin.Begin));

      Assert.AreEqual(10L, stream.Position);
      Assert.AreEqual(14L, stream.InnerStream.Position);
    }

    [Test]
    public void TestSeekCurrent()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, false);
      var stream = new PartialStream(inner, 4);

      // in range
      Assert.AreEqual(2L, stream.Seek(2, SeekOrigin.Current));
      Assert.AreEqual(2L, stream.Position);
      Assert.AreEqual(6L, stream.InnerStream.Position);

      // beyond the length
      Assert.AreEqual(10L, stream.Seek(8, SeekOrigin.Current));
      Assert.AreEqual(10L, stream.Position);
      Assert.AreEqual(14L, stream.InnerStream.Position);

      // before start of stream
      Assert.Throws<IOException>(() => stream.Seek(-12, SeekOrigin.Current));

      Assert.AreEqual(10L, stream.Position);
      Assert.AreEqual(14L, stream.InnerStream.Position);
    }

    [Test]
    public void TestSetPosition()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, false);
      var stream = new PartialStream(inner, 4);

      // in range
      stream.Position = 2;

      Assert.AreEqual(2L, stream.Position);
      Assert.AreEqual(6L, stream.InnerStream.Position);

      // beyond the length
      stream.Position = 10;

      Assert.AreEqual(10L, stream.Position);
      Assert.AreEqual(14L, stream.InnerStream.Position);

      // before start of stream
      Assert.Throws<ArgumentOutOfRangeException>(() => stream.Position = -2);

      Assert.AreEqual(10L, stream.Position);
      Assert.AreEqual(14L, stream.InnerStream.Position);
    }

    [Test]
    public void TestSeekEndLengthSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, false);
      var stream = new PartialStream(inner, 4, 4);

      // in range
      Assert.AreEqual(3L, stream.Seek(-1, SeekOrigin.End));
      Assert.AreEqual(3L, stream.Position);
      Assert.AreEqual(7L, stream.InnerStream.Position);

      // beyond the length
      Assert.AreEqual(8L, stream.Seek(4, SeekOrigin.End));
      Assert.AreEqual(8L, stream.Position);
      Assert.AreEqual(12L, stream.InnerStream.Position);

      // before start of stream
      Assert.Throws<IOException>(() => stream.Seek(-5, SeekOrigin.End));

      Assert.AreEqual(8L, stream.Position);
      Assert.AreEqual(12L, stream.InnerStream.Position);
    }

    [Test]
    public void TestSeekEndLengthNotSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, false);
      var stream = new PartialStream(inner, 4);

      // in range
      Assert.AreEqual(3L, stream.Seek(-1, SeekOrigin.End));
      Assert.AreEqual(3L, stream.Position);
      Assert.AreEqual(7L, stream.InnerStream.Position);

      // beyond the length
      Assert.AreEqual(8L, stream.Seek(4, SeekOrigin.End));
      Assert.AreEqual(8L, stream.Position);
      Assert.AreEqual(12L, stream.InnerStream.Position);

      // before start of stream
      Assert.Throws<IOException>(() => stream.Seek(-5, SeekOrigin.End));

      Assert.AreEqual(8L, stream.Position);
      Assert.AreEqual(12L, stream.InnerStream.Position);
    }
  }
}
