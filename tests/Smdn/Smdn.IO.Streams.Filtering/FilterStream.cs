// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

#if NET46_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
using _Array = System.Array; // Array.Empty
#else
using _Array = Smdn.ArrayExtensions; // ArrayExtensions.Empty
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
        Assert.DoesNotThrow(() => baseStream.ReadByte());
      else
        Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte());
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

      Assert.IsFalse(stream.CanRead, nameof(stream.CanRead));
      Assert.IsFalse(stream.CanWrite, nameof(stream.CanWrite));
      Assert.IsFalse(stream.CanSeek, nameof(stream.CanSeek));
      Assert.IsFalse(stream.CanTimeout, nameof(stream.CanTimeout));

      Assert.Throws<ObjectDisposedException>(() => stream.ReadByte(), nameof(stream.ReadByte));
      Assert.Throws<ObjectDisposedException>(() => stream.Read(_Array.Empty<byte>(), 0, 0), nameof(stream.Read));
      Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(_Array.Empty<byte>(), 0, 0), nameof(stream.ReadAsync));
      Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00), nameof(stream.WriteByte));
      Assert.Throws<ObjectDisposedException>(() => stream.Write(_Array.Empty<byte>(), 0, 0), nameof(stream.Write));
      Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(_Array.Empty<byte>(), 0, 0), nameof(stream.WriteAsync));
      Assert.Throws<ObjectDisposedException>(() => stream.Flush(), nameof(stream.Flush));
      Assert.Throws<ObjectDisposedException>(() => stream.FlushAsync(), nameof(stream.FlushAsync));
      Assert.Throws<ObjectDisposedException>(() => stream.Seek(0, SeekOrigin.Begin), nameof(stream.Seek));
      Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L), nameof(stream.SetLength));
      Assert.Throws<ObjectDisposedException>(() => { Assert.AreEqual(-1, stream.Length); }, nameof(stream.Length));
      Assert.Throws<ObjectDisposedException>(() => { Assert.AreEqual(-1, stream.Position); }, nameof(stream.Length));

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      Assert.DoesNotThrow(() => stream.Close(), nameof(stream.Close));
#endif
    }

    [Test] public void TestConstruct_StreamNull() => Assert.Throws<ArgumentNullException>(() => new FilterStream(stream: null, EmptyFilters()));
    [Test] public void TestConstruct_FiltersNull() => Assert.Throws<ArgumentNullException>(() => new FilterStream(Stream.Null, filters: null));

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(1)]
    public void TestConstruct_BufferSizeOutOfRange(int bufferSize)
      => Assert.Throws<ArgumentOutOfRangeException>(() => new FilterStream(Stream.Null, EmptyFilters(), bufferSize));

    [Test] public void TestWrite() => Assert.Throws<NotSupportedException>(() => new FilterStream(Stream.Null, EmptyFilters()).Write(_Array.Empty<byte>(), 0, 0));
    [Test] public void TestWriteAsync() => Assert.ThrowsAsync<NotSupportedException>(async () => await new FilterStream(Stream.Null, EmptyFilters()).WriteAsync(_Array.Empty<byte>(), 0, 0));
    [Test] public void TestWriteByte() => Assert.Throws<NotSupportedException>(() => new FilterStream(Stream.Null, EmptyFilters()).WriteByte(0x00));
    [Test] public void TestFlush() => Assert.Throws<NotSupportedException>(() => new FilterStream(Stream.Null, EmptyFilters()).Flush());
    [Test] public void TestFlushAsync() => Assert.ThrowsAsync<NotSupportedException>(async () => await new FilterStream(Stream.Null, EmptyFilters()).FlushAsync());
    [Test] public void TestSetLength() => Assert.Throws<NotSupportedException>(() => new FilterStream(Stream.Null, EmptyFilters()).SetLength(0L));

    [Test] public void TestRead_ArgumentOutOfRange() => TestRead_ArgumentException(runAsync: false);
    [Test] public void TestReadAsync_ArgumentOutOfRange() => TestRead_ArgumentException(runAsync: true);

    private void TestRead_ArgumentException(bool runAsync)
    {
      using (var stream = new FilterStream(Stream.Null, EmptyFilters())) {
        var buffer = new byte[2];

        if (runAsync)
          Assert.Throws<ArgumentNullException>(() => stream.ReadAsync(buffer: null, 0, 2));
        else
          Assert.Throws<ArgumentNullException>(() => stream.Read(buffer: null, 0, 2));

        if (runAsync)
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, -1, 0));
        else
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, -1, 0));

        if (runAsync)
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, 0, -1));
        else
          Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, 0, -1));

        if (runAsync)
          Assert.Throws<ArgumentException>(() => stream.ReadAsync(buffer, 1, 2));
        else
          Assert.Throws<ArgumentException>(() => stream.Read(buffer, 1, 2));

        if (runAsync)
          Assert.Throws<ArgumentException>(() => stream.ReadAsync(buffer, 2, 1));
        else
          Assert.Throws<ArgumentException>(() => stream.Read(buffer, 2, 1));
      }
    }

    [Test] public Task TestRead() => TestRead(runAsync: false);
    [Test] public Task TestReadAsync() => TestRead(runAsync: true);

    private async Task TestRead(bool runAsync)
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
        Assert.AreEqual(0L, stream.Position, nameof(stream.Position));
        Assert.AreEqual(8L, stream.Length, nameof(stream.Length));

        var buffer = new byte[8];
        var read = runAsync
          ? await stream.ReadAsync(buffer, 0, buffer.Length)
          : stream.Read(buffer, 0, buffer.Length);

        Assert.AreEqual(buffer.Length, read);
        Assert.AreEqual(8L, stream.Position, nameof(stream.Position));

        CollectionAssert.AreEqual(expected, buffer);

        read = runAsync
          ? await stream.ReadAsync(buffer, 0, buffer.Length)
          : stream.Read(buffer, 0, buffer.Length);

        Assert.AreEqual(0, read);
        Assert.AreEqual(8L, stream.Position, nameof(stream.Position));
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
        Assert.AreEqual(0L, stream.Position, nameof(stream.Position));
        Assert.AreEqual(8L, stream.Length, nameof(stream.Length));

        for (var position = 0; position < expected.Length; position++) {
          Assert.AreEqual(expected[position], stream.ReadByte(), $"position {position}");
        }
      }
    }

    [Test] public Task TestRead_ReadToPartialBuffer() => TestRead_ReadToPartialBuffer(runAsync: false);
    [Test] public Task TestReadAsync_ReadToPartialBuffer() => TestRead_ReadToPartialBuffer(runAsync: true);

    private async Task TestRead_ReadToPartialBuffer(bool runAsync)
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
          (0, 0, 0L, expected.Slice(0, 0)),
          (1, 1, 1L, expected.Slice(0, 1)),
          (2, 2, 3L, expected.Slice(1, 2)),
          (2, 2, 5L, expected.Slice(3, 2)),
          (4, 3, 8L, expected.Slice(5, 3)),
          (8, 0, 8L, _Array.Empty<byte>()),
        }) {
          var read = runAsync
            ? await stream.ReadAsync(buffer, 0, lengthToRead)
            : stream.Read(buffer, 0, lengthToRead);

          Assert.AreEqual(expectedLength, read);
          Assert.AreEqual(expectedPosition, stream.Position, nameof(stream.Position));
          CollectionAssert.AreEqual(
            expectedSequence,
            buffer.Slice(0, expectedLength)
          );
        }
      }
    }

    [Test] public Task TestRead_SeekAndRead() => TestRead_SeekAndRead(runAsync: false);
    [Test] public Task TestReadAsync_SeekAndRead() => TestRead_SeekAndRead(runAsync: true);

    private async Task TestRead_SeekAndRead(bool runAsync)
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

          var read = runAsync
          ? await stream.ReadAsync(buffer, offset, length)
          : stream.Read(buffer, offset, length);

          Assert.AreEqual(length, read, test + " read count");
          CollectionAssert.AreEqual(
            expected.Slice(offset, length),
            buffer.Slice(offset, length),
            test
          );
        }
      }
    }

    [Test] public Task TestRead_ZeroLengthFilter() => TestRead_ZeroLengthFilter(runAsync: false);
    [Test] public Task TestReadAsync_ZeroLengthFilter() => TestRead_ZeroLengthFilter(runAsync: true);

    private async Task TestRead_ZeroLengthFilter(bool runAsync)
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
        var read = runAsync
          ? await stream.ReadAsync(buffer, 0, buffer.Length)
          : stream.Read(buffer, 0, buffer.Length);

        Assert.AreEqual(buffer.Length, read);
        CollectionAssert.AreEqual(
          expected,
          buffer
        );
      }
    }

    [Test] public Task TestRead_OffsetWithinFilter_OffsetNegative() => TestRead_OffsetWithinFilter_OffsetNegative(runAsync: false);
    [Test] public Task TestReadAsync_OffsetWithinFilter_OffsetNegative() => TestRead_OffsetWithinFilter_OffsetNegative(runAsync: true);

    private async Task TestRead_OffsetWithinFilter_OffsetNegative(bool runAsync)
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
        var read = runAsync
          ? await stream.ReadAsync(buffer, 0, buffer.Length)
          : stream.Read(buffer, 0, buffer.Length);

        Assert.AreEqual(buffer.Length, read);
        CollectionAssert.AreEqual(
          expected,
          buffer
        );
      }
    }

    [Test] public Task TestRead_OffsetWithinFilter_OffsetPositive() => TestRead_OffsetWithinFilter_OffsetPositive(runAsync: false);
    [Test] public Task TestReadAsync_OffsetWithinFilter_OffsetPositive() => TestRead_OffsetWithinFilter_OffsetPositive(runAsync: true);

    private async Task TestRead_OffsetWithinFilter_OffsetPositive(bool runAsync)
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
        var read = runAsync
          ? await stream.ReadAsync(buffer, 0, buffer.Length)
          : stream.Read(buffer, 0, buffer.Length);

        Assert.AreEqual(buffer.Length, read);
        CollectionAssert.AreEqual(
          expected,
          buffer
        );
      }
    }

    [Test]
    public void TestSeek_SeekOriginBegin()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        Assert.AreEqual(2L, stream.Seek(2, SeekOrigin.Begin));
        Assert.AreEqual(2L, stream.Position);
        Assert.AreEqual(0x02, stream.ReadByte());

        // beyond the stream length
        var newOffset = stream.Seek(10, SeekOrigin.Begin);
        Assert.AreEqual(stream.Position, newOffset);
        Assert.AreEqual(-1, stream.ReadByte());

        // before start of stream
        Assert.Throws<IOException>(() => stream.Seek(-1, SeekOrigin.Begin));

        Assert.AreEqual(10L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());
      }
    }

    [Test]
    public void TestSeek_SeekOriginCurrent()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        Assert.AreEqual(2L, stream.Seek(2, SeekOrigin.Current));
        Assert.AreEqual(2L, stream.Position);
        Assert.AreEqual(0x02, stream.ReadByte());

        // beyond the stream length
        var newOffset = stream.Seek(8, SeekOrigin.Current);
        Assert.AreEqual(stream.Position, newOffset);
        Assert.AreEqual(-1, stream.ReadByte());

        // before start of stream
        stream.Position = 10L;
        Assert.Throws<IOException>(() => stream.Seek(-12, SeekOrigin.Current));

        Assert.AreEqual(10L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());
      }
    }

    [Test]
    public void TestSetPosition()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        stream.Position = 2;

        Assert.AreEqual(2L, stream.Position);
        Assert.AreEqual(0x02, stream.ReadByte());

        // beyond the stream length
        stream.Position = 10;

        Assert.AreEqual(10L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());

        // before start of stream
        Assert.Throws<ArgumentOutOfRangeException>(() => stream.Position = -2);

        Assert.AreEqual(10L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());
      }
    }

    [Test]
    public void TestSeek_SeekOriginEnd()
    {
      using (var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }, false)) {
        var stream = new FilterStream(baseStream, EmptyFilters());

        // in range
        Assert.AreEqual(2L, stream.Seek(-6, SeekOrigin.End));
        Assert.AreEqual(2L, stream.Position);
        Assert.AreEqual(0x02, stream.ReadByte());

        // beyond the stream length
        var newOffset = stream.Seek(4, SeekOrigin.End);
        Assert.AreEqual(stream.Position, newOffset);
        Assert.AreEqual(-1, stream.ReadByte());

        // before start of stream
        Assert.Throws<IOException>(() => stream.Seek(-9, SeekOrigin.End));

        Assert.AreEqual(12L, stream.Position);
        Assert.AreEqual(-1, stream.ReadByte());
      }
    }
  }
}
