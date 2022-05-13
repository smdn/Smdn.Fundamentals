// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

namespace Smdn.IO.Streams.LineOriented;

[TestFixture]
public partial class LineOrientedStreamTests {
  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidWriteBufferArguments)
  )]
  public void Write_InvalidBufferArguments(
    byte[] buffer,
    int offset,
    int count,
    Type expectedExceptionType
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.Write(buffer, offset, count), $"type = {type}");
    }
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidWriteBufferArguments)
  )]
  public void WriteAsync_InvalidBufferArguments(
    byte[] buffer,
    int offset,
    int count,
    Type expectedExceptionType
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.WriteAsync(buffer, offset, count), $"type = {type}");
    }
  }

  [Test]
  public async Task Write(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(true, false)] bool runAsync
  )
  {
    using var baseStream = new MemoryStream(capacity: 8);
    using var stream = CreateStream(type, baseStream, 8);

    Assert.IsTrue(stream.CanWrite, nameof(stream.CanWrite));

    var data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

    if (runAsync)
      await stream.WriteAsync(data, 0, data.Length);
    else
      stream.Write(data, 0, data.Length);

    Assert.AreEqual(4L, stream.Length, nameof(stream.Length));
    Assert.AreEqual(4L, stream.Position, nameof(stream.Position));

    Assert.That(baseStream.ToArray().AsMemory(), Is.EqualTo(data.AsMemory()));
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  [Test]
  public async Task WriteAsync_ToReadOnlyMemory(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var baseStream = new MemoryStream(capacity: 8);
    using var stream = CreateStream(type, baseStream, 8);

    Assert.IsTrue(stream.CanWrite, nameof(stream.CanWrite));

    ReadOnlyMemory<byte> data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

    await stream.WriteAsync(data);

    Assert.AreEqual(4L, stream.Length, nameof(stream.Length));
    Assert.AreEqual(4L, stream.Position, nameof(stream.Position));

    Assert.That(baseStream.ToArray().AsMemory(), Is.EqualTo(data));
  }
#endif
}
