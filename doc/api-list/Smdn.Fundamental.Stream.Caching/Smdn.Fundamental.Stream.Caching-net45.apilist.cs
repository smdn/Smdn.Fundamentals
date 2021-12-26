// Smdn.Fundamental.Stream.Caching.dll (Smdn.Fundamental.Stream.Caching-3.0.0 (net45))
//   Name: Smdn.Fundamental.Stream.Caching
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System.IO;
using Smdn.IO.Streams.Caching;

namespace Smdn.IO.Streams.Caching {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class CachedStreamBase : Stream {
    protected CachedStreamBase(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public int BlockSize { get; }
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public bool LeaveInnerStreamOpen { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    protected abstract byte[] GetBlock(long blockIndex);
    public override int Read(byte[] buffer, int offset, int count) {}
    protected byte[] ReadBlock(long blockIndex) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte @value) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class NonPersistentCachedStream : CachedStreamBase {
    public NonPersistentCachedStream(Stream innerStream) {}
    public NonPersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public NonPersistentCachedStream(Stream innerStream, int blockSize) {}
    public NonPersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public override void Close() {}
    protected override byte[] GetBlock(long blockIndex) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class PersistentCachedStream : CachedStreamBase {
    public PersistentCachedStream(Stream innerStream) {}
    public PersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public PersistentCachedStream(Stream innerStream, int blockSize) {}
    public PersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public override void Close() {}
    protected override byte[] GetBlock(long blockIndex) {}
  }
}

