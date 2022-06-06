// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture]
  public class BinaryWriterTests {
    private class NonWritableStream : Stream {
      public override bool CanRead {
        get { return true; }
      }

      public override bool CanSeek {
        get { return false; }
      }

      public override bool CanWrite {
        get { return false; }
      }


      public override long Length {
        get { throw new NotImplementedException(); }
      }

      public override long Position {
        get { throw new NotImplementedException(); }
        set { throw new NotImplementedException(); }
      }

      public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }
      public override void SetLength(long @value) { throw new NotImplementedException(); }
      public override void Flush() { throw new NotImplementedException(); }
      public override int Read(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
      public override void Write(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
    }

    [Test]
    public void TestConstructWithNonWritableStream()
    {
      Assert.Throws<ArgumentException>(() => {
        using (var writer = new Smdn.IO.Binary.BinaryWriter(new NonWritableStream())) {
        }
      });
    }

    [Test]
    public void TestClose()
    {
      TestCloseDispose(true);
    }

    [Test]
    public void TestDispose()
    {
      TestCloseDispose(false);
    }

    private void TestCloseDispose(bool close)
    {
      using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
        Assert.IsNotNull(writer.BaseStream);
        Assert.IsFalse(writer.LeaveBaseStreamOpen);

        var baseStream = writer.BaseStream;

        Assert.IsNotNull(baseStream);

        if (close) {
          writer.Close();
        }
        else {
          (writer as IDisposable).Dispose();
        }

        Assert.Throws<ObjectDisposedException>(() => Assert.IsNull(writer.BaseStream));

        Assert.Throws<ObjectDisposedException>(() => baseStream.WriteByte(0x00));
      }
    }

    [Test]
    public void TestClose2()
    {
      using (var stream = new MemoryStream()) {
        try {
          stream.WriteByte(0x00);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown by base stream");
        }

        using (var writer = new Smdn.IO.Binary.BinaryWriter(stream)) {
          Assert.IsNotNull(writer.BaseStream);
          Assert.IsFalse(writer.LeaveBaseStreamOpen);
        }

        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00));
      }
    }

    private class BinaryWriterEx : Smdn.IO.Binary.BinaryWriter {
      public BinaryWriterEx(Stream stream)
        : base(stream, true)
      {
      }
    }

    [Test]
    public void TestCloseLeaveBaseStreamOpen()
    {
      using (var writer = new BinaryWriterEx(new MemoryStream())) {
        Assert.IsNotNull(writer.BaseStream);
        Assert.IsTrue(writer.LeaveBaseStreamOpen);

        var baseStream = writer.BaseStream;

        Assert.IsNotNull(baseStream);

        writer.Close();

        Assert.Throws<ObjectDisposedException>(() => Assert.IsNull(writer.BaseStream));

        try {
          baseStream.WriteByte(0x00);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown by base stream");
        }
      }
    }

    [Test]
    public void TestFlush()
    {
      using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Write((int)0);
        writer.Flush();

        Assert.AreEqual(4L, writer.BaseStream.Position);

        writer.Close();

        Assert.Throws<ObjectDisposedException>(() => writer.Flush());
      }
    }

    [Test]
    public void TestWriteByteArray()
    {
      var data = new byte[] {0x11, 0x22, 0x33, 0x44};

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Write(data);
        writer.Flush();

        Assert.AreEqual(4L, writer.BaseStream.Position);

        writer.Close();

        CollectionAssert.AreEqual(data, stream.ToArray());

        Assert.Throws<ObjectDisposedException>(() => writer.Write(data));
      }

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Write(data, 1, 2);
        writer.Flush();

        Assert.AreEqual(2L, writer.BaseStream.Position);

        writer.Close();

        CollectionAssert.AreEqual(data.Skip(1).Take(2).ToArray(), stream.ToArray());

        Assert.Throws<ObjectDisposedException>(() => writer.Write(data, 1, 2));
      }
    }

    [Test]
    public void TestWriteArraySegmentOfByte()
    {
      var data = new byte[] {0x11, 0x22, 0x33, 0x44};

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Write(new ArraySegment<byte>(data, 0, 4));
        writer.Flush();

        Assert.AreEqual(4L, writer.BaseStream.Position);

        writer.Close();

        CollectionAssert.AreEqual(data, stream.ToArray());

        Assert.Throws<ObjectDisposedException>(() => writer.Write(data));
      }

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Write(new ArraySegment<byte>(data, 1, 2));
        writer.Flush();

        Assert.AreEqual(2L, writer.BaseStream.Position);

        writer.Close();

        CollectionAssert.AreEqual(data.Skip(1).Take(2).ToArray(), stream.ToArray());

        Assert.Throws<ObjectDisposedException>(() => writer.Write(new ArraySegment<byte>(data, 1, 2)));
      }
    }

    [Test]
    public void TestWriteArraySegmentOfByteEmpty()
    {
      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        Assert.Throws<ArgumentException>(() => writer.Write(new ArraySegment<byte>()));

        writer.Flush();

        Assert.AreEqual(0L, writer.BaseStream.Position);
      }
    }

    [Test]
    public void TestWriteZero()
    {
      Predicate<byte> allzero = delegate(byte b) {
        return b == 0x00;
      };

      foreach (var len in new[] {
        0,
        3,
        7,
        8,
        12,
      }) {
        byte[] arr;

        using (var stream = new MemoryStream(0x10)) {
          var writer = new Smdn.IO.Binary.BinaryWriter(stream);

          writer.WriteZero(len);
          writer.Flush();

          Assert.AreEqual(len, writer.BaseStream.Position);

          writer.Close();

          stream.Dispose();

          arr = stream.ToArray();

          Assert.AreEqual(len, arr.Length);
          Assert.IsTrue(Array.TrueForAll(arr, allzero));
        }
      }
    }

    [Test]
    public void TestWriteZeroBytesBuffer()
    {
      var zero = new byte[0];

      using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Write(zero);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Close();

        try {
          writer.Write(zero);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }
      }
    }

    [Test]
    public void TestWriteZeroZeroLength()
    {
      using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.WriteZero(0);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.WriteZero(0L);

        Assert.AreEqual(0L, writer.BaseStream.Position);

        writer.Close();

        try {
          writer.WriteZero(0);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }

        try {
          writer.WriteZero(0L);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }
      }
    }

    [Test]
    public void TestWriteInt32()
    {
      using (var stream = new MemoryStream()) {
        using (var writer = new Smdn.IO.Binary.BinaryWriter(stream)) {
          writer.Write((int)0x11223344);

          writer.Close();
        }

        CollectionAssert.AreEqual(
          BitConverter.IsLittleEndian
            ? new byte[] {0x44, 0x33, 0x22, 0x11}
            : new byte[] {0x11, 0x22, 0x33, 0x44},
          stream.ToArray()
        );
      }
    }

    [Test]
    public void TestWrite()
    {
      using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
        foreach (var test in new[] {
          Tuple.Create<object, long>((byte)0,       1L),
          Tuple.Create<object, long>((sbyte)0,      1L),
          Tuple.Create<object, long>((short)0,      2L),
          Tuple.Create<object, long>((ushort)0,     2L),
          Tuple.Create<object, long>((int)0,        4L),
          Tuple.Create<object, long>((uint)0,       4L),
          Tuple.Create<object, long>((long)0,       8L),
          Tuple.Create<object, long>((ulong)0,      8L),
          Tuple.Create<object, long>(UInt24.Zero,   3L),
          Tuple.Create<object, long>(UInt48.Zero,   6L),
          Tuple.Create<object, long>(FourCC.Empty,  4L),
        }) {
          writer.BaseStream.Seek(0L, SeekOrigin.Begin);

          Assert.AreEqual(0L, writer.BaseStream.Position);

          try {
            typeof(Smdn.IO.Binary.BinaryWriter).GetTypeInfo()
                                               .GetMethod("Write",
                                                          new[] {test.Item1.GetType()},
                                                          null)
                                               !.Invoke(writer, new[] {test.Item1});
          }
          catch (MissingMethodException) {
            Assert.Fail("invocation failed: type = {0}", test.Item1.GetType().FullName);
          }

          Assert.AreEqual(test.Item2, writer.BaseStream.Position);
        }
      }
    }

    [Test]
    public void TestWriteToClosedWriter()
    {
      foreach (var arg in new object[] {
        (byte)0,
        (sbyte)0,
        (short)0,
        (ushort)0,
        (int)0,
        (uint)0,
        (long)0,
        (ulong)0,
        UInt24.Zero,
        UInt48.Zero,
        FourCC.Empty,
      }) {
        using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
          writer.Close();

          var ex = Assert.Throws<TargetInvocationException>(() => {
            typeof(Smdn.IO.Binary.BinaryWriter).GetTypeInfo()
                                               .GetMethod("Write",
                                                          new[] {arg.GetType()},
                                                          null)
                                               !.Invoke(writer, new[] {arg});
          });

          Assert.IsInstanceOf<ObjectDisposedException>(ex!.InnerException);
        }
      }
    }
  }
}
