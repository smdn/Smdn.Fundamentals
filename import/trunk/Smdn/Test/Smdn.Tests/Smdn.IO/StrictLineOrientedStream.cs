using System;
using System.IO;
using NUnit.Framework;

using Smdn.Extensions;
using Smdn.Text;

namespace Smdn.IO {
  [TestFixture]
  public class StrictLineOrientedStreamTests {
    [Test]
    public void TestReadLineCRLF()
    {
      var data = new byte[] {0x00, Octets.CR, 0x02, Octets.LF, 0x04, Octets.LF, Octets.CR, 0x07, Octets.CR, Octets.LF, 0x10};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 10), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineDiscardEOL()
    {
      var data = new byte[] {0x00, 0x01, 0x02, 0x03, Octets.CR, Octets.LF, 0x04, 0x05};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 4), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineKeepEOL()
    {
      var data = new byte[] {0x00, 0x01, 0x02, 0x03, Octets.CR, Octets.LF, 0x04, 0x05};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 6), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineLongerThanBufferDiscardEOL()
    {
      var data = new byte[] {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, Octets.CR, Octets.LF,
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 10), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 12), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineLongerThanBufferKeepEOL()
    {
      var data = new byte[] {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, Octets.CR, Octets.LF,
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 12), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 12), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

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
  }
}
