// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

#if SYSTEM_ARRAY_EMPTY
using ArrayEmptyShim = System.Array; // System.Array.Empty
#else
using ArrayEmptyShim = Smdn.ArrayShim; // Smdn.ArrayShim.Empty
#endif

namespace Smdn.IO.Streams.Filtering {
  [TestFixture]
  public class FilterStreamTests {
    private static IEnumerable<FilterStream.Filter> EmptyFilters() => Enumerable.Empty<FilterStream.Filter>();

    [Test] public void TestLeaveStreamOpen() => TestLeaveStreamOpen(true);
    [Test] public void TestLeaveStreamNotOpen() => TestLeaveStreamOpen(false);

    private void TestLeaveStreamOpen(bool leaveOpen)
    {
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03});

      using (var stream = new FilterStream(baseStream, EmptyFilters(), leaveStreamOpen: leaveOpen)) {
      }

      if (leaveOpen)
        Assert.That(baseStream.ReadByte, Throws.Nothing);
      else
        Assert.That(baseStream.ReadByte, Throws.TypeOf<ObjectDisposedException>());
    }

    [Test] public void TestClose_LeaveStreamOpen() => TestClose_LeaveStreamOpen(true);
    [Test] public void TestClose_LeaveStreamNotOpen() => TestClose_LeaveStreamOpen(false);

    private void TestClose_LeaveStreamOpen(bool leaveOpen)
    {
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03});

      using (var stream = new FilterStream(baseStream, EmptyFilters(), leaveStreamOpen: leaveOpen)) {
        TestClose(stream);
      }
    }

    private void TestClose(FilterStream stream)
    {
      stream.Dispose();

      Assert.That(stream.CanRead, Is.False, nameof(stream.CanRead));
      Assert.That(stream.CanWrite, Is.False, nameof(stream.CanWrite));
      Assert.That(stream.CanSeek, Is.False, nameof(stream.CanSeek));
      Assert.That(stream.CanTimeout, Is.False, nameof(stream.CanTimeout));

      Assert.That(stream.ReadByte, Throws.TypeOf<ObjectDisposedException>(), nameof(stream.ReadByte));
      Assert.That(() => _ = stream.Read(ArrayEmptyShim.Empty<byte>(), 0, 0), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.Read));
      Assert.That(() => stream.ReadAsync(ArrayEmptyShim.Empty<byte>(), 0, 0), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.ReadAsync));
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      Assert.That(
        async () => _ = await stream.ReadAsync(Memory<byte>.Empty),
        Throws.TypeOf<ObjectDisposedException>(),
        nameof(stream.ReadAsync)
      );
#endif
      Assert.That(() => stream.WriteByte(0x00), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.WriteByte));
      Assert.That(() => stream.Write(ArrayEmptyShim.Empty<byte>(), 0, 0), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.Write));
      Assert.That(() => stream.WriteAsync(ArrayEmptyShim.Empty<byte>(), 0, 0), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.WriteAsync));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      Assert.That(
        async () => await stream.WriteAsync(ReadOnlyMemory<byte>.Empty),
        Throws.TypeOf<ObjectDisposedException>(),
        nameof(stream.WriteAsync)
      );
#endif
      Assert.That(stream.Flush, Throws.TypeOf<ObjectDisposedException>(), nameof(stream.Flush));
      Assert.That(stream.FlushAsync, Throws.TypeOf<ObjectDisposedException>(), nameof(stream.FlushAsync));
      Assert.That(() => stream.Seek(0, SeekOrigin.Begin), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.Seek));
      Assert.That(() => stream.SetLength(0L), Throws.TypeOf<ObjectDisposedException>(), nameof(stream.SetLength));
      Assert.That(() => _ = stream.Length, Throws.TypeOf<ObjectDisposedException>(), nameof(stream.Length));
      Assert.That(() => _ = stream.Position, Throws.TypeOf<ObjectDisposedException>(), nameof(stream.Length));

#if SYSTEM_IO_STREAM_CLOSE
      Assert.That(stream.Close, Throws.Nothing, nameof(stream.Close));
#endif
    }

    [Test] public void TestConstruct_StreamNull() => Assert.That(() => new FilterStream(stream: null, EmptyFilters()), Throws.ArgumentNullException);
    [Test] public void TestConstruct_FiltersNull() => Assert.That(() => new FilterStream(Stream.Null, filters: null), Throws.ArgumentNullException);

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(1)]
    public void TestConstruct_BufferSizeOutOfRange(int bufferSize)
      => Assert.That(() => new FilterStream(Stream.Null, EmptyFilters(), bufferSize), Throws.TypeOf<ArgumentOutOfRangeException>());

    [Test] public void TestWrite() => Assert.That(() => new FilterStream(Stream.Null, EmptyFilters()).Write(ArrayEmptyShim.Empty<byte>(), 0, 0), Throws.TypeOf<NotSupportedException>());
    [Test] public void TestWriteAsync() => Assert.That(async () => await new FilterStream(Stream.Null, EmptyFilters()).WriteAsync(ArrayEmptyShim.Empty<byte>(), 0, 0), Throws.TypeOf<NotSupportedException>());
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    [Test] public void TestWriteAsync_FromReadOnlyMemory() => Assert.That(async () => await new FilterStream(Stream.Null, EmptyFilters()).WriteAsync(ReadOnlyMemory<byte>.Empty), Throws.TypeOf<NotSupportedException>());
#endif
    [Test] public void TestWriteByte() => Assert.That(() => new FilterStream(Stream.Null, EmptyFilters()).WriteByte(0x00), Throws.TypeOf<NotSupportedException>());
    [Test] public void TestFlush() => Assert.That(() => new FilterStream(Stream.Null, EmptyFilters()).Flush(), Throws.TypeOf<NotSupportedException>());
    [Test] public void TestFlushAsync() => Assert.That(async () => await new FilterStream(Stream.Null, EmptyFilters()).FlushAsync(), Throws.TypeOf<NotSupportedException>());
    [Test] public void TestSetLength() => Assert.That(() => new FilterStream(Stream.Null, EmptyFilters()).SetLength(0L), Throws.TypeOf<NotSupportedException>());

    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
    )]
    public void TestRead_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      using var stream = new FilterStream(Stream.Null, EmptyFilters());

      Assert.That(
        () => _ = stream.Read(buffer, offset, count),
        Throws.TypeOf(expectedExceptionType)
      );
    }

    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
    )]
    public void TestReadAsync_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      using var stream = new FilterStream(Stream.Null, EmptyFilters());

      Assert.That(
        () => stream.ReadAsync(buffer, offset, count),
        Throws.TypeOf(expectedExceptionType)
      );
    }

    private enum ReadMethod {
      Read,
      ReadAsync,
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      ReadAsyncToMemory
#endif
    }

    [Test] public Task TestRead() => TestRead(ReadMethod.Read);
    [Test] public Task TestReadAsync() => TestRead(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory() => TestRead(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead(ReadMethod readMethod)
    {
      //                  | internal buffer 0   | internal buffer 1   |
      // offset      -1   | 0    1    2    3    | 4    5    6    7    |
      // baseStream:      | 0xFF 0xFF 0xFF 0xFF | 0xFF 0xFF 0xFF 0xFF |
      // filter0:    0x00 | 0x00 0x00           |                     |
      // filter1:         |                0x01 | 0x01 0x01           |
      // filter2:         |                     | 0x02 0x02           |
      // filter3:         |                     |           0x03 0x03 |
      // filter4:         |                     |                     | 0x04 0x04
      // expected:        | 0x00 0x00 0xFF 0x01 | 0x02 0x02 0x03 0x03 |
      var baseStream = new MemoryStream(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF});
      var expected = new byte[] {0x00, 0x00, 0xFF, 0x01, 0x02, 0x02, 0x03, 0x03 };
      var filters = new [] {
        new FilterStream.FillFilter(-1, 3, 0x00), // filter0
        new FilterStream.FillFilter( 3, 3, 0x01), // filter1
        new FilterStream.FillFilter( 4, 2, 0x02), // filter2
        new FilterStream.FillFilter( 6, 2, 0x03), // filter3
        new FilterStream.FillFilter( 8, 2, 0x04), // filter4
      };

      using (var stream = new FilterStream(baseStream, filters, leaveStreamOpen: true, bufferSize: 4)) {

        Assert.That(stream.Position, Is.Zero, nameof(stream.Position));
        Assert.That(stream.Length, Is.EqualTo(8L), nameof(stream.Length));

        var buffer = new byte[8];
        var read = readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory()),
#endif
          _ => stream.Read(buffer, 0, buffer.Length),
        };

        Assert.That(read, Is.EqualTo(buffer.Length));
        Assert.That(stream.Position, Is.EqualTo(8L), nameof(stream.Position));

        Assert.That(buffer, Is.EqualTo(expected).AsCollection);

        read = readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory()),
#endif
          _ => stream.Read(buffer, 0, buffer.Length),
        };

        Assert.That(read, Is.Zero);
        Assert.That(stream.Position, Is.EqualTo(8L), nameof(stream.Position));
      }
    }

    [Test]
    public void TestReadByte()
    {
      //                  | internal buffer 0   | internal buffer 1   |
      // offset      -1   | 0    1    2    3    | 4    5    6    7    |
      // baseStream:      | 0xFF 0xFF 0xFF 0xFF | 0xFF 0xFF 0xFF 0xFF |
      // filter0:    0x00 | 0x00 0x00           |                     |
      // filter1:         |                0x01 | 0x01 0x01           |
      // filter2:         |                     |                0x02 | 0x02 0x02
      // expected:        | 0x00 0x00 0xFF 0x01 | 0x01 0x01 0xFF 0x02 |
      var baseStream = new MemoryStream(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF});
      var expected = new int[] {0x00, 0x00, 0xFF, 0x01, 0x01, 0x01, 0xFF, 0x02, -1, -1 };
      var filters = new [] {
        new FilterStream.FillFilter(-1, 3, 0x00), // filter0
        new FilterStream.FillFilter( 3, 3, 0x01), // filter1
        new FilterStream.FillFilter( 7, 3, 0x02), // filter2
      };

      using (var stream = new FilterStream(baseStream, filters, leaveStreamOpen: true, bufferSize: 4)) {
        Assert.That(stream.Position, Is.Zero, nameof(stream.Position));
        Assert.That(stream.Length, Is.EqualTo(8L), nameof(stream.Length));

        for (var position = 0; position < expected.Length; position++) {
          Assert.That(stream.ReadByte(), Is.EqualTo(expected[position]), $"position {position}");
        }
      }
    }

    [Test] public Task TestRead_ReadToPartialBuffer() => TestRead_ReadToPartialBuffer(ReadMethod.Read);
    [Test] public Task TestReadAsync_ReadToPartialBuffer() => TestRead_ReadToPartialBuffer(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_ReadToPartialBuffer() => TestRead_ReadToPartialBuffer(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_ReadToPartialBuffer(ReadMethod readMethod)
    {
      //             | internal buffer 0   | internal buffer 1   |
      // offset      | 0    1    2    3    | 4    5    6    7    |
      // baseStream: | 0x00 0x01 0x02 0x03 | 0x04 0x05 0x06 0x07 |
      // filter(OR): |           0xF0 0xF0 | 0xF0 0xF0           |
      // expected:   | 0x00 0x01 0xF2 0xF3 | 0xF4 0xF5 0x06 0x07 |
      var baseStream = new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07});
      var expected = new byte[] {0x00, 0x01, 0xF2, 0xF3, 0xF4, 0xF5, 0x06, 0x07};
      var filter = new FilterStream.BitwiseOrFilter(2, 4, 0xF0);

      using (var stream = new FilterStream(baseStream, filter, leaveStreamOpen: true, bufferSize: 4)) {
        var buffer = new byte[8];

        foreach (var (lengthToRead, expectedLength, expectedPosition, expectedSequence) in new[] {
          (0, 0, 0L, expected.Skip(0).Take(0).ToArray()),
          (1, 1, 1L, expected.Skip(0).Take(1).ToArray()),
          (2, 2, 3L, expected.Skip(1).Take(2).ToArray()),
          (2, 2, 5L, expected.Skip(3).Take(2).ToArray()),
          (4, 3, 8L, expected.Skip(5).Take(3).ToArray()),
          (8, 0, 8L, ArrayEmptyShim.Empty<byte>()),
        }) {
          var read = readMethod switch {
            ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, lengthToRead),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
            ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(0, lengthToRead)),
#endif
            _ => stream.Read(buffer, 0, lengthToRead),
          };

          Assert.That(read, Is.EqualTo(expectedLength));
          Assert.That(stream.Position, Is.EqualTo(expectedPosition), nameof(stream.Position));
          Assert.That(
            buffer.Skip(0).Take(expectedLength).ToArray(),
            Is.EqualTo(expectedSequence).AsCollection
          );
        }
      }
    }

    [Test] public Task TestRead_SeekAndRead() => TestRead_SeekAndRead(ReadMethod.Read);
    [Test] public Task TestReadAsync_SeekAndRead() => TestRead_SeekAndRead(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_SeekAndRead() => TestRead_SeekAndRead(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_SeekAndRead(ReadMethod readMethod)
    {
      //             | internal buffer 0   | internal buffer 1   |
      // offset      | 0    1    2    3    | 4    5    6    7    |
      // baseStream: | 0xFF 0xFF 0xFF 0xFF | 0xFF 0xFF 0xFF 0xFF |
      // filter0:    | 0x00 0x00           |                     |
      // filter1:    |                0x01 | 0x01                |
      // filter2:    |                     |           0x02 0x02 |
      // expected:   | 0x00 0x00 0xFF 0x01 | 0x01 0xFF 0x02 0x02 |
      var baseStream = new MemoryStream(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF});
      var expected = new byte[] {0x00, 0x00, 0xFF, 0x01, 0x01, 0xFF, 0x02, 0x02 };
      var filters = new [] {
        new FilterStream.FillFilter(0, 2, 0x00), // filter0
        new FilterStream.FillFilter(3, 2, 0x01), // filter1
        new FilterStream.FillFilter(6, 2, 0x02), // filter2
      };

      using (var stream = new FilterStream(baseStream, filters, leaveStreamOpen: true, bufferSize: 4)) {
        var buffer = new byte[8];

        foreach (var (offset, length, test) in new[] {
          (0, 8, "(read all)"),
          (0, 3, "#0"),
          (5, 3, "#1"),
          (4, 3, "#2"),
          (2, 3, "#3"),
          (3, 3, "#4"),
          (1, 3, "#5"),
          (6, 2, "#6"),
        }) {
          stream.Position = offset;

          var read = readMethod switch {
            ReadMethod.ReadAsync => await stream.ReadAsync(buffer, offset, length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
            ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory(offset, length)),
#endif
            _ => stream.Read(buffer, offset, length),
          };

          Assert.That(read, Is.EqualTo(length), test + " read count");
          Assert.That(
            buffer.Skip(offset).Take(length).ToArray(), Is.EqualTo(expected.Skip(offset).Take(length).ToArray()).AsCollection,
            test
          );
        }
      }
    }

    [Test] public Task TestRead_ZeroLengthFilter() => TestRead_ZeroLengthFilter(ReadMethod.Read);
    [Test] public Task TestReadAsync_ZeroLengthFilter() => TestRead_ZeroLengthFilter(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_ZeroLengthFilter() => TestRead_ZeroLengthFilter(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_ZeroLengthFilter(ReadMethod readMethod)
    {
      //                  | internal buffer 0   | internal buffer 1   |
      // offset           | 0    1    2    3    | 4    5    6    7    |
      // baseStream:      | 0xFF 0xFF 0xFF 0xFF | 0xFF 0xFF 0xFF 0xFF |
      // expected:        | 0x01 0x02 0x03 0x04 | 0x05 0x06 0x07 0xFF |
      var expected = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
      var baseStream = new MemoryStream((byte[])expected.Clone());
      var filter = new FilterStream.ZeroFilter(0, 0);

      using (var stream = new FilterStream(baseStream, filter, leaveStreamOpen: true, bufferSize: 4)) {
        var buffer = new byte[8];
        var read = readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory()),
#endif
          _ => stream.Read(buffer, 0, buffer.Length),
        };

        Assert.That(read, Is.EqualTo(buffer.Length));
        Assert.That(
          buffer,
          Is.EqualTo(expected).AsCollection
        );
      }
    }

    [Test] public Task TestRead_OffsetWithinFilter_OffsetNegative() => TestRead_OffsetWithinFilter_OffsetNegative(ReadMethod.Read);
    [Test] public Task TestReadAsync_OffsetWithinFilter_OffsetNegative() => TestRead_OffsetWithinFilter_OffsetNegative(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_OffsetWithinFilter_OffsetNegative() => TestRead_OffsetWithinFilter_OffsetNegative(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_OffsetWithinFilter_OffsetNegative(ReadMethod readMethod)
    {
      //                  | internal buffer 0   | internal buffer 1   |
      // offset           | 0    1    2    3    | 4    5    6    7    |
      // baseStream:      | 0xFF 0xFF 0xFF 0xFF | 0xFF 0xFF 0xFF 0xFF |
      // filter:     0x00 | 0x01 0x02 0x03 0x04 | 0x05 0x06 0x07      |
      // expected:        | 0x01 0x02 0x03 0x04 | 0x05 0x06 0x07 0xFF |
      var baseStream = new MemoryStream(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF});
      var expected = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xFF };
      var filter = FilterStream.CreateFilter(-1, 8, (buffer, offsetWithinFilter) => {
        for (var index = 0; index < buffer.Length; index++) {
          buffer[index] = (byte)(offsetWithinFilter + index);
        }
      });

      using (var stream = new FilterStream(baseStream, filter, leaveStreamOpen: true, bufferSize: 4)) {
        var buffer = new byte[8];
        var read = readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory()),
#endif
          _ => stream.Read(buffer, 0, buffer.Length),
        };

        Assert.That(read, Is.EqualTo(buffer.Length));
        Assert.That(
          buffer,
          Is.EqualTo(expected).AsCollection
        );
      }
    }

    [Test] public Task TestRead_OffsetWithinFilter_OffsetPositive() => TestRead_OffsetWithinFilter_OffsetPositive(ReadMethod.Read);
    [Test] public Task TestReadAsync_OffsetWithinFilter_OffsetPositive() => TestRead_OffsetWithinFilter_OffsetPositive(ReadMethod.ReadAsync);
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    [Test] public Task TestReadAsync_ToMemory_OffsetWithinFilter_OffsetPositive() => TestRead_OffsetWithinFilter_OffsetPositive(ReadMethod.ReadAsyncToMemory);
#endif

    private async Task TestRead_OffsetWithinFilter_OffsetPositive(ReadMethod readMethod)
    {
      //                  | internal buffer 0   | internal buffer 1   |
      // offset           | 0    1    2    3    | 4    5    6    7    |
      // baseStream:      | 0xFF 0xFF 0xFF 0xFF | 0xFF 0xFF 0xFF 0xFF |
      // filter:          |      0x00 0x01 0x02 | 0x03 0x04 0x05 0x06 | 0x07
      // expected:        | 0xFF 0x00 0x01 0x02 | 0x03 0x04 0x05 0x06 |
      var baseStream = new MemoryStream(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF});
      var expected = new byte[] {0xFF, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
      var filter = FilterStream.CreateFilter(1, 8, (buffer, offsetWithinFilter) => {
        for (var index = 0; index < buffer.Length; index++) {
          buffer[index] = (byte)(offsetWithinFilter + index);
        }
      });

      using (var stream = new FilterStream(baseStream, filter, leaveStreamOpen: true, bufferSize: 4)) {
        var buffer = new byte[8];
        var read = readMethod switch {
          ReadMethod.ReadAsync => await stream.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncToMemory => await stream.ReadAsync(buffer.AsMemory()),
#endif
          _ => stream.Read(buffer, 0, buffer.Length),
        };

        Assert.That(read, Is.EqualTo(buffer.Length));
        Assert.That(
          buffer,
          Is.EqualTo(expected).AsCollection
        );
      }
    }

    [Test]
    public void TestSeek_SeekOriginBegin()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        Assert.That(stream.Seek(2, SeekOrigin.Begin), Is.EqualTo(2L));
        Assert.That(stream.Position, Is.EqualTo(2L));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x02));

        // beyond the stream length
        var newOffset = stream.Seek(10, SeekOrigin.Begin);
        Assert.That(newOffset, Is.EqualTo(stream.Position));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));

        // before start of stream
        Assert.That(() => stream.Seek(-1, SeekOrigin.Begin), Throws.TypeOf<IOException>());

        Assert.That(stream.Position, Is.EqualTo(10L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
      }
    }

    [Test]
    public void TestSeek_SeekOriginCurrent()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        Assert.That(stream.Seek(2, SeekOrigin.Current), Is.EqualTo(2L));
        Assert.That(stream.Position, Is.EqualTo(2L));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x02));

        // beyond the stream length
        var newOffset = stream.Seek(8, SeekOrigin.Current);
        Assert.That(newOffset, Is.EqualTo(stream.Position));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));

        // before start of stream
        stream.Position = 10L;
        Assert.That(() => stream.Seek(-12, SeekOrigin.Current), Throws.TypeOf<IOException>());

        Assert.That(stream.Position, Is.EqualTo(10L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
      }
    }

    [Test]
    public void TestSetPosition()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        stream.Position = 2;

        Assert.That(stream.Position, Is.EqualTo(2L));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x02));

        // beyond the stream length
        stream.Position = 10;

        Assert.That(stream.Position, Is.EqualTo(10L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));

        // before start of stream
        Assert.That(() => stream.Position = -2, Throws.TypeOf<ArgumentOutOfRangeException>());

        Assert.That(stream.Position, Is.EqualTo(10L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
      }
    }

    [Test]
    public void TestSeek_SeekOriginEnd()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        Assert.That(stream.Seek(-6, SeekOrigin.End), Is.EqualTo(2L));
        Assert.That(stream.Position, Is.EqualTo(2L));
        Assert.That(stream.ReadByte(), Is.EqualTo(0x02));

        // beyond the stream length
        var newOffset = stream.Seek(4, SeekOrigin.End);
        Assert.That(newOffset, Is.EqualTo(stream.Position));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));

        // before start of stream
        Assert.That(() => stream.Seek(-9, SeekOrigin.End), Throws.TypeOf<IOException>());

        Assert.That(stream.Position, Is.EqualTo(12L));
        Assert.That(stream.ReadByte(), Is.EqualTo(-1));
      }
    }
  }
}
