// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2008-2012 smdn
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

using Smdn.Collections;
using Smdn.IO;

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

    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      string currentName = null;
      StringBuilder currentValue = null;

      for (;;) {
        var lineBytes = stream.ReadLine(keepWhitespaces);

        if (lineBytes == null)
          break; // unexpected end of stream

        var line = ByteString.CreateImmutable(lineBytes);

        if (keepWhitespaces) {
          if ((line.Length == 1 && (line[0] == Octets.CR || line[0] == Octets.LF)) ||
              (line.Length == 2 && (line[0] == Octets.CR && line[1] == Octets.LF)))
            break; // end of headers
        }
        else {
          if (line.IsEmpty)
            break; // end of headers
        }

        if (line[0] == Octets.HT || line[0] == Octets.SP) { // LWSP-char
          // folding
          if (currentName == null)
            // ignore incorrect formed header
            continue;

          if (!keepWhitespaces)
            line = line.Trim();

          currentValue.Append(line.ToString());
        }
        else {
          // field       =  field-name ":" [ field-body ] CRLF
          // field-name  =  1*<any CHAR, excluding CTLs, SPACE, and ":">
          const byte nameBodyDelimiter = (byte)':';
          var delim = line.IndexOf(nameBodyDelimiter);

          if (delim < 0) {
            // ignore incorrect formed header
            currentName = null;
            currentValue = null;
            continue;
          }
          else {
            if (currentName != null)
              yield return new KeyValuePair<string, string>(currentName, currentValue.ToString());

            currentName = line.Substring(0, delim).TrimEnd().ToString();

            if (currentName.Length <= 0) {
              // ignore incorrect formed header
              currentName = null;
              currentValue = null;
            }
            else {
              line = line.Substring(delim + 1);

              if (!keepWhitespaces)
                line = line.TrimStart();

              currentValue = new StringBuilder(line.ToString());
            }
          }
        }
      }

      if (currentName != null)
        yield return new KeyValuePair<string, string>(currentName, currentValue.ToString());
    }
  }
}