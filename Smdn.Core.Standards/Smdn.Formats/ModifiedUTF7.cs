// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2013 smdn
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
using System.Text;
using System.Security.Cryptography;

using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  /*
   * RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
   * 5.1.3. Mailbox International Naming Convention
   * http://tools.ietf.org/html/rfc3501#section-5.1.3
   */
  public static class ModifiedUTF7 {
    public static string Encode(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      using (var transform = new ToRFC3501ModifiedBase64Transform()) {
        var encoded = new StringBuilder();
        var index = -1;
        var shiftFrom = -1;

        foreach (var c in str) {
          index++;

          if (('\u0020' <= c && c <= '\u007e')) {
            if (0 <= shiftFrom) {
              // string -> modified UTF7
              encoded.Append('&');
              encoded.Append(transform.TransformStringTo(str.Substring(shiftFrom, index - shiftFrom), Encoding.BigEndianUnicode));
              encoded.Append('-');

              shiftFrom = -1;
            }

            // printable US-ASCII characters
            if (c == '\u0026')
              // except for "&"
              encoded.Append("&-");
            else
              encoded.Append(c);
          }
          else {
            if (shiftFrom == -1)
              shiftFrom = index;
          }
        }

        if (0 <= shiftFrom) {
          // string -> modified UTF7
          encoded.Append('&');
          encoded.Append(transform.TransformStringTo(str.Substring(shiftFrom), Encoding.BigEndianUnicode));
          encoded.Append('-');
        }

        return encoded.ToString();
      }
    }

    public static string Decode(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      using (var transform = new FromRFC3501ModifiedBase64Transform(FromBase64TransformMode.DoNotIgnoreWhiteSpaces)) {
        var chars = str.ToCharArray();
        var decoded = new StringBuilder();

        for (var index = 0; index < chars.Length; index++) {
          if (chars[index] < '\u0020' || '\u007e' < chars[index])
            throw new FormatException(string.Format("contains non-ascii or non-printable character: at index {0} of '{1}', \\u{2:x4}", index, str, (int)chars[index]));

          // In modified UTF-7, printable US-ASCII characters, except for "&",
          // represent themselves
          // "&" is used to shift to modified BASE64
          if (chars[index] != '&') {
            decoded.Append(chars[index]);
            continue;
          }

          if (chars.Length <= ++index)
            // incorrect form
            throw new FormatException("incorrect form");

          if (chars[index] == '-') {
            // The character "&" (0x26) is represented by the two-octet sequence "&-".
            decoded.Append('&');
            continue;
          }

          var nonprintable = new StringBuilder();

          for (; index < chars.Length; index++) {
            if (chars[index] == '-')
              // "-" is used to shift back to US-ASCII
              break;

            nonprintable.Append(chars[index]);
          }

          // modified UTF7 -> string
          decoded.Append(transform.TransformStringFrom(nonprintable.ToString(), Encoding.BigEndianUnicode));
        }

        return decoded.ToString();
      }
    }
  }
}
