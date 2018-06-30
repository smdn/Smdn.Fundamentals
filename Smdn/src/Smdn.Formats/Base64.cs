// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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

#if NET || NETSTANDARD20
using FromBase64Transform = System.Security.Cryptography.FromBase64Transform;
using FromBase64TransformMode = System.Security.Cryptography.FromBase64TransformMode;
using ToBase64Transform = System.Security.Cryptography.ToBase64Transform;
#else
using FromBase64Transform = Smdn.Security.Cryptography.FromBase64Transform;
using FromBase64TransformMode = Smdn.Security.Cryptography.FromBase64TransformMode;
using ToBase64Transform = Smdn.Security.Cryptography.ToBase64Transform;
#endif

namespace Smdn.Formats {
  public static class Base64 {
    public static string GetEncodedString(string str)
    {
      return GetEncodedString(str, Encoding.ASCII);
    }

    public static string GetEncodedString(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));

      var bytes = encoding.GetBytes(str);

      return GetEncodedString(bytes, 0, bytes.Length);
    }

    public static string GetEncodedString(byte[] bytes)
    {
#if NET46 || NETSTANDARD20
      return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
#else
      return System.Convert.ToBase64String(bytes);
#endif
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count)
    {
#if NET46 || NETSTANDARD20
      return System.Convert.ToBase64String(bytes, offset, count, Base64FormattingOptions.None);
#else
      return System.Convert.ToBase64String(bytes, offset, count);
#endif
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

      return Encode(bytes, 0, bytes.Length);
    }

    public static byte[] Encode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return Encode(bytes, 0, bytes.Length);
    }

    public static byte[] Encode(byte[] bytes, int offset, int count)
    {
      using (var transform = new ToBase64Transform()) {
        return ICryptoTransformExtensions.TransformBytes(transform, bytes, offset, count);
      }
    }

    public static string GetDecodedString(string str)
    {
      return GetDecodedString(str, Encoding.ASCII);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));

      return encoding.GetString(Decode(str));
    }

    public static string GetDecodedString(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return GetDecodedString(bytes, 0, bytes.Length);
    }

    public static string GetDecodedString(byte[] bytes, int offset, int count)
    {
      return Encoding.ASCII.GetString(Decode(bytes, offset, count));
    }

    public static byte[] Decode(string str)
    {
      return System.Convert.FromBase64String(str);
    }

    public static byte[] Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return Decode(bytes, 0, bytes.Length);
    }

    public static byte[] Decode(byte[] bytes, int offset, int count)
    {
      using (var transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces)) {
        return ICryptoTransformExtensions.TransformBytes(transform, bytes, offset, count);
      }
    }

    public static Stream CreateEncodingStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      return new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Write);
    }

    public static Stream CreateDecodingStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      return new CryptoStream(stream, new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces), CryptoStreamMode.Read);
    }
  }
}
