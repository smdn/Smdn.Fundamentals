// Smdn.Fundamental.Stream.LineOriented.dll (Smdn.Fundamental.Stream.LineOriented-3.1.0)
//   Name: Smdn.Fundamental.Stream.LineOriented
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+9f376fbefdaee2cc51ca4d0636a210d354e186c3
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Smdn.IO.Streams.LineOriented;

namespace Smdn.IO.Streams.LineOriented {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LineOrientedStream : Stream {
    [NullableContext(byte.MinValue)]
    public readonly struct Line {
      public Line(ReadOnlySequence<byte> sequenceWithNewLine, SequencePosition positionOfNewLine) {}

      public bool IsEmpty { get; }
      public ReadOnlySequence<byte> NewLine { get; }
      public SequencePosition PositionOfNewLine { get; }
      public ReadOnlySequence<byte> Sequence { get; }
      public ReadOnlySequence<byte> SequenceWithNewLine { get; }
    }

    protected const int DefaultBufferSize = 1024;
    protected const bool DefaultLeaveStreamOpen = false;
    protected const int MinimumBufferSize = 1;

    [NullableContext(byte.MinValue)]
    public LineOrientedStream([Nullable(1)] Stream stream, ReadOnlySpan<byte> newLine, int bufferSize = 1024, bool leaveStreamOpen = false) {}

    public int BufferSize { get; }
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public virtual Stream InnerStream { get; }
    public bool IsStrictNewLine { get; }
    public override long Length { get; }
    [Nullable(byte.MinValue)]
    public ReadOnlySpan<byte> NewLine { get; }
    public override long Position { get; set; }

    public override void CopyTo(Stream destination, int bufferSize) {}
    public void CopyTo(Stream destination) {}
    public Task CopyToAsync(Stream destination, CancellationToken cancellationToken) {}
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken = default) {}
    protected override void Dispose(bool disposing) {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public long Read(Stream targetStream, long length) {}
    [NullableContext(byte.MinValue)]
    public override int Read(Span<byte> buffer) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public Task<long> ReadAsync(Stream targetStream, long length, CancellationToken cancellationToken = default) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    [NullableContext(byte.MinValue)]
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) {}
    public override int ReadByte() {}
    public LineOrientedStream.Line? ReadLine() {}
    [NullableContext(2)]
    public byte[] ReadLine(bool keepEOL) {}
    [return: Nullable] public Task<ReadOnlySequence<byte>?> ReadLineAsync(bool keepEOL, CancellationToken cancellationToken = default) {}
    public Task<LineOrientedStream.Line?> ReadLineAsync(CancellationToken cancellationToken = default) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    [NullableContext(byte.MinValue)]
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LooseLineOrientedStream : LineOrientedStream {
    [NullableContext(1)]
    public LooseLineOrientedStream(Stream stream, int bufferSize = 1024, bool leaveStreamOpen = false) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class StrictLineOrientedStream : LineOrientedStream {
    [NullableContext(1)]
    public StrictLineOrientedStream(Stream stream, int bufferSize = 1024, bool leaveStreamOpen = false) {}
    public StrictLineOrientedStream([Nullable(1)] Stream stream, ReadOnlySpan<byte> newLine, int bufferSize = 1024, bool leaveStreamOpen = false) {}
  }
}

