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
public class StrictLineOrientedStreamTests {
  private const byte CR = (byte)'\r';
  private const byte LF = (byte)'\n';

  [Test]
  public void NewLine()
  {
    using var stream = new StrictLineOrientedStream(new MemoryStream(new byte[0]), 8);

    Assert.IsFalse(stream.NewLine.IsEmpty);
    CollectionAssert.AreEqual(new byte[] {0x0d, 0x0a}, stream.NewLine.ToArray(), "must return CRLF");
  }

  [Test]
  public void IsStrictNewLine()
  {
    using var stream = new LooseLineOrientedStream(new MemoryStream(new byte[0]), 8);

    Assert.IsFalse(stream.IsStrictNewLine);
  }

  [Test]
  public async Task ReadLine(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, CR, LF,
      CR, LF,
      0x42, CR, 0x43, LF,
    };

    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    // CRLF
    var ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, CR, LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41 },
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[] { CR, LF },
                              ret.Value.NewLine.ToArray());

    // CRLF (empty line)
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNotNull(ret);
    Assert.IsTrue(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { CR, LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[0],
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[] { CR, LF },
                              ret.Value.NewLine.ToArray());

    // <EOS>
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    CollectionAssert.AreEqual(new byte[] { 0x42, CR, 0x43, LF, },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x42, CR, 0x43, LF, },
                              ret.Value.Sequence.ToArray());
    CollectionAssert.AreEqual(new byte[0],
                              ret.Value.NewLine.ToArray());
  }

  [Test]
  public async Task ReadAndReadLine(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);
    var buffer = new byte[8];

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(5, stream.Read(buffer, 0, 5));

    Assert.AreEqual(data.Skip(0).Take(5).ToArray(), buffer.Skip(0).Take(5).ToArray());
    Assert.AreEqual(5L, stream.Position, "Position");

    var line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNotNull(line);
    Assert.AreEqual(data.Skip(5).Take(3).ToArray(), line.Value.SequenceWithNewLine.ToArray());
    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
    Assert.AreEqual(8L, stream.Position, "Position");
  }

  [Test]
  public async Task ReadLine_CRLF(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, CR, 0x42, LF, 0x44, LF, CR, 0x47, CR, LF, 0x50};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.AreEqual(0L, stream.Position, "Position");

    var line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNotNull(line);
    Assert.AreEqual(data.Skip(0).Take(10).ToArray(), line.Value.SequenceWithNewLine.ToArray());
    Assert.AreEqual(10L, stream.Position, "Position");

    line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNotNull(line);
    Assert.AreEqual(data.Skip(10).Take(1).ToArray(), line.Value.SequenceWithNewLine.ToArray());
    Assert.AreEqual(11L, stream.Position, "Position");

    line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.IsNull(line);
    Assert.AreEqual(11L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_DiscardEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), stream.ReadLine(keepEOL: false));
    Assert.AreEqual(6L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(6).Take(2).ToArray(), stream.ReadLine(keepEOL: false));
    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
    Assert.AreEqual(8L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_KeepEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(6).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(6L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(6).Take(2).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
  }

  [Test]
  public void ReadLine_LongerThanBuffer_DiscardEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, CR, LF,
      0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(10).ToArray(), stream.ReadLine(keepEOL: false));
    Assert.AreEqual(12L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(12).ToArray(), stream.ReadLine(keepEOL: false));
    Assert.AreEqual(22L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
    Assert.AreEqual(22L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_LongerThanBuffer_KeepEOL()
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, CR, LF,
      0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(12).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(12L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(12).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(22L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine());
    Assert.AreEqual(22L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_EOLSplittedBetweenBuffer_DiscardEOL()
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(7).ToArray(), stream.ReadLine(keepEOL: false)); // CRLF
    Assert.AreEqual(9L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(9).Take(23).ToArray(), stream.ReadLine(keepEOL: false));
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
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(9).ToArray(), stream.ReadLine(keepEOL: true)); // CRLF
    Assert.AreEqual(9L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(9).Take(23).ToArray(), stream.ReadLine(keepEOL: true));
    Assert.AreEqual(32L, stream.Position, "Position");

    Assert.IsNull(stream.ReadLine(true)); // EOS
    Assert.AreEqual(32L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_IncompleteEOL_DiscardEOL(
    [Values(4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, CR,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, CR}, stream.ReadLine(keepEOL: false));
    Assert.AreEqual(4L, stream.Position, "Position");
  }

  [Test]
  public void ReadLine_IncompleteEOL_KeepEOL(
    [Values(4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, CR,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.AreEqual(0L, stream.Position, "Position");

    Assert.AreEqual(new byte[] {0x40, 0x41, 0x42, CR}, stream.ReadLine(keepEOL: true));

    Assert.AreEqual(4L, stream.Position, "Position");
  }
}
