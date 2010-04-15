// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2010 smdn
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
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.Mime {
  public static class QuotedPrintableEncoding {
    public static string GetEncodedString(string str)
    {
      return GetEncodedString(str, Encoding.ASCII);
    }
    
    public static string GetEncodedString(string str, Encoding encoding)
    {
      return ICryptoTransformExtensions.TransformStringTo(new ToQuotedPrintableTransform(),
                                                          str,
                                                          encoding);
    }

    public static string GetEncodedString(byte[] bytes)
    {
      return Encoding.ASCII.GetString(ICryptoTransformExtensions.TransformBytes(new ToQuotedPrintableTransform(),
                                                                                bytes));
    }

    public static byte[] Encode(string str)
    {
      return Encode(str, Encoding.ASCII);
    }

    public static byte[] Encode(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return ICryptoTransformExtensions.TransformBytes(new ToQuotedPrintableTransform(),
                                                       encoding.GetBytes(str));
    }

    public static string GetDecodedString(string str)
    {
      return GetDecodedString(str, Encoding.ASCII);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      return ICryptoTransformExtensions.TransformStringFrom(new FromQuotedPrintableTransform(),
                                                            str,
                                                            encoding);
    }

    public static byte[] Decode(string str)
    {
      return ICryptoTransformExtensions.TransformBytes(new FromQuotedPrintableTransform(),
                                                       Encoding.ASCII.GetBytes(str));
    }

    public static Stream CreateEncodingStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      // TODO: impl
      throw new NotImplementedException();
    }

    public static Stream CreateDecodingStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      return new CryptoStream(stream, new FromQuotedPrintableTransform(), CryptoStreamMode.Read);
    }

#if false
    public static string GetEncodedString(byte[] bytes)
    {
      var quoted = new StringBuilder(bytes.Length * 4);
      var charcount = 0;

      for (var index = 0; index < bytes.Length; index++) {
        var octet = bytes[index];

        if (73 < charcount) {
          // 次のエスケープで76文字を越える可能性がある場合
          var escaped = false;

          quoted.Append('\u003d'); // '=' 0x3d

#warning "TODO: remove folding"
          if (octet == Octets.HT || octet == Octets.SP) {
            // '\t' 0x09 or ' ' 0x20
            quoted.Append(Chars.UpperCaseHexChars[octet >> 4]);
            quoted.Append(Chars.UpperCaseHexChars[octet & 0xf]);

            escaped = true;
          }

          quoted.Append(Chars.CRLF); // '\r' 0x0d, '\n' 0x0a

          charcount = 0;

          if (escaped)
            continue;
        }

        if ((0x21 <= octet && octet <= 0x3c) ||
            (0x3e <= octet && octet <= 0x7e) ||
            octet == Octets.HT ||
            octet == Octets.SP) {
          // printable char (except '=' 0x3d)
          quoted.Append((char)octet);

          charcount++;
        }
        else {
          // '=' 0x3d or control char
          quoted.Append('\u003d'); // '=' 0x3d
          quoted.Append(Chars.UpperCaseHexChars[octet >> 4]);
          quoted.Append(Chars.UpperCaseHexChars[octet & 0xf]);

          charcount += 3;
        }
      }

      return quoted.ToString();
    }

    public static byte[] Decode(string str)
    {
      var quoted = ToPrintableAsciiByteArray(str, true, true);
      var decoded = new MemoryStream(str.Length);
      var prevQuoted = false;

      for (var index = 0; index < quoted.Length;) {
        if (quoted[index] == 0x3d) { // '=' 0x3d
          index++;

          if (quoted[index] == Octets.CR && quoted[index + 1] == Octets.LF) {
            // '\r' 0x0d, '\n' 0x0a
            index += 2;

            prevQuoted = false;
          }
          else {
            byte d = 0x00;

            for (var i = 0; i < 2; i++) {
              d <<= 4;

              if (0x30 <= quoted[index] && quoted[index] <= 0x39)
                // '0' 0x30 to '9' 0x39
                d |= (byte)(quoted[index++] - 0x30);
              else if (0x41 <= quoted[index] && quoted[index] <= 0x46)
                // 'A' 0x41 to 'F' 0x46
                d |= (byte)(quoted[index++] - 0x37);
              else if (0x61 <= quoted[index] && quoted[index] <= 0x66)
                // 'a' 0x61 to 'f' 0x66
                d |= (byte)(quoted[index++] - 0x57);
              else
                throw new FormatException("incorrect form");
            }

            decoded.WriteByte(d);

            prevQuoted = true;
          }
        }
        else if (quoted[index] == Octets.CR || quoted[index] == Octets.LF) {
          // '\r' 0x0d, '\n' 0x0a
          if (!prevQuoted)
            decoded.WriteByte(quoted[index]);

          index++;
        }
        else {
          decoded.WriteByte(quoted[index++]);

          prevQuoted = false;
        }
      }

      return decoded.ToArray();
    }
#endif
  }
}
