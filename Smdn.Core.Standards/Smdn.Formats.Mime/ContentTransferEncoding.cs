// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2012 smdn
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
using System.Security.Cryptography;
using System.Text;

namespace Smdn.Formats.Mime {
  public static class ContentTransferEncoding {
    public const string HeaderName = "Content-Transfer-Encoding";

    private static Dictionary<string, ContentTransferEncodingMethod> contentTransferEncodingMethods =
      new Dictionary<string, ContentTransferEncodingMethod>(StringComparer.OrdinalIgnoreCase) {
        // standards
        {"7bit",              ContentTransferEncodingMethod.SevenBit},
        {"8bit",              ContentTransferEncodingMethod.EightBit},
        {"binary",            ContentTransferEncodingMethod.Binary},
        {"base64",            ContentTransferEncodingMethod.Base64},
        {"quoted-printable",  ContentTransferEncodingMethod.QuotedPrintable},

        // non-standards
        {"x-uuencode",    ContentTransferEncodingMethod.UUEncode},
        {"x-uuencoded",   ContentTransferEncodingMethod.UUEncode},
        {"x-uu",          ContentTransferEncodingMethod.UUEncode},
        {"x-uue",         ContentTransferEncodingMethod.UUEncode},
        {"uuencode",      ContentTransferEncodingMethod.UUEncode},
        {"x-gzip64",      ContentTransferEncodingMethod.GZip64},
        {"gzip64",        ContentTransferEncodingMethod.GZip64},
      };

    public static ContentTransferEncodingMethod GetEncodingMethod(string encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      ContentTransferEncodingMethod method;

      if (contentTransferEncodingMethods.TryGetValue(encoding, out method))
        return method;
      else
        return ContentTransferEncodingMethod.Unknown;
    }

    public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding)
    {
      var ret = GetEncodingMethod(encoding);

      if (ret == ContentTransferEncodingMethod.Unknown)
        throw new NotSupportedException(string.Format("unsupported content transfer encoding: '{0}'",
                                                      encoding));

      return ret;
    }

    public static string GetEncodingName(ContentTransferEncodingMethod method)
    {
      switch (method) {
        case ContentTransferEncodingMethod.SevenBit: return "7bit";
        case ContentTransferEncodingMethod.EightBit: return "8bit";
        case ContentTransferEncodingMethod.Binary: return "binary";
        case ContentTransferEncodingMethod.Base64: return "base64";
        case ContentTransferEncodingMethod.QuotedPrintable: return "quoted-printable";
        case ContentTransferEncodingMethod.UUEncode: return "x-uuencode";
        case ContentTransferEncodingMethod.GZip64: return "x-gzip64";
        default:
          throw ExceptionUtils.CreateNotSupportedEnumValue(method);
      }
    }

    public static Stream CreateDecodingStream(Stream stream, string encoding)
    {
      return CreateDecodingStream(stream, GetEncodingMethodThrowException(encoding));
    }

    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      switch (encoding) {
        case ContentTransferEncodingMethod.SevenBit:
        case ContentTransferEncodingMethod.EightBit:
        case ContentTransferEncodingMethod.Binary:
          return stream;
        case ContentTransferEncodingMethod.Base64:
          return new CryptoStream(stream, new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), CryptoStreamMode.Read);
        case ContentTransferEncodingMethod.QuotedPrintable:
          return new CryptoStream(stream, new FromQuotedPrintableTransform(), CryptoStreamMode.Read);
        case ContentTransferEncodingMethod.UUEncode:
        case ContentTransferEncodingMethod.GZip64:
        default:
          throw ExceptionUtils.CreateNotSupportedEnumValue(encoding);
      }
    }

    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset)
    {
      return CreateTextReader(stream,
                              GetEncodingMethodThrowException(encoding),
                              charset == null
                                ? Encoding.GetEncoding("ISO-8859-1")
                                : EncodingUtils.GetEncodingThrowException(charset));
    }

    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
    {
      if (encoding == ContentTransferEncodingMethod.Binary)
        throw new InvalidOperationException("can't create TextReader from message of binary transfer encoding");

      return new StreamReader(CreateDecodingStream(stream, encoding), charset);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, string encoding)
    {
      return CreateBinaryReader(stream,
                                GetEncodingMethodThrowException(encoding),
                                null);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding)
    {
      return CreateBinaryReader(stream,
                                encoding,
                                null);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
    {
      if (charset == null)
        return new BinaryReader(CreateDecodingStream(stream, encoding));
      else
        return new BinaryReader(CreateDecodingStream(stream, encoding), charset);
    }
  }
}
