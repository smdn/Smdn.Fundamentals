// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;

using Smdn.Formats.QuotedPrintableEncodings;
using Smdn.Formats.UUEncodings;
using Smdn.IO.Streams;
using Smdn.Text.Encodings;

namespace Smdn.Formats.Mime;

#pragma warning disable IDE0040
static partial class ContentTransferEncoding {
#pragma warning restore IDE0040
  public static Stream CreateDecodingStream(Stream stream, string encoding)
    => CreateDecodingStream(stream, GetEncodingMethodThrowException(encoding), false);

  public static Stream CreateDecodingStream(Stream stream, string encoding, bool leaveStreamOpen)
    => CreateDecodingStream(stream, GetEncodingMethodThrowException(encoding), leaveStreamOpen);

  public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding)
    => CreateDecodingStream(stream, encoding, false);

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
        ?
#if SYSTEM_TEXT_ENCODING_LATIN1
          Encoding.Latin1
#else
          Encoding.GetEncoding("ISO-8859-1")
#endif
        : EncodingUtils.GetEncodingThrowException(charset),
      leaveStreamOpen
    );

  public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset)
    => CreateTextReader(stream, encoding, charset, false);

  public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen)
  {
    if (encoding == ContentTransferEncodingMethod.Binary)
      throw new InvalidOperationException("can't create TextReader from message of binary transfer encoding");

    return new(
      stream: CreateDecodingStream(stream, encoding, false),
      encoding: charset,
#if !(NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER)
      detectEncodingFromByteOrderMarks: true,
      bufferSize: 1024,
#endif
      leaveOpen: leaveStreamOpen
    );
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

  public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding? charset)
    => CreateBinaryReader(stream, encoding, charset, false);

  public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding? charset, bool leaveStreamOpen)
    => new(
      CreateDecodingStream(stream, encoding, false),
      charset ?? Encoding.UTF8,
      leaveStreamOpen
    );
}
