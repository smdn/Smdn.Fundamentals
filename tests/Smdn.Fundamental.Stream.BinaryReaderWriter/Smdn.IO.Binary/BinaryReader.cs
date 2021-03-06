// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture]
  public class BinaryReaderTests {
    private class NonReadableStream : Stream {
      public override bool CanRead {
        get { return false; }
      }

      public override bool CanSeek {
        get { return false; }
      }

      public override bool CanWrite {
        get { return true; }
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
    public void TestConstructWithNonReadableStream()
    {
      Assert.Throws<ArgumentException>(() => {
        using (var reader = new Smdn.IO.Binary.BinaryReader(new NonReadableStream())) {
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
      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(new byte[0]))) {
        Assert.IsNotNull(reader.BaseStream);
        Assert.IsFalse(reader.LeaveBaseStreamOpen);

        var baseStream = reader.BaseStream;

        Assert.IsNotNull(baseStream);

        if (close) {
          reader.Close();
        }
        else {
          (reader as IDisposable).Dispose();
        }

        Assert.Throws<ObjectDisposedException>(() => Assert.IsNull(reader.BaseStream));

        Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte());
      }
    }

    [Test]
    public void TestClose2()
    {
      using (var stream = new MemoryStream(new byte[0])) {
        try {
          stream.ReadByte();
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown by base stream");
        }

        using (var reader = new Smdn.IO.Binary.BinaryReader(stream)) {
          Assert.IsNotNull(reader.BaseStream);
          Assert.IsFalse(reader.LeaveBaseStreamOpen);
        }

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }
    }

    private class BinaryReaderEx : Smdn.IO.Binary.BinaryReader {
      public BinaryReaderEx(Stream stream)
        : base(stream, true)
      {
      }
    }

    [Test]
    public void TestCloseLeaveBaseStreamOpen()
    {
      using (var reader = new BinaryReaderEx(new MemoryStream(new byte[0]))) {
        Assert.IsNotNull(reader.BaseStream);
        Assert.IsTrue(reader.LeaveBaseStreamOpen);

        var baseStream = reader.BaseStream;

        Assert.IsNotNull(baseStream);

        reader.Close();

        Assert.Throws<ObjectDisposedException>(() => Assert.IsNull(reader.BaseStream));

        try {
          baseStream.ReadByte();
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown by base stream");
        }
      }
    }

    [Test]
    public void TestReadToEnd()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.IsFalse(reader.EndOfStream);

        Assert.AreEqual(actual, reader.ReadToEnd());
        Assert.IsTrue(reader.EndOfStream);
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        reader.ReadInt16();

        Assert.IsFalse(reader.EndOfStream);
        Assert.AreEqual(actual.Skip(2).ToArray(), reader.ReadToEnd());
        Assert.IsTrue(reader.EndOfStream);
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        reader.ReadToEnd();
        Assert.IsTrue(reader.EndOfStream);

        Assert.AreEqual(new byte[] {}, reader.ReadToEnd());
        Assert.IsTrue(reader.EndOfStream);
      }
    }

    [Test]
    public void TestReadToEndWithUnseekableStream()
    {
      byte[] actual = new byte[4096];
      byte[] actualCompressed;

      (new Random()).NextBytes(actual);

      using (var compressedMemoryStream = new MemoryStream()) {
        using (var compressStream = new System.IO.Compression.DeflateStream(compressedMemoryStream, System.IO.Compression.CompressionMode.Compress)) {
          compressStream.Write(actual, 0, actual.Length);
          compressStream.Flush();
        }

        compressedMemoryStream.Dispose();

        actualCompressed = compressedMemoryStream.ToArray();
      }

      using (var decompressStream = new System.IO.Compression.DeflateStream(new MemoryStream(actualCompressed), System.IO.Compression.CompressionMode.Decompress)) {
        Assert.IsFalse(decompressStream.CanSeek);

        var reader = new Smdn.IO.Binary.BinaryReader(decompressStream);

        Assert.AreEqual(actual, reader.ReadToEnd());
      }
    }

    [Test]
    public void TestReadBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadBytes((int)reader.BaseStream.Length));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadBytes(reader.BaseStream.Length));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual.Skip(0).Take(3).ToArray(), reader.ReadBytes(3L));
        Assert.AreEqual(actual.Skip(3).Take(3).ToArray(), reader.ReadBytes(3L));
        Assert.AreEqual(actual.Skip(6).Take(2).ToArray(), reader.ReadBytes(3L));
        Assert.AreEqual(new byte[] {}, reader.ReadBytes(3L));
      }
    }

    [Test]
    public void TestReadExactBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadExactBytes((int)reader.BaseStream.Length));

        Assert.Throws<EndOfStreamException>(() => reader.ReadExactBytes((int)1));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadExactBytes(reader.BaseStream.Length));

        Assert.Throws<EndOfStreamException>(() => reader.ReadExactBytes(1L));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual.Skip(0).Take(3).ToArray(), reader.ReadExactBytes(3L));
        Assert.AreEqual(actual.Skip(3).Take(3).ToArray(), reader.ReadExactBytes(3L));

        Assert.Throws<EndOfStreamException>(() => reader.ReadExactBytes(3L));
      }
    }

    [Test]
    public void TestReadBytesZeroBytes()
    {
      var zero = new byte[0];

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(new byte[] {0xff}))) {
        Assert.AreEqual(0L, reader.BaseStream.Position);

        CollectionAssert.AreEqual(zero, reader.ReadBytes(0));

        Assert.AreEqual(0L, reader.BaseStream.Position);

        CollectionAssert.AreEqual(zero, reader.ReadBytes(0L));

        Assert.AreEqual(0L, reader.BaseStream.Position);

        reader.Close();

        try {
          CollectionAssert.AreEqual(zero, reader.ReadBytes(0));
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }

        try {
          CollectionAssert.AreEqual(zero, reader.ReadBytes(0L));
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }
      }
    }

    [Test]
    public void TestReadExactBytesZeroBytes()
    {
      var zero = new byte[0];

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(new byte[] {0xff}))) {
        Assert.AreEqual(0L, reader.BaseStream.Position);

        CollectionAssert.AreEqual(zero, reader.ReadExactBytes(0));

        Assert.AreEqual(0L, reader.BaseStream.Position);

        CollectionAssert.AreEqual(zero, reader.ReadExactBytes(0L));

        Assert.AreEqual(0L, reader.BaseStream.Position);

        reader.Close();

        try {
          CollectionAssert.AreEqual(zero, reader.ReadExactBytes(0));
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }

        try {
          CollectionAssert.AreEqual(zero, reader.ReadExactBytes(0L));
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }
      }
    }

    [Test]
    public void TestReadInt32()
    {
      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(new byte[] {0x11, 0x22, 0x33, 0x44}))) {
        Assert.AreEqual(
          BitConverter.IsLittleEndian
            ? 0x44332211
            : 0x11223344,
          reader.ReadInt32()
        );
      }
    }

    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadByte), 1)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadSByte), 1)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt16), 2)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt16), 2)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt32), 4)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt32), 4)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt64), 8)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt64), 8)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt24), 3)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt48), 6)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadFourCC), 4)]
    public void TestRead(string methodName, int expectedCount)
    {
      var actual = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

      using var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual));

      reader.BaseStream.Seek(0L, SeekOrigin.Begin);

      Assert.IsFalse(reader.EndOfStream);
      Assert.AreEqual(0L, reader.BaseStream.Position);

      var ret = typeof(Smdn.IO.Binary.BinaryReader)
        .GetTypeInfo()
        .GetDeclaredMethod(methodName)
        !.Invoke(reader, null);

      Assert.AreNotEqual(0, ret, "read value must be non-zero value");
      Assert.AreEqual((long)expectedCount, reader.BaseStream.Position);
    }

    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadByte))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadSByte))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt16))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt16))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt32))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt32))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt64))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt64))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt24))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt48))]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadFourCC))]
    public void TestReadFromClosedReader(string methodName)
    {
      var actual = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

      using var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual));
      reader.Close();

      var ex = Assert.Throws<TargetInvocationException>(() => {
        typeof(Smdn.IO.Binary.BinaryReader)
          .GetTypeInfo()
          .GetDeclaredMethod(methodName)
          !.Invoke(reader, null);
      });

      Assert.IsInstanceOf<ObjectDisposedException>(ex!.InnerException);
    }

    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadByte), 1)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadSByte), 1)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt16), 2)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt16), 2)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt32), 4)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt32), 4)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadInt64), 8)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt64), 8)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt24), 3)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadUInt48), 6)]
    [TestCase(nameof(Smdn.IO.Binary.BinaryReader.ReadFourCC), 4)]
    public void TestReadEndOfStreamException(string methodName, int expectedCount)
    {
      var actual = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

      using var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual));

      reader.BaseStream.Seek(-(expectedCount - 1), SeekOrigin.End);

      if (1 < expectedCount)
        Assert.IsFalse(reader.EndOfStream, "EndOfStream before read: {0}", methodName);
      else
        Assert.IsTrue(reader.EndOfStream, "EndOfStream before read: {0}", methodName);

      Assert.Throws<TargetInvocationException>(() => {
        typeof(Smdn.IO.Binary.BinaryReader)
          .GetTypeInfo()
          .GetDeclaredMethod(methodName)
          !.Invoke(reader, null);
      });

      Assert.AreEqual(reader.BaseStream.Position, reader.BaseStream.Length, "Stream.Position: {0}", methodName);

      Assert.IsTrue(reader.EndOfStream, "EndOfStream after read: {0}", methodName);
    }
  }
}
