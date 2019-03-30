// 
// Copyright (c) 2008 smdn <smdn@smdn.jp>
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Smdn.Collections;
using Smdn.IO.Streams.LineOriented;
using Smdn.Text;

namespace Smdn.Formats.Mime {
  public static class MimeUtils {
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead")]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream)
    {
      return ParseHeader(new LooseLineOrientedStream(stream), false);
    }

    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead")]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces)
    {
      return ParseHeader(new LooseLineOrientedStream(stream), keepWhitespaces);
    }

    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead")]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream)
    {
      return ParseHeader(stream, false);
    }

    private static readonly char[] lineDelmiters = new char[] {'\r', '\n'};

    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead")]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces)
    {
      foreach (var header in ParseHeaderRaw(stream)) {
        if (keepWhitespaces) {
          yield return new KeyValuePair<string, string>(header.Name.ToString(), header.Value.ToString());
        }
        else {
          var valueLines = header.Value.ToString().Split(lineDelmiters);

          for (var i = 0; i < valueLines.Length; i++) {
            valueLines[i] = valueLines[i].Trim();
          }

          yield return new KeyValuePair<string, string>(header.Name.Trim().ToString(), string.Concat(valueLines));
        }
      }
    }

    public static Task<IReadOnlyList<KeyValuePair<string, string>>> ParseHeaderAsNameValuePairsAsync(
      LineOrientedStream stream,
      bool keepWhitespaces = false,
      bool ignoreMalformed = true,
      CancellationToken cancellationToken = default
    ) =>
    ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: ParseHeaderAsNameValuePairsConverter,
      arg: keepWhitespaces,
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

    private static KeyValuePair<string, string> ParseHeaderAsNameValuePairsConverter(RawHeaderField header, bool keepWhitespaces)
    {
      if (keepWhitespaces)
        return KeyValuePair.Create(header.NameString, header.ValueString);

      var valueLines = header.ValueString.Split(lineDelmiters);

      for (var i = 0; i < valueLines.Length; i++) {
        valueLines[i] = valueLines[i].Trim();
      }

      return new KeyValuePair<string, string>(header.NameString.Trim(), string.Concat(valueLines));
    }

    [Obsolete("use ParseHeaderAsync() instead")]
    public struct HeaderField
    {
      public ByteString RawData {
        get { return rawData; }
      }

      public ByteString Name {
        get { return ByteString.CreateImmutable(rawData.Segment.Array, 0, indexOfDelmiter); }
      }

      public ByteString Value {
        get { return ByteString.CreateImmutable(rawData.Segment.Array, indexOfDelmiter + 1); }
      }

      public int IndexOfDelimiter {
        get { return indexOfDelmiter; }
      }

      internal HeaderField(Smdn.Text.ByteString rawData, int indexOfDelimiter)
      {
        this.rawData = rawData;
        this.indexOfDelmiter = indexOfDelimiter;
      }

      internal static HeaderField FromRawHeaderField(RawHeaderField rawHeaderField)
        => new HeaderField(ByteString.CreateImmutable(rawHeaderField.HeaderFieldSequence.ToArray()), rawHeaderField.OffsetOfDelimiter);

      private readonly ByteString rawData;
      private readonly int indexOfDelmiter;
    }

    [Obsolete("use ParseHeaderAsync() instead")]
    public static IEnumerable<HeaderField> ParseHeaderRaw(LineOrientedStream stream)
    => ParseHeaderAsync(
      stream: stream,
      converter: HeaderField.FromRawHeaderField,
      ignoreMalformed: true,
      cancellationToken: default
    ).GetAwaiter().GetResult();

    public static Task<IReadOnlyList<RawHeaderField>> ParseHeaderAsync(
      LineOrientedStream stream,
      bool ignoreMalformed = true,
      CancellationToken cancellationToken = default
    ) =>
    ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: ParseHeaderNullConverter,
      arg: default(int),
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

    private static RawHeaderField ParseHeaderNullConverter(RawHeaderField f, int _) => f;

    public static /*IAsyncEnumerable<T>*/ Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField>(
      LineOrientedStream stream,
      Converter<RawHeaderField, THeaderField> converter,
      bool ignoreMalformed = true,
      CancellationToken cancellationToken = default
    ) =>
    ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: ParseHeaderConverter,
      arg: converter ?? throw new ArgumentNullException(nameof(converter)),
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

    private static THeaderField ParseHeaderConverter<THeaderField>(RawHeaderField header, Converter<RawHeaderField, THeaderField> converter)
      => converter(header);

    private static /*IAsyncEnumerable<T>*/ async Task<IReadOnlyList<THeaderField>> ParseHeaderAsyncCore<THeaderField, TArg>(
      LineOrientedStream stream,
      Func<RawHeaderField, TArg, THeaderField> converter,
      TArg arg,
      bool ignoreMalformed = true,
      CancellationToken cancellationToken = default
    )
    {
      var headerFields = new List<THeaderField>();
      HeaderFieldLineSegment lineFirst = null;
      HeaderFieldLineSegment lineLast = null;
      var offsetOfDelimiter = -1;

      for (;;) {
        var ret = await stream.ReadLineAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        if (ret.IsEndOfStream)
          break;
        if (ret.IsEmptyLine)
          break;

        var line = ret.LineWithNewLine;
        var firstByteOfLine = line.First.Span[0];

        if (firstByteOfLine == Ascii.Octets.HT || firstByteOfLine == Ascii.Octets.SP) { // LWSP-char
          // folding
          if (lineFirst == null) {
            if (ignoreMalformed)
              continue;
            throw new InvalidDataException($"malformed header field: '{ByteString.ToString(null, ret.Line)}'");
          }

          lineLast = HeaderFieldLineSegment.Append(lineLast, line, out _);
        }
        else {
          if (lineFirst != null)
            headerFields.Add(Result());

          // field       =  field-name ":" [ field-body ] CRLF
          // field-name  =  1*<any CHAR, excluding CTLs, SPACE, and ":">
          const byte nameBodyDelimiter = (byte)':';

          var posOfDelim = line.PositionOf(nameBodyDelimiter);

          if (posOfDelim.HasValue)
            offsetOfDelimiter = (int)line.Slice(0, posOfDelim.Value).Length;
          else
            offsetOfDelimiter = -1;

          if (0 < offsetOfDelimiter)
            lineLast = HeaderFieldLineSegment.Append(null, line, out lineFirst);
          else if (ignoreMalformed)
            lineFirst = null;
          else
            throw new InvalidDataException($"malformed header field: '{ByteString.ToString(null, ret.Line)}'");
        }
      }

      if (lineFirst != null)
        headerFields.Add(Result());

      return headerFields;

      THeaderField Result()
      {
        var ret = new RawHeaderField(
          new ReadOnlySequence<byte>(lineFirst, 0, lineLast, lineLast.Memory.Length),
          offsetOfDelimiter
        );

        lineFirst = null;

        return converter(ret, arg);
      }
    }

    private class HeaderFieldLineSegment : ReadOnlySequenceSegment<byte> {
      public static HeaderFieldLineSegment Append(HeaderFieldLineSegment last, ReadOnlySequence<byte> line, out HeaderFieldLineSegment first)
      {
        first = null;

        var position = line.Start;

        while (line.TryGet(ref position, out var memory, advance: true)) {
          last = new HeaderFieldLineSegment(last, memory);

          if (first == null)
            first = last;
        }

        return last;
      }

      private HeaderFieldLineSegment(HeaderFieldLineSegment prev, ReadOnlyMemory<byte> memory)
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

    /// <param name="val">header field value.</param>
    public static string RemoveHeaderWhiteSpaceAndComment(string val)
    {
      /*
       * RFC 5322 - Internet Message Format
       * http://tools.ietf.org/html/rfc5322
       * 3.2.2. Folding White Space and Comments
       * 
       *    FWS             =   ([*WSP CRLF] 1*WSP) /  obs-FWS
       *                                           ; Folding white space
       *    ctext           =   %d33-39 /          ; Printable US-ASCII
       *                        %d42-91 /          ;  characters not including
       *                        %d93-126 /         ;  "(", ")", or "\"
       *                        obs-ctext
       *    ccontent        =   ctext / quoted-pair / comment
       *    comment         =   "(" *([FWS] ccontent) [FWS] ")"
       *    CFWS            =   (1*([FWS] comment) [FWS]) / FWS
       * 
       *    quoted-pair     =   ("\" (VCHAR / WSP)) / obs-qp
       */
      if (string.IsNullOrEmpty(val))
        return val;

      var ret = new StringBuilder(val.Length);
      var fws = 0;
      var nest = 0;

      for (var index = 0; index < val.Length; index++) {
        var ch = val[index];

        switch (ch) {
          case '(':
            nest++;
            break;

          case ')':
            nest = Math.Max(0, nest - 1);

            if (nest == 0)
              fws = 0;

            break;

          case '\\':
            index++;

            if (nest == 0) {
              if (0 < fws)
                ret.Append(' ');

              ret.Append(ch);

              if (index < val.Length)
                ret.Append(val[index]);

              fws = 0;
            }

            break;

          case ' ':
          case '\t':
          case '\r':
          case '\n':
            fws++;
            break;

          default:
            if (nest == 0) {
              if (0 < fws)
                ret.Append(' ');

              ret.Append(ch);

              fws = 0;
            }

            break;
        }
      }

      return ret.ToString();
    }
  }
}
