// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Smdn.Formats;
using Smdn.Formats.QuotedPrintableEncodings;
using Smdn.Formats.UUEncodings;
using Smdn.IO.Streams;
using Smdn.Text.Encodings;

namespace Smdn.Formats.Mime {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ContentTransferEncoding {
    public const string HeaderName = "Content-Transfer-Encoding";

    private static readonly Dictionary<string, ContentTransferEncodingMethod> contentTransferEncodingMethods =
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
        throw new ArgumentNullException(nameof(encoding));

      if (contentTransferEncodingMethods.TryGetValue(encoding, out var method))
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
      return CreateDecodingStream(stream, GetEncodingMethodThrowException(encoding), false);
    }

    public static Stream CreateDecodingStream(Stream stream, string encoding, bool leaveStreamOpen)
    {
      return CreateDecodingStream(stream, GetEncodingMethodThrowException(encoding), leaveStreamOpen);
    }

    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding)
    {
      return CreateDecodingStream(stream, encoding, false);
    }

    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      Stream decodingStream = null;

      switch (encoding) {
        case ContentTransferEncodingMethod.SevenBit:
        case ContentTransferEncodingMethod.EightBit:
        case ContentTransferEncodingMethod.Binary:
          if (leaveStreamOpen)
            decodingStream = new NonClosingStream(stream);
          else
            decodingStream = stream;
          break;
        case ContentTransferEncodingMethod.Base64:
          decodingStream = Base64.CreateDecodingStream(stream, leaveStreamOpen);
          break;
        case ContentTransferEncodingMethod.QuotedPrintable:
          decodingStream = QuotedPrintableEncoding.CreateDecodingStream(stream, leaveStreamOpen);
          break;
        case ContentTransferEncodingMethod.UUEncode:
          decodingStream = new UUDecodingStream(stream, leaveStreamOpen);
          break;
        case ContentTransferEncodingMethod.GZip64:
        default:
          throw ExceptionUtils.CreateNotSupportedEnumValue(encoding);
      }

      return decodingStream;
    }

    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset)
    {
      return CreateTextReader(stream, encoding, charset, false);
    }

    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset, bool leaveStreamOpen)
    {
      return CreateTextReader(stream,
                              GetEncodingMethodThrowException(encoding),
                              charset == null
                                ? Encoding.GetEncoding("ISO-8859-1")
                                : EncodingUtils.GetEncodingThrowException(charset),
                              leaveStreamOpen);
    }

    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
    {
      return CreateTextReader(stream, encoding, charset, false);
    }

    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen)
    {
      if (encoding == ContentTransferEncodingMethod.Binary)
        throw new InvalidOperationException("can't create TextReader from message of binary transfer encoding");

      stream = CreateDecodingStream(stream, encoding, false);

      return new StreamReader(stream, charset, true, 1024, leaveStreamOpen);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, string encoding)
    {
      return CreateBinaryReader(stream, encoding, false);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, string encoding, bool leaveStreamOpen)
    {
      return CreateBinaryReader(stream,
                                GetEncodingMethodThrowException(encoding),
                                null,
                                leaveStreamOpen);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding)
    {
      return CreateBinaryReader(stream, encoding, false);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen)
    {
      return CreateBinaryReader(stream,
                                encoding,
                                null,
                                leaveStreamOpen);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
    {
      return CreateBinaryReader(stream, encoding, charset, false);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen)
    {
      stream = CreateDecodingStream(stream, encoding, false);

      return new BinaryReader(stream, charset ?? Encoding.UTF8, leaveStreamOpen);
    }
  }
}
