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

using Smdn.IO.Streams;
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
        throw new ArgumentNullException(nameof(encoding));

      var bytes = encoding.GetBytes(str);

      return GetEncodedString(bytes, 0, bytes.Length);
    }

    public static string GetEncodedString(byte[] bytes)
    {
#if NET || NETSTANDARD2_0
      return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
#else
      return System.Convert.ToBase64String(bytes);
#endif
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count)
    {
#if NET || NETSTANDARD2_0
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
      using (var transform = CreateToBase64Transform()) {
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
      using (var transform = CreateFromBase64Transform(ignoreWhiteSpaces: true)) {
        return ICryptoTransformExtensions.TransformBytes(transform, bytes, offset, count);
      }
    }

    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

#if NET472
      return new CryptoStream(stream, CreateToBase64Transform(), CryptoStreamMode.Write, leaveStreamOpen);
#else
      var s = new CryptoStream(stream, CreateToBase64Transform(), CryptoStreamMode.Write);

      if (leaveStreamOpen)
        return new NonClosingStream(s);
      else
        return s;
#endif
    }

    public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

#if NET472
      return new CryptoStream(stream, CreateFromBase64Transform(ignoreWhiteSpaces: true), CryptoStreamMode.Read, leaveStreamOpen);
#else
      var s = new CryptoStream(stream, CreateFromBase64Transform(ignoreWhiteSpaces: true), CryptoStreamMode.Read);

      if (leaveStreamOpen)
        return new NonClosingStream(s);
      else
        return s;
#endif
    }

    public static ICryptoTransform CreateFromBase64Transform(bool ignoreWhiteSpaces = true)
    {
#if NET
      var mode = ignoreWhiteSpaces ? System.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : System.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

      return new System.Security.Cryptography.FromBase64Transform(mode);
#elif NETSTANDARD2_0
      if (Runtime.IsRunningOnNetCore/* && Runtime.Version < new Version(2, 2)*/) {
        var mode = ignoreWhiteSpaces ? Smdn.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : Smdn.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

        return new Smdn.Security.Cryptography.FromBase64Transform(mode);
      }
      else {
        var mode = ignoreWhiteSpaces ? System.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : System.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

        return new System.Security.Cryptography.FromBase64Transform(mode);
      }
#else
      var mode = ignoreWhiteSpaces ? Smdn.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : Smdn.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

      return new Smdn.Security.Cryptography.FromBase64Transform(mode);
#endif
    }

    public static ICryptoTransform CreateToBase64Transform()
    {
#if NET || NETSTANDARD2_0
      return new System.Security.Cryptography.ToBase64Transform();
#else
      return new Smdn.Security.Cryptography.ToBase64Transform();
#endif
    }
  }
}
