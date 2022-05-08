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
    => ReadLineAsync().GetAwaiter().GetResult();

  public byte[] ReadLine(bool keepEOL)
    => ReadLine()?.GetLine(keepEOL)?.ToArray();

  public Task<Line?> ReadLineAsync(CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    return ReadLineAsyncCore(cancellationToken: cancellationToken);
  }

  private enum EolState {
    NotMatched = 0,
    NewLine,
    CR,
    LF,
  }

  private async Task<Line?> ReadLineAsyncCore(
    CancellationToken cancellationToken
  )
  {
    if (bufRemain == 0 && await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0)
      return null;

    const byte CR = 0x0d;
    const byte LF = 0x0a;
    var newLineLength = 0;
    var bufCopyFrom = bufOffset;
    var eol = EolState.NotMatched;
    var eos = false;
    var retCount = 0;
    LineSequenceSegment segmentHead = null;
    LineSequenceSegment segmentTail = null;

    for (; ; ) {
      if (newLine is null) {
        // loose EOL (CR/LF/CRLF)
        if (buffer[bufOffset] == CR) {
          eol = EolState.CR;
          newLineLength = 1;
        }
        else if (buffer[bufOffset] == LF) {
          eol = EolState.LF;
          newLineLength = 1;
        }
      }
      else {
        // strict EOL
        if (buffer[bufOffset] == newLine[newLineLength]) {
          if (newLine.Length == ++newLineLength)
            eol = EolState.NewLine;
        }
        else {
          newLineLength = 0;
        }
      }

      bufRemain--;
      bufOffset++;

      if (
        bufRemain == 0 &&
        (eol == EolState.NotMatched || eol == EolState.CR /* read ahead; CRLF */)
      ) {
        var count = bufOffset - bufCopyFrom;

        segmentTail = new(
          segmentTail,
          buffer.AsSpan(bufCopyFrom, count).ToArray() // TODO: allocation
        );
        segmentHead ??= segmentTail;

        retCount += count;

        eos = await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0;

        bufCopyFrom = bufOffset;
      }

      if (eol != EolState.NotMatched || eos)
        break;
    }

    var retLength = retCount + (bufOffset - bufCopyFrom);

    if (eol == EolState.CR && buffer[bufOffset] == LF) {
      // CRLF
      retLength++;
      newLineLength++;

      bufOffset++;
      bufRemain--;
    }

    if (eol == EolState.NotMatched)
      newLineLength = 0;

    if (segmentHead is null || 0 < retLength - retCount) {
      segmentTail = new(
        segmentTail,
        buffer.AsSpan(bufCopyFrom, retLength - retCount).ToArray() // TODO: allocation
      );
      segmentHead ??= segmentTail;
    }

    var sequenceWithNewLine = new ReadOnlySequence<byte>(
      segmentHead,
      0,
      segmentTail,
      segmentTail.Memory.Length
    ).Slice(0, retLength);

    return new Line(
      sequenceWithNewLine: sequenceWithNewLine,
      positionOfNewLine: sequenceWithNewLine.GetPosition(retLength - newLineLength)
    );
  }
}
