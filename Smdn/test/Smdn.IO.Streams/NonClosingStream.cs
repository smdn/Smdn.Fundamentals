using System;
using System.Collections.Generic;
using System.IO;
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

        Assert.AreSame(baseStream, stream.InnerStream);
        Assert.AreEqual(baseStream.CanRead, stream.CanRead, "CanRead");
        Assert.AreEqual(baseStream.CanWrite, stream.CanWrite, "CanWrite");
        Assert.AreEqual(baseStream.CanSeek, stream.CanSeek, "CanSeek");
        Assert.AreEqual(baseStream.CanTimeout, stream.CanTimeout, "CanTimeout");
        Assert.AreEqual(baseStream.Length, stream.Length, "Length");
        Assert.AreEqual(baseStream.Position, stream.Position, "Position");

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate { Assert.IsNull(stream.InnerStream); }, "InnerStream");
        Assert.IsFalse(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");
        Assert.Throws<ObjectDisposedException>(delegate { Assert.AreEqual(-1, stream.Length); }, "Length");
        Assert.Throws<ObjectDisposedException>(delegate { Assert.AreEqual(-1, stream.Position); }, "Position");

        Assert.AreEqual(initialCanRead, baseStream.CanRead, "CanRead");
        Assert.AreEqual(initialCanWrite, baseStream.CanWrite, "CanWrite");
        Assert.AreEqual(initialCanSeek, baseStream.CanSeek, "CanSeek");
        Assert.AreEqual(initialCanTimeout, baseStream.CanTimeout, "CanTimeout");
        Assert.AreEqual(initialLength, baseStream.Length, "Length");
        Assert.AreEqual(initialPosition, baseStream.Position, "Position");
      }
    }

    [Test]
    public void TestProperties_ReadOnly()
    {
      using (var baseStream = new MemoryStream(new byte[8])) {
        var initialCanWrite = baseStream.CanWrite;

        var stream = new NonClosingStream(baseStream, writable: false);

        Assert.IsFalse(stream.CanWrite, "CanWrite");

        stream.Dispose();

        Assert.IsFalse(stream.CanWrite, "CanWrite");

        Assert.AreEqual(initialCanWrite, baseStream.CanWrite, "CanWrite");
      }
    }

    [Test]
    public void TestSetLength()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream);

        Assert.AreEqual(baseStream.Length, stream.Length, "before");

        stream.SetLength(8L);

        Assert.AreEqual(8L, stream.Length);
        Assert.AreEqual(baseStream.Length, stream.Length, "after");

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate{ stream.SetLength(0L); });
      }
    }

    [Test]
    public void TestSetLength_ReadOnly()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream, writable: false);

        Assert.AreEqual(0L, baseStream.Length);
        Assert.AreEqual(0L, stream.Length);

        Assert.Throws<NotSupportedException>(() => stream.SetLength(8L));

        Assert.AreEqual(0L, baseStream.Length);
        Assert.AreEqual(0L, stream.Length);

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L));
      }
    }

    [Test]
    public void TestSeek()
    {
      using (var baseStream = new MemoryStream(new byte[8])) {
        var stream = new NonClosingStream(baseStream);

        Assert.AreEqual(4L, stream.Seek(4L, SeekOrigin.Begin));
        Assert.AreEqual(4L, baseStream.Position);

        Assert.AreEqual(6L, stream.Seek(2L, SeekOrigin.Current));
        Assert.AreEqual(6L, baseStream.Position);

        Assert.AreEqual(2L, stream.Seek(-6L, SeekOrigin.End));
        Assert.AreEqual(2L, baseStream.Position);

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate{ stream.Seek(0L, SeekOrigin.Begin); });
        Assert.Throws<ObjectDisposedException>(delegate{ stream.Seek(0L, SeekOrigin.Current); });
        Assert.Throws<ObjectDisposedException>(delegate{ stream.Seek(0L, SeekOrigin.End); });
      }
    }

    [Test]
    public void TestRead()
    {
      using (var baseStream = new MemoryStream(new byte[] {1, 2, 3, 4})) {
        var stream = new NonClosingStream(baseStream);
        var buffer = new byte[6];

        Array.Clear(buffer, 0, 6);

        Assert.AreEqual(1, stream.Read(buffer, 0, 1));

        CollectionAssert.AreEqual(new byte[] {1, 0, 0, 0, 0, 0},
                                  buffer);

        Array.Clear(buffer, 0, 6);

        Assert.AreEqual(3, stream.Read(buffer, 2, 4));

        CollectionAssert.AreEqual(new byte[] {0, 0, 2, 3, 4, 0},
                                  buffer);

        stream.Position = 2L;

        Assert.AreEqual(3, stream.ReadByte());

        stream.Position = 0L;

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate{ stream.Read(buffer, 0, 6); });
        Assert.Throws<ObjectDisposedException>(delegate{ stream.ReadByte(); });
      }
    }

    [Test]
    public void TestWrite()
    {
      var buffer = new byte[4];

      Array.Clear(buffer, 0, buffer.Length);

      using (var baseStream = new MemoryStream(buffer, true)) {
        var stream = new NonClosingStream(baseStream);

        stream.Write(new byte[] {1}, 0, 1);

        CollectionAssert.AreEqual(new byte[] {1, 0, 0, 0},
                                  buffer);

        stream.WriteByte(2);

        CollectionAssert.AreEqual(new byte[] {1, 2, 0, 0},
                                  buffer);

        Assert.Throws<NotSupportedException>(delegate {
          stream.Write(new byte[] {3, 4, 5, 6}, 0, 4);
        }, "expand base array");

        CollectionAssert.AreEqual(new byte[] {1, 2, 0, 0},
                                  buffer);

        stream.Position = 0L;

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(delegate{ stream.Write(new byte[] {0, 1, 2, 3}, 0, 4); });
        Assert.Throws<ObjectDisposedException>(delegate{ stream.WriteByte(4); });
      }
    }

    [Test]
    public void TestWrite_ReadOnly()
    {
      using (var baseStream = new MemoryStream()) {
        var stream = new NonClosingStream(baseStream, writable: false);

        Assert.AreEqual(0L, baseStream.Length);
        Assert.AreEqual(0L, baseStream.Position);

        Assert.Throws<NotSupportedException>(() => stream.Write(new byte[] { 1 }, 0, 1));

        Assert.AreEqual(0L, baseStream.Length);
        Assert.AreEqual(0L, baseStream.Position);

        Assert.Throws<NotSupportedException>(() => stream.WriteByte(2));

        Assert.AreEqual(0L, baseStream.Length);
        Assert.AreEqual(0L, baseStream.Position);

        Assert.Throws<NotSupportedException>(() => stream.BeginWrite(new byte[] { 1 }, 0, 1, null, null));
        Assert.Throws<NotSupportedException>(() => stream.WriteAsync(new byte[] { 1 }, 0, 1).Wait());

        Assert.DoesNotThrow(stream.Flush);
        Assert.DoesNotThrow(() => stream.FlushAsync().Wait());

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.Write(new byte[] { 1 }, 0, 1));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(2));
      }
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
