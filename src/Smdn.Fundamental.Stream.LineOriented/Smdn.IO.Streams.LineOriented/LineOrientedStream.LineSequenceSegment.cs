// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;

namespace Smdn.IO.Streams.LineOriented;

partial class LineOrientedStream {
  private sealed class LineSequenceSegment : ReadOnlySequenceSegment<byte> {
    public LineSequenceSegment(LineSequenceSegment? prev, ReadOnlyMemory<byte> memory)
    {
      Memory = memory;

      if (prev == null) {
        RunningIndex = 0;
      }
      else {
        RunningIndex = prev.RunningIndex + prev.Memory.Length;
        prev.Next = this;
      }
    }
  }
}
