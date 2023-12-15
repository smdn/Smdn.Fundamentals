// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.IO.Streams.LineOriented;

partial class LineOrientedStreamTests {
  [Test]
  public async Task ReadLine_SingleSegment(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(6, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(ret.Value.SequenceWithNewLine.IsSingleSegment, Is.True);
    Assert.That(ret.Value.Sequence.IsSingleSegment, Is.True);
    Assert.That(
      ret.Value.SequenceWithNewLine.ToArray(),
      Is.EqualTo(new byte[] { 0x40, 0x41, 0x42, 0x43, CR, LF }).AsCollection
    );
    Assert.That(
      ret.Value.Sequence.ToArray(),
      Is.EqualTo(new byte[] { 0x40, 0x41, 0x42, 0x43 }).AsCollection
    );
  }

  [Test]
  public async Task ReadLine_MultipleSegment(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, CR, LF};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret!.Value.IsEmpty, Is.False);
    Assert.That(ret.Value.SequenceWithNewLine.IsSingleSegment, Is.False);
    Assert.That(ret.Value.Sequence.IsSingleSegment, Is.False);
    Assert.That(
      ret.Value.SequenceWithNewLine.ToArray(),
      Is.EqualTo(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, CR, LF }).AsCollection
    );
    Assert.That(
      ret.Value.Sequence.ToArray(),
      Is.EqualTo(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f }).AsCollection
    );
  }

  [Test]
  public async Task ReadLineAsync_EndOfStream(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    stream.CopyTo(Stream.Null);

    var ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Null);
  }

  [Test]
  public async Task ReadLineAsync_EndOfStream_NothingBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(true, false)] bool runAsync
  )
  {
    using var stream = CreateStream(type, Stream.Null, 8);

    var ret = runAsync
      ? await stream.ReadLineAsync()
      : stream.ReadLine();

    Assert.That(ret, Is.Null);
  }
}
