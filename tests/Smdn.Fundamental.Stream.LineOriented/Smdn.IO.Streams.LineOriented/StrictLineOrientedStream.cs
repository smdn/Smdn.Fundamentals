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

    Assert.That(stream.NewLine.IsEmpty, Is.False);
    Assert.That(stream.NewLine.ToArray(), Is.EqualTo(new byte[] {0x0d, 0x0a}).AsCollection, "must return CRLF");
  }

  [Test]
  public void IsStrictNewLine()
  {
    using var stream = new LooseLineOrientedStream(new MemoryStream(new byte[0]), 8);

    Assert.That(stream.IsStrictNewLine, Is.False);
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

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(
      ret.Value.SequenceWithNewLine.ToArray(),
      Is.EqualTo(new byte[] { 0x40, 0x41, CR, LF }).AsCollection
    );
    Assert.That(
      ret.Value.Sequence.ToArray(),
      Is.EqualTo(new byte[] { 0x40, 0x41 }).AsCollection
    );
    Assert.That(
      ret.Value.NewLine.ToArray(),
      Is.EqualTo(new byte[] { CR, LF }).AsCollection
    );

    // CRLF (empty line)
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.True);
    Assert.That(
      ret.Value.SequenceWithNewLine.ToArray(),
      Is.EqualTo(new byte[] { CR, LF }).AsCollection
    );
    Assert.That(
      ret.Value.Sequence.ToArray(),
      Is.EqualTo(new byte[0]).AsCollection
    );
    Assert.That(
      ret.Value.NewLine.ToArray(),
      Is.EqualTo(new byte[] { CR, LF }).AsCollection
    );

    // <EOS>
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(
      ret.Value.SequenceWithNewLine.ToArray(),
      Is.EqualTo(new byte[] { 0x42, CR, 0x43, LF, }).AsCollection
    );
    Assert.That(
      ret.Value.Sequence.ToArray(),
      Is.EqualTo(new byte[] { 0x42, CR, 0x43, LF, }).AsCollection
    );
    Assert.That(
      ret.Value.NewLine.ToArray(),
      Is.EqualTo(new byte[0]).AsCollection
    );
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

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    Assert.That(stream.Read(buffer, 0, 5), Is.EqualTo(5));

    Assert.That(buffer.Skip(0).Take(5).ToArray(), Is.EqualTo(data.Skip(0).Take(5).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(5L), "Position");

    var line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(line, Is.Not.Null);
    Assert.That(line!.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(data.Skip(5).Take(3).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    Assert.That(stream.ReadLine(), Is.Null);
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");
  }

  [Test]
  public async Task ReadLine_CRLF(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, CR, 0x42, LF, 0x44, LF, CR, 0x47, CR, LF, 0x50};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(line, Is.Not.Null);
    Assert.That(line!.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(data.Skip(0).Take(10).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(10L), "Position");

    line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(line, Is.Not.Null);
    Assert.That(line!.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(data.Skip(10).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(11L), "Position");

    line = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(line, Is.Null);
    Assert.That(stream.Position, Is.EqualTo(11L), "Position");
  }

  [Test]
  public async Task ReadLine_DiscardEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(6L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(6).Take(2).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(stream.ReadLine(), Is.Null);
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");
  }

  [Test]
  public async Task ReadLine_KeepEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(6).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(6L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(6).Take(2).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.Null);
  }

  [Test]
  public async Task ReadLine_LongerThanBuffer_DiscardEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, CR, LF,
      0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(10).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(12).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(22L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(stream.ReadLine(), Is.Null);
    Assert.That(stream.Position, Is.EqualTo(22L), "Position");
  }

  [Test]
  public async Task ReadLine_LongerThanBuffer_KeepEOL(
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, CR, LF,
      0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(12).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(12).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(22L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.Null);
    Assert.That(stream.Position, Is.EqualTo(22L), "Position");
  }

  [Test]
  public async Task ReadLine_EOLSplittedBetweenBuffer_DiscardEOL(
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(7).ToArray())); // CRLF
    Assert.That(stream.Position, Is.EqualTo(9L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(9).Take(23).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.Null); // EOS
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");
  }

  [Test]
  public async Task ReadLine_EOLSplittedBetweenBuffer_KeepEOL(
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(9).ToArray())); // CRLF
    Assert.That(stream.Position, Is.EqualTo(9L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(9).Take(23).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.Null); // EOS
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");
  }

  [Test]
  public async Task ReadLine_IncompleteEOL_DiscardEOL(
    [Values(4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, CR,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(new byte[] {0x40, 0x41, 0x42, CR}));
    Assert.That(stream.Position, Is.EqualTo(4L), "Position");
  }

  [Test]
  public async Task ReadLine_IncompleteEOL_KeepEOL(
    [Values(4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, CR,
    };
    using var stream = new StrictLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(new byte[] {0x40, 0x41, 0x42, CR}));

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");
  }
}
