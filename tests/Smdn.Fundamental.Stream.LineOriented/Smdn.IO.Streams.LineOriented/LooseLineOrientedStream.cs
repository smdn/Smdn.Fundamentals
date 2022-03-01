// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

using Smdn.Text;

namespace Smdn.IO.Streams.LineOriented {
  [TestFixture]
  public class LooseLineOrientedStreamTests {
    [Test]
    public void TestNewLine()
    {
      using (var stream = new LooseLineOrientedStream(new MemoryStream(new byte[0]), 8)) {
        Assert.IsTrue(stream.NewLine.IsEmpty);
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
        0x42, 0x43, Ascii.Octets.CR,
        0x44, 0x45, Ascii.Octets.LF,
        Ascii.Octets.LF,
        0x46, 0x47
      };

      using (var stream = new LooseLineOrientedStream(new MemoryStream(data), 8)) {
        // CRLF
        var ret = await stream.ReadLineAsync();

        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Value.IsEmpty);
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x40, 0x41 },
                                  ret.Value.Sequence.ToArray());
        CollectionAssert.AreEqual(new byte[] { Ascii.Octets.CR, Ascii.Octets.LF },
                                  ret.Value.NewLine.ToArray());

        // CR
        ret = await stream.ReadLineAsync();

        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Value.IsEmpty);
        CollectionAssert.AreEqual(new byte[] { 0x42, 0x43, Ascii.Octets.CR },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x42, 0x43 },
                                  ret.Value.Sequence.ToArray());
        CollectionAssert.AreEqual(new byte[] { Ascii.Octets.CR },
                                  ret.Value.NewLine.ToArray());

        // LF
        ret = await stream.ReadLineAsync();

        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Value.IsEmpty);
        CollectionAssert.AreEqual(new byte[] { 0x44, 0x45, Ascii.Octets.LF },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x44, 0x45 },
                                  ret.Value.Sequence.ToArray());
        CollectionAssert.AreEqual(new byte[] { Ascii.Octets.LF },
                                  ret.Value.NewLine.ToArray());

        // LF (empty line)
        ret = await stream.ReadLineAsync();

        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Value.IsEmpty);
        CollectionAssert.AreEqual(new byte[] { Ascii.Octets.LF },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[0],
                                  ret.Value.Sequence.ToArray());
        CollectionAssert.AreEqual(new byte[] { Ascii.Octets.LF },
                                  ret.Value.NewLine.ToArray());

        // <EOS>
        ret = await stream.ReadLineAsync();

        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Value.IsEmpty);
        CollectionAssert.AreEqual(new byte[] { 0x46, 0x47 },
                                  ret.Value.SequenceWithNewLine.ToArray());
        CollectionAssert.AreEqual(new byte[] { 0x46, 0x47 },
                                  ret.Value.Sequence.ToArray());
        CollectionAssert.AreEqual(new byte[0],
                                  ret.Value.NewLine.ToArray());
      }
    }

    [Test]
    public void TestReadAndReadLineKeepEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);
      var buffer = new byte[8];

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(5, stream.Read(buffer, 0, 5));

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 5), ArrayExtensions.Slice(buffer, 0, 5));
      Assert.AreEqual(5L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 5, 1), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(6L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(8L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine(keepEOL: true));
      Assert.AreEqual(8L, stream.Position, "Position");
    }

    [Test]
    public void TestReadAndReadLineDiscardEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, Ascii.Octets.CR, Ascii.Octets.LF, 0x44, 0x45};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);
      var buffer = new byte[8];

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(5, stream.Read(buffer, 0, 5));

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 5), ArrayExtensions.Slice(buffer, 0, 5));
      Assert.AreEqual(5L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 5, 0), stream.ReadLine(false));
      Assert.AreEqual(6L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 2), stream.ReadLine(false));
      Assert.AreEqual(8L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(8L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineKeepEOL()
    {
      var data = new byte[] {0x40, Ascii.Octets.CR, 0x42, Ascii.Octets.LF, 0x44, Ascii.Octets.LF, Ascii.Octets.CR, 0x47, Ascii.Octets.CR, Ascii.Octets.LF, 0x50};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 2), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(2L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 2, 2), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(4L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 4, 2), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(6L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 6, 1), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(7L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 7, 3), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(10L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine(keepEOL: true));
      Assert.AreEqual(11L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine(keepEOL: true));
      Assert.AreEqual(11L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineDiscardEOL()
    {
      var data = new byte[] {0x40, Ascii.Octets.CR, 0x42, Ascii.Octets.LF, 0x44, Ascii.Octets.LF, Ascii.Octets.CR, 0x47, Ascii.Octets.CR, Ascii.Octets.LF, 0x50};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 1), stream.ReadLine(false));
      Assert.AreEqual(2L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 2, 1), stream.ReadLine(false));
      Assert.AreEqual(4L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 4, 1), stream.ReadLine(false));
      Assert.AreEqual(6L, stream.Position, "Position");

      Assert.AreEqual(new byte[] {}, stream.ReadLine(false));
      Assert.AreEqual(7L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 7, 1), stream.ReadLine(false));
      Assert.AreEqual(10L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 10, 1), stream.ReadLine(false));
      Assert.AreEqual(11L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine());
      Assert.AreEqual(11L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineBufferEndsWithEOLKeepEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Ascii.Octets.CR};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(data, stream.ReadLine(true));

      Assert.AreEqual(8L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineBufferEndsWithEOLDiscardEOL()
    {
      var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Ascii.Octets.CR};
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 0, 7), stream.ReadLine(false));

      Assert.AreEqual(8L, stream.Position, "Position");
    }

    [Test]
    public void TestReadLineEOLSplittedBetweenBufferDiscardEOL()
    {
      var data = new byte[] {
        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, Ascii.Octets.CR,
        Ascii.Octets.LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, Ascii.Octets.CR,
        0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, Ascii.Octets.LF,
        Ascii.Octets.CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, Ascii.Octets.CR,
      };
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data,  0, 7), stream.ReadLine(false)); // CRLF
      Assert.AreEqual(9L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data,  9, 6), stream.ReadLine(false)); // CR
      Assert.AreEqual(16L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 16, 7), stream.ReadLine(false)); // LF
      Assert.AreEqual(24L, stream.Position, "Position");

      Assert.AreEqual(new byte[0], stream.ReadLine(false)); // CR
      Assert.AreEqual(25L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 25, 6), stream.ReadLine(false)); // CR
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
        Ascii.Octets.CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, Ascii.Octets.CR,
      };
      var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

      Assert.AreEqual(0L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data,  0, 9), stream.ReadLine(true)); // CRLF
      Assert.AreEqual(9L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data,  9, 7), stream.ReadLine(true)); // CR
      Assert.AreEqual(16L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 16, 8), stream.ReadLine(true)); // LF
      Assert.AreEqual(24L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 24, 1), stream.ReadLine(true)); // CR
      Assert.AreEqual(25L, stream.Position, "Position");

      Assert.AreEqual(ArrayExtensions.Slice(data, 25, 7), stream.ReadLine(true)); // CR
      Assert.AreEqual(32L, stream.Position, "Position");

      Assert.IsNull(stream.ReadLine(true)); // EOS
      Assert.AreEqual(32L, stream.Position, "Position");
    }
  }
}
