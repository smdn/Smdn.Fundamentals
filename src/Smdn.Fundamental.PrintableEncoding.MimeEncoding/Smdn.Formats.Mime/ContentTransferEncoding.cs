// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Smdn.Formats.QuotedPrintableEncodings;
using Smdn.Formats.UUEncodings;
using Smdn.IO.Streams;
using Smdn.Text.Encodings;

namespace Smdn.Formats.Mime {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ContentTransferEncoding {
    public const string HeaderName = "Content-Transfer-Encoding";

    private static readonly Dictionary<string, ContentTransferEncodingMethod> contentTransferEncodingMethods =
      new(StringComparer.OrdinalIgnoreCase) {
        // standards
        { "7bit",              ContentTransferEncodingMethod.SevenBit },
        { "8bit",              ContentTransferEncodingMethod.EightBit },
        { "binary",            ContentTransferEncodingMethod.Binary },
        { "base64",            ContentTransferEncodingMethod.Base64 },
        { "quoted-printable",  ContentTransferEncodingMethod.QuotedPrintable },

        // non-standards
        { "x-uuencode",    ContentTransferEncodingMethod.UUEncode },
        { "x-uuencoded",   ContentTransferEncodingMethod.UUEncode },
        { "x-uu",          ContentTransferEncodingMethod.UUEncode },
        { "x-uue",         ContentTransferEncodingMethod.UUEncode },
        { "uuencode",      ContentTransferEncodingMethod.UUEncode },
        { "x-gzip64",      ContentTransferEncodingMethod.GZip64 },
        { "gzip64",        ContentTransferEncodingMethod.GZip64 },
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
        throw new NotSupportedException($"unsupported content transfer encoding: '{encoding}'");

      return ret;
    }

    public static string GetEncodingName(ContentTransferEncodingMethod method)
      => method switch {
        ContentTransferEncodingMethod.SevenBit => "7bit",
        ContentTransferEncodingMethod.EightBit => "8bit",
        ContentTransferEncodingMethod.Binary => "binary",
        ContentTransferEncodingMethod.Base64 => "base64",
        ContentTransferEncodingMethod.QuotedPrintable => "quoted-printable",
        ContentTransferEncodingMethod.UUEncode => "x-uuencode",
        ContentTransferEncodingMethod.GZip64 => "x-gzip64",
        _ => throw ExceptionUtils.CreateNotSupportedEnumValue(method),
      };

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

      return encoding switch {
        ContentTransferEncodingMethod.SevenBit or
        ContentTransferEncodingMethod.EightBit or
        ContentTransferEncodingMethod.Binary => leaveStreamOpen ? new NonClosingStream(stream) : stream,
        ContentTransferEncodingMethod.Base64 => Base64.CreateDecodingStream(stream, leaveStreamOpen),
        ContentTransferEncodingMethod.QuotedPrintable => QuotedPrintableEncoding.CreateDecodingStream(stream, leaveStreamOpen),
        ContentTransferEncodingMethod.UUEncode => new UUDecodingStream(stream, leaveStreamOpen),

        // ContentTransferEncodingMethod.GZip64
        _ => throw ExceptionUtils.CreateNotSupportedEnumValue(encoding),
      };
    }

    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset)
      => CreateTextReader(stream, encoding, charset, false);

    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset, bool leaveStreamOpen)
      => CreateTextReader(
        stream,
        GetEncodingMethodThrowException(encoding),
        charset is null
          ? Encoding.GetEncoding("ISO-8859-1")
          : EncodingUtils.GetEncodingThrowException(charset),
        leaveStreamOpen
      );

    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
      => CreateTextReader(stream, encoding, charset, false);

    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen)
    {
      if (encoding == ContentTransferEncodingMethod.Binary)
        throw new InvalidOperationException("can't create TextReader from message of binary transfer encoding");

      stream = CreateDecodingStream(stream, encoding, false);

      return new StreamReader(stream, charset, true, 1024, leaveStreamOpen);
    }

    public static BinaryReader CreateBinaryReader(Stream stream, string encoding)
      => CreateBinaryReader(stream, encoding, false);

    public static BinaryReader CreateBinaryReader(Stream stream, string encoding, bool leaveStreamOpen)
      => CreateBinaryReader(
        stream,
        GetEncodingMethodThrowException(encoding),
        null,
        leaveStreamOpen
      );

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding)
      => CreateBinaryReader(stream, encoding, false);

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen)
      => CreateBinaryReader(
        stream,
        encoding,
        null,
        leaveStreamOpen
      );

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
      => CreateBinaryReader(stream, encoding, charset, false);

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen)
      => new(
        CreateDecodingStream(stream, encoding, false),
        charset ?? Encoding.UTF8,
        leaveStreamOpen
      );
  }
}
