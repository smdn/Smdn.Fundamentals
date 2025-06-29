// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore LWSP,ctext,ccontent,CFWS,VCHAR
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Smdn.Buffers;
using Smdn.IO.Streams.LineOriented;

namespace Smdn.Formats.Mime;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class MimeUtils {
  private static readonly char[] LineDelimiters = new char[] { '\r', '\n' };

  public static Task<IReadOnlyList<KeyValuePair<string, string>>> ParseHeaderAsNameValuePairsAsync(
    LineOrientedStream stream,
    bool keepWhitespaces = false,
    bool ignoreMalformed = true,
    CancellationToken cancellationToken = default
  )
    => ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: ParseHeaderAsNameValuePairsConverter,
      arg: keepWhitespaces,
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

  private static KeyValuePair<string, string> ParseHeaderAsNameValuePairsConverter(
    RawHeaderField header,
    bool keepWhitespaces
  )
  {
    if (keepWhitespaces)
#if SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
      return KeyValuePair.Create(header.NameString, header.ValueString);
#else
      return new KeyValuePair<string, string>(header.NameString, header.ValueString);
#endif

    var valueLines = header.ValueString.Split(LineDelimiters);

    for (var i = 0; i < valueLines.Length; i++) {
      valueLines[i] = valueLines[i].Trim();
    }

#if SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
    return KeyValuePair.Create(header.NameString.Trim(), string.Concat(valueLines));
#else
    return new KeyValuePair<string, string>(header.NameString.Trim(), string.Concat(valueLines));
#endif
  }

  public static Task<IReadOnlyList<RawHeaderField>> ParseHeaderAsync(
    LineOrientedStream stream,
    bool ignoreMalformed = true,
    CancellationToken cancellationToken = default
  )
    => ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: ParseHeaderNullConverter,
      arg: default(int),
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

  private static RawHeaderField ParseHeaderNullConverter(RawHeaderField f, int discard) => f;

  public static /*IAsyncEnumerable<T>*/ Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField>(
    LineOrientedStream stream,
#if SYSTEM_CONVERTER
    Converter<RawHeaderField, THeaderField> converter,
#else
    Func<RawHeaderField, THeaderField> converter,
#endif
    bool ignoreMalformed = true,
    CancellationToken cancellationToken = default
  )
    => ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: ParseHeaderConverter,
      arg: converter ?? throw new ArgumentNullException(nameof(converter)),
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

  private static THeaderField ParseHeaderConverter<THeaderField>(
    RawHeaderField header,
#if SYSTEM_CONVERTER
    Converter<RawHeaderField, THeaderField> converter
#else
    Func<RawHeaderField, THeaderField> converter
#endif
  )
    => converter(header);

  public static
  Task<IReadOnlyList<THeaderField>> // IAsyncEnumerable<T>
  ParseHeaderAsync<THeaderField, TArg>(
    LineOrientedStream stream,
    Func<RawHeaderField, TArg, THeaderField> converter,
    TArg arg,
    bool ignoreMalformed = true,
    CancellationToken cancellationToken = default
  )
    => ParseHeaderAsyncCore(
      stream: stream ?? throw new ArgumentNullException(nameof(stream)),
      converter: converter ?? throw new ArgumentNullException(nameof(converter)),
      arg: arg,
      ignoreMalformed: ignoreMalformed,
      cancellationToken: cancellationToken
    );

  private static
  async Task<IReadOnlyList<THeaderField>> // IAsyncEnumerable<T>
  ParseHeaderAsyncCore<THeaderField, TArg>(
    LineOrientedStream stream,
    Func<RawHeaderField, TArg, THeaderField> converter,
    TArg arg,
    bool ignoreMalformed = true,
    CancellationToken cancellationToken = default
  )
  {
    const byte HT = (byte)'\t';
    const byte SP = (byte)' ';
    var headerFields = new List<THeaderField>();
    HeaderFieldLineSegment? lineFirst = null;
    HeaderFieldLineSegment? lineLast = null;
    var offsetOfDelimiter = -1;

    for (; ; ) {
      var ret = await stream.ReadLineAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

      if (ret == null)
        break;
      if (ret.Value.IsEmpty)
        break;

      var line = ret.Value.SequenceWithNewLine;
      var firstByteOfLine = line
#if SYSTEM_BUFFERS_READONLYSEQUENCE_FIRSTSPAN
        .FirstSpan[0];
#else
        .First.Span[0];
#endif

      if (firstByteOfLine is HT or SP) { // LWSP-char
        // folding
        if (lineFirst == null) {
          if (ignoreMalformed)
            continue;
          throw new InvalidDataException($"malformed header field: '{ret.Value.Sequence.CreateString()}'");
        }

        lineLast = HeaderFieldLineSegment.Append(lineLast, line, out _);

        continue;
      }

      if (TryGetHeaderField(ref lineFirst, lineLast, offsetOfDelimiter, out var rawHeaderField))
        headerFields.Add(converter(rawHeaderField, arg));

      // field       =  field-name ":" [ field-body ] CRLF
      // field-name  =  1*<any CHAR, excluding CTLs, SPACE, and ":">
      const byte NameBodyDelimiter = (byte)':';

      var posOfDelim = line.PositionOf(NameBodyDelimiter);

      offsetOfDelimiter = posOfDelim.HasValue ? (int)line.Slice(0, posOfDelim.Value).Length : -1;

      if (0 < offsetOfDelimiter) {
        lineLast = HeaderFieldLineSegment.Append(null, line, out lineFirst);
      }
      else {
        lineFirst = ignoreMalformed
          ? null
          : throw new InvalidDataException($"malformed header field: '{ret.Value.Sequence.CreateString()}'");
      }
    }

    if (TryGetHeaderField(ref lineFirst, lineLast, offsetOfDelimiter, out var rawHeaderFieldFinal))
      headerFields.Add(converter(rawHeaderFieldFinal, arg));

    return headerFields;

    static bool TryGetHeaderField(
      ref HeaderFieldLineSegment? first,
      HeaderFieldLineSegment? last,
      int offsetOfDelimiter,
      out RawHeaderField rawHeaderField
    )
    {
      rawHeaderField = default;

      if (first is null || last is null)
        return false;

      rawHeaderField = new(
        new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length),
        offsetOfDelimiter
      );

      first = null;

      return true;
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
