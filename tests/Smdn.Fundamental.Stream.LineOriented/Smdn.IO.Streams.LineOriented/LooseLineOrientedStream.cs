// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.IO.Streams.LineOriented;

[TestFixture]
public class LooseLineOrientedStreamTests {
  private const byte CR = (byte)'\r';
  private const byte LF = (byte)'\n';

  [Test]
  public void NewLine()
  {
    using var stream = new LooseLineOrientedStream(new MemoryStream(new byte[0]), 8);

    Assert.IsTrue(stream.NewLine.IsEmpty);
  }

  [Test]
  public void IsStrictNewLine()
  {
    using var stream = new LooseLineOrientedStream(new MemoryStream(new byte[0]), 8);

    Assert.IsFalse(stream.IsStrictNewLine);
  }

  [Test]
  public async Task ReadLineAsync()
  {
    var data = new byte[] {
      0x40, 0x41, CR, LF,
      0x42, 0x43, CR,
      0x44, 0x45, LF,
      LF,
      0x46, 0x47
    };

    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    // CRLF
    var ret = await stream.ReadLineAsync();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, CR, LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41 },
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[] { CR, LF },
                              ret.Value.NewLine.ToArray());

    // CR
    ret = await stream.ReadLineAsync();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { 0x42, 0x43, CR },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x42, 0x43 },
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[] { CR },
                              ret.Value.NewLine.ToArray());

    // LF
    ret = await stream.ReadLineAsync();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { 0x44, 0x45, LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x44, 0x45 },
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[] { LF },
                              ret.Value.NewLine.ToArray());

    // LF (empty line)
    ret = await stream.ReadLineAsync();

    Assert.IsNotNull(ret);
    Assert.IsTrue(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[0],
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[] { LF },
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

  [Test]
  public void ReadAndReadLine_KeepEOL()
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);
    var buffer = new byte[8];

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(5, stream.Read(buffer, 0, 5));

    Assert.AreEqual(data.Skip(0).Take(5).ToArray(), buffer.Skip(0).Take(5).ToArray());
    Assert.AreEqual(5L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(5).Take(1).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(6L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(6).Take(2).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine(keepEOL: true));
    Assert.AreEqual(8L, stream.Position, "Position");
  }

  [Test]
  public void ReadAndReadLine_DiscardEOL()
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);
    var buffer = new byte[8];

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(5, stream.Read(buffer, 0, 5));

    Assert.AreEqual(data.Skip(0).Take(5).ToArray(), buffer.Skip(0).Take(5).ToArray());
    Assert.AreEqual(5L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(5).Take(0).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(6L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(6).Take(2).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
    Assert.AreEqual(8L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_KeepEOL()
  {
    var data = new byte[] {0x40, CR, 0x42, LF, 0x44, LF, CR, 0x47, CR, LF, 0x50};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(2).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(2L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(2).Take(2).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(4).Take(2).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(6L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(6).Take(1).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(7L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(7).Take(3).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(10L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(10).Take(1).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(11L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine(keepEOL: true));
    Assert.AreEqual(11L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_DiscardEOL()
  {
    var data = new byte[] {0x40, CR, 0x42, LF, 0x44, LF, CR, 0x47, CR, LF, 0x50};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(1).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(2L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(2).Take(1).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(4).Take(1).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(6L, stream.Position, "Position");

    Assert.AreEqual(new byte[] {}, stream.ReadLine(false));
    Assert.AreEqual(7L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(7).Take(1).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(10L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(10).Take(1).ToArray(), stream.ReadLine(false));
    Assert.AreEqual(11L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
    Assert.AreEqual(11L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_BufferEndsWithEOL_KeepEOL()
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data, stream.ReadLine(true));

    Assert.AreEqual(8L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_BufferEndsWithEOL_DiscardEOL()
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(7).ToArray(), stream.ReadLine(false));

    Assert.AreEqual(8L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_EOLSplittedBetweenBuffer_DiscardEOL()
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, CR,
    };
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip( 0).Take(7).ToArray(), stream.ReadLine(false)); // CRLF
    Assert.AreEqual(9L, stream.Position, "Position");

    Assert.AreEqual(data.Skip( 9).Take(6).ToArray(), stream.ReadLine(false)); // CR
    Assert.AreEqual(16L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(16).Take(7).ToArray(), stream.ReadLine(false)); // LF
    Assert.AreEqual(24L, stream.Position, "Position");

    Assert.AreEqual(new byte[0], stream.ReadLine(false)); // CR
    Assert.AreEqual(25L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(25).Take(6).ToArray(), stream.ReadLine(false)); // CR
    Assert.AreEqual(32L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine(false)); // EOS
    Assert.AreEqual(32L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_EOLSplittedBetweenBuffer_KeepEOL()
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, CR,
    };
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip( 0).Take(9).ToArray(), stream.ReadLine(true)); // CRLF
    Assert.AreEqual(9L, stream.Position, "Position");

    Assert.AreEqual(data.Skip( 9).Take(7).ToArray(), stream.ReadLine(true)); // CR
    Assert.AreEqual(16L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(16).Take(8).ToArray(), stream.ReadLine(true)); // LF
    Assert.AreEqual(24L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(24).Take(1).ToArray(), stream.ReadLine(true)); // CR
    Assert.AreEqual(25L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(25).Take(7).ToArray(), stream.ReadLine(true)); // CR
    Assert.AreEqual(32L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine(true)); // EOS
    Assert.AreEqual(32L, stream.Position, "Position");
  }
}
