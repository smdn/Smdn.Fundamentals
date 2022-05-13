// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  public Line? ReadLine()
  {
    ThrowIfDisposed();

    return ReadToEndOfLine();
  }

  public byte[] ReadLine(bool keepEOL)
    => ReadLine()?.GetLine(keepEOL)?.ToArray();

  public Task<Line?> ReadLineAsync(CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    return ReadToEndOfLineAsync(cancellationToken);
  }

  public Task<ReadOnlySequence<byte>?> ReadLineAsync(
    bool keepEOL,
    CancellationToken cancellationToken = default
  )
  {
    ThrowIfDisposed();

    return ReadLineAsyncCore();

    async Task<ReadOnlySequence<byte>?> ReadLineAsyncCore()
    {
      var line = await ReadToEndOfLineAsync(cancellationToken).ConfigureAwait(false);

      return line.HasValue
        ? line.Value.GetLine(keepEOL: keepEOL)
        : null;
    }
  }

  private const byte CR = 0x0D;
  private const byte LF = 0x0A;

  private enum LineTerminator {
    None = 0,
    NewLine,
    CR,
    LF,
    CRLF,
  }

  private Line? ReadToEndOfLine()
  {
    if (bufRemain == 0 && FillBuffer() <= 0)
      return default;

    var bufOffsetReadStart = bufOffset;
    LineTerminator lineTerminator;
    LineSequenceSegment lineSegmentHead = null;
    LineSequenceSegment lineSegmentTail = null;
    var testedOffsetOfLineTerminator = 0;

    for (; ; ) {
      int offsetAdvanced;

      if (newLine is null) {
        AdvanceToLineTerminatorAny(
          buffer.AsSpan(bufOffset, bufRemain),
          out lineTerminator,
          out offsetAdvanced
        );
      }
      else {
        AdvanceToLineTerminator(
          buffer.AsSpan(bufOffset, bufRemain),
          newLine.AsSpan(),
          ref testedOffsetOfLineTerminator,
          out lineTerminator,
          out offsetAdvanced
        );
      }

      bufRemain -= offsetAdvanced;
      bufOffset += offsetAdvanced;

      // fill buffer if not have been reached to any line terminater or have been reached to CR (to test CRLF sequence)
      if (
        bufRemain == 0 &&
        (lineTerminator == LineTerminator.None || lineTerminator == LineTerminator.CR)
      ) {
        AppendLineSequence(
          ref lineSegmentHead,
          ref lineSegmentTail,
          buffer.AsSpan(bufOffsetReadStart, bufOffset - bufOffsetReadStart)
        );

        var reachedToEndOfStream = FillBuffer() <= 0;

        bufOffsetReadStart = bufOffset;

        if (reachedToEndOfStream)
          break;
      }

      if (lineTerminator != LineTerminator.None)
        break;
    }

    return PostProcessReadToEndOfLine(
      lineTerminator,
      lineSegmentHead,
      lineSegmentTail,
      bufOffsetReadStart
    );
  }

  private async Task<Line?> ReadToEndOfLineAsync(
    CancellationToken cancellationToken
  )
  {
    if (bufRemain == 0 && await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0)
      return default;

    var bufOffsetReadStart = bufOffset;
    LineTerminator lineTerminator;
    LineSequenceSegment lineSegmentHead = null;
    LineSequenceSegment lineSegmentTail = null;
    var testedOffsetOfLineTerminator = 0;

    for (; ; ) {
      int offsetAdvanced;

      if (newLine is null) {
        AdvanceToLineTerminatorAny(
          buffer.AsSpan(bufOffset, bufRemain),
          out lineTerminator,
          out offsetAdvanced
        );
      }
      else {
        AdvanceToLineTerminator(
          buffer.AsSpan(bufOffset, bufRemain),
          newLine.AsSpan(),
          ref testedOffsetOfLineTerminator,
          out lineTerminator,
          out offsetAdvanced
        );
      }

      bufRemain -= offsetAdvanced;
      bufOffset += offsetAdvanced;

      // fill buffer if not have been reached to any line terminater or have been reached to CR (to test CRLF sequence)
      if (
        bufRemain == 0 &&
        (lineTerminator == LineTerminator.None || lineTerminator == LineTerminator.CR)
      ) {
        AppendLineSequence(
          ref lineSegmentHead,
          ref lineSegmentTail,
          buffer.AsSpan(bufOffsetReadStart, bufOffset - bufOffsetReadStart)
        );

        var reachedToEndOfStream = await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0;

        bufOffsetReadStart = bufOffset;

        if (reachedToEndOfStream)
          break;
      }

      if (lineTerminator != LineTerminator.None)
        break;
    }

    return PostProcessReadToEndOfLine(
      lineTerminator,
      lineSegmentHead,
      lineSegmentTail,
      bufOffsetReadStart
    );
  }

  private Line? PostProcessReadToEndOfLine(
    LineTerminator lineTerminator,
    LineSequenceSegment lineSegmentHead,
    LineSequenceSegment lineSegmentTail,
    int bufOffsetReadStart
  )
  {
    // read ahead to test CRLF sequece
    if (lineTerminator == LineTerminator.CR && buffer[bufOffset] == LF) {
      // line terminator is sequence of CRLF
      lineTerminator = LineTerminator.CRLF;

      // consume LF
      bufOffset++;
      bufRemain--;
    }

    if (lineSegmentHead is null || bufOffsetReadStart < bufOffset) {
      AppendLineSequence(
        ref lineSegmentHead,
        ref lineSegmentTail,
        buffer.AsSpan(bufOffsetReadStart, bufOffset - bufOffsetReadStart)
      );
    }

    if (lineSegmentHead is null)
      return null;

    var lineSequence = new ReadOnlySequence<byte>(
      lineSegmentHead,
      0,
      lineSegmentTail,
      lineSegmentTail.Memory.Length
    );
    var lengthOfLineTerminator = lineTerminator switch {
      LineTerminator.CR or LineTerminator.LF => 1,
      LineTerminator.CRLF => 2,
      LineTerminator.NewLine => newLine.Length,
      _ => 0,
    };

    return new(
      sequenceWithNewLine: lineSequence,
      positionOfNewLine: lineSequence.GetPosition(
#pragma warning disable SA1114
#if SYSTEM_BUFFERS_READONLYSEQUENCE_GETOFFSET
        lineSequence.GetOffset(lineSequence.End) - lengthOfLineTerminator
#else
        lineSequence.Length - lengthOfLineTerminator
#endif
#pragma warning restore SA1114
      )
    );
  }

  private static void AdvanceToLineTerminator(
    ReadOnlySpan<byte> buffer, // must be non-empty
    ReadOnlySpan<byte> expectedLineTerminator, // must be non-empty
    ref int testedOffsetOfLineTerminator,
    out LineTerminator terminator,
    out int offsetAdvanced
  )
  {
    offsetAdvanced = 0;

    for (; ; ) {
      terminator = LineTerminator.None;

      if (buffer[offsetAdvanced++] == expectedLineTerminator[testedOffsetOfLineTerminator]) {
        if (expectedLineTerminator.Length == ++testedOffsetOfLineTerminator) {
          terminator = LineTerminator.NewLine;
          return; // reached to end of line
        }
      }
      else {
        testedOffsetOfLineTerminator = 0;
      }

      if (offsetAdvanced == buffer.Length)
        return; // reached to end of buffer
    }
  }

  private static void AdvanceToLineTerminatorAny(
    ReadOnlySpan<byte> buffer, // must be non-empty
    out LineTerminator terminator,
    out int offsetAdvanced
  )
  {
    offsetAdvanced = 0;

    for (; ; ) {
      switch (buffer[offsetAdvanced++]) {
        case CR:
          terminator = LineTerminator.CR; // CR (including the case for the first byte of CRLF sequence)
          return; // reached to end of line
        case LF:
          terminator = LineTerminator.LF; // LF
          return; // reached to end of line
        default:
          terminator = LineTerminator.None;
          break;
      }

      if (offsetAdvanced == buffer.Length)
        return; // reached to end of buffer
    }
  }

  private static void AppendLineSequence(
    ref LineSequenceSegment head,
    ref LineSequenceSegment tail,
    ReadOnlySpan<byte> lineSequence
  )
  {
    tail = new(
      tail,
      lineSequence.ToArray() // TODO: allocation
    );
    head ??= tail;
  }
}
