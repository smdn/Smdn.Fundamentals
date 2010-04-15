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
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  public static class Base64 {
    public static string GetEncodedString(string str)
    {
      return GetEncodedString(str, Encoding.ASCII);
    }

    public static string GetEncodedString(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return GetEncodedString(encoding.GetBytes(str));
    }

    public static string GetEncodedString(byte[] bytes)
    {
      return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
    }

    public static byte[] Encode(string str)
    {
      return Encode(str, Encoding.ASCII);
    }

    public static byte[] Encode(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return Encode(encoding.GetBytes(str));
    }

    public static byte[] Encode(byte[] bytes)
    {
      return ICryptoTransformExtensions.TransformBytes(new ToBase64Transform(), bytes);
    }

    public static string GetDecodedString(string str)
    {
      return GetDecodedString(str, Encoding.ASCII);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return encoding.GetString(Decode(str));
    }

    public static string GetDecodedString(byte[] bytes)
    {
      return Encoding.ASCII.GetString(Decode(bytes));
    }

    public static byte[] Decode(string str)
    {
      return System.Convert.FromBase64String(str);
    }

    public static byte[] Decode(byte[] bytes)
    {
      return ICryptoTransformExtensions.TransformBytes(new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), bytes);
    }

    public static Stream CreateEncodingStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      return new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Write);
    }

    public static Stream CreateDecodingStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      return new CryptoStream(stream, new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), CryptoStreamMode.Read);
    }
  }
}
