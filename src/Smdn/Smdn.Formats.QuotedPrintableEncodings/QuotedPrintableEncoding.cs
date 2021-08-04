// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Smdn.IO.Streams;
using Smdn.Security.Cryptography;

namespace Smdn.Formats.QuotedPrintableEncodings {
  public static class QuotedPrintableEncoding {
    public static string GetEncodedString(string str)
    {
      return GetEncodedString(str, Encoding.ASCII);
    }

    public static string GetEncodedString(string str, Encoding encoding)
    {
      return ICryptoTransformExtensions.TransformStringTo(new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.ContentTransferEncoding),
                                                          str,
                                                          encoding);
    }

    public static string GetEncodedString(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return GetEncodedString(bytes, 0, bytes.Length);
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count)
    {
      return Encoding.ASCII.GetString(ICryptoTransformExtensions.TransformBytes(new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.ContentTransferEncoding),
                                                                                bytes,
                                                                                offset,
                                                                                count));
    }

    public static byte[] Encode(string str)
    {
      return Encode(str, Encoding.ASCII);
    }

    public static byte[] Encode(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));

      var bytes = encoding.GetBytes(str);

      return ICryptoTransformExtensions.TransformBytes(new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.ContentTransferEncoding),
                                                       bytes,
                                                       0,
                                                       bytes.Length);
    }

    public static string GetDecodedString(string str)
    {
      return GetDecodedString(str, Encoding.ASCII);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      return ICryptoTransformExtensions.TransformStringFrom(new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding),
                                                            str,
                                                            encoding);
    }

    public static byte[] Decode(string str)
    {
      var bytes = Encoding.ASCII.GetBytes(str);

      return ICryptoTransformExtensions.TransformBytes(new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding),
                                                       bytes,
                                                       0,
                                                       bytes.Length);
    }

    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false)
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

#if NET472
      return new CryptoStream(stream, new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding), CryptoStreamMode.Read, leaveStreamOpen);
#else
      var s = new CryptoStream(stream, new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.ContentTransferEncoding), CryptoStreamMode.Read);

      if (leaveStreamOpen)
        return new NonClosingStream(s);
      else
        return s;
#endif
    }
  }
}
