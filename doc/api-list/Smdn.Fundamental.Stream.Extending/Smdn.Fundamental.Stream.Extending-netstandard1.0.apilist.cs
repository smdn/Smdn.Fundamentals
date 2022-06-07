// Smdn.Fundamental.Stream.Extending.dll (Smdn.Fundamental.Stream.Extending-3.0.3)
//   Name: Smdn.Fundamental.Stream.Extending
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+62b030d52118749c63983987fb6ae99a47e81e56
//   TargetFramework: .NETStandard,Version=v1.0
//   Configuration: Release

using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Smdn.IO.Streams.Extending;

namespace Smdn.IO.Streams.Extending {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ExtendStream : ExtendStreamBase {
    [NullableContext(2)]
    public ExtendStream([Nullable(1)] Stream innerStream, Stream prependStream, Stream appendStream, bool leaveInnerStreamOpen = true, bool leavePrependStreamOpen = true, bool leaveAppendStreamOpen = true) {}
    [NullableContext(2)]
    public ExtendStream([Nullable(1)] Stream innerStream, byte[] prependData, byte[] appendData, bool leaveInnerStreamOpen = true) {}

    protected override bool CanSeekAppendedData { get; }
    protected override bool CanSeekPrependedData { get; }

    protected override void Dispose(bool disposing) {}
    protected override int ReadAppendedData(byte[] buffer, int offset, int count) {}
    protected override Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected override int ReadPrependedData(byte[] buffer, int offset, int count) {}
    protected override Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected override void SetAppendedDataPosition(long position) {}
    protected override void SetPrependedDataPosition(long position) {}
  }

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class ExtendStreamBase : Stream {
    [NullableContext(byte.MinValue)]
    protected enum StreamSection : int {
      Append = 2,
      EndOfStream = 3,
      Prepend = 0,
      Stream = 1,
    }

    protected ExtendStreamBase(Stream innerStream, long prependLength, long appendLength, bool leaveInnerStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    protected abstract bool CanSeekAppendedData { get; }
    protected abstract bool CanSeekPrependedData { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public bool LeaveInnerStreamOpen { get; }
    public override long Length { get; }
    public override long Position { get; set; }
    protected ExtendStreamBase.StreamSection Section { get; }

    protected override void Dispose(bool disposing) {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    protected abstract int ReadAppendedData(byte[] buffer, int offset, int count);
    protected abstract Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected abstract int ReadPrependedData(byte[] buffer, int offset, int count);
    protected abstract Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    public override long Seek(long offset, SeekOrigin origin) {}
    protected abstract void SetAppendedDataPosition(long position);
    public override void SetLength(long @value) {}
    protected abstract void SetPrependedDataPosition(long position);
    protected void ThrowIfDisposed() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }
}

