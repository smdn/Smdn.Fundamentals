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
  public async Task CopyToAsync(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Assert.AreEqual(data[0], stream.ReadByte());

    var targetStream = new MemoryStream();

    await stream.CopyToAsync(targetStream, cancellationToken: default);

    CollectionAssert.AreEqual(data.Skip(1).ToArray(), targetStream.ToArray());

    Assert.AreEqual(-1, stream.ReadByte());
  }

  [Test]
  public async Task CopyToAsync_NothingBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var targetStream = new MemoryStream();

    await stream.CopyToAsync(targetStream, cancellationToken: default);

    CollectionAssert.AreEqual(data, targetStream.ToArray());

    Assert.AreEqual(-1, stream.ReadByte());
  }

  [Test]
  public void CopyToAsync_ArgumentNull_Destination(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null));
    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null, bufferSize: 0));
    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null, bufferSize: 0, cancellationToken: default));
  }

  [Test]
  public async Task CopyToAsync_BufferSizeDoesNotAffectToBehaviour(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 1024, int.MaxValue)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), 8);

    var targetStream = new MemoryStream();

    await stream.CopyToAsync(targetStream, bufferSize: bufferSize, cancellationToken: default);

    CollectionAssert.AreEqual(data, targetStream.ToArray());

    Assert.AreEqual(-1, stream.ReadByte());
  }
}
