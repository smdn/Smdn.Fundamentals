// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CA1034
#pragma warning disable CA1815

using System;
using System.Buffers;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  public readonly struct Line {
    public ReadOnlySequence<byte> SequenceWithNewLine { get; }
    public SequencePosition PositionOfNewLine { get; }

    public ReadOnlySequence<byte> Sequence => SequenceWithNewLine.Slice(0, PositionOfNewLine);
    public ReadOnlySequence<byte> NewLine => SequenceWithNewLine.Slice(PositionOfNewLine);
    public bool IsEmpty => SequenceWithNewLine.IsEmpty || PositionOfNewLine.Equals(SequenceWithNewLine.Start);

    public Line(ReadOnlySequence<byte> sequenceWithNewLine, SequencePosition positionOfNewLine)
    {
      SequenceWithNewLine = sequenceWithNewLine;
      PositionOfNewLine = positionOfNewLine;
    }

    internal ReadOnlySequence<byte> GetLine(bool keepEOL) => keepEOL ? SequenceWithNewLine : Sequence;
  }
}
