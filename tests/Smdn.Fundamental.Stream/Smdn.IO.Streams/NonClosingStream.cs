// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

using Smdn;

namespace Smdn.IO.Streams {
  [TestFixture]
  public class NonClosingStreamTests {
    [Test]
    public void TestProperties()
    {
      using (var baseStream = new MemoryStream(new byte[8])) {
        var initialCanRead = baseStream.CanRead;
        var initialCanWrite = baseStream.CanWrite;
        var initialCanSeek = baseStream.CanSeek;
        var initialCanTimeout = baseStream.CanTimeout;
        var initialLength = baseStream.Length;
        var initialPosition = baseStream.Position;

        var stream = new NonClosingStream(baseStream);

        Assert.That(stream.InnerStream, Is.SameAs(baseStream));
        Assert.That(stream.CanRead, Is.EqualTo(baseStream.CanRead), "CanRead");
        Assert.That(stream.CanWrite, Is.EqualTo(baseStream.CanWrite), "CanWrite");
        Assert.That(stream.CanSeek, Is.EqualTo(baseStream.CanSeek), "CanSeek");
        Assert.That(stream.CanTimeout, Is.EqualTo(baseStream.CanTimeout), "CanTimeout");
        Assert.That(stream.Length, Is.EqualTo(baseStream.Length), "Length");
        Assert.That(stream.Position, Is.EqualTo(baseStream.Position), "Position");

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate { Assert.That(stream.InnerStream, Is.Null); }, "InnerStream");
        Assert.That(stream.CanRead, Is.False, "CanRead");
        Assert.That(stream.CanWrite, Is.False, "CanWrite");
        Assert.That(stream.CanSeek, Is.False, "CanSeek");
        Assert.That(stream.CanTimeout, Is.False, "CanTimeout");
        Assert.Throws<ObjectDisposedException>(delegate { Assert.That(stream.Length, Is.EqualTo(-1)); }, "Length");
        Assert.Throws<ObjectDisposedException>(delegate { Assert.That(stream.Position, Is.EqualTo(-1)); }, "Position");

        Assert.That(baseStream.CanRead, Is.EqualTo(initialCanRead), "CanRead");
        Assert.That(baseStream.CanWrite, Is.EqualTo(initialCanWrite), "CanWrite");
        Assert.That(baseStream.CanSeek, Is.EqualTo(initialCanSeek), "CanSeek");
        Assert.That(baseStream.CanTimeout, Is.EqualTo(initialCanTimeout), "CanTimeout");
        Assert.That(baseStream.Length, Is.EqualTo(initialLength), "Length");
        Assert.That(baseStream.Position, Is.EqualTo(initialPosition), "Position");
      }
    }

    [Test]
    public void TestProperties_ReadOnly()
    {
      using (var baseStream = new MemoryStream(new byte[8])) {
        var initialCanWrite = baseStream.CanWrite;

        var stream = new NonClosingStream(baseStream, writable: false);

        Assert.That(stream.CanWrite, Is.False, "CanWrite");

        stream.Dispose();

        Assert.That(stream.CanWrite, Is.False, "CanWrite");

        Assert.That(baseStream.CanWrite, Is.EqualTo(initialCanWrite), "CanWrite");
      }
    }

    [Test]
    public void TestSetLength()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream);

        Assert.That(stream.Length, Is.EqualTo(baseStream.Length), "before");

        stream.SetLength(8L);

        Assert.That(stream.Length, Is.EqualTo(8L));
        Assert.That(stream.Length, Is.EqualTo(baseStream.Length), "after");

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate{ stream.SetLength(0L); });
      }
    }

    [Test]
    public void TestSetLength_ReadOnly()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream, writable: false);

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(stream.Length, Is.EqualTo(0L));

        Assert.Throws<NotSupportedException>(() => stream.SetLength(8L));

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(stream.Length, Is.EqualTo(0L));

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L));
      }
    }

    [Test]
    public void TestSeek()
    {
      using (var baseStream = new MemoryStream(new byte[8])) {
        var stream = new NonClosingStream(baseStream);

        Assert.That(stream.Seek(4L, SeekOrigin.Begin), Is.EqualTo(4L));
        Assert.That(baseStream.Position, Is.EqualTo(4L));

        Assert.That(stream.Seek(2L, SeekOrigin.Current), Is.EqualTo(6L));
        Assert.That(baseStream.Position, Is.EqualTo(6L));

        Assert.That(stream.Seek(-6L, SeekOrigin.End), Is.EqualTo(2L));
        Assert.That(baseStream.Position, Is.EqualTo(2L));

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate{ stream.Seek(0L, SeekOrigin.Begin); });
        Assert.Throws<ObjectDisposedException>(delegate{ stream.Seek(0L, SeekOrigin.Current); });
        Assert.Throws<ObjectDisposedException>(delegate{ stream.Seek(0L, SeekOrigin.End); });
      }
    }

    [Test]
    public async Task TestRead()
    {
      using (var baseStream = new MemoryStream(new byte[] {1, 2, 3, 4, 5, 6})) {
        var stream = new NonClosingStream(baseStream);
        var buffer = new byte[6];

        Array.Clear(buffer, 0, 6);

        Assert.That(stream.Read(buffer, 0, 1), Is.EqualTo(1));

        Assert.That(buffer, Is.EqualTo(new byte[] {1, 0, 0, 0, 0, 0}).AsCollection);

        Array.Clear(buffer, 0, 6);

        Assert.That(stream.Read(buffer, 2, 4), Is.EqualTo(4));

        Assert.That(buffer, Is.EqualTo(new byte[] {0, 0, 2, 3, 4, 5}).AsCollection);

        stream.Position = 2L;

        Assert.That(stream.ReadByte(), Is.EqualTo(3));

        stream.Position = 0L;

        Array.Clear(buffer, 0, 6);

        Assert.That(await stream.ReadAsync(buffer, 1, 3), Is.EqualTo(3));

        Assert.That(buffer, Is.EqualTo(new byte[] {0, 1, 2, 3, 0, 0}).AsCollection);

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        Array.Clear(buffer, 0, 6);

        Assert.That(await stream.ReadAsync(buffer.AsMemory(2, 2)), Is.EqualTo(2));

        Assert.That(buffer, Is.EqualTo(new byte[] {0, 0, 4, 5, 0, 0}).AsCollection);
#endif

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, 6));
        Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(buffer, 0, 6));
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.ReadAsync(buffer.AsMemory(0, 6)));
#endif
        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
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
      using var stream = new NonClosingStream(new MemoryStream(new byte[8]));

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
      using var stream = new NonClosingStream(new MemoryStream(new byte[8]));

      Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count));
    }

    [Test]
    public void TestWrite()
    {
      var buffer = new byte[4];

      Array.Clear(buffer, 0, buffer.Length);

      using (var baseStream = new MemoryStream(buffer, true)) {
        var stream = new NonClosingStream(baseStream);

        stream.Write(new byte[] {1}, 0, 1);

        Assert.That(buffer, Is.EqualTo(new byte[] {1, 0, 0, 0}).AsCollection);

        stream.WriteByte(2);

        Assert.That(buffer, Is.EqualTo(new byte[] {1, 2, 0, 0}).AsCollection);

        Assert.DoesNotThrowAsync(async () => await stream.WriteAsync(new byte[] { 3 }, 0, 1));
        Assert.That(buffer, Is.EqualTo(new byte[] {1, 2, 3, 0}).AsCollection);

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        Assert.DoesNotThrowAsync(async () => await stream.WriteAsync(new byte[] { 4 }.AsMemory()));
        Assert.That(buffer, Is.EqualTo(new byte[] {1, 2, 3, 4}).AsCollection);
#endif

        Assert.Throws<NotSupportedException>(delegate {
          stream.Write(new byte[] {3, 4, 5, 6}, 0, 4);
        }, "expand base array");

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        Assert.That(buffer, Is.EqualTo(new byte[] {1, 2, 3, 4}).AsCollection);
#else
        Assert.That(buffer, Is.EqualTo(new byte[] {1, 2, 3, 0}).AsCollection);
#endif

        stream.Position = 0L;

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.Write(new byte[] {0, 1, 2, 3}, 0, 4));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(new byte[] {0, 1, 2, 3}, 0, 4));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.WriteAsync((new byte[] {0, 1, 2, 3}).AsMemory()));
#endif
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(4));
      }
    }

    [Test]
    public void TestWrite_ReadOnly()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream, writable: false);

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(baseStream.Position, Is.EqualTo(0L));

        Assert.Throws<NotSupportedException>(() => stream.Write(new byte[] { 1 }, 0, 1));

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(baseStream.Position, Is.EqualTo(0L));

        Assert.Throws<NotSupportedException>(() => stream.WriteByte(2));

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(baseStream.Position, Is.EqualTo(0L));

        Assert.ThrowsAsync<NotSupportedException>(async () => await stream.WriteAsync(new byte[] { 1 }, 0, 1));

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(baseStream.Position, Is.EqualTo(0L));

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        Assert.ThrowsAsync<NotSupportedException>(async () => await stream.WriteAsync(new byte[] { 1 }.AsMemory()));

        Assert.That(baseStream.Length, Is.EqualTo(0L));
        Assert.That(baseStream.Position, Is.EqualTo(0L));
#endif

#if SYSTEM_IO_STREAM_BEGINWRITE
        Assert.Throws<NotSupportedException>(() => stream.BeginWrite(new byte[] { 1 }, 0, 1, null!, null!));
#endif
        Assert.Throws<NotSupportedException>(() => stream.WriteAsync(new byte[] { 1 }, 0, 1));

        Assert.DoesNotThrow(stream.Flush);
        Assert.DoesNotThrowAsync(async () => await stream.FlushAsync());

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.Write(new byte[] { 1 }, 0, 1));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(2));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(new byte[] { 1 }, 0, 1));
        Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.WriteAsync(new byte[] { 1 }.AsMemory()));
#endif
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
      using var stream = new NonClosingStream(new MemoryStream(new byte[4], true));

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
      using var stream = new NonClosingStream(new MemoryStream(new byte[4], true));

      Assert.Throws(expectedExceptionType, () => stream.WriteAsync(buffer, offset, count));
    }

    [Test]
    public void TestClose()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream);

        stream.Dispose();

        Assert.DoesNotThrow(delegate {baseStream.ReadByte();}, "ReadByte");
        Assert.DoesNotThrow(delegate {baseStream.WriteByte(1);}, "WriteByte");

        Assert.DoesNotThrow(delegate {stream.Dispose();}, "Dispose again");
      }
    }

    private class TestFlushMemoryStream : MemoryStream {
      public override bool CanWrite {
        get { return canWrite; }
      }

      public TestFlushMemoryStream(bool canWrite)
      {
        this.canWrite = canWrite;
      }

      public override void Flush()
      {
        throw new NotSupportedException();
      }

      private readonly bool canWrite;
    }

    [Test]
    public void TestCloseInnerStreamFlushed()
    {
      using (var baseStream = new TestFlushMemoryStream(true)) {
        var stream = new NonClosingStream(baseStream);

        Assert.DoesNotThrow(delegate {stream.WriteByte(1);});

        Assert.Throws<NotSupportedException>(delegate {stream.Dispose();});

        Assert.DoesNotThrow(delegate {baseStream.WriteByte(1);});
      }
    }

    [Test]
    public void TestCloseInnerStreamNotFlushedIfStreamIsNotWritable()
    {
      using (var baseStream = new TestFlushMemoryStream(false)) {
        var stream = new NonClosingStream(baseStream);

        Assert.DoesNotThrow(delegate {stream.ReadByte();});

        Assert.DoesNotThrow(delegate {stream.Dispose();});

        Assert.DoesNotThrow(delegate {baseStream.ReadByte();});
      }
    }
  }
}
