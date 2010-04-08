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

using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  public static class TextConvert {
#region "text tranform"
    internal static string TransformTo(string str, ICryptoTransform transform, Encoding encoding)
    {
      return transform.TransformStringTo(str, encoding);
    }

    internal static string TransformFrom(string str, ICryptoTransform transform, Encoding encoding)
    {
      return transform.TransformStringFrom(str, encoding);
    }

    internal static byte[] TransformBytes(byte[] inputBuffer, ICryptoTransform transform)
    {
      return ICryptoTransformExtensions.TransformBytes(transform, inputBuffer);
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

#region "Base64"
    [Obsolete("use Base64.GetEncodedString()")]
    public static string ToBase64String(string str)
    {
      return Base64.GetEncodedString(str);
    }

    [Obsolete("use Base64.GetEncodedString()")]
    public static string ToBase64String(string str, Encoding encoding)
    {
      return Base64.GetEncodedString(str, encoding);
    }

    [Obsolete("use Base64.GetEncodedString()")]
    public static string ToBase64String(byte[] bytes)
    {
      return Base64.GetEncodedString(bytes);
    }

    [Obsolete("use Base64.Encode()")]
    public static byte[] ToBase64ByteArray(byte[] bytes)
    {
      return Base64.Encode(bytes);
    }

    [Obsolete("use Base64.GetDecodedString()")]
    public static string FromBase64String(string str)
    {
      return Base64.GetDecodedString(str);
    }

    [Obsolete("use Base64.GetDecodedString()")]
    public static string FromBase64String(string str, Encoding encoding)
    {
      return Base64.GetDecodedString(str, encoding);
    }

    [Obsolete("use Base64.Decode()")]
    public static byte[] FromBase64StringToByteArray(string str)
    {
      return Base64.Decode(str);
    }

    [Obsolete("use Base64.Decode()")]
    public static byte[] FromBase64ByteArray(byte[] bytes)
    {
      return Base64.Decode(bytes);
    }
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
    [Obsolete("use QuotedPrintableEncoding.GetEncodedString()")]
    public static string ToQuotedPrintableString(string str)
    {
      return QuotedPrintableEncoding.GetEncodedString(str);
    }
    
    [Obsolete("use QuotedPrintableEncoding.GetEncodedString()")]
    public static string ToQuotedPrintableString(string str, Encoding encoding)
    {
      return QuotedPrintableEncoding.GetEncodedString(str, encoding);
    }

    [Obsolete("use QuotedPrintableEncoding.GetEncodedString()")]
    public static string ToQuotedPrintableString(byte[] bytes)
    {
      return QuotedPrintableEncoding.GetEncodedString(bytes);
    }

    [Obsolete("use QuotedPrintableEncoding.GetDecodedString()")]
    public static string FromQuotedPrintableString(string str)
    {
      return QuotedPrintableEncoding.GetDecodedString(str);
    }

    [Obsolete("use QuotedPrintableEncoding.GetDecodedString()")]
    public static string FromQuotedPrintableString(string str, Encoding encoding)
    {
      return QuotedPrintableEncoding.GetDecodedString(str, encoding);
    }

    [Obsolete("use QuotedPrintableEncoding.Decode()")]
    public static byte[] FromQuotedPrintableStringToByteArray(string str)
    {
      return QuotedPrintableEncoding.Decode(str);
    }
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
    [Obsolete("use PercentEncoding.GetEncodedString()")]
    public static string ToPercentEncodedString(string str, ToPercentEncodedTransformMode mode)
    {
      return PercentEncoding.GetEncodedString(str, mode);
    }

    [Obsolete("use PercentEncoding.GetEncodedString()")]
    public static string ToPercentEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
    {
      return PercentEncoding.GetEncodedString(str, mode, encoding);
    }

    [Obsolete("use PercentEncoding.GetEncodedString()")]
    public static string ToPercentEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode)
    {
      return PercentEncoding.GetEncodedString(bytes, mode);
    }

    [Obsolete("use PercentEncoding.GetDecodedString()")]
    public static string FromPercentEncodedString(string str)
    {
      return PercentEncoding.GetDecodedString(str);
    }

    [Obsolete("use PercentEncoding.GetDecodedString()")]
    public static string FromPercentEncodedString(string str, bool decodePlusToSpace)
    {
      return PercentEncoding.GetDecodedString(str, decodePlusToSpace);
    }

    [Obsolete("use PercentEncoding.GetDecodedString()")]
    public static string FromPercentEncodedString(string str, Encoding encoding)
    {
      return PercentEncoding.GetDecodedString(str, encoding);
    }

    [Obsolete("use PercentEncoding.GetDecodedString()")]
    public static string FromPercentEncodedString(string str, Encoding encoding, bool decodePlusToSpace)
    {
      return PercentEncoding.GetDecodedString(str, encoding, decodePlusToSpace);
    }

    [Obsolete("use PercentEncoding.Decode()")]
    public static byte[] FromPercentEncodedStringToByteArray(string str)
    {
      return PercentEncoding.Decode(str);
    }

    [Obsolete("use PercentEncoding.Decode()")]
    public static byte[] FromPercentEncodedStringToByteArray(string str, bool decodePlusToSpace)
    {
      return PercentEncoding.Decode(str, decodePlusToSpace);
    }
#endregion

#region "MIME encoding"
    [Obsolete("use MimeEncoding.Encode()")]
    public static string ToMimeEncodedString(string str, MimeEncodingMethod encoding)
    {
      return MimeEncoding.Encode(str, encoding);
    }

    [Obsolete("use MimeEncoding.Encode()")]
    public static string ToMimeEncodedString(string str, MimeEncodingMethod encoding, Encoding charset)
    {
      return MimeEncoding.Encode(str, encoding, charset);
    }

    [Obsolete("use MimeEncoding.Encode()")]
    public static string ToMimeEncodedString(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset)
    {
      return MimeEncoding.Encode(str, encoding, foldingLimit, foldingOffset);
    }

    [Obsolete("use MimeEncoding.Encode()")]
    public static string ToMimeEncodedString(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset, string foldingString)
    {
      return MimeEncoding.Encode(str, encoding, foldingLimit, foldingOffset, foldingString);
    }

    [Obsolete("use MimeEncoding.Encode()")]
    public static string ToMimeEncodedString(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset)
    {
      return MimeEncoding.Encode(str, encoding, charset, foldingLimit, foldingOffset);
    }

    [Obsolete("use MimeEncoding.Encode()")]
    public static string ToMimeEncodedString(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString)
    {
      return MimeEncoding.Encode(str, encoding, charset, foldingLimit, foldingOffset, foldingString);
    }

    [Obsolete("use MimeEncoding.DecodeNullable()")]
    public static string FromMimeEncodedStringNullable(string str)
    {
      return MimeEncoding.DecodeNullable(str);
    }

    [Obsolete("use MimeEncoding.Decode()")]
    public static string FromMimeEncodedString(string str)
    {
      return MimeEncoding.Decode(str);
    }

    [Obsolete("use MimeEncoding.DecodeNullable()")]
    public static string FromMimeEncodedStringNullable(string str, out MimeEncodingMethod encoding, out Encoding charset)
    {
      return MimeEncoding.DecodeNullable(str, out encoding, out charset);
    }

    [Obsolete("use MimeEncoding.Decode()")]
    public static string FromMimeEncodedString(string str, out MimeEncodingMethod encoding, out Encoding charset)
    {
      return MimeEncoding.Decode(str, out encoding, out charset);
    }
#endregion

#region "XHTML and HTML style escape"
    public static string ToHtmlEscapedString(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      return ToXhtmlEscapedString(str, false);
    }

    public static string ToXhtmlEscapedString(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      return ToXhtmlEscapedString(str, true);
    }

    public static string ToHtmlEscapedStringNullable(string str)
    {
      if (str == null)
        return null;
      else
        return ToXhtmlEscapedString(str, false);
    }

    public static string ToXhtmlEscapedStringNullable(string str)
    {
      if (str == null)
        return null;
      else
        return ToXhtmlEscapedString(str, true);
    }

    private static string ToXhtmlEscapedString(string str, bool xhtml)
    {
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
