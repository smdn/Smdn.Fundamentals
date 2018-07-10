// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2017 smdn
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
