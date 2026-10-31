// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Smdn.IO.Streams;

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

      Assert.That(() => _ = stream.InnerStream, Throws.TypeOf<ObjectDisposedException>(), "InnerStream");
      Assert.That(stream.CanRead, Is.False, "CanRead");
      Assert.That(stream.CanWrite, Is.False, "CanWrite");
      Assert.That(stream.CanSeek, Is.False, "CanSeek");
      Assert.That(stream.CanTimeout, Is.False, "CanTimeout");
      Assert.That(() => _ = stream.Length, Throws.TypeOf<ObjectDisposedException>(), "Length");
      Assert.That(() => _ = stream.Position, Throws.TypeOf<ObjectDisposedException>(), "Position");

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

      Assert.That(() => stream.SetLength(0L), Throws.TypeOf<ObjectDisposedException>());
    }
  }

  [Test]
  public void TestSetLength_ReadOnly()
  {
    using (var baseStream = new MemoryStream()) {
      var stream = new NonClosingStream(baseStream, writable: false);

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(stream.Length, Is.Zero);

      Assert.That(() => stream.SetLength(8L), Throws.TypeOf<NotSupportedException>());

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(stream.Length, Is.Zero);

      stream.Dispose();

      Assert.That(() => stream.SetLength(0L), Throws.TypeOf<ObjectDisposedException>());
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

      Assert.That(() => stream.Seek(0L, SeekOrigin.Begin), Throws.TypeOf<ObjectDisposedException>());
      Assert.That(() => stream.Seek(0L, SeekOrigin.Current), Throws.TypeOf<ObjectDisposedException>());
      Assert.That(() => stream.Seek(0L, SeekOrigin.End), Throws.TypeOf<ObjectDisposedException>());
    }
  }

  [Test]
  public async Task TestRead()
  {
    using (var baseStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6 })) {
      var stream = new NonClosingStream(baseStream);
      var buffer = new byte[6];

      Array.Clear(buffer, 0, 6);

      Assert.That(stream.Read(buffer, 0, 1), Is.EqualTo(1));

      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 0, 0, 0, 0, 0 }).AsCollection);

      Array.Clear(buffer, 0, 6);

      Assert.That(stream.Read(buffer, 2, 4), Is.EqualTo(4));

      Assert.That(buffer, Is.EqualTo(new byte[] { 0, 0, 2, 3, 4, 5 }).AsCollection);

      stream.Position = 2L;

      Assert.That(stream.ReadByte(), Is.EqualTo(3));

      stream.Position = 0L;

      Array.Clear(buffer, 0, 6);

      Assert.That(await stream.ReadAsync(buffer, 1, 3), Is.EqualTo(3));

      Assert.That(buffer, Is.EqualTo(new byte[] { 0, 1, 2, 3, 0, 0 }).AsCollection);

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      Array.Clear(buffer, 0, 6);

      Assert.That(await stream.ReadAsync(buffer.AsMemory(2, 2)), Is.EqualTo(2));

      Assert.That(buffer, Is.EqualTo(new byte[] { 0, 0, 4, 5, 0, 0 }).AsCollection);
#endif

      stream.Dispose();

      Assert.That(() => _ = stream.Read(buffer, 0, 6), Throws.TypeOf<ObjectDisposedException>());
      Assert.That(() => stream.ReadAsync(buffer, 0, 6), Throws.TypeOf<ObjectDisposedException>());
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      Assert.That(async () => _ = await stream.ReadAsync(buffer.AsMemory(0, 6)), Throws.TypeOf<ObjectDisposedException>());
#endif
      Assert.That(stream.ReadByte, Throws.TypeOf<ObjectDisposedException>());
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

    Assert.That(
      () => _ = stream.Read(buffer, offset, count),
      Throws.TypeOf(expectedExceptionType)
    );
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

    Assert.That(
      () => stream.ReadAsync(buffer, offset, count),
      Throws.TypeOf(expectedExceptionType)
    );
  }

  [Test]
  public void TestWrite()
  {
    var buffer = new byte[4];

    Array.Clear(buffer, 0, buffer.Length);

    using (var baseStream = new MemoryStream(buffer, true)) {
      var stream = new NonClosingStream(baseStream);

      stream.Write(new byte[] { 1 }, 0, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 0, 0, 0 }).AsCollection);

      stream.WriteByte(2);

      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 2, 0, 0 }).AsCollection);

      Assert.That(async () => await stream.WriteAsync(new byte[] { 3 }, 0, 1), Throws.Nothing);
      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 2, 3, 0 }).AsCollection);

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.That(async () => await stream.WriteAsync(new byte[] { 4 }.AsMemory()), Throws.Nothing);
      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 2, 3, 4 }).AsCollection);
#endif

      Assert.That(
        () => stream.Write(new byte[] { 3, 4, 5, 6 }, 0, 4),
        Throws.TypeOf<NotSupportedException>(),
        "expand base array"
      );

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 2, 3, 4 }).AsCollection);
#else
      Assert.That(buffer, Is.EqualTo(new byte[] { 1, 2, 3, 0 }).AsCollection);
#endif

      stream.Position = 0L;

      stream.Dispose();

      Assert.That(() => stream.Write(new byte[] { 0, 1, 2, 3 }, 0, 4), Throws.TypeOf<ObjectDisposedException>());
      Assert.That(() => stream.WriteAsync(new byte[] { 0, 1, 2, 3 }, 0, 4), Throws.TypeOf<ObjectDisposedException>());
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.That(async () => await stream.WriteAsync((new byte[] { 0, 1, 2, 3 }).AsMemory()), Throws.TypeOf<ObjectDisposedException>());
#endif
      Assert.That(() => stream.WriteByte(4), Throws.TypeOf<ObjectDisposedException>());
    }
  }

  [Test]
  public void TestWrite_ReadOnly()
  {
    using (var baseStream = new MemoryStream()) {
      var stream = new NonClosingStream(baseStream, writable: false);

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(baseStream.Position, Is.Zero);

      Assert.That(() => stream.Write(new byte[] { 1 }, 0, 1), Throws.TypeOf<NotSupportedException>());

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(baseStream.Position, Is.Zero);

      Assert.That(() => stream.WriteByte(2), Throws.TypeOf<NotSupportedException>());

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(baseStream.Position, Is.Zero);

      Assert.That(async () => await stream.WriteAsync(new byte[] { 1 }, 0, 1), Throws.TypeOf<NotSupportedException>());

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(baseStream.Position, Is.Zero);

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.That(async () => await stream.WriteAsync(new byte[] { 1 }.AsMemory()), Throws.TypeOf<NotSupportedException>());

      Assert.That(baseStream.Length, Is.Zero);
      Assert.That(baseStream.Position, Is.Zero);
#endif

#if SYSTEM_IO_STREAM_BEGINWRITE
      Assert.That(() => stream.BeginWrite(new byte[] { 1 }, 0, 1, null!, null!), Throws.TypeOf<NotSupportedException>());
#endif
      Assert.That(() => stream.WriteAsync(new byte[] { 1 }, 0, 1), Throws.TypeOf<NotSupportedException>());

      Assert.That(stream.Flush, Throws.Nothing);
      Assert.That(stream.FlushAsync, Throws.Nothing);

      stream.Dispose();

      Assert.That(() => stream.Write(new byte[] { 1 }, 0, 1), Throws.TypeOf<ObjectDisposedException>());
      Assert.That(() => stream.WriteByte(2), Throws.TypeOf<ObjectDisposedException>());
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.That(() => stream.WriteAsync(new byte[] { 1 }, 0, 1), Throws.TypeOf<ObjectDisposedException>());
      Assert.That(async () => await stream.WriteAsync(new byte[] { 1 }.AsMemory()), Throws.TypeOf<ObjectDisposedException>());
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

    Assert.That(
      () => stream.Write(buffer, offset, count),
      Throws.TypeOf(expectedExceptionType)
    );
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

    Assert.That(
      () => stream.WriteAsync(buffer, offset, count),
      Throws.TypeOf(expectedExceptionType)
    );
  }

  [Test]
  public void TestClose()
  {
    using (var baseStream = new MemoryStream()) {
      var stream = new NonClosingStream(baseStream);

      stream.Dispose();

      Assert.That(delegate { baseStream.ReadByte(); }, Throws.Nothing, "ReadByte");
      Assert.That(delegate { baseStream.WriteByte(1); }, Throws.Nothing, "WriteByte");

      Assert.That(stream.Dispose, Throws.Nothing, "Dispose again");
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

      Assert.That(() => stream.WriteByte(1), Throws.Nothing);

      Assert.That(stream.Dispose, Throws.TypeOf<NotSupportedException>());

      Assert.That(() => baseStream.WriteByte(1), Throws.Nothing);
    }
  }

  [Test]
  public void TestCloseInnerStreamNotFlushedIfStreamIsNotWritable()
  {
    using (var baseStream = new TestFlushMemoryStream(false)) {
      var stream = new NonClosingStream(baseStream);

      Assert.That(stream.ReadByte, Throws.Nothing);

      Assert.That(stream.Dispose, Throws.Nothing);

      Assert.That(baseStream.ReadByte, Throws.Nothing);
    }
  }
}
