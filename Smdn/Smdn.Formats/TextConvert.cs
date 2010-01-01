// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
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

#define TRANSFORMIMPL_FAST
#undef TRANSFORMIMPL_FAST

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Smdn.Formats {
  public static class TextConvert {
#region "text tranform"
    internal static string TransformTo(string str, ICryptoTransform transform, Encoding encoding)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return Encoding.ASCII.GetString(TransformBytes(encoding.GetBytes(str), transform));
    }

    internal static string TransformFrom(string str, ICryptoTransform transform, Encoding encoding)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return encoding.GetString(TransformBytes(Encoding.ASCII.GetBytes(str), transform));
    }

    internal static byte[] TransformBytes(byte[] inputBuffer, ICryptoTransform transform)
    {
      var outputBuffer = new byte[inputBuffer.Length * transform.OutputBlockSize];
      var outputOffset = 0;
      var inputOffset  = 0;

      if (transform.CanTransformMultipleBlocks) {
        var inputCount = (inputBuffer.Length / transform.InputBlockSize) * transform.InputBlockSize;

        outputOffset += transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        inputOffset  += inputCount;
      }

      var inputRemain = inputBuffer.Length - inputOffset;

      while (transform.InputBlockSize <= inputRemain) {
        outputOffset += transform.TransformBlock(inputBuffer, inputOffset, transform.InputBlockSize, outputBuffer, outputOffset);

        inputOffset += transform.InputBlockSize;
        inputRemain -= transform.InputBlockSize;
      }

      if (0 < inputRemain) {
        var finalBlock = transform.TransformFinalBlock(inputBuffer, inputOffset, inputBuffer.Length - inputOffset);

        if (outputBuffer.Length != outputOffset + finalBlock.Length)
          Array.Resize(ref outputBuffer, outputOffset + finalBlock.Length);

        Buffer.BlockCopy(finalBlock, 0, outputBuffer, outputOffset, finalBlock.Length);
      }
      else {
        if (outputBuffer.Length != outputOffset)
          Array.Resize(ref outputBuffer, outputOffset);
      }

      return outputBuffer;
    }

    internal static byte[] ToPrintableAsciiByteArray(string str, bool allowCrLf, bool allowTab)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      var chars = str.ToCharArray();
      var bytes = new byte[chars.Length];

      for (var i = 0; i < chars.Length; i++) {
        if (chars[i] < '\u0020') {
          var isAllowedChar =
            (allowCrLf && (chars[i] == Chars.CR || chars[i] == Chars.LF)) ||
            (allowTab && (chars[i] == Chars.HT));

          if (!isAllowedChar)
            throw new FormatException(string.Format("contains non-printable character: at index {0} of '{1}', \\u{2:x4}", i, str, (int)chars[i]));
        }
        else if ('\u0080' <= chars[i]) {
          if (chars[i] == '\u007f')
            throw new FormatException(string.Format("contains non-printable character: at index {0} of '{1}', \\u{2:x4}", i, str, (int)chars[i]));
          else
            throw new FormatException(string.Format("contains non-ascii character: at index {0} of '{1}', \\u{2:x4}", i, str, (int)chars[i]));
        }

        bytes[i] = (byte)chars[i];
      }

      return bytes;
    }
#endregion

#region "hexadecimal"
    public static string ToLowerCaseHexString(byte[] bytes)
    {
      return new string(ConvertByteArrayToHex(bytes, Chars.LowerCaseHexChars));
    }

    public static string ToUpperCaseHexString(byte[] bytes)
    {
      return new string(ConvertByteArrayToHex(bytes, Chars.UpperCaseHexChars));
    }

    public static byte[] ToLowerCaseHexByteArray(byte[] bytes)
    {
      return ConvertByteArrayToHex(bytes, Octets.LowerCaseHexOctets);
    }

    public static byte[] ToUpperCaseHexByteArray(byte[] bytes)
    {
      return ConvertByteArrayToHex(bytes, Octets.UpperCaseHexOctets);
    }

    private static T[] ConvertByteArrayToHex<T>(byte[] bytes, T[] table)
    {
      var hex = new T[bytes.Length * 2];

      for (int b = 0, c = 0; b < bytes.Length;) {
        hex[c++] = table[bytes[b] >> 4];
        hex[c++] = table[bytes[b] & 0xf];
        b++;
      }

      return hex;
    }

    public static byte[] FromHexString(string str)
    {
      return FromHexString(str, true, true);
    }

    public static byte[] FromLowerCaseHexString(string str)
    {
      return FromHexString(str, true, false);
    }

    public static byte[] FromUpperCaseHexString(string str)
    {
      return FromHexString(str, false, true);
    }

    private static byte[] FromHexString(string str, bool allowLowerCaseChar, bool allowUpperCaseChar)
    {
      if ((str.Length & 0x1) != 0)
        throw new FormatException("incorrect form");

      var chars = str.ToCharArray();
      var bytes = new byte[chars.Length / 2];
      var high = true;

      for (int c = 0, b = 0; c < chars.Length;) {
        int val;

        if ('0' <= chars[c] && chars[c] <= '9') {
          val = (int)(chars[c] - '0');
        }
        else if ('a' <= chars[c] && chars[c] <= 'f') {
          if (allowLowerCaseChar)
            val = 0xa + (int)(chars[c] - 'a');
          else
            throw new FormatException("incorrect form");
        }
        else if ('A' <= chars[c] && chars[c] <= 'F') {
          if (allowUpperCaseChar)
            val = 0xa + (int)(chars[c] - 'A');
          else
            throw new FormatException("incorrect form");
        }
        else {
          throw new FormatException("incorrect form");
        }

        if (high) {
          bytes[b] = (byte)(val << 4);
        }
        else {
          bytes[b] = (byte)(bytes[b] | val);
          b++;
        }

        c++;
        high = !high;
      }

      return bytes;
    }
#endregion

#region "Base64"
#if TRANSFORMIMPL_FAST
    public static string ToBase64String(string str)
    {
      return ToBase64String(str, Encoding.ASCII);
    }

    public static string ToBase64String(string str, Encoding encoding)
    {
      return ToBase64String(encoding.GetBytes(str));
    }

    public static string ToBase64String(byte[] bytes)
    {
      return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
    }

    public static string FromBase64String(string str)
    {
      return FromBase64String(str, Encoding.ASCII);
    }

    public static string FromBase64String(string str, Encoding encoding)
    {
      return encoding.GetString(FromBase64StringToByteArray(str));
    }

    public static byte[] FromBase64StringToByteArray(string str)
    {
      return System.Convert.FromBase64String(str);
    }
#else
    public static string ToBase64String(string str)
    {
      return TransformTo(str, new ToBase64Transform(), Encoding.ASCII);
    }

    public static string ToBase64String(string str, Encoding encoding)
    {
      return TransformTo(str, new ToBase64Transform(), encoding);
    }

    public static string ToBase64String(byte[] bytes)
    {
      return Encoding.ASCII.GetString(ToBase64ByteArray(bytes));
    }

    public static byte[] ToBase64ByteArray(byte[] bytes)
    {
      return TransformBytes(bytes, new ToBase64Transform());
    }

    public static string FromBase64String(string str)
    {
      return TransformFrom(str, new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), Encoding.ASCII);
    }

    public static string FromBase64String(string str, Encoding encoding)
    {
      return TransformFrom(str, new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), encoding);
    }

    public static byte[] FromBase64StringToByteArray(string str)
    {
      return FromBase64ByteArray(Encoding.ASCII.GetBytes(str));
    }

    public static byte[] FromBase64ByteArray(byte[] bytes)
    {
      return TransformBytes(bytes, new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces));
    }
#endif
#endregion

#region "RFC 2152 Modified Base64"
#if TRANSFORMIMPL_FAST
    public static string ToRFC2152ModifiedBase64String(string str)
    {
      return ToRFC2152ModifiedBase64String(Encoding.ASCII.GetBytes(str));
    }

    public static string ToRFC2152ModifiedBase64String(string str, Encoding encoding)
    {
      return ToRFC2152ModifiedBase64String(encoding.GetBytes(str));
    }

    public static string ToRFC2152ModifiedBase64String(byte[] bytes)
    {
      var base64 = System.Convert.ToBase64String(bytes);
      var padding = base64.IndexOf('=');

      if (0 <= padding)
        return base64.Substring(0, padding);
      else
        return base64;
    }

    public static string FromRFC2152ModifiedBase64String(string str)
    {
      return FromRFC2152ModifiedBase64String(str, Encoding.ASCII);
    }

    public static string FromRFC2152ModifiedBase64String(string str, Encoding encoding)
    {
      return encoding.GetString(FromRFC2152ModifiedBase64StringToByteArray(str));
    }

    public static byte[] FromRFC2152ModifiedBase64StringToByteArray(string str)
    {
      var padding = 4 - str.Length & 3;

      if (padding == 4)
        return System.Convert.FromBase64String(str);
      else if (padding == 3)
        throw new FormatException("incorrect form");
      else
        return System.Convert.FromBase64String(str + (new String('=', padding)));
    }
#else
    public static string ToRFC2152ModifiedBase64String(string str)
    {
      return TransformTo(str, new ToRFC2152ModifiedBase64Transform(), Encoding.ASCII);
    }

    public static string ToRFC2152ModifiedBase64String(string str, Encoding encoding)
    {
      return TransformTo(str, new ToRFC2152ModifiedBase64Transform(), encoding);
    }

    public static string ToRFC2152ModifiedBase64String(byte[] bytes)
    {
      return Encoding.ASCII.GetString(TransformBytes(bytes, new ToRFC2152ModifiedBase64Transform()));
    }

    public static string FromRFC2152ModifiedBase64String(string str)
    {
      return TransformFrom(str, new FromRFC2152ModifiedBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), Encoding.ASCII);
    }

    public static string FromRFC2152ModifiedBase64String(string str, Encoding encoding)
    {
      return TransformFrom(str, new FromRFC2152ModifiedBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), encoding);
    }

    public static byte[] FromRFC2152ModifiedBase64StringToByteArray(string str)
    {
      return TransformBytes(Encoding.ASCII.GetBytes(str), new FromRFC2152ModifiedBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces));
    }
#endif
#endregion

#region "RFC 3501 Modified Base64"
#if TRANSFORMIMPL_FAST
    public static string ToRFC3501ModifiedBase64String(string str)
    {
      return ToRFC3501ModifiedBase64String(Encoding.ASCII.GetBytes(str));
    }

    public static string ToRFC3501ModifiedBase64String(string str, Encoding encoding)
    {
      return ToRFC3501ModifiedBase64String(encoding.GetBytes(str));
    }

    public static string ToRFC3501ModifiedBase64String(byte[] bytes)
    {
      return ToRFC2152ModifiedBase64String(bytes).Replace('/', ',');
    }

    public static string FromRFC3501ModifiedBase64String(string str)
    {
      return FromRFC3501ModifiedBase64String(str, Encoding.ASCII);
    }

    public static string FromRFC3501ModifiedBase64String(string str, Encoding encoding)
    {
      return encoding.GetString(FromRFC3501ModifiedBase64StringToByteArray(str));
    }

    public static byte[] FromRFC3501ModifiedBase64StringToByteArray(string str)
    {
      return FromRFC2152ModifiedBase64StringToByteArray(str.Replace('/', ','));
    }
#else
    public static string ToRFC3501ModifiedBase64String(string str)
    {
      return TransformTo(str, new ToRFC3501ModifiedBase64Transform(), Encoding.ASCII);
    }

    public static string ToRFC3501ModifiedBase64String(string str, Encoding encoding)
    {
      return TransformTo(str, new ToRFC3501ModifiedBase64Transform(), encoding);
    }

    public static string ToRFC3501ModifiedBase64String(byte[] bytes)
    {
      return Encoding.ASCII.GetString(TransformBytes(bytes, new ToRFC3501ModifiedBase64Transform()));
    }

    public static string FromRFC3501ModifiedBase64String(string str)
    {
      return TransformFrom(str, new FromRFC3501ModifiedBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), Encoding.ASCII);
    }

    public static string FromRFC3501ModifiedBase64String(string str, Encoding encoding)
    {
      return TransformFrom(str, new FromRFC3501ModifiedBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), encoding);
    }

    public static byte[] FromRFC3501ModifiedBase64StringToByteArray(string str)
    {
      return TransformBytes(Encoding.ASCII.GetBytes(str), new FromRFC3501ModifiedBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces));
    }
#endif
#endregion

#region "quoted printable"
#if TRANSFORMIMPL_FAST
    public static string ToQuotedPrintableString(string str)
    {
      return ToQuotedPrintableString(str, Encoding.ASCII);
    }

    public static string ToQuotedPrintableString(string str, Encoding encoding)
    {
      return ToQuotedPrintableString(encoding.GetBytes(str));
    }

    public static string ToQuotedPrintableString(byte[] bytes)
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

    public static string FromQuotedPrintableString(string str)
    {
      return FromQuotedPrintableString(str, Encoding.ASCII);
    }

    public static string FromQuotedPrintableString(string str, Encoding encoding)
    {
      return encoding.GetString(FromQuotedPrintableStringToByteArray(str));
    }

    public static byte[] FromQuotedPrintableStringToByteArray(string str)
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
#else
    public static string ToQuotedPrintableString(string str)
    {
      return TransformTo(str, new ToQuotedPrintableTransform(), Encoding.ASCII);
    }
    
    public static string ToQuotedPrintableString(string str, Encoding encoding)
    {
      return TransformTo(str, new ToQuotedPrintableTransform(), encoding);
    }

    public static string ToQuotedPrintableString(byte[] bytes)
    {
      return Encoding.ASCII.GetString(TransformBytes(bytes, new ToQuotedPrintableTransform()));
    }

    public static string FromQuotedPrintableString(string str)
    {
      return TransformFrom(str, new FromQuotedPrintableTransform(), Encoding.ASCII);
    }

    public static string FromQuotedPrintableString(string str, Encoding encoding)
    {
      return TransformFrom(str, new FromQuotedPrintableTransform(), encoding);
    }

    public static byte[] FromQuotedPrintableStringToByteArray(string str)
    {
      return TransformBytes(Encoding.ASCII.GetBytes(str), new FromQuotedPrintableTransform());
    }
#endif
#endregion

#region "Modified UTF-7"
    // RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
    // 5.1.3. Mailbox International Naming Convention
    // http://tools.ietf.org/html/rfc3501#section-5.1.3
    public static string ToModifiedUTF7String(string str)
    {
      var encoded = new StringBuilder();
      var chars = str.ToCharArray();
      var shiftFrom = -1;

      for (var index = 0; index < chars.Length; index++) {
        var c = chars[index];

        if (('\u0020' <= c && c <= '\u007e')) {
          if (0 <= shiftFrom) {
            // string -> modified UTF7
            encoded.Append('&');
            encoded.Append(ToRFC3501ModifiedBase64String(str.Substring(shiftFrom, index - shiftFrom), Encoding.BigEndianUnicode));
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
        encoded.Append(ToRFC3501ModifiedBase64String(str.Substring(shiftFrom), Encoding.BigEndianUnicode));
        encoded.Append('-');
      }

      return encoded.ToString();
    }

    public static string FromModifiedUTF7String(string str)
    {
      var bytes = ToPrintableAsciiByteArray(str, false, false);
      var decoded = new StringBuilder();

      for (var index = 0; index < bytes.Length; index++) {
        // In modified UTF-7, printable US-ASCII characters, except for "&",
        // represent themselves
        // "&" is used to shift to modified BASE64
        if (bytes[index] != 0x26) { // '&'
          decoded.Append((char)bytes[index]);
          continue;
        }

        if (bytes.Length <= ++index)
          // incorrect form
          throw new FormatException("incorrect form");

        if (bytes[index] == 0x2d) { // '-'
          // The character "&" (0x26) is represented by the two-octet sequence "&-".
          decoded.Append('&');
          continue;
        }

        var nonprintable = new StringBuilder();

        for (; index < bytes.Length; index++) {
          if (bytes[index] == 0x2d) // '-'
            // "-" is used to shift back to US-ASCII
            break;

          nonprintable.Append((char)bytes[index]);
        }

        // modified UTF7 -> string
        decoded.Append(FromRFC3501ModifiedBase64String(nonprintable.ToString(), Encoding.BigEndianUnicode));
      }

      return decoded.ToString();
    }
#endregion

#region "PercentEncoding"
    public static string ToPercentEncodedString(string str)
    {
      return TransformTo(str, new ToPercentEncodedTransform(), Encoding.ASCII);
    }

    public static string ToPercentEncodedString(string str, Encoding encoding)
    {
      return TransformTo(str, new ToPercentEncodedTransform(), encoding);
    }

    public static string ToPercentEncodedString(byte[] bytes)
    {
      return Encoding.ASCII.GetString(TransformBytes(bytes, new ToPercentEncodedTransform()));
    }

    public static string FromPercentEncodedString(string str)
    {
      return TransformFrom(str, new FromPercentEncodedTransform(), Encoding.ASCII);
    }

    public static string FromPercentEncodedString(string str, bool decodePlusToSpace)
    {
      return TransformFrom(str, new FromPercentEncodedTransform(decodePlusToSpace), Encoding.ASCII);
    }

    public static string FromPercentEncodedString(string str, Encoding encoding)
    {
      return TransformFrom(str, new FromPercentEncodedTransform(), encoding);
    }

    public static string FromPercentEncodedString(string str, Encoding encoding, bool decodePlusToSpace)
    {
      return TransformFrom(str, new FromPercentEncodedTransform(decodePlusToSpace), encoding);
    }

    public static byte[] FromPercentEncodedStringToByteArray(string str)
    {
      return TransformBytes(Encoding.ASCII.GetBytes(str), new FromPercentEncodedTransform());
    }

    public static byte[] FromPercentEncodedStringToByteArray(string str, bool decodePlusToSpace)
    {
      return TransformBytes(Encoding.ASCII.GetBytes(str), new FromPercentEncodedTransform(decodePlusToSpace));
    }
#endregion

#region "MIME encoding"
    // http://tools.ietf.org/html/rfc2047
    // RFC 2047 - MIME (Multipurpose Internet Mail Extensions) Part Three: Message Header Extensions for Non-ASCII Text
    // 2. Syntax of encoded-words
    // 3. Character sets
    // 4. Encodings

    // encoded-word = "=?" charset "?" encoding "?" encoded-text "?="
    // charset = token    ; see section 3
    // encoding = token   ; see section 4
    // token = 1*<Any CHAR except SPACE, CTLs, and especials>
    // especials = "(" / ")" / "<" / ">" / "@" / "," / ";" / ":" / "
    //             <"> / "/" / "[" / "]" / "?" / "." / "="
    // encoded-text = 1*<Any printable ASCII character other than "?"
    //                   or SPACE>
    //               ; (but see "Use of encoded-words in message
    //               ; headers", section 5)
    public static string ToMimeEncodedString(string str, MimeEncoding encoding)
    {
      return ToMimeEncodedString(str, encoding, Encoding.ASCII, false, 0, 0, null);
    }

    public static string ToMimeEncodedString(string str, MimeEncoding encoding, Encoding charset)
    {
      return ToMimeEncodedString(str, encoding, charset, false, 0, 0, null);
    }

    public static string ToMimeEncodedString(string str, MimeEncoding encoding, int foldingLimit, int foldingOffset)
    {
      return ToMimeEncodedString(str, encoding, Encoding.ASCII, true, foldingLimit, foldingOffset, mimeEncodingFoldingString);
    }

    public static string ToMimeEncodedString(string str, MimeEncoding encoding, int foldingLimit, int foldingOffset, string foldingString)
    {
      return ToMimeEncodedString(str, encoding, Encoding.ASCII, true, foldingLimit, foldingOffset, foldingString);
    }

    public static string ToMimeEncodedString(string str, MimeEncoding encoding, Encoding charset, int foldingLimit, int foldingOffset)
    {
      return ToMimeEncodedString(str, encoding, charset, true, foldingLimit, foldingOffset, mimeEncodingFoldingString);
    }

    public static string ToMimeEncodedString(string str, MimeEncoding encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString)
    {
      return ToMimeEncodedString(str, encoding, charset, true, foldingLimit, foldingOffset, foldingString);
    }

    private static readonly string mimeEncodingFoldingString = Chars.CRLF + Chars.HT;
    private static readonly byte[] mimeEncodingPostamble = new byte[] {0x3f, 0x3d}; // "?="

    private static string ToMimeEncodedString(string str, MimeEncoding encoding, Encoding charset, bool doFold, int foldingLimit, int foldingOffset, string foldingString)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (charset == null)
        throw new ArgumentNullException("charset");
      if (doFold) {
        if (foldingLimit < 1)
          throw new ArgumentOutOfRangeException("foldingLimit");
        if (foldingOffset < 0)
          throw new ArgumentOutOfRangeException("foldingOffset");
        if (foldingLimit <= foldingOffset)
          throw new ArgumentOutOfRangeException("foldingOffset");
        if (foldingString == null)
          throw new ArgumentNullException("foldingString");
      }

      ICryptoTransform transform;
      char encodingChar;

      switch (encoding) {
        case MimeEncoding.Base64:
          transform = new ToBase64Transform();
          encodingChar = 'b';
          break;
        case MimeEncoding.QuotedPrintable:
          transform = new ToQuotedPrintableTransform();
          encodingChar = 'q';
          break;
        default:
          throw new System.ComponentModel.InvalidEnumArgumentException("encoding", (int)encoding, typeof(MimeEncoding));
      }

      var preambleText = string.Format("=?{0}?{1}?", charset.BodyName, encodingChar);

      if (!doFold) {
        lock (transform) {
          return preambleText + TransformTo(str, transform, charset) + "?=";
        }
      }

      // folding
      var ret = new StringBuilder();
      var preamble = Encoding.ASCII.GetBytes(preambleText);
      var firstLine = true;
      var inputCharBuffer = str.ToCharArray();
      var inputCharOffset = 0;
      var outputBuffer = new byte[foldingLimit];
      var ambleLength = preamble.Length + mimeEncodingPostamble.Length;
      var outputLimit = foldingLimit - (foldingOffset + ambleLength);

      if (outputLimit <= 0)
        throw new ArgumentOutOfRangeException("foldingLimit", "too short");

      // copy preamble to buffer
      Buffer.BlockCopy(preamble, 0, outputBuffer, 0, preamble.Length);

      for (;;) {
        var inputBlockSizeLimit = (outputLimit * transform.InputBlockSize) / transform.OutputBlockSize - 1;
        var transformCharCount = 0;
        var outputCount = preamble.Length;

        // decide char count to transform
        for (transformCharCount = inputBlockSizeLimit / charset.GetMaxByteCount(1);; transformCharCount++) {
          if (inputCharBuffer.Length <= inputCharOffset + transformCharCount) {
            transformCharCount = inputCharBuffer.Length - inputCharOffset;
            break;
          }

          if (inputBlockSizeLimit <= charset.GetByteCount(inputCharBuffer, inputCharOffset, transformCharCount + 1))
            break;
        }

        // transform chars
        byte[] transformed = null;

        for (;;) {
          var t = TransformBytes(charset.GetBytes(inputCharBuffer, inputCharOffset, transformCharCount), transform);

          if (transformed == null || t.Length <= outputLimit) {
            transformed = t;

            if (inputCharBuffer.Length <= inputCharOffset + transformCharCount + 1)
              break;

            transformCharCount++;
            continue;
          }
          else {
            transformCharCount--;
            break;
          }
        }

        if (outputBuffer.Length < ambleLength + transformed.Length)
          throw new ArgumentOutOfRangeException("foldingLimit", string.Format("too short, at least {0} is required", ambleLength + transformed.Length));

        // copy transformed chars to buffer
        Buffer.BlockCopy(transformed, 0, outputBuffer, outputCount, transformed.Length);

        outputCount += transformed.Length;

        // copy postanble to buffer
        Buffer.BlockCopy(mimeEncodingPostamble, 0, outputBuffer, outputCount, mimeEncodingPostamble.Length);

        outputCount += mimeEncodingPostamble.Length;

        ret.Append(Encoding.ASCII.GetString(outputBuffer, 0, outputCount));

        inputCharOffset += transformCharCount;

        if (inputCharOffset < inputCharBuffer.Length) {
          ret.Append(foldingString);

          if (firstLine) {
            outputLimit = foldingLimit - ambleLength;
            firstLine = false;
          }
        }
        else {
          break;
        }
      }

      return ret.ToString();
    }

    public static string FromMimeEncodedString(string str)
    {
      MimeEncoding discard1;
      Encoding discard2;

      return FromMimeEncodedString(str, out discard1, out discard2);
    }

    public static string FromMimeEncodedString(string str, out MimeEncoding encoding, out Encoding charset)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      charset = null;
      encoding = MimeEncoding.None;

      Encoding lastCharset = null;
      var lastEncoding = MimeEncoding.None;

      var ret = mimeEncodedWordRegex.Replace(str, delegate(Match m) {
        // charset
        try {
          lastCharset = Encoding.GetEncoding(m.Groups[1].Value);
        }
        catch {
          throw new FormatException(string.Format("{0} is an unsupported or invalid charset", m.Groups[1].Value));
        }

        // encoding
        switch (m.Groups[2].Value.ToLowerInvariant()) {
          case "b":
            lastEncoding = MimeEncoding.Base64;
            return FromBase64String(m.Groups[3].Value, lastCharset);
          case "q":
            lastEncoding = MimeEncoding.QuotedPrintable;
            return FromQuotedPrintableString(m.Groups[3].Value, lastCharset);
        }

        throw new FormatException(string.Format("{0} is an invalid encoding", m.Groups[2].Value));
      });

      charset = lastCharset;
      encoding = lastEncoding;

      return ret;
    }

    private static readonly Regex mimeEncodedWordRegex = new Regex(@"\s*\=\?([^?]+)\?([^?]+)\?([^\?\s]+)\?\=\s*", RegexOptions.Singleline);
#endregion

#region "XHTML and HTML style escape"
    public static string ToHtmlEscapedString(string str)
    {
      return ToXhtmlEscapedString(str, false);
    }

    public static string ToXhtmlEscapedString(string str)
    {
      return ToXhtmlEscapedString(str, true);
    }

    private static string ToXhtmlEscapedString(string str, bool xhtml)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      var sb = new StringBuilder(str.Length);
      var len = str.Length;

      for (var i = 0; i < len; i++) {
        var ch = str[i];

        switch (ch) {
          case Chars.Ampersand:   sb.Append("&amp;"); break;
          case Chars.LessThan:    sb.Append("&lt;"); break;
          case Chars.GreaterThan: sb.Append("&gt;"); break;
          case Chars.DQuote:      sb.Append("&quot;"); break;
          case Chars.Quote:
            if (xhtml) sb.Append("&apos;");
            else sb.Append(Chars.Quote);
            break;
          default: sb.Append(ch); break;
        }
      }

      return sb.ToString();
    }

    public static string FromHtmlEscapedString(string str)
    {
      return FromXhtmlEscapedString(str, false);
    }

    public static string FromXhtmlEscapedString(string str)
    {
      return FromXhtmlEscapedString(str, true);
    }

    private static string FromXhtmlEscapedString(string str, bool xhtml)
    {
      throw new NotImplementedException();
    }
#endregion
  }
}
