using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class ExtendStreamTests {
    [Test]
    public void TestConstruct()
    {
      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, (byte[])null, (byte[])null)).Length);
      }
      catch {
        Assert.Fail("construct null/null");
      }

      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, new byte[] {}, null)).Length);
      }
      catch {
        Assert.Fail("construct byte[]/null");
      }

      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, null, new byte[] {})).Length);
      }
      catch {
        Assert.Fail("construct null/byte[]");
      }

      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, new byte[] {}, new byte[] {})).Length);
      }
      catch {
        Assert.Fail("construct byte[]/byte[]");
      }

      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, Stream.Null, null)).Length);
      }
      catch {
        Assert.Fail("construct Stream/null");
      }

      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, null, Stream.Null)).Length);
      }
      catch {
        Assert.Fail("construct null/Stream");
      }

      try {
        Assert.AreEqual(0L, (new ExtendStream(Stream.Null, Stream.Null, Stream.Null)).Length);
      }
      catch {
        Assert.Fail("construct Stream/Stream");
      }
    }

    [Test]
    public void TestLength()
    {
      using (var innerStream = new MemoryStream(new byte[] {0x00, 0x00})) {
        Assert.AreEqual(2L, (new ExtendStream(innerStream, new byte[] {}, new byte[] {}, true)).Length);
        Assert.AreEqual(4L, (new ExtendStream(innerStream, new byte[] {0x00, 0x00}, new byte[] {}, true)).Length);
        Assert.AreEqual(4L, (new ExtendStream(innerStream, new byte[] {}, new byte[] {0x00, 0x00}, true)).Length);
        Assert.AreEqual(6L, (new ExtendStream(innerStream, new byte[] {0x00, 0x00}, new byte[] {0x00, 0x00}, true)).Length);
      }
    }

    [Test]
    public void TestWritingOperation()
    {
      using (var innerStream = new MemoryStream(new byte[] {0x00, 0x00})) {
        using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x00}, new byte[] {0x00, 0x00})) {
          Assert.IsFalse(extended.CanWrite);

          try {
            extended.SetLength(10L);
            Assert.Fail("no exception thrown with SetLength");
          }
          catch (NotSupportedException) {
          }

          try {
            extended.WriteByte(0xff);
            Assert.Fail("no exception thrown with WriteByte");
          }
          catch (NotSupportedException) {
          }

          try {
            extended.Write(new byte[] {0xff, 0xff}, 0, 2);
            Assert.Fail("no exception thrown with Write");
          }
          catch (NotSupportedException) {
          }

          try {
            extended.Flush();
            Assert.Fail("no exception thrown with Flush");
          }
          catch (NotSupportedException) {
          }
        }
      }
    }

    [Test]
    public void TestReadExtendedStream()
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 13; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] {0x04, 0x05, 0x06, 0x07})) {
          using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x01, 0x02, 0x03}, new byte[] {0x08, 0x09, 0x0a, 0x0b})) {
            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test]
    public void TestReadPrependedStream()
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 9; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] {0x04, 0x05, 0x06, 0x07})) {
          using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x01, 0x02, 0x03}, null)) {
            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test]
    public void TestReadAppendedStream()
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 9; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03})) {
          using (var extended = new ExtendStream(innerStream, null, new byte[] {0x04, 0x05, 0x06, 0x07})) {
            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test]
    public void TestReadNonExtendedStream()
    {
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0xff, 0xff, 0xff, 0xff};

      for (var len = 0; len < 5; len++) {
        var buffer = new byte[len];

        using (var innerStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03})) {
          using (var extended = new ExtendStream(innerStream, (Stream)null, (Stream)null)) {
            if (extended.Length < len) {
              Assert.AreEqual(extended.Length, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, (int)extended.Length), buffer, "read content {0}", len);
              Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
              Assert.AreEqual(-1, extended.ReadByte());
            }
            else {
              Assert.AreEqual(len, extended.Read(buffer, 0, len), "read length {0}", len);
              Assert.AreEqual(ArrayExtensions.Slice(expected, 0, len), buffer, "read content {0}", len);
              Assert.AreEqual(len, extended.Position, "position {0}", len);
            }
          }
        }
      }
    }

    [Test]
    public void TestReadByte()
    {
      using (var innerStream = new MemoryStream(new byte[] {0x04, 0x05, 0x06, 0x07})) {
        using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x01, 0x02, 0x03}, new byte[] {0x08, 0x09, 0x0a, 0x0b})) {
          Assert.AreEqual(12L, extended.Length);
          Assert.AreEqual(0L, extended.Position);
          Assert.IsTrue(extended.CanRead);

          for (var expected = 0L; expected < 12L; expected++) {
            Assert.AreEqual(expected, extended.ReadByte(), "offset: {0}", expected);
            Assert.AreEqual(expected + 1, extended.Position, "position {0}", expected);
          }

          Assert.AreEqual(-1, extended.ReadByte(), "end of stream");
        }
      }
    }

    [Test]
    public void TestSetPosition()
    {
      using (var innerStream = new MemoryStream(new byte[] {0x04, 0x05, 0x06, 0x07})) {
        using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x01, 0x02, 0x03}, new byte[] {0x08, 0x09, 0x0a, 0x0b})) {
          try {
            extended.Position = -1L;
            Assert.Fail("ArgumentOutOfRangeException not thrown");
          }
          catch (ArgumentOutOfRangeException) {
          }

          for (var expected = 0L; expected < 12L; expected++) {
            extended.Position = expected;

            Assert.AreEqual(expected, extended.ReadByte());
          }

          extended.Position = 13; // no exception will be thrown
        }
      }
    }

    [Test]
    public void TestSeekBegin()
    {
      using (var innerStream = new MemoryStream(new byte[] {0x04, 0x05, 0x06, 0x07})) {
        using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x01, 0x02, 0x03}, new byte[] {0x08, 0x09, 0x0a, 0x0b})) {
          Assert.IsTrue(extended.CanSeek);

          foreach (var offset in new long[] {
            0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
          }) {
            try {
              Assert.AreEqual(offset, extended.Seek(offset, SeekOrigin.Begin));
            }
            catch (IOException) {
              Assert.Fail("IOException thrown while seeking to {0}", offset);
            }

            Assert.AreEqual(offset, extended.ReadByte());
          }

          extended.Position = 6L;

          try {
            extended.Seek(-1L, SeekOrigin.Begin);
            Assert.Fail("IOException not thrown");
          }
          catch (IOException) {
            Assert.AreEqual(6L, extended.Position);
          }

          Assert.AreEqual(12L, extended.Seek(12L, SeekOrigin.Begin));
          Assert.AreEqual(-1, extended.ReadByte());
        }
      }
    }

    [Test]
    public void TestSeekCurrent()
    {
      using (var innerStream = new MemoryStream(new byte[] {0x04, 0x05, 0x06, 0x07})) {
        using (var extended = new ExtendStream(innerStream, new byte[] {0x00, 0x01, 0x02, 0x03}, new byte[] {0x08, 0x09, 0x0a, 0x0b})) {
          Assert.IsTrue(extended.CanSeek);

          extended.Position = 6L;

          try {
            extended.Seek(-7, SeekOrigin.Current);
            Assert.Fail("IOException not thrown");
          }
          catch (IOException) {
            Assert.AreEqual(6L, extended.Position);
          }

          var pair = new long[][] {
            // offset / position
            new long[] { 0, 6},
            new long[] {-2, 5},
            new long[] { 1, 7},
            new long[] {-4, 4},
            new long[] { 3, 8},
            new long[] {-6, 3},
            new long[] { 5, 9},
            new long[] {-8, 2},
            new long[] { 7,10},
            new long[] {-10, 1},
            new long[] { 9,11},
          };

          for (var index = 0; index < pair.Length; index++) {
            try {
              Assert.AreEqual(pair[index][1], extended.Seek(pair[index][0], SeekOrigin.Current), "seeked position {0}", index);
            }
            catch (IOException) {
              Assert.Fail("IOException thrown while seeking ({0})", index);
            }

            Assert.AreEqual(pair[index][1], extended.ReadByte(), "read value {0}", index);
          }

          Assert.AreEqual(-1, extended.ReadByte());
          Assert.AreEqual(13, extended.Seek(1, SeekOrigin.Current));
          Assert.AreEqual(-1, extended.ReadByte());
        }
      }
    }
  }
}