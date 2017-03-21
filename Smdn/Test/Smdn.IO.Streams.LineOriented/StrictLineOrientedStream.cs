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
    public void TestReadAndReadLine()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Octets.CR, Octets.LF, 0x44, 0x45};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);
      var buffer = new byte[8];

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(5, stream.Read(buffer, 0, 5));

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 5), ArrayExtensions.Slice(buffer, 0, 5));
      Assert.AreEqual(5L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 5, 3), stream.ReadLine());
      Assert.AreEqual(8L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(8L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineCRLF()
    {
      var data = new byte[] {0x40, Octets.CR, 0x42, Octets.LF, 0x44, Octets.LF, Octets.CR, 0x47, Octets.CR, Octets.LF, 0x50};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");
      
      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 10), stream.ReadLine());
      Assert.AreEqual(10L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine());
      Assert.AreEqual(11L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(11L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineDiscardEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Octets.CR, Octets.LF, 0x44, 0x45};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 4), stream.ReadLine(false));
      Assert.AreEqual(6L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine(false));
      Assert.AreEqual(8L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(8L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineKeepEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Octets.CR, Octets.LF, 0x44, 0x45};
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 6), stream.ReadLine());
      Assert.AreEqual(6L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine());
      Assert.AreEqual(8L, stream.Position, "Position");

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

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 10), stream.ReadLine(false));
      Assert.AreEqual(12L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 12), stream.ReadLine(false));
      Assert.AreEqual(22L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(22L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineLongerThanBufferKeepEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, Octets.CR, Octets.LF,
        0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 12), stream.ReadLine());
      Assert.AreEqual(12L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 12), stream.ReadLine());
      Assert.AreEqual(22L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(22L, stream.Position, "Position");
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

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0,  7), stream.ReadLine(false)); // CRLF
      Assert.AreEqual(9L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 9, 23), stream.ReadLine(false));
      Assert.AreEqual(32L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine(false)); // EOS
      Assert.AreEqual(32L, stream.Position, "Position");
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

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0,  9), stream.ReadLine(true)); // CRLF
      Assert.AreEqual(9L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 9, 23), stream.ReadLine(true));
      Assert.AreEqual(32L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine(true)); // EOS
      Assert.AreEqual(32L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineIncompleteEOLDiscardEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, Octets.CR,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, Octets.CR}, stream.ReadLine(false));
      Assert.AreEqual(4L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineIncompleteEOLKeepEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, Octets.CR,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, Octets.CR}, stream.ReadLine(true));

      Assert.AreEqual(4L, stream.Position, "Position");
    }
  }
}
