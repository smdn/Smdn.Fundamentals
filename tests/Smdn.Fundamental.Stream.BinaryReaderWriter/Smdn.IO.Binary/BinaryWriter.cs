// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
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
        Assert.That(writer.BaseStream, Is.Not.Null);
        Assert.That(writer.LeaveBaseStreamOpen, Is.False);

        var baseStream = writer.BaseStream;

        Assert.That(baseStream, Is.Not.Null);

        if (close) {
          writer.Close();
        }
        else {
          writer.Dispose();
        }

        Assert.Throws<ObjectDisposedException>(() => Assert.That(writer.BaseStream, Is.Null));

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
          Assert.That(writer.BaseStream, Is.Not.Null);
          Assert.That(writer.LeaveBaseStreamOpen, Is.False);
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
        Assert.That(writer.BaseStream, Is.Not.Null);
        Assert.That(writer.LeaveBaseStreamOpen, Is.True);

        var baseStream = writer.BaseStream;

        Assert.That(baseStream, Is.Not.Null);

        writer.Close();

        Assert.Throws<ObjectDisposedException>(() => Assert.That(writer.BaseStream, Is.Null));

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
        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.Write((int)0);
        writer.Flush();

        Assert.That(writer.BaseStream.Position, Is.EqualTo(4L));

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

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.Write(data);
        writer.Flush();

        Assert.That(writer.BaseStream.Position, Is.EqualTo(4L));

        writer.Close();

        Assert.That(stream.ToArray(), Is.EqualTo(data).AsCollection);

        Assert.Throws<ObjectDisposedException>(() => writer.Write(data));
      }

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.Write(data, 1, 2);
        writer.Flush();

        Assert.That(writer.BaseStream.Position, Is.EqualTo(2L));

        writer.Close();

        Assert.That(stream.ToArray(), Is.EqualTo(data.Skip(1).Take(2).ToArray()).AsCollection);

        Assert.Throws<ObjectDisposedException>(() => writer.Write(data, 1, 2));
      }
    }

    [Test]
    public void TestWriteArraySegmentOfByte()
    {
      var data = new byte[] {0x11, 0x22, 0x33, 0x44};

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.Write(new ArraySegment<byte>(data, 0, 4));
        writer.Flush();

        Assert.That(writer.BaseStream.Position, Is.EqualTo(4L));

        writer.Close();

        Assert.That(stream.ToArray(), Is.EqualTo(data).AsCollection);

        Assert.Throws<ObjectDisposedException>(() => writer.Write(data));
      }

      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.Write(new ArraySegment<byte>(data, 1, 2));
        writer.Flush();

        Assert.That(writer.BaseStream.Position, Is.EqualTo(2L));

        writer.Close();

        Assert.That(stream.ToArray(), Is.EqualTo(data.Skip(1).Take(2).ToArray()).AsCollection);

        Assert.Throws<ObjectDisposedException>(() => writer.Write(new ArraySegment<byte>(data, 1, 2)));
      }
    }

    [Test]
    public void TestWriteArraySegmentOfByteEmpty()
    {
      using (var stream = new MemoryStream()) {
        var writer = new Smdn.IO.Binary.BinaryWriter(stream);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        Assert.Throws<ArgumentException>(() => writer.Write(new ArraySegment<byte>()));

        writer.Flush();

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));
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

          Assert.That(writer.BaseStream.Position, Is.EqualTo(len));

          writer.Close();

          stream.Dispose();

          arr = stream.ToArray();

          Assert.That(arr.Length, Is.EqualTo(len));
          Assert.That(Array.TrueForAll(arr, allzero), Is.True);
        }
      }
    }

    [Test]
    public void TestWriteZeroBytesBuffer()
    {
      var zero = new byte[0];

      using (var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream())) {
        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.Write(zero);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

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
        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.WriteZero(0);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

        writer.WriteZero(0L);

        Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

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

        Assert.That(
          stream.ToArray(),
          Is.EqualTo(
            BitConverter.IsLittleEndian
              ? new byte[] {0x44, 0x33, 0x22, 0x11}
              : new byte[] {0x11, 0x22, 0x33, 0x44}
          ).AsCollection
        );
      }
    }

    private static IEnumerable YieldTestCases_Write()
    {
      yield return new object[] { (byte)0,       1L };
      yield return new object[] { (sbyte)0,      1L };
      yield return new object[] { (short)0,      2L };
      yield return new object[] { (ushort)0,     2L };
      yield return new object[] { (int)0,        4L };
      yield return new object[] { (uint)0,       4L };
      yield return new object[] { (long)0,       8L };
      yield return new object[] { (ulong)0,      8L };
      yield return new object[] { UInt24.Zero,   3L };
      yield return new object[] { UInt48.Zero,   6L };
      yield return new object[] { FourCC.Empty,  4L };
    }

    [TestCaseSource(nameof(YieldTestCases_Write))]
    public void TestWrite(object value, long expectedPosition)
    {
      using var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream());

      writer.BaseStream.Seek(0L, SeekOrigin.Begin);

      Assert.That(writer.BaseStream.Position, Is.EqualTo(0L));

      try {
        typeof(Smdn.IO.Binary.BinaryWriter)
          .GetTypeInfo()
          .GetMethod(
            "Write",
            new[] { value.GetType() },
            null
          )
          !.Invoke(writer, new[] { value });
      }
      catch (MissingMethodException) {
        Assert.Fail($"invocation failed: type = {value.GetType().FullName}");
      }

      Assert.That(writer.BaseStream.Position, Is.EqualTo(expectedPosition));
    }

    [TestCaseSource(nameof(YieldTestCases_Write))]
    public void TestWriteToClosedWriter(object value, long discard)
    {
      using var writer = new Smdn.IO.Binary.BinaryWriter(new MemoryStream());

      writer.Close();

      var ex = Assert.Throws<TargetInvocationException>(() => {
        typeof(Smdn.IO.Binary.BinaryWriter)
          .GetTypeInfo()
          .GetMethod(
            "Write",
            new[] { value.GetType() },
            null
          )
          !.Invoke(writer, new[] { value });
      });

      Assert.That(ex!.InnerException, Is.InstanceOf<ObjectDisposedException>());
    }
  }
}
