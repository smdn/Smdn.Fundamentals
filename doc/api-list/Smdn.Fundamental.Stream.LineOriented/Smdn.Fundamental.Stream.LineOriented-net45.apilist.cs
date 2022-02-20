// Smdn.Fundamental.Stream.LineOriented.dll (Smdn.Fundamental.Stream.LineOriented-3.0.2)
//   Name: Smdn.Fundamental.Stream.LineOriented
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+f6313286e6c89f255a4e55ad199841306da18ec2
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Smdn.IO.Streams.LineOriented;

namespace Smdn.IO.Streams.LineOriented {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LineOrientedStream : Stream {
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
    protected const int MinimumBufferSize = 8;

    public LineOrientedStream(Stream stream, ReadOnlySpan<byte> newLine, int bufferSize = 1024, bool leaveStreamOpen = false) {}

    public int BufferSize { get; }
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public virtual Stream InnerStream { get; }
    public bool IsStrictNewLine { get; }
    public override long Length { get; }
    public ReadOnlySpan<byte> NewLine { get; }
    public override long Position { get; set; }

    public override Task CopyToAsync(Stream destination, int bufferSize = 0, CancellationToken cancellationToken = default) {}
    protected override void Dispose(bool disposing) {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public long Read(Stream targetStream, long length) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public Task<long> ReadAsync(Stream targetStream, long length, CancellationToken cancellationToken = default) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    public override int ReadByte() {}
    public LineOrientedStream.Line? ReadLine() {}
    public byte[] ReadLine(bool keepEOL) {}
    public Task<LineOrientedStream.Line?> ReadLineAsync(CancellationToken cancellationToken = default) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LooseLineOrientedStream : LineOrientedStream {
    public LooseLineOrientedStream(Stream stream, int bufferSize = 1024, bool leaveStreamOpen = false) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class StrictLineOrientedStream : LineOrientedStream {
    public StrictLineOrientedStream(Stream stream, ReadOnlySpan<byte> newLine, int bufferSize = 1024, bool leaveStreamOpen = false) {}
    public StrictLineOrientedStream(Stream stream, int bufferSize = 1024, bool leaveStreamOpen = false) {}
  }
}

