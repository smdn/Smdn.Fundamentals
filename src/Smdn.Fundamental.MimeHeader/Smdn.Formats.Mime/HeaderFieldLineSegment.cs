// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;

namespace Smdn.Formats.Mime;

internal class HeaderFieldLineSegment : ReadOnlySequenceSegment<byte> {
  public static HeaderFieldLineSegment? Append(
    HeaderFieldLineSegment? last,
    ReadOnlySequence<byte> line,
    out HeaderFieldLineSegment? first
  )
  {
    first = null;

    var position = line.Start;

    while (line.TryGet(ref position, out var memory, advance: true)) {
      last = new HeaderFieldLineSegment(last, memory);

      first ??= last;
    }

    return last;
  }

  private HeaderFieldLineSegment(
    HeaderFieldLineSegment? prev,
    ReadOnlyMemory<byte> memory
  )
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
