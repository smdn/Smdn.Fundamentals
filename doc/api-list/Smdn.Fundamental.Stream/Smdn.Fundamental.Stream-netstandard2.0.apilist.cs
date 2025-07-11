// Smdn.Fundamental.Stream.dll (Smdn.Fundamental.Stream-3.0.4)
//   Name: Smdn.Fundamental.Stream
//   AssemblyVersion: 3.0.4.0
//   InformationalVersion: 3.0.4+50cd3a5ddb6026e07a1bf790427b237a96c07bb8
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Smdn.IO.Streams;

namespace Smdn.IO {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class StreamExtensions {
    public static void CopyTo(this Stream stream, BinaryWriter writer, int bufferSize = 10240) {}
    public static Task CopyToAsync(this Stream stream, BinaryWriter writer, int bufferSize = 10240, CancellationToken cancellationToken = default) {}
    public static byte[] ReadToEnd(this Stream stream, int readBufferSize = 4096, int initialCapacity = 4096) {}
    public static Task<byte[]> ReadToEndAsync(this Stream stream, int readBufferSize = 4096, int initialCapacity = 4096, CancellationToken cancellationToken = default) {}
    public static void Write(this Stream stream, ArraySegment<byte> segment) {}
    public static void Write(this Stream stream, ReadOnlySequence<byte> sequence) {}
    public static Task WriteAsync(this Stream stream, ReadOnlySequence<byte> sequence, CancellationToken cancellationToken = default) {}
  }
}

namespace Smdn.IO.Streams {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ChunkedMemoryStream : Stream {
    public delegate ChunkedMemoryStream.Chunk Allocator(int chunkSize);

    public abstract class Chunk : IDisposable {
      public byte[] Data;

      protected Chunk() {}

      public abstract void Dispose();
    }

    public static readonly int DefaultChunkSize = 40960;

    public ChunkedMemoryStream() {}
    public ChunkedMemoryStream(ChunkedMemoryStream.Allocator allocator) {}
    public ChunkedMemoryStream(int chunkSize) {}
    public ChunkedMemoryStream(int chunkSize, ChunkedMemoryStream.Allocator allocator) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public int ChunkSize { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public byte[] ToArray() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte @value) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class NonClosingStream : Stream {
    public NonClosingStream(Stream innerStream) {}
    public NonClosingStream(Stream innerStream, bool writable) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class PartialStream :
    Stream,
    ICloneable
  {
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length) {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length, bool seekToBegin) {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length) {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length, bool seekToBegin) {}

    public PartialStream(Stream innerStream, long offset) {}
    public PartialStream(Stream innerStream, long offset, bool @readonly, bool leaveInnerStreamOpen) {}
    public PartialStream(Stream innerStream, long offset, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin) {}
    public PartialStream(Stream innerStream, long offset, bool leaveInnerStreamOpen) {}
    public PartialStream(Stream innerStream, long offset, long length) {}
    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen) {}
    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin) {}
    public PartialStream(Stream innerStream, long offset, long length, bool leaveInnerStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public bool LeaveInnerStreamOpen { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public PartialStream Clone() {}
    public override void Close() {}
    public override void Flush() {}
    protected long GetRemainderLength() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    object ICloneable.Clone() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.6.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.4.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
