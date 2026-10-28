// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.IO.Streams.LineOriented;

partial class LineOrientedStreamTests {
  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidCopyToArguments)
  )]
  public void CopyTo_InvalidBufferArguments(
    Stream destination,
    int bufferSize,
    Type expectedExceptionType,
    string message
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.CopyTo(destination, bufferSize), $"{message} (type = {type})");
    }
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidCopyToArguments)
  )]
  public void CopyToAsync_InvalidBufferArguments(
    Stream destination,
    int bufferSize,
    Type expectedExceptionType,
    string message
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.CopyToAsync(destination, bufferSize), $"{message} (type = {type})");
    }
  }

  [Test]
  public async Task CopyTo(
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Assert.That(stream.ReadByte(), Is.EqualTo(data[0]));

    var targetStream = new MemoryStream();

    if (runAsync)
      await stream.CopyToAsync(targetStream, cancellationToken: default);
    else
      stream.CopyTo(targetStream);

    Assert.That(targetStream.ToArray(), Is.EqualTo(data.Skip(1).ToArray()).AsCollection);

    Assert.That(stream.ReadByte(), Is.EqualTo(-1));
  }

  [Test]
  public async Task CopyTo_NothingBuffered(
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var targetStream = new MemoryStream();

    if (runAsync)
      await stream.CopyToAsync(targetStream, cancellationToken: default);
    else
      stream.CopyTo(targetStream);

    Assert.That(targetStream.ToArray(), Is.EqualTo(data).AsCollection);

    Assert.That(stream.ReadByte(), Is.EqualTo(-1));
  }

  [Test]
  public void CopyTo_ArgumentNull_Destination(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.CopyTo(destination: null!));
    Assert.Throws<ArgumentNullException>(() => stream.CopyTo(destination: null!, bufferSize: 0));
  }

  [Test]
  public void CopyToAsync_ArgumentNull_Destination(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null!));
    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null!, bufferSize: 0));
    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null!, bufferSize: 0, cancellationToken: default));
  }

#if SYSTEM_IO_STREAM_COPYTO_VIRTUAL
  [Test]
  public async Task CopyTo_BufferSizeDoesNotAffectToBehavior(
    [Values] StreamType type,
    [Values(1, 1024, int.MaxValue)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), 8);

    var targetStream = new MemoryStream();

    if (runAsync)
      await stream.CopyToAsync(targetStream, bufferSize: bufferSize, cancellationToken: default);
    else
      stream.CopyTo(targetStream, bufferSize: bufferSize);

    Assert.That(targetStream.ToArray(), Is.EqualTo(data).AsCollection);

    Assert.That(stream.ReadByte(), Is.EqualTo(-1));
  }
#endif
}
