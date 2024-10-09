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

    Assert.That(stream.NewLine.IsEmpty, Is.True);
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
    [Values] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, CR, LF,
      0x42, 0x43, CR,
      0x44, 0x45, LF,
      LF,
      0x46, 0x47
    };

    using var stream = new LooseLineOrientedStream(new MemoryStream(data), bufferSize);

    // CRLF
    var ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(ret.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(new byte[] { 0x40, 0x41, CR, LF }).AsCollection);
    Assert.That(ret.Value.Sequence.ToArray(), Is.EqualTo(new byte[] { 0x40, 0x41 }).AsCollection);
    Assert.That(ret.Value.NewLine.ToArray(), Is.EqualTo(new byte[] { CR, LF }).AsCollection);

    // CR
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(ret.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(new byte[] { 0x42, 0x43, CR }).AsCollection);
    Assert.That(ret.Value.Sequence.ToArray(), Is.EqualTo(new byte[] { 0x42, 0x43 }).AsCollection);
    Assert.That(ret.Value.NewLine.ToArray(), Is.EqualTo(new byte[] { CR }).AsCollection);

    // LF
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(ret.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(new byte[] { 0x44, 0x45, LF }).AsCollection);
    Assert.That(ret.Value.Sequence.ToArray(), Is.EqualTo(new byte[] { 0x44, 0x45 }).AsCollection);
    Assert.That(ret.Value.NewLine.ToArray(), Is.EqualTo(new byte[] { LF }).AsCollection);

    // LF (empty line)
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.True);
    Assert.That(ret.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(new byte[] { LF }).AsCollection);
    Assert.That(ret.Value.Sequence.ToArray(), Is.EqualTo(new byte[0]).AsCollection);
    Assert.That(ret.Value.NewLine.ToArray(), Is.EqualTo(new byte[] { LF }).AsCollection);

    // <EOS>
    ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(ret.Value.SequenceWithNewLine.ToArray(), Is.EqualTo(new byte[] { 0x46, 0x47 }).AsCollection);
    Assert.That(ret.Value.Sequence.ToArray(), Is.EqualTo(new byte[] { 0x46, 0x47 }).AsCollection);
    Assert.That(ret.Value.NewLine.ToArray(), Is.EqualTo(new byte[0]).AsCollection);
  }

  [Test]
  public async Task ReadAndReadLine_KeepEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), bufferSize);
    var buffer = new byte[8];

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    Assert.That(stream.Read(buffer, 0, 5), Is.EqualTo(5));

    Assert.That(buffer.Skip(0).Take(5).ToArray(), Is.EqualTo(data.Skip(0).Take(5).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(5L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(5).Take(1).ToArray()));
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
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");
  }

  [Test]
  public async Task ReadAndReadLine_DiscardEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), bufferSize);
    var buffer = new byte[8];

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    Assert.That(stream.Read(buffer, 0, 5), Is.EqualTo(5));

    Assert.That(buffer.Skip(0).Take(5).ToArray(), Is.EqualTo(data.Skip(0).Take(5).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(5L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(5).Take(0).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(6L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(6).Take(2).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.Null);
    Assert.That(stream.Position, Is.EqualTo(8L), "Position");
  }

  [Test]
  public async Task ReadLine_KeepEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, CR, 0x42, LF, 0x44, LF, CR, 0x47, CR, LF, 0x50};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(2).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(2L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(2).Take(2).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(4).Take(2).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(6L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(6).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(7L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(7).Take(3).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(10L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(10).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(11L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.Null);
    Assert.That(stream.Position, Is.EqualTo(11L), "Position");
  }

  [Test]
  public async Task ReadLine_DiscardEOL(
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, CR, 0x42, LF, 0x44, LF, CR, 0x47, CR, LF, 0x50};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), bufferSize);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(2L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(2).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(4).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(6L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(new byte[] {}));
    Assert.That(stream.Position, Is.EqualTo(7L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(7).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(10L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(10).Take(1).ToArray()));
    Assert.That(stream.Position, Is.EqualTo(11L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.Null);
    Assert.That(stream.Position, Is.EqualTo(11L), "Position");
  }

  [Test]
  public async Task ReadLine_BufferEndsWithEOL_KeepEOL(
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data));

    Assert.That(stream.Position, Is.EqualTo(8L), "Position");
  }

  [Test]
  public async Task ReadLine_BufferEndsWithEOL_DiscardEOL(
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR};
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(7).ToArray()));

    Assert.That(stream.Position, Is.EqualTo(8L), "Position");
  }

  [Test]
  public async Task ReadLine_EOLSplittedBetweenBuffer_DiscardEOL(
    [Values] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, CR,
    };
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip( 0).Take(7).ToArray())); // CRLF
    Assert.That(stream.Position, Is.EqualTo(9L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip( 9).Take(6).ToArray())); // CR
    Assert.That(stream.Position, Is.EqualTo(16L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(16).Take(7).ToArray())); // LF
    Assert.That(stream.Position, Is.EqualTo(24L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(new byte[0])); // CR
    Assert.That(stream.Position, Is.EqualTo(25L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.EqualTo(data.Skip(25).Take(6).ToArray())); // CR
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: false))?.ToArray()
      : stream.ReadLine(keepEOL: false);

    Assert.That(line, Is.Null); // EOS
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");
  }

  [Test]
  public async Task ReadLine_EOLSplittedBetweenBuffer_KeepEOL(
    [Values] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, CR,
      LF, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, CR,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, LF,
      CR, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, CR,
    };
    using var stream = new LooseLineOrientedStream(new MemoryStream(data), 8);

    Assert.That(stream.Position, Is.EqualTo(0L), "Position");

    var line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip( 0).Take(9).ToArray())); // CRLF
    Assert.That(stream.Position, Is.EqualTo(9L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip( 9).Take(7).ToArray())); // CR
    Assert.That(stream.Position, Is.EqualTo(16L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(16).Take(8).ToArray())); // LF
    Assert.That(stream.Position, Is.EqualTo(24L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(24).Take(1).ToArray())); // CR
    Assert.That(stream.Position, Is.EqualTo(25L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.EqualTo(data.Skip(25).Take(7).ToArray())); // CR
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");

    line = runAsync
      ? (await stream.ReadLineAsync(keepEOL: true))?.ToArray()
      : stream.ReadLine(keepEOL: true);

    Assert.That(line, Is.Null); // EOS
    Assert.That(stream.Position, Is.EqualTo(32L), "Position");
  }
}
