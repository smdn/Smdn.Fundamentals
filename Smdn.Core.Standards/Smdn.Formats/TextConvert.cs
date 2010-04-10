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

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Smdn.Formats.Mime;
using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  public static class TextConvert {
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
    [Obsolete("use ModifiedUTF7.Encode()")]
    public static string ToModifiedUTF7String(string str)
    {
      return ModifiedUTF7.Encode(str);
    }

    [Obsolete("use ModifiedUTF7.Decode()")]
    public static string FromModifiedUTF7String(string str)
    {
      return ModifiedUTF7.Decode(str);
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
