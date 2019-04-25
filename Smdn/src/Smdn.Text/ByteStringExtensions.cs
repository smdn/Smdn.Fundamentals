// 
// Copyright (c) 2019 smdn <smdn@smdn.jp>
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
using System.Buffers;

namespace Smdn.Text {
  public static class ByteStringExtensions {
    public static ReadOnlySequence<byte> AsReadOnlySequence(this ByteString str)
    {
      return new ReadOnlySequence<byte>(str.Segment.AsMemory());
    }

    public static bool SequenceEqual(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
    {
      if (sequence.Length != value.Length)
        return false;

      var offset = 0;
      var pos = sequence.Start;

      while (sequence.TryGet(ref pos, out var memory, advance: true)) {
        if (memory.Length == 0)
          continue; // XXX: never happen?

        if (!value.Slice(offset, memory.Length).SequenceEqual(memory.Span))
          return false;

        offset += memory.Length;
      }

      return true;
    }

    public static bool StartsWith(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
    {
      if (value.Length == 0)
        return true;
      if (sequence.Length < value.Length)
        return false;

      return SequenceEqual(sequence.Slice(0, value.Length), value);
    }
  }
}
