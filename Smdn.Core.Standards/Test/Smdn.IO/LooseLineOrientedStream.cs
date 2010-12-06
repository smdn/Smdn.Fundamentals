using System;
using System.IO;
using NUnit.Framework;

using Smdn.Formats;

namespace Smdn.IO {
  [TestFixture]
  public class LooseLineOrientedStreamTests {
    [Test]
    public void TestReadLineKeepEOL()
    {
      var data = new byte[] {0x40, Octets.CR, 0x42, Octets.LF, 0x44, Octets.LF, Octets.CR, 0x47, Octets.CR, Octets.LF, 0x50};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 2), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 2, 2), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 4, 2), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 1), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 7, 3), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineDiscardEOL()
    {
      var data = new byte[] {0x40, Octets.CR, 0x42, Octets.LF, 0x44, Octets.LF, Octets.CR, 0x47, Octets.CR, Octets.LF, 0x50};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 1), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 2, 1), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 4, 1), stream.ReadLine(false));
      Assert.AreEqual(new byte[] {}, stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 7, 1), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineBufferEndsWithEOLKeepEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Octets.CR};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(data, stream.ReadLine(true));
    }

    [Test]
    public void TestReadLineBufferEndsWithEOLDiscardEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Octets.CR};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 7), stream.ReadLine(false));
    }

    [Test]
    public void TestReadLineEOLSplittedBetweenBufferDiscardEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Octets.CR,
        Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66,
      };
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 7), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 9), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineEOLSplittedBetweenBufferKeepEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Octets.CR,
        Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66,
      };
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 9), stream.ReadLine(true));
      Assert.AreEqual(ArrayExtensions.Slice(data, 9), stream.ReadLine(true));
      Assert.IsNull(stream.ReadLine());
    }
  }
}