using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture]
  public class BinaryReaderTest {
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
      try {
        using (var reader = new Smdn.IO.BinaryReader(new NonReadableStream())) {
          Assert.Fail("ArgumentException not thrown");
        }
      }
      catch (ArgumentException) {
      }
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
      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(new byte[0]))) {
        Assert.IsNotNull(reader.BaseStream);
        Assert.IsFalse(reader.LeaveBaseStreamOpen);

        var baseStream = reader.BaseStream;

        Assert.IsNotNull(baseStream);

        if (close)
          reader.Close();
        else
          (reader as IDisposable).Dispose();

        try {
          Assert.IsNull(reader.BaseStream);
          Assert.Fail("ObjectDisposedException not thrown by reader");
        }
        catch (ObjectDisposedException) {
        }

        try {
          baseStream.ReadByte();
          Assert.Fail("ObjectDisposedException not thrown by base stream");
        }
        catch (ObjectDisposedException) {
        }
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

        using (var reader = new Smdn.IO.BinaryReader(stream)) {
          Assert.IsNotNull(reader.BaseStream);
          Assert.IsFalse(reader.LeaveBaseStreamOpen);
        }

        try {
          stream.ReadByte();
          Assert.Fail("ObjectDisposedException not thrown by base stream");
        }
        catch (ObjectDisposedException) {
        }
      }
    }

    private class BinaryReaderEx : Smdn.IO.BinaryReader {
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

        try {
          Assert.IsNull(reader.BaseStream);
          Assert.Fail("ObjectDisposedException not thrown by reader");
        }
        catch (ObjectDisposedException) {
        }

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

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.IsFalse(reader.EndOfStream);

        Assert.AreEqual(actual, reader.ReadToEnd());
        Assert.IsTrue(reader.EndOfStream);
      }

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        reader.ReadInt16();

        Assert.IsFalse(reader.EndOfStream);
        Assert.AreEqual(actual.Slice(2), reader.ReadToEnd());
        Assert.IsTrue(reader.EndOfStream);
      }

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
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

        compressedMemoryStream.Close();

        actualCompressed = compressedMemoryStream.ToArray();
      }

      using (var decompressStream = new System.IO.Compression.DeflateStream(new MemoryStream(actualCompressed), System.IO.Compression.CompressionMode.Decompress)) {
        Assert.IsFalse(decompressStream.CanSeek);

        var reader = new Smdn.IO.BinaryReader(decompressStream);

        Assert.AreEqual(actual, reader.ReadToEnd());
      }
    }

    [Test]
    public void TestReadBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadBytes((int)reader.BaseStream.Length));
      }

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadBytes(reader.BaseStream.Length));
      }

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual.Slice(0, 3), reader.ReadBytes(3L));
        Assert.AreEqual(actual.Slice(3, 3), reader.ReadBytes(3L));
        Assert.AreEqual(actual.Slice(6, 2), reader.ReadBytes(3L));
        Assert.AreEqual(new byte[] {}, reader.ReadBytes(3L));
      }
    }

    [Test]
    public void TestReadExactBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadExactBytes((int)reader.BaseStream.Length));

        try {
          reader.ReadExactBytes((int)1);
          Assert.Fail("EndOfStreamException not thrown");
        }
        catch (EndOfStreamException) {
        }
      }

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadExactBytes(reader.BaseStream.Length));

        try {
          reader.ReadExactBytes(1L);
          Assert.Fail("EndOfStreamException not thrown");
        }
        catch (EndOfStreamException) {
        }
      }

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual.Slice(0, 3), reader.ReadExactBytes(3L));
        Assert.AreEqual(actual.Slice(3, 3), reader.ReadExactBytes(3L));

        try {
          reader.ReadExactBytes(3L);
          Assert.Fail("EndOfStreamException not thrown");
        }
        catch (EndOfStreamException) {
        }
      }
    }

    [Test]
    public void TestReadBytesZeroBytes()
    {
      var zero = new byte[0];

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(new byte[] {0xff}))) {
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

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(new byte[] {0xff}))) {
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
      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(new byte[] {0x11, 0x22, 0x33, 0x44}))) {
        switch (Platform.Endianness) {
          case Endianness.BigEndian:
            Assert.AreEqual(0x11223344, reader.ReadInt32());
            break;

          case Endianness.LittleEndian:
            Assert.AreEqual(0x44332211, reader.ReadInt32());
            break;

          default:
            Assert.Ignore("unsupported endian: {0}", Platform.Endianness);
            break;
        }
      }
    }

    [Test]
    public void TestRead()
    {
      var actual = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        var type = reader.GetType();

        foreach (var test in new[] {
          new {Method = "ReadByte",   Count = 1},
          new {Method = "ReadSByte",  Count = 1},
          new {Method = "ReadInt16",  Count = 2},
          new {Method = "ReadUInt16", Count = 2},
          new {Method = "ReadInt32",  Count = 4},
          new {Method = "ReadUInt32", Count = 4},
          new {Method = "ReadInt64",  Count = 8},
          new {Method = "ReadUInt64", Count = 8},
          new {Method = "ReadUInt24", Count = 3},
          new {Method = "ReadUInt48", Count = 6},
          new {Method = "ReadFourCC", Count = 4},
        }) {
          reader.BaseStream.Seek(0L, SeekOrigin.Begin);

          Assert.IsFalse(reader.EndOfStream);
          Assert.AreEqual(0L, reader.BaseStream.Position);

          var ret = type.InvokeMember(test.Method,
                                      BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.ExactBinding,
                                      null,
                                      reader,
                                      null);

          Assert.AreNotEqual(0, ret, "read value must be non-zero value");
          Assert.AreEqual((long)test.Count, reader.BaseStream.Position);
        }
      }
    }

    [Test]
    public void TestReadFromClosedReader()
    {
      var actual = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

      foreach (var test in new[] {
        new {Method = "ReadByte",   Count = 1},
        new {Method = "ReadSByte",  Count = 1},
        new {Method = "ReadInt16",  Count = 2},
        new {Method = "ReadUInt16", Count = 2},
        new {Method = "ReadInt32",  Count = 4},
        new {Method = "ReadUInt32", Count = 4},
        new {Method = "ReadInt64",  Count = 8},
        new {Method = "ReadUInt64", Count = 8},
        new {Method = "ReadUInt24", Count = 3},
        new {Method = "ReadUInt48", Count = 6},
        new {Method = "ReadFourCC", Count = 4},
      }) {
        using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
          reader.Close();

          try {
            reader.GetType().InvokeMember(test.Method,
                                          BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.ExactBinding,
                                          null,
                                          reader,
                                          null);

            Assert.Fail("ObjectDisposedException not thrown; method = {0}", test.Method);
          }
          catch (TargetInvocationException ex) {
            if (!(ex.InnerException is ObjectDisposedException))
              Assert.Fail("unexpected exception: {0}", ex);
          }
        }
      }
    }

    [Test]
    public void TestReadEndOfStreamException()
    {
      var actual = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

      using (var reader = new Smdn.IO.BinaryReader(new MemoryStream(actual))) {
        var type = reader.GetType();

        foreach (var test in new[] {
          new {Method = "ReadByte",   Count = 1},
          new {Method = "ReadSByte",  Count = 1},
          new {Method = "ReadInt16",  Count = 2},
          new {Method = "ReadUInt16", Count = 2},
          new {Method = "ReadInt32",  Count = 4},
          new {Method = "ReadUInt32", Count = 4},
          new {Method = "ReadInt64",  Count = 8},
          new {Method = "ReadUInt64", Count = 8},
          new {Method = "ReadUInt24", Count = 3},
          new {Method = "ReadUInt48", Count = 6},
          new {Method = "ReadFourCC", Count = 4},
        }) {
          reader.BaseStream.Seek(-(test.Count - 1), SeekOrigin.End);

          if (1 < test.Count)
            Assert.IsFalse(reader.EndOfStream, "EndOfStream before read: {0}", test.Method);
          else
            Assert.IsTrue(reader.EndOfStream, "EndOfStream before read: {0}", test.Method);

          try {
            type.InvokeMember(test.Method,
                              BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.ExactBinding,
                              null,
                              reader,
                              null);

            Assert.Fail("EndOfStreamException not thrown: {0}", test.Method);
          }
          catch (TargetInvocationException ex) {
            if (!(ex.InnerException is EndOfStreamException))
              Assert.Fail("unexpected exception: {0}", ex);
          }

          Assert.AreEqual(reader.BaseStream.Position, reader.BaseStream.Length, "Stream.Position: {0}", test.Method);

          Assert.IsTrue(reader.EndOfStream, "EndOfStream after read: {0}", test.Method);
        }
      }
    }
  }
}
