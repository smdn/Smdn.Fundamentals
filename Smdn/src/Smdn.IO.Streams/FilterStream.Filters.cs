// 
// Copyright (c) 2020 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;

namespace Smdn.IO.Streams {
  public partial class FilterStream : Stream {
    public interface IFilter {
      long Offset { get; }
      long Length { get; }
      void Apply(Span<byte> buffer, long offsetWithinFilter);
    }

    private class _NullFilter : IFilter {
      public long Offset => throw new NotSupportedException();
      public long Length => throw new NotSupportedException();
      public void Apply(Span<byte> buffer, long offsetWithinFilter) => throw new NotSupportedException();
    }

    public static readonly IFilter NullFilter = new _NullFilter();

    public abstract class Filter : IFilter {
      public long Offset { get; }
      public long Length { get; }

      protected Filter(long offset, long length)
      {
        this.Offset = offset;
        this.Length = 0 <= length ? length : throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(length), length);
      }

      public abstract void Apply(Span<byte> buffer, long offsetWithinFilter);
    }

    public delegate void FilterAction(Span<byte> buffer, long offsetWithinFilter);

    private class DelegatedFilter : Filter {
      private readonly FilterAction filter;

      public DelegatedFilter(long offset, long length, FilterAction filter)
        : base(offset, length)
      {
        this.filter = filter;
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
        => filter(buffer, offsetWithinFilter);
    }

    public static Filter CreateFilter(long offset, long length, FilterAction filter)
      => new DelegatedFilter(offset, length, filter ?? throw new ArgumentNullException(nameof(filter)));

    public abstract class SingleValueFilter : Filter {
      public byte Value { get; }

      protected SingleValueFilter(long offset, long length, byte value)
        : base(offset, length)
      {
        this.Value = value;
      }
    }

    public sealed class FillFilter : SingleValueFilter {
      public FillFilter(long offset, long length, byte value)
        : base(offset, length, value)
      {
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
        => buffer.Fill(Value);
    }

    public sealed class ZeroFilter : Filter {
      public ZeroFilter(long offset, long length)
        : base(offset, length)
      {
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
        => buffer.Clear();
    }

    public sealed class BitwiseNotFilter : Filter {
      public BitwiseNotFilter(long offset, long length)
        : base(offset, length)
      {
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
      {
        for (var index = 0; index < buffer.Length; index++) {
          buffer[index] = (byte)~buffer[index];
        }
      }
    }

    public sealed class BitwiseOrFilter : SingleValueFilter {
      public BitwiseOrFilter(long offset, long length, byte value)
        : base(offset, length, value)
      {
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
      {
        for (var index = 0; index < buffer.Length; index++) {
          buffer[index] |= Value;
        }
      }
    }

    public sealed class BitwiseAndFilter : SingleValueFilter {
      public BitwiseAndFilter(long offset, long length, byte value)
        : base(offset, length, value)
      {
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
      {
        for (var index = 0; index < buffer.Length; index++) {
          buffer[index] &= Value;
        }
      }
    }

    public sealed class BitwiseXorFilter : SingleValueFilter {
      public BitwiseXorFilter(long offset, long length, byte value)
        : base(offset, length, value)
      {
      }

      public override void Apply(Span<byte> buffer, long offsetWithinFilter)
      {
        for (var index = 0; index < buffer.Length; index++) {
          buffer[index] ^= Value;
        }
      }
    }
  }
}
