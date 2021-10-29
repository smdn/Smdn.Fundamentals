// Smdn.Fundamental.Stream.Filtering.dll (Smdn.Fundamental.Stream.Filtering-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.Stream.Filtering
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Smdn.IO.Streams.Filtering;

namespace Smdn.IO.Streams.Filtering {
  // Forwarded to "Smdn.Fundamental.Stream.Filtering, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class FilterStream : Stream {
    public delegate void FilterAction(Span<byte> buffer, long offsetWithinFilter);

    public interface IFilter {
      long Length { get; }
      long Offset { get; }

      void Apply(Span<byte> buffer, long offsetWithinFilter);
    }

    public sealed class BitwiseAndFilter : SingleValueFilter {
      public BitwiseAndFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class BitwiseNotFilter : Filter {
      public BitwiseNotFilter(long offset, long length) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class BitwiseOrFilter : SingleValueFilter {
      public BitwiseOrFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class BitwiseXorFilter : SingleValueFilter {
      public BitwiseXorFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class FillFilter : SingleValueFilter {
      public FillFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public abstract class Filter : IFilter {
      protected Filter(long offset, long length) {}

      public long Length { get; }
      public long Offset { get; }

      public abstract void Apply(Span<byte> buffer, long offsetWithinFilter);
    }

    public abstract class SingleValueFilter : Filter {
      protected SingleValueFilter(long offset, long length, byte @value) {}

      public byte Value { get; }
    }

    public sealed class ZeroFilter : Filter {
      public ZeroFilter(long offset, long length) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    protected const int DefaultBufferSize = 1024;
    protected const bool DefaultLeaveStreamOpen = false;
    protected const int MinimumBufferSize = 2;
    public static readonly FilterStream.IFilter NullFilter; // = "Smdn.IO.Streams.Filtering.FilterStream+_NullFilter"

    public FilterStream(Stream stream, FilterStream.IFilter filter, int bufferSize = 1024, bool leaveStreamOpen = false) {}
    public FilterStream(Stream stream, IEnumerable<FilterStream.IFilter> filters, int bufferSize = 1024, bool leaveStreamOpen = false) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public IReadOnlyList<FilterStream.IFilter> Filters { get; protected set; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public static FilterStream.Filter CreateFilter(long offset, long length, FilterStream.FilterAction filter) {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default) {}
    protected virtual Task<int> ReadAsyncUnchecked(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected virtual int ReadUnchecked(byte[] buffer, int offset, int count) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    protected void ThrowIfDisposed() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }
}

