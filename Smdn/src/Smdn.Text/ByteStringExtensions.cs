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

    public static unsafe bool StartsWith(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
    {
      if (value.Length == 0)
        return true;
      if (sequence.Length < value.Length)
        return false;

      fixed (byte* substr0 = value) {
        byte* substr = substr0;
        byte* substrEnd = substr0 + value.Length;

        var pos = sequence.Start;

        while (sequence.TryGet(ref pos, out var memory, advance: true)) {
          if (memory.Length == 0)
            continue; // XXX: never happen?

          fixed (byte* str0 = memory.Span) {
            for (byte* str = str0, strEnd = str0 + memory.Length; ;) {
              if (*str != *substr)
                return false;
              if (++substr == substrEnd)
                return true;
              if (++str == strEnd)
                break;
            }
          }
        }
      }

      return false;
    }
  }
}
