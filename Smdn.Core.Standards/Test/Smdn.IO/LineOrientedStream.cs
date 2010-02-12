using System;
using System.IO;
using NUnit.Framework;

using Smdn.Formats;

namespace Smdn.IO {
  [TestFixture]
  public class LineOrientedStreamTests {
    [Test]
    public void TestReadByte()
    {
      var data = new byte[] {0x00, 0x01, 0x02, 0x03, Octets.CR, Octets.LF, 0x04, 0x05};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);
      var index = 0;

      for (;;) {
        var val = stream.ReadByte();

        if (index == data.Length)
          Assert.AreEqual(-1, val);
        else
          Assert.AreEqual(data[index++], val);

        if (val == -1)
          break;
      }
    }

    [Test]
    public void TestReadAndReadLine()
    {
      var data = new byte[] {0x00, 0x01, 0x02, 0x03, Octets.CR, Octets.LF, 0x04, 0x05};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);
      var buffer = new byte[8];

      stream.Read(buffer, 0, 5);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 5), ArrayExtensions.Slice(buffer, 0, 5));
      Assert.AreEqual(ArrayExtensions.Slice(data, 5, 3), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadToStreamBufferEmpty()
    {
      var data = new byte[] {0x00, 0x01, Octets.CR, Octets.LF, 0x02, 0x03, 0x04, Octets.CR, Octets.LF, 0x05, 0x06, 0x07};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      var copyStream = new MemoryStream();

      stream.Read(copyStream, 12L);

      copyStream.Close();

      Assert.AreEqual(data, copyStream.ToArray());
    }

    [Test]
    public void TestReadToStreamLessThanBuffered()
    {
      var data = new byte[] {0x00, 0x01, Octets.CR, Octets.LF, 0x02, 0x03, 0x04, Octets.CR, Octets.LF, 0x05, 0x06, 0x07};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 16);

      var line = stream.ReadLine(true);

      Assert.AreEqual(data.Slice(0, 4), line);

      var copyStream = new MemoryStream();

      stream.Read(copyStream, 4L);

      copyStream.Close();

      Assert.AreEqual(data.Slice(4, 4), copyStream.ToArray());
    }

    [Test]
    public void TestReadToStreamLongerThanBuffered()
    {
      var data = new byte[] {0x00, 0x01, Octets.CR, Octets.LF, 0x02, 0x03, 0x04, Octets.CR, Octets.LF, 0x05, 0x06, 0x07};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      var line = stream.ReadLine(true);

      Assert.AreEqual(data.Slice(0, 4), line);

      var copyStream = new MemoryStream();

      stream.Read(copyStream, 8L);

      copyStream.Close();

      Assert.AreEqual(data.Slice(4, 8), copyStream.ToArray());
    }
  }
}
