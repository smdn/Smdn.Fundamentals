// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010 smdn
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
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  public static class PercentEncoding {
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode)
    {
      return GetEncodedString(str, mode, Encoding.ASCII);
    }

    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
    {
      return ICryptoTransformExtensions.TransformStringTo(new ToPercentEncodedTransform(mode),
                                                          str,
                                                          encoding);
    }

    public static string GetEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode)
    {
      if (bytes == null)
        throw new ArgumentNullException();

      return GetEncodedString(bytes, 0, bytes.Length, mode);
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count, ToPercentEncodedTransformMode mode)
    {
      return Encoding.ASCII.GetString(ICryptoTransformExtensions.TransformBytes(new ToPercentEncodedTransform(mode),
                                                                                bytes,
                                                                                offset,
                                                                                count));
    }

    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode)
    {
      return Encode(str, mode, Encoding.ASCII);
    }

    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      var bytes = encoding.GetBytes(str);

      return ICryptoTransformExtensions.TransformBytes(new ToPercentEncodedTransform(mode),
                                                       bytes,
                                                       0,
                                                       bytes.Length);
    }

    public static string GetDecodedString(string str)
    {
      return GetDecodedString(str, Encoding.ASCII, false);
    }

    public static string GetDecodedString(string str, bool decodePlusToSpace)
    {
      return GetDecodedString(str, Encoding.ASCII, decodePlusToSpace);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      return GetDecodedString(str, encoding, false);
    }

    public static string GetDecodedString(string str, Encoding encoding, bool decodePlusToSpace)
    {
      return ICryptoTransformExtensions.TransformStringFrom(new FromPercentEncodedTransform(decodePlusToSpace),
                                                            str,
                                                            encoding);
    }

    public static byte[] Decode(string str)
    {
      return Decode(str, false);
    }

    public static byte[] Decode(string str, bool decodePlusToSpace)
    {
      var bytes = Encoding.ASCII.GetBytes(str);

      return ICryptoTransformExtensions.TransformBytes(new FromPercentEncodedTransform(decodePlusToSpace),
                                                       bytes,
                                                       0,
                                                       bytes.Length);
    }
  }
}
