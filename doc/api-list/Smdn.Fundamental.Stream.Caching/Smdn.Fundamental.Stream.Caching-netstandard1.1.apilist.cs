// Smdn.Fundamental.Stream.Caching.dll (Smdn.Fundamental.Stream.Caching-3.0.1)
//   Name: Smdn.Fundamental.Stream.Caching
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+5d6c4605b04017bfcf25bb41821556a4a8af61d6
//   TargetFramework: .NETStandard,Version=v1.1
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

    protected override void Dispose(bool disposing) {}
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

    protected override void Dispose(bool disposing) {}
    protected override byte[] GetBlock(long blockIndex) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class PersistentCachedStream : CachedStreamBase {
    public PersistentCachedStream(Stream innerStream) {}
    public PersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public PersistentCachedStream(Stream innerStream, int blockSize) {}
    public PersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    protected override void Dispose(bool disposing) {}
    protected override byte[] GetBlock(long blockIndex) {}
  }
}
