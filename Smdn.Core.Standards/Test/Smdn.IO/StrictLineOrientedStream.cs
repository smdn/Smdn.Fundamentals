using System;
using System.IO;
using NUnit.Framework;

using Smdn.Formats;

namespace Smdn.IO {
  [TestFixture]
  public class StrictLineOrientedStreamTests {
    [Test]
    public void TestNewLine()
    {
      using (var stream = new StrictLineOrientedStream(new MemoryStream(new byte[0]), 8)) {
        var newLine = stream.NewLine;

        Assert.IsNotNull(newLine);
        Assert.AreEqual(new byte[] {0x0d, 0x0a}, newLine, "must return CRLF");

        Assert.AreNotSame(newLine, stream.NewLine, "must be different instance");
      }
    }

    [Test]
    public void TestReadLineCRLF()
    {
      var data = new byte[] {0x40, Octets.CR, 0x42, Octets.LF, 0x44, Octets.LF, Octets.CR, 0x47, Octets.CR, Octets.LF, 0x50};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 10), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineDiscardEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Octets.CR, Octets.LF, 0x44, 0x45};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 4), stream.ReadLine(false));
      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineKeepEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Octets.CR, Octets.LF, 0x44, 0x45};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 6), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineLongerThanBufferDiscardEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, Octets.CR, Octets.LF,
        0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
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
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, Octets.CR, Octets.LF,
        0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 12), stream.ReadLine());
      Assert.AreEqual(ArrayExtensions.Slice(data, 12), stream.ReadLine());
      Assert.IsNull(stream.ReadLine());
    }

    [Test]
    public void TestReadLineEOLSplittedBetweenBufferDiscardEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Octets.CR,
        Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, Octets.CR,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, Octets.LF,
        Octets.CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0,  7), stream.ReadLine(false)); // CRLF
      Assert.AreEqual(ArrayExtensions.Slice(data, 9, 23), stream.ReadLine(false));
      Assert.IsNull(stream.ReadLine(false)); // EOS
    }

    [Test]
    public void TestReadLineEOLSplittedBetweenBufferKeepEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Octets.CR,
        Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, Octets.CR,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, Octets.LF,
        Octets.CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(ArrayExtensions.Slice(data, 0,  9), stream.ReadLine(true)); // CRLF
      Assert.AreEqual(ArrayExtensions.Slice(data, 9, 23), stream.ReadLine(true));
      Assert.IsNull(stream.ReadLine(true)); // EOS
    }

    [Test]
    public void TestReadLineIncompleteEOLDiscardEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, Octets.CR,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, Octets.CR}, stream.ReadLine(false));
    }

    [Test]
    public void TestReadLineIncompleteEOLKeepEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, Octets.CR,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, Octets.CR}, stream.ReadLine(true));
    }
  }
}
