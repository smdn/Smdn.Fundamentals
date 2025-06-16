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
        Assert.That(reader.BaseStream, Is.Not.Null);
        Assert.That(reader.LeaveBaseStreamOpen, Is.False);

        var baseStream = reader.BaseStream;

        Assert.That(baseStream, Is.Not.Null);

        if (close) {
          reader.Close();
        }
        else {
          reader.Dispose();
        }

        Assert.Throws<ObjectDisposedException>(() => Assert.That(reader.BaseStream, Is.Null));

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
          Assert.That(reader.BaseStream, Is.Not.Null);
          Assert.That(reader.LeaveBaseStreamOpen, Is.False);
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
        Assert.That(reader.BaseStream, Is.Not.Null);
        Assert.That(reader.LeaveBaseStreamOpen, Is.True);

        var baseStream = reader.BaseStream;

        Assert.That(baseStream, Is.Not.Null);

        reader.Close();

        Assert.Throws<ObjectDisposedException>(() => Assert.That(reader.BaseStream, Is.Null));

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
        Assert.That(reader.EndOfStream, Is.False);

        Assert.That(reader.ReadToEnd(), Is.EqualTo(actual));
        Assert.That(reader.EndOfStream, Is.True);
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        reader.ReadInt16();

        Assert.That(reader.EndOfStream, Is.False);
        Assert.That(reader.ReadToEnd(), Is.EqualTo(actual.Skip(2).ToArray()));
        Assert.That(reader.EndOfStream, Is.True);
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        reader.ReadToEnd();
        Assert.That(reader.EndOfStream, Is.True);

        Assert.That(reader.ReadToEnd(), Is.EqualTo(new byte[] {}));
        Assert.That(reader.EndOfStream, Is.True);
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
        Assert.That(decompressStream.CanSeek, Is.False);

        var reader = new Smdn.IO.Binary.BinaryReader(decompressStream);

        Assert.That(reader.ReadToEnd(), Is.EqualTo(actual));
      }
    }

    [Test]
    public void TestReadBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.That(reader.ReadBytes((int)reader.BaseStream.Length), Is.EqualTo(actual));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.That(reader.ReadBytes(reader.BaseStream.Length), Is.EqualTo(actual));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.That(reader.ReadBytes(3L), Is.EqualTo(actual.Skip(0).Take(3).ToArray()));
        Assert.That(reader.ReadBytes(3L), Is.EqualTo(actual.Skip(3).Take(3).ToArray()));
        Assert.That(reader.ReadBytes(3L), Is.EqualTo(actual.Skip(6).Take(2).ToArray()));
        Assert.That(reader.ReadBytes(3L), Is.EqualTo(new byte[] {}));
      }
    }

    [Test]
    public void TestReadExactBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.That(reader.ReadExactBytes((int)reader.BaseStream.Length), Is.EqualTo(actual));

        Assert.Throws<EndOfStreamException>(() => reader.ReadExactBytes((int)1));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.That(reader.ReadExactBytes(reader.BaseStream.Length), Is.EqualTo(actual));

        Assert.Throws<EndOfStreamException>(() => reader.ReadExactBytes(1L));
      }

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(actual))) {
        Assert.That(reader.ReadExactBytes(3L), Is.EqualTo(actual.Skip(0).Take(3).ToArray()));
        Assert.That(reader.ReadExactBytes(3L), Is.EqualTo(actual.Skip(3).Take(3).ToArray()));

        Assert.Throws<EndOfStreamException>(() => reader.ReadExactBytes(3L));
      }
    }

    [Test]
    public void TestReadBytesZeroBytes()
    {
      var zero = new byte[0];

      using (var reader = new Smdn.IO.Binary.BinaryReader(new MemoryStream(new byte[] {0xff}))) {
        Assert.That(reader.BaseStream.Position,Is.Zero);

        Assert.That(reader.ReadBytes(0), Is.EqualTo(zero).AsCollection);

        Assert.That(reader.BaseStream.Position,Is.Zero);

        Assert.That(reader.ReadBytes(0L), Is.EqualTo(zero).AsCollection);

        Assert.That(reader.BaseStream.Position,Is.Zero);

        reader.Close();

        try {
          Assert.That(reader.ReadBytes(0), Is.EqualTo(zero).AsCollection);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }

        try {
          Assert.That(reader.ReadBytes(0L), Is.EqualTo(zero).AsCollection);
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
        Assert.That(reader.BaseStream.Position,Is.Zero);

        Assert.That(reader.ReadExactBytes(0), Is.EqualTo(zero).AsCollection);

        Assert.That(reader.BaseStream.Position,Is.Zero);

        Assert.That(reader.ReadExactBytes(0L), Is.EqualTo(zero).AsCollection);

        Assert.That(reader.BaseStream.Position,Is.Zero);

        reader.Close();

        try {
          Assert.That(reader.ReadExactBytes(0), Is.EqualTo(zero).AsCollection);
        }
        catch (ObjectDisposedException) {
          Assert.Fail("ObjectDisposedException thrown");
        }

        try {
          Assert.That(reader.ReadExactBytes(0L), Is.EqualTo(zero).AsCollection);
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
        Assert.That(
          reader.ReadInt32(),
          Is.EqualTo(
            BitConverter.IsLittleEndian
              ? 0x44332211
              : 0x11223344
          )
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

      Assert.That(reader.EndOfStream, Is.False);
      Assert.That(reader.BaseStream.Position,Is.Zero);

      var ret = typeof(Smdn.IO.Binary.BinaryReader)
        .GetTypeInfo()
        .GetDeclaredMethod(methodName)
        !.Invoke(reader, null);

      Assert.That(ret, Is.Not.Zero, "read value must be non-zero value");
      Assert.That(reader.BaseStream.Position, Is.EqualTo((long)expectedCount));
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

      Assert.That(ex!.InnerException, Is.InstanceOf<ObjectDisposedException>());
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
        Assert.That(reader.EndOfStream, Is.False, $"EndOfStream before read: {methodName}");
      else
        Assert.That(reader.EndOfStream, Is.True, $"EndOfStream before read: {methodName}");

      Assert.Throws<TargetInvocationException>(() => {
        typeof(Smdn.IO.Binary.BinaryReader)
          .GetTypeInfo()
          .GetDeclaredMethod(methodName)
          !.Invoke(reader, null);
      });

      Assert.That(reader.BaseStream.Length, Is.EqualTo(reader.BaseStream.Position), $"Stream.Position: {methodName}");

      Assert.That(reader.EndOfStream, Is.True, $"EndOfStream after read: {methodName}");
    }
  }
}
