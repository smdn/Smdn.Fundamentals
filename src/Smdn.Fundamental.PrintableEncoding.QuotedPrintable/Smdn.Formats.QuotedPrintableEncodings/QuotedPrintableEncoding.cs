// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#if !SYSTEM_SECURITY_CRYPTOGRAPHY_CRYPTOSTREAM_CTOR_LEAVEOPEN
using Smdn.IO.Streams; // NonClosingStream
#endif
using Smdn.Security.Cryptography;

namespace Smdn.Formats.QuotedPrintableEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class QuotedPrintableEncoding {
#if !SYSTEM_ARRAY_EMPTY
  internal static readonly byte[] EmptyByteArray = new byte[0];
#endif

  public static string GetEncodedString(string str)
    => GetEncodedString(str, Encoding.ASCII);

  public static string GetEncodedString(string str, Encoding encoding)
  {
    using var transform = new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.ContentTransferEncoding);

    return ICryptoTransformExtensions.TransformStringTo(
      transform,
      str,
      encoding
    );
  }

  public static string GetEncodedString(byte[] bytes)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof(bytes));

    return GetEncodedString(bytes, 0, bytes.Length);
  }

  public static string GetEncodedString(byte[] bytes, int offset, int count)
  {
    using var transform = new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.ContentTransferEncoding);

    return Encoding.ASCII.GetString(
      ICryptoTransformExtensions.TransformBytes(
        transform,
        bytes,
        offset,
        count
      )
    );
  }

  public static byte[] Encode(string str)
    => Encode(str, Encoding.ASCII);

  public static byte[] Encode(string str, Encoding encoding)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    using var transform = new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.ContentTransferEncoding);
    var bytes = encoding.GetBytes(str);

    return ICryptoTransformExtensions.TransformBytes(
      transform,
      bytes,
      0,
      bytes.Length
    );
  }

  public static string GetDecodedString(string str)
    => GetDecodedString(str, Encoding.ASCII);

  public static string GetDecodedString(string str, Encoding encoding)
  {
    using var transform = new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding);

    return ICryptoTransformExtensions.TransformStringFrom(
      transform,
      str,
      encoding
    );
  }

  public static byte[] Decode(string str)
  {
    using var transform = new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding);
    var bytes = Encoding.ASCII.GetBytes(str);

    return ICryptoTransformExtensions.TransformBytes(
      transform,
      bytes,
      0,
      bytes.Length
    );
  }

  public static Stream CreateEncodingStream(
    Stream stream,
#pragma warning disable IDE0060
    bool leaveStreamOpen = false
#pragma warning restore IDE0060
  )
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));

    // TODO: impl
    throw new NotImplementedException();
  }

  public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));

#pragma warning disable CA2000
    var transform = new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding);
#pragma warning restore CA2000

#if SYSTEM_SECURITY_CRYPTOGRAPHY_CRYPTOSTREAM_CTOR_LEAVEOPEN
    return new CryptoStream(stream, transform, CryptoStreamMode.Read, leaveStreamOpen);
#else
    var s = new CryptoStream(stream, transform, CryptoStreamMode.Read);

    if (leaveStreamOpen)
      return new NonClosingStream(s);
    else
      return s;
#endif
  }
}
