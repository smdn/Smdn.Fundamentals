// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2008-2017 smdn
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
using System.Collections.Generic;
using System.IO;
using System.Text;

using Smdn.IO.Streams.LineOriented;
using Smdn.Text;

namespace Smdn.Formats.Mime {
  public static class MimeUtils {
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream)
    {
      return ParseHeader(new LooseLineOrientedStream(stream), false);
    }

    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces)
    {
      return ParseHeader(new LooseLineOrientedStream(stream), keepWhitespaces);
    }

    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream)
    {
      return ParseHeader(stream, false);
    }

    private static readonly char[] lineDelmiters = new char[] {'\r', '\n'};

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

      private readonly ByteString rawData;
      private readonly int indexOfDelmiter;
    }

    public static IEnumerable<HeaderField> ParseHeaderRaw(LineOrientedStream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      ByteStringBuilder header = null;
      var indexOfDelimiter = -1;

      for (;;) {
        var line = stream.ReadLine(true);

        if (line == null)
          break; // unexpected end of stream

        if ((line.Length == 1 && (line[0] == Ascii.Octets.CR || line[0] == Ascii.Octets.LF)) ||
            (line.Length == 2 && (line[0] == Ascii.Octets.CR && line[1] == Ascii.Octets.LF)))
          break; // end of headers

        if (line[0] == Ascii.Octets.HT || line[0] == Ascii.Octets.SP) { // LWSP-char
          // folding
          if (header == null)
            // ignore incorrect formed header
            continue;

          header.Append(line);
        }
        else {
          if (0 < indexOfDelimiter)
            yield return new HeaderField(header.ToByteString(true), indexOfDelimiter);

          // field       =  field-name ":" [ field-body ] CRLF
          // field-name  =  1*<any CHAR, excluding CTLs, SPACE, and ":">
          const byte nameBodyDelimiter = (byte)':';

          indexOfDelimiter = -1;

          for (var index = 0; index < line.Length; index++) {
            if (line[index] == nameBodyDelimiter) {
              indexOfDelimiter = index;
              break;
            }
          }

          if (indexOfDelimiter == -1) {
            // ignore incorrect formed header
            header = null;
          }
          else {
            header = new ByteStringBuilder(line.Length);
            header.Append(line);
          }
        }
      }

      if (0 < indexOfDelimiter)
        yield return new HeaderField(header.ToByteString(true), indexOfDelimiter);
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