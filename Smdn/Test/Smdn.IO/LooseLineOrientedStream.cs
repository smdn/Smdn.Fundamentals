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
      var data = new byte[] {0x00, Octets.CR, 0x02, Octets.LF, 0x04, Octets.LF, Octets.CR, 0x07, Octets.CR, Octets.LF, 0x00};
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
      var data = new byte[] {0x00, Octets.CR, 0x02, Octets.LF, 0x04, Octets.LF, Octets.CR, 0x07, Octets.CR, Octets.LF, 0x00};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 1), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 2, 1), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 4, 1), stream.ReadLine(false));
      Assert.AreEqual(new byte[] {}, stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 7, 1), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine());
    }
  }
}