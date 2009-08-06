using System;
using System.IO;
using NUnit.Framework;
namespace Smdn.IO {
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

      try {
        inner.ReadByte();
      }
      catch (ObjectDisposedException) {
        Assert.Fail("ObjectDisposedException thrown");
      }
    }

    [Test]
    public void TestLeaveInnerStreamClose()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});

      using (var stream = new PartialStream(inner, 2, 4, false)) {
        Assert.AreEqual(2, inner.Position);
        Assert.IsFalse(stream.LeaveInnerStreamOpen);
      }

      try {
        inner.ReadByte();
        Assert.Fail("ObjectDisposedException not thrown");
      }
      catch (ObjectDisposedException) {
      }
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

      try {
        stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 0, 4);
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }

      try {
        stream.WriteByte(0x00);
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }

      try {
        stream.Flush();
        Assert.Fail("InvalidOperationException not thrown");
      }
      catch (InvalidOperationException) {
      }
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

      Assert.IsInstanceOfType(typeof(PartialStream), nestInner.InnerStream);

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

      Assert.IsNotInstanceOfType(typeof(PartialStream), nestInner.InnerStream);

      var buffer = new byte[inner.Length];

      nestInner.Position = 0;
      Assert.AreEqual(4, nestInner.Read(buffer, 0, buffer.Length));
      Assert.AreEqual(new byte[] {0x02, 0x03, 0x04, 0x05, 0x00, 0x00, 0x00, 0x00}, buffer);

      nestOuter.Position = 0;
      Assert.AreEqual(6, nestOuter.Read(buffer, 0, buffer.Length));
      Assert.AreEqual(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x00, 0x00}, buffer);
    }

    [Test]
    public void TestReadLengthSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, 2, 4);

      Assert.AreEqual(8, stream.InnerStream.Length);
      Assert.AreEqual(2, stream.InnerStream.Position);

      Assert.AreEqual(4, stream.Length);
      Assert.AreEqual(0, stream.Position);

      var buffer = new byte[2];

      Assert.AreEqual(2, stream.Read(buffer, 0, 2));
      Assert.AreEqual(new byte[] {0x02, 0x03}, buffer);

      Assert.AreEqual(4, stream.InnerStream.Position);
      Assert.AreEqual(2, stream.Position);

      Assert.AreEqual(0x04, stream.ReadByte());

      Assert.AreEqual(1, stream.Read(buffer, 0, 2));
      Assert.AreEqual(new byte[] {0x05, 0x03}, buffer);

      Assert.AreEqual(6, stream.InnerStream.Position);
      Assert.AreEqual(4, stream.Position);

      Assert.AreEqual(0, stream.Read(buffer, 0, 3));
      Assert.AreEqual(-1, stream.ReadByte());
    }

    [Test]
    public void TestReadLengthNotSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var stream = new PartialStream(inner, 2);

      Assert.AreEqual(8, stream.InnerStream.Length);
      Assert.AreEqual(2, stream.InnerStream.Position);

      Assert.AreEqual(6, stream.Length);
      Assert.AreEqual(0, stream.Position);

      var buffer = new byte[3];

      Assert.AreEqual(3, stream.Read(buffer, 0, 3));
      Assert.AreEqual(new byte[] {0x02, 0x03, 0x04}, buffer);

      Assert.AreEqual(5, stream.InnerStream.Position);
      Assert.AreEqual(3, stream.Position);

      Assert.AreEqual(0x05, stream.ReadByte());

      Assert.AreEqual(6, stream.InnerStream.Position);
      Assert.AreEqual(4, stream.Position);

      Assert.AreEqual(2, stream.Read(buffer, 0, 3));
      Assert.AreEqual(new byte[] {0x06, 0x07, 0x04}, buffer);

      Assert.AreEqual(8, stream.InnerStream.Position);
      Assert.AreEqual(6, stream.Position);

      Assert.AreEqual(0, stream.Read(buffer, 0, 3));
      Assert.AreEqual(-1, stream.ReadByte());
    }

    [Test]
    public void TestWriteLengthSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});

      using (var stream = new PartialStream(inner, 2, 4, false)) {
        Assert.AreEqual(8, stream.InnerStream.Length);
        Assert.AreEqual(2, stream.InnerStream.Position);

        Assert.AreEqual(4, stream.Length);
        Assert.AreEqual(0, stream.Position);

        stream.Write(new byte[] {0x02, 0x03}, 0, 2);
        stream.WriteByte(0x04);

        Assert.AreEqual(3, stream.Position);

        try {
          stream.Write(new byte[] {0x05, 0x06}, 0, 2);
          Assert.Fail("IOException not thrown");
        }
        catch (IOException) {
        }

        Assert.AreEqual(3, stream.Position);

        stream.Write(new byte[] {0x05}, 0, 1);

        Assert.AreEqual(4, stream.Position);

        try {
          stream.WriteByte(0x06);
          Assert.Fail("IOException not thrown");
        }
        catch (IOException) {
        }

        Assert.AreEqual(4, stream.Position);
      }

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x02, 0x03, 0x04, 0x05, 0x00, 0x00}, inner.ToArray());
    }

    [Test]
    public void TestWriteLengthNotSpecified()
    {
      var inner = new MemoryStream(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});

      using (var stream = new PartialStream(inner, 2, false)) {
        Assert.AreEqual(8, stream.InnerStream.Length);
        Assert.AreEqual(2, stream.InnerStream.Position);

        Assert.AreEqual(6, stream.Length);
        Assert.AreEqual(0, stream.Position);

        stream.Write(new byte[] {0x02, 0x03, 0x04}, 0, 3);
        stream.WriteByte(0x05);

        Assert.AreEqual(4, stream.Position);

        try {
          stream.Write(new byte[] {0x06, 0x07, 0x08}, 0, 3);
          Assert.Fail("NotSupportedException not thrown");
        }
        catch (NotSupportedException) {
          // cannot expand MemoryStream
        }

        Assert.AreEqual(4, stream.Position);

        stream.Write(new byte[] {0x06, 0x07}, 0, 2);

        Assert.AreEqual(6, stream.Position);

        try {
          stream.WriteByte(0x08);
          Assert.Fail("NotSupportedException not thrown");
        }
        catch (NotSupportedException) {
          // cannot expand MemoryStream
        }

        Assert.AreEqual(6, stream.Position);
      }

      Assert.AreEqual(new byte[] {0x00, 0x00, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, inner.ToArray());
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
      try {
        stream.Seek(-1, SeekOrigin.Begin);
        Assert.Fail("IOException not thrown");
      }
      catch (IOException) {
      }

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
      try {
        stream.Seek(-12, SeekOrigin.Current);
        Assert.Fail("IOException not thrown");
      }
      catch (IOException) {
      }

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
      try {
        stream.Position = -2;
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }

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
      try {
        stream.Seek(-5, SeekOrigin.End);
        Assert.Fail("IOException not thrown");
      }
      catch (IOException) {
      }

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
      try {
        stream.Seek(-5, SeekOrigin.End);
        Assert.Fail("IOException not thrown");
      }
      catch (IOException) {
      }

      Assert.AreEqual(8L, stream.Position);
      Assert.AreEqual(12L, stream.InnerStream.Position);
    }
  }
}
