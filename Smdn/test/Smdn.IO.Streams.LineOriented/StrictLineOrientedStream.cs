using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

using Smdn.Text;

namespace Smdn.IO.Streams.LineOriented {
  [TestFixture]
  public class StrictLineOrientedStreamTests {
    [Test]
    public void TestNewLine()
    {
      using (var stream = new StrictLineOrientedStream(new MemoryStream(new byte[0]), 8)) {
        Assert.IsFalse(stream.NewLine.IsEmpty);
        CollectionAssert.AreEqual(new byte[] {0x0d, 0x0a}, stream.NewLine.ToArray(), "must return CRLF");
      }
    }

    [Test]
    public void TestIsStrictNewLine()
    {
      using (var stream = new LooseLineOrientedStream(new MemoryStream(new byte[0]), 8)) {
        Assert.IsFalse(stream.IsStrictNewLine);
      }
    }

    [Test]
    public async Task TestReadLineAsync()
    {
      var data = new byte[] {
        0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF,
        Ascii.Octets.CR, Ascii.Octets.LF,
        0x42, Ascii.Octets.CR, 0x43, Ascii.Octets.LF,
      };

      using (var stream = new StrictLineOrientedStream(new MemoryStream(data), 8)) {
        // CRLF
        var ret = await stream.ReadLineAsync();

        Assert.IsFalse(ret.IsEndOfStream);
        Assert.IsFalse(ret.IsEmptyLine);
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.LineWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41 },
                                  ret.Line.ToArray());

        // CRLF (empty line)
        ret = await stream.ReadLineAsync();

        Assert.IsFalse(ret.IsEndOfStream);
        Assert.IsTrue(ret.IsEmptyLine);
        CollectionAssert.AreEqual(new byte[] { Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.LineWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[0],
                                  ret.Line.ToArray());

        // <EOS>
        ret = await stream.ReadLineAsync();

        Assert.IsFalse(ret.IsEndOfStream);
        Assert.IsFalse(ret.IsEmptyLine);
        CollectionAssert.AreEqual(new byte[] { 0x42, Ascii.Octets.CR, 0x43, Ascii.Octets.LF, },
                                  ret.LineWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x42, Ascii.Octets.CR, 0x43, Ascii.Octets.LF, },
                                  ret.Line.ToArray());
      }
    }

    [Test]
    public void TestReadAndReadLine()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
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
      var data = new byte[] {0x40, Ascii.Octets.CR, 0x42, Ascii.Octets.LF, 0x44, Ascii.Octets.LF, Ascii.Octets.CR, 0x47, Ascii.Octets.CR, Ascii.Octets.LF, 0x50};
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
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
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
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
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
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, Ascii.Octets.CR, Ascii.Octets.LF,
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
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, Ascii.Octets.CR, Ascii.Octets.LF,
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
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Ascii.Octets.CR,
        Ascii.Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, Ascii.Octets.CR,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, Ascii.Octets.LF,
        Ascii.Octets.CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
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
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Ascii.Octets.CR,
        Ascii.Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, Ascii.Octets.CR,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, Ascii.Octets.LF,
        Ascii.Octets.CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
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
        0x40, 0x41, 0x42, Ascii.Octets.CR,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, Ascii.Octets.CR}, stream.ReadLine(false));
      Assert.AreEqual(4L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineIncompleteEOLKeepEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, Ascii.Octets.CR,
      };
      var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, Ascii.Octets.CR}, stream.ReadLine(true));

      Assert.AreEqual(4L, stream.Position, "Position");
    }
  }
}
